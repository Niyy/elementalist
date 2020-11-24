# Elementalist
------------------
Elementalist was a group project that was worked on by a team of fourteen members of many different disaplines. The full game can be found here: https://cagd.itch.io/elementalist

## Austin's(Niyy) Contributions
### UI Cursors
Since there were only three engineers we all had many tasks to take the design of the game and implement it. 

One of my large tasks was implementing the UI cursors for players. Each player needed their own cursor to inform the player where their joystick was pointing relative to their player. I had to go over it a couple of times due to bugs and the addition of other features. The reticle uses Unity's raycast system to detect walls, ledges, and enemies. If it hits one of these objects, then the reticle will render where the raycast hits the object, rather than a max radius from the player.

```c#
protected void ReticleMovement()
{
    RaycastHit hit;
    Vector3 left_stick_position = new Vector3(move.x, move.y, 0.0f) * retical_radius;


    if (Physics.Raycast(this.transform.position, left_stick_position, out hit, retical_radius, ~(1 << 10))
        && !hit.collider.gameObject.tag.Equals("Player"))
    {
        retical.transform.position = hit.point;
    }
    else
    {
        left_stick_position += this.transform.position;
        retical.transform.position = left_stick_position;
    }

    ui_retical.transform.position = Camera.main.WorldToScreenPoint(retical.transform.position);

}
```
<br>

#### Animation's
I was tasked with implementing 3D animations into the game as well. I learned a lot from this process since it was my first time with 3D animations.

I learned a lot from the animators on how to have the animations not add to the transform of the object. This was causing issues translating the object slowly away from where it actually is.

We had four different characters in the game. We used OOP design to create the base character. Using the existing code archetecture I made virtual function for the animations to lay out a template for future animations if they differed dramatically. Most characters used the base character animation handler.

```c#
protected virtual void AnimationHandler()
{
    PlayerCollision player_collision = GetComponent<PlayerCollision>();

    DashAnimation(player_collision);
    JumpAnimation(player_collision);
    WallSlideAnimation(player_collision);
    RunningAnimation(player_collision);
    IdleAnimation(player_collision);

    DefineFacingDirection();
}
```

The water character on the other hand needed the base to be virtual, because it had more animations than the base class.

```c#
protected override void AnimationHandler()
{
    PlayerCollision player_collision = GetComponent<PlayerCollision>();

    DashAnimation(player_collision);
    JumpAnimation(player_collision);
    WallSlideAnimation(player_collision);
    RunningAnimation(player_collision);
    IdleAnimation(player_collision);
    HoverAnimation(player_collision);
    AttackAnimation(player_collision);

    DefineFacingDirection();
}
```
