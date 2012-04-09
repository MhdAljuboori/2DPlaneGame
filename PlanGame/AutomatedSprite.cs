using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PlaneGame
{
    class AutomatedSprite : Sprite
    {
        public AutomatedSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
                sheetSize, speed, scoreValue)
        {}

        public AutomatedSprite(Texture2D textureImage, Vector2 position,
        Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
        Vector2 speed, int millisecondsPerFrame, int scoreValue)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame, scoreValue)
        {}

        public override Vector2 direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position.X += direction.X;
            base.Update(gameTime, clientBounds);
        }
    }
}
