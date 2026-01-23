using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

public class Player : Sprite
{

    bool _isDamageApplied = false;
    List<Sprite> _collidables;
    Rectangle _window;

    Rectangle _miracleRect;

    Vector2 _lastAction;
   
    DateTime _immunityExpiration = DateTime.Now.AddMilliseconds(2000);
    DateTime _scoreUpdatedLast = DateTime.Now;

    SpriteFont _font;

    public bool IsDead { get => Hearts <= 0; }

    private int _score = 0;
    public int Score { get => _score; }

    Texture2D _heart;
    Texture2D _explode;
    Texture2D _shattered;

    public int Hearts { get; private set; } = 3;

    private const int XSPEED = 4;
    private const int YSPEED = 4;
    private const int MIRACLE = 10;
    private const double HEART_SIZE = 8;
    private const int IMMUNITY_TIME = 1000;

    bool isShattered = false;
    int shatterTimer = 0;

    Sprite _leftShatter;
    Sprite _rightShatter;
    
    public Player(Texture2D texture, Texture2D heartTexture, SpriteFont font, Vector2 position, List<Sprite> collidables, Viewport viewport, Texture2D explode, Texture2D shattered) : base(texture, position, 8f)
	{
        _collidables = collidables;
        int miracle = MIRACLE;

        _window = viewport.Bounds;

        _heart = heartTexture;

        _miracleRect = new Rectangle(Rect.X + miracle, Rect.Y + miracle, Rect.Width - miracle, Rect.Height - miracle);
        _font = font;

        _explode = explode;
        _shattered = shattered;

	}

    public override void Update(GameTime deltaTime)
    {
        _isDamageApplied = false;
        int miracle = MIRACLE;  
        _miracleRect = new Rectangle(Rect.X + miracle, Rect.Y + miracle, Rect.Width - miracle, Rect.Height - miracle);

        //_score = (int)Math.Floor((deltaTime.TotalGameTime.Seconds * Math.Pow(1.03, (double)deltaTime.TotalGameTime.Seconds)));
        if (_scoreUpdatedLast.AddSeconds(1) < DateTime.Now)
        {
            _score +=(int)Math.Pow(1.05, _score);
            _scoreUpdatedLast = DateTime.Now;
        }

        _lastAction.X = 0;
        _lastAction.Y = 0;

        if (Keyboard.GetState().IsKeyDown(Keys.Down) && !isShattered)
        {
            _lastAction.Y += YSPEED;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Up) && !isShattered)
        {
            _lastAction.Y -= YSPEED;
        }
        MoveY(_lastAction.Y);
        CheckCollisions(false);

        if (Keyboard.GetState().IsKeyDown(Keys.Right) && !isShattered)
        {
            _lastAction.X += XSPEED;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Left) && !isShattered)
        {
            _lastAction.X -= XSPEED;
        }
        MoveX(_lastAction.X);
        
        CheckCollisions(true);

        if (shatterTimer <= 50 && isShattered && shatterTimer >= 10)
        {
            _leftShatter.Rotation -= 0.02f;
            _rightShatter.Rotation += 0.02f;

            _leftShatter.MoveX(-1);
            _rightShatter.MoveX(1);
        }

        //_texture = new Texture2D(graphicsDevice, 1, 1);
        //_texture.SetData(new Color[] { Color.White });
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!isShattered)
        {
            if (_isDamageApplied)
            {
                spriteBatch.Draw(Texture, Rect, Color.Red);

                if (DateTime.Now > _immunityExpiration)
                {
                    Hearts--;
                    _immunityExpiration = DateTime.Now.AddMilliseconds(IMMUNITY_TIME);
                }
            }
            else
            {
                spriteBatch.Draw(Texture, Rect, Color.White);
            }

            for (int i = 0; i < Hearts; i++)
            {
                Rectangle sizeHeart = _heart.Bounds;
                Rectangle heartBox = new Rectangle(sizeHeart.Width * i * (int)(HEART_SIZE * 1.2) + 10, 10, (int)(sizeHeart.Width * HEART_SIZE), (int)(sizeHeart.Height * HEART_SIZE));

                spriteBatch.Draw(_heart, heartBox, Color.White);
            }
        }
        else
        {
            shatterTimer--;
            if (shatterTimer > 50)
            {
                Rectangle actual = new Rectangle(Rect.Location, Rect.Size);
                actual.X -= (int)((int)16f*base.scale);
                actual.Y -= (int)((int)16f * base.scale);
                actual.Width += (int)((int)32 * base.scale);
                actual.Height += (int)((int)32 * base.scale);

                

                spriteBatch.Draw(_explode, actual, Color.White);
            }
            if (shatterTimer <= 50)
            {
                _leftShatter.Draw(spriteBatch);
                _rightShatter.Draw(spriteBatch);
            }
        }

        float scale = 1f; // 3x the original size
        spriteBatch.DrawString(
            _font,
            _score.ToString(),
            new Vector2(160, 6),
            Color.White,
            0f,             // Rotation
            Vector2.Zero,   // Origin
            scale,          // Scale factor
            SpriteEffects.None,
            0f
        );
    }

    public void Reset()
    {
        Hearts = 3;
        _score = 0;
        _immunityExpiration = DateTime.Now.AddMilliseconds(2000);
        isShattered = false;
        shatterTimer = 0;
    }


    private void CheckCollisions(bool isX)
    {
        foreach(Sprite collidable in _collidables)
        {
            if (collidable.Rect.Intersects(_miracleRect))
            {
                _isDamageApplied = true;
            }
        }

        if (!_window.Contains(base.Rect))
        {
            NoMove(isX);
        }
    }

    private void NoMove(bool isX)
    {
        if (isX)
        {
            MoveX(-_lastAction.X);
        }
        else
        {
            MoveY(-_lastAction.Y);
        }
    }

    public void UpdateCollisionRef(List<Sprite> collidables)
    {
        _collidables = collidables;
    }


    public void Shatter()
    {
        isShattered = true;
        shatterTimer = 100;


        Vector2 location = new Vector2(base.Rect.Location.X, base.Rect.Location.Y);

        _leftShatter = new(_shattered, location, 8);
        _rightShatter = new(_shattered, location, 8);

        Rectangle srcLeft = new(0, 0, 6, 7);
        Rectangle srcRight = new(6, 0, 6, 7);

        _leftShatter.SourceRect = srcLeft;
        _rightShatter.SourceRect = srcRight;

        _leftShatter.Origin = new Vector2(3, 3.5f);
        _rightShatter.Origin = new Vector2(3, 3.5f);
    }
}
