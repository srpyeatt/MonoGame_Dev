using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;

namespace Assignment01
{
    public class Assignment01 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        AnimatedSprite player;

        int currDir = 0;
        Vector2 dir = new Vector2 (0,1);

        Sprite bonus;

        ProgressBar timeBar;
        ProgressBar timeBarBack;
        ProgressBar distanceWalk;
        ProgressBar distanceWalkBack;

        Random rnd = new Random();
        int rndX;
        int rndY;

        SpriteFont font;
        public Assignment01()
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
            player = new AnimatedSprite(Content.Load<Texture2D>("explorer"), 8, 0);
            timeBar = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Red);
            distanceWalk = new ProgressBar(Content.Load<Texture2D>("Square"), Color.Blue);
            timeBarBack = new ProgressBar(Content.Load<Texture2D>("Square"), Color.White);
            distanceWalkBack = new ProgressBar(Content.Load<Texture2D>("Square"), Color.White);

            bonus = new Sprite(Content.Load<Texture2D>("Square"));

            timeBar.Position = new Vector2(150,25);
            distanceWalk.Position = new Vector2(250,25);
            timeBarBack.Position = new Vector2(150, 25);
            distanceWalkBack.Position = new Vector2(250, 25);

            player.Position = new Vector2(400,400);

            bonus.Position = new Vector2(150,150);

            distanceWalk.Value = 0;
            font = Content.Load<SpriteFont>("font");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            InputManager.Update();
            Time.Update(gameTime);
            player.Update();

            timeBar.Value -= Time.ElapsedGameTime; 

            if (InputManager.IsKeyDown(Keys.W))
            {
                player.Position -= dir * 5;
                distanceWalk.Value += 0.05f;
            }

            if (InputManager.IsKeyPressed(Keys.D))
            {
                if (currDir == 0)
                {
                    dir = new Vector2(-1,0);
                    currDir = 1;
                    player.Section = 96;
                } 
                else if (currDir == 1)
                {
                    dir = new Vector2(0,-1);
                    currDir = 2;
                    player.Section = 32;
                } 
                else if (currDir == 2)
                {
                    dir = new Vector2(1,0);
                    currDir = 3;
                    player.Section = 64;
                }
                else if (currDir == 3)
                {
                    dir = new Vector2(0,1);
                    currDir = 0;
                    player.Section = 0;
                }
            }

            if (InputManager.IsKeyPressed(Keys.A))
            {
                if (currDir == 0)
                {
                    dir = new Vector2(1, 0);
                    currDir = 3;
                    player.Section = 64;
                }
                else if (currDir == 1)
                {
                    dir = new Vector2(0, 1);
                    currDir = 0;
                    player.Section = 0;
                }
                else if (currDir == 2)
                {
                    dir = new Vector2(-1, 0);
                    currDir = 1;
                    player.Section = 96;
                }
                else if (currDir == 3)
                {
                    dir = new Vector2(0, -1);
                    currDir = 2;
                    player.Section = 32;
                }
            }

            if (((player.Position - new Vector2(90,90))- bonus.Position).Length() < 32)
            {
                rndX = rnd.Next(64, GraphicsDevice.Viewport.Width);
                rndY = rnd.Next(64, GraphicsDevice.Viewport.Height);
                timeBar.Value += 5;
                bonus.Position = new Vector2(rndX, rndY);
                if (timeBar.Value > 64)
                {
                    timeBar.Value = 64;
                }
            }

            if (timeBar.Value <= 0) { Exit(); }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //sprite.Draw(_spriteBatch);
            player.Draw(_spriteBatch);

            timeBarBack.Draw(_spriteBatch);
            distanceWalkBack.Draw(_spriteBatch);
            timeBar.Draw(_spriteBatch);
            distanceWalk.Draw(_spriteBatch);

            bonus.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}