using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SwordsDance
{
    public enum Frame
    {
        Left,
        SpinLeft,
        Right,
        SpinRight
    }
    /// <summary>
    /// A class representing a sword sprite
    /// </summary>
    public class SwordSprite
    {
        private Texture2D texture;
        private double frameTimer;

        public short animationFrame = 0;

        /// <summary>
        /// The frame of the animation
        /// </summary>
        public Frame Frame;

        public Vector2 Position { get; set; }

        /// <summary>
        /// Loads the sword sprite texture
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("SwordSheet");
        }

        /// <summary>
        /// Draws the animated sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Update animation timer
            frameTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //Update animation frame
            if (frameTimer > 0.1)
            {
                animationFrame++;
                if (animationFrame > 3)
                {
                    animationFrame = 0;
                }
                frameTimer -= 0.1;
            }
            //Draw the sprite
            var source = new Rectangle(animationFrame * 100, 0, 100, 150);
            spriteBatch.Draw(texture, Position, source, Color.White);
        }
    }
}
