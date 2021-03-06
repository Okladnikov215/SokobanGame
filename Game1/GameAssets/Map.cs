﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using SokobanGame.GameAssets;

namespace SokobanGame
{
    public class Map
    {
        public Tile[,] Tiles;
        public List<Box> Boxes;
        public int Height;
        public int Width;
        public int tileSize;

        public Map(char[,] levelMap)
        {
            tileSize = int.Parse(GameSettings.TileSize);
            Boxes = new List<Box>();
            Width = levelMap.GetLength(0);
            Height = levelMap.GetLength(1);
            Tiles = new Tile[Width, Height];
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    var newTile = new Tile();
                    Tiles[i, j] = newTile;

                    if (levelMap[i, j] == 'b')
                    {
                        var newBox = new Box(new Vector2(i, j));
                        Boxes.Add(newBox);
                        Tiles[i, j].BoxOnATile = newBox;
                    }

                    if (levelMap[i, j] == 'w')
                    {
                        Tiles[i, j] = new Wall();
                        Tiles[i, j].Texture = Wall.DefaultTexture;
                    }

                    if (levelMap[i, j] == 'f')
                    {
                        Tiles[i, j].IsEndTile = true;
                    }

                    if (levelMap[i, j] == 'p')
                    {
                        Player.Position = new Vector2(i, j);
                    }

                }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawOuterWall(spriteBatch);
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    var currentTile = Tiles[i, j];
                    var destinationRectangle = new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize);
                    var isEndTile = currentTile.IsEndTile;
                    var currentTileTexture = (isEndTile) ? Tile.DefaultEndTexture : currentTile.Texture;
                    spriteBatch.Draw(currentTileTexture, destinationRectangle, Color.White);

                    if (currentTile.BoxOnATile != null)
                    {
                        var tintColor = (isEndTile) ? Color.MediumVioletRed : Color.SaddleBrown;
                        spriteBatch.Draw(Box.DefaultTexture, destinationRectangle, tintColor);
                    }
                }
        }

        private void DrawOuterWall(SpriteBatch spriteBatch)
        {
            var tintColor = Color.MediumVioletRed;
            var texture = Wall.DefaultTexture;
            for (int i = -1; i <= Width; i += 1)
            {
                var destinationRectangle = new Rectangle(i * tileSize, Width * tileSize, tileSize, tileSize);
                spriteBatch.Draw(texture, destinationRectangle, tintColor);
            }

            for (int j = -1; j <= Height; j += 1)
            {
                var destinationRectangle = new Rectangle(Width * tileSize, j * tileSize, tileSize, tileSize);
                spriteBatch.Draw(texture, destinationRectangle, tintColor);
            }
        }

        public Tile this[Vector2 position]
        {
            get
            {
                return Tiles[(int)position.X, (int)position.Y];
            }
        }

        internal bool CheckVictory()
        {
            foreach (var box in Boxes)
            {
                if (!box.IsOnEndTile) return false;
            }
            return true;
        }
    }
}