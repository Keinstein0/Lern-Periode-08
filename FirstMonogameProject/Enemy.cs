using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

public class Enemy : Sprite
{
    private TimeSpan lastTextureUpdate;

    private Texture2D[] _textures;
    private int _activeIndex = 0;

    private Vector2 _velocity = new Vector2(0,0);

    private Rectangle _window;

    private List<Sprite> _collidables;

    private Random _globalrandom = new Random();

    private int _collisionImmunity = 0;

    private int _yOffset = 0;

    public Enemy(Texture2D[] texture, Vector2 position, Viewport window, List<Sprite> collidables) : base(texture[0], position)
	{
        _textures = texture;
        _window = window.Bounds;
        _collidables = collidables;
	}


    public override void Update(Microsoft.Xna.Framework.GameTime deltaTime)
    {
        TimeSpan gameDuration = deltaTime.TotalGameTime;
        if ((gameDuration - lastTextureUpdate).TotalMilliseconds > 70)
        {
            _activeIndex++;
            if (_activeIndex >= 3)
            {
                _activeIndex = 0;
            }
            lastTextureUpdate = gameDuration;
            base.Texture = _textures[_activeIndex];
        }
        MoveX(_velocity.X);
        CheckBoxes(true);
        MoveY(_velocity.Y);
        CheckBoxes(false);
        
        if (_collisionImmunity > 0)
        {
            _collisionImmunity--;
        }

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle actual = new Rectangle(Rect.Location, Rect.Size);
        actual.Y += _yOffset;
        
        spriteBatch.Draw(Texture, actual, Color.White);
    }


    private void CheckBoxes(bool isX)
    {
        if (!_window.Contains(base.Rect) && _collisionImmunity == 0)
        {
            Bounce(isX);
        }


        foreach (Sprite collidable in _collidables)
        {
            if (collidable.Rect.Intersects(Rect) && collidable != this && _collisionImmunity == 0)
            {
                Bounce(isX);
            }
        }
    }


    private void Bounce(bool isX)
    {
        if (isX)
        {
            MoveX(-_velocity.X);
            _velocity.X *= -1;
            _velocity.Y = Math.Clamp(_velocity.Y + _globalrandom.Next(-1, 1), -5, 5);
        }
        else
        {
            MoveY(-_velocity.Y);
            _velocity.Y *= -1;
            _velocity.X = Math.Clamp(_velocity.X + _globalrandom.Next(-1, 1), -5, 5);
        }
    }


    public void SetVelocity(Vector2 vel, int collisionImmunity = 0)
    {
        _velocity = vel;
        _collisionImmunity = collisionImmunity;
    }

    public void ApplyOffset(int offset)
    {
        _yOffset = offset;
    }

}
