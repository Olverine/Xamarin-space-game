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
    class Joystick : GameObject
    {
        /// <summary>
        /// The position of the joystick as viewport coordinates
        /// </summary>
        Vector initialPosition = new Vector((GLView1.GetViewportWidth()/2) - 5, -(GLView1.GetViewportHeight()/2) + 5);

        public Joystick()
        {
            DrawMode = OpenTK.Graphics.ES11.All.Points;         // Draw as points to minimize the number of vertices
            UIElement = true;                                   // This object should follow tthe camera

            // Set the vertices
            vertices = new float[]{
                2, 0, 0,
                0, 0, 0,
                1, 1, 0,
                1, -1, 0
            };

            // Set the color
            colors = new byte[]
            {
                255, 0, 255, 255,
                255, 0, 255, 255,
                255, 0, 255, 255,
                255, 0, 255, 255,
                255, 0, 255, 255,
            };
        }

        public override void Tick()
        {
            GetJoystickInput();                                 // Update the joystick
        }

        /// <summary>
        /// Updates the joystick
        /// </summary>
        /// <returns>
        /// Input as a 3-dimensional vector where
        /// x = x-axis input
        /// y = y-axis input
        /// z = joystick angle
        /// </returns>
        public Vector GetJoystickInput()
        {
            int width = MainActivity.GetScreenWidth();
            // Get the current touch coordinates
            float pointX = (MainActivity.TouchPoint.x / (width / GLView1.GetViewportWidth())) - (GLView1.GetViewportWidth()/2);
            float pointY = (-MainActivity.TouchPoint.y / (width / GLView1.GetViewportWidth())) + (GLView1.GetViewportHeight()/2);
            float angle = (float)Math.Atan2(pointY - initialPosition.y, pointX - initialPosition.x); // Calculate the angle of the joystick
            float distance = (float)Math.Sqrt(Math.Pow(pointX - initialPosition.x, 2) + Math.Pow(pointY - initialPosition.y, 2)); // Calculate the distance between the position of the joystick and the touch
            // Clamp the distance between 0 and 1
            if (distance >= 1)
                distance = 1;
            // Reset the distance if the user is not touching the screen
            if (!MainActivity.isTouching)
                distance = 0;
            // Offset the joystick from it's initial position
            position.x = initialPosition.x + (float)Math.Cos(angle) * distance;
            position.y = initialPosition.y + (float)Math.Sin(angle) * distance;
            Vector inputVector = position - initialPosition;    // Set the input vector x and y values
            inputVector.z = angle;                              // Set the z value to the angle
            return inputVector;
        }

        /// <summary>
        /// Set a new position of the joystick on the viewport
        /// </summary>
        /// <param name="pos">New position on the viewport</param>
        public void SetPosition(Vector pos)
        {
            initialPosition = pos;
        }
    }
}