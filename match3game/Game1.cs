using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static match3game.Gem;

namespace match3game
{
    public class Game1 : Game
    {
        Texture2D rectTexture;
        Dictionary<string, Texture2D> textures;
        SpriteFont font;
        Vector2 rectPosition;
        
        GameController gameController;

        FieldController fieldController;
        InputController inputController;
        AnimationController animationController;
        ScoreController scoreController;

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
            rectPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2 - (55 * 4),
                _graphics.PreferredBackBufferHeight / 2 - (55 * 4));
            textures = new Dictionary<string, Texture2D>();

            //GameController gameController = new GameController();

            animationController = new AnimationController();
            fieldController = new FieldController(8, 8, rectPosition, animationController);
            inputController = new InputController();
            scoreController = new ScoreController(fieldController);
            
            animationController.SubscribeToFieldController(fieldController);

            inputController.MouseClicked += fieldController.OnClick;

            fieldController.GenerateField();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            DirectoryInfo contentDir = new DirectoryInfo(Content.RootDirectory + "/Textures");
            FileInfo[] files = contentDir.GetFiles("*.xnb");

            font = Content.Load<SpriteFont>("font");

            foreach (FileInfo texture in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(texture.Name);
                textures.Add(fileName, Content.Load<Texture2D>(fileName));
            }
;

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            inputController.Update();
            GemUpdateRoutine();
            BonusUpdate();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();

            GemDrawRoutine();
            BonusDraw();

            _spriteBatch.DrawString(font, scoreController.Score.ToString(), rectPosition - new Vector2(100, 100), Color.Black);

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
                        Gem gemToDraw = fieldController.GemGrid[i, j];
                        _spriteBatch.Draw(textures[gemToDraw.TextureName],
                        new Rectangle(gemToDraw.Position.X,
                            gemToDraw.Position.Y,
                            (int)(textures[gemToDraw.TextureName].Width * gemToDraw.Scale),
                            (int)(textures[gemToDraw.TextureName].Height * gemToDraw.Scale)),
                        gemToDraw.Color);
                    }
                }
            }
        }

        protected void BonusDraw()
        {
            foreach (HorizontalDestroyer destroyer in fieldController.ActiveHorizontalDestroyers)
            {
                _spriteBatch.Draw(textures[destroyer.TextureName],
                    new Vector2(destroyer.Position.X, destroyer.Position.Y),
                    Color.White);
            }
        }

        protected void BonusUpdate()
        {
            if (fieldController.ActiveHorizontalDestroyers.Count > 0)
            {
                foreach (HorizontalDestroyer destroyer in fieldController.ActiveHorizontalDestroyers)
                {
                    destroyer.Update();
                }

                if (fieldController.ActiveHorizontalDestroyers.All( (HorizontalDestroyer item) => item.CurrentState == HorizontalDestroyer.State.Stoped) )
                {
                    fieldController.ActiveHorizontalDestroyers.Clear();
                }
            }
            
        }

        protected void GemUpdateRoutine()
        {
            if (animationController.GemsToUpdate.Count > 0)
            {
                foreach (Gem gemToUpdate in animationController.GemsToUpdate)
                {
                    gemToUpdate.Update();
                }

                if (!animationController.HasDyingGems() &&
                    !animationController.HasMovingGems() &&
                    !animationController.HasSpawningGems() ||
                    animationController.HasDestroyedGems())
                {
                    animationController.ClearUpdatingGems();
                }
            }
        }

    }
}