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

function createParticleViewerWindow()
{
    // Sanity!
    if ( !isObject(ParticleViewerWindow) )
    {
        // Create the scene window.
        new SceneWindow(ParticleViewerWindow);

        // Set profile.        
        ParticleViewerWindow.Profile = ParticleViewerWindowProfile;
        
        // Push the window.
        Canvas.setContent( ParticleViewerWindow );                     
    }

    // Set camera to a canonical state.
    %allBits = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31";
    ParticleViewerWindow.stopCameraMove();
    ParticleViewerWindow.dismount();
    ParticleViewerWindow.setViewLimitOff();
    ParticleViewerWindow.setRenderGroups( %allBits );
    ParticleViewerWindow.setRenderLayers( %allBits );
    ParticleViewerWindow.setObjectInputEventGroupFilter( %allBits );
    ParticleViewerWindow.setObjectInputEventLayerFilter( %allBits );
    ParticleViewerWindow.setLockMouse( true );
    ParticleViewerWindow.setCameraPosition( 0, 0 );
    ParticleViewerWindow.setCameraSize( 100, 75 );
    ParticleViewerWindow.setCameraZoom( 1 );
    ParticleViewerWindow.setCameraAngle( 0 );

    // Create a ParticleViewer scene.
    createParticleViewerScene();
}

//-----------------------------------------------------------------------------

function destroyParticleViewerWindow()
{
    // Finish if no window available.
    if ( !isObject(ParticleViewerWindow) )
        return;
    
    // Delete the window.
    ParticleViewerWindow.delete();
}

//-----------------------------------------------------------------------------

function createParticleViewerScene()
{
    // Destroy the scene if it already exists.
    if ( isObject(ParticleViewerScene) )
        destroyParticleViewerScene();
    
    // Create the scene.
    new Scene(ParticleViewerScene);
            
    // Sanity!
    if ( !isObject(ParticleViewerWindow) )
    {
        error( "ParticleViewer: Created scene but no window available." );
        return;
    }
        
    // Set window to scene.
    setSceneToWindow();    
}

//-----------------------------------------------------------------------------

function destroyParticleViewerScene()
{
    // Finish if no scene available.
    if ( !isObject(ParticleViewerScene) )
        return;

    // Delete the scene.
    ParticleViewerScene.delete();
}

//-----------------------------------------------------------------------------

function setSceneToWindow()
{
    // Sanity!
    if ( !isObject(ParticleViewerScene) )
    {
        error( "Cannot set ParticleViewer Scene to Window as the Scene is invalid." );
        return;
    }
    
     // Set scene to window.
    ParticleViewerWindow.setScene( ParticleViewerScene );

    // Set camera to a canonical state.
    %allBits = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31";
    ParticleViewerWindow.stopCameraMove();
    ParticleViewerWindow.dismount();
    ParticleViewerWindow.setViewLimitOff();
    ParticleViewerWindow.setRenderGroups( %allBits );
    ParticleViewerWindow.setRenderLayers( %allBits );
    ParticleViewerWindow.setObjectInputEventGroupFilter( %allBits );
    ParticleViewerWindow.setObjectInputEventLayerFilter( %allBits );
    ParticleViewerWindow.setLockMouse( true );
    ParticleViewerWindow.setCameraPosition( 0, 0 );
    ParticleViewerWindow.setCameraSize( 100, 75 );
    ParticleViewerWindow.setCameraZoom( 1 );
    ParticleViewerWindow.setCameraAngle( 0 );
    
    // Update the toolbox options.
    //updateToolboxOptions();
    
    // reset the ParticleViewer manipulation modes.
    //ParticleViewer.resetManipulationModes();       
}

//-----------------------------------------------------------------------------

function setCustomScene( %scene )
{
    // Sanity!
    if ( !isObject(%scene) )
    {
        error( "Cannot set ParticleViewer to use an invalid Scene." );
        return;
    }
   
    // Destroy the existing scene.  
    destroyParticleViewerScene();

    // The ParticleViewer needs the scene to be named this.
    %scene.setName( "ParticleViewerScene" );    
    
    // Set the scene to the window.
    setSceneToWindow();
}
