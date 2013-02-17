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

function moveBehavior::initialize(%this, %up, %down, %left, %right, %acceleration, %turnSpeed, %damping)
{
   %this.upKey = %up;
   %this.downKey = %down;
   %this.leftKey = %left;
   %this.rightKey = %right;
   %this.acceleration = %acceleration;
   %this.turnSpeed = %turnSpeed;
   %this.damping = %damping;
}

function moveBehavior::onBehaviorAdd(%this)
{
   if (!isObject(moveMap))
      return;
   
   %this.owner.setLinearDamping(%this.damping);
   
   moveMap.bindObj(getWord(%this.upKey, 0), getWord(%this.upKey, 1), "moveUp", %this);
   moveMap.bindObj(getWord(%this.downKey, 0), getWord(%this.downKey, 1), "moveDown", %this);
   moveMap.bindObj(getWord(%this.leftKey, 0), getWord(%this.leftKey, 1), "moveLeft", %this);
   moveMap.bindObj(getWord(%this.rightKey, 0), getWord(%this.rightKey, 1), "moveRight", %this);
   
   %this.up = 0;
   %this.down = 0;
   %this.left = 0;
   %this.right = 0;
}

function moveBehavior::onBehaviorRemove(%this)
{
   if (!isObject(moveMap))
      return;

   moveMap.unbindObj(getWord(%this.upKey, 0), getWord(%this.upKey, 1), %this);
   moveMap.unbindObj(getWord(%this.downKey, 0), getWord(%this.downKey, 1), %this);
   moveMap.unbindObj(getWord(%this.leftKey, 0), getWord(%this.leftKey, 1), %this);
   moveMap.unbindObj(getWord(%this.rightKey, 0), getWord(%this.rightKey, 1), %this);
   
   %this.up = 0;
   %this.down = 0;
   %this.left = 0;
   %this.right = 0;
}

function moveBehavior::onMoveUpDown(%this)
{
   %this.owner.setLinearVelocityPolar(%this.owner.getAngle(), (%this.down - %this.up) * %this.acceleration);
}

function moveBehavior::onMoveLeftRight(%this)
{
   %this.owner.setAngularVelocity((%this.left - %this.right) * %this.turnSpeed);
}

function moveBehavior::moveUp(%this, %val)
{
   %this.up = %val;
   
   if (%val == 0)
      return;
      
   %this.onMoveUpDown();
}

function moveBehavior::moveDown(%this, %val)
{
   %this.down = %val;
   
   if (%val == 0)
      return;
      
   %this.onMoveUpDown();
}

function moveBehavior::moveLeft(%this, %val)
{
   %this.left = %val;
   %this.onMoveLeftRight();
}

function moveBehavior::moveRight(%this, %val)
{
   %this.right = %val;
   %this.onMoveLeftRight();
}
