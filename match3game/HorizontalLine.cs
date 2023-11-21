using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class HorizontalLine : Gem
    {

        public HorizontalLine(Point position, Color color) : base(position, color)
        {
            Scale = 1f;
            UnselectedTextureName = "rect_white_h";
            SelectedTextureName = "rect_white_h_border";
            TextureName = UnselectedTextureName;
            CurrentState = State.Idle;
        }

        public override void Action()
        {
            base.Action();

        }
    }

}
