# Monogame.TextureAtlas

This monogame pipeline extension packs png files into a texture atlas. 
The packing code was taken from https://spritesheetpacker.codeplex.com/

## Usage

1. Using the content pipeline tool, add `Monogame.TextureAtlas.PipelineExtension.dll` to your mgcb
2. Create a text file with `.atlas` extension, this is used to specify a list of target folders containing the pngs. One subfolder per line, currently absolute paths not supported, just use `myimagefolder` or `myimagefolder/myothersubfolder`
3. From the pipeline tool, add the `.atlas` to your mgcb, and configure just like a regular texture. Build

4. In your C# Monogame project add a reference to `Monogame.TextureAtlas' and load the atlas using the `ContentManager`:
```csharp
var myAtlas = Content.Load<TextureAtlas>("myatlas");
//then retrieves the sprites
this.Sprites = myAtlas.Sprites
```

5. Then using a SpriteBatch extension, draw a sprite:
```csharp
 SpriteBatch.Draw(Sprites[0], new Vector2(0, 0), Color.White);
```
