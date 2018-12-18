using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SokobanGame
{
    public class Box
    {
        public static Texture2D DefaultTexture { get; set; }
        public Vector2 Position { get; set; }
        public bool IsOnEndTile { get; internal set; }

        public Box(Vector2 position)
        {
            Position = position;
        }
    }
}
