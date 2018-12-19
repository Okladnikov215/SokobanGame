using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame.States
{
    public class OptionsState : State
    {
        private Dictionary<int, string> optionPoints;
        private int selectedOptionPoint;
        public int SelectedOptionPoint
        {
            get => selectedOptionPoint;
            set
            {
                if (value == -1)
                    selectedOptionPoint = optionPoints.Count - 1;
                else selectedOptionPoint = value % optionPoints.Count;
            }
        }
        private Keys lastKeyDown;
        Texture2D optionSelector;
        SpriteFont optionFont;

        public OptionsState(SokobanGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            optionPoints = GetSettingsFromFile("Settings.ini");
            optionFont = content.Load<SpriteFont>("MainFont");
            optionSelector = content.Load<Texture2D>("MenuSelector");
        }

        public Dictionary<int, string> GetSettingsFromFile(string filename)
        {
            var dict = new Dictionary<int, string>();
            int propertyIndex = 0;
            foreach (PropertyInfo prop in typeof(GameSettings).GetProperties())
            {
                dict.Add(propertyIndex, prop.Name);
                propertyIndex++;
            }

            if (!File.Exists(filename))
            {
                var file = File.Create(filename);
                file.Close();
                foreach (PropertyInfo prop in typeof(GameSettings).GetProperties())
                {
                    var stringToWrite = string.Format("{0} {1}", prop.Name, prop.GetValue(null, null));
                    File.AppendAllText(filename, stringToWrite + Environment.NewLine);
                }
            }

            var fileLines = File.ReadAllLines(filename);

            Debug.Write(GameSettings.PlayerName);
            Debug.Write(GameSettings.TileSize);

            for (int i=0; i< fileLines.Length; i++)
            foreach (var valueName in dict.Values)
            {
               if (valueName == fileLines[i].Split(' ')[0])
                    {
                       var prop = typeof(GameSettings).GetProperty(valueName);
                       prop.SetValue(null, fileLines[i].Split(' ')[1]);
                    }
            }

            Debug.Write(GameSettings.PlayerName);
            Debug.Write(GameSettings.TileSize);

            return dict;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            int menuSize = optionPoints.Count;
            for (int i = 0; i < menuSize; i++)
            {
                var menuPointText = optionPoints[i];
                spriteBatch.DrawString(optionFont, menuPointText, new Vector2(100, i * 100 + 100), Color.Black);
            }
            string correspondingValueText = GameSettings.TileSize.ToString();

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

            PropertyInfo prop;
            var propertyName = optionPoints[SelectedOptionPoint];
            prop = typeof(GameSettings).GetProperty("TileSize");
            var propValue = prop.GetValue(null, null);

            if (currentKeyboardState.IsKeyUp(Keys.Left) && lastKeyDown == Keys.Left)
            {
                if (propValue is int propValueInt)
                {
                    propValueInt -= 5;
                    prop.SetValue(null, propValueInt);
                }

                lastKeyDown = Keys.None;
            }

            if (currentKeyboardState.IsKeyUp(Keys.Right) && lastKeyDown == Keys.Right)
            {
                if (propValue is int propValueInt)
                {
                    propValueInt += 5;
                    prop.SetValue(null, propValueInt);
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
