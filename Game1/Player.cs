using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame
{
    static class Player
    {
        public static Texture2D Texture;
        public static Vector2 Position;

        public static void Draw(SpriteBatch spriteBatch)
        {
            int xPos = (int)Position.X * Map.tileSize;
            int yPos = (int)Position.Y * Map.tileSize;
            var destRectangle = new Rectangle(xPos, yPos, Map.tileSize, Map.tileSize);
            spriteBatch.Draw(Texture, destRectangle, Color.White);
        }

        public static void Initialize(Texture2D texture, Vector2 position)
        {
            Position = position;
            Texture = texture;
        }
    }
}
