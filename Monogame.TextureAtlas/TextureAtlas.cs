using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame.TextureAtlas
{
    public class Sprite
    {
        public string Name { get; internal set; }
        public Rectangle Source { get; internal set; }
        public Point Offset { get; internal set; }
        public Point Size { get; internal set; }
        public TextureAtlas Atlas { get; internal set; }
    }

    public class TextureAtlas
    {
        public Texture2D Texture { get; internal set; }
        public Sprite[] Regions { get; internal set; }
    }
}
