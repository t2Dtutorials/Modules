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

ParticleViewer.ActivePlayer = "";

//-----------------------------------------------------------------------------

function scanForAssets()
{
    // Create query instance.
    ParticleViewer.AssetList = new AssetQuery();

    // Find all ParticleAsset types.
    AssetDatabase.findAssetType(ParticleViewer.AssetList, "ParticleAsset");

    // Fetch the query count.
    %queryCount = ParticleViewer.AssetList.getCount();
}

//-----------------------------------------------------------------------------

function loadParticleEffect(%assetId)
{
    // Unload the active particle effect.
    if (isObject(ParticleViewer.ActivePlayer))
        unloadParticleEffect();    
        
    // Load the particle.
    %player = new ParticlePlayer();
    %player.Particle = %assetId;
    %player.SceneLayer = 5;

    // Add the player to the scene.
    ParticleViewerScene.add(%player);
    
    // Set the active player so we can reference it elsewhere.
    ParticleViewer.ActivePlayer = %player; 
}

//-----------------------------------------------------------------------------

function unloadParticleEffect()
{
    // Stop the effect and kill it.
    ParticleViewer.ActivePlayer.stop(false, true);

    // Reset the ActivePlayer field.
    ParticleViewer.ActivePlayer = "";
}
