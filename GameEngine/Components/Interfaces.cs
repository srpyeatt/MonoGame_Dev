using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CPI311.GameEngine.Components
{
    public interface IUpdateable { void Update(); }
    public interface IRenderable { void Draw(); }
    public interface IDrawable { void Draw(SpriteBatch spriteBatch); }
}
