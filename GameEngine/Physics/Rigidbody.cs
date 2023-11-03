using System;
using CPI311.GameEngine.Components;
using CPI311.GameEngine.Manager;
using Microsoft.Xna.Framework;

namespace CPI311.GameEngine.Physics
{
    public class Rigidbody : Component, Components.IUpdateable
    {
        public Vector3 Velocity { get; set; }
        public float Mass { get; set; }
        public Vector3 Acceleration { get; set; }
        public Vector3 Impulse { get; set; }

        public void Update()
        {
            Velocity += Acceleration * Time.ElapsedGameTime + Impulse / Mass;
            Transform.LocalPosition += Velocity * Time.ElapsedGameTime;
            Impulse = Vector3.Zero;
        }
    }
}
