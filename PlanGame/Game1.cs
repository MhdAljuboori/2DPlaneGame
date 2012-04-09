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
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace PlaneGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteManager spriteManager;

        Texture2D backgroundTexture;

        //Game State
        enum GameState { Start, InGame, GameOver };
        GameState currentGameState = GameState.Start;

        //Fonts
        SpriteFont scoreFont;
        SpriteFont GameFont;
        SpriteFont AboutMe;

        //Random
        public Random rnd { get; private set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            rnd = new Random();

            //Screen Size
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
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

            //Initialize Script Manager
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            
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

            // TODO: use this.Content to load your game content here

            //Load Font
            scoreFont = Content.Load<SpriteFont>(@"fonts\arial");
            GameFont = Content.Load<SpriteFont>(@"fonts\arial");
            AboutMe = Content.Load<SpriteFont>(@"fonts\rockwell");

            //Disable Sprite Manager
            spriteManager.Visible = false;
            spriteManager.Enabled = false;

            //load background
            backgroundTexture = Content.Load<Texture2D>(@"Images\background");
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
            
            // TODO: Add your update logic here

            //Exit From Game
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.AliceBlue);
            string text;
            
            // TODO: Add your drawing code here
            switch (currentGameState)
            {
                case GameState.Start:
                    spriteBatch.Begin();

                    // Draw text for intro splash screen
                    text = "Shoot with Space";
                    spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2)
                    - (scoreFont.MeasureString(text).X / 2), (Window.ClientBounds.Height / 2)
                    - (scoreFont.MeasureString(text).Y / 2)), Color.Red);

                    text = "(Press any key to begin)";
                    spriteBatch.DrawString(scoreFont, text,new Vector2((Window.ClientBounds.Width / 2)
                    - (scoreFont.MeasureString(text).X / 2),(Window.ClientBounds.Height / 2)
                    - (scoreFont.MeasureString(text).Y / 2) + 30),Color.SaddleBrown);

                    text = "Mohammed Al jobory";
                    spriteBatch.DrawString(AboutMe, text, new Vector2((Window.ClientBounds.Width / 2)
                    - (scoreFont.MeasureString(text).X / 2) - 15, (Window.ClientBounds.Height)
                    - (scoreFont.MeasureString(text).Y) - 30), Color.SaddleBrown);

                    if (Keyboard.GetState().GetPressedKeys().Length > 0)
                    {
                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    spriteBatch.End();
                    break;
                case GameState.InGame:
                    spriteBatch.Begin();
                    
                    // Draw background image
                    spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Window.ClientBounds.Width,
                        Window.ClientBounds.Height), null, Color.White, 0, Vector2.Zero
                        , SpriteEffects.None, 0);

                    //Score
                    text = "Score = " + spriteManager.currentScore;
                    spriteBatch.DrawString(scoreFont, text, new Vector2(10, 10), Color.White);

                    //Lives
                    text = "Lives = " + spriteManager.GetPlayerLives;
                    spriteBatch.DrawString(scoreFont, text, new Vector2(Window.ClientBounds.Width
                        - scoreFont.MeasureString(text).X - 15, 10), Color.White);

                    //Me
                    text = "Mohammed Al jobory";
                    spriteBatch.DrawString(AboutMe, text, new Vector2((Window.ClientBounds.Width / 2)
                    - (scoreFont.MeasureString(text).X / 2 + 40), 10), Color.Red);

                    if (spriteManager.IsGameOver)
                    {
                        currentGameState = GameState.GameOver;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    spriteBatch.End();
                    break;
                case GameState.GameOver:
                    spriteBatch.Begin();

                    //Game Over
                    text = "Game Over";
                    spriteBatch.DrawString(GameFont, text, new Vector2(Window.ClientBounds.Width / 2-50
                        , Window.ClientBounds.Height / 2-10), Color.Red, 0, Vector2.Zero, 1
                        , SpriteEffects.None, 1);
                    
                    //Draw text for intro splash screen
                    text = "Your Score is " + spriteManager.currentScore;
                    spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(text).X / 2), (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(text).Y / 2) + 30), Color.SaddleBrown);

                    text = "(Press Enter to play agin)";
                    spriteBatch.DrawString(scoreFont, text, new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(text).X / 2), (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(text).Y / 2) + 60), Color.SaddleBrown);

                    text = "Mohammed Al jobory";
                    spriteBatch.DrawString(AboutMe, text, new Vector2((Window.ClientBounds.Width / 2)
                    - (scoreFont.MeasureString(text).X / 2) - 15, (Window.ClientBounds.Height)
                    - (scoreFont.MeasureString(text).Y) - 30), Color.SaddleBrown);

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.InGame;
                        spriteManager.ReSetAll();
                        spriteManager.StartGame();
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    spriteBatch.End();
                    break;
            }
            
            base.Draw(gameTime);
        }
    }
}
