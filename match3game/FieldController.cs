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

        public Vector2 Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Gem[,] GemGrid { get; private set; }

        Color[] colors;

        public FieldController(int width, int height, Vector2 position)
        {

            Width = width;
            Height = height;
            Position = position;

            GemGrid = new Gem[width, height];

            GenerateColors();
            GenerateField();
            Position = position;
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
            (GemGrid[firstGemPos[0], firstGemPos[1]], GemGrid[secondGemPos[0], secondGemPos[1]]) 
                = (GemGrid[secondGemPos[0], secondGemPos[1]], GemGrid[firstGemPos[0], firstGemPos[1]]);
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

        public void SelectGem(Vector2 position)
        {
            GemGrid[((int)position.X  - (int)Position.X) / 55, ((int)position.Y - (int)Position.Y) / 55].CurrentState = Gem.State.Selected;
            GemGrid[((int)position.X - (int)Position.X) / 55, ((int)position.Y - (int)Position.Y) / 55].TextureName = "rect_white_border";
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

        public void OnClick(Vector2 position)
        {
            if ((position.X > Position.X && position.X < Position.X + Width * 55) &&
                (position.Y > Position.Y && position.Y < Position.Y + Height * 55))
            {
                SelectGem(position);
            }
        }

    }
}
