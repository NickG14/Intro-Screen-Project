using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SwordsDance.Collisions;

namespace SwordsDance
{
    public enum Rows
    {
        Top,
        Bottom
    }

    public class NoteSprite
    {
        private Texture2D texture;
        private double frameTimer;

        public Rows row;
        //Used for note speed/spacing
        private const float bpm = 132f;
        public Vector2 speed = new Vector2(-bpm / 16, 0);

        public int animationFrame = 0;

        private Random selectRow = new Random();

        private Vector2 position = new Vector2(980, 72);

        public bool hit = false;

        public bool stopped = true;

        private BoundingRectangle bounds = new BoundingRectangle(new Vector2(980, 72), 50, 50);


        /// <summary>
        /// The bounding volume of the sprite
        /// </summary>
        public BoundingRectangle Bounds => bounds;

        public Color Color { get; set; } = Color.White;

        /// <summary>
        /// Loads the note texture
        /// </summary>
        /// <param name="content">The ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("NoteHit");

            if (selectRow.Next(0, 2) == 0)
            {
                position = new Vector2(980, 72);
                bounds = new BoundingRectangle(new Vector2(980, 72), 100, 50);
            }
            else
            {
                position = new Vector2(980, 302);
                bounds = new BoundingRectangle(new Vector2(980, 302), 100, 50);
            }
        }


        /// <summary>
        /// Updates the note's position
        /// </summary>
        /// <param name="gameTime">The GameTime</param>
        public void Update(GameTime gameTime)
        {
            if (!hit && !stopped)
            {
                position += speed;
                bounds.X = position.X;
                bounds.Y = position.Y;
            }
            
        }

        /// <summary>
        /// Draws the animated sprite
        /// </summary>
        /// <param name="gameTime">The game time</param>
        /// <param name="spriteBatch">The spritebatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (animationFrame < 14)
            {
                //Update animation timer
                frameTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (hit)
                {
                    bounds.X = 700;
                    bounds.Y = 700;
                    //Update animation frame
                    if (frameTimer > 0.02)
                    {
                        animationFrame++;
                        frameTimer -= 0.02;
                    }
                }
                //Draw the sprite
                var source = new Rectangle(animationFrame * 96, 0, 96, 96);
                spriteBatch.Draw(texture, position, source, Color.White);
            }
        }

        public void Reset(GameTime gameTime)
        {
            speed = new Vector2(-bpm / 16, 0);

            animationFrame = 0;

            selectRow = new Random();

            if(selectRow.Next(0,2) == 0)
            {
                position = new Vector2(980, 72);
                bounds = new BoundingRectangle(new Vector2(980, 72), 100, 50);
            }
            else
            {
                position = new Vector2(980, 302);
                bounds = new BoundingRectangle(new Vector2(980, 302), 100, 50);
            }
            
            hit = false;

            stopped = true;

    }
    }
}
