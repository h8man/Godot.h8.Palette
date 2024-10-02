using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileMapPlugin;

namespace Godot.h8.Palette;

[Tool]
[GlobalClass]
internal partial class ImagePaletteDock: Control
{
    [Export]
    public TextureRect SourceTextureRect {  get; set; }
    [Export]
    public TextureRect IndexMapRect { get; set;}
    [Export]
    public TextureRect PaletteRect {  get; set;}
    [Export]
    public Button ClearPalette { get; set; }
    [Export]
    public Button ExtractPalette { get; set; }
    [Export]
    public Button ExtractIndexMap { get; set; }
    [Export]
    public Button SaveMap { get; set; }
    [Export]
    public Button SavePalette { get; set; }

    private ImagePalette ImagePalette { get; set; }
    private IndexMap IndexMap { get; set; }

    public override void _Ready()
    {
        base._Ready();
        ClearPalette.Pressed += OnClearPalette;
        ExtractPalette.Pressed += OnExtractPalette;
        ExtractIndexMap.Pressed += OnExtractIndexMap;
        SaveMap.Pressed += OnSaveMap;
        SavePalette.Pressed += OnSavePalette;
    }

    private void OnSavePalette()
    {
        if (ImagePalette == null)
        {
            ShowDialog("Drag Image Palette texture2d or Extract first.");
            return;
        }
        var path = SourceTextureRect.Texture.ResourcePath.GetBaseName();
        var image = ImagePaletteExtensios.CreateImage(ImagePalette);
        image.SavePng(path + "_palette.png");
        EditorInterface.Singleton.GetResourceFilesystem().Scan();

    }

    private void OnSaveMap()
    {
        if (IndexMap == null)
        {
            ShowDialog("Extract Index Map first.");
            return;
        }
        var path = SourceTextureRect.Texture.ResourcePath.GetBaseName();
        IndexMap.Image.SavePng(path + "_imap.png");
        EditorInterface.Singleton.GetResourceFilesystem().Scan();

    }

    private void OnExtractIndexMap()
    {
        if (SourceTextureRect.Texture == null)
        {
            ShowDialog("Select Source texture2d for edit in file tree.");
            return;
        }
        if (ImagePalette == null)
        {
            ShowDialog("Drag Image Palette texture2d or Extract first.");
            return;
        }

        IndexMap = IndexMap ?? new IndexMap();
        IndexMap.Map(SourceTextureRect.Texture, ImagePalette);
        IndexMapRect.Texture = ImageTexture.CreateFromImage(IndexMap.Image);
    }

    public void OnExtractPalette()
    {
        if (SourceTextureRect.Texture == null)
        {
            ShowDialog("Select Source texture2d for edit in file tree.");
            return;
        }

        ImagePalette = ImagePalette.CreatePaletteFromTexture(SourceTextureRect.Texture);
        PaletteRect.Texture = ImagePaletteExtensios.CreateTexture(ImagePalette);
    }
    public void OnClearPalette()
    {
        PaletteRect.Texture = null;
        ImagePalette = null;
    }
    public void SetSourceTexture(Texture2D texture)
    {
        SourceTextureRect.Texture = texture;
    }
    public override void _DropData(Vector2 atPosition, Variant data)
    {
        var file = GetFile(data);
        if (file == null)
        {
            return;
        }
        var texture = ResourceLoader.Load<Texture2D>(file);

        ImagePalette = ImagePalette.CreatePaletteFromTexture(texture);
        PaletteRect.Texture = texture;
    }
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return IsTexture(data);
        //return data.AsGodotObject() is Texture2D;
    }
    private void ShowDialog(string message)
    {
        var dialog = new AcceptDialog() { Title = "Warning", DialogText = message };
        this.AddChild(dialog);
        dialog.Owner = this;
        dialog.PopupCentered();
    }

    private static bool IsTexture(Variant data)
    {
        var file = GetFile(data);
        return EditorInterface.Singleton.GetResourceFilesystem().GetFileType(file)?.Contains("Texture2D") ?? false;
    }

    private static string GetFile(Variant data)
    {
        var dic = data.AsGodotDictionary<string, Variant>();
        if (dic == null)
        {
            return null;
        }
        var files = dic["files"].AsGodotArray<string>();
        if (files == null || files.Count != 1)
        {
            return null;
        }
        return files[0];
    }
}
