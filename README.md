# Urf's Revenge

## Synopsis

Urf's Revenge is a 2D platform shooter, using League of Legends API data to procedurally generate enemies. The game client is built in the freely available Unity Engine, with a Python script hosted on Google App Engine to power the API requests.

## Loading A Match

There are three ways to load up a match in Urf's Revenge

1. Use the summoner lookup tool to find the 10 most recent games played, then select the desired match
2. Use a random match from the server's cache of match data
3. Enter a specific match ID, this can be found on the Match History details page by looking at the URL (ie. http://matchhistory.na.leagueoflegends.com/en/#match-details/NA1/XXXXXXXXXX)

Once a match has been loaded, click on any of the champion icons to begin the match as that champion.

## Controls

Movement: Arrow Keys or WASD
Jump: Space Bar
Crouch: Left Shift Key or Left Ctrl Key
Shoot: F or J
Pause: Escape

## Procedural Generation

The player continues to level up throughout each section of the game.  There are a number of match values that affect statistics in the game, in the form of a "growth stat" which defines how the stat scales with levels.

* Player health is dependent on total damage taken in the match
* Minion health and damage are dependent on the final CS of the current champion
* Enemy champions are scaled depending on their final damage taken and damage dealt to champions stat
* Enemy champions appear in the order and proportionally to when they were either killed or assisted by the current player

## Scoring Information

Once the player dies, their score will be calculated using the following formula:

```
Cs = Caster Minions Killed
Me = Melee Minions Killed
Cn = Cannon Minions Killed
En = Enemy Champions Killed
Ac = Projectile Accuracy
Wb = Waffles Collected
Fs = Final Score
```

    ((Cs*5 + Me*10 + Cn*25 + En*50) * Ac) + Wa*50 = Fs

### Example Score


```
25  Caster Minions Killed
15  Melee Minions Killed
3   Cannon Minions Killed
25  Enemy Champions Killed
75% Projectile Accuracy
15  Waffles Collected
```
    ((25*5 + 15*10 + 3*25 + 25*50) * 0.75) + 15*50 = 1950

## Live Demo

A live demo of the project can be accessed at http://urfsrevenge.com

## Installation

The only program required to build the game is the Unity Engine (http://unity3d.com/get-unity/download).  Simply clone the repository, tell Unity to open an existing project, and select the local repository directory.  Once the project has been opened, you can use Unity to build to any number of targets.  For development, the Web Player and WebGL Preview were targeted.

## Planned Features

* Support multiple server regions
* Use Baron/Dragon events to give a short powerup
* Add melee attacks
* Death animations for the player, minions, and enemies
* Multiple attack patterns for final boss Urf
* Music and sounds effects
* Passive health regeneration according to healing done
* Add a "Share Match" button

## Authors

This project is the product of the combined efforts of Ben Fischer and Ronnie Blackburn, for the Riot API coding challenge.  The project was started with no prior Unity or Google App Engine experience, and is the product of two weeks of development time.
