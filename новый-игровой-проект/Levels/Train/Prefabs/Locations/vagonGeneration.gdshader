shader_type canvas_item;

uniform float speed = 0.6;
uniform float amount = 1.0;
uniform float assembleFreq1 = 1.5;
uniform float assembleFreq2 = 2;
void fragment() {
    vec2 texSz = vec2(textureSize(TEXTURE, 0));
    vec2 px = UV * texSz;
    vec2 cell = floor(px);
    vec2 cell_center_uv = (cell + 0.5) / texSz;
	
	
	float h = fract(sin(dot(cell, vec2(12.9898,78.233))) * 43758.5453);
    float assembleFactor = 2.0 + sin((SCREEN_UV.x + TIME) * assembleFreq1) + cos((SCREEN_UV.y + TIME) * assembleFreq2);
    float t = PI * 2.0 * fract(TIME * speed + h) + assembleFactor;

    float angleX = sin(t);
	float angleY = cos(t);

    // смещение в UV, выражено в клетках * размер клетки / размер текстуры
    vec2 disp = vec2(angleX, angleY) * (amount * assembleFactor) / texSz;
	vec4 orig = texture(TEXTURE, cell_center_uv);
	vec4 offset = texture(TEXTURE, cell_center_uv + disp);
    COLOR = vec4(offset.r, orig.g, offset.ba);
}
