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

function WorldWrapToy::createBackground(%this)
{    
    // Create the sprite.
    %object = new Sprite();
    
    // Set the sprite as "static" so it is not affected by gravity.
    %object.setBodyType(static);
       
    // Always try to configure a scene-object prior to adding it to a scene for best performance.

    // Set the position.
    %object.Position = "0 0";

    // Set the size.        
    %object.Size = "100 75";
    
    // Set to the furthest background layer.
    %object.SceneLayer = 31;
    
    // Set the image
    %object.Image = "ToyAssets:highlightBackground";
    
    // Change the image background color for better contrast with the player sprite
    %object.setBlendMode(true);
    %object.setBlendColor("SteelBlue");
            
    // Add the sprite to the scene.
    SandboxScene.add(%object);    
}

function WorldWrapToy::createPlayer( %this )
{    
    // Create the sprite.
    %object = new Sprite() {class = "PlayerClass";};
    
    // Always try to configure a scene-object prior to adding it to a scene for best performance.

    // Set the position.
    %object.Position = "0 0";
        
    // If the size is to be square then we can simply pass a single value.
    // This applies to any 'Vector2' engine type.
    %object.Size = 6;
    
    // Set the sprite to use an image.  This is known as "static" image mode.
    %object.Image = "ToyAssets:hollowArrow";
    
    // Set the group and a field to handle trigger collisions
    %object.SceneGroup = "14";
    %object.hitTrigger = 0;
    
    %object.createCircleCollisionShape(3);
    %object.setCollisionGroups(15);
    // Player collision callback not needed right now thanks to onCollision changes
    // %object.setCollisionCallback(true);
    
    // Add the move behavior to the player
    %moveBehavior = moveBehavior.createInstance();
    %moveBehavior.initialize(%this.upKey, %this.downKey, %this.leftKey, %this.rightKey, %this.acceleration, %this.turnSpeed, %this.damping);
    %object.addBehavior(%moveBehavior);
        
    // Add the sprite to the scene.
    SandboxScene.add(%object);
}

function WorldWrapToy::createTriggers(%this)
{
    // Left trigger
    %leftTrigger = new SceneObject() {class = "LevelBoundary";};
    
    %leftTrigger.side = "left";
    %leftTrigger.setSize(5, 85);
    %leftTrigger.setPosition(-58, 0);
    %leftTrigger.setSceneLayer(1);
    %leftTrigger.setSceneGroup(15);
    %leftTrigger.setCollisionGroups(14);
    %leftTrigger.createPolygonBoxCollisionShape();
    %leftTrigger.setDefaultDensity(1);
    %leftTrigger.setDefaultFriction(1.0);        
    %leftTrigger.setAwake(true);
    %leftTrigger.setActive(true);
    %leftTrigger.setCollisionCallback(true);
    %leftTrigger.setBodyType("static");
    %leftTrigger.setCollisionShapeIsSensor(0, true);
    SandboxScene.add(%leftTrigger);
    
    // Right trigger
    %rightTrigger = new SceneObject() {class = "LevelBoundary";};
    
    %rightTrigger.setSize(5, 85);
    %rightTrigger.side = "right";
    %rightTrigger.setPosition(58, 0);
    %rightTrigger.setSceneLayer(1);
    %rightTrigger.setSceneGroup(15);
    %rightTrigger.setCollisionGroups(14);
    %rightTrigger.createPolygonBoxCollisionShape();
    %rightTrigger.setDefaultDensity(1);
    %rightTrigger.setDefaultFriction(1.0);    
    %rightTrigger.setAwake(true);
    %rightTrigger.setActive(true);
    %rightTrigger.setCollisionCallback(true);
    %rightTrigger.setBodyType("static");
    %rightTrigger.setCollisionShapeIsSensor(0, true);
    SandboxScene.add(%rightTrigger);
    
    // Top trigger
    %topTrigger = new SceneObject() {class = "LevelBoundary";};
    
    %topTrigger.setSize(110, 5);
    %topTrigger.side = "top";
    %topTrigger.setPosition(0, 45);
    %topTrigger.setSceneLayer(1);
    %topTrigger.setSceneGroup(15);
    %topTrigger.setCollisionGroups(14);
    %topTrigger.createPolygonBoxCollisionShape();
    %topTrigger.setDefaultDensity(1);
    %topTrigger.setDefaultFriction(1.0);    
    %topTrigger.setAwake(true);
    %topTrigger.setActive(true);
    %topTrigger.setCollisionCallback(true);
    %topTrigger.setBodyType("static");
    %topTrigger.setCollisionShapeIsSensor(0, true);
    SandboxScene.add(%topTrigger);
    
    // Bottom trigger
    %bottomTrigger = new SceneObject() {class = "LevelBoundary";};
    
    %bottomTrigger.setSize(110, 5);
    %bottomTrigger.side = "bottom";
    %bottomTrigger.setPosition(0, -45);
    %bottomTrigger.setSceneLayer(1);
    %bottomTrigger.setSceneGroup(15);
    %bottomTrigger.setCollisionGroups(14);
    %bottomTrigger.createPolygonBoxCollisionShape();
    %bottomTrigger.setDefaultDensity(1);
    %bottomTrigger.setDefaultFriction(1.0);    
    %bottomTrigger.setAwake(true);
    %bottomTrigger.setActive(true);
    %bottomTrigger.setCollisionCallback(true);
    %bottomTrigger.setBodyType("static");
    %bottomTrigger.setCollisionShapeIsSensor(0, true);
    SandboxScene.add(%bottomTrigger);
}

function LevelBoundary::onCollision(%this, %object, %collisionDetails)
{
   // Check if the object has the player class, then check if the wrap function should be called
    if (%object.class $= "PlayerClass")
    {
       if (%object.hitTrigger == 0)
       {
          %object.wrap(%this.side);
       }else
       {
          %object.hitTrigger = 0;
       }  
    }
}

function PlayerClass::wrap(%this, %side)
{
   // This will prevent the wrap function from being called again after being moved to the other
   // side of the screen. Bad things happen otherwise...
   %this.hitTrigger = 1;
   
   // Set up the new position
   switch$(%side)
   {
      case "left":
      %newX = %this.Position.x * -1;
      %newY = %this.Position.y;
      
      case "right":
      %newX = %this.Position.x * -1;
      %newY = %this.Position.y;
      
      case "top":
      %newX = %this.Position.x;
      %newY = %this.Position.y * -1;
      
      case "bottom":
      %newX = %this.Position.x;
      %newY = %this.Position.y * -1;
   }

   %newPosition = %newX SPC %newY;
   %this.setPosition(%newPosition);
      
}

function WorldWrapToy::createMoveBehavior(%this)
{
   %this.moveBehavior = new BehaviorTemplate(moveBehavior);
   
   %this.moveBehavior.friendlyName = "Spaceship Movement";
   %this.moveBehavior.behaviorType = "Input";
   %this.moveBehavior.description  = "Spaceship style movement control";
   
   %this.moveBehavior.addBehaviorField(upKey, "Key to bind to acceleration", keybind, "keyboard up");
   %this.moveBehavior.addBehaviorField(downKey, "Key to bind to deceleration", keybind, "keyboard down");
   %this.moveBehavior.addBehaviorField(leftKey, "Key to bind to rotate left", keybind, "keyboard left");
   %this.moveBehavior.addBehaviorField(rightKey, "Key to bind to rotate right", keybind, "keyboard right");
   
   %this.moveBehavior.addBehaviorField(acceleration, "Forward acceleration", float, 2.0);
   %this.moveBehavior.addBehaviorField(turnSpeed, "Velocity of turning", float, 120.0);
   %this.moveBehavior.addBehaviorField(damping, "Amount to damp movement", float, 1.0);
   
   // Add the BehaviorTemplate to the scope set so it is destroyed when the module is unloaded
   WorldWrapToy.add(%this.moveBehavior);
}