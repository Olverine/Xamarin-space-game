using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace App1
{
    class Coin : GameObject
    {
        public Coin()
        {
            // Randomize the position of the coin
            Random r = new Random();
            position = new Vector(r.Next((int)-Bounds.size.x, (int)Bounds.size.x), r.Next((int)-Bounds.size.y, (int)Bounds.size.y));

            // Arrange the vertices in a circle
            vertices = new float[11 * 3];
            for (int i = 1; i < vertices.Length / 3; i++)
            {
                vertices[i * 3] = (float)Math.Cos(i * (360 / ((vertices.Length / 3) - 1) + (360 / ((vertices.Length / 3) - 1) / ((vertices.Length / 3) - 2))) / (180 / Math.PI));
                vertices[(i * 3) + 1] = (float)Math.Sin(i * (360 / ((vertices.Length / 3) - 1) + (360 / ((vertices.Length / 3) - 1) / ((vertices.Length / 3) - 2))) / (180 / Math.PI));
            }

            // Set the color of each vertex
            colors = new byte[]
            {
                255, 255, 255, 255,
                255, 0, 255, 255,
                255, 128, 0, 255,
                255, 128, 0, 255,
                0, 255, 0, 255,
                0, 255, 0, 255,
                0, 255, 255, 255,
                0, 255, 255, 255,
                255, 255, 0, 255,
                255, 255, 0, 255,
                255, 0, 255, 255
            };

            DrawMode = OpenTK.Graphics.ES11.All.TriangleFan;    // Draw as a triangle fan
        }

        public override void Tick()
        {
            rotation += new Vector(5, 0, 0);                    // Rotate the coin

            // Check if the coin is colliding with the spaceship
            SpaceShip player = GLView1.GetSpaceShip();
            if(CheckCollision(player, 1.5))
            {
                GLView1.Spawn(new Coin());                      // Spawn a new coin
                GLView1.Spawn(new Enemy(position));                     // Spawn a new enemy
                GLView1.Spawn(new ParticleSystem(position, 100, 1, 120, 8, null));  //Spawn a particle system
                GLView1.IncrementScore();
                GLView1.Remove(this);                           // Remove the coin
            }
        }
    }
}