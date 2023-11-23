using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class GameController
    {

        enum GameState
        {
            Menu,
            Game,
            End
        }

        public event Action GameStart;
        public event Action GameStop;

        public GameController()
        {
        }
    }
}
