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

function FadeInOutToy::create(%this)
{
   // Load scripts
   exec("./scripts/blendBehavior.cs");
   
   // Create behaviors
   %this.createBlendBehavior();
   
   // Configure the default values
   %this.spriteTime = 1.5;
   %this.textTime = 1.0;
   %this.increment = 30;
   
   // Add configuration option.
   addNumericOption("Time (Sprites)", 0.1, 5.0, 0.1, "setSpriteTime", FadeInOutToy.spriteTime, true, "Time (in seconds) to complete a blend change");
   addNumericOption("Time (Text)", 0.1, 5.0, 0.1, "setTextTime", FadeInOutToy.textTime, true, "Time (in seconds) to complete a blend change");
   addNumericOption("Increment", 1, 50, 1, "setIncrement", FadeInOutToy.increment, true, "Sets the smoothness of the blend changes");
    
   // Reset the toy.
   FadeInOutToy.reset();
}


//-----------------------------------------------------------------------------

function FadeInOutToy::destroy(%this)
{
}

//-----------------------------------------------------------------------------

function FadeInOutToy::reset(%this)
{
   // Clear the scene.
   SandboxScene.clear();
       
   // Create the background
   %this.createBackground();
       
   // Create some static sprites
   %this.createStaticSprites();
    
   // Create some text
   %this.createText();
}

//-----------------------------------------------------------------------------

function FadeInOutToy::createBackground(%this)
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
    
   // Set the scroller to use an animation!
   %object.Image = "ToyAssets:highlightBackground";
    
   // Set the blend color.
   %object.BlendColor = SlateGray;
            
   // Add the sprite to the scene.
   SandboxScene.add(%object);
}

//-----------------------------------------------------------------------------

function FadeInOutToy::createStaticSprites(%this)
{
   // Create the sprites.
   %object1 = new Sprite();
   %object2 = new Sprite();
   %object3 = new Sprite();

   // Set the position.
   %object1.Position = "-25 10";
   %object2.Position = "0 10";
   %object3.Position = "25 10";
        
   // If the size is to be square then we can simply pass a single value.
   %object1.Size = 20;
   %object2.Size = 20;
   %object3.Size = 20;
    
   // Set the sprites to use an image.  This is known as "static" image mode.
   %object1.Image = "ToyAssets:Crosshair3";
   %object2.Image = "ToyAssets:Crosshair3";
   %object3.Image = "ToyAssets:Crosshair3";

   // To make the objects rotate, set as dynamic body type and give it an angular velocity
   %object1.setBodyType("dynamic");
   %object1.setAngularVelocity(15);
   
   %object2.setBodyType("dynamic");
   %object2.setAngularVelocity(-15);
   
   %object3.setBodyType("dynamic");
   %object3.setAngularVelocity(15);
   
   // Assign the sprites the blend behavior
   %blendBehavior1 = BlendBehavior.createInstance();
   %blendBehavior1.initialize("CornflowerBlue", FadeInOutToy.spriteTime, FadeInOutToy.increment, true);
   %object1.addBehavior(%blendBehavior1);
   
   %blendBehavior2 = BlendBehavior.createInstance();
   %blendBehavior2.initialize("Red", FadeInOutToy.spriteTime, FadeInOutToy.increment, true);
   %object2.addBehavior(%blendBehavior2);
   
   %blendBehavior3 = BlendBehavior.createInstance();
   %blendBehavior3.initialize("Olive", FadeInOutToy.spriteTime, FadeInOutToy.increment, true);
   %object3.addBehavior(%blendBehavior3);
        
   // Add the sprites to the scene.
   SandboxScene.add(%object1);
   SandboxScene.add(%object2);
   SandboxScene.add(%object3);
}

//-----------------------------------------------------------------------------

function FadeInOutToy::createText(%this)
{
   // Create the image font.
   %object = new ImageFont();
    
   // Set the image font to use the font image asset.
   %object.Image = "ToyAssets:Font";
    
   // Set the position
   %object.Position = "0 -20";
   
   // Set the font size in both axis.  This is in world-units and not typicaly font "points".
   %object.FontSize = "3 3";

   // Set the text alignment.
   %object.TextAlignment = "Center";

   // Set the text to display.
   %object.Text = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
   
   // Assign the text the blend behavior
   %blendBehavior = BlendBehavior.createInstance();
   %blendBehavior.initialize("TransparentWhite", FadeInOutToy.textTime, FadeInOutToy.increment, true);
   %object.addBehavior(%blendBehavior);

   // Add the sprite to the scene.
   SandboxScene.add(%object); 
}

//-----------------------------------------------------------------------------

function FadeInOutToy::createBlendBehavior(%this)
{
   %template = new BehaviorTemplate(BlendBehavior);
   
   %template.friendlyName = "Fade In Out";
   %template.behaviorType = "Effects";
   %template.description  = "Changes the objects alpha or color";

   %template.addBehaviorField(time, "The time (in seconds) to change", float, 1.0);
   %template.addBehaviorField(increment, "Sets the smoothness of the color changes", int, 30);   

   %template.addBehaviorField(pulse, "Continuously change back and forth (Pulse)", bool, false);
   
   // Add the BehaviorTemplate to the scope set so it is destroyed when the module is unloaded
   FadeInOutToy.add(%template);
}

//-----------------------------------------------------------------------------

function FadeInOutToy::setSpriteTime(%this, %value)
{
   %this.spriteTime = %value;
}

function FadeInOutToy::setTextTime(%this, %value)
{
   %this.textTime = %value;
}

function FadeInOutToy::setIncrement(%this, %value)
{
   %this.increment = %value;
}