using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CPI311.GameEngine.AI;
using CPI311.GameEngine.Components;
using CPI311.GameEngine.Rendering;
using CPI311.GameEngine.Physics;
//using System.Reflection.Metadata;

namespace Assignment5
{
    public class Agent : GameObject
    {
        public AStarSearch search;
        List<Vector3> path;

        private float speed = 5f; //moving speed
        private int gridSize = 20; //grid size
        private TerrainRenderer Terrain;

        public Agent(TerrainRenderer terrain, ContentManager Content, Camera camera, 
            GraphicsDevice graphicsDevice, Light light) : base()
        {
            Terrain = terrain;
            path = null;  

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

            // *** Make a path ***
            search = new AStarSearch(gridSize, gridSize); // A Star Search has the same grid size
            float gridW = Terrain.size.X / gridSize;
            float gridH = Terrain.size.Y / gridSize;
            for (int i = 0; i < gridSize; i++)
                for (int j = 0; j < gridSize; j++)
                {
                    Vector3 pos = new Vector3(
                        gridW * i + gridW / 2 - Terrain.size.X / 2,
                        0,
                        gridH * j + gridH / 2 - Terrain.size.Y / 2);
                    if (Terrain.GetAltitude(pos) > 1.0)
                        search.Nodes[j, i].Passable = false;
                }
        }

        public override void Update()
        {
            if (path != null && path.Count > 0)
            {
                // Move to the destination along the path
                Vector3 currP = Transform.Position;
                Vector3 destP = GetGridPosition(path[0]);
                Debug.WriteLine("Path 0 is " + path[0]);
                currP.Y = 0;
                destP.Y = 0;

                Vector3 direction = Vector3.Distance(currP, destP) == 0? 
                    Vector3.Zero : 
                    Vector3.Normalize(destP - currP);
                this.Rigidbody.Velocity = new Vector3(direction.X, 0, direction.Z) * speed;
                //Debug.WriteLine("{0}, {1}", currP, destP);
                if (Vector3.Distance(currP,destP) < 1f) // if it reaches to a point, go to the next in path
                {
                    path.RemoveAt(0);  // path[0] is removed.
                    if (path.Count == 0) // if it reached to the goal
                    {
                        path = null;
                        return;
                    }
                }
            }
            else
            {
                // Search again to make a new path.
                RandomPathFinding();
            }

            this.Transform.LocalPosition = new Vector3(
               this.Transform.LocalPosition.X,
               Terrain.GetAltitude(this.Transform.LocalPosition),
               this.Transform.LocalPosition.Z) + Vector3.Up;
            Transform.Update();

            base.Update();
        }

        private Vector3 GetGridPosition(Vector3 gridPos)
        {
            float gridW = Terrain.size.X / search.Cols;
            float gridH = Terrain.size.Y / search.Rows;
            return new Vector3(gridW * gridPos.X + gridW / 2 - Terrain.size.X / 2,
                0,
                gridH * gridPos.Z + gridH / 2 - Terrain.size.Y / 2);
        }
        public void RandomPathFinding()
        {
            Random random = new Random();
            while (!(search.Start = search.Nodes[random.Next(search.Rows),random.Next(search.Cols)]).Passable) ;
            search.End = search.Nodes[search.Rows / 2, search.Cols / 2]; // center position 
            search.Search();
            path = new List<Vector3>();
            AStarNode current = search.End;
            var count = 0;
            while (current != null)  // List class should have reverse function!
            {
                Vector3 normal;count++;
                path.Insert(0, current.Position);
                current = current.Parent;
            }
            // Moved from the Update() method
            Transform.LocalPosition = GetGridPosition(path[0]); // move to the start position
        }
        public virtual bool CheckCollision(Player player)
        {
            Vector3 normal;
            if (player.Get<Collider>().Collides(this.Get<Collider>(), out normal))
            {
                path = null;
                return true;
            }
            return false;
        }
    }
}
