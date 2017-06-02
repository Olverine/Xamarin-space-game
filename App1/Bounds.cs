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
    class Bounds : GameObject
    {
        /// <summary>
        /// The size of the playing area
        /// </summary>
        public static Vector size = new Vector(20, 20);

        /// <summary>
        /// Create a visible border around the playing area
        /// </summary>
        public Bounds()
        {
            // Set the vertices
            vertices = new float[]
            {
                size.x, size.y, 0,
                -size.x, size.y, 0,
                -size.x, -size.y, 0,
                size.x, -size.y, 0
            };
            
            // Set the colors
            colors = new byte[]{
                255, 128, 0, 255,
                255, 0, 255, 255,
                255, 255, 0, 255,
                0, 128, 128, 255,
            };
        } 
    }
}