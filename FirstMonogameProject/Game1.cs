using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        bool _isOnTitleScreen = true;
        Title _title;

        private SpriteFont _font;
        private Background _background;

        const int ENEMY_COUNT = 10;
        Viewport _viewport;

        private DateTime _gameStartTime;
        private DateTime _lastEnemyReleasedAt;

        int exitCooldown = 0;
        double _yvel = 0;
        double _ypos = 0;

        Texture2D _water;


        // Assets
        Texture2D[] _enemy_textures;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1024;
            _graphics.PreferredBackBufferHeight = 512;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _viewport = _graphics.GraphicsDevice.Viewport;

            Texture2D player_texture = Content.Load<Texture2D>("tinycoffee");
            Texture2D heart_texture = Content.Load<Texture2D>("heart");
            Texture2D background_texture = Content.Load<Texture2D>("background");

            Texture2D explode = Content.Load<Texture2D>("tinycoffee_explode");
            Texture2D shattered = Content.Load<Texture2D>("tinycoffee_split");

            _water = Content.Load<Texture2D>("water");

            _background = new Background(background_texture, _water);
            _background.Initialize();

            _enemy_textures = new Texture2D[3]
            {
                Content.Load<Texture2D>("saws/saw0"),
                Content.Load<Texture2D>("saws/saw1"),
                Content.Load<Texture2D>("saws/saw2")
            };

            _font = Content.Load<SpriteFont>("Fonts/astro");
            _player = new Player(player_texture, heart_texture,_font, new Vector2(600, 400), _enemies, _viewport, explode, shattered);


            Texture2D inactive = Content.Load<Texture2D>("Buttons/buttonnormal");
            Texture2D active = Content.Load<Texture2D>("Buttons/buttonactive");
            int scale = 2;

            Rectangle startRect = new Rectangle(_graphics.PreferredBackBufferWidth / 2 - inactive.Width * scale, 100, inactive.Width * scale*2, inactive.Height * scale);

            Button start = new Button(startRect, inactive, active, "Start Game", _font);
            _title = new Title(start, _font);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (!_isOnTitleScreen)
            {
                foreach (var sprite in _sprites)
                {
                    sprite.Update(gameTime);
                }


                if (_player.IsDead && exitCooldown == 0)
                {
                    _player.Shatter();
                    _title.UpdateData(_player.Score);
                    exitCooldown = 150;
                }

                if (exitCooldown > 0)
                {
                    exitCooldown--;

                    if (exitCooldown < 100)
                    {
                        // gravity
                        _yvel += 0.20;
                        _ypos -= _yvel;
                        _ypos = Math.Max(_ypos, -(_viewport.Height / 4));

                        foreach (Enemy e in _enemies)
                        {
                            e.ApplyOffset((int)_ypos * 8);
                        }

                        _background.ApplyOffset((int)_ypos);
                    }




                    if (exitCooldown == 0)
                    {
                        _isOnTitleScreen = true;
                    }
                }
















            }
            else
            {
                _title.Update(gameTime, ref _isOnTitleScreen);
                if (!_isOnTitleScreen)
                {
                    _player.Reset();
                    _title.Reset();
                    ResetGame();
                    _gameStartTime = DateTime.Now;
                    _lastEnemyReleasedAt = DateTime.Now;

                    _yvel = 0;
                    _ypos = 0;

                    _background.ApplyOffset((int)_ypos);

                }
            }

            if (_enemies.Count < ENEMY_COUNT && _lastEnemyReleasedAt.AddSeconds(1) < DateTime.Now)
            {
                _lastEnemyReleasedAt = DateTime.Now;
                AddEnemy();
            }



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);



            // TODO: Add your drawing code here
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _background.Draw(_spriteBatch, gameTime);


            if (!_isOnTitleScreen)
            {
                foreach (var sprite in _sprites)
                {
                    sprite.Draw(_spriteBatch);
                }
            }
            else
            {
                _title.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void ResetGame()
        {

            _sprites = new List<Sprite>();
            _enemies = new List<Sprite>();

            _sprites.Add(_player);
            _player.UpdateCollisionRef(_enemies);
        }

        private void AddEnemy()
        {
            Random r = new();

            Vector2 position = new(r.Next(_viewport.Width - _enemy_textures[0].Width * 8), r.Next(_viewport.Height) - _enemy_textures[0].Width * 8);
            Vector2 positionv2;
            Vector2 velocity;


            if (_enemies.Count % 2 == 0)
            {
                positionv2 = new Vector2(0, -30);
                velocity = new Vector2(2, 3);
            }
            else
            {
                positionv2 = new Vector2(_viewport.Width, -30);
                //positionv2 = new Vector2(100, 100);
                velocity = new Vector2(-2, 3);
            }

            Enemy enemy = new Enemy(_enemy_textures, positionv2, _viewport, _enemies);
            enemy.SetVelocity(velocity, 20);




            _sprites.Add(enemy);
            _enemies.Add(enemy);
        }
    }
}
