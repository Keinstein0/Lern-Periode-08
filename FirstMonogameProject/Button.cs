using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using static System.Net.Mime.MediaTypeNames;

namespace FirstMonogameProject
{
    public class Button
    {
        private Rectangle _place;
        private Texture2D _texture;
        private SpriteFont _font;
        private string _text;

        private Texture2D _active;
        private Texture2D _inactive;

        Vector2 _textSize;

        int yOffsetAnimation = 0;
        bool isDropping = false;
        int yvel = 0;
        
        
        public Button(Rectangle place, Texture2D textureStd, Texture2D textureAct, string text, SpriteFont font)
        {
            _place = place;
            
            _active = textureAct;
            _inactive = textureStd;
            _texture = textureStd;
            _text = text;

            _font = font;

            _textSize = _font.MeasureString(_text);
        }


        public bool Update(GameTime gameTime)
        {

            var state = Mouse.GetState();
            Point mousePos = state.Position;



            bool hovering = _place.Contains(mousePos);

            if (hovering)
            {
                _texture = _active;
                if (state.LeftButton == ButtonState.Pressed)
                {
                    return false;
                }

            }
            else
            {
                _texture = _inactive;
            }

            if (!isDropping)
            {
                // 1. Define these variables at the top of your class
                float amplitude = 15f; // How many pixels up/down it moves
                float speed = 2f;      // How fast it hovers

                // 2. In your Update method
                double time = gameTime.TotalGameTime.TotalSeconds;

                // Math.Sin returns a value between -1.0 and 1.0
                // We multiply by amplitude to get the pixel offset
                yOffsetAnimation = (int)(Math.Sin(time * speed) * amplitude);
            }
            else
            {
                yvel++;
                yOffsetAnimation += yvel;
            }



                return true;
        }


        public void Draw(SpriteBatch sprite)
        {
            Rectangle background = new Rectangle(_place.X, _place.Y + (int)(yOffsetAnimation / 1.3), _place.Width, _place.Height);
           
            
            
            sprite.Draw(_texture, background, Color.White);

            float scale = 1f;


            Vector2 origin = _textSize / 2f;

            Vector2 position = new Vector2(
                _place.X + (_place.Width / 2f),
                _place.Y + (_place.Height / 2f) + yOffsetAnimation
            );
            sprite.DrawString(
                _font,
                _text,
                position,      // Position is now the center of the box
                Color.White,
                0f,
                origin,        // Origin is the center of the text
                scale,
                SpriteEffects.None,
                0f
            );
        }

        public void Drop()
        {
            isDropping = true;
        }

        public void Reset()
        {
            isDropping = false;
            yOffsetAnimation = 0;
            yvel = 0;
        }

    }
}
