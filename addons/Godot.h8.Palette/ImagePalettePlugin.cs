#if TOOLS
using Godot;
using System;
namespace Godot.h8.Palette;

[Tool]
public partial class ImagePalettePlugin : EditorPlugin
{
    private ImagePaletteDock _imagePaletteDock;
    public override void _Edit(GodotObject @object)
    { 
        base._Edit(@object);
        var texture = @object as Texture2D;
        if (texture != null)
        {
            _imagePaletteDock.SetSourceTexture(texture);

        }
    }
    public override bool _Handles(GodotObject @object)
    {
        return @object is Texture2D;
    }
    public override void _EnterTree()
    {
        var scene = GD.Load<PackedScene>("res://addons/Godot.h8.Palette/ImagePalatteDock.tscn");
        _imagePaletteDock = scene.Instantiate<ImagePaletteDock>();

        AddControlToDock(DockSlot.LeftUr, _imagePaletteDock);
    }

    public override void _ExitTree()
    {
        RemoveControlFromDocks(_imagePaletteDock);
        _imagePaletteDock.Free();
    }
}
#endif
