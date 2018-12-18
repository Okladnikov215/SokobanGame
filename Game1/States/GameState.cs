using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanGame.States
{
    class GameState : Game
    {
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        Map levelMap;
        int currentLevel;
        List<char[,]> levels;

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            currentLevel = 0;
            levels = new List<char[,]>();
            levelMap = new Map(levels[0]);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //var charMap = new char[3, 3] { { 'e', 'e', 'e' }, { 'p', 'b', 'f' }, { 'e', 'b', 'f' } };
            //levels.Add(charMap);
            //charMap = new char[2, 2] { { 'p', 'f' }, { 'f', 'b' } };
            //levels.Add(charMap);

            levels = LoadLevels();
            Box.DefaultTexture = this.Content.Load<Texture2D>("box");
            Player.Texture = this.Content.Load<Texture2D>("Player");
            Tile.DefaultTexture = this.Content.Load<Texture2D>("Floor");
            Tile.DefaultEndTexture = this.Content.Load<Texture2D>("EndFloor");
        }

    }

}
