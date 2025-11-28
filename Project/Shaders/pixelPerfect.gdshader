shader_type canvas_item;

uniform float speed;
uniform float texturePixelScaling = 0.1;
uniform float blurSize = 0.1;

vec4 mode(vec4 c0, vec4 c1, vec4 c2, vec4 c3, vec4 c4) {
    return (c0+c1+c2+c3+c4) * vec4(0.2,0.2,0.2,999.0);
}
vec2 rotateUV(vec2 uv, float angle, vec2 texturePixelSize) {
    uv = floor(uv / texturePixelSize) * texturePixelSize + texturePixelSize * 0.5;
    float c = cos(angle), s = sin(angle);
	return vec2(uv.x*c - uv.y*s, uv.x*s + uv.y*c) + 0.5;
}

void fragment() {
    vec2 uv = UV - 0.5;
	float angle = speed * TIME;
	vec2 texturePixelSize = TEXTURE_PIXEL_SIZE * texturePixelScaling;
    COLOR = mode(
		texture(TEXTURE, rotateUV(uv, angle, texturePixelSize)),
		texture(TEXTURE, rotateUV(uv, angle + blurSize, texturePixelSize)),
		texture(TEXTURE, rotateUV(uv, angle + blurSize * 2.0, texturePixelSize)),
		texture(TEXTURE, rotateUV(uv, angle + blurSize * 3.0, texturePixelSize)),
		texture(TEXTURE, rotateUV(uv, angle + blurSize * 4.0, texturePixelSize)));
}
