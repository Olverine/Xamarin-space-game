using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using Android.Util;

namespace App1
{
    [Activity(
        Label = "RymdSkepp",
        MainLauncher = true,
        Icon = "@drawable/icon",
        ConfigurationChanges = ConfigChanges.KeyboardHidden, 
        ScreenOrientation = ScreenOrientation.Landscape
#if __ANDROID_11__
        , HardwareAccelerated=false
#endif
        )]
    public class MainActivity : Activity
    {
        GLView1 view;

        public static Vector TouchPoint = new Vector(0, 0, 0);
        public static bool isTouching;
        static Context context;
        static int width;
        static float density;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            this.Window.AddFlags(WindowManagerFlags.Fullscreen);
            // Create our OpenGL view, and display it
            view = new GLView1(this);
            context = BaseContext;

            view.Touch += TouchInput;
            width = Resources.DisplayMetrics.WidthPixels;
            density = Resources.DisplayMetrics.Xdpi; 
            SetContentView(view);
        }

        protected override void OnPause()
        {
            base.OnPause();
            view.Pause();
        }

        protected override void OnResume()
        {
            base.OnResume();
            view.Resume();
        }

        /// <summary>
        /// Udate the TouchPoint variable to the screen coordinates of the current touch event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="touchEventArgs"></param>
        private void TouchInput(object sender, View.TouchEventArgs touchEventArgs)
        {
            switch (touchEventArgs.Event.Action)
            {
                // a finger is touching the screen
                case MotionEventActions.Down:
                case MotionEventActions.Move:
                    TouchPoint.x = touchEventArgs.Event.GetX();
                    TouchPoint.y = touchEventArgs.Event.GetY();
                    isTouching = true;
                    break;
                // a finger stopped touching the screen
                case MotionEventActions.Up:
                    isTouching = false;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Write a new highscore to the save file
        /// </summary>
        /// <param name="highscore">The new highscore to be saved</param>
        public static void SaveHighscore(int highscore)
        {
            // Check if the file exists
            // If not, create it
            if (!File.Exists(context.GetDir("save", FileCreationMode.Private) + "hs.save"))
            {
                File.Create(context.GetDir("save", FileCreationMode.Private) + "hs.save").Close();
            }
            // Write the new score to the file
            File.WriteAllText(context.GetDir("save", FileCreationMode.Private) + "hs.save", "" + highscore);
        }

        /// <summary>
        /// Read the highscore from the save file.
        /// </summary>
        /// <returns>The file content parsed to an integer</returns>
        public static int LoadHighscore()
        {
            // Check if the file exists
            // If not, create it and write a 0 to it.
            if (!File.Exists(context.GetDir("save", FileCreationMode.Private) + "hs.save"))
            {
                File.Create(context.GetDir("save", FileCreationMode.Private) + "hs.save").Close();
                File.WriteAllText(context.GetDir("save", FileCreationMode.Private) + "hs.save", "0");
                return 0;
            }
            // Read the file at return it's content
            string highscore = File.ReadAllText(context.GetDir("save", FileCreationMode.Private) + "hs.save");
            return int.Parse(highscore);
        }

        public static int GetScreenWidth()
        {
            return width;
        }

        public static float GetScreenDensity()
        {
            return density;
        }
    }
}

