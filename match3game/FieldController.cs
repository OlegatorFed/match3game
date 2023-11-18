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

        private List<Vector2> SelectedPositions;
        private List<int[]> GemsToDestroy;
        private List<int[]> HorizontalLineCandidates;
        private List<int[]> VerticalLineCandidates;
        private List<int[]> BombCandidates;

        Color[] colors;

        public FieldController(int width, int height, Vector2 position)
        {

            Width = width;
            Height = height;
            Position = position;

            GemGrid = new Gem[width, height];
            SelectedPositions = new List<Vector2>();
            GemsToDestroy = new List<int[]>();
            HorizontalLineCandidates = new List<int[]>();
            VerticalLineCandidates = new List<int[]>();
            BombCandidates = new List<int[]>();

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

        public void DestroyMatches()
        {
            foreach (int[] positionToDestroy in GemsToDestroy)
            {
                DestroyGem(positionToDestroy);
            }
        }

        public void SelectGem(Vector2 position)
        {
            int[] selectedGemPos = new int[2] { ((int)position.X - (int)Position.X) / 55,
                ((int)position.Y - (int)Position.Y) / 55 };
            Gem selectedGem = GemGrid[selectedGemPos[0], selectedGemPos[1]];

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

                List<Vector2> firstGemMatch = FindMatch(x2, y2, GemGrid[x1, y1].Color);
                List<Vector2> secondGemMatch = FindMatch(x1, y1, GemGrid[x2, y2].Color);

                if (firstGemMatch.Count == 0 &&  secondGemMatch.Count == 0)
                {
                    SwapGems(new int[] { x1, y1 },
                    new int[] { x2, y2 });
                }
                else
                {
                    SetBonusCandidate(new int[] { x1, y1 }, secondGemMatch.Count);
                    SetBonusCandidate(new int[] { x2, y2 }, firstGemMatch.Count);

                    foreach (Vector2 gemPostion in firstGemMatch)
                        GemsToDestroy.Add(new int[] { (int)gemPostion.X, (int)gemPostion.Y });
                    foreach (Vector2 gemPostion in secondGemMatch)
                        GemsToDestroy.Add(new int[] { (int)gemPostion.X, (int)gemPostion.Y });

                    DestroyMatches();
                }

                SelectedPositions.Clear();
            }
            //selectedGem.ChangeState();
        }

        private void SetBonusCandidate(int[] candidate, int matchCount)
        {
            if (matchCount >= 5) BombCandidates.Add(candidate);
            else if (matchCount == 4) HorizontalLineCandidates.Add(candidate);
        }

        public List<Vector2> GetAllMatches()
        {
            List<Vector2> checkedGems = new List<Vector2>();

            for (int i = 0; i < Width - 2; i++)
            {
                for (int j = 0; j < Height - 2; j++)
                {
                    if (checkedGems.Contains(new Vector2(i, j)))
                    {
                        Color matchingColor = GemGrid[i, j].Color;

                        checkedGems = checkedGems.Concat(FindMatch(i, j, matchingColor)).ToList<Vector2>();
                    }
                }
            }

            return checkedGems;
        }

        private List<Vector2> FindMatch(int x, int y, Color matchingColor)
        {
            int hScore = 0;
            int vScore = 0;
            int totalScore = 0;
            List<Vector2> gemsInMatch = new List<Vector2> { new Vector2(x, y) };

            gemsInMatch = gemsInMatch.
                Concat(FindHorizontalMatch(x + 1, y, 1, GemGrid[x, y].Color).
                Concat(FindHorizontalMatch(x - 1, y, -1, GemGrid[x, y].Color)).
                ToList<Vector2>()).ToList<Vector2>();

            hScore += gemsInMatch.Count;

            gemsInMatch = gemsInMatch.
                Concat(FindVerticalMatch(x, y + 1, 1, GemGrid[x, y].Color).
                Concat(FindVerticalMatch(x, y - 1, -1, GemGrid[x, y].Color)).
                ToList<Vector2>()).ToList<Vector2>();

            vScore += gemsInMatch.Count - hScore + 1;

            totalScore = hScore + vScore;

            if (totalScore < 3) return new List<Vector2>();
            else return gemsInMatch;
        }

        private List<Vector2> FindHorizontalMatch(int x, int y, int offset, Color matchingColor)
        {
            List<Vector2> gemsInMatch = new List<Vector2> { };

            if (x >= 0 && x < Width)
            {
                if (GemGrid[x, y].Color == matchingColor)
                {
                    gemsInMatch.Add(new Vector2(x, y));
                    return gemsInMatch.
                        Concat(FindHorizontalMatch(x + offset, y, offset, matchingColor)).ToList<Vector2>();
                }
            }

            return gemsInMatch;
        }

        private List<Vector2> FindVerticalMatch(int x, int y, int offset, Color matchingColor)
        {
            List<Vector2> gemsInMatch = new List<Vector2> { };

            if (y >= 0 && y < Width)
            {
                if (GemGrid[x, y].Color == matchingColor)
                {
                    gemsInMatch.Add(new Vector2(x, y));
                    return gemsInMatch.Concat(FindVerticalMatch(x, y + offset, offset, matchingColor)).ToList<Vector2>();
                }
            }

            return gemsInMatch;
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

            GetAllMatches();
            DestroyMatches();
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
