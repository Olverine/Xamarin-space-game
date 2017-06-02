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
    class SpaceShip : GameObject
    {
        Joystick joystick;

        public SpaceShip(Joystick joystick)
        {
            // Set the vertices of the shape
            vertices = new float[]
            {
                -1, -1, 0,
                0, 1, 0,
                1, -1, 0
            };

            DrawMode = OpenTK.Graphics.ES11.All.Triangles;  // draw as triangles

            this.joystick = joystick;
        }

        public override void Tick()
        {
            Vector input = joystick.GetJoystickInput();     // Get input from the joystick
            rotation.z = (joystick.GetJoystickInput().z) * (180 / (float)Math.PI) - 90; // Convert joystick angle from radians to degrees
            position += input * 0.25f;                      // Translate the spaceship in the direction of the joystick
            position.z = 0;

            // Clamp the position of the spaceship inside the bounds
            if (position.x > Bounds.size.x)
                position.x = Bounds.size.x;
            else if (position.x < -Bounds.size.x)
                position.x = -Bounds.size.x;
            if (position.y > Bounds.size.y)
                position.y = Bounds.size.y;
            else if (position.y < -Bounds.size.y)
                position.y = -Bounds.size.y;
        }
    }
}