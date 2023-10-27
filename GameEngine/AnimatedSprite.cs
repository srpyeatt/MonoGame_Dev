using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CPI311.GameEngine
{
    public class AnimatedSprite : Sprite
    {
        public Texture2D Texture2D { get; set; }
        public int Frames { get; set; }
        public float Frame { get; set; }
        public float Speed { get; set; }
        public int Section { get; set; }

        int index;

        public AnimatedSprite(Texture2D texture, int frames, int section) : base(texture)
        {
            Texture = texture;
            Frames = frames;
            Frame = 0;
            Speed = 2f;
            Section = section;
        }

        public override void Update()
        {
            Frame += (Speed * Time.ElapsedGameTime); 
            index = (int)Math.Floor(Frame) % Frames;
            Source = new Rectangle(index*32, Section, 32, 32);
        }
    }
}
