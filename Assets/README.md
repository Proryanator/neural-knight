# Neural Defender: A Neural Algorithm Top-Down Shooter #


Here's information on mechanics and game design that you may need to reference and update from time to time.


## Game Mechanics ##
Put information in here on how CURRENT game mechanics work. As new mechanics are added, do update this.

### Player ###
<b>Movement:</b>
    You always face the direction of the mouse, and you fire in that direction too.
    Your legs face either your facing direction, or make you 'walk backwards' based on your current movement.

Shooting:
    Player shots will damage enemies, and will disappear on good data contact + hitting walls. Shots also
    have a set lifetime that will fade at some point and die off.
    


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