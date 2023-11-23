using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class HorizontalDestroyer
    {
        public string TextureName { get; private set; }
        public Point Position { get; private set; }
        public Point Destination { get; private set; }

        private List<Point> GemsToPass;

        public event Action<HorizontalDestroyer> Finished;
        public event Action<Point> PassedBy;

        public enum State
        {
            Move,
            Stoped
        }

        public State CurrentState { get; private set; }

        public HorizontalDestroyer(Point position, Point destination, List<Point> gemsToPass)
        {
            if (position.X - destination.X > 0)
            {
                TextureName = "bracket_h_l";
            }
            else if (position.X - destination.X < 0)
            {
                TextureName = "bracket_h_r";
            }
            Position = position;
            Destination = destination;
            GemsToPass = gemsToPass;

            CurrentState = State.Move;
        }

        public void MoveUpdate(Point destination)
        {
            int speed = 5;
            int hDirection = Math.Sign(destination.X - Position.X);
            int hDistance = Math.Abs(destination.X - Position.X);

            Position = new Point(Position.X + speed * hDirection, Position.Y);

            foreach (Point p in GemsToPass)
            {
                if (Position.X > p.X && Position.X < p.X + 55)
                {
                    PassedBy?.Invoke(p);
                    //GemsToPass.Remove(p);
                }
            }

            if (hDistance <= 5)
            {
                Position = destination;
                CurrentState = State.Stoped;
                Finished?.Invoke(this);
            }
        }

        public void Update()
        {
            MoveUpdate(Destination);
        }
    }
}
