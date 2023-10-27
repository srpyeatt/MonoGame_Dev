using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;

namespace Assignment02
{
    public class Assignment02 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Camera 
        Camera camera;
        Transform cameraTransform;
        Camera cameraFirst;
        Camera cameraThird;
        Transform cameraFTransform;
        Transform cameraTTransform;
        private bool isToggleCamera, canMove = true;

        // Elements
        Model sunModel;
        Model earthModel;
        Model moonModel;
        Model mercuryModel;
        Transform sunTransform;
        Transform earthTransform;
        Transform moonTransform;
        Transform mercuryTransform;

        Model platformModel;
        Transform platformTransform;

        private float animSpeed = 1.0f;

        public Assignment02()
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
            // Camera Content
            cameraFTransform = new Transform();
            cameraFTransform.LocalPosition = Vector3.Backward * 5 + Vector3.Up * 2;
            cameraFirst = new Camera();
            cameraFirst.Transform = cameraFTransform;

            cameraTTransform = new Transform();
            cameraTTransform.LocalPosition = Vector3.Backward * 5 + Vector3.Up * 75;
            cameraTTransform.LocalRotation = Quaternion.CreateFromAxisAngle(Vector3.Right, 5);
            cameraThird = new Camera();
            cameraThird.Transform = cameraTTransform;

            // Element Conents
            sunModel = Content.Load<Model>("sun");
            earthModel = Content.Load<Model>("earth");
            moonModel = Content.Load<Model>("moon");
            mercuryModel = Content.Load<Model>("mercury");

            sunTransform = new Transform();
            earthTransform = new Transform();
            moonTransform = new Transform();
            mercuryTransform = new Transform();

            sunTransform.Position = new Vector3(0,25,0);
            earthTransform.Position = Vector3.Left * 20;
            moonTransform.Position = Vector3.Up * 20;
            mercuryTransform.Position = Vector3.Right * 20;

            // Parenting Elements
            mercuryTransform.Parent = sunTransform;
            earthTransform.Parent = sunTransform;
            moonTransform.Parent = earthTransform;

            platformModel = Content.Load<Model>("plane");
            platformTransform = new Transform();
            platformTransform.Position = new Vector3(0, 0, 0);

            // Lighting Effect ON
            foreach (ModelMesh mesh in earthModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in sunModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in moonModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in mercuryModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
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

            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);

            // Change Speed
            if (InputManager.IsKeyPressed(Keys.J))
                animSpeed += 0.1f;
            if (InputManager.IsKeyPressed(Keys.L))
                animSpeed -= 0.1f;

            // Planet Rotation
            sunTransform.Rotate(Vector3.Up, Time.ElapsedGameTime * animSpeed);
            earthTransform.Rotate(Vector3.Right, Time.ElapsedGameTime * (2f*animSpeed));

            // Perspective Toggle
            if (InputManager.IsKeyPressed(Keys.Tab))
                isToggleCamera = !isToggleCamera;

            if (isToggleCamera)
            {
                // Third-Person Perspective
                camera = cameraThird;
                cameraTransform = cameraTTransform;
                canMove = true;
            }
            else
            {
                // First-Person Perspective
                camera = cameraFirst;
                cameraTransform = cameraFTransform;
                canMove = false;
            }

            // Zoom In/Out
            if (InputManager.IsKeyPressed(Keys.I))
                camera.FieldOfView -= 0.1f;
            if (InputManager.IsKeyPressed(Keys.K))
                camera.FieldOfView += 0.1f;

            // First-Person Movement
            if (!canMove)
            {
                // Keyboard Movement
                if (InputManager.IsKeyDown(Keys.W))
                    cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime * 5;
                if (InputManager.IsKeyDown(Keys.S))
                    cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime * 5;
                if (InputManager.IsKeyDown(Keys.A))
                    cameraTransform.LocalPosition += cameraTransform.Left * Time.ElapsedGameTime * 5;
                if (InputManager.IsKeyDown(Keys.D))
                    cameraTransform.LocalPosition += cameraTransform.Right * Time.ElapsedGameTime * 5;

                if (InputManager.IsKeyDown(Keys.Up))
                    cameraTransform.Rotate(Vector3.Right, Time.ElapsedGameTime);
                if (InputManager.IsKeyDown(Keys.Down))
                    cameraTransform.Rotate(Vector3.Left, Time.ElapsedGameTime);

                // Mouse Movement
                if (InputManager.LeftButtonDown())
                    cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
                if (InputManager.RightButtonDown())
                    cameraTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            sunModel.Draw(sunTransform.World, camera.View, camera.Projection);
            earthModel.Draw(earthTransform.World, camera.View, camera.Projection);
            moonModel.Draw(moonTransform.World, camera.View, camera.Projection);
            mercuryModel.Draw(mercuryTransform.World, camera.View, camera.Projection);
            platformModel.Draw(platformTransform.World, camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}