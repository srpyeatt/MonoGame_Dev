﻿using CPI311.GameEngine.Manager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CPI311.GameEngine.Components
{
    public class Camera : Component
    {
        // Properties
        public float FieldOfView { get; set; }
        public float AspectRatio { get; set; }
        public float NearPlane { get; set; }
        public float FarPlane { get; set; }
        public Transform Transform { get; set; }

        // *** Lab08 ***
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Viewport Viewport
        {
            get
            {
                return new Viewport((int)(ScreenManager.Width * Position.X),
                                (int)(ScreenManager.Height * Position.Y),
                                (int)(ScreenManager.Width * Size.X),
                                (int)(ScreenManager.Height * Size.Y));
            }
        }
        public Ray ScreenPointToWorldRay(Vector2 position)
        {
            Vector3 start = Viewport.Unproject(new Vector3(position, 0),
                Projection, View, Matrix.Identity);
            Vector3 end = Viewport.Unproject(new Vector3(position, 1),
                Projection, View, Matrix.Identity);
            return new Ray(start, end - start);
        }
        // ************

        public Matrix Projection
        {
            get
            {
                return Matrix.CreatePerspectiveFieldOfView(
                FieldOfView, AspectRatio, NearPlane, FarPlane);
            }
        }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(
                    Transform.LocalPosition,
                    Transform.LocalPosition + Transform.Forward,
                    Transform.Up);
            }
        }

        // Constrctor
        public Camera()
        {
            FieldOfView = MathHelper.PiOver2;
            AspectRatio = 1.33f;
            NearPlane = 0.1f;
            FarPlane = 100f;
            Transform = null; // Need to initialize in Game Class
        }
    }
}
