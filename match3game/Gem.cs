using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class Gem
    {
        public Color Color { get; private set; }
        public string TextureName { get; set; }
        public enum State
        {
            Unselected,
            Selected
        }
        public State CurrentState = State.Unselected;

        public Gem(Color color)
        {
            Color = color;
            TextureName = "rect_white";
        }

        public void ChangeState()
        {
            if (CurrentState == State.Selected)
            {
                CurrentState = State.Unselected;
                TextureName = "rect_white";
            }
            else 
            {
                CurrentState = State.Selected;
                TextureName = "rect_white_border";
            }
        }

        public virtual void Action() 
        { 
            
        }
    }
}
