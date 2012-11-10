using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Common;

namespace CPI311
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sounds : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;        // Drawing 2D stuff
        Texture2D bombTexture;          // Object for a texture
        SpriteFont textFont;            // Object for a font
        BasicEffect effect;
        Ray ray;
        bool clicked = false;
        Common.Plane plane;
        AudioListener listener = new AudioListener();
        AudioEmitter emitter = new AudioEmitter();

        Camera camera;
        AnimatedObject cube;
        ModelObject[] cubes = new ModelObject[5];
        Random random = new Random();

        int nextClick = -1;

        SoundEffect applause;
        SoundEffectInstance currentSound;

        KeyboardState prevKeyboardState = Keyboard.GetState();
        MouseState prevMouseState = Mouse.GetState();

        public Sounds()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bombTexture = Content.Load<Texture2D>("Textures/Bomb");      // Load the texture
            textFont = Content.Load<SpriteFont>("Fonts/SegoeUI");    // Load the font

            // TODO: use this.Content to load your game content here
            camera = new Camera();
            camera.Position = new Vector3(0, 0, -20);

            camera.AspectRatio = GraphicsDevice.Viewport.AspectRatio;

            plane = new Common.Plane(99);
            plane.Texture = Content.Load<Texture2D>("Textures/Jellyfish");
            plane.Scale *= 50;
            //plane.RotateX = 0.50f;
            plane.Position = new Vector3(0, -5, 0);

            cube = new AnimatedObject();
            cube.Model = Content.Load<Model>("Models/Dude/Dude");
            cube.Scale *= 0.25f;

            applause = Content.Load<SoundEffect>("Sounds/applause");
            currentSound = applause.CreateInstance();

            cube.StartAnimation("Take 001");
            effect = new BasicEffect(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Space) && prevKeyboardState.IsKeyUp(Keys.Space))
            {
                currentSound = applause.CreateInstance();
                listener = new AudioListener();
                listener.Position = camera.Position;
                listener.Up = camera.Up;
                listener.Forward = camera.Forward;
                
                emitter = new AudioEmitter();
                emitter.Position = cube.Position;
                emitter.Up = Vector3.Up;
                emitter.Forward = -camera.Forward;

                currentSound.Apply3D(listener, emitter);
                currentSound.Play();
            }

            cube.RotateY = 0.05f;
            cube.RotateX = 0.01f;

            MouseState mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                ray = new Ray();
                ray.Position = GraphicsDevice.Viewport.Unproject(
                    new Vector3(mouseState.X, mouseState.Y, 0),
                    camera.Projection, camera.View, cube.World);
                ray.Direction = GraphicsDevice.Viewport.Unproject(
                    new Vector3(mouseState.X, mouseState.Y, 1),
                    camera.Projection, camera.View, cube.World) - ray.Position;
                BoundingBox box = new BoundingBox(-Vector3.One, Vector3.One);

                if (box.Intersects(ray) != null)
                /*if(nextClick == i)*/
                {
                    SetupNext();

                    currentSound = applause.CreateInstance();
                    currentSound.Play();
                    clicked = true;
                }
                else
                {

                }

            }

            listener.Position += Vector3.UnitZ * 0.05f;
            currentSound.Apply3D(listener, emitter);
            currentSound.Play();

            prevKeyboardState = keyboardState;
            prevMouseState = mouseState;

            cube.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen
            GraphicsDevice.Clear(Color.Goldenrod);
            // Set the Depth Stencil State to default for 3D rendering

            
            GraphicsDevice.DepthStencilState = new DepthStencilState();
            // Setup some colors for lighting
            effect.EmissiveColor = new Vector3(0.2f, 0.2f, 0.2f);
            effect.DiffuseColor = new Vector3(0.5f, 0.0f, 0.0f);
            effect.SpecularColor = new Vector3(0.0f, 0.5f, 0.0f);
            effect.SpecularPower = 10;
            effect.LightingEnabled = true; // Enable lighting!
            // Some parameters for the Directional lighting
            effect.DirectionalLight0.Direction = new Vector3(0, -1, 1);
            effect.DirectionalLight0.SpecularColor = Vector3.One;
            // Provide the different matrices
            effect.View = camera.View;
            effect.Projection = camera.Projection;

            effect.World = cube.World;
            cube.Draw(camera.View, camera.Projection);


            // Draw some string with rotation and scale
            spriteBatch.Begin();
            if (clicked)
            spriteBatch.DrawString(textFont, "Playing Sound", new Vector2(50, 50), Color.Black);
            spriteBatch.DrawString(textFont, "CPI 311",
                    new Vector2(50, 300), // The position (top left)
                    Color.Black,    // Text color
                    0,     // Rotation angle
                    textFont.MeasureString("CSE 311") / 2, // Rotation center
                // using MeasureString, we can get the width/height
                    (float)Math.Sin(0), // Scale
                    SpriteEffects.None, 0);
            spriteBatch.End();

            // TODO: Add your drawing code here

            clicked = false;
            base.Draw(gameTime);
        }

        public void SetupNext()
        {
            nextClick = random.Next(cubes.Length);
            //sounds[nextClick].CreateInstance().Play();
        }
    }
}
