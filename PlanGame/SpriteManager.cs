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
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;

        //User & enemy & bum variablees
        UserControlledSprite player;
        List<Sprite> spriteList = new List<Sprite>();
        List<Sprite> BumList = new List<Sprite>();
        List<Sprite> ListSprite = new List<Sprite>();
        List<Bumb> LBumb = new List<Bumb>();
        List<Sprite> LiveList = new List<Sprite>();
        
        //Time of Gun
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame = 200;

        //Random enemy variables
        int enemySpawnMinMilliseconds = 1000;
        int enemySpawnMaxMilliseconds = 2000;
        int enemyMinSpeed = 2;
        int enemyMaxSpeed = 4;

        //Time to next Spawn
        int nextSpawnTime = 0;

        //Score
        int likelihoodAutomated = 75;
        int likelihoodChasing = 20;

        //Score
        public int currentScore;

        //Game Stop
        bool StopGame = false;

        //Player Lives
        int PlayerLive = 3;

        //Time To Increase Enemy in Second
        int timeOfGame = 0;
        int timeToIncreaseEnemy = 1000;


        public void StartGame()
        {
            StopGame = false;
        }

        public void ReSetAll()
        {
            PlayerLive = 3;
            spriteList.Clear();
            BumList.Clear();
            LiveList.Clear();
            LBumb.Clear();
            enemySpawnMinMilliseconds = 1000;
            enemySpawnMaxMilliseconds = 2000;
            timeOfGame = 0;
            currentScore = 0;
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 58);
            player.SetPostion(new Vector2(Game.Window.ClientBounds.Width / 2
                , Game.Window.ClientBounds.Height - 58));
        }

        public bool IsGameOver
        {
            get { return StopGame; }
        }

        public int GetPlayerLives
        {
            get { return PlayerLive; }
        }

        private void ResetSpawnTime()
        {
            if (enemySpawnMaxMilliseconds < enemySpawnMinMilliseconds)
                enemySpawnMaxMilliseconds = enemySpawnMinMilliseconds + 1;
            nextSpawnTime = ((Game1)Game).rnd.Next(
            enemySpawnMinMilliseconds,
            enemySpawnMaxMilliseconds);
        }

        private void SpawnEnemy()
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;
            
            // Default frame size
            Point frameSize = new Point(58, 58);

            // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((Game1)Game).rnd.Next(4))
            {
                case 0: // LEFT to RIGHT
                    position = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                    - frameSize.Y));
                    speed = new Vector2(((Game1)Game).rnd.Next(
                    enemyMinSpeed,enemyMaxSpeed), 0);
                    break;
                case 1: // RIGHT to LEFT
                    position = new Vector2(
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                    ((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                    - frameSize.Y));
                    speed = new Vector2(-((Game1)Game).rnd.Next(
                    enemyMinSpeed, enemyMaxSpeed), 0);
                    break;
                case 2: // BOTTOM to TOP
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X),
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
                    speed = new Vector2(0,-((Game1)Game).rnd.Next(enemyMinSpeed,
                    enemyMaxSpeed));
                    break;
                case 3: // TOP to BOTTOM
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    speed = new Vector2(0,((Game1)Game).rnd.Next(enemyMinSpeed,
                    enemyMaxSpeed));
                    break;
            }            
            
            // Get random number
            int random = ((Game1)Game).rnd.Next(100);
            if (random < likelihoodAutomated)
            {
                
                // Create AutomatedSprite
                spriteList.Add(
                    new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\enemy"),
                    position, new Point(58, 58), 10, new Point(0, 0), new Point(0, 0),
                    speed, 0));
            }
            else if (random < likelihoodAutomated +likelihoodChasing)
            {
                // Create ChasingSprite
                spriteList.Add(
                new ChasingSprite(Game.Content.Load<Texture2D>(@"images\enemy"),
                position, new Point(58, 58), 10, new Point(0, 0), new Point(0, 0),
                speed, this, 0));
            }
            else
            {
                // Create EvadingSprite
                LiveList.Add(
                new EvadingSprite(Game.Content.Load<Texture2D>(@"images\Plane"),
                position, new Point(58, 58), 10, new Point(1, 0), new Point(0, 0),
                speed, this, .75f, 150, 0));
            }
        }

        public Vector2 GetPlayerPosition()
        {
            return player.GetPostion();
        }

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            ResetSpawnTime();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            //load player
            player = new UserControlledSprite(Game.Content.Load<Texture2D>(@"Images\Plane")
                , new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 58)
                , new Point(58, 58), 10, new Point(1, 0), new Point(0, 0), new Vector2(6f));
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 58);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            //Time To Increase enemy
            timeOfGame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeOfGame > timeToIncreaseEnemy)
            {
                timeOfGame = 0;
                if (enemySpawnMinMilliseconds > 10)
                    enemySpawnMinMilliseconds -= 30;
                else
                    enemySpawnMinMilliseconds = 10;
                if (enemySpawnMaxMilliseconds > 15)
                    enemySpawnMaxMilliseconds -= 30;
                else
                    enemySpawnMaxMilliseconds = 15;
            }
            
            //when it’s time to spawn a new enemy
            if (!StopGame)
            {
                nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
                if (nextSpawnTime < 0)
                {
                    SpawnEnemy();

                    // Reset spawn timer
                    ResetSpawnTime();
                }
            }

            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);

            // Update all Live sprites
            foreach (Sprite s in LiveList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    PlayerLive++;
                    LiveList.Remove(s);
                    break;
                }
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    LiveList.Remove(s);
                    break;
                }
            }


            // Update All enemy sprites
            foreach (Sprite s in spriteList)
            {
                s.Update(gameTime, Game.Window.ClientBounds);
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    if (PlayerLive == 0)
                        StopGame = true;
                    PlayerLive--;
                    Mouse.SetPosition(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 58);
                    player.SetPostion(new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height - 58));
                    spriteList.Remove(s);
                    break;
                }
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    spriteList.Remove(s);
                    break;
                }
            }
            
            //Create Bums of Player
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;
                    BumList.Add(new Bumb(Game.Content.Load<Texture2D>(@"Images\bum")
                        , new Vector2(player.GetPostion().X + 14, player.GetPostion().Y), new Point(26, 26)
                        , 5, new Point(0, 0), new Point(0, 0), new Vector2(3f),0));

                    LBumb.Add(new Bumb(Game.Content.Load<Texture2D>(@"Images\bumm"), new Vector2(-500, -500), new Point(26, 26)
                        , 1, new Point(0, 0), new Point(0, 0), new Vector2(3f),0));
                }
            }
            
            //Update All Bums
            foreach (Bumb b in BumList)
            {
                b.Update(gameTime, Game.Window.ClientBounds);
                if (b.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    BumList.Remove(b);
                    break;
                }
            }

            //if there is collision between enemy sprites and Gun Bums
            bool found = false;
            int i = 0;
            foreach (Sprite s in spriteList)
            {
                foreach (Bumb b in BumList)
                {
                    if (s.collisionRect.Intersects(b.collisionRect))
                    {
                        Vector2 vec = s.GetPostion();
                        vec = new Vector2(vec.X - (58 / 2), vec.Y - (58 / 2));
                        try
                        {
                            Bumb B = LBumb.ElementAt(0);
                            B.SetPostion(vec);
                        }
                        catch { };
                        spriteList.Remove(s);
                        BumList.Remove(b);
                        currentScore++;
                        found = true;
                        break;
                    }
                }
                i++;
                if (found)
                    break;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);

            //Draw Player
            player.Draw(gameTime, spriteBatch);

            //Draw all enemy Sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);

            //Draw all Bum Sprites
            foreach (Bumb b in BumList)
                b.Draw(gameTime, spriteBatch);

            //Show Bumm in postion of collission brtween sprites and bums
            foreach (Bumb b in LBumb)
            {
                b.Draw(gameTime, spriteBatch);
                if (b.GetPostion() != new Vector2(-500, -500))
                    LBumb.Remove(b);
                break;
            }

            //Draw All Live Sprites
            foreach (Sprite s in LiveList)
            {
                s.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}