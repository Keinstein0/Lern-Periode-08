using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FirstMonogameProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;


        List<Sprite> _sprites = new();
        List<Sprite> _enemies = new();
        Player _player;


        private SpriteFont _font;
        private Background _background;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Viewport viewport = _graphics.GraphicsDevice.Viewport;

            Texture2D player_texture = Content.Load<Texture2D>("tinycoffee");
            Texture2D heart_texture = Content.Load<Texture2D>("heart");
            Texture2D background_texture = Content.Load<Texture2D>("background");

            _background = new Background(background_texture);
            _background.Initialize();

            Texture2D[] enemy_textures = new Texture2D[3]
            {
                Content.Load<Texture2D>("saws/saw0"),
                Content.Load<Texture2D>("saws/saw1"),
                Content.Load<Texture2D>("saws/saw2")
            };

            int enemyCount = 5;

            _font = Content.Load<SpriteFont>("Fonts/astro");
            _player = new Player(player_texture, heart_texture,_font, new Vector2(600, 400), _enemies, viewport);

            for (int i = 0; i < enemyCount; i++)
            {
                Enemy enemy = new Enemy(enemy_textures, new Vector2(i*100,100), viewport, _enemies);

                _sprites.Add(enemy);
                _enemies.Add(enemy);
            }
            _sprites.Add(_player);


            

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            foreach (var sprite in _sprites)
            {
                sprite.Update(gameTime);
            }


            if (_player.IsDead)
            {
                base.Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);



            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _background.Draw(_spriteBatch);

            foreach (var sprite in _sprites)
            {
                sprite.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
