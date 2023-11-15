using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace match3game
{
    public class Game1 : Game
    {
        Texture2D rectTexture;
        Vector2 rectPosition;
        
        FieldController fieldController;


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
            rectPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - (55 * 4), _graphics.PreferredBackBufferHeight / 2 - (55 * 4));

            fieldController = new FieldController(8, 8);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            rectTexture = Content.Load<Texture2D>("rect_white");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            for (int i = 0; i < fieldController.Width; i++)
            {
                for (int j = 0; j < fieldController.Height; j++)
                {
                    _spriteBatch.Draw(rectTexture, rectPosition + new Vector2(i * 55, j * 55), fieldController.GemGrid[i, j].Color);
                }
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}