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

function Template::create( %this )
{
   // Load the preferences.
    %this.loadPreferences();
    
    // Load system scripts
    exec( "./scripts/canvas.cs" );
    exec( "./scripts/openal.cs" );
    exec( "./scripts/scene.cs" );

    // Initialize the canvas.
    initializeCanvas("Template");
    
    // Set the canvas color.
    Canvas.BackgroundColor = "CornflowerBlue";
    Canvas.UseBackgroundColor = false;
    
    // Initialize audio.
    initializeOpenAL();
    
    // Load GUI profiles.
    exec("./gui/guiProfiles.cs");

    // Create the template window.
    createTemplateWindow();
    
    // Create the template scene.
    createTemplateScene();
    
    // Reset the template.
    Template.reset();
    
}

//-----------------------------------------------------------------------------

function Template::destroy( %this )
{
    // Destroy the template window.
    destroyTemplateWindow();
}

//-----------------------------------------------------------------------------

function Template::loadPreferences( %this )
{
    // Load the default preferences.
    exec( "./scripts/defaultPreferences.cs" );
    
    // Load the last session preferences if available.
    if ( isFile("preferences.cs") )
        exec( "preferences.cs" );   
}

function Template::reset( %this )
{
    // Clear the scene.
    TemplateScene.clear();
       
    // Start your game here.
        
}