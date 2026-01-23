using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sprite
{
    public Texture2D Texture { get; protected set; }
    public Vector2 Position { get; set; }
    public float scale { get; set; } = 1;

    public float Rotation { get; set; } = 0f;
    public Rectangle? SourceRect { get; set; } = null;
    public Vector2 Origin { get; set; } = Vector2.Zero;

    public Rectangle Rect
    {
        get
        {
            int width = SourceRect.HasValue ? SourceRect.Value.Width : Texture.Width;
            int height = SourceRect.HasValue ? SourceRect.Value.Height : Texture.Height;
            return new Rectangle((int)Position.X, (int)Position.Y, (int)(width * scale), (int)(height * scale));
        }
    }

    public Sprite(Texture2D texture, Vector2 position, float scale = 8)
    {
        this.Texture = texture;
        this.Position = position;
        this.scale = scale;

        this.Origin = Vector2.Zero;
    }

    public void Move(Vector2 movement) => Position += movement;
    public void MoveX(float movement) => Position = new Vector2(Position.X + movement, Position.Y);
    public void MoveY(float movement) => Position = new Vector2(Position.X, Position.Y + movement);

    public virtual void Update(GameTime gameTime) { }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            Position,
            SourceRect,
            Color.White,
            Rotation,
            Origin,
            scale,
            SpriteEffects.None,
            0f
        );
    }
}