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
    class Button : GameObject
    {
        protected Vector size;
        public Button(Vector size, Vector position)
        {
            this.size = size;
            this.position = position;

            vertices = new float[]
            {
                -size.x / 2, -size.y / 2, 0,
                -size.x / 2, size.y / 2, 0,
                size.x / 2, -size.y / 2, 0,
                size.x / 2, size.y / 2, 0,
            };

            colors = new byte[]
            {
                255, 128, 0, 255,
                255, 0, 255, 255,
                0, 255, 255, 255,
                0, 255, 0, 255
            };

            DrawMode = OpenTK.Graphics.ES11.All.TriangleStrip;  // Draw as a triangle strip
            UIElement = true;                                   // This GameObject should follow the screen
        }

        public override void Tick()
        {
            // Check if the user is currently touching the button
            if (MainActivity.isTouching)
            {
                int width = MainActivity.GetScreenWidth();
                if(
                    (MainActivity.TouchPoint.x / (width / GLView1.GetViewportWidth())) - (GLView1.GetViewportWidth() / 2) <= size.x / 2 &&
                    (MainActivity.TouchPoint.x / (width / GLView1.GetViewportWidth())) - (GLView1.GetViewportWidth() / 2) >= size.x / -2 &&
                    (MainActivity.TouchPoint.y / (width / GLView1.GetViewportWidth())) - (GLView1.GetViewportHeight() / 2) <= size.y / 2 &&
                    (MainActivity.TouchPoint.y / (width / GLView1.GetViewportWidth())) - (GLView1.GetViewportHeight() / 2) >= size.y / -2)
                {
                    OnClick();
                }
            }
        }

        /// <summary>
        /// This method runs when the button is tapped
        /// </summary>
        public virtual void OnClick()
        {

        }
    }
}