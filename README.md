# Zig Zag Toe

## General
### What it is
This is a sample game based on the 'Endless runner' principle.  
It is inspired by the legenday 'Zig Zag' game which can be found here: https://play.google.com/store/apps/details?id=com.ketchapp.zigzaggame&hl=en 
### Functionalities
The basic functionalities are:
* Changing the player's direction by tapping on the screen
* Jumping by swiping up
### Goal
Since the game is based on the endless runner principle, there is no end goal besides maximizing the score in each try.  
The player should stay alive for as long as he can, jumping arbitray placed obstacles (holes).  
The score is increased by one for each tap (a change in direction), or by +3 from a green power-up (described below) 
### Power ups
Along the path, the player can pick up 3 types of power-ups:
* Green power-up: Awards player +3 points
* Purple power-up: Speeds up player for 20% of current speed for 7 seconds
* Yellow power-up: Slows down player for 20% of current speed for 7 seconds
### What's different from the original?
1. **Jumping**: For starters, we've included holes in the pathway, which itself required a jumping feature to be introduced as well. This required altering the physics engine in the game, as well as handling the specific input events from the player.
2. **Additional powerups**: In order to make the game even harder and more unpredictable, we've added powerups that speed up and slow down the player.

## Solution
In the creating of the solution, we've used a lot of the functionalities provided by the engines of **Unity**, alongside scripting.  
For this purpose, a few classes were developed:
### PlayerScript.cs
This script is attached to the player.  
It has many different properties, such as: Speed, Speed State, Direction and so on.  
It mainly handles the inputs so it can change direction, as well as triggers from Pick Up colliders.
### TileManager.cs
This script is attached to an empty GameObject that exists within the game.  
The manager is built upon the Singleton Design Pattern, in order to ensure that one and only one manager per game exists.  
Its main properties are the 2 Stacks of tiles (for each type of tile accordingly: left tiles and top tiles).  
The goal of this script is to recycle the tiles that fall down after the player passes them, in order not to use too much CPU power for creating new ones all the time, as well as saving RAM.  
### TileScipt.cs (code described below)
This script is attached to each tile in the game.  
It's point is initiate fall down sequence once the player passes it.  
### ParticleScript.cs
This script is attached to a particle system.  
It's point is to initate the particle system in order to create an illusion of an explotion once the player collects a power up.

## Script Description (TileScript.cs)
As previously mentioned, each tile in the game has this script atteched to it. 
The script has a single property:
```cs
    public static int fallDelay = 1;
```
Also, each tile has a **Box Collider** component, with the 'Is Trigger' property set to True.  
This leads to the explanation of the first method within the script:
```cs
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            TileManager tileManager = TileManager.getInstance();
            tileManager.SpawnTile();
            StartCoroutine(FallDown());
        }
    }
```
This method is called by the engine whenever the other collider i.e. the player exits the box collider of the tile previously mentioned.  
What this methods does is, once the player leaves a tile, an instance of the TileManager is obtained (through the Singleton Pattern) and a new tile is spawned at the end of the path.  
Also, a FallDown() coroutine is started, which leads to the explanation of the second method within the script:
```cs
    IEnumerator FallDown()
    {
        yield return new WaitForSeconds(fallDelay);
        GetComponent<Rigidbody>().isKinematic = false;

        yield return new WaitForSeconds(fallDelay);
        TileManager tileManager = TileManager.getInstance();

        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        gameObject.SetActive(false);

        switch (gameObject.name)
        {
            case "LeftTile":
                tileManager.LeftTiles.Push(gameObject);
                break;

            case "TopTile":
                tileManager.TopTiles.Push(gameObject);
                break;
        }
    }
```
What this method does is, it waits for 1 second once the player exits the tile, starts the fall down sequence of the player (by setting the isKinematic property of the player to false).  
Then it waits for 1 more second and recycles the tile by resetting its properties and pushing it to the appropriate stack of the tile manager.

## Screenshots
![Gameplay screenshots](https://github.com/ibanezo/ZigZagToe/blob/master/screenshots.png "Gameplay screenshots")

