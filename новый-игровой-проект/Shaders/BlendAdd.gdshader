shader_type canvas_item;

render_mode blend_add;
uniform sampler2D flares : filter_nearest, repeat_enable;
uniform vec2 flares_chop;
instance uniform float offset;
void fragment() {
    COLOR = texture(TEXTURE, UV) * texture(flares, UV * flares_chop + vec2(offset, 0));
}