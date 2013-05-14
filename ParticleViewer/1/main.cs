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

function ParticleViewer::create( %this )
{    
    // Load the preferences.
    %this.loadPreferences();
    
    // Load ParticleViewer scripts.
    exec("./scripts/console.cs");
    exec("./scripts/options.cs");
    exec("./scripts/scene.cs");
    exec("./scripts/particles.cs");
        
    // Load GUI profiles.
    exec("./gui/guiProfiles.cs");

    // Create the ParticleViewer window.
    CreateParticleViewerWindow();
    
    // Load and configure the console.
    ParticleViewer.add(TamlRead("./gui/ConsoleDialog.gui.taml"));
    GlobalActionMap.bind(keyboard, "ctrl 1", toggleConsole);
    
    // Load and configure the toolbox.
    ParticleViewer.add(TamlRead("./gui/ToolboxDialog.gui.taml"));

    // Load and configure the asset list.
    ParticleViewer.add(TamlRead("./gui/LoadAssetDialog.gui.taml"));

    // Load and configure the main overlay.
    ParticleViewer.add(TamlRead("./gui/MainOverlay.gui.taml"));
    
    // Scan for ParticleAssets.
    scanForAssets();

    // Initialize the toolbox.    
    initializeToolbox();
    
    // Initialize the "cannot render" proxy.
    new RenderProxy(CannotRenderProxy)
    {
        Image = "ParticleViewer:CannotRender";
    };
    ParticleViewer.add(CannotRenderProxy);
}

//-----------------------------------------------------------------------------

function ParticleViewer::destroy( %this )
{
    // Save ParticleViewer preferences.
    %this.savePreferences();    
    
    // Unload the active toy.
    unloadParticleEffect();
    
    // Destroy the ParticleViewer window.
    destroyParticleViewerWindow();
    
    // Destroy the ParticleViewer scene.
    destroyParticleViewerScene();
}

//-----------------------------------------------------------------------------

function ParticleViewer::loadPreferences( %this )
{
    // Load the default preferences.
    exec( "./scripts/pvPreferences.cs" );
    
    // Load the last session preferences if available.
    if ( isFile("preferences.cs") )
        exec( "preferences.cs" );   
}

//-----------------------------------------------------------------------------

function ParticleViewer::savePreferences( %this )
{
    // Export only the ParticleViewer preferences.
    export("$pref::ParticleViewer::*", "preferences.cs", false );        
    export("$pref::Video::*", "preferences.cs", true );
}
