using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

public class Sprite
{
	public Texture2D Texture { get; protected set; }
	public Vector2 Position { get; set; }

	public Rectangle Rect
	{
		get
		{
			return new Rectangle((int)Position.X, (int)Position.Y, (int)(Texture.Width*scale), (int)(Texture.Height*scale));
		}
	}

	public float scale { get; set; } = 1;

	
	public Sprite(Texture2D texture, Vector2 position, float scale=10)
	{
		this.Texture = texture;
		this.Position = position;
		this.scale = scale;
	}

	public void Move(Vector2 movement)
	{
		Position += movement;
	}

	public void MoveX(float movement)
	{
		Position = new Vector2(Position.X + movement, Position.Y);
	}

    public void MoveY(float movement)
    {
        Position = new Vector2(Position.X, Position.Y + movement);
    }

    public virtual void Update(GameTime deltaTime)
	{

    }

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Texture, Rect, Color.White);
	}

}
