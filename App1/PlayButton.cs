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
    class PlayButton : Button
    {
        public PlayButton() : base(new Vector(4, 3), new Vector(0, 0))
        {
            vertices = new float[]
            {

                -size.x / 2, -size.y / 2, 0,
                size.x / 2, -size.y / 2, 0,
                size.x / 2, size.y / 2, 0,
                -size.x / 2, size.y / 2, 0,
            };
            colors = new byte[]
            {
                255, 128, 0, 255,
                255, 0, 255, 255,
                255, 255, 0, 255,
                0, 128, 128, 255,
            };
            DrawMode = OpenTK.Graphics.ES11.All.LineLoop;
        }

        public override void OnClick()
        {
            base.OnClick();
            GLView1.StartGame();
            GLView1.Remove(this);
        }
    }
}