﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlaneGame
{
    abstract class Sprite
    {
        protected Texture2D textureImage;
        protected Vector2 position;
        protected Point frameSize;
        protected Point currentFrame;
        int collisionOffset;
        Point sheetSize;
        int millisecondsPerFrame;
        protected Vector2 speed;
        const int defaultMillisecondsPerFrame = 16;
        int scoreValue;

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,int scoreValue)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame, scoreValue)
        {}

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
        int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
        int millisecondsPerFrame, int scoreValue)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            this.scoreValue = scoreValue;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage,position,new Rectangle(currentFrame.X * frameSize.X,
            currentFrame.Y * frameSize.Y,frameSize.X, frameSize.Y),Color.White, 0, Vector2.Zero,
            1f, SpriteEffects.None, 0);
        }

        public bool IsOutOfBounds(Rectangle clientRect)
        {
            if (position.X < -frameSize.X ||
            position.X > clientRect.Width ||
            position.Y < -frameSize.Y ||
            position.Y > clientRect.Height)
            {
                return true;
            }
            return false;
        }

        public abstract Vector2 direction
        {
            get;
        }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                (int)position.X + collisionOffset,
                (int)position.Y + collisionOffset,
                frameSize.X - (collisionOffset * 2),
                frameSize.Y - (collisionOffset * 2));
            }
        }

        public Vector2 GetPostion()
        { return position; }

        public void SetPostion(Vector2 NewPos)
        { position = NewPos; }
    }
}
