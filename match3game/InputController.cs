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

        public Vector2 GetMousePos()
        {
            return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

        public void CheckMouseClick()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                MouseClicked?.Invoke(GetMousePos());
            }
        }

        public void Update()
        {
            CheckMouseClick();
        }
    }
}
