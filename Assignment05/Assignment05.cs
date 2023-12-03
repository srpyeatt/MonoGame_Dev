using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine.Rendering;
using CPI311.GameEngine.Components;
using CPI311.GameEngine.Manager;
using Assignment5;
using CPI311.GameEngine.Physics;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace Assignment05
{
    public class Assignment05 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        TerrainRenderer terrain;
        Effect effect;

        Camera camera;
        Light light;

        Player player;
        Agent agent;
        Agent agent2;
        Agent agent3;

        SpriteFont font;
        int hits = 0;

        Model model;
        private int gridSize = 20; //grid size
        List<Transform> transforms;
        List<Collider> colliders;
        List<Rigidbody> rigidbodies;

        Rigidbody wallRB;
        SphereCollider wallBC;
        Transform wallT;

        int touching = 0;

        public Assignment05()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);

            transforms = new List<Transform>();
            colliders = new List<Collider>();
            rigidbodies = new List<Rigidbody>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            terrain = new TerrainRenderer(Content.Load<Texture2D>("mazeH2"), Vector2.One * 100, Vector2.One * 200);
            terrain.NormalMap = Content.Load<Texture2D>("mazeN2");
            terrain.Transform = new Transform();
            terrain.Transform.LocalScale *= new Vector3(1, 5, 1);

            effect = Content.Load<Effect>("TerrainShader");
            effect.Parameters["AmbientColor"].SetValue(new Vector3(0.3f,0.3f,0.3f));
            effect.Parameters["DiffuseColor"].SetValue(new Vector3(0.1f,0.1f,0.1f));
            effect.Parameters["SpecularColor"].SetValue(new Vector3(0.2f,0.2f,0.2f));
            effect.Parameters["Shininess"].SetValue(20f);

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 50;
            camera.Transform.Rotate(Vector3.Left, MathHelper.PiOver2 - 0.2f);

            light = new Light();
            light.Transform = new Transform();
            light.Transform.LocalPosition = Vector3.Backward * 5 + Vector3.Right * 5 + Vector3.Up * 5;

            player = new Player(terrain, Content, camera, GraphicsDevice, light);
            agent = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent2 = new Agent(terrain, Content, camera, GraphicsDevice, light);
            agent3 = new Agent(terrain, Content, camera, GraphicsDevice, light);

            font = Content.Load<SpriteFont>("Font");
            model = Content.Load<Model>("Sphere");

            float gridW = terrain.size.X / gridSize;
            float gridH = terrain.size.Y / gridSize;
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    Vector3 pos = new Vector3(
                    gridW * i + gridW / 2 - terrain.size.X / 2,
                    0,
                        gridH * j + gridH / 2 - terrain.size.Y / 2);
                    if (terrain.GetAltitude(pos) > 1.0)
                    {
                        wallT = new Transform();
                        wallT.LocalPosition = pos;

                        wallRB = new Rigidbody();
                        wallRB.Transform = wallT;
                        wallRB.Mass = 1;

                        wallBC = new SphereCollider();
                        wallBC.Radius = 5;
                        wallBC.Transform = wallT;

                        transforms.Add(wallT);
                        colliders.Add(wallBC);
                        rigidbodies.Add(wallRB);
                    }
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Time.Update(gameTime);
            InputManager.Update();

            if (InputManager.IsKeyDown(Keys.Up))
                camera.Transform.Rotate(Vector3.Right, Time.ElapsedGameTime);
            if (InputManager.IsKeyDown(Keys.Down))
                camera.Transform.Rotate(Vector3.Left, Time.ElapsedGameTime);

            if (agent.CheckCollision(player))
                hits++;
            
            if (agent2.CheckCollision(player))
                hits++;
           
            if (agent3.CheckCollision(player))
                hits++;

            player.Update();
            agent.Update();
            agent2.Update();
            agent3.Update();

            player.CheckCollision(transforms, colliders, rigidbodies);

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
            effect.Parameters["LightPosition"].SetValue(light.Transform.Position);
            effect.Parameters["NormalMap"].SetValue(terrain.NormalMap);

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                terrain.Draw();
                player.Draw();
                agent.Draw();
                agent2.Draw();
                agent3.Draw();
            }

            //foreach (Transform transform in transforms)
            //   model.Draw(transform.World, camera.View, camera.Projection);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Hits: " + hits, new Vector2(50, 50), Color.Red);
            _spriteBatch.DrawString(font, "Time: " + Time.TotalGameTime.ToString(), new Vector2(50, 20), Color.Red);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}