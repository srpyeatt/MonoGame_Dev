using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using CPI311.GameEngine.Components;
using CPI311.GameEngine.Physics;
using CPI311.GameEngine.Rendering;
using CPI311.GameEngine.Manager;

namespace Assignment04
{
    public class Ship : GameObject
    {
        public Model Model;
        public Matrix[] Transforms;

        public Vector3 Position = Vector3.Zero;
        private const float VelocityScale = 5.0f;
        public bool isActive = true;

        public Vector3 direction;
        public float speed;

        public Vector3 Velocity = Vector3.Zero;
        public Matrix RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2);
        private float rotation;
        public float Rotation
        {
            get { return rotation; }
            set
            {
                float newVal = value;
                while (newVal >= MathHelper.TwoPi)
                {
                    newVal -= MathHelper.TwoPi;
                }
                while (newVal < 0)
                {
                    newVal += MathHelper.TwoPi;
                }
                if (rotation != newVal)
                {
                    rotation = newVal;
                    RotationMatrix = Matrix.CreateRotationX(MathHelper.PiOver2) *
                                                                Matrix.CreateRotationZ(rotation);
                }
            }
        }

        public Ship() { }

        public Ship(ContentManager Content, Camera camera, GraphicsDevice graphicDevice, Light light)
        {
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("p1_wedge1"), Transform, camera, Content, graphicDevice, light, 1, null, 20f, texture);
            Add<Renderer>(renderer);

            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius;
            Add<Collider>(sphereCollider);
        }

        public void Update(float delta)
        {
            Position += direction * speed * GameConstants.Playerspeed * delta;
            if (Position.X > GameConstants.PlayfieldSizeX) Position.X -= 2 * GameConstants.PlayfieldSizeX;
            if (Position.X < -GameConstants.PlayfieldSizeX) Position.X += 2 * GameConstants.PlayfieldSizeX;
            if (Position.Y > GameConstants.PlayfieldSizeY) Position.Y -= 2 * GameConstants.PlayfieldSizeY;
            if (Position.Y < -GameConstants.PlayfieldSizeY) Position.Y += 2 * GameConstants.PlayfieldSizeY;
        }

        public void Update(GameTime gameTime)
        {
            if (InputManager.IsKeyDown(Keys.W))
                Position += RotationMatrix.Forward * Time.ElapsedGameTime * GameConstants.Playerspeed;

            if (InputManager.IsKeyDown(Keys.A))
                Rotation += 0.1f;

            if (InputManager.IsKeyDown(Keys.S))
                Position += RotationMatrix.Backward * Time.ElapsedGameTime * GameConstants.Playerspeed;

            if (InputManager.IsKeyDown(Keys.D))
                Rotation -= 0.1f;

            base.Update();
        }
    }
}