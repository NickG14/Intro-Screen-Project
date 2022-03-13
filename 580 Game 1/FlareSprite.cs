using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SwordsDance
{
    class FlareSprite
    {
        private Texture2D texture;
        private double frameTimer;
        private float angle = 0;

        public short animationFrame = 0;

        /// <summary>
        /// The frame of the animation
        /// </summary>
        public Frame Frame;

        public Vector2 Position;

        /// <summary>
        /// Loads the sword sprite texture
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("flare");
           
        }

        /// <summary>
        /// Draws the animated sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, bool paused)
        {
            if(!paused)
            //Update animation timer
            Position.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 100;
  
            //Update animation frame
            if (Position.X < -800)
            {
                Position.X += 2100;
            }
            spriteBatch.Draw(texture, Position, null, Color.White);

        }
    }
}
