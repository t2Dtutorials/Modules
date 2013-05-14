T2D-Modules
===========

Additional Modules for [Torque 2D](https://github.com/GarageGames/Torque2D). Simply clone this repo or download the zip package and add the toy folders to your module folder. See the descriptions below for more details on each individual module.

### AudioBasicsToy ###

A Sandbox toy that covers the basics of audio playing in T2D. Includes a background music player and a short sound effects demo. All sounds are from the ToyAssets module.

### FadeInOutToy ###

This Sandbox toy shows how to use the color blending features of scene objects to create a fade in / fade out effect or to transition from one color to another.

### WorldWrapToy ###

A toy for the Sandbox that demonstrates keyboard movement (customizable using arrow keys or WSAD) and a world limit wrap function to move the player object from one side of the screen to the other.

### ParticleViewer ###

A seperate module that does **not** run in the Sandbox. This module will load appropriately defined ParticleAssets and also gives you the ability to reload them, allowing for semi-real time particle editing when used together with a TAML editing program. To load this module, go into AppCore, main.cs and change this line:

```cpp
ModuleDatabase.loadGroup("gameBase");
```

to

```cpp
ModuleDatabase.loadGroup("viewer");
```
