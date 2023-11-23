using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class Gem
    {
        public Color Color { get; private set; }
        public float Scale { get; protected set; }
        public string TextureName { get; protected set; }
        protected string UnselectedTextureName;
        protected string SelectedTextureName;
        public Point Position { get; set; }
        public Point Destination { get; set; }
        public enum State
        {
            Idle,
            Spawning,
            Moving,
            Dying,
            Destroyed
        }
        enum SelectState
        {
            Selected,
            Unselected
        }

        public event Action Spawned;
        public event Action FinieshedMoving;
        public event Action<Point> Destroyed;

        public State CurrentState { get; protected set; }
        private SelectState CurrentSelectState;

        public Gem(Color color)
        {
            Color = color;
            TextureName = "rect_white";
            ChangeState(State.Idle);
        }

        public Gem(Point position, Color color)
        {
            Position = position;
            Destination = Position;
            Color = color;
            Scale = 0f;
            UnselectedTextureName = "rect_white";
            SelectedTextureName = "rect_white_border";
            TextureName = UnselectedTextureName;
            CurrentSelectState = SelectState.Unselected;
            CurrentState = State.Spawning;

        }

        public void SpawnUpdate()
        {
            Scale += 0.1f;

            if (Scale >= 1f)
            {
                Spawned?.Invoke();
                ChangeState(State.Idle);
                Scale = 1f;
            }
        }

        public void MoveUpdate(Point destination)
        {
            int speed = 5;
            int hDirection = Math.Sign(destination.X - Position.X);
            int vDirection = Math.Sign(destination.Y - Position.Y);
            int hDistance = Math.Abs(destination.X - Position.X);
            int vDistance = Math.Abs(destination.Y - Position.Y);

            Position = new Point(Position.X + speed * hDirection, Position.Y + speed * vDirection);

            if (hDistance <= 5 && vDistance <= 5)
            {
                Position = destination;
                ChangeState(State.Idle);
                FinieshedMoving?.Invoke();
            }

        }

        public void DestroyUpdate()
        {
            Scale -= 0.1f;

            if (Scale <= 0.1f)
            {
                Destroyed?.Invoke(Position);
                ChangeState(State.Destroyed);
                Scale = 0f;
            }
        }

        public void SwtichSelectState()
        {
            if (CurrentSelectState == SelectState.Selected)
            {
                CurrentSelectState = SelectState.Unselected;
                TextureName = UnselectedTextureName;
            }
            else if (CurrentSelectState == SelectState.Unselected)
            {
                CurrentSelectState = SelectState.Selected;
                TextureName = SelectedTextureName;
            }
        }

        public void ChangeState(State state)
        {
            CurrentState = state;
        }

        public void InitiateDestroying()
        {
            ChangeState(State.Dying);
        }

        public void Update()
        {
            if (CurrentState == State.Spawning)
                SpawnUpdate();
            if (CurrentState == State.Moving)
                MoveUpdate(Destination);
            if (CurrentState == State.Dying)
                DestroyUpdate();
        }

        

        public virtual void Action() 
        { 
            InitiateDestroying();
        }
    }
}
