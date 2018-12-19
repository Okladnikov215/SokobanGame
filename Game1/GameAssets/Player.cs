using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame
{
    static class Player
    {
        public static Texture2D Texture { get; set; }
        public static Vector2 Position { get; set; }
        public static string Name { get; set; }

        public static void Draw(SpriteBatch spriteBatch)
        {
            var tileSize = int.Parse(GameSettings.TileSize);
            int xPos = (int)Position.X * tileSize;
            int yPos = (int)Position.Y * tileSize;
            var destRectangle = new Rectangle(xPos, yPos, tileSize, tileSize);
            spriteBatch.Draw(Texture, destRectangle, Color.White);
        }

        public static void Initialize(Texture2D texture, Vector2 position)
        {
            Position = position;
            Texture = texture;
            Name = GameSettings.PlayerName;
        }
    }
}
