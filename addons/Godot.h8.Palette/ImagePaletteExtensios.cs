using Godot;

namespace Godot.h8.Palette;

public class ImagePaletteExtensios
{
    public static Image CreateImage(ImagePalette palette)
    {
        var image = Image.CreateEmpty(palette.Count, 1, false, format: Image.Format.Rgba8);

        for (int i = 0; i < palette.Count; i++)
        {
            image.SetPixel(i, 0, palette[i]);
        }
        return image;
    }

    public static Texture2D CreateTexture(ImagePalette palette)
    {
        return ImageTexture.CreateFromImage(CreateImage(palette));
    }
}