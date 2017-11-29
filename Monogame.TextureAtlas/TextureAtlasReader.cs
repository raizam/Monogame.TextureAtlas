using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame.TextureAtlas
{
    public class TextureAtlasReader : ContentTypeReader<TextureAtlas>
    {
        protected override TextureAtlas Read(ContentReader input, TextureAtlas existingInstance)
        {
            TextureAtlas a = existingInstance == null ? new TextureAtlas() : existingInstance;

            a.Texture = (Texture2D)input.ReadExternalReference<Texture>();

            Sprite[] regions = new Sprite[input.ReadInt32()];

            for (int i = 0; i < regions.Length; i++)
            {
                var name = input.ReadString();
                var x = input.ReadInt32();
                var y = input.ReadInt32();
                var w = input.ReadInt32();
                var h = input.ReadInt32();

                Point offset = new Point(input.ReadInt32(), input.ReadInt32());
                Point size = new Point(input.ReadInt32(), input.ReadInt32());
                Rectangle source = new Rectangle(x, y, w, h);

                var region = a.Regions?.FirstOrDefault(f => f.Name.Equals(name)) ?? new Sprite { Name = name };

                region.Source = source;
                region.Offset = offset;
                region.Size = size;
                region.Atlas = a;
                regions[i] = region;
            }

            a.Regions = regions;

            return a;
        }
    }
}
