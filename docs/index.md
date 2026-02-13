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
Draw(): The function where you paint. For cleanness i wouldn't put any calculations in here
Update(): Here is where you actually insert your logic (e.g if player click space -> jump)
GameTime: The time since the game has started. Useful if you want all players to move at the same speed, regardless of how many frames they get
spriteBatch: The "renderer" of MonoGame. In the draw function you tell it to begin and end and it will draw everything you specified
Sprite: The "standart" way of structuring things in a game. It's an image with a position attached to tell it where to be rendered. But nobody forces you to use it in monogame.

### Tutorial
#### Game1.cs
To start off with 
























