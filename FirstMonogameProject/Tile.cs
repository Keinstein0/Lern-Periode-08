using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonogameProject
{
    public class Tile
    {
        private int _type;
        public int Type { get => _type; }
        private int _variation;
        private Texture2D _textureMap;
        private Rectangle _sourceRect;

        bool[] neigbours = new bool[8];

        bool _n;
        bool _no;
        bool _o;
        bool _os;
        bool _s;
        bool _sw;
        bool _w;
        bool _wn;

        const int SIZE = 4;
        
        public Tile(int type, Texture2D texture)
        {
            _type = type;
            _textureMap = texture;
        }

        public void AddNeigbours(
            bool n,
            bool no,
            bool o,
            bool os,
            bool s,
            bool sw,
            bool w,
            bool wn
            )
        {
            _n = n;
            _no = no;
            _o = o;
            _os = os;
            _s = s;
            _sw = sw;
            _w = w;
            _wn = wn;
        }


        public void Initialize()
        {
            switch (_type)
            {
                case 2:
                    _sourceRect = new Rectangle(
                    1,
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
            if (_type == 2)
            {
                if (!_n)
                {
                    Overlay(sb, x, y, 2);
                }
                if (!_o)
                {
                    Overlay(sb, x, y, 3);
                }
                if (!_s)
                {
                    Overlay(sb, x, y, 4);
                }
                if (!_w)
                {
                    Overlay(sb, x, y, 1);
                }
                if (!_no)
                {
                    Overlay(sb, x, y, 5);
                }
                if (!_os)
                {
                    Overlay(sb, x, y, 8);
                }
                if (!_sw)
                {
                    Overlay(sb, x, y, 7);
                }
                if (!_wn)
                {
                    Overlay(sb, x, y, 6);
                }
            }
        }


        public void Overlay(SpriteBatch sb, int x, int y, int slide)
        {
            Rectangle src = new Rectangle(
                    (slide * 16) + 1,
                    16,
                    16,
                    16
                );

            sb.Draw(_textureMap, new Rectangle(x * SIZE, y * SIZE, 16 * SIZE, 16 * SIZE), src, Color.White);
        }
    }
}
