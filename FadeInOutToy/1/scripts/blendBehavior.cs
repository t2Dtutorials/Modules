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

function BlendBehavior::initialize(%this, %endColor, %time, %increment, %pulse)
{
   %this.endColor = getStockColorF(%endColor);
   %this.time = %time;
   %this.increment = %increment;
   %this.pulse = %pulse;
}

function BlendBehavior::onAddToScene(%this)
{
   %colorName = %this.owner.getBlendColor();
   %this.startColor = getStockColorF(%colorName);
   %this.duration = %this.time * 1000;
   %this.changeStartColor(%this.changeLength);
}

function BlendBehavior::changeStartColor(%this, %time)
{
   if(%time > %this.increment)
   {
      %color = %this.owner.getBlendColor();
      
      // Check if this is a stock color or not. If stock, we need to convert the name to RGBA values.
      if(%color.count == 1)
      {
         %colorF = getStockColorF(%color);
         %red = %colorF.r;
         %green = %colorF.g;
         %blue = %colorF.b;
         %alpha = %colorF.a;
      }else
      {
         %red = %color.r;
         %green = %color.g;
         %blue = %color.b;
         %alpha = %color.a;
      }
      
      %updatesRemaining = %time / %this.increment;
      %red += (%this.endColor.r - %red) / %updatesRemaining;
      %green += (%this.endColor.g - %green) / %updatesRemaining;
      %blue += (%this.endColor.b - %blue) / %updatesRemaining;
      %alpha += (%this.endColor.a - %alpha) / %updatesRemaining;
      %newColor = %red SPC %green SPC %blue SPC %alpha;
      
      %this.owner.setBlendColor(%newColor);
      
      %this.schedule(%this.increment, "changeStartColor", %time - %this.increment);
   }else
   {
      %this.owner.setBlendColor(%this.endColor);
      
      if (%this.pulse == true)
         %this.schedule(1000, "changeEndColor", %this.duration);
   }
}

function BlendBehavior::changeEndColor(%this, %time)
{
   if(%time > %this.increment)
   {
      %color = %this.owner.getBlendColor();
      
      // Check if this is a stock color or not. If stock, we need to convert the name to RGBA values.
      if(%color.count == 1)
      {
         %colorF = getStockColorF(%color);
         %red = %colorF.r;
         %green = %colorF.g;
         %blue = %colorF.b;
         %alpha = %colorF.a;
      }else
      {
         %red = %color.r;
         %green = %color.g;
         %blue = %color.b;
         %alpha = %color.a;
      }
      
      %updatesRemaining = %time / %this.increment;
      %red += (%this.startColor.r - %red) / %updatesRemaining;
      %green += (%this.startColor.g - %green) / %updatesRemaining;
      %blue += (%this.startColor.b - %blue) / %updatesRemaining;
      %alpha += (%this.startColor.a - %alpha) / %updatesRemaining;
      %newColor = %red SPC %green SPC %blue SPC %alpha;
      
      %this.owner.setBlendColor(%newColor);
      
      %this.schedule(%this.increment, "changeEndColor", %time - %this.increment);
   }else
   {
      %this.owner.setBlendColor(%this.startColor);
      
      if (%this.pulse == true)
         %this.schedule(1000, "changeStartColor", %this.duration);
   }
}