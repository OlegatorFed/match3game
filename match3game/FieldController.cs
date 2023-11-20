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
        private string[] CustomMap;
        public Vector2 Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Gem[,] GemGrid { get; private set; }
        public List<Gem> GemsToUpdate { get; private set; }
        public List<Gem> GemsToSpawn { get; private set; }
        public State CurrentState { get; private set; }

        private int gemSize = 55;

        private List<Vector2> SelectedPositions;

        private HashSet<Vector2> GemsToDestroy;
        
        private List<int[]> HorizontalLineCandidates;
        private List<int[]> VerticalLineCandidates;
        private List<int[]> BombCandidates;

        Color[] colors;

        public event Action Swapped;

        public enum State
        {
            Generate,
            Idle,
            Move,
            Destroy
        }

        public FieldController(int width, int height, Vector2 position)
        {
            CustomMap = new string[] { "30310101",
                                       "12101010",
                                       "01010101",
                                       "10101010",
                                       "01010101",
                                       "10101010",
                                       "01010101",
                                       "10101010"}; 

            Width = width;
            Height = height;
            Position = position;

            GemGrid = new Gem[width, height];

            SelectedPositions = new List<Vector2>();

            GemsToUpdate = new List<Gem>();
            GemsToDestroy = new HashSet<Vector2>();

            HorizontalLineCandidates = new List<int[]>();
            VerticalLineCandidates = new List<int[]>();
            BombCandidates = new List<int[]>();

            CurrentState = State.Generate;

            GenerateColors();
            //GenerateField();
            //GenerateCustomField();

            ChangeState(State.Idle);
            //DestroyAllMatches();
            
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

            if (TrySwap(firstGemPos, secondGemPos))
            {
                /*if (GemGrid[x1, y1] != null)
                    GemGrid[x1, y1].MoveTo(new Point((int)Position.X + x2 * gemSize, (int)Position.Y + y2 * gemSize));*/
                //ChangeState(State.Move);

                (GemGrid[x1, y1], GemGrid[x2, y2])
                    = (GemGrid[x2, y2], GemGrid[x1, y1]);

                UpdateGemPosition(x1, y1);
                UpdateGemPosition(x2, y2);
            }
                

        }

        private bool TrySwap(int[] firstGemPos, int[] secondGemPos)
        {
            int x1 = firstGemPos[0];
            int y1 = firstGemPos[1];
            int x2 = secondGemPos[0];
            int y2 = secondGemPos[1];

            return (Math.Abs(x1 - x2) == 1 && y1 == y2 || x1 == x2 && Math.Abs(y1 - y2) == 1);
        }

        private void UpdateGemPosition(int x, int y)
        {
            if (GemGrid[x, y] != null)
            {
                GemGrid[x, y].Destination = new Point((int)Position.X + x * gemSize, (int)Position.Y + y * gemSize);
                GemsToUpdate.Add(GemGrid[x, y]);
                GemGrid[x, y].ChangeState(Gem.State.Moving);
            }
        }

        public void DestroyGem(Vector2 destroyPosition)
        {
            /*GemsToUpdate.Add(GemGrid[(int)destroyPosition.X, (int)destroyPosition.Y]); 

            GemGrid[(int)destroyPosition.X, (int)destroyPosition.Y].InitiateDestroying();*/
            GemGrid[(int)destroyPosition.X, (int)destroyPosition.Y] = null;
        }

        public void DeleteGem(Gem gem)
        {
            gem = null;
        }

        public void CreateGem(int[] position)
        {
            Random random = new Random();
            GemGrid[position[0], position[1]] = 
                new Gem(new Point((int)Position.X + position[0] * gemSize, (int)Position.Y + position[1] * gemSize), colors[random.Next(colors.Length)]);

            GemsToUpdate.Add(GemGrid[position[0], position[1]]);

            GemGrid[position[0], position[1]].Destroyed += DeleteGem;
        }

        public void CreateGem(int[] position, int colorNumber)
        {
            Random random = new Random();
            GemGrid[position[0], position[1]] = 
                new Gem(new Point((int)Position.X + position[0] * gemSize, (int)Position.Y + position[1] * gemSize), colors[colorNumber]);

            GemsToUpdate.Add(GemGrid[position[0], position[1]]);

            GemGrid[position[0], position[1]].Destroyed += DeleteGem;
        }

        public void ClearUpdateList()
        {
            GemsToUpdate.Clear();
            //UpdateCleared?.Invoke();
        }

        public bool IsUpdateListEmpty()
        {
            return !HasMovingGems() && !HasDyingGems() && !HasSpawningGems();
        }

        public bool HasMovingGems()
        {
            return GemsToUpdate.Any(gem => gem.CurrentState == Gem.State.Moving);
        }

        public bool HasSpawningGems()
        {
            return GemsToUpdate.Any(gem => gem.CurrentState == Gem.State.Spawning);
        }

        public bool HasDyingGems()
        {
            return GemsToUpdate.Any(gem => gem.CurrentState == Gem.State.Dying);
        }

        public void DestroyMatches()
        {
            //ChangeState(State.Destroy);

            foreach (Vector2 positionToDestroy in GemsToDestroy)
            {
                DestroyGem(positionToDestroy);
            }

            //foreach (Vector2 freePosition in GemsToDestroy)
            //    FallGemsAbove(freePosition);
            FallAllGems();

            GemsToDestroy.Clear();
            //FillEmptySlots();
        }

        private void FallAllGems()
        {
            for (int x = 0; x < Width;  x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (GemGrid[x, y] == null)
                    {
                        FallGemsAbove(new Vector2(x, y));
                    }
                }
            }
        }

        private void FallGemsAbove(Vector2 fallPosition)
        {
            for (int y = (int)fallPosition.Y; y > 0; y--)
                SwapGems(new int[] { (int)fallPosition.X, y }, new int[] { (int)fallPosition.X, y - 1 });
        }

        public void SelectGem(Vector2 position)
        {
            if (CurrentState == State.Idle)
            {
                int[] selectedGemPos = new int[2] { ((int)position.X - (int)Position.X) / gemSize,
                ((int)position.Y - (int)Position.Y) / gemSize };
                Gem selectedGem = GemGrid[selectedGemPos[0], selectedGemPos[1]];

                if (SelectedPositions.Count < 2)
                {
                    selectedGem.SwtichSelectState();
                    SelectedPositions.Add(new Vector2(selectedGemPos[0], selectedGemPos[1]));
                }
                if (SelectedPositions.Count == 2)
                {
                    int x1 = (int)SelectedPositions[0].X;
                    int y1 = (int)SelectedPositions[0].Y;
                    int x2 = (int)SelectedPositions[1].X;
                    int y2 = (int)SelectedPositions[1].Y;

                    if (TrySwap(new int[] { x1, y1 },
                            new int[] { x2, y2 }))
                    {
                        GemGrid[x1, y1].SwtichSelectState();
                        GemGrid[x2, y2].SwtichSelectState();

                        SwapGems(new int[] { x1, y1 },
                        new int[] { x2, y2 });

                        FindSwapMatches(x1, y1, x2, y2);
                    }

                    SelectedPositions.Clear();
                }
            }
        }

        private void FindSwapMatches(int x1, int y1, int x2, int y2)
        {
            List<Vector2> firstGemMatch = FindMatch(x2, y2, GemGrid[x2, y2].Color);
            List<Vector2> secondGemMatch = FindMatch(x1, y1, GemGrid[x1, y1].Color);

            if (firstGemMatch.Count == 0 && secondGemMatch.Count == 0)
            {
                SwapGems(new int[] { x1, y1 },
                    new int[] { x2, y2 });
            }
            else
            {
                SetBonusCandidate(new int[] { x1, y1 }, secondGemMatch.Count);
                SetBonusCandidate(new int[] { x2, y2 }, firstGemMatch.Count);
                
                foreach (Vector2 gemPostion in firstGemMatch)
                    GemsToDestroy.Add(gemPostion);
                foreach (Vector2 gemPostion in secondGemMatch)
                    GemsToDestroy.Add(gemPostion);
                
                DestroyMatches();
            }
        }

        private void SetGemsToDesroy()
        {

        }

        private void SetBonusCandidate(int[] candidate, int matchCount)
        {
            if (matchCount >= 5) BombCandidates.Add(candidate);
            else if (matchCount == 4) HorizontalLineCandidates.Add(candidate);
        }

        public void GetAllMatches()
        {
            HashSet<Vector2> checkedGems = new HashSet<Vector2>();

            for (int y = 0; y < Width; y++)
            {
                for (int x = 0; x < Height; x++)
                {
                    if (!checkedGems.Contains(new Vector2(x, y)))
                    {
                        Color matchingColor = GemGrid[x, y].Color;

                        checkedGems = checkedGems.Concat(FindMatch(x, y, matchingColor)).ToHashSet<Vector2>();
                    }
                }
            }

            foreach (Vector2 gemPos in checkedGems)
                GemsToDestroy.Add(gemPos);

            
        }

        private List<Vector2> FindMatch(int x, int y, Color matchingColor)
        {
            int hScore = 0;
            int vScore = 0;
            int totalScore = 0;
            List<Vector2> gemsInMatch = new List<Vector2> { new Vector2(x, y) };
            List<Vector2> rightMatch;
            List<Vector2> leftMatch;
            List<Vector2> upMatch;
            List<Vector2> downMatch;

            rightMatch = FindHorizontalMatch(x + 1, y, 1, GemGrid[x, y].Color);
            leftMatch = FindHorizontalMatch(x - 1, y, -1, GemGrid[x, y].Color);
            leftMatch.Reverse();

            upMatch = FindVerticalMatch(x, y - 1, -1, GemGrid[x, y].Color);
            downMatch = FindVerticalMatch(x, y + 1, 1, GemGrid[x, y].Color);
            upMatch.Reverse();

            hScore += rightMatch.Count + leftMatch.Count;
            vScore += upMatch.Count + downMatch.Count;

            if (vScore >= 2) 
                gemsInMatch = upMatch.Concat(gemsInMatch.Concat(downMatch).ToList()).ToList<Vector2>();

            if (hScore >= 2) 
                gemsInMatch = leftMatch.Concat(gemsInMatch.Concat(rightMatch).ToList()).ToList<Vector2>(); ;

            totalScore = hScore + vScore;

            if (hScore < 2 && vScore < 2) return new List<Vector2>();
            else return gemsInMatch;
        }

        private List<Vector2> FindHorizontalMatch(int x, int y, int offset, Color matchingColor)
        {
            List<Vector2> gemsInMatch = new List<Vector2> { };

            if (x >= 0 && x < Width && GemGrid[x, y] != null)
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

            if (y >= 0 && y < Height && GemGrid[x, y] != null)
            {
                if (GemGrid[x, y].Color == matchingColor)
                {
                    gemsInMatch.Add(new Vector2(x, y));
                    return gemsInMatch.
                        Concat(FindVerticalMatch(x, y + offset, offset, matchingColor)).ToList<Vector2>();
                }
            }

            return gemsInMatch;
        }

        public void FillEmptySlots()
        {
            bool filled = false;

            //ChangeState(State.Generate);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (GemGrid[x, y] == null)
                    {
                        CreateGem(new int[] { x, y });
                        filled = true;
                    }
                }
            }

            if (filled)
            {
                DestroyAllMatches();
            }
            else
            {
                //ChangeState(State.Idle);
            }
            
        }

        public void GenerateField()
        {
            //ChangeState(State.Generate);

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    CreateGem(new int[] { x, y });

                }
            }

            //GetAllMatches();
            //DestroyMatches();
        }

        public void GenerateCustomField()
        {
            //ChangeState(State.Generate);

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                {
                    CreateGem(new int[] { x, y },  CustomMap[y][x] - '0');
                }

            //GetAllMatches();
            //DestroyMatches();

            //DestroyAllMatches();
        }

        public void DecideAction()
        {
            if (CurrentState == State.Move)
            {
                
            }
        }

        public void ChangeState(State state)
        {
            CurrentState = state;
        }

        public void DestroyAllMatches()
        {
            GetAllMatches();

            if (GemsToDestroy.Count > 0)
            {
                //ChangeState(State.Destroy);
                DestroyMatches();
            }
            
            FillEmptySlots();
        }

        public void OnClick(Vector2 position)
        {
            if ((position.X > Position.X && position.X < Position.X + Width * 55) &&
                (position.Y > Position.Y && position.Y < Position.Y + Height * 55) && CurrentState == State.Idle)
            {
                SelectGem(position);
            }
        }

        public void Update()
        {
            
        }

    }
}
