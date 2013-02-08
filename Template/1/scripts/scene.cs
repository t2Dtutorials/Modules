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

function createTemplateWindow()
{
    // Sanity!
    if ( !isObject(TemplateWindow) )
    {
        // Create the scene window.
        new SceneWindow(TemplateWindow);

        // Set profile.        
        TemplateWindow.Profile = TemplateWindowProfile;
        
        // Push the window.
        Canvas.setContent( TemplateWindow );                     
    }

    // Set camera to a canonical state.
    %allBits = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31";
    TemplateWindow.stopCameraMove();
    TemplateWindow.dismount();
    TemplateWindow.setViewLimitOff();
    TemplateWindow.setRenderGroups( %allBits );
    TemplateWindow.setRenderLayers( %allBits );
    TemplateWindow.setObjectInputEventGroupFilter( %allBits );
    TemplateWindow.setObjectInputEventLayerFilter( %allBits );
    TemplateWindow.setLockMouse( true );
    TemplateWindow.setCameraPosition( 0, 0 );
    TemplateWindow.setCameraSize( 100, 75 );
    TemplateWindow.setCameraZoom( 1 );
    TemplateWindow.setCameraAngle( 0 );
}

//-----------------------------------------------------------------------------

function destroyTemplateWindow()
{
    // Finish if no window available.
    if ( !isObject(TemplateWindow) )
        return;
    
    // Delete the window.
    TemplateWindow.delete();
}

//-----------------------------------------------------------------------------

function createTemplateScene()
{
    // Destroy the scene if it already exists.
    if ( isObject(TemplateScene) )
        destroyTemplateScene();
    
    // Create the scene.
    new Scene(TemplateScene);
            
    // Sanity!
    if ( !isObject(TemplateWindow) )
    {
        error( "Template: Created scene but no window available." );
        return;
    }
        
    // Set window to scene.
    setSceneToWindow();    
}

//-----------------------------------------------------------------------------

function destroyTemplateScene()
{
    // Finish if no scene available.
    if ( !isObject(TemplateScene) )
        return;

    // Delete the scene.
    TemplateScene.delete();         
}

//-----------------------------------------------------------------------------

function setSceneToWindow()
{
    // Sanity!
    if ( !isObject(TemplateScene) )
    {
        error( "Cannot set Template Scene to Window as the Scene is invalid." );
        return;
    }
    
     // Set scene to window.
    TemplateWindow.setScene( TemplateScene );

    // Set camera to a canonical state.
    %allBits = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31";
    TemplateWindow.stopCameraMove();
    TemplateWindow.dismount();
    TemplateWindow.setViewLimitOff();
    TemplateWindow.setRenderGroups( %allBits );
    TemplateWindow.setRenderLayers( %allBits );
    TemplateWindow.setObjectInputEventGroupFilter( %allBits );
    TemplateWindow.setObjectInputEventLayerFilter( %allBits );
    TemplateWindow.setLockMouse( true );
    TemplateWindow.setCameraPosition( 0, 0 );
    TemplateWindow.setCameraSize( 100, 75 );
    TemplateWindow.setCameraZoom( 1 );
    TemplateWindow.setCameraAngle( 0 );
           
}

//-----------------------------------------------------------------------------

function setCustomScene( %scene )
{
    // Sanity!
    if ( !isObject(%scene) )
    {
        error( "Cannot set Template to use an invalid Scene." );
        return;
    }
   
    // Destroy the existing scene.  
    destroyTemplateScene();

    // The Template needs the scene to be named this.
    %scene.setName( "TemplateScene" );    
    
    // Set the scene to the window.
    setSceneToWindow();
}

//-----------------------------------------------------------------------------

function TemplateScene::onCollision(%this, %sceneObjectA, %sceneObjectB, %collisionDetails)
{
    if (%sceneObjectA.isMethod(handleCollision))
        %sceneObjectA.handleCollision(%sceneObjectB, %collisionDetails);
    else
        %sceneObjectA.callOnBehaviors(handleCollision, %sceneObjectB, %collisionDetails);

    if (%sceneObjectB.isMethod(handleCollision))
        %sceneObjectB.handleCollision(%sceneObjectA, %collisionDetails);
    else
        %sceneObjectB.callOnBehaviors(handleCollision, %sceneObjectA, %collisionDetails);
}