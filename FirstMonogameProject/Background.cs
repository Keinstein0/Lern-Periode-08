using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstMonogameProject
{
    public class Background
    {
        const int HEIGHT = 16;
        const int WIDTH = 16;

        Tile[,] _tileMap = new Tile[WIDTH, HEIGHT];
        Texture2D _texture;

        int[,] _primaryMap;
        
        public Background(Texture2D template)
        {
            _primaryMap = MapGenerator.GenerateSpotMap(WIDTH, HEIGHT, 2.5f, 0.65f, new Random().Next());
            _texture = template;
        }

        int _yoffset = 0;


        public void Initialize()
        {
            

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    int clamped = Math.Clamp(_primaryMap[x, y], 1, 2);   
                    _tileMap[x, y] = new Tile(clamped, _texture);
                }
            }

            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    bool n = IsType(x, y+1, _tileMap[x, y].Type);
                    bool no = IsType(x+1, y+1, _tileMap[x, y].Type);
                    bool o = IsType(x+1, y, _tileMap[x, y].Type);
                    bool os = IsType(x+1, y-1, _tileMap[x, y].Type);
                    bool s = IsType(x, y-1, _tileMap[x, y].Type);
                    bool sw = IsType(x-1, y-1, _tileMap[x, y].Type);
                    bool w = IsType(x-1, y, _tileMap[x, y].Type);
                    bool wn = IsType(x-1, y+1, _tileMap[x, y].Type);

                    _tileMap[x, y].AddNeigbours(n, no, o, os, s, sw, w, wn);

                    _tileMap[x, y].Initialize();
                }
            }
        }

        public void Draw(SpriteBatch sb)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    _tileMap[x, y].Draw(sb,x*16,y*16 + _yoffset);
                }
            }
        }


        private bool IsType(int x, int y, int type)
        {
            if (x >= 0 && y >= 0 && x < WIDTH && y < HEIGHT)
            {
                if (_tileMap[x, y].Type == type) return true;
            }
            else
            {
                return false;
            }

            return false;
        }

        public void ApplyOffset(int offset)
        {
            _yoffset = offset;
        }
    }
}
