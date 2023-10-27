using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine
{
    public class ProgressBar : Sprite
    {
        public Color FillColor { get; set; }
        public float Value { get; set; }

        public ProgressBar(Texture2D texture, Color col) : base(texture)
        {
            FillColor = col;
            Value = 32;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(Texture, Position, new Rectangle(0,0,(int)Value,32),
                FillColor, Rotation, Origin, new Vector2(2,1), Effect, Layer);
        }
    }
}
