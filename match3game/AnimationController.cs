using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class AnimationController
    {
        public List<Gem> GemsToUpdate;

        public event Action Spawned;
        public event Action Moved;
        public event Action Destroyed;

        public AnimationController()
        {
            GemsToUpdate = new List<Gem>();
        }

    }
}
