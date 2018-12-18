using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SokobanGame.States
{
    class GameState : State
    {
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;
        Map levelMap;
        int currentLevel;
        List<char[,]> levels;

        public GameState(SokobanGame game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            currentLevel = 0;
            levels = new List<char[,]>();
            LoadContent();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            levels = LoadLevels();
            levelMap = new Map(levels[currentLevel]);

            Box.DefaultTexture = this.content.Load<Texture2D>("box");
            Player.Texture = this.content.Load<Texture2D>("Player");
            Tile.DefaultTexture = this.content.Load<Texture2D>("Floor");
            Tile.DefaultEndTexture = this.content.Load<Texture2D>("EndFloor");
        }

        private List<char[,]> LoadLevels()
        {
            var levels = new List<char[,]>();
            var charMap = new char[3, 3] { { 'e', 'e', 'e' }, { 'p', 'b', 'f' }, { 'e', 'b', 'f' } };
            levels.Add(charMap);
            charMap = new char[2, 2] { { 'p', 'f' }, { 'f', 'b' } };
            levels.Add(charMap);

            return levels;
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                game.ChangeState(new MenuState(game, graphicsDevice, content));

            UpdatePlayer(gameTime);

            if (levelMap.CheckVictory())
                NextLevel(levelMap);
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
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            levelMap.Draw(spriteBatch);
            Player.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }

}
