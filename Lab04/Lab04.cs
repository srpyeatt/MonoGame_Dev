using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine.Manager;
using CPI311.GameEngine.Components;

namespace Lab04
{
    public class Lab04 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model model;
        Camera camera;
        Transform modelTransform;
        Transform cameraTransform;

        // *** Lab04-C ***
        Model parentModel;
        Transform parentTransform;
        // ***************

        public Lab04()
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
            model = Content.Load<Model>("Torus");
            modelTransform = new Transform();
            modelTransform.LocalPosition = Vector3.Left * 5;
            cameraTransform = new Transform();
            cameraTransform.LocalPosition = Vector3.Backward * 5 + Vector3.Up*3;
            camera = new Camera();
            camera.Transform = cameraTransform;

            // *** Lab04-C ***
            parentModel = Content.Load<Model>("Sphere");
            parentTransform = new Transform();
            parentTransform.LocalPosition = Vector3.Right * 5;

            // Question how to make the Torus as a Child of Sphere
            // Parenting (model & parent) here!
            modelTransform.Parent = parentTransform;
            // ***************

            // Lighting Effect ON
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                }
            }
            foreach (ModelMesh mesh in parentModel.Meshes)
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

            if (InputManager.IsKeyDown(Keys.W))
                cameraTransform.LocalPosition += cameraTransform.Forward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.S))
                cameraTransform.LocalPosition += cameraTransform.Backward * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.A))
                cameraTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.D))
                cameraTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);

            // *** Lab04-C ***
            if (InputManager.IsKeyDown(Keys.Up))
                parentTransform.LocalPosition += parentTransform.Up * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Down))
                parentTransform.LocalPosition += parentTransform.Down * Time.ElapsedGameTime;
            if (InputManager.IsKeyDown(Keys.Left))
                parentTransform.Rotate(Vector3.Up, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Right))
                parentTransform.Rotate(Vector3.Down, Time.ElapsedGameTime);
            // *********************

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            model.Draw(modelTransform.World, camera.View, camera.Projection);
            parentModel.Draw(parentTransform.World, camera.View, camera.Projection);

            base.Draw(gameTime);
        }
    }
}