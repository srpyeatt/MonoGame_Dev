using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading;
using CPI311.GameEngine;

namespace Assignment03
{
    public class Assignment03 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        BoxCollider boxCollider;

        List<Transform> transforms;
        List<Collider> colliders;
        List<Rigidbody> rigidbodies;
        List<Renderer> renderers;

        Random random;
        Model model;
        Camera camera;

        bool showDiagnostics = true;

        bool haveThreadRunning = false;
        int lastSecondCollisions = 0;
        int numberCollisions;
        SpriteFont font;

        Light light;
        Transform lightTransform;

        public Assignment03()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InputManager.Initialize();
            Time.Initialize();
            ScreenManager.Initialize(_graphics);

            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));

            random = new Random();
            transforms = new List<Transform>();
            rigidbodies = new List<Rigidbody>();
            colliders = new List<Collider>();
            boxCollider = new BoxCollider();
            boxCollider.Size = 10;

            renderers = new List<Renderer>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            model = Content.Load<Model>("Sphere");
            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 5;
            camera.Position = new Vector2(0f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            font = Content.Load<SpriteFont>("Font");

            // *** Lab07 Light *****
            lightTransform = new Transform();
            lightTransform.LocalPosition = Vector3.Backward * 10 + Vector3.Right * 5;

            light = new Light();
            light.Transform = lightTransform;
            // **********************
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);

            if (InputManager.IsKeyPressed(Keys.Right)) ;
            if (InputManager.IsKeyPressed(Keys.Left)) ;

            if (InputManager.IsKeyPressed(Keys.Up)) AddSphere();
            if (InputManager.IsKeyPressed(Keys.Left)) RemoveSphere();

            if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift))
            {
                if (showDiagnostics)
                {
                    showDiagnostics = false;
                }
                else
                {
                    showDiagnostics = true;
                }
            }
            if (InputManager.IsKeyPressed(Keys.Space)) ;
            if (InputManager.IsKeyPressed(Keys.LeftAlt) || InputManager.IsKeyPressed(Keys.RightAlt)) ;

            foreach (Rigidbody rigidbody in rigidbodies) rigidbody.Update();

            Vector3 normal;
            for (int i = 0; i < transforms.Count; i++)
            {
                if (boxCollider.Collides(colliders[i], out normal))
                {
                    numberCollisions++;
                    if (Vector3.Dot(normal, rigidbodies[i].Velocity) < 0)
                        rigidbodies[i].Impulse += Vector3.Dot(normal, rigidbodies[i].Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < transforms.Count; j++)
                {
                    if (colliders[i].Collides(colliders[j], out normal))
                        numberCollisions++;

                    Vector3 velocityNormal = Vector3.Dot(normal,
                        rigidbodies[i].Velocity - rigidbodies[j].Velocity) * -2
                        * normal * rigidbodies[i].Mass * rigidbodies[j].Mass;
                    rigidbodies[i].Impulse += velocityNormal / 2;
                    rigidbodies[j].Impulse += -velocityNormal / 2;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            for (int i = 0; i < renderers.Count; i++) renderers[i].Draw();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "RIGHT/LEFT: Speed", new Vector2(), Color.Black);
            _spriteBatch.DrawString(font, "UP/DOWN: # of Spheres", new Vector2(), Color.Black);
            _spriteBatch.DrawString(font, "SHIFT: Hide/Show Info", new Vector2(), Color.Black);
            _spriteBatch.DrawString(font, "SPACE: Speed Colors", new Vector2(), Color.Black);
            _spriteBatch.DrawString(font, "ALT: Toggle TExtures" + lastSecondCollisions, Vector2.Zero, Color.Black);
            
            if (showDiagnostics)
            {

            }
            else
            {

            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CollisionReset(Object obj)
        {
            while (haveThreadRunning)
            {
                lastSecondCollisions = numberCollisions;
                numberCollisions = 0;
                Thread.Sleep(1000);
            }
        }

        private void AddSphere()
        {
            // Step 1: Create Transform
            Transform transform = new Transform();
            transform.LocalPosition += Vector3.Right * 3;
            // Adding Render
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(model, transform, camera, Content, GraphicsDevice, light, 2, "SimpleShading", 20f, texture);
            renderers.Add(renderer);

            // Step 2: Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = transform;
            rigidbody.Mass = random.Next();

            Vector3 direction = new Vector3(
                (float)random.NextDouble(),
                (float)random.NextDouble(),
                (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity =
                direction * ((float)random.NextDouble() * 5 + 5);
            // Step 3: Collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * transform.LocalScale.Y;
            sphereCollider.Transform = transform;
            Debug.WriteLine(sphereCollider.Transform.World);
            // Step 4: Add to List
            transforms.Add(transform);
            colliders.Add(sphereCollider);
            rigidbodies.Add(rigidbody);
        }

        private void RemoveSphere()
        {

        }
    }
}