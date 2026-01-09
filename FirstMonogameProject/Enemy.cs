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

    public List<Sprite> Collidables { get;  set; }
   
    public Enemy(Texture2D[] texture, Vector2 position, Viewport window) : base(texture[0], position)
	{
        _textures = texture;
        RandomizeVelocity();
        _window = window.Bounds;
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
        
        

    }


    private void CheckBoxes(bool isX)
    {
        Random r = new Random();

        if (!_window.Contains(base.Rect))
        {
            if (isX)
            {
                MoveX(-_velocity.X);
                _velocity.X *= -1;
                _velocity.Y = Math.Clamp(_velocity.Y + r.Next(-1, 1), -5, 5);
            }
            else
            {
                MoveY(-_velocity.Y);
                _velocity.Y *= -1;
                _velocity.X = Math.Clamp(_velocity.X + r.Next(-1, 1), -5, 5);
            }
        }
    }


    private void RandomizeVelocity()
    {
        Random r = new Random();
        
        _velocity.X = r.Next(-5, 5);
        _velocity.Y = r.Next(-5, 5);
    }

}
