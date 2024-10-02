using Godot;
using System;

namespace Godot.h8.Palette;

[GlobalClass]
[Tool]
public partial class PaletteCycling : Node
{
    [Export]
    public CanvasItem CanvasItem { get; set; }
    [Export]
    public float ScrollRate { get; set; }

    private const string PALETTE_IDX = "paletteIdx";
    //private ShaderMaterial _material;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        var _material = CanvasItem?.Material as ShaderMaterial;
        if (_material != null)
        {
            var deltaY = ScrollRate * delta;
            var paletteY = _material.GetShaderParameter(PALETTE_IDX);
            paletteY = (paletteY.As<float>() + deltaY) % 1.0f;
            _material.SetShaderParameter(PALETTE_IDX, paletteY);
        }
    }
}
