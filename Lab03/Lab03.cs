using CPI311.GameEngine.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Lab03
{
    public class Lab03 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont font;

        private bool isToggleMatrix, isToggleCamera = true;
        private string cameraMode; 

        // *** First Step
        Model model; //for FBX 3D model
        Matrix world, view, projection;

        // *** Second Step
        Vector3 cameraPos = new Vector3(0, 0, 5);
        Vector3 modelPos = new Vector3(0, 0, 0);
        float modelScale = 1.0f;
        float yaw = 0, roll = 0, pitch = 0;

        float viewingNear = 0.1f, viewingFar = 1000f;

        Vector2 cameraCenter = new Vector2(0,0);
        Vector2 cameraSize = new Vector2(1,1);

        public Lab03()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // CPI311 Manager Init
            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            model = Content.Load<Model>("Torus");
            font = Content.Load<SpriteFont>("Font");
            // Lighting Effect ON
            foreach(ModelMesh mesh in model.Meshes) 
            {
                foreach(BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // CPI311 Manager Update
            InputManager.Update();
            Time.Update(gameTime);

            // *** Object Movement Only (Arrow Key) ***
            if (InputManager.IsKeyDown(Keys.Up))
                modelPos += Vector3.Forward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.Down))
                modelPos += Vector3.Backward * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.Left))
                modelPos += Vector3.Left * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.Right))
                modelPos += Vector3.Right * Time.ElapsedGameTime * 5;

            // *** Rotation Movement ***
            // Yaw Movement (Insert/Delete)
            if (InputManager.IsKeyDown(Keys.Insert))
                yaw += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.Delete))
                yaw -= Time.ElapsedGameTime * 5;
            // Pitch Movement (Home/End)
            if (InputManager.IsKeyDown(Keys.Home))
                pitch += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.End))
                pitch -= Time.ElapsedGameTime * 5;
            // Roll Movement (PageUp/PageDown)
            if (InputManager.IsKeyDown(Keys.PageUp))
                roll += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.PageDown))
                roll -= Time.ElapsedGameTime * 5;

            // *** Scale Movement (Shift + Up/Down) ***
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.Up))
                modelScale += 1.0f;
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.Down))
                modelScale -= 1.0f;

            // *** World Matrix Change ***
            if (InputManager.IsKeyPressed(Keys.Space))
                isToggleMatrix = !isToggleMatrix;

            if(isToggleMatrix)
            {
                world = Matrix.CreateScale(modelScale) *
                    Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                    Matrix.CreateTranslation(modelPos);
            }
            else
            {
                world = Matrix.CreateTranslation(modelPos) *
                    Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                    Matrix.CreateScale(modelScale);
            }

            // *** Camera Movement (WASD) ***
            if (InputManager.IsKeyDown(Keys.W)) 
                cameraPos += Vector3.Up * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.S)) 
                cameraPos += Vector3.Down * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.A)) 
                cameraPos += Vector3.Right * Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.D)) 
                cameraPos += Vector3.Left * Time.ElapsedGameTime * 5;

            // *** Orthohraphic/Perspective Switch ***
            if (InputManager.IsKeyPressed(Keys.Tab))
                isToggleCamera = !isToggleCamera;

            if (isToggleCamera)
            {
                projection = Matrix.CreatePerspectiveOffCenter(cameraCenter.X - cameraSize.X, cameraCenter.X + cameraSize.X, cameraCenter.Y - cameraSize.Y, cameraCenter.Y + cameraSize.Y, viewingNear, viewingFar);
                cameraMode = "Perspective";
            }
            else
            {
                projection = Matrix.CreateOrthographicOffCenter(cameraCenter.X - cameraSize.X, cameraCenter.X + cameraSize.X, cameraCenter.Y - cameraSize.Y, cameraCenter.Y + cameraSize.Y, viewingNear, viewingFar);
                cameraMode = "Orthographic";
            }
            
            // *** Camera Center Movement ***
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.W))
                cameraCenter.Y += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.S))
                cameraCenter.Y -= Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.A))
                cameraCenter.X += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.LeftShift) && InputManager.IsKeyDown(Keys.D))
                cameraCenter.X -= Time.ElapsedGameTime * 5;

            // *** Width/Height Change ***
            if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.W))
                cameraSize.X += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.S))
                cameraSize.X -= Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.A))
                cameraSize.Y += Time.ElapsedGameTime * 5;
            if (InputManager.IsKeyDown(Keys.LeftControl) && InputManager.IsKeyDown(Keys.D))
                cameraSize.Y -= Time.ElapsedGameTime * 5;

            // Viewpoint
            view = Matrix.CreateLookAt(
                cameraPos, // Camera Position
                cameraPos + Vector3.Forward, // Camera Facing
                new Vector3(0, 1, 0) // Direction of the upper edge of the camera
                );

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            // Fix the Depth
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = new DepthStencilState();

            // Draw Model
            model.Draw(world, view, projection);

            // Draw Information
            _spriteBatch.Begin();

            // Space & Tab Instructions
            _spriteBatch.DrawString(font, "SPACE: World = Scale * Rotation * Translation", new Vector2(50, 50), Color.Black);
            _spriteBatch.DrawString(font, "TAB: Current Camera Mode - " + cameraMode, new Vector2(50, 70), Color.Black);
            
            // Camera Movement Instructions
            _spriteBatch.DrawString(font, "WASD: Move Camera", new Vector2(50, 100), Color.Black);
            _spriteBatch.DrawString(font, "SHIFT + WASD: Move Camera Center", new Vector2(50, 120), Color.Black);
            _spriteBatch.DrawString(font, "CTRL + WASD: Change Camera Width/Height", new Vector2(50, 140), Color.Black);

            // Model Movement Instructions
            _spriteBatch.DrawString(font, "Arrow Kays: Move Model", new Vector2(50, 170), Color.Black);
            _spriteBatch.DrawString(font, "Insert/Delete: Model Yaw", new Vector2(50, 190), Color.Black);
            _spriteBatch.DrawString(font, "Home/End: Model Pitch", new Vector2(50, 210), Color.Black);
            _spriteBatch.DrawString(font, "PageUp/PageDown: Model Roll", new Vector2(50, 230), Color.Black);
            _spriteBatch.DrawString(font, "SHIFT + Up/Down: Scale Model", new Vector2(50, 250), Color.Black);

            // Camera Information
            _spriteBatch.DrawString(font, "Camera Position: (" + cameraPos.X.ToString("0.00") + "," + cameraPos.Y.ToString("0.00") + ")", new Vector2(500, 50), Color.Black);
            _spriteBatch.DrawString(font, "Camera Center: (" + cameraCenter.X.ToString("0.00") + "," + cameraCenter.Y.ToString("0.00") + ")", new Vector2(500, 70), Color.Black);
            _spriteBatch.DrawString(font, "Camera Width/Height: (" + cameraSize.X.ToString("0.00") + "," + cameraSize.Y.ToString("0.00") + ")", new Vector2(500, 90), Color.Black);

            // Model Information
            _spriteBatch.DrawString(font, "Model Position: (" + modelPos.X.ToString("0.00") + "," + modelPos.Y.ToString("0.00") + ")", new Vector2(500, 120), Color.Black);
            _spriteBatch.DrawString(font, "Model Rotation: (" + yaw.ToString("0.00") + "," + pitch.ToString("0.00") + roll.ToString("0.00") + ")", new Vector2(500, 140), Color.Black);
            _spriteBatch.DrawString(font, "Model Scale: (" + modelScale.ToString("0.00") + ")", new Vector2(500, 160), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}