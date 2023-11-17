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
        public List<Vector2> SelectedPositions { get; private set; }

        Color[] colors;

        public FieldController(int width, int height, Vector2 position)
        {

            Width = width;
            Height = height;
            Position = position;

            GemGrid = new Gem[width, height];
            SelectedPositions = new List<Vector2>();

            GenerateColors();
            GenerateField();
            Position = position;
        }

        private void GenerateColors()
        {
            colors = new Color[5];

            colors[0] = Color.Red;
            colors[1] = Color.Blue;
            colors[2] = Color.Green;
            colors[3] = Color.Orange;
            colors[4] = Color.Purple;
        }

        public void SwapGems(int[] firstGemPos, int[] secondGemPos)
        {
            int x1 = firstGemPos[0];
            int y1 = firstGemPos[1];
            int x2 = secondGemPos[0];
            int y2 = secondGemPos[1];

            if (Math.Abs(x1 - x2) == 1 && y1 == y2 || x1 == x2 && Math.Abs(y1 - y2) == 1)
            (GemGrid[x1, y1], GemGrid[x2, y2]) 
                = (GemGrid[x2, y2], GemGrid[x1, y1]);
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
            int[] selectedGemPos = new int[2] { ((int)position.X - (int)Position.X) / 55,
                ((int)position.Y - (int)Position.Y) / 55 };
            Gem selectedGem = GemGrid[selectedGemPos[0], selectedGemPos[1]];

            //GemGrid[((int)position.X  - (int)Position.X) / 55, ((int)position.Y - (int)Position.Y) / 55].CurrentState = Gem.State.Selected;
            //GemGrid[((int)position.X - (int)Position.X) / 55, ((int)position.Y - (int)Position.Y) / 55].TextureName = "rect_white_border";
            if (SelectedPositions.Count < 2)
            {
                selectedGem.ChangeState();
                SelectedPositions.Add(new Vector2(selectedGemPos[0], selectedGemPos[1]));
            }
            if (SelectedPositions.Count == 2)
            {
                int x1 = (int)SelectedPositions[0].X;
                int y1 = (int)SelectedPositions[0].Y;
                int x2 = (int)SelectedPositions[1].X;
                int y2 = (int)SelectedPositions[1].Y;

                GemGrid[x1, y1].ChangeState();
                GemGrid[x2, y2].ChangeState();

                SwapGems(new int[] { x1, y1 },
                    new int[] { x2, y2 });

                SelectedPositions.Clear();
            }
            //selectedGem.ChangeState();
        }

        public void UnselectGem()
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
