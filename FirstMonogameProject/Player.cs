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
   
    DateTime _immunityExpiration = DateTime.Now;

    public bool IsDead { get => Hearts <= 0; }


    Texture2D _heart;

    public int Hearts { get; private set; } = 3;

    private const int XSPEED = 4;
    private const int YSPEED = 4;
    private const int MIRACLE = 10;
    private const double HEART_SIZE = 6;
    private const int IMMUNITY_TIME = 1000;
    
    public Player(Texture2D texture, Texture2D heartTexture, Vector2 position, List<Sprite> collidables, Viewport viewport) : base(texture, position, 9f)
	{
        _collidables = collidables;
        int miracle = MIRACLE;

        _window = viewport.Bounds;

        _heart = heartTexture;

        _miracleRect = new Rectangle(Rect.X + miracle, Rect.Y + miracle, Rect.Width - miracle, Rect.Height - miracle);
	}

    public override void Update(GameTime deltaTime)
    {
        _isDamageApplied = false;
        int miracle = MIRACLE;  
        _miracleRect = new Rectangle(Rect.X + miracle, Rect.Y + miracle, Rect.Width - miracle, Rect.Height - miracle);


        int differenceX = 0;
        int differenceY = 0;

        _lastAction.X = 0;
        _lastAction.Y = 0;

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _lastAction.Y += YSPEED;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _lastAction.Y -= YSPEED;
        }
        MoveY(_lastAction.Y);
        CheckCollisions(false);

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _lastAction.X += XSPEED;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _lastAction.X -= XSPEED;
        }
        MoveX(_lastAction.X);
        
        CheckCollisions(true);

        //_texture = new Texture2D(graphicsDevice, 1, 1);
        //_texture.SetData(new Color[] { Color.White });
    }

    public override void Draw(SpriteBatch spriteBatch)
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

        for (int i = 0; i<Hearts; i++)
        {
            Rectangle sizeHeart = _heart.Bounds;
            Rectangle heartBox = new Rectangle(sizeHeart.Width * i * (int)(HEART_SIZE* 1.3), 0, (int)(sizeHeart.Width*HEART_SIZE), (int)(sizeHeart.Height*HEART_SIZE));

            spriteBatch.Draw(_heart, heartBox, Color.White);
        }
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
}
