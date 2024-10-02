using Godot;
using System.Collections.Generic;

namespace Godot.h8.Palette;

public class PaletteColor
{
    public Color Color;
    public string Name;

    public PaletteColor(Color color, string name)
    {
        this.Color = color;
        this.Name = name;
    }
}

public class ImagePalette
{
	public string PaletteName;
	List<PaletteColor> ColorsInPalette;
	string defaultColorName = "Color";

	public Color this [int index] {
		get {
			return GetColor (index);
		}
		set {
			SetColor (index, value);
		}
	}
	
	public int Count {
		get {
			return ColorsInPalette.Count;
		}
	}

	Color GetColor (int index)
	{
		return ColorsInPalette [index].Color;
	}

	void SetColor (int index, Color color)
	{
		ColorsInPalette [index].Color = color;
	}
	
	public ImagePalette () : this ("RBPalette")
	{
	}

	public ImagePalette (string paletteName)
	{
		this.PaletteName = paletteName;
		this.ColorsInPalette = new List<PaletteColor> ();
	}

	public ImagePalette (ImagePalette paletteToCopy)
	{
		this.PaletteName = paletteToCopy.PaletteName;
		this.ColorsInPalette = new List<PaletteColor> ();
		this.ColorsInPalette.AddRange (paletteToCopy.ColorsInPalette);
	}

	public void AddColor (Color color)
	{
		ColorsInPalette.Add (new PaletteColor (color, defaultColorName + ColorsInPalette.Count));
	}

	public void RemoveColorAtIndex (int index)
	{
		ColorsInPalette.RemoveAt (index);
	}

	public bool ContainsColor (Color colorToFind)
	{
		return IndexOf (colorToFind) >= 0;
	}

	public int IndexOf (Color colorToFind)
	{
		int index = -1;
		for (int i = 0; i < ColorsInPalette.Count; i++) {
			bool colorToFindIsZeroAlpha = Mathf.IsEqualApprox (colorToFind.A, 0.0f);
			bool currentColorIsZeroAlpha = Mathf.IsEqualApprox (ColorsInPalette [i].Color.A, 0.0f);
			if ((colorToFindIsZeroAlpha && currentColorIsZeroAlpha) || ColorsInPalette [i].Color == colorToFind) {
				index = i;
				break;
			}
		}

		return index;
	}

	public static ImagePalette CreatePaletteFromTexture(Texture2D sourceTexture)
	{
		var sourcePixels = sourceTexture.GetImage();
		ImagePalette palette = new ImagePalette ();
		palette.PaletteName = sourceTexture.ResourcePath.GetBaseName().GetFile();
		// Get all unique colors
		for (int i = 0; i < sourcePixels.GetWidth(); i++)
		{
			for (int j = 0; j < sourcePixels.GetHeight(); j++)
			{
				Color colorAtSource = ClearRGBIfNoAlpha(sourcePixels.GetPixel(i, j));
				if (!palette.ContainsColor(colorAtSource))
				{
					palette.AddColor(colorAtSource);
				}
			}
		}
		
		return palette;
	}

    /// <summary>
    /// Clears the RGB if there is no alpha. This is useful to keep duplicate full-transparent colors out of the palette
    /// </summary>
    /// <returns>If color has no alpha returns black with 0 alpha, otherwise returns the original color.</returns>
    /// <param name="colorToClear">Color to clear.</param>
    static Color ClearRGBIfNoAlpha (Color colorToClear)
	{
		Color clearedColor = colorToClear;
		if (Mathf.IsEqualApprox (clearedColor.A, 0.0f)) {
			clearedColor = Color.Color8(0,0,0,0);
		}
		return clearedColor;
	}

	public override string ToString ()
	{
		string fullString = "";
		fullString += "[RBPalette: Name=" + PaletteName  + " Count=" + Count + " Colors=";
		for (int i =0; i < Count; i++) {
			string colorString = "{";
			colorString += ColorsInPalette[i].Color;
			colorString += "}";
			fullString += colorString;
		}
		return fullString;
	}
}