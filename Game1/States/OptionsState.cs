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
    class OptionsState : State
    {
        private List<string> optionPoints;
        private int selectedOptionPoint;
        public int SelectedOptionPoint
        {
            get => selectedOptionPoint;
            set
            {
                if (value == -1)
                    selectedOptionPoint = 2;
                else selectedOptionPoint = value % optionPoints.Count;
            }
        }
        private Keys lastKeyDown;
        Texture2D optionSelector;
        SpriteFont optionFont;

        public OptionsState(SokobanGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            optionPoints = new List<string>();
            optionFont = content.Load<SpriteFont>("MainFont");
            optionSelector = content.Load<Texture2D>("MenuSelector");
            optionPoints.Add("Tile size:");
            optionPoints.Add("Return to menu");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int menuSize = optionPoints.Count;
            for (int i = 0; i < menuSize; i++)
            {
                var menuPointText = optionPoints[i];
                spriteBatch.DrawString(optionFont, menuPointText, new Vector2(100, i * 100 + 100), Color.Black);
            }
            string correspondingValueText = Map.tileSize.ToString();
            spriteBatch.DrawString(optionFont, correspondingValueText, new Vector2(200, 0 * 100 + 100), Color.Black);
            spriteBatch.Draw(optionSelector, new Vector2(50, 100 * (SelectedOptionPoint + 1)), Color.White);

        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            var currentKeyboardState = Keyboard.GetState();
            var pressedKeys = currentKeyboardState.GetPressedKeys();
            if (pressedKeys.Length != 0)
            {
                lastKeyDown = pressedKeys.Last();
            }

            if (currentKeyboardState.IsKeyUp(Keys.Enter) && lastKeyDown == Keys.Enter)
                switch (SelectedOptionPoint)
                {
                    case 0:
                        {
                            TryChangeOptionValue(gameTime, SelectedOptionPoint);
                            break;
                        }
                    case 1:
                        {
                            game.ChangeState(new MenuState(game, graphicsDevice, content));
                            break;
                        }
                }

            if (currentKeyboardState.IsKeyUp(Keys.Escape) && lastKeyDown == Keys.Escape)
                game.Exit();

            if (currentKeyboardState.IsKeyUp(Keys.Up) && lastKeyDown == Keys.Up)
            {
                SelectedOptionPoint = SelectedOptionPoint - 1;
                lastKeyDown = Keys.None;
            }

            if (currentKeyboardState.IsKeyUp(Keys.Down) && lastKeyDown == Keys.Down)
            {
                SelectedOptionPoint = SelectedOptionPoint + 1;
                lastKeyDown = Keys.None;
            }


            if (currentKeyboardState.IsKeyUp(Keys.Left) && lastKeyDown == Keys.Left)
            {
                ref int propertyToChange = ref Map.tileSize;
                if (propertyToChange > 15)
                {
                    propertyToChange -= 5;
                }

                lastKeyDown = Keys.None;
            }

            if (currentKeyboardState.IsKeyUp(Keys.Right) && lastKeyDown == Keys.Right)
            {
                ref int propertyToChange = ref Map.tileSize;
                if (propertyToChange < 1000)
                {
                    propertyToChange += 5;
                }
                lastKeyDown = Keys.None;
            }
        }

        private void TryChangeOptionValue(GameTime gameTime, int selectedOptionPoint)
        {
            var currentKeyboardState = Keyboard.GetState();
            var pressedKeys = currentKeyboardState.GetPressedKeys();
            if (pressedKeys.Length != 0)
            {
                lastKeyDown = pressedKeys.Last();
            }
        }
    }
}
