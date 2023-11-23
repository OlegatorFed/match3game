using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace match3game
{
    internal class Button
    {
        public bool Active { get; set; }
        public Point Position { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string Text { get; set; }

        public Button(Point position, int height, int width, InputController inputController) 
        {
            Position = position;
            Width = width;
            Height = height;
            Text = string.Empty;

            inputController.MouseClicked += OnClick;

            Active = false;
        }
        public Button(Point position, int height, int width, string text, InputController inputController)
        {
            Position = position;
            Width = width;
            Height = height;
            Text = text;

            inputController.MouseClicked += OnClick;

            Active = false;
        }

        public void Action()
        {

        }

        public void OnClick(Vector2 clickPos)
        {
            if ((clickPos.X > Position.X && clickPos.X < Position.X + Width) &&
                (clickPos.Y > Position.Y && clickPos.Y < Position.Y + Height) &&
                Active)
            {
                Action();
            }
        }
    }
}
