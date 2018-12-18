using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SokobanGame
{
    public class Map
    {
        public Tile[,] Tiles;
        public List<Box> Boxes;
        public int Height;
        public int Width;
        public static int tileSize = 100;

        public Map(char[,] levelMap)
        {
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
            for (int i = 0; i < Width; i++)
                for (int j = 0; j < Height; j++)
                {
                    var currentTile = Tiles[i, j];
                    var destinationRectangle = new Rectangle(i * tileSize, j * tileSize, tileSize, tileSize);
                    var isEndTile = currentTile.IsEndTile;
                    var currentTileTexture = (isEndTile) ? Tile.DefaultEndTexture : Tile.DefaultTexture;
                    spriteBatch.Draw(currentTileTexture, destinationRectangle, Color.White);

                    if (currentTile.BoxOnATile != null)
                    {
                        var tintColor = (isEndTile) ? Color.MediumVioletRed : Color.SaddleBrown;
                        spriteBatch.Draw(Box.DefaultTexture, destinationRectangle, tintColor);
                    }
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