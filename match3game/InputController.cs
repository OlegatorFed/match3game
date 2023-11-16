using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class InputController
    {
        public MouseState MState;
        public event Action<Vector2> MouseClicked;

        public InputController()
        {
            MState = Mouse.GetState();
        }

        public Vector2 GetMousePos() { return new Vector2(MState.X, MState.Y); }

        public void CheckMouseClick()
        {
            if (MState.LeftButton == ButtonState.Pressed)
            {
                MouseClicked?.Invoke(GetMousePos());
            }
        }

        public void Update()
        {
            MState = Mouse.GetState();
            CheckMouseClick();
        }
    }
}
