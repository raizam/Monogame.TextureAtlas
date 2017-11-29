using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Monogame.TextureAtlas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame.TextureAtlas.PipelineExtension
{
    [ContentTypeWriter]
    public class TextureAtlasWriter : ContentTypeWriter<AtlasContent>
    {
        protected override void Write(ContentWriter output, AtlasContent value)
        {
            output.WriteExternalReference(value.Texture);
            var l = value.Sprites.Length;
            output.Write(l);

            for (int i = 0; i < l; i++)
            {
                var current = value.Sprites[i];
                output.Write(current.Name);
                output.Write(current.Source.X);
                output.Write(current.Source.Y);
                output.Write(current.Source.Width);
                output.Write(current.Source.Height);
                output.Write(current.Offset.X);
                output.Write(current.Offset.Y);
                output.Write(current.Size.X);
                output.Write(current.Size.Y);
            }
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return typeof(TextureAtlas).FullName;
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Monogame.TextureAtlas.TextureAtlasReader, Monogame.TextureAtlas";
        }
    }
}
