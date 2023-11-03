using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;
using CPI311.GameEngine.AI;
using CPI311.GameEngine.Manager;
using CPI311.GameEngine.Components;

namespace Lab09
{
    public class Lab09 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Model cube;
        Model sphere;
        Camera camera;
        AStarSearch search;
        List<Vector3> path;

        Random ran = new Random();
        private int size = 10;

        public Lab09()
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
            ScreenManager.Initialize(_graphics);

            search = new AStarSearch(size, size); // size of grid
            foreach (AStarNode node in search.Nodes)
                if (ran.NextDouble() < 0.2)
                    search.Nodes[ran.Next(size), ran.Next(size)].Passable = false;
            search.Start = search.Nodes[0, 0];
            search.Start.Passable = true;
            search.End = search.Nodes[size - 1, size - 1];
            search.End.Passable = true;

            search.Search(); // A search is made here.
            path = new List<Vector3>();
            AStarNode current = search.End;
            while (current != null)
            {
                path.Insert(0, current.Position);
                current = current.Parent;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            cube = Content.Load<Model>("cube");
            sphere = Content.Load<Model>("Sphere");

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Up * 13 + Vector3.Right * 4 + Vector3.Backward * 4;
            camera.Transform.Rotate(Vector3.Right, -MathHelper.PiOver2);
            camera.Position = new Vector2(0f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            Time.Update(gameTime);
            InputManager.Update();

            if (InputManager.IsKeyPressed(Keys.Space))
            {
                search.Start = search.Nodes[ran.Next(0,size), ran.Next(0,size)];
                search.End = search.Nodes[ran.Next(0, size - 1), ran.Next(0, size - 1)];
                search.Search();
                path.Clear();
                AStarNode current = search.End;
                while (current != null)
                {
                    path.Insert(0, current.Position);
                    current = current.Parent;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            foreach (AStarNode node in search.Nodes)
            {
                if (!node.Passable)
                    cube.Draw(Matrix.CreateScale(0.5f, 0.05f, 0.5f) * Matrix.CreateTranslation(node.Position), camera.View, camera.Projection);
            }
            foreach (Vector3 position in path)
            {
                sphere.Draw(Matrix.CreateScale(0.1f, 0.1f, 0.1f) * Matrix.CreateTranslation(position), camera.View, camera.Projection);
            }

            base.Draw(gameTime);
        }
    }
}