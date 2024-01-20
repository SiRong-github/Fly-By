

**The University of Melbourne**
# COMP30019 – Graphics and Interaction

## Teamwork plan/summary

<!-- [[StartTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

<!-- Fill this section by Milestone 1 (see specification for details) -->
* kevin - initial models, user interface, fog shader, day-night transition, questionnaire evaluation
* pratika - infinite terrain, player movement
* william - obstacles, collision detection
* shanaia - music, particle systems, water shader, observational evaluation
<!-- [[EndTeamworkPlan]] PLEASE LEAVE THIS LINE UNTOUCHED -->

## Final report

Read the specification for details on what needs to be covered in this report... 

Remember that _"this document"_ should be `well written` and formatted **appropriately**. 
Below are examples of markdown features available on GitHub that might be useful in your report. 
For more details you can find a guide [here](https://docs.github.com/en/github/writing-on-github).

### Table of contents
* [Game Summary](#game-summary)
* [Game Play](#game-play)
* [Game Design](#game-design)
* [Game Feedback](#game-feedback)
* [Game Changes](#game-changes)
* [Technologies](#technologies)
* [Using Images](#using-images)
* [Code Snipets](#code-snippets)
* [References](#references)

### Game Summary 
_Fly-by_ is an endless runner in which you travel through the sky as a plane or a hot air balloon. You will encounter different obstacles, power-ups and weather conditions as you move forwards. The obstacles and weather conditions are designed to make your journey difficult. For example, the fog and night modes will make it difficult for you to see what is ahead of you. Conversely, the power-ups will help you along your journey. Depending on the power-up you obtain you may either be granted a period of invincibility or both invincibility and a massive speed boost! The aim of the game is to fly as far as possible without crashing into an obstacle. If you crash into an obstacle, your jouney will come to an end!


### Game Play
* How to play the game (supplementary material in addition to in-game instructions).
#### The game uses standard keyboard controls: 
* The left key should be pressed to move your player left 
* The right key should be pressed to move your player right 
##### Power-ups
* The red ball provides a speed boost and invincibility<br/>
![Red ball](Assets/Screen%20Shot%202022-10-30%20at%2012.50.00%20pm%20copy.png)<br/>
* The green ball provides a speed boost<br/>
![Green ball](Assets/Screen%20Shot%202022-10-30%20at%2012.49.51%20pm%20copy.png)<br/>

### Game Design
#### Gameplay
##### Rationale - ideas that were not included in the final game
Initially, we were planning to include a power up that allows players to shoot obstacles using the space key. We had first used particle systems that emit one particle at a time, since it is less expensive in terms of performance, especially when one has to create a higher number of ammo. However, its collision implementation is much more complex. Thus, we decided to use game objects instead, which should not have affected the computational process much due to having to just instantiate a maximum of 10 bullets for each power up spawn and then destroying them after collision or a few seconds. Nonetheless, we found that it would have made our game code more complicated, since the hitboxes of the obstacles are slightly different. For instance, the hot air balloon obstacles would have a hitbox that is located higher than those of the cars and the buses. We had thought of two main possible solutions: 1) adjusting the height of the projectile with the up and down keys, or 2) only being able to shoot cars and buses when facing forward. There are a few problems. For the first solution, it would have made the game more challenging for players who have less experience in gaming or do not play this type of game genre, since they would have to focus on not only the obstacles, but also the height of the projectile and shooting it. As for the second solution, it would not have much of a difference, because the tracker would still chase after them and the only way to dodge is to move left and right. Of course, we had thought of other solutions which could make this feature work, but our time was limited so we had to focus on other more important aspects of the game. 

#### Graphical Assets
###### Model prototypes & current models
The initial design of the game had a blocky and simplistic style, so this was kept in mind when designing the first prototypes of the models. The first batch of models included a plane, a bird obstacle, and clouds, but later on expanded to also include trees and mountains. 
Although the models were in line with our initial requirements, problems arose such as finding textures for the models, and also difficulty in adding new features on top of the simplistic style the game was designed to be. It became difficult to expand on the game, as when new features were implemented, a new model was required, which is why we ultimately decided to use models from the Unity Asset Store. The models used include planes, air balloons, cars, trucks and buildings. These new models also fit well with our procedural generation which is less blocky and more smooth.

#### Graphics Pipeline

###### Fog Shader
The fog shader should make it so that objects which are further away from the camera become more obscured, relative to objects which are closer to the camera. There are also three different types of fog; linear fog, which is just the further away the more foggy the object is, exponential fog, which is the foggy effect is increased exponentially the further away the object is, and exponential squared fog, which removes fog that is closer to the camera, creating a more realistic effect. 
By using these facts, the fog shader is created using Unity's built-in camera depth texture, which represents the distance of each pixel from the camera. The texture is then linearised, and multiplied by the farClipPlane distance, to find the view distance to each object. This view distance can now be used to calculate the exponential squared which the project is using, by multiplying it by a fog colour.

###### Water Shader
The water shader located in Assets/Prefabs/Terrain/WaterShader.shader was developed to make the effects of the water environment more realistic. With regards to the environment, our idea was initially to make it a beach, so we utilised the wave effect. However, this looked unnatural and flat. With the suggestion of a tutor, we decided to research scrolling textures and found that it has improved the appearance, especially since the plane is flying from above. Nonetheless, we still included options for a wave effect, which could be disregarded if the Amplitude, Distance, Amount, and Speed are set to 0.

The vertex shader was modified to set the wave (if used) and scrolling effects, while the fragment/pixel shader was modified to include a tint colour, which is used for increasing and decreasing the brightness of the given texture, and transparency. 

Please note that the game environments are selected at random. To check the water environment, head​ to the RoadSpawner script and set the index to 1.


#### Procedural Generation Technique

##### Gameplay
* High level gameplay related design decisions you made.

###### Approach 1: A Perlin noise map
Initially, we sought to generate an endless terrain by the following approach:
* Generate a Perlin noise mesh of size 1000 X 1000 when the player moves a certain distance forward
* Apply materials to this mesh
* Decorate this mesh with houses, trees, etc.
<br />
After we completed step 1, and could generate noise maps of size 1000 X 1000 as the player moved foward, it became evident that this approach was unsuitable. This is because the noise maps were dynamically created in an update function, and the algorithm to create a noise map is _O(m * n)_ where _m_ is the number of vertices along the x axis, and _n_ is the number of vertices along the z axis. The significant time complexity of this approach caused our game to lag.
<br />

###### Approach 2: Creating a flat endless terrain, then decorating terrain tiles
![Terrain prefab](Assets/infinite%20terrain.PNG)<br/>

###### Part 1: Creating a flat endless terrain
To create a flat endless terrain, we created a terrain prefab measuring 1000 X 48 and applied a terrain layer to it. As seen above, this terrain prefab consisted of a terrain tile of size 1000 X 48 and a `SpawnTrigger` component.<br/>
This spawn trigger component was an invisible, tall prism and had a mesh filter, box collider, and a rigid body component. Thus, collisions between the plane (player) and this `SpawnTrigger` could be detected. This was core in generating an endless terrain, which is as follows:
<br/>
* In our start method, we generated 25 terrain tiles of size 1000 X 48. They were placed one after the other along the z axis, this created the illusion of infinite land ahead. The player was placed in the middle of the second terrain tile.
* As the player flew across the terrain described above, they would eventually reach the SpawnTrigger component of the terrain tile they were currently flying over. The effect of this is always the same – when the player collides with the SpawnTrigger component, we will drop the tile that is behind the player (currently out of the camera view) and instantiate a new terrain tile and at the very end of the ‘infinite landscape’. Hence, as the player moves forwards, they can never reach the end of the terrain since terrain tiles will always be generated ahead.

###### Part 2: Decorating this endless terrain
To decorate our terrain, we: <br/>
* Created many terrain prefabs with the same terrain layer applied to it. The same terrain layer was applied so that all terrains seemed to be part of the same landscape.
* We then used Unity’s terrain toolbox to add mountains and trees to these terrains.
* Then, instead of spawning flat tiles as part of our terrain, we spawned the decorated tiles.
* The problem with this approach was that: it did not suit our game – we were aiming for a cartoony game, but the terrain generated looked quite realistic; it was obvious that the terrain tiles were repeated

##### Approach 3: Creating a procedurally decorated endless terrain
Our final approach was quite similar to part 1 of our second approach, with a few additional steps. A description of how we created the desert terrain is provided below: <br/>
* We created a flat terrain prefab – it consisted of: a flat terrain tile with some terrain layer, and a SpawnTrigger component.
* In our start method, we once again generated 25 terrain tiles of size 1000 X 48. They were placed one after the other along the z axis, this created the illusion of infinite land ahead. However, before placing each of these tiles in the correct position they were decorated.
* For each tile, we generated one Perlin noise map of size 150 X 48.
* For each adjacent terrain, their Perlin noise maps were adjacent. This means that we cannot have difference in heights between noise maps of two adjacent terrains as this will be obvious.
* So, the Perlin noise map of a terrain was calculated based on the actual location where we would place it in the tile. This ensures smoothness in height difference between noise maps on two adjacent terrains.
* Then, we applied a material to the Perlin noise map.
* Then we duplicated the Perlin noise map and placed two copies on the left hand side of the terrain and two copies on the right hand side of the terrain. There was appropriate spacing kept between the two noise maps on the same side. This was done to create the illusion of endless noise in the x direction as well.
* We chose to copy noise maps, rather than create new noise maps, because as described above, the latter is computationally expensive.
* We then placed trees on the terrain. For trees that could easily be seen, they were placed at a correct height above ground level through some computation involving Perlin noise.
* Trees at a distance were placed at ground level. It is not necessary to place them correctly above ground level since they cannot be seen by the game’s user. 

#### Particle System
Particle Systems used in the game are FuelDump with sub-emitter FuelDumpBurst and PlaneExplosion with sub-emitter PlaneExplosionSpark. The particle system we would like to be marked on is the PlaneExplosion with sub-emitter PlaneExplosionSpark located in Assets/Prefabs/Particle Systems/PlaneExplosion.prefab.

A few of the modifications we have made from the default particle system attributes are:

* Increased gravity modifier: To make the particles fall.

* Decreased maximum particles: To put less burden on the system.

* Destroy stop action: To let the game object destroy itself automatically.

* Emission bursts: To have a more impactful explosion. 

* Sphere shape: To have the explosion all around the shape instead of just ​​at an angle as with if Cone were used.

* Sub-emitters: To have a more impactful explosion.

* Cube mesh renderer + Sprites material: Our original plan was to have the game blocky-themed. While we did not follow this idea anymore, we found that using a cube mesh still creates an impactful effect. 

* Random Between: Start Lifetime, Start Speed, Start Size, and Start Colour: We had observed from videos that explosions in real life also exhibit randomness such as in the velocity of the smoke and exploded material and the colour of the smoke which is som​​etimes a mix of red, black and grey depending on the scale of explosion and how much time has passed. Additionally, randomness in the Particle System attributes also exaggerate the effects. 



### Game Feedback 
* Description of the querying and observational methods used, including a description of the participants (how many, demographics), description of the methodology (which techniques did you use, what did you have participants do, how did you record the data), and feedback gathered.
* Use at least one querying technique to evaluate and then improve your game, with a minimum of 5 participants.
* Use at least one observational method to evaluate and then improve your game, with a minimum of 5 different participants.

#### Think Aloud

##### Reason for Choosing
It is very convenient but also insightful at the same time. It also shows us how players think and act while in the zone, which was helpful considering how the game is based on how long you can survive the game.

##### Demographics
* These are based on the questions written in the next section
* 6 participants, 5M 1F
* Play games for an average of 3x a week, depending on circumstances, with 3 playing daily
* Most commonly played game is fps, followed by rpgs, and puzzles/strategy
* All players are able to multi-task (50% and above), though with varying degrees


##### Method in Conducting
First, we invited the participants (one-on-one) to join a Zoom meeting in order to play the game. We explained the objective of the game as well as how the Think Aloud Evaluation method works. Then, we asked them the following questions: 
* Age & Gender
* How often do you play games
* What types of games do you play, if any
* How much of a multi-tasker are you (this question is asked to determine whether the player's performance is affected by having to think aloud)
* Do you mind sharing your screen? May we record you playing (video or voice memo)? Please note that this is only for recording purposes, and we will not send it to anyone else.

Afterwards, we asked the players to download the game and observed them while taking notes of their thoughts and actions in a document. Since all of them are fine with being recorded, we have video recordings for further revision. Before the end of the meeting, we discussed about the changes we plan on making to the game based on the feedback we were given and also what other players have mentioned in the game. Finally, we thanked them for participating and ended the meeting. 

##### Comments 
* Those who play more frequently prefer WASD keys but are adaptable: have options for both
* Music is loud: have an option for volume
* Did not use/notice power ups, some thought these were obstacles: include in Instructions
* Player can be safe in the middle of the road for the most part, which makes it boring for some: have more obstacles in the middle of the road
* Many did not like the tracker (sphere that chases after the player), though one really liked it; some were able to dodge it after a few tries
* Make points seeable in the dark
* Incentives (coins), shoot bullets
* Everyone liked the change in environment, music and overall look and feel of the game
* Some would like it if it were a mobile game

##### Observations
* Half looked at the instructions, while other half did not: place it in the beginning of the game
* Many of the power ups were also spawned in unattainable locations
* Many wanted to play again

#### Questionnaire

##### Reason for Choosing
Due to convenience and time constraints, we decided to use questionnaires. 

##### Demographics
The demographic the questionnaire was conducted to was to young adults, ages 18-21. The surveyred match our target demography, with all respondants saying they play games often, or every day, playing action or fighting games primarily. 
##### Method in Conducting
The method in which the questionnaire was conducted was the questionnaire was given to the player after they had tried out the game. We asked questions about the game:
* Is the game's objective clear
* How are the visuals
* How is the audio
* How are the controls
* Is it too easy
* What is your highest score
These questions allow us to quickly pinpoint which aspects of the game we need to improve more on, by checking if a certain question's feedback is lackluster.
##### Comments
The player usually would play multiple attempts after dying to the tracker obstacle which was unexpected to them. The game's objective was very clear as it related to games they had played previously. The visuals are good, but one person commented on the tracker which killed them, but they were quickly able to work around it and dodge it the next few times of playing. All players commented on the music being too loud, which we have fixed after receiving the feedback. The game is too easy, but that is as intended. The players had achieved varying scores from 3000 to 9000.

### Game Changes
* Documentation of the changes made to your game based on the information collected during the evaluation.
* Included power ups in Instructions
* Lowered volume of music
* Modified obstacles to be able to move in the middle of the road



### Technologies
Project is created with:
* Unity 2022.1.9f1 
* Ipsum version: 2.33
* Ament library version: 999

### Using Images

You can include images/gifs by adding them to a folder in your repo, currently `Gifs/*`:

<p align="center">
  <img src="Gifs/sample.gif" width="300">
</p>

To create a gif from a video you can follow this [link](https://ezgif.com/video-to-gif/ezgif-6-55f4b3b086d4.mov).

### Code Snippets 

You may wish to include code snippets, but be sure to explain them properly, and don't go overboard copying
every line of code in your project!

```c#
public class CameraController : MonoBehaviour
{
    void Start ()
    {
        // Do something....
    }
}
```

### References
* References and external resources that you used.
https://www.youtube.com/watch?v=EFt_lLVDeRo
* Endless runner
https://www.youtube.com/watch?v=RYouWeqZbPc&ab_channel=Quick%27nDirty
* Perlin noise
https://en.wikipedia.org/wiki/Perlin_noise
https://www.youtube.com/watch?v=bG0uEXV6aHQ&ab_channel=Brackeys
https://www.youtube.com/watch?v=XpG3YqUkCTY&t=92s&ab_channel=Lejynn
https://www.youtube.com/watch?v=WP-Bm65Q-1Y&t=5s&ab_channel=SebastianLague

Particle Systems

https://docs.unity3d.com/ScriptReference/ParticleSystem.html

https://learn.unity.com/tutorial/introduction-to-particle-systems-1?uv=2019.1#

https://www.youtube.com/watch?v=hyBbcFCvDR8

https://www.youtube.com/watch?v=FEA1wTMJAR0

https://www.youtube.com/watch?v=BXh6LC1H5S0

https://www.youtube.com/watch?v=xenW67bXTgM

https://learn.unity.com/tutorial/recorded-video-session-controlling-particles-via-script#5c7f8528edbc2a002053b645

https://www.youtube.com/watch?v=Eua7G7Vct0A&t=3s


H2O Shader

https://docs.unity3d.com/Manual/SL-VertexFragmentShaderExamples.html

https://www.youtube.com/watch?v=lWCPFwxZpVg

https://www.youtube.com/watch?v=kfM-yu0iQBk&t=362s

https://learn.unity.com/tutorial/writing-your-first-shader-in-unity#5c7f8528edbc2a002053b575

https://www.youtube.com/watch?v=kfM-yu0iQBk&t=11076s

https://www.youtube.com/watch?v=30aD1gQ0_-M

https://www.youtube.com/watch?v=8rCRsOLiO7k


Graphics & Interaction Workshops

Workshop 7: https://github.com/COMP30019/Workshop-7-Solution

Workshop 9: https://github.com/COMP30019/Workshop-9-Solution.git

Workshop 10: https://github.com/COMP30019/Workshop-10-Solution.git

Workshop 11: https://github.com/COMP30019/Workshop-11-Solution.git


Aeroplane Fuel Dump & Explosion

https://www.youtube.com/watch?v=U7833zX-l5c

https://www.youtube.com/watch?v=s_3nDVNozJs&t=50s

Audio in Unity
https://www.youtube.com/watch?v=J77CMuAwVDY&t=611s


