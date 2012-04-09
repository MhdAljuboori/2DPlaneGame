using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlaneGame
{
    class UserControlledSprite : Sprite
    {
        MouseState prevMouseState;

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed)
        : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, 0)
        {}

        public UserControlledSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
        : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, 0)
        {}

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    inputDirection.X -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    inputDirection.X += 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    inputDirection.Y -= 1;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    inputDirection.Y += 1;

                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            currentFrame.X = 1;
            // Move the sprite according to the direction property
            position += direction;
            
            // If the mouse moved, set the position of the sprite to the mouse position
            MouseState currMouseState = Mouse.GetState();
            if (currMouseState.X != prevMouseState.X ||
                currMouseState.Y != prevMouseState.Y)
            {
                if (currMouseState.X > prevMouseState.X)
                    currentFrame.X = 2;
                else if (currMouseState.X < prevMouseState.X)
                    currentFrame.X = 0;
                position = new Vector2(currMouseState.X, currMouseState.Y);
            }
            prevMouseState = currMouseState;

            //Plane Movement
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                currentFrame.X = 0;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                currentFrame.X = 2;
            // If the sprite is off the screen, put it back in play
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;
            base.Update(gameTime, clientBounds);
        }
    }
}
