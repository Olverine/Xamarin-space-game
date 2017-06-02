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

using OpenTK.Graphics.ES11;

namespace App1
{
    class GameObject
    {
        public Vector position = new Vector(0, 0, 0);
        public Vector rotation = new Vector(0, 0, 0);
        public Vector scale = new Vector(1, 1, 1);

        /// <summary>
        /// How the object should be drawn
        /// </summary>
        public All DrawMode = All.LineLoop;
        /// <summary>
        /// Is the object a UIElement?
        /// </summary>
        public bool UIElement;

        /// <summary>
        /// The vertices of the object
        /// </summary>
        public float[] vertices = {
            -0.5f, -0.5f, 0,
            0.5f, -0.5f, 0,
            -0.5f, 0.5f, 0,
            0.5f, 0.5f, 0
        };

        /// <summary>
        /// The color of each vertex
        /// </summary>
        public byte[] colors = {
            255, 255,   255, 255,
            255, 255,   255, 255,
            255, 255,   255, 255,
            255, 255,   255, 255,
            255, 255,   255, 255,
            255, 255,   255, 255,
        };

        /// <summary>
        /// Runs each time the game renders
        /// </summary>
        public virtual void Tick()
        {

        }

        /// <summary>
        /// Check the distance from this GameObject to another to see it they collide.
        /// </summary>
        /// <param name="other">The other GameObject</param>
        /// <param name="radius">The maximum distance that counts as a collision</param>
        /// <returns>Wether or not the objects are colliding</returns>
        protected bool CheckCollision(GameObject other, double radius)
        {
            return (Math.Sqrt(Math.Pow(position.x - other.position.x, 2) + Math.Pow(position.y - other.position.y, 2)) <= radius);
        }

        /// <summary>
        /// Linear interpolation function
        /// </summary>
        /// <param name="a">first number</param>
        /// <param name="b">second number</param>
        /// <param name="w">interpolation</param>
        protected float Lerp(float a, float b, float w)
        {
            return a + w * (b - a);
        }
    }
}