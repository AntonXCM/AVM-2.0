shader_type canvas_item;

void fragment() {
	float texSize = float(textureSize(TEXTURE, 0).x);
	float x = UV.x * texSize - 0.5;
	float invTexSize = 1.0 / texSize;
	vec4 left = texture(TEXTURE, vec2((floor(x) + 0.5) * invTexSize, UV.y));
	vec4 right = texture(TEXTURE, vec2((ceil(x) + 0.5) * invTexSize, UV.y));

    COLOR = mix(left, right, fract(x));
}