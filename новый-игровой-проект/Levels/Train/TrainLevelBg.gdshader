shader_type canvas_item;

uniform vec2 light_pos;
uniform sampler2D suntexture : filter_nearest;
uniform sampler2D forgroundtexture : filter_nearest;
uniform float decay = 0.95;
uniform float exposure : hint_range(0.0,5.0) = 0.3;
uniform float density = 0.8;

void fragment() {
	vec4 godrays = vec4(0);
	vec2 delta = (light_pos - UV) * density;
	vec2 coord = UV;
	float illum = 1.0;
	
    for (int i = 0; i < 16; i++) {
        coord += delta;
        godrays += clamp(texture(suntexture, coord) - texture(forgroundtexture, coord).a,0,1) * illum;
        illum *= decay;
    }

	vec4 forgroundcolor = texture(forgroundtexture, UV);
	vec4 suncolor = texture(suntexture, UV);
	float forgroundAinv = 1.0 - forgroundcolor.a;
	float alpha = forgroundcolor.a + suncolor.a * forgroundAinv;
	vec4 color = vec4(
		(forgroundcolor.rgb + suncolor.rgb * forgroundAinv) / alpha,
		alpha);

    COLOR = color + godrays * exposure * alpha;
}
