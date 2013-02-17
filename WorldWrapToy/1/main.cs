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

function WorldWrapToy::create( %this )
{
    // Load game scripts
    exec("./scripts/game.cs");
    exec("./scripts/moveBehavior.cs");
   
    // Setup keyboard bindings.
    new ActionMap(moveMap);   
    moveMap.push();
   
    $enableDirectInput = true;
    activateDirectInput();
    
    // Create behaviors
    %this.createMoveBehavior();
    
    // Create starting values
    WorldWrapToy.upKey = "keyboard up";
    WorldWrapToy.downKey = "keyboard down";
    WorldWrapToy.leftKey = "keyboard left";
    WorldWrapToy.rightKey = "keyboard right";
    WorldWrapToy.acceleration = 30;
    WorldWrapToy.turnSpeed = 150;
    WorldWrapToy.damping = 1;
    
    // Add Sandbox config options
    addNumericOption( "Acceleration", 1, 100, 5, "setAcceleration", WorldWrapToy.acceleration, true, "Sets the forward acceleration" );
    addNumericOption( "Turn Speed", 1, 300, 5, "setTurnSpeed", WorldWrapToy.turnSpeed, true, "Sets the velocity of turning" );
    addNumericOption( "Damping", 0, 5, 0.1, "setDamping", WorldWrapToy.damping, true, "Sets the amount to damp movement" );
    addSelectionOption( "keyboard up,keyboard w", "Up Key", 2, "setUpKey", true, "Key to bind to acceleration" );
    addSelectionOption( "keyboard down,keyboard s", "Down Key", 2, "setDownKey", true, "Key to bind to deceleration" );
    addSelectionOption( "keyboard left,keyboard a", "Left Key", 2, "setLeftKey", true, "Key to bind to rotate left" );
    addSelectionOption( "keyboard right,keyboard d", "Right Key", 2, "setRightKey", true, "Key to bind to rotate right" );
    
    // Reset the game.
    WorldWrapToy.reset();
    
}

//-----------------------------------------------------------------------------

function WorldWrapToy::destroy( %this )
{
    // Destroy keyboard bindings.
    moveMap.pop();
    moveMap.delete();
}

//-----------------------------------------------------------------------------

function WorldWrapToy::reset( %this )
{
    // Clear the scene.
    SandboxScene.clear();
       
    // Start your game here.
    %this.createBackground();
    %this.createPlayer();
    %this.createTriggers();
}

//-----------------------------------------------------------------------------

function WorldWrapToy::setUpKey(%this, %value)
{
   %this.upKey = %value;
}

function WorldWrapToy::setDownKey(%this, %value)
{
   %this.downKey = %value;
}

function WorldWrapToy::setLeftKey(%this, %value)
{
   %this.leftKey = %value;
}

function WorldWrapToy::setRightKey(%this, %value)
{
   %this.rightKey = %value;
}

function WorldWrapToy::setAcceleration(%this, %value)
{
   %this.acceleration = %value;
}

function WorldWrapToy::setTurnSpeed(%this, %value)
{
   %this.turnSpeed = %value;
}

function WorldWrapToy::setDamping(%this, %value)
{
   %this.damping = %value;
}