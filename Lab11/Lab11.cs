using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine.GUI;
using CPI311.GameEngine.Manager;
using System.Collections.Generic;
using SharpDX.Direct3D9;

namespace Lab11
{
    public class Lab11 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D texture;
        SpriteFont font;
        Color background = Color.White;
        //Button exitButton;

        Dictionary<string, Scene> scenes;
        Scene currScene;

        List<GUIElement> elements;
        CheckBox checkBox;
        Button fullButton;

        public Lab11()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Time.Initialize();
            InputManager.Initialize();
            ScreenManager.Initialize(_graphics);

            scenes = new Dictionary<string, Scene>();
            elements = new List<GUIElement>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            texture = Content.Load<Texture2D>("Square");
            font = Content.Load<SpriteFont>("font");

            scenes.Add("Menu", new Scene(MainMenuUpdate, MainMenuDraw));
            scenes.Add("Play", new Scene(PlayUpdate, PlayDraw));
            currScene = scenes["Menu"];

            /*
            exitButton = new Button();
            exitButton.Texture = texture;
            exitButton.Text = "Exit";
            exitButton.Bounds = new Rectangle(50, 50, 300, 20);
            exitButton.Action += ExitGame;
            */

            checkBox = new CheckBox();
            checkBox.Box = texture;
            checkBox.Text = "Switch Scene";
            checkBox.Bounds = new Rectangle(50,50,300,50);
            checkBox.Action += SwitchScene;
            elements.Add(checkBox);

            fullButton = new Button();
            fullButton.Texture = texture;
            fullButton.Text = "Fullscreen Mode";
            fullButton.Bounds = new Rectangle(50, 200, 300, 20);
            fullButton.Action += Fullscreen;
            elements.Add(fullButton);
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();

            // TODO: Add your update logic here
            Time.Update(gameTime);
            InputManager.Update();

            //exitButton.Update();

            currScene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(background);

            // TODO: Add your drawing code here
            //_spriteBatch.Begin();
            //exitButton.Draw(_spriteBatch, font);
            //_spriteBatch.End();

            currScene.Draw();

            base.Draw(gameTime); 
        }

        /*
         * void ExitGame(GUIElement element)
        {
            background = (background == Color.White ? Color.Blue : Color.White);
        }
        */
        void MainMenuUpdate()
        {
            foreach (GUIElement element in elements)
                element.Update();
        }
        void MainMenuDraw()
        {
            _spriteBatch.Begin();
            foreach (GUIElement element in elements)
                element.Draw(_spriteBatch, font);
            _spriteBatch.End();
        }
        void PlayUpdate()
        {
            if (InputManager.IsKeyReleased(Keys.Escape))
                currScene = scenes["Menu"];
        }
        void PlayDraw()
        {
            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "Play Mode! Press \"Esc\" to go back", Vector2.Zero, Color.Black);
            _spriteBatch.End();
        }
        void SwitchScene(GUIElement element)
        {
            currScene = scenes["Play"];
            background = (background == Color.White ? Color.Blue : Color.White);
        }
        void Fullscreen(GUIElement element)
        {
            //ScreenManager.Setup(1280, 720);
            ScreenManager.IsFullScreen = !ScreenManager.IsFullScreen;
        }
    }
}