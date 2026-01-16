using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonogameProject
{
    public class Tile
    {
        private int _type;
        private int _variation;
        private Texture2D _textureMap;
        private Rectangle _sourceRect;

        const int SIZE = 4;
        
        public Tile(int type, Texture2D texture)
        {
            _type = type;
            _textureMap = texture;


        }

        public void Initialize()
        {
            switch (_type)
            {
                case 2:
                    _variation = 0;
                    
                    _sourceRect = new Rectangle(
                    (_variation * 16) + 1,
                    16,
                    16,
                    16
                    );
                    break;
                
                
                
                default:
                    Random r = new Random();
                    _variation = Math.Max(0, r.Next(0, 20) - 16);

                    _sourceRect = new Rectangle(
                        (_variation * 16) + 1,
                        0,
                        16,
                        16
                        );
                    break;

            }
        }


        public void Draw(SpriteBatch sb, int x, int y)
        {
            sb.Draw(_textureMap, new Rectangle(x*SIZE, y*SIZE, 16*SIZE, 16*SIZE), _sourceRect, Color.White);
        }
    }
}
