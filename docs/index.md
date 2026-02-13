---
title: My Tutorial
---
## MonoGame tutorial
In this tutorial we will create a simple pong game using the MonoGame framework. 

### Introduction
#### What is MonoGame?
Monogame is a game engine that allows you to create games in the C# programming language. In comparison to other frameworks for game development like Unity, MonoGame does not offer a scene editor or other external tools. All configuration, implementing and other actions (With the exception of uploading images) are done 100% in code. It also does not force you to implement your project using a certain structure (like sprites) and you can specialize your code structure to *your* game. 

#### Before we start
For this tutorial we'll assume that you are familliar with programming in C# and know the basics of OOP (object oriented programming). We'll also assume that you Visual Studio 2022 (or any other IDE) set up and have installed the MonoGame framework. For the installation of Monogame you can find a guide here (https://docs.monogame.net/articles/getting_started/2_choosing_your_ide_visual_studio.html)

#### The core concept behind monogame
As previously stated MonoGame uses a code first approach while also giving you a lot of freedom to customize the structure of your game. That's why we need to understand some concepts monogame introduces:

Game1.cs: The main file of your game, this is where you write your code
GameTime: The time since the game has started. Useful if you want all players to move at the same speed, regardless of how many frames they get
spriteBatch: The "renderer" of MonoGame. In the draw function you tell it to begin and end and it will draw everything you specified
Sprite: The "standart" way of structuring things in a game. It's an image with a position attached to tell it where to be rendered. But nobody forces you to use it in monogame.

### Tutorial
#### Game1.cs
Start off with creating a new empty monogame project. Now you should see a class Came1.cs that contains a constructor and 4 functions. I'll explain what each of them are for and how to use them.

Constructor: Mainly for settings, can be ignored for a small desktop pong
Initialize(): Used for settings of the graphics (such as window size)
LoadContent(): Used to load images into memory to paint them onto the window later
Update(): For doing the game logic (for example calculating player position, applying gravity etc.)
Draw(): For painting textures

For now we can write all the code into these functions. Later we'll make specific classes to have the code be a bit cleaner

#### Drawing
For our first step we'll try to load a simple image. In our case a bat for the pong later. In order to do that we'll first need to find an image. For this you can google a nice image for your bat and download it. Then **you can't just paste the image into a folder**. You'll need to use a tool provided by monogame. You'll find it in the Contend/Content.mgcb folder where you can run it by double clicking it.
![image of a folder structure with Content.mgcb](image.png)
You should then see the editor where you can upload your assets.
![MGCB editor](image-1.png)
Using the "Add Existing Item" button (box with green plus) you can then select your asset and upload it into monogame. It then shows a popup that tells you how to upload it. You upload by using the setting "copy the file to the directory". After your assets have uploaded you can then build/rebuild your images. This way monogame knows what files you have and how to load them. 

Next up you need to tell MonoGame to load your asset so that you can use it later. By creating a Texture2D field and in loading the image into it in the LoadContend function with the following line you can then use it in the Draw function.
```paddle = Content.Load<Texture2D>("paddle"); //filename without ending```
Now in order to actually draw this texture you need to adapt your draw function to look like this:
```
protected override void Draw(GameTime gameTime)
{
    GraphicsDevice.Clear(Color.CornflowerBlue);


    _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

    double scale = 0.5;
    int ypos = 50;
    // a rectangle where the paddle is
    Rectangle box = new Rectangle(750, ypos, (int)(paddle.Width * scale), (int)(paddle.Height * scale));

    // draw the paddle where the box is and warp the color towards white
    _spriteBatch.Draw(paddle, box, Color.White);

    _spriteBatch.End(); //tell monogame that the frame is finished and can now be displayed

    base.Draw(gameTime);
}
```
If you now run the file you should now see your paddle displayed.














