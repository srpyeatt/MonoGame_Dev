using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPI311.GameEngine
{
    public class SpiralMover
    {
        // Properties
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }
        public float Radius { get; set; } 
        public float Speed { get; set; }
        public float Frequency { get; set; }
        public float Amplitude { get; set; }
        public float Phase { get; set; }

        // Methods
        public SpiralMover(Texture2D texture, Vector2 position, float radius = 150, float amplitude = 10, float frequency = 20, float speed = 1) 
        {
            Sprite = new Sprite(texture);
            Position = position;
            Radius = radius;
            Amplitude = amplitude;
            Frequency = frequency;
            Speed = speed;
            Sprite.Position = Position + new Vector2(Radius, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update()
        {
            Position = new Vector2(300, 300);
            Phase += Time.ElapsedGameTime * Speed; // 0.015"

            if (InputManager.IsKeyDown(Keys.Left)) Radius += 1;
            if (InputManager.IsKeyDown(Keys.Right)) Radius -= 1;
            if (InputManager.IsKeyDown(Keys.Up)) Speed += 0.01f;
            if (InputManager.IsKeyDown(Keys.Down)) Speed -= 0.01f;

            if (InputManager.IsKeyDown(Keys.W)) Amplitude += 1;
            if (InputManager.IsKeyDown(Keys.S)) Amplitude -= 1;
            if (InputManager.IsKeyDown(Keys.A)) Frequency += 1;
            if (InputManager.IsKeyDown(Keys.D)) Frequency -= 1;

            Sprite.Position = Position + new Vector2(
                (float)((Radius + Amplitude * Math.Cos(Phase * Frequency)) * Math.Cos(Phase)),
                (float)((Radius + Amplitude * Math.Cos(Phase * Frequency)) * Math.Sin(Phase))
                );
                //(float)(Radius * Math.Cos(Phase)),
                //(float)(Radius * Math.Sin(Phase)));
        }
    }
}
