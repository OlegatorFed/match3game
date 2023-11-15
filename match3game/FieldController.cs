using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class FieldController
    {

        public int Width { get; private set; }
        public int Height { get; private set; }
        public Gem[,] GemGrid { get; private set; }

        Color[] colors;

        public FieldController(int width, int height)
        {

            Width = width;
            Height = height;

            GemGrid = new Gem[width, height];

            GenerateColors();
            GenerateField();

        }

        private void GenerateColors()
        {
            colors = new Color[5];

            colors[0] = Color.Maroon;
            colors[1] = Color.Blue;
            colors[2] = Color.DarkGreen;
            colors[3] = Color.Khaki;
            colors[4] = Color.Violet;
        }

        public void SwapGems(int[] firstGemPos, int[] secondGemPos)
        {
            (GemGrid[firstGemPos[0], firstGemPos[1]], GemGrid[secondGemPos[0], secondGemPos[1]]) = (GemGrid[secondGemPos[0], secondGemPos[1]], GemGrid[firstGemPos[0], firstGemPos[1]]);
        }

        public void DestroyGem(int[] position)
        {
            GemGrid[position[0], position[1]] = null;
        }

        public void CreateGem(int[] position)
        {
            Random random = new Random();
            GemGrid[position[0], position[1]] = new Gem(colors[random.Next(colors.Length)]);
        }

        public void DestroyMatch()
        {

        }

        public void GenerateField()
        {

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    CreateGem(new int[] { i, j });

                }
            }


        }

    }
}
