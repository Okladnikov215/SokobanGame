using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SokobanGame : Game
    {
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Map levelMap;
        int currentLevel;
        List<char[,]> levels;


        public SokobanGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

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
            var charMap = new char[3, 3] { { 'e', 'e', 'e' }, { 'p', 'b', 'f' }, { 'e', 'b', 'f' } };
            levels.Add(charMap);

            charMap = new char[2, 2] { { 'p', 'f' }, { 'f', 'b' } };
            levels.Add(charMap);
            levelMap =  new Map(levels[0]);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Box.DefaultTexture = this.Content.Load<Texture2D>("box");
            Player.Texture = this.Content.Load<Texture2D>("Player");
            Tile.DefaultTexture = this.Content.Load<Texture2D>("Floor");
            Tile.DefaultEndTexture = this.Content.Load<Texture2D>("EndFloor");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Save the previous state of the keyboard and game pad so we can determine single key/button presses

            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();

            UpdatePlayer(gameTime);

            if (levelMap.CheckVictory())
                NextLevel(levelMap);

            base.Update(gameTime);
        }

        private void NextLevel(Map levelMap)
        {
            this.levelMap = new Map(levels[++currentLevel]);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            if (currentKeyboardState.IsKeyDown(Keys.Right) && previousKeyboardState.IsKeyUp(Keys.Right))
            {
                MovePlayer(Direction.Right);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Left) && previousKeyboardState.IsKeyUp(Keys.Left))
            {
                MovePlayer(Direction.Left);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Up) && previousKeyboardState.IsKeyUp(Keys.Up))
            {
                MovePlayer(Direction.Up);
            }

            if (currentKeyboardState.IsKeyDown(Keys.Down) && previousKeyboardState.IsKeyUp(Keys.Down))
            {
                MovePlayer(Direction.Down);
            }
        }


        public void MovePlayer(Direction dir)
        {
            var newPosition = GetNewPosition(dir, Player.Position);
            if (levelMap[newPosition].IsEmpty)
            {
                Player.Position = newPosition;
                return;
            }

            var box = levelMap[newPosition].BoxOnATile;
            if (TryMoveBox(box, dir))
                Player.Position = newPosition;
        }

        private Vector2 GetNewPosition(Direction dir, Vector2 oldPosition)
        {
            var newPosition = oldPosition;
            var height = (levelMap.Height - 1) * 1;
            var width = (levelMap.Width - 1) * 1;
            switch (dir)
            {
                case Direction.Up:
                    {
                        newPosition.Y -= 1;
                        newPosition.Y = MathHelper.Clamp(newPosition.Y, 0, height);
                        break;
                    }
                case Direction.Down:
                    {
                        newPosition.Y += 1;
                        newPosition.Y = MathHelper.Clamp(newPosition.Y, 0, height);
                        break;
                    }
                case Direction.Left:
                    {
                        newPosition.X -= 1;
                        newPosition.X = MathHelper.Clamp(newPosition.X, 0, width);
                        break;
                    }
                case Direction.Right:
                    {
                        newPosition.X += 1;
                        newPosition.X = MathHelper.Clamp(newPosition.X, 0, width);
                        break;
                    }
            }

            return newPosition;
        }

        private bool TryMoveBox(Box box, Direction dir)
        {
            if (box == null) return false;
            var oldPosition = box.Position;
            var newPosition = GetNewPosition(dir, oldPosition);
            if (levelMap[newPosition].IsEmpty)
            {
                levelMap[oldPosition].Empty();
                box.Position = newPosition;
                box.IsOnEndTile = levelMap[newPosition].IsEndTile;
                levelMap[newPosition].BoxOnATile = box;
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here
            spriteBatch.Begin();
            levelMap.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
