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
        public string TextureName { get; private set; }
        public Point Position { get; set; }
        public Point Destination { get; set; }
        public enum State
        {
            Unselected,
            Selected,
            Moving
        }

        public event Action FinieshedMoving;

        public State CurrentState { get; private set; }

        public Gem(Color color)
        {
            Color = color;
            TextureName = "rect_white";
            CurrentState = State.Unselected;
        }

        public Gem(Point position, Color color)
        {
            Position = position;
            Color = color;
            TextureName = "rect_white";
            CurrentState = State.Unselected;
        }

        public void MoveTo(Point destination)
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
                CurrentState = State.Unselected;
                FinieshedMoving?.Invoke();
            }

        }

        public void SwtichSelectState()
        {
            if (CurrentState == State.Selected)
            {
                ChangeState(State.Unselected);
                TextureName = "rect_white";
            }
            else
            {
                ChangeState(State.Selected);
                TextureName = "rect_white_border";
            }
        }

        public void ChangeState(State state)
        {
            CurrentState = state;
        }

        public void Update()
        {
            if (CurrentState == State.Moving)
                MoveTo(Destination);
        }

        public virtual void Action() 
        { 
            
        }
    }
}
