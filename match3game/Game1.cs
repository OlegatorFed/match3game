using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using static match3game.Gem;

namespace match3game
{
    public class Game1 : Game
    {
        Texture2D rectTexture;
        Dictionary<string, Texture2D> textures;
        Vector2 rectPosition;
        //MouseState mouseState = Mouse.GetState();
        
        FieldController fieldController;
        InputController inputController;

        enum GameState
        {
            Selecting
        }


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            rectPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - (55 * 4),
                _graphics.PreferredBackBufferHeight / 2 - (55 * 4));
            textures = new Dictionary<string, Texture2D>();
            //mouseState = Mouse.GetState();

            fieldController = new FieldController(8, 8, rectPosition);
            inputController = new InputController();

            inputController.MouseClicked += fieldController.OnClick;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            
            textures.Add("rect_white", Content.Load<Texture2D>("rect_white"));
            textures.Add("rect_white_border", Content.Load<Texture2D>("rect_white_border"));

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            inputController.Update();
            GemUpdateRoutine();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            GemDrawRoutine();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void GemDrawRoutine()
        {
            for (int i = 0; i < fieldController.Width; i++)
            {
                for (int j = 0; j < fieldController.Height; j++)
                {
                    if (fieldController.GemGrid[i, j] != null)
                    {
                        _spriteBatch.Draw(textures[fieldController.GemGrid[i, j].TextureName],
                        new Vector2(fieldController.GemGrid[i, j].Position.X, fieldController.GemGrid[i, j].Position.Y),
                        fieldController.GemGrid[i, j].Color);
                    }
                }
            }
        }

        protected void GemUpdateRoutine()
        {
            if (fieldController.GemsToUpdate.Count > 0)
            {
                foreach (Gem gemToUpdate in fieldController.GemsToUpdate)
                {
                    gemToUpdate.Update();
                }

                fieldController.CheckMovingGems();
            }
        }

    }
}