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
    class Enemy : GameObject
    {
        /// <summary>
        /// The speed of the enemy
        /// </summary>
        Vector speed = new Vector((float)Math.Cos(45) * 0.1f, (float)Math.Sin(45) * 0.1f);

        /// <summary>
        /// Is this enemy moving?
        /// Can it kill the player?
        /// </summary>
        bool Hunting;

        /// <summary>
        /// The number of ticks that passes before the enemy starts hunting
        /// </summary>
        int timer = 60;

        /// <summary>
        /// The color of the Enemy when it's hunting
        /// </summary>
        byte[] HuntingColor;

        public Enemy(Vector position)
        {
            this.position = position;
            // Set the vertices of the shape
            vertices = new float[]
            {
                -1, -1, -1,
                1, -1, -1,
                1, -1, -1,
                1, 1, -1,
                1, 1, -1,
                -1, 1, -1,
                -1, 1, -1,
                -1, -1, -1,
                -1, -1, 1,
                1, -1, 1,
                1, -1, 1,
                1, 1, 1,
                1, 1, 1,
                -1, 1, 1,
                -1, 1, 1,
                -1, -1, 1,
                -1, -1, 1,
                -1, -1, -1,
                1, -1, 1,
                1, -1, -1,
                1, 1, 1,
                1, 1, -1,
                -1, 1, 1,
                -1, 1, -1
            };

            // Set the color of the vertices
            colors = new byte[96];
            for(int i = 0; i < colors.Length; i += 4)
            {
                colors[i] = 128;
                colors[i + 3] = 128;
            }

            HuntingColor = new byte[96];
            for (int i = 0; i < colors.Length; i += 4)
            {
                HuntingColor[i] = 255;
                HuntingColor[i + 3] = 255;
            }

            DrawMode = OpenTK.Graphics.ES11.All.Lines;      // Draw as Lines

            // Randomize the rotation of the enemy
            rotation.y = new Random().Next(180);
        }

        public override void Tick()
        {
            if (!Hunting)
            {
                timer--;
                if(timer <= 0)
                {
                    Hunting = true;
                    colors = HuntingColor;
                }
                return;
            }

            rotation += new Vector(1, 0, 0);                // Rotate the enemy
            position += speed;                              // Move the enemy

            // Make the enemy bounce one the walls
            if(position.x >= Bounds.size.x || position.x <= -Bounds.size.x)
            {
                speed.x *= -1;
            }
            if (position.y >= Bounds.size.y || position.y <= -Bounds.size.y)
            {
                speed.y *= -1;
            }

            // Check in the enemy is colliding with the player
            if(CheckCollision(GLView1.GetSpaceShip(), 1.5))
            {
                // Kill the player
                GLView1.Remove(GLView1.GetSpaceShip());
                GLView1.Spawn(new ParticleSystem(GLView1.GetSpaceShip().position, 10, 0.5f, 240, 32, GLView1.Reset));
            }
        }
    }
}