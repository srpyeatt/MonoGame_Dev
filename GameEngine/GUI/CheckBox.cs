﻿using CPI311.GameEngine.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CPI311.GameEngine.GUI
{
    public class CheckBox : GUIElement
    {
        public Texture2D Box { get; set; }
        public bool Checked { get; set; }
        public override void Update()
        {
            if (InputManager.IsMouseReleased(0) && Bounds.Contains(InputManager.GetMousePosition()))
            {
                Checked = !Checked;
                OnAction();
            }
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            base.Draw(spriteBatch, font);
            int width = Math.Min(Bounds.Width, Bounds.Height);
            spriteBatch.Draw(Box, new Rectangle(Bounds.X, Bounds.Y, width, width), Checked ? Color.Red : Color.White);
            spriteBatch.DrawString(font, Text, new Vector2(Bounds.X + width, Bounds.Y), Color.Black);
        }
    }
}