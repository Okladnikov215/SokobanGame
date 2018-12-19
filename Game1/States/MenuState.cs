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
    class MenuState : State
    {
        private List<string> menuPoints;
        private int selectedMenuPoint;
        public int SelectedMenuPoint
        {
            get => selectedMenuPoint;
            set
            {
                if (value == -1)
                    selectedMenuPoint = 2;
                else selectedMenuPoint = value % menuPoints.Count;
            }
        }
        private Keys lastKeyDown;
        Texture2D menuSelector;
        SpriteFont menuFont;

        public MenuState(SokobanGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            menuFont = content.Load<SpriteFont>("MainFont");
            menuSelector = content.Load<Texture2D>("MenuSelector");
            menuPoints = new List<string>();
            menuPoints.Add("Start Game");
            menuPoints.Add("Look at settings");
            menuPoints.Add("Exit");
            SelectedMenuPoint = 0;
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int menuSize = menuPoints.Count;
            for (int i = 0; i < menuSize; i++)
            {
                var menuPointText = menuPoints[i];
                spriteBatch.DrawString(menuFont, menuPointText, new Vector2(100, i * 100 + 100), Color.Black);
            }
            spriteBatch.Draw(menuSelector, new Vector2(50, 100 * (SelectedMenuPoint + 1)), Color.White);
        }

        public override void PostUpdate(GameTime gameTime)
        {

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
                switch (SelectedMenuPoint)
                {
                    case 0:
                        {
                            game.ChangeState(new GameState(game, graphicsDevice, content));
                            break;
                        }
                    case 1:
                        {
                            game.ChangeState(new OptionsState(game, graphicsDevice, content));
                            break;
                        }
                    case 2:
                        {
                            game.Exit();
                            break;
                        }
                }

            if (currentKeyboardState.IsKeyUp(Keys.Escape) && lastKeyDown == Keys.Escape)
                game.Exit();

            if (currentKeyboardState.IsKeyUp(Keys.Up) && lastKeyDown == Keys.Up)
            {
                SelectedMenuPoint = SelectedMenuPoint - 1;
                lastKeyDown = Keys.None;
            }

            if (currentKeyboardState.IsKeyUp(Keys.Down) && lastKeyDown == Keys.Down)
            {
                SelectedMenuPoint = SelectedMenuPoint + 1;
                lastKeyDown = Keys.None;
            }
        }
    }
}
