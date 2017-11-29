using System;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Monogame.TextureAtlas;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Drawing.Imaging;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using sspack;

namespace Monogame.TextureAtlas.PipelineExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentProcessor(DisplayName = "TextureAtlas Processor")]
    public class TextureAtlasProcessor : ContentProcessor<AtlasDeclaration, AtlasContent>
    {

        Color colorKeyColor;
        bool colorKeyEnabled;
        bool generateMipmaps;
        bool premultiplyTextureAlpha;
        TextureProcessorOutputFormat textureFormat;

        /// <summary>
        /// Gets or sets the color value to replace with transparent black.
        /// </summary>
        /// <value>Color value of the material to replace with transparent black.</value>
        [DefaultValue(typeof(Color), "255, 0, 255, 255")]
        [DisplayName("Color Key Color")]
        [Description("If the texture is color-keyed, pixels of this color are replaced with transparent black.")]
        public virtual Color ColorKeyColor { get { return colorKeyColor; } set { colorKeyColor = value; } }

        /// <summary>
        /// Specifies whether color keying of a texture is enabled.
        /// </summary>
        /// <value>true if color keying is enabled; false otherwise.</value>
        [DefaultValue(true)]
        [DisplayName("Color Key Enabled")]
        [Description("If enabled, the source texture is color-keyed. Pixels matching the value of \"Color Key Color\" are replaced with transparent black.")]
        public virtual bool ColorKeyEnabled { get { return colorKeyEnabled; } set { colorKeyEnabled = value; } }


        /// <summary>
        /// Specifies if a full chain of mipmaps are generated from the source material. Existing mipmaps of the material are not replaced.
        /// </summary>
        /// <value>true if mipmap generation is enabled; false otherwise.</value>
        [DefaultValue(false)]
        [DisplayName("Generate Mipmaps")]
        [Description("If enabled, a full chain of mipmaps are generated from the source material. Existing mipmaps of the material are not replaced.")]
        public virtual bool GenerateMipmaps { get { return generateMipmaps; } set { generateMipmaps = value; } }

        /// <summary>
        /// Specifies whether alpha premultiply of textures is enabled.
        /// </summary>
        /// <value>true if alpha premultiply is enabled; false otherwise.</value>
        [DefaultValue(true)]
        [DisplayName("Premultiply Alpha")]
        [Description("If enabled, the texture is converted to premultiplied alpha format.")]
        public virtual bool PremultiplyTextureAlpha { get { return premultiplyTextureAlpha; } set { premultiplyTextureAlpha = value; } }


        /// <summary>
		/// Specifies the texture format of output materials. Materials can either be left unchanged from the source asset, converted to a corresponding Color, or compressed using the appropriate DxtCompressed format.
        /// </summary>
        /// <value>The texture format of the output.</value>
        [DefaultValue(typeof(TextureProcessorOutputFormat), "Color")]
        [DisplayName("Texture Format")]
        [Description("Specifies the SurfaceFormat type of processed textures. Textures can either remain unchanged from the source asset, converted to the Color format, or DXT compressed.")]
        public virtual TextureProcessorOutputFormat TextureFormat { get { return textureFormat; } set { textureFormat = value; } }


        protected virtual ExternalReference<TextureContent> BuildTexture(string textureName, ExternalReference<TextureContent> texture, ContentProcessorContext context)
        {
            var parameters = new OpaqueDataDictionary();
            parameters.Add("ColorKeyColor", ColorKeyColor);
            parameters.Add("ColorKeyEnabled", ColorKeyEnabled);
            parameters.Add("GenerateMipmaps", GenerateMipmaps);
            parameters.Add("PremultiplyAlpha", PremultiplyTextureAlpha);
            parameters.Add("ResizeToPowerOfTwo", false);
            parameters.Add("TextureFormat", TextureFormat);
            texture.Name = textureName;
            var result = context.BuildAsset<TextureContent, TextureContent>(texture, "TextureProcessor", parameters, "TextureImporter", textureName);
            //result.Filename = textureName;
            return result;
        }



        public override AtlasContent Process(AtlasDeclaration input, ContentProcessorContext context)
        {


            Dictionary<int, Bitmap> images = new Dictionary<int, Bitmap>();
            Dictionary<int, string> imageNames = new Dictionary<int, string>();


            ImagePacker imagePacker = new ImagePacker();
            var imgFiles = input.Images.Select(i => Path.Combine(input.AtlasRootDir, i.Replace('/', '\\')));

            if (imgFiles.Count() == 0)
                throw new ArgumentException("No Image found");
            Bitmap output;
            Dictionary<string, Sprite> map;
            imagePacker.PackImage(imgFiles, true, true, 4096, 4096, 0, true, out output, out map);


            var finalSprites = map.Select(s => { s.Value.Name = s.Key.Substring(0, s.Key.LastIndexOf('.')).Substring(input.AtlasRootDir.Length + 1).Replace('\\', '/').Trim('.', '/'); return s.Value; }).ToArray();
            var atlasPngPath = Path.Combine(input.AtlasRootDir, input.Name + ".png");

            using (FileStream outputSpriteFile = new FileStream(atlasPngPath, FileMode.Create))
            {
                output.Save(outputSpriteFile, ImageFormat.Png);
            }
            context.AddOutputFile(atlasPngPath);
            ExternalReference<TextureContent> texture = new ExternalReference<TextureContent>(atlasPngPath);
            texture = BuildTexture($"{input.Name}Texture", texture, context);

            return new AtlasContent { Texture = texture, Sprites = finalSprites };
        }
    }

   
}