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
        private bool HoldingButton;

        public InputController()
        {
            MState = Mouse.GetState();
            HoldingButton = false;
        }

        public Vector2 GetMousePos() { return new Vector2(MState.X, MState.Y); }

        public void CheckMouseClick()
        {
            if (MState.LeftButton == ButtonState.Pressed && !HoldingButton)
            {
                MouseClicked?.Invoke(GetMousePos());
                HoldingButton = true;
            }
            else if (MState.LeftButton == ButtonState.Released)
            {
                HoldingButton = false;
            }
        }

        public void Update()
        {
            MState = Mouse.GetState();
            CheckMouseClick();
        }
    }
}
