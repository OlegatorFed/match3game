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

        public Gem(Color color) => Color = color;

        public virtual void Action() 
        { 
            
        }
    }
}
