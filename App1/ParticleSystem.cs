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
    class ParticleSystem : GameObject
    {
        int particles;                                  // The number of particles to be emitted from this particle system
        float speed;                                    // The velocity of the particles
        float duration;                                 // The lifetime of the particles
        public int size;                                // The point size of the particles
        Func<int> OnDone;                               // Function to be executed when the particle system is done
        int[] angles;                                   // The angles of the velocity of the particles
        float iteration;                                // The number of times this particle system has been updated

        /// <summary>
        /// Create a particle system
        /// </summary>
        /// <param name="position">The position of the particle system</param>
        /// <param name="particles">The number of particles to be emitted from this particle system</param>
        /// <param name="speed">The velocity of the particles</param>
        /// <param name="duration">The lifetime of the particles</param>
        /// <param name="size">The point size of the particles</param>
        /// <param name="OnDone">Function to be executed when the particle system is done</param>
        public ParticleSystem(Vector position, int particles, float speed, float duration, int size, Func<int> OnDone)
        {
            this.position = position;
            this.particles = particles;
            this.speed = speed;
            this.duration = duration;
            this.size = size;
            this.OnDone = OnDone;

            // Randomize an angle for every particle
            angles = new int[particles];
            for(int i = 0; i < particles; i++)
            {
                angles[i] = new Random().Next(359);
                Android.Util.Log.Verbose("ANGLES", "" + angles[i]);
            }

            vertices = new float[particles * 3];        // Create the vertices
            // Make all vertices white
            colors = new byte[vertices.Length * 4];     
            for(int i = 0; i < colors.Length; i++)
            {
                colors[i] = 255;
            }

            DrawMode = OpenTK.Graphics.ES11.All.Points; // Draw as points
        }

        public override void Tick()
        {
            // Move all particles in their direction
            for(int i = 0; i < particles; i++)
            {
                vertices[i * 3] = (float)Math.Cos(angles[i] * (180 / Math.PI)) * (iteration / 10) * speed;
                vertices[(i * 3) + 1] = (float)Math.Sin(angles[i] * (180 / Math.PI)) *(iteration / 10) * speed;
            }

            // Fade the color of each particle to black
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = (byte)Lerp((float)colors[i], 0, 1 / (duration/2));
            }

            // Check if the particle system is done
            iteration++;
            if(iteration >= duration)
            {
                if(OnDone != null)
                    OnDone();
                GLView1.Remove(this);
            }
        }
    }
}