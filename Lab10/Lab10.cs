using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine.Components;
using CPI311.GameEngine.Rendering;
using CPI311.GameEngine.Manager;

namespace Lab10
{
    public class Lab10 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        TerrainRenderer terrain;
        Camera camera;
        Effect effect;

        public Lab10()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ScreenManager.Initialize(_graphics);
            InputManager.Initialize();
            Time.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            terrain = new TerrainRenderer(
                Content.Load<Texture2D>("Heightmap"),
                Vector2.One * 100, Vector2.One * 200); // (100,100) size, (200,200) resolution

            terrain.NormalMap = Content.Load<Texture2D>("Normalmap");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);

            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.1f,0.1f,0.1f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.3f, 0.1f, 0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0f, 0f, 0.2f));
            effect.Parameters["Shininess"].SetValue(20f);

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 3 + Vector3.Up * 5;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);

            if (InputManager.IsKeyDown(Keys.W)) // move forward
                camera.Transform.LocalPosition += camera.Transform.Forward * Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.A)) // move left
                camera.Transform.LocalPosition += camera.Transform.Left * Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.S)) // move backward
                camera.Transform.LocalPosition += camera.Transform.Backward * Time.ElapsedGameTime * 10;
            if (InputManager.IsKeyDown(Keys.D)) // move right
                camera.Transform.LocalPosition += camera.Transform.Right * Time.ElapsedGameTime * 10;

            camera.Transform.LocalPosition = new Vector3(
                camera.Transform.LocalPosition.X,
                terrain.GetAltitude(camera.Transform.LocalPosition),
                camera.Transform.LocalPosition.Z) + Vector3.Up;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            effect.Parameters["View"].SetValue(camera.View);
            effect.Parameters["Projection"].SetValue(camera.Projection);
            effect.Parameters["World"].SetValue(terrain.Transform.World);
            effect.Parameters["CameraPosition"].SetValue(camera.Transform.Position);
            effect.Parameters["LightPosition"].SetValue(camera.Transform.Position + Vector3.Up * 10);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            foreach(EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
            }


            base.Draw(gameTime);
        }
    }
}