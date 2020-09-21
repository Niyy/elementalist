# Elementalist

Elementalist was a group project that was worked on by a team of fourteen members of many different disaplines. The full game can be found here: https://cagd.itch.io/elementalist

## Austin's(Niyy) Contributions

Since there were only three engineers we all had many tasks to take the design of the game and implement it. 

One of my large tasks was implementing UI cursors for players. Each player needed their own cursor to inform the player where their joystick was pointing relative to the game world. I had to go over it a couple of times due to bugs and the addition of other features. The reticle uses Unity's raycast system to detect when the reticle is at max distance from character, if it is hitting another object. If it is, then the reticle will render where the raycast hits the object. This can be found here in the code: https://github.com/Niyy/elementalist/blob/d236cdc7a365bb0f2d7944a7c17277afa8fcc807/elementalist_programmers/Assets/Scripts/Player/PlayerController.cs#L628
