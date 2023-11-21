using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class ScoreController
    {
        public int Score;
        private FieldController FieldController;

        public ScoreController(FieldController fieldController)
        {
            Score = 0;
            FieldController = fieldController;

            fieldController.Score += OnScore;
        }

        public void OnScore(int score)
        {
            Score += score;
        }

    }
}
