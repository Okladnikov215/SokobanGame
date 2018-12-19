using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
                selectedOptionPoint = value;
                if (value == 0)
                    selectedOptionPoint = optionPoints.Count - 1;
                if (value == optionPoints.Count)
                    selectedOptionPoint = 1;
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
            optionPoints.Add(optionPoints.Count, "Return To Main Menu");
            selectedOptionPoint = 1;
        }

        // need to split on 3 methods
        public Dictionary<int, string> GetSettingsFromFile(string filename)
        {
            //block of number - corresponding property name
            var dict = new Dictionary<int, string>();
            dict.Add(dict.Count, "TO CHANGE VALUES - CHANGE \"Settings.ini\" FILE");            
            int propertyIndex = dict.Count;
            foreach (PropertyInfo prop in typeof(GameSettings).GetProperties())
            {
                dict.Add(propertyIndex, prop.Name);
                propertyIndex++;
            }

            //If no file - create new one with default values
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

            //Change all GameSettings fields which are mentioned in Settings.ini
            var fileLines = File.ReadAllLines(filename);

            for (int i = 0; i < fileLines.Length; i++)
                foreach (var valueName in dict.Values)
                {
                    if (valueName == fileLines[i].Split(' ')[0])
                    {
                        var prop = typeof(GameSettings).GetProperty(valueName);
                        prop.SetValue(null, fileLines[i].Split(' ')[1]);
                    }
                }
            return dict;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var menuPointText = optionPoints[0];
            spriteBatch.DrawString(optionFont, menuPointText, new Vector2(100, 100), Color.Red);

            int menuSize = optionPoints.Count;
            for (int i = 1; i < menuSize-1; i++)
            {
                menuPointText = optionPoints[i];
                spriteBatch.DrawString(optionFont, menuPointText, new Vector2(100, i * 100 + 100), Color.Black);

                var prop = typeof(GameSettings).GetProperty(menuPointText);
                string correspondingValueText = (string)prop.GetValue(null,null);
                spriteBatch.DrawString(optionFont, correspondingValueText, new Vector2(300, i * 100 + 100), Color.Black);
            }

            menuPointText = optionPoints[optionPoints.Count-1];
            spriteBatch.DrawString(optionFont, menuPointText, new Vector2(100, (optionPoints.Count - 1) * 100 + 100), Color.Black);

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
                if (selectedOptionPoint == optionPoints.Count - 1)
                {
                    game.ChangeState(new MenuState(game, graphicsDevice, content));
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
        }
    }
}
