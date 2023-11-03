using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine.Manager;
using CPI311.GameEngine.Sprite;

namespace Lab02
{
    public class Lab02 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Sprite sprite;
        SpiralMover _spiralMover;

        public Lab02()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            //sprite = new Sprite(Content.Load<Texture2D>("Square"));
            _spiralMover = new SpiralMover(Content.Load<Texture2D>("Square"), new Vector2(300,300));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);
            _spiralMover.Update();

            /*
            // Moves sprite left
            if (InputManager.IsKeyPressed(Keys.Left))
            {
                //sprite.Position += Vector2.UnitX * -5;
            }
            // Moves sprite right
            if (InputManager.IsKeyPressed(Keys.Right))
            {
                //sprite.Position += Vector2.UnitX * 5;
            }
            // Moves sprite up
            if (InputManager.IsKeyPressed(Keys.Up))
            {
                //sprite.Position += Vector2.UnitY * -5;
            }
            // Moves sprite down
            if (InputManager.IsKeyPressed(Keys.Down))
            {
                //sprite.Position += Vector2.UnitY * 5;
            }
            // Rotates sprite clockwise
            if (InputManager.IsKeyDown(Keys.Space))
            {
                //sprite.Rotation += 0.05f;
            }
            */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //sprite.Draw(_spriteBatch);
            _spiralMover.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}