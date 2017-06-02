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
    class UIText : GameObject
    {
        /// <summary>
        /// Create a new UIText
        /// </summary>
        /// <param name="position">The position of the top left corner of the text</param>
        public UIText(Vector position)
        {
            this.position = position;
            DrawMode = OpenTK.Graphics.ES11.All.Lines;
            UIElement = true;
            vertices = new float[0];
            colors = new byte[0];
        }

        /// <summary>
        /// Set th text to be drawn
        /// </summary>
        /// <param name="text">The text to be drawn</param>
        public void SetText(string text)
        {
            byte[] textBytes = Encoding.ASCII.GetBytes(text);   // Get the bytes of the text in ascii format
            List<float> verts = new List<float>();              // Create a list of vertices

            // Add the vertices of every character to the verts list
            for(int i = 0; i < textBytes.Length; i++)
            {
                for(int j = 0; j < characters[textBytes[i] - 48].Length; j++)
                {
                    if(j % 3 == 0)
                        verts.Add(characters[textBytes[i] - 48][j] + (i * 1.5f));
                    else
                        verts.Add(characters[textBytes[i] - 48][j]);
                }
            }

            vertices = verts.ToArray();                         // Convert the verts list to an array

            // Set the color of every vertex to white
            colors = new byte[(vertices.Length / 3) * 4];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = 255;
            }
        }

        // The vertices of the characters
        float[][] characters =
        {
            // 0
            new float[]{
                0, 0, 0,
                1, 0, 0,
                1, 0, 0,
                1, -2, 0,
                1, -2, 0,
                0, -2, 0,
                0, -2, 0,
                0, 0, 0,
            },

            // 1
            new float[]
            {
                0, 0, 0,
                0.5f, 0, 0,
                0.5f, 0, 0,
                0.5f, -2, 0,
                0, -2, 0,
                1, -2, 0,
            },

            // 2
            new float[]
            {
                0, 0, 0,
                1, 0, 0,
                1, 0, 0,
                1, -1, 0,
                1, -1, 0,
                0, -1, 0,
                0, -1, 0,
                0, -2, 0,
                0, -2, 0,
                1, -2, 0,
            },

            // 3
            new float[]
            {
                1, 0, 0,
                1, -2, 0,
                0, 0, 0,
                1, 0, 0,
                0, -1, 0,
                1, -1, 0,
                0, -2, 0,
                1, -2, 0
            },

            // 4
            new float[]
            {
                0, 0, 0,
                0, -1, 0,
                0, -1, 0,
                1, -1, 0,
                1, 0, 0,
                1, -2, 0
            },

            // 5
            new float[]
            {
                1, 0, 0,
                0, 0, 0,
                0, 0, 0,
                0, -1, 0,
                0, -1, 0,
                1, -1, 0,
                1, -1, 0,
                1, -2, 0,
                1, -2, 0,
                0, -2, 0,
            },

            // 6
            new float[]
            {
                1, 0, 0,
                0, 0, 0,
                0, 0, 0,
                0, -2, 0,
                0, -2, 0,
                1, -2, 0,
                1, -2, 0,
                1, -1, 0,
                1, -1, 0,
                0, -1, 0
            },

            // 7
            new float[]
            {
                0, 0, 0,
                1, 0, 0,
                1, 0, 0,
                1, -2, 0
            },

            // 8
            new float[]
            {
                1, 0, 0,
                1, -2, 0,
                0, 0, 0,
                0, -2, 0,
                0, 0, 0,
                1, 0, 0,
                0, -1, 0,
                1, -1, 0,
                0, -2, 0,
                1, -2, 0,
            },

            // 9
            new float[]
            {
                1, 0, 0,
                1, -2, 0,
                0, 0, 0,
                1, 0, 0,
                0, 0, 0,
                0, -1, 0,
                0, -1, 0,
                1, -1, 0,
            }
        };
    }
}