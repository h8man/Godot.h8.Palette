shader_type canvas_item;

uniform sampler2D indexMap : filter_nearest, repeat_disable;
uniform sampler2D palette : filter_nearest, repeat_disable;
uniform float paletteIdx = 0;
uniform float paletteIdy = 0;

void fragment() {
	vec4 paletteMapColor = texture(indexMap, UV);
	float colorIdx = paletteMapColor.r+paletteIdy;
	vec2 paletteUV = vec2(colorIdx, paletteIdx);
	vec4 outColor = texture(palette,paletteUV);
	outColor = vec4(outColor.rgb,paletteMapColor.a);
	COLOR = outColor;
}