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

        enum State
        {
            Idle,
            Spawn,
            Move,
            Destroy
        }

        State CurrentState = State.Idle;

        public event Action Finished;
        public event Action Spawned;
        public event Action Moved;
        public event Action Destroyed;



        public AnimationController()
        {
            GemsToUpdate = new List<Gem>();
        }

        public void SubscribeToFieldController(FieldController fieldController)
        {
            fieldController.Spawning += SetSpawningGems;
            fieldController.Swaping += SetMovingGems;
            fieldController.Destroying += SetDyingGems;
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

        public bool HasDestroyedGems()
        {
            return GemsToUpdate.Any(gem => gem.CurrentState == Gem.State.Destroyed);
        }

        private void ChangeState(State state)
        {
            CurrentState = state;
        }

        public void SetSpawningGems(Gem spawningGem)
        {
            ChangeState(State.Spawn);
            GemsToUpdate.Add(spawningGem);
            spawningGem.ChangeState(Gem.State.Spawning);
        }

        public void SetDyingGems(Gem dyingGem)
        {
            ChangeState(State.Destroy);
            GemsToUpdate.Add(dyingGem);
            dyingGem.Action();
        }

        public void SetMovingGems(Gem movingGems)
        {
            ChangeState(State.Move);
            GemsToUpdate.Add(movingGems);
            movingGems.ChangeState(Gem.State.Moving);
        }

        public void ClearUpdatingGems()
        {
            if (CurrentState == State.Move)
            {
                GemsToUpdate.Clear();
                Moved?.Invoke();
                ChangeState(State.Idle);
            }
            else if (CurrentState == State.Destroy)
            {
                Destroyed?.Invoke();
                GemsToUpdate.Clear();
                ChangeState(State.Idle);
            } 
            else if (CurrentState == State.Spawn)
            {
                GemsToUpdate.Clear();
                Spawned?.Invoke();
                ChangeState(State.Idle);
            }

            Finished?.Invoke();
        }

    }
}
