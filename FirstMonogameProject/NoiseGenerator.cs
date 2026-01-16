using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//// BLIND /////

namespace FirstMonogameProject
{
    using System;
    using System.Collections.Generic;

    public static class MapGenerator
    {
        public static int[,] GenerateSpotMap(int width, int height, float scale, float threshold, int seed)
        {
            int[,] map = new int[width, height];
            FastNoise noise = new FastNoise(seed);

            // 1. Thresholding: Create Background (1) and "Possible Spot" (-1)
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Get noise value between 0 and 1
                    float val = noise.GetPerlin(x / scale, y / scale);
                    map[x, y] = (val > threshold) ? -1 : 1;
                }
            }

            // 2. Identification: Group -1s into unique IDs (2, 3, 4...)
            int currentID = 2;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (map[x, y] == -1)
                    {
                        FloodFill(map, x, y, width, height, currentID);
                        currentID++;
                    }
                }
            }
            return map;
        }

        private static void FloodFill(int[,] map, int startX, int startY, int width, int height, int id)
        {
            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
            stack.Push(new Tuple<int, int>(startX, startY));

            while (stack.Count > 0)
            {
                var p = stack.Pop();
                int x = p.Item1; int y = p.Item2;

                if (x < 0 || x >= width || y < 0 || y >= height || map[x, y] != -1)
                    continue;

                map[x, y] = id;

                stack.Push(new Tuple<int, int>(x + 1, y));
                stack.Push(new Tuple<int, int>(x - 1, y));
                stack.Push(new Tuple<int, int>(x, y + 1));
                stack.Push(new Tuple<int, int>(x, y - 1));
            }
        }
    }

    // Simple Perlin Noise implementation for Pure C#
    public class FastNoise
    {
        private int[] perm;
        public FastNoise(int seed)
        {
            Random r = new Random(seed);
            perm = new int[512];
            for (int i = 0; i < 256; i++) perm[i] = i;
            for (int i = 0; i < 256; i++)
            {
                int j = r.Next(256);
                int tmp = perm[i]; perm[i] = perm[j]; perm[j] = tmp;
                perm[i + 256] = perm[i];
            }
        }

        public float GetPerlin(float x, float y)
        {
            int X = (int)Math.Floor(x) & 255;
            int Y = (int)Math.Floor(y) & 255;
            x -= (float)Math.Floor(x);
            y -= (float)Math.Floor(y);
            float u = Fade(x);
            float v = Fade(y);
            int a = perm[X] + Y, aa = perm[a], ab = perm[a + 1];
            int b = perm[X + 1] + Y, ba = perm[b], bb = perm[b + 1];

            float res = Lerp(v, Lerp(u, Grad(perm[aa], x, y), Grad(perm[ba], x - 1, y)),
                                Lerp(u, Grad(perm[ab], x, y - 1), Grad(perm[bb], x - 1, y - 1)));
            return (res + 1) / 2; // Normalize to 0-1
        }

        private float Fade(float t) => t * t * t * (t * (t * 6 - 15) + 10);
        private float Lerp(float t, float a, float b) => a + t * (b - a);
        private float Grad(int hash, float x, float y)
        {
            int h = hash & 15;
            float grad = 1 + (h & 7);
            if ((h & 8) != 0) grad = -grad;
            return (h < 4) ? grad * x : grad * y;
        }
    }
}
