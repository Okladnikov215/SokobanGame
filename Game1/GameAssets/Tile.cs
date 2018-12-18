using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SokobanGame.GameAssets;
namespace SokobanGame
{
    public class Tile
    {
        public static Texture2D DefaultTexture { get; set; }
        public static Texture2D DefaultEndTexture { get; set; }
        public bool IsEndTile { get; set; }
        public Box BoxOnATile { get; set; }
        public Texture2D Texture { get; set; }
        public bool IsEmpty
        {
            get
            {
                return (BoxOnATile == null && !(this is Wall));
            }
        }

        public Tile()
        {
            IsEndTile = false;
            BoxOnATile = null;
            Texture = DefaultTexture;
        }

        public void Empty()
        {
            BoxOnATile = null;
        }
    }
}
