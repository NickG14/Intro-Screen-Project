using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _580_Game_1
{
    public class TitleScreen : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;

        private SpriteFont arial;

        private Texture2D title;
        private Texture2D arrow;
        private Texture2D bg;
        private Texture2D star;

        private SwordSprite[] swords;

        public TitleScreen()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            swords = new SwordSprite[]{
            new SwordSprite() { Position = new Vector2(200,200), animationFrame = 0},
            new SwordSprite() {Position = new Vector2(500,200), animationFrame = 2}
            };

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            arial = Content.Load<SpriteFont>("Arial");
            title = Content.Load<Texture2D>("Title");
            arrow = Content.Load<Texture2D>("Arrow");
            bg = Content.Load<Texture2D>("CityBG");
            star = Content.Load<Texture2D>("Star");

            foreach (var sword in swords) sword.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkMagenta);
            spriteBatch.Begin();
            // TODO: Add your drawing code here

            //Draw Stars
            spriteBatch.Draw(star, new Vector2(500, 20), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(50, 170), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(350, 250), new Rectangle(0, 0, 64, 64), Color.White);
            spriteBatch.Draw(star, new Vector2(780, 100), new Rectangle(0, 0, 64, 64), Color.White);

            //Draw Background
            spriteBatch.Draw(bg, new Vector2(-50, 100), new Rectangle(0, 0, 700, 500), Color.Purple);
            spriteBatch.Draw(bg, new Vector2(650, 100), new Rectangle(0, 0, 700, 500), Color.Purple);



            //Draw exit text
            spriteBatch.DrawString(arial, "Press Escape/B to exit the game", new Vector2(290, 350), Color.White);

            //Draw title
            spriteBatch.Draw(title, new Vector2(110, 30), new Rectangle(0, 0, 300, 100), Color.LightBlue, 0, new Vector2(0,0), (float)2.0, SpriteEffects.None, 0);

            //Draw arrows
            for(int i = 1; i < 5; i++)
            {
                spriteBatch.Draw(arrow, new Vector2(50, (i-1) * 100 + 50), new Rectangle(0, 0, 100, 100), Color.White, 0, new Vector2(0,0), (float)0.25, SpriteEffects.None, 0);
                spriteBatch.Draw(arrow, new Vector2(725, i * 100 - 50), new Rectangle(0, 0, 100, 100), Color.White, 0, new Vector2(0, 0), (float)0.25, SpriteEffects.FlipVertically, 0);
            }
            
            //Draw Swords
            foreach(var sword in swords)
            {
                sword.Draw(gameTime, spriteBatch);
            }
            
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
