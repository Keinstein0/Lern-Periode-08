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
        const int HEIGHT = 8;
        const int WIDTH = 13;

        Tile[,] _tileMap = new Tile[WIDTH, HEIGHT];
        Texture2D _texture;

        int[,] _primaryMap;
        
        public Background(Texture2D template)
        {
            _primaryMap = MapGenerator.GenerateSpotMap(WIDTH, HEIGHT, 2.5f, 0.65f, new Random().Next());
            _texture = template;
        }


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
                    _tileMap[x, y].Draw(sb,x*16,y*16);
                }
            }
        }
    }
}
