using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Threading; // For Multi Threads
using CPI311.GameEngine.Manager;
using CPI311.GameEngine.Physics;
using CPI311.GameEngine.Rendering;
using CPI311.GameEngine.Components;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Assignment03
{
    public class Assignment03 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Camera camera;
        Light light;
        Transform lightTransform;

        BoxCollider boxCollider;
        List<GameObject> gameObjects;
        Random random;

        Model model;
        SpriteFont font;
        Texture2D texture;
        Effect effect;

        int numberCollisions;
        float speed;
        bool showDiagnostics = true;
        bool showSpeed = false;
        bool textureEnabled = true;
        Timer timer;
        static float frames;
        static float fps;

        bool haveThreadRunning = false;
        int lastSecondCollisions = 0;

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

            boxCollider = new BoxCollider();
            boxCollider.Size = 10;
            numberCollisions = 0;

            gameObjects = new List<GameObject>();
            timer = new Timer(1000);
            frames = 0;
            fps = 0;
            random = new Random();
            speed = 1f;

            timer.AutoReset = true;
            timer.Enabled = true;
            timer.Elapsed += OnTimedEvent;

            haveThreadRunning = true;
            ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));
            

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            model = Content.Load<Model>("Sphere");
            font = Content.Load<SpriteFont>("Font");
            texture = Content.Load<Texture2D>("Square");
            effect = Content.Load<Effect>("SimpleShading");

            camera = new Camera();
            camera.Transform = new Transform();
            camera.Transform.LocalPosition = Vector3.Backward * 20;
            camera.Position = new Vector2(0f, 0f);
            camera.Size = new Vector2(0.5f, 1f);
            camera.AspectRatio = camera.Viewport.AspectRatio;

            light = new Light();
            lightTransform = new Transform();
            lightTransform.LocalPosition = Vector3.Backward * 10 + Vector3.Right * 5;
            light.Transform = lightTransform;

            for (int i = 0; i < 5; i++)
            {
                AddGameObject();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
            }

            frames++;

            if (InputManager.IsKeyDown(Keys.Left))
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.Get<Rigidbody>().Velocity *= .95f;
                speed -= .005f;
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                foreach (GameObject gameObject in gameObjects)
                    gameObject.Get<Rigidbody>().Velocity *= 1.05f;
                speed += .005f;
            }

            if (InputManager.IsKeyPressed(Keys.Up))
            {
                speed = 1f;
                AddGameObject();
            }
            if (InputManager.IsKeyPressed(Keys.Down))
            {
                if (gameObjects.Count > 0) gameObjects.RemoveAt(0);
            }

            if (InputManager.IsKeyPressed(Keys.LeftShift) || InputManager.IsKeyPressed(Keys.RightShift)) { showDiagnostics = !showDiagnostics; }
            
            if (InputManager.IsKeyPressed(Keys.Space)) { showSpeed = !showSpeed; }
            
            if (InputManager.IsKeyPressed(Keys.LeftAlt) || InputManager.IsKeyPressed(Keys.RightAlt))
            {
                textureEnabled = !textureEnabled;
                foreach (GameObject gameObject in gameObjects)
                    if (textureEnabled == true)
                    {
                        Renderer renderer = new Renderer(model, gameObject.Transform, camera, Content, GraphicsDevice, light, 2, "SimpleShading", 20f, texture);
                        gameObject.Add<Renderer>(renderer);
                    }
                    else
                    {
                        Renderer renderer = new Renderer(model, gameObject.Transform, camera, Content, GraphicsDevice, light, 2, "SimpleShading", 20f, null);
                        gameObject.Add<Renderer>(renderer);
                    }
            }

            if (InputManager.IsKeyPressed(Keys.M))
            {
                haveThreadRunning = !haveThreadRunning;
                ThreadPool.QueueUserWorkItem(new WaitCallback(CollisionReset));
            }

            Vector3 normal;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (boxCollider.Collides(gameObjects[i].Get<Collider>(), out normal))
                {
                    numberCollisions++;
                    if (Vector3.Dot(normal, gameObjects[i].Get<Rigidbody>().Velocity) < 0)
                        gameObjects[i].Get<Rigidbody>().Impulse +=
                            Vector3.Dot(normal, gameObjects[i].Get<Rigidbody>().Velocity) * -2 * normal;
                }
                for (int j = i + 1; j < gameObjects.Count; j++)
                {
                    if (gameObjects[i].Get<Collider>().Collides(gameObjects[j].Get<Collider>(), out normal))
                    numberCollisions++;

                    Vector3 velocityNormal = Vector3.Dot(normal,
                        gameObjects[i].Get<Rigidbody>().Velocity - gameObjects[j].Get<Rigidbody>().Velocity) * -2
                        * normal * gameObjects[i].Get<Rigidbody>().Mass * gameObjects[j].Get<Rigidbody>().Mass;
                    gameObjects[i].Get<Rigidbody>().Impulse += velocityNormal / 2;
                    gameObjects[j].Get<Rigidbody>().Impulse += -velocityNormal / 2;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw();
            }

            if (showSpeed == true)
            {
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    Transform transform = gameObjects[i].Get<Rigidbody>().Transform;
                    float speed = gameObjects[i].Get<Rigidbody>().Velocity.Length();
                    float speedValue = MathHelper.Clamp(speed / 50f, 0, 1);
                    (model.Meshes[0].Effects[0] as BasicEffect).DiffuseColor =
                                               new Vector3(speedValue, speedValue, 1);

                    model.Draw(transform.World, camera.View, camera.Projection);
                }
            }

            _spriteBatch.DrawString(font, "Controls", new Vector2(10, 10), Color.Black);
            _spriteBatch.DrawString(font, "RIGHT/LEFT: Speed", new Vector2(10, 30), Color.Black);
            _spriteBatch.DrawString(font, "UP/DOWN: Number of Spheres", new Vector2(10, 50), Color.Black);
            _spriteBatch.DrawString(font, "SHIFT: Hide/Show Info", new Vector2(10, 70), Color.Black);
            _spriteBatch.DrawString(font, "SPACE: Speed Colors", new Vector2(10, 90), Color.Black);
            _spriteBatch.DrawString(font, "ALT: Toggle Textures", new Vector2(10, 110), Color.Black);
            _spriteBatch.DrawString(font, "M: Toggle MultiThreading", new Vector2(10, 130), Color.Black);
            _spriteBatch.DrawString(font, "Average Collisions: " + lastSecondCollisions, new Vector2(10, 150), Color.Black);

            if (showDiagnostics == true)
            {
                _spriteBatch.DrawString(font, "Animation Speed: " + speed, new Vector2(10, 200), Color.Black);
                _spriteBatch.DrawString(font, "Show Speed Color: " + showSpeed, new Vector2(10, 220), Color.Black);
                _spriteBatch.DrawString(font, "Show Textures: " + textureEnabled, new Vector2(10, 240), Color.Black);
                _spriteBatch.DrawString(font, "MultiThreading: " + haveThreadRunning, new Vector2(10, 260), Color.Black);
                _spriteBatch.DrawString(font, "FPS: " + fps, new Vector2(10, 280), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            fps = frames;
            Debug.Print(fps.ToString());
            frames = 0;
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

        private void AddGameObject()
        {
            GameObject gameObject = new GameObject();
            gameObject.Transform.LocalPosition += Vector3.Right * 10 * (float)random.NextDouble();
            
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = gameObject.Transform;
            rigidbody.Mass = 1;
            gameObject.Add<Rigidbody>(rigidbody);
            Vector3 direction = new Vector3(
              (float)random.NextDouble(), (float)random.NextDouble(),
              (float)random.NextDouble());
            direction.Normalize();
            rigidbody.Velocity = direction * ((float)random.NextDouble() * 5 + 5);
            gameObject.Add<Rigidbody>(rigidbody);

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f * gameObject.Transform.LocalScale.Y;
            sphereCollider.Transform = gameObject.Transform;
            gameObject.Add<Collider>(sphereCollider);

            if (textureEnabled == true)
            {
                Renderer renderer = new Renderer(model, gameObject.Transform, camera, Content, GraphicsDevice, light, 2, "SimpleShading", 20f, texture);
                gameObject.Add<Renderer>(renderer);
            }
            else
            {
                Renderer renderer = new Renderer(model, gameObject.Transform, camera, Content, GraphicsDevice, light, 2, "SimpleShading", 20f, null);
                gameObject.Add<Renderer>(renderer);
            }

            gameObjects.Add(gameObject);
        }
    }
}