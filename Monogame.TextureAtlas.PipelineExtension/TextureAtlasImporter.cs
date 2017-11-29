using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Monogame.TextureAtlas;

namespace Monogame.TextureAtlas.PipelineExtension
{
    public class AtlasDeclaration
    {
        public string Name { get; }
        public string AtlasRootDir { get; }
        public string[] Images { get; }
        public AtlasDeclaration(string atlasRootDir, string atlasName, string[] v)
        {
            this.Name = atlasName;
            this.AtlasRootDir = atlasRootDir;
            this.Images = v;
        }
    }

    public class AtlasContent
    {
        public Sprite[] Sprites { get; internal set; }
        public ExternalReference<TextureContent> Texture { get; internal set; }
    }


    [ContentImporter(".atlas", DisplayName = "Texture Atlas Importer", DefaultProcessor = "TextureAtlasProcessor")]
    public class TextureAtlasImporter : ContentImporter<AtlasDeclaration>
    {

        public override AtlasDeclaration Import(string filename, ContentImporterContext context)
        {
            // TODO: process the input object, and return the modified data.
            var folders = File.ReadLines(filename);
            //.Select(s => Path.GetFullPath(Path.Combine(Path.GetDirectoryName(filename), s)));
            var rootDir = Path.GetDirectoryName(filename);
            List<string> pngList = new List<string>();
            foreach (var f in folders)
            {
                var pngs = Directory.EnumerateFiles(Path.GetFullPath(Path.Combine(rootDir, f))).Where(w => w.EndsWith(".png")).Select(s => s.Substring(rootDir.Length + 1));//, SearchOption.TopDirectoryOnly);
                foreach (var p in pngs)
                {
                    pngList.Add(p);
                    context.AddDependency(Path.Combine(rootDir, p));
                }
            }

            return new AtlasDeclaration(rootDir, Path.GetFileNameWithoutExtension(filename), pngList.Select(s => s.Replace('\\', '/')).ToArray());
        }

    }

}
