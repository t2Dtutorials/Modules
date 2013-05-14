//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function initializeToolbox()
{   
    // Populate the stock colors.
    %colorCount = getStockColorCount();
    for ( %i = 0; %i < %colorCount; %i++ )
    {
        // Fetch stock color name.
        %colorName = getStockColorName(%i);
        
        // Add to the list.        
        BackgroundColorSelectList.add( getStockColorName(%i), %i );
        
        // Select the color if it's the default one.
        if ( %colorName $= $pref::ParticleViewer::defaultBackgroundColor )
            BackgroundColorSelectList.setSelected( %i );
    }
    BackgroundColorSelectList.sort();

    AssetListArray.initialize();

    // Is this on the desktop?
    if ( $platform $= "windows" || $platform $= "macos" )
    {
        // Yes, so make the controls screen controls visible.
        ResolutionSelectLabel.Visible = true;
        ResolutionSelectList.Visible = true;
        FullscreenOptionLabel.Visible = true;
        FullscreenOptionButton.Visible = true;
        
        // Fetch the active resolution.
        %activeResolution = getRes();
        %activeWidth = getWord(%activeResolution, 0);
        %activeHeight = getWord(%activeResolution, 1);
        %activeBpp = getWord(%activeResolution, 2);
        
        // Fetch the resolutions.
        %resolutionList = getResolutionList( $pref::Video::displayDevice );
        %resolutionCount = getWordCount( %resolutionList ) / 3;
        %inputIndex = 0;
        %outputIndex = 0;
        for( %i = 0; %i < %resolutionCount; %i++ )
        {
            // Fetch the resolution entry.
            %width = getWord( %resolutionList, %inputIndex );
            %height = getWord( %resolutionList, %inputIndex+1 );
            %bpp = getWord( %resolutionList, %inputIndex+2 );
            %inputIndex += 3;
            
            // Skip the 16-bit ones.
            if ( %bpp == 16 )
                continue;
                
            // Store the resolution.
            $ParticleViewerResolutions[%outputIndex] = %width SPC %height SPC %bpp;
            
            // Add to the list.
            ResolutionSelectList.add( %width @ "x" @ %height SPC "(" @ %bpp @ ")", %outputIndex );
            
            // Select the resolution if it's the default one.
            if ( %width == %activeWidth && %height == %activeHeight && %bpp == %activeBpp )
                ResolutionSelectList.setSelected( %outputIndex );
                
            %outputIndex++;
        }
    }
    
    // Configure the main overlay.
    ParticleViewerWindow.add(MainOverlay);    
    %horizPosition = getWord(ParticleViewerWindow.Extent, 0) - getWord(MainOverlay.Extent, 0);
    %verticalPosition = getWord(ParticleViewerWindow.Extent, 1) - getWord(MainOverlay.Extent, 1);    
    MainOverlay.position = %horizPosition SPC %verticalPosition;    
}

//-----------------------------------------------------------------------------

function toggleToolbox(%make)
{
    // Finish if being released.
    if ( !%make )
        return;
        
    // Finish if the console is awake.
    if ( ConsoleDialog.isAwake() )
        return;       
        
    // Is the toolbox awake?
    if ( ToolboxDialog.isAwake() )
    {
        // Yes, so deactivate it.
        if ( $enableDirectInput )
            activateKeyboard();
        Canvas.popDialog(ToolboxDialog);
        MainOverlay.setVisible(1);
        return;
    }
    
    // Activate it.
    if ( $enableDirectInput )
        deactivateKeyboard();

    MainOverlay.setVisible(0);
    Canvas.pushDialog(ToolboxDialog);
}

//-----------------------------------------------------------------------------

function toggleAssetList(%make)
{
    // Finish if being released.
    if ( !%make )
        return;
        
    // Finish if the console is awake.
    if ( ConsoleDialog.isAwake() )
        return;       
        
    // Is the asset list awake?
    if ( LoadAssetDialog.isAwake() )
    {
        // Yes, so deactivate it.
        if ( $enableDirectInput )
            activateKeyboard();
        Canvas.popDialog(LoadAssetDialog);
        MainOverlay.setVisible(1);
        return;
    }
    
    // Activate it.
    if ( $enableDirectInput )
        deactivateKeyboard();

    MainOverlay.setVisible(0);
    Canvas.pushDialog(LoadAssetDialog);
}

//-----------------------------------------------------------------------------

function BackgroundColorSelectList::onSelect(%this)
{           
    // Fetch the index.
    $activeBackgroundColor = %this.getSelected();
 
    // Finish if the ParticleViewer scene is not available.
    if ( !isObject(ParticleViewerScene) )
        return;
            
    // Set the scene color.
    Canvas.BackgroundColor = getStockColorName($activeBackgroundColor);
    Canvas.UseBackgroundColor = true;
}

//-----------------------------------------------------------------------------

function ResolutionSelectList::onSelect(%this)
{
    // Finish if the ParticleViewer scene is not available.
    if ( !isObject(ParticleViewerScene) )
        return;
            
    // Fetch the index.
    %index = %this.getSelected();

    // Fetch resolution.
    %resolution = $ParticleViewerResolutions[%index];
    
    // Set the screen mode.    
    setScreenMode( GetWord( %resolution , 0 ), GetWord( %resolution, 1 ), GetWord( %resolution, 2 ), $pref::Video::fullScreen );
}

//-----------------------------------------------------------------------------

function PlayStopModeButton::onClick(%this)
{
    // Sanity!
    if (!isObject(ParticleViewer.ActivePlayer))
    {
        error( "Cannot play/stop the ParticleViewer player as it does not exist." );
        return;
    }
    
    // Toggle the player play/stop state.
    echo(ParticleViewer.ActivePlayer.getIsPlaying());
    if (ParticleViewer.ActivePlayer.getIsPlaying())
    {
        ParticleViewer.ActivePlayer.stop(false, false);
        %this.Text = "Play";
    }else
    {
        ParticleViewer.ActivePlayer.play();
        %this.Text = "Stop";
    }
}

//-----------------------------------------------------------------------------

function PauseModeButton::onClick(%this)
{
    // Sanity!
    if (!isObject(ParticleViewer.ActivePlayer))
    {
        error( "Cannot pause/unpause the ParticleViewer player as it does not exist." );
        return;
    }
    
    // Toggle the player pause state.
    if (ParticleViewer.ActivePlayer.getPaused())
    {
        ParticleViewer.ActivePlayer.setPaused(false);
        %this.Text = "Pause";
    }else
    {
        ParticleViewer.ActivePlayer.setPaused(true);
        %this.Text = "Unpause";
    }
}

//-----------------------------------------------------------------------------

function ReloadAssetButton::onClick(%this)
{
    // Sanity!
    if (!isObject(ParticleViewer.ActivePlayer))
    {
        error("Cannot reload an asset that has not been first loaded");
        return;
    }
    
    // Get the current asset ID.
    %assetId = ParticleViewer.ActivePlayer.getParticleAsset();

    // Get the file path.
    %filePath = AssetDatabase.getAssetFilePath(%assetId);

    // Get the current, loaded module.
    %moduleDef = ModuleDatabase.findModules(true);

    // Unload the existing effect.
    unloadParticleEffect();

    // Remove then reload the specified asset from disk.
    AssetDatabase.removeDeclaredAsset(%assetId);
    AssetDatabase.addDeclaredAsset(%moduleDef, %filePath);

    // Reload the particle effect.
    loadParticleEffect(%assetId);
}

//-----------------------------------------------------------------------------

function updateToolboxOptions()
{
    // Finish if the ParticleViewer scene is not available.
    if ( !isObject(ParticleViewerScene) )
        return;
        
    // Set the scene color.
    Canvas.BackgroundColor = getStockColorName($activeBackgroundColor);
    Canvas.UseBackgroundColor = true;        
       
    // Set option.
    if ( $pref::ParticleViewer::metricsOption )
        ParticleViewerScene.setDebugOn( "metrics" );
    else
        ParticleViewerScene.setDebugOff( "metrics" );

    // Set option.
    if ( $pref::ParticleViewer::fpsmetricsOption )
        ParticleViewerScene.setDebugOn( "fps" );
    else
        ParticleViewerScene.setDebugOff( "fps" );

    // Set option.
    if ( $pref::ParticleViewer::controllersOption )
        ParticleViewerScene.setDebugOn( "controllers" );
    else
        ParticleViewerScene.setDebugOff( "controllers" );
                    
    // Set option.
    if ( $pref::ParticleViewer::jointsOption )
        ParticleViewerScene.setDebugOn( "joints" );
    else
        ParticleViewerScene.setDebugOff( "joints" );

    // Set option.
    if ( $pref::ParticleViewer::wireframeOption )
        ParticleViewerScene.setDebugOn( "wireframe" );
    else
        ParticleViewerScene.setDebugOff( "wireframe" );
        
    // Set option.
    if ( $pref::ParticleViewer::aabbOption )
        ParticleViewerScene.setDebugOn( "aabb" );
    else
        ParticleViewerScene.setDebugOff( "aabb" );

    // Set option.
    if ( $pref::ParticleViewer::oobbOption )
        ParticleViewerScene.setDebugOn( "oobb" );
    else
        ParticleViewerScene.setDebugOff( "oobb" );
        
    // Set option.
    if ( $pref::ParticleViewer::sleepOption )
        ParticleViewerScene.setDebugOn( "sleep" );
    else
        ParticleViewerScene.setDebugOff( "sleep" );                

    // Set option.
    if ( $pref::ParticleViewer::collisionOption )
        ParticleViewerScene.setDebugOn( "collision" );
    else
        ParticleViewerScene.setDebugOff( "collision" );
        
    // Set option.
    if ( $pref::ParticleViewer::positionOption )
        ParticleViewerScene.setDebugOn( "position" );
    else
        ParticleViewerScene.setDebugOff( "position" );
        
    // Set option.
    if ( $pref::ParticleViewer::sortOption )
        ParticleViewerScene.setDebugOn( "sort" );
    else
        ParticleViewerScene.setDebugOff( "sort" );        
                          
    // Set the options check-boxe.
    MetricsOptionCheckBox.setStateOn( $pref::ParticleViewer::metricsOption );
    FpsMetricsOptionCheckBox.setStateOn( $pref::ParticleViewer::fpsmetricsOption );
    ControllersOptionCheckBox.setStateOn( $pref::ParticleViewer::controllersOption );
    JointsOptionCheckBox.setStateOn( $pref::ParticleViewer::jointsOption );
    WireframeOptionCheckBox.setStateOn( $pref::ParticleViewer::wireframeOption );
    AABBOptionCheckBox.setStateOn( $pref::ParticleViewer::aabbOption );
    OOBBOptionCheckBox.setStateOn( $pref::ParticleViewer::oobbOption );
    SleepOptionCheckBox.setStateOn( $pref::ParticleViewer::sleepOption );
    CollisionOptionCheckBox.setStateOn( $pref::ParticleViewer::collisionOption );
    PositionOptionCheckBox.setStateOn( $pref::ParticleViewer::positionOption );
    SortOptionCheckBox.setStateOn( $pref::ParticleViewer::sortOption );
    BatchOptionCheckBox.setStateOn( ParticleViewerScene.getBatchingEnabled() );
    
    // Is this on the desktop?
    //if ( $platform $= "windows" || $platform $= "macos" )
    if ( $platform !$= "iOS" )
    {
        // Set the fullscreen check-box.
        FullscreenOptionButton.setStateOn( $pref::Video::fullScreen );
        
        // Set the full-screen mode appropriately.
        if ( isFullScreen() != $pref::Video::fullScreen )
            toggleFullScreen();            
    }  
}

//-----------------------------------------------------------------------------

function setFullscreenOption( %flag )
{
    $pref::Video::fullScreen = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setMetricsOption( %flag )
{
    $pref::ParticleViewer::metricsOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setFPSMetricsOption( %flag )
{
    $pref::ParticleViewer::fpsmetricsOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setMetricsOption( %flag )
{
    $pref::ParticleViewer::metricsOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setControllersOption( %flag )
{
    $pref::ParticleViewer::controllersOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setJointsOption( %flag )
{
    $pref::ParticleViewer::jointsOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setWireframeOption( %flag )
{
    $pref::ParticleViewer::wireframeOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setAABBOption( %flag )
{
    $pref::ParticleViewer::aabbOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setOOBBOption( %flag )
{
    $pref::ParticleViewer::oobbOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setSleepOption( %flag )
{
    $pref::ParticleViewer::sleepOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setCollisionOption( %flag )
{
    $pref::ParticleViewer::collisionOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setPositionOption( %flag )
{
    $pref::ParticleViewer::positionOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function setSortOption( %flag )
{
    $pref::ParticleViewer::sortOption = %flag;
    updateToolboxOptions();
}

//-----------------------------------------------------------------------------

function ToyListScroller::scrollToNext(%this)
{
    %currentScroll = %this.getScrollPositionY();
    %currentScroll += 85;
    %this.setScrollPosition(0, %currentScroll);
}

//-----------------------------------------------------------------------------

function ToyListScroller::scrollToPrevious(%this)
{
    %currentScroll = %this.getScrollPositionY();
    %currentScroll -= 85;
    %this.setScrollPosition(0, %currentScroll);
}

//-----------------------------------------------------------------------------

function AssetListArray::initialize(%this)
{
    //%this.clear();
    //%currentExtent = %this.extent;
    //%newExtent = getWord(%currentExtent, 0) SPC "20";
    //%this.Extent = %newExtent;
    
    // Fetch the ParticleAsset count.
    %assetCount = ParticleViewer.AssetList.getCount();

    // Populate the particle asset list.
    for (%assetIndex = 0; %assetIndex < %assetCount; %assetIndex++)
    {
        // Fetch the asset.
        %assetId = ParticleViewer.AssetList.getAsset(%assetIndex);

        // Fetch the asset name.
        %assetName = AssetDatabase.getAssetName(%assetId);

        // Add to the GUI list.
        %this.addAssetButton(%assetName, %assetId);
    }
}

//-----------------------------------------------------------------------------

function AssetListArray::addAssetButton(%this, %assetName, %assetId)
{
    %button = new GuiButtonCtrl()
    {
        canSaveDynamicFields = "0";
        HorizSizing = "relative";
        class = "AssetSelectButton";
        VertSizing = "relative";
        isContainer = "0";
        Profile = "BlueButtonProfile";
        toolTipProfile = "GuiToolTipProfile";
        toolTip = "Load this particle effect";
        Position = "0 0";
        Extent = "160 80";
        Visible = "1";
        particle = %assetId;
        isContainer = "0";
        Active = "1";
        text = %assetName;
        groupNum = "-1";
        buttonType = "PushButton";
        useMouseEvents = "0";
    };
     
    %button.command = %button @ ".performSelect();";
    
    %this.add(%button);
}

//-----------------------------------------------------------------------------

function AssetSelectButton::performSelect(%this)
{
    // Finish if already selected.
    //if (%this.particle == ParticleViewer.ActiveAsset)
        //return;
    
    // Load the selected particle effect.
    loadParticleEffect(%this.particle);
}
