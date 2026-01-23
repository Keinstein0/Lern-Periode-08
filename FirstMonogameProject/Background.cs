using Microsoft.Xna.Framework;
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
        Texture2D _water;
        int _waterOffset = -500;
        int _waterOffsetY = 0;


        Random r = new Random();
        
        public Background(Texture2D template, Texture2D water)
        {
            _primaryMap = MapGenerator.GenerateSpotMap(WIDTH, HEIGHT, 2.5f, 0.65f, new Random().Next());
            _texture = template;
            _water = water;
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

        public void Draw(SpriteBatch sb, GameTime gameTime)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    _tileMap[x, y].Draw(sb,x*16,y*16 + _yoffset);
                }
            }
            Rectangle destination = new(_waterOffset, 370 + (int)(_yoffset*4.5f) + _waterOffsetY, _water.Width * 7, _water.Height * 7);
            sb.Draw(_water, destination, Color.White);

            // 1. Define these variables at the top of your class
            float amplitude = 50; // How many pixels up/down it moves
            float speed = 2.4f;      // How fast it hovers

            float amplitudeY = 15; // How many pixels up/down it moves
            float speedY = 2.4f;      // How fast it hovers

            // 2. In your Update method
            double time = gameTime.TotalGameTime.TotalSeconds;

            // Math.Sin returns a value between -1.0 and 1.0
            // We multiply by amplitude to get the pixel offset
            _waterOffset = (int)(Math.Sin(time * speed) * amplitude) - 600;
            _waterOffsetY = (int)(Math.Cos(time * speedY) * amplitudeY);


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
