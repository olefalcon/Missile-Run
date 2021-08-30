# Missile-Run
Repo for unity build missile run game
![MainMenu](https://github.com/olefalcon/Missile-Run/blob/main/image_2021-08-26_193109.png)
## How To Install
1. Navigate to releases (https://github.com/olefalcon/Missile-Run/releases).
2. Select the most recent release.
3. Download the zip file named Missile Run v.(release version).
4. Unzip the file into a folder and run the executable.
## How To Play
#### Player Controls
Each player controls 1 player figure. From the main camera, they look like circles. Use **W, A, S, and D** to move your character.
#### Missile
After a short delay from when players start moving, the missile will begin to chase the closest player. If another player gets closer the missile will stop chasing its current target and begin chasing the new, closer, target. You can confuse the missile by running it into pillars, the white squares scattered around the map. When the missile hits a square it bounces off and loses its tracking capabilities for a few seconds. Use this time to relocate yourself farther from the missile to hopefully avoid being the target.
#### Powerups
Powerups are the colored squares scattered in place. Players can run into them to activate one of the powerup effects.
##### Speed Powerup
<img src="https://github.com/olefalcon/Missile-Run/blob/main/Assets/Images/SpeedSprite.png" alt="speed" width="200"/>
The speed powerup gives the player bonus speed for a few seconds. Useful to get away from the missile.
##### Ghost Powerup
<img src="https://github.com/olefalcon/Missile-Run/blob/main/Assets/Images/InvisSprite.png" alt="ghost" width="200"/>
The ghost powerup makes you unable to be tracked by the missile for a few seconds. Useful to lose target status.
##### Glitch Powerup
<img src="https://github.com/olefalcon/Missile-Run/blob/main/Assets/Images/GlitchSprite.png" alt="glitch" width="200"/>
The glitch powerup marks a the spot you picked up the powerup at. After the glitch delay, you will be teleported back to that spot. Use the time during the glitch delay to run the missile towards other players, as you will be teleported back when it is over.
##### Teleport Powerup
<img src="https://github.com/olefalcon/Missile-Run/blob/main/Assets/Images/SwapSprite.png" alt="swap" width="200"/>
The teleport powerup swaps you and the farthest player's positions. Use this to cheese another player who is keeping on the far perimeter of the map.
##### Shield Powerup
<img src="https://github.com/olefalcon/Missile-Run/blob/main/Assets/Images/ShieldSprite.png" alt="shield" width="200"/>
The shield powerup grants the player a protective shield for a short time. While the shield is active the player will act like a pillar, causing confusion to the missile if it hits them.
## Overview
Missile Run is my first attempt to finish a project. I want to complete a game so I can experience all aspects of game design, from the beginning steps to the finishing touches. Missil Run is a simple 4 player online multiplayer games that pits players against each other. After an initial countdown, a missile in the center of the map will start chasing the closest player. Players must make use of powerups scattered across the play area, as well as pillars that block the missile to survive and be the last player alive.
