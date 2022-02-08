using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SwordsDance.Collisions;

namespace SwordsDance
{
    public class JButton
    {
        private Texture2D texture;

        private Vector2 position = new Vector2(100, 300);

        private BoundingRectangle bounds = new BoundingRectangle(100, 300, 100, 50);

        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        /// <summary>
        /// The color overlay of the button
        /// </summary>
        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the sprite texture using the provided ContentManager
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("JButton");
        }

        /// <summary>
        /// Draws the sprite using the supplied SpriteBatch
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to render with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color);
        }
    }
}
