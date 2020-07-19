# Neural Defender: A Neural Algorithm Top-Down Shooter #


Here's information on mechanics and game design that you may need to reference and update from time to time.


## Game Mechanics ##
Put information in here on how CURRENT game mechanics work. As new mechanics are added, do update this.


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

<b>Map Asset Specifications</b>
<br>
- Doorway/space size: 270/50 px (respectively vertical or horizontal)
- Vertical pieces: 1080px
- Horizontal pieces: 1920px 

<b>Enemy Spawning</b>

<b>Level Manager</b>