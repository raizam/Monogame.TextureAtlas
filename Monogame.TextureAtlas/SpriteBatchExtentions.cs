using Monogame.TextureAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.Graphics
{
    public static class SpriteBatchExtentions
    {
        public static void Draw(this SpriteBatch sp, Sprite sprite, Vector2 position, Color color)
        {
            var texture = sprite.Atlas.Texture;
            var sourceRectangle = sprite.Source;
            sp.Draw(texture, new Vector2(position.X + sprite.Offset.X, position.Y + sprite.Offset.Y), sourceRectangle, color);
        }
    }
}
