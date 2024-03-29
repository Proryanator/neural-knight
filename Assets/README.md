﻿# Neural Defender: A Neural Algorithm Top-Down Shooter #

Here's information on mechanics and game design that you may need to reference and update from time to time.

## Table of Contents ##
-Project Structure
-Prefab Structure
-Game Mechanics
-Player Mechanics/Abilities
-Maps
-Art Source (If Any)

## Prefab Structure: ##
You will find nested prefabs all throughout this project.

You'll have your main prefab, which will be the one that you drag into your scenes. Those will be at the highest level in the prefab folder.
If there are sub-prefabs you'll also have a folder called 'Parts' that will have the nested prefabs, or parts, of that prefab.

Do take care to open the prefab using the 'Open prefab' menu when editing it, this way you ensure that you're actually updating the prefab itself.
 
## Game Mechanics ##
Put information in here on how CURRENT game mechanics work. As new mechanics are added, do update this.

### Player ###
Movement:
    You always face the direction of the mouse, and you fire in that direction too.
    Your legs face either your facing direction, or make you 'walk backwards' based on your current movement.

Death:
    When the player dies, we still want enemies and AI to track it's last spot.
    
    There's an object in the scene with the tag of AllTags.AI_TRACKER, which is a child of the player. It will
    be detached and left in the scene to be a tracking point for enemy movement.
    
Shooting:
    Player shots will damage enemies, and will disappear on good data contact + hitting walls. Shots also
    have a set lifetime that will fade at some point and die off. How enemies are damaged is handled in their
    own script, however how much damage to apply is determined by the weapon type, and passed through the projectile.
    

### Enemies ###
Movement:
Enemy movement is controlled via an AIMovementController, which takes an assigned AbstractAIMovementPattern.

These patterns decide what the enemy does per-update call, either apply force, follow some pre-defined path, or nothing at all.

To change the enemy's movement, simply change their pattern type and the controller will just run that defined movement.

Taking Damage/Applying Damage:

Enemies apply damage to the player by colliding with them.

When enemies are attacked/hit, they will swap out their current movement pattern for that of the NoMovementPattern.
A force will also be applied to the enemy to cause them to fly back, as well as their layer will be adjusted to a custom layer,
'Damaged Enemy', which makes enemies not collide with other enemies (so they can fly back when attacked).

### Maps ###
Maps will be a big part of this game. Let's add how maps will work, and how new ones will be made.

<b>Map Composition</b>
<br>

A map will consist of parts and pieces that are intended to be swapped out at runtime.

<i>Walls</i>
<br>There will be 4 walls at all times, 2 assets (top/down and left/right).
Walls will either be <i>open</i> with a space in the middle, or will be fully closed.

Wall objects that are open will have a collider surrounding the wall, as well as child objects for the player boundary. See 'Player Boundary' section.

<i>Player Boundaries</i>
<br>
When there's an opening in the wall, the player cannot walk through it. There is an object nested in wall objects that acts as the 'Player Boundary Trigger', and a child object that
is the actual boundary. When the trigger is triggered, the child boundary/collider is enabled, thus preventing the player from passing.
<br>
At some point there will be a texture attached to this to show players that you cannot pass (at least while enemies are present).

The parent is using a trigger, and thus nothing collides with it. The child, uses the layer 'Player Boundary', and this layer only collides with the player and it's shots.
Allowing enemies and other entities to pass through.

<i>Play Area</i>

Defines the playable area, via a collider. Currently this is used to change a controller for the enemy movement when outside the playable area.
 
The play area must be large enough to get incoming entities, but not so small that enemies moving around inside of it would re-trigger it.

This would cause some uneeded logic + calls to happen.

Layer to keep entities inside of playable map: EntityBoundary + Entity.
These layers will be used to keep entities that wander around the map inside of the playable area.
When an entity spawns, it does a check with the play area to see if it's inside.

If it's inside, it'll be set to the 'Entity' layer.
If not, it'll remain as it was before.
 
<b>Map Asset Specifications</b>
<br>
- Doorway/space size: 270/50 px (respectively vertical or horizontal)
- Vertical pieces: 1080px
- Horizontal pieces: 1920px 

<b>Enemy Spawning</b>
<i>Spawn Points</i>
Spawn points are just objects that are children of a SpawnCollection object (an actual script). That script simply gives you all the active children in the heirarchy.
This will be useful if at some point this game has room generation, with walls that have no doors being generated, and needing to have those spawners turned off.

Spawn points will be 'outside the play area', and will spawn enemies there. When an enemy is outside the play area,
they will be set to a movement pattern that locks onto an object tagged with 'MapCentralPoint'. The enemy will them move towards that object, until they are considered 'inside the play area' via a trigger call,
which will then set them to their original movement pattern.

This allows for straight movement towards the center of the map upon spawning.

<i>SpawnManagers</i>
 
These have information defined about them in a SpawnProperties object, things like spawn delay, speed, and max/min values.

This is read by the SpawnManager, and is used to determine if spawning is still happening.

<b>Level Manager</b>
## Art Source (If Any) ##
I may borrow art during the development process for the sake of playing with mechanics, let's keep track of those here.
Basic Ground texture: https://www.artstation.com/artwork/OywBxv