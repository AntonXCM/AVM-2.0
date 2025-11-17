shader_type canvas_item;
uniform vec2 power = vec2(600, 0);
uniform vec2 powerA = vec2(2, 0);
uniform sampler2D snapMap;
uniform vec4 alphaColor : source_color;
void fragment() {
	vec2 offset = (SCREEN_UV.x - 0.5) * texture(snapMap, UV).r * SCREEN_PIXEL_SIZE;

	vec4 color = texture(TEXTURE, UV - offset * power);
    COLOR = vec4(mix(alphaColor.rgb, color.rgb, color.a), texture(TEXTURE, UV - offset * powerA).a);
}