using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES11;
using OpenTK.Platform;
using OpenTK.Platform.Android;
using Android.Views;
using Android.Content;
using Android.Util;
using System.Collections.Generic;

namespace App1
{
    class GLView1 : AndroidGameView
    {
        static float viewportWidth = 48;

        static List<GameObject> gameObjects = new List<GameObject>();
        static SpaceShip spaceShip;
        static int score;
        static UIText scoreText = new UIText(new Vector(-(GetViewportWidth() / 2) + 2, (GetViewportHeight() / 2) - 2));

        static List<GameObject> SpawnQueue = new List<GameObject>();
        static List<GameObject> RemoveQueue = new List<GameObject>();


        public GLView1(Context context) : base(context)
        {
        }

        // This gets called when the drawing surface is ready
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Joystick joystick = new Joystick();
            spaceShip = new SpaceShip(joystick);

            Reset();

            // Run the render loop
            Run();
            GL.LineWidth(24 * (MainActivity.GetScreenDensity() / 442));
        }

        /// <summary>
        /// Start the actual game when the player taps the play button
        /// </summary>
        public static void StartGame()
        {
            score = 0;                              // Reset the score to 0
            spaceShip.position = new Vector(0, 0);  // Reset the spaceshi position to origin
            gameObjects = new List<GameObject>();   // Clear the gameObjects list
            // Add necessary game objects 
            gameObjects.Add(spaceShip);            
            gameObjects.Add(new Coin());
            gameObjects.Add(new Bounds());
            gameObjects.Add(scoreText);
            gameObjects.Add(new Joystick());
            scoreText.SetText("" + score);          // Reset the score text
        }

        /// <summary>
        /// Reset the game when the player dies
        /// </summary>
        public static int Reset()
        {
            // Check if the current score is a new highscore
            if(score > MainActivity.LoadHighscore())
            {
                MainActivity.SaveHighscore(score);
            }
            gameObjects = new List<GameObject>();   // Clear the gameObjects list
            gameObjects.Add(new PlayButton());      // Add a new playbutton
            // Create a triangle and but it in the play button
            GameObject play = new GameObject();
            play.vertices = new float[]
            {
                1, 0, 0,
                -1, 1, 0,
                -1, -1, 0,
            };
            play.DrawMode = All.Triangles;
            play.UIElement = true;
            play.position = (new Vector(0,0));
            gameObjects.Add(play);
            UIText highscoreText = new UIText(new Vector(-(GetViewportWidth() / 2) + 2, -(GetViewportHeight() / 2) + 4));
            highscoreText.SetText("" + MainActivity.LoadHighscore());
            gameObjects.Add(highscoreText);
            return 0;
        }

        // This method is called everytime the context needs
        // to be recreated. Use it to set any egl-specific settings
        // prior to context creation
        //
        // In this particular case, we demonstrate how to set
        // the graphics mode and fallback in case the device doesn't
        // support the defaults
        protected override void CreateFrameBuffer()
        {
            // the default GraphicsMode that is set consists of (16, 16, 0, 0, 2, false)
            try
            {
                Log.Verbose("GLCube", "Loading with default settings");

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                return;
            }
            catch (Exception ex)
            {
                Log.Verbose("GLCube", "{0}", ex);
            }

            // this is a graphics setting that sets everything to the lowest mode possible so
            // the device returns a reliable graphics setting.
            try
            {
                Log.Verbose("GLCube", "Loading with custom Android settings (low mode)");
                GraphicsMode = new AndroidGraphicsMode(0, 0, 0, 0, 0, false);

                // if you don't call this, the context won't be created
                base.CreateFrameBuffer();
                return;
            }
            catch (Exception ex)
            {
                Log.Verbose("GLCube", "{0}", ex);
            }
            throw new Exception("Can't load egl, aborting");
        }

        // This gets called on each frame render
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            // Clear the gameview
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear((uint)All.ColorBufferBit);

            // Create projection matrix
            GL.MatrixMode(All.Projection);          // Set matrix mode to projection
            GL.LoadIdentity();                      // Reset to an identity matrix
            // Set clipping planes of the camrea
            GL.Ortho(
                -(viewportWidth/2) + spaceShip.position.x,
                (viewportWidth / 2) + spaceShip.position.x, 
                -((viewportWidth / 2) / 1.777778f) + spaceShip.position.y,
                ((viewportWidth / 2) / 1.777778f) + spaceShip.position.y, 
                -2.0f, 
                2.0f);

            GL.MatrixMode(All.Modelview);           // Set matrix mode to modelview

            // Render all objects in the gameObjects list
            foreach(GameObject gameObject in gameObjects)
            {
                gameObject.Tick();                  // Run every gameObjects tick method

                GL.LoadIdentity();                  // Reset matrix to an identity matrix
                GL.PushMatrix();
                // Transform the game objects
                GL.Translate(gameObject.position.x, gameObject.position.y, gameObject.position.z);
                GL.Rotate(gameObject.rotation.x, 1, 0, 0);
                GL.Rotate(gameObject.rotation.y, 0, 1, 0);
                GL.Rotate(gameObject.rotation.z, 0, 0, 1);
                GL.Scale(gameObject.scale.x, gameObject.scale.y, gameObject.scale.z);
                // Translate UI elements to the screen position
                if (gameObject.UIElement)
                    GL.Translate(spaceShip.position.x, spaceShip.position.y, -0.25f);

                // Set point size of particles
                if (gameObject.GetType() == typeof(ParticleSystem))
                    GL.PointSize((gameObject as ParticleSystem).size);
                else if(gameObject.GetType() == typeof(Joystick))
                    GL.PointSize(64);

                //Render object
                GL.VertexPointer(3, All.Float, 0, gameObject.vertices);
                GL.EnableClientState(All.VertexArray);
                GL.ColorPointer(4, All.UnsignedByte, 0, gameObject.colors);
                GL.EnableClientState(All.ColorArray);

                GL.DrawArrays(gameObject.DrawMode, 0, gameObject.vertices.Length / 3);
                GL.PopMatrix();
            }

            // Spawn all objects in the spawn queue
            foreach(GameObject go in SpawnQueue)
            {
                gameObjects.Insert(1, go);
            }

            // Remove all objects in the remove queue
            foreach(GameObject go in RemoveQueue)
            {
                gameObjects.Remove(go);
            }

            // Reset queues
            SpawnQueue = new List<GameObject>();
            RemoveQueue = new List<GameObject>();

            SwapBuffers();
        }
        
        /// <returns>The spaceship</returns>
        public static SpaceShip GetSpaceShip()
        {
            return spaceShip;
        }

        /// <summary>
        /// Add a game object to the spawn queue.
        /// Objects in the spawn queue will spawn when the frame has finished rendering
        /// </summary>
        /// <param name="gameObject">The GameObject to be added</param>
        public static void Spawn(GameObject gameObject)
        {
            SpawnQueue.Add(gameObject);
        }

        /// <summary>
        /// Add a GameObject to the remove queue.
        /// Objects in the remove queue will be removed from the game scene when the frame has finnished rendering
        /// </summary>
        /// <param name="gameObject">The GameObject to be removed</param>
        public static void Remove(GameObject gameObject)
        {
            RemoveQueue.Add(gameObject);
        }

        /// <summary>
        /// Add 1 to the score and update the scoreText.
        /// </summary>
        public static void IncrementScore()
        {
            score++;
            scoreText.SetText(""+score);
        }

        public static float GetViewportWidth()
        {
            return viewportWidth;
        }

        public static void SetViewportWidth(float width)
        {
            viewportWidth = width;
        }

        public static float GetViewportHeight()
        {
            return viewportWidth / 1.7777778f;
        }
    }
}