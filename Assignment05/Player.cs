using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using CPI311.GameEngine.Components;
using CPI311.GameEngine.Rendering;
using CPI311.GameEngine.Physics;
using CPI311.GameEngine.Manager;
using System.IO;
using System.Collections.Generic;

namespace Assignment5
{
    public class Player : GameObject
    {
        public TerrainRenderer Terrain { get; set; }

        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            Terrain = terrain;
            // *** Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // *** SphereCollider
            SphereCollider collider = new SphereCollider();
            collider.Radius = 1;
            collider.Transform = Transform;
            Add<Collider>(collider);

            // *** Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(
                Content.Load<Model>("Sphere"), Transform, camera, Content, graphicsDevice, light,
                1, "SimpleShading", 20f, texture);
            Add<Renderer>(renderer);
        }

        public override void Update()
        {
            // Control the player
            if (InputManager.IsKeyDown(Keys.W)) // move forward
                this.Transform.LocalPosition += this.Transform.Forward * Time.ElapsedGameTime * 10f;
            if (InputManager.IsKeyDown(Keys.S)) // move backwars
                this.Transform.LocalPosition += this.Transform.Backward * Time.ElapsedGameTime * 10f;
            if (InputManager.IsKeyDown(Keys.A)) // move backwars
                this.Transform.LocalPosition += this.Transform.Left * Time.ElapsedGameTime * 10f;
            if (InputManager.IsKeyDown(Keys.D)) // move backwars
                this.Transform.LocalPosition += this.Transform.Right * Time.ElapsedGameTime * 10f;

            // change the Y position corresponding to the terrain (maze)
            this.Transform.LocalPosition = new Vector3(
                this.Transform.LocalPosition.X,
                Terrain.GetAltitude(this.Transform.LocalPosition),
                this.Transform.LocalPosition.Z) + Vector3.Up;

            base.Update();
        }

        public virtual bool CheckCollision(List<Transform> transforms, List<Collider> colliders, List<Rigidbody> rigidbodies)
        {
            Vector3 normal;
            for (int i = 0; i < transforms.Count; i++)
            {
                if (this.Get<Collider>().Collides(colliders[i], out normal))
                {
                    this.Transform.LocalPosition += normal;
                    return true;
                }
            }
            return false;
        }
    }
}
