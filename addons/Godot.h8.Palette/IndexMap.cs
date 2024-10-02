using Godot;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace Godot.h8.Palette;

class IndexMap
{
    public Image Image { get; private set; }
    int width;
    int height;

    public void Map(Texture2D sourceTexture, ImagePalette basePalette)
    {
        this.width = sourceTexture.GetWidth();
        this.height = sourceTexture.GetHeight();

        var sourcePixels = sourceTexture.GetImage();
        Image = Image.CreateEmpty(width, height, false, Image.Format.Rgba8);

        // Remap original colors to point to indeces in the palette
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                // Get the alpha value by looking it up in the paletteKey
                var curentColor = sourcePixels.GetPixel(i, j);

                int paletteIndex = basePalette.IndexOf(curentColor);
                if (paletteIndex < 0)
                {
                    Vector2 coordinateFromBottomLeft = new Vector2(i, j);

                    throw new System.ArgumentException("Encountered color in source PaletteMap image that is not in the base palette." +
                                                        " Color in PaletteMap: " + curentColor +
                                                        " At coordinate: " + coordinateFromBottomLeft);
                }
                float scale;
                if (basePalette.Count == 1)
                {
                    scale = 0.0f;
                }
                else
                {
                    scale = paletteIndex / (float)(basePalette.Count - 1);
                    // For some reason, 1.0f wraps around in the shader. Maybe it's epsilon issues.
                    scale = Mathf.Clamp(scale, 0.0f, 0.99f);
                }
                //Image.SetPixel(i,j, new Color(0.0f, 0.0f, 0.0f, alpha));
                Image.SetPixel(i, j, new Color(scale, scale, scale, curentColor.A));
            }
        }
    }

    //public Image CreateAsImage()
    //{
    //	var paletteMap = Image.CreateEmpty(width, height, false, Image.Format.Rgb8);
    //	for (int i = 0; i < width; i++)
    //	{
    //		for (int j = 0; j < height; j++)
    //		{
    //			paletteMap.SetPixel(i,j,)
    //		}
    //	}
    //       return paletteMap;
    //}
}
