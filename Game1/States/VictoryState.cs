using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame.States
{
    class VictoryState : State
    {
        public TimeSpan FinishTime { get; set; }
        SpriteFont victoryFont;
        public VictoryState(SokobanGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            victoryFont = content.Load<SpriteFont>("MainFont");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(victoryFont, "YOU WIN!\nPress space to return to main menu", new Vector2(100, 100), Color.Black);
            spriteBatch.DrawString(victoryFont, "Your finish time in seconds: "+ FinishTime.TotalSeconds, new Vector2(100, 200), Color.Black);

        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().GetPressedKeys().Contains(Keys.Space))
                game.ChangeState(new MenuState(game, graphicsDevice, content));
        }
    }
}
