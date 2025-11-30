shader_type canvas_item;

uniform sampler2D fog_tex : source_color;
uniform vec2 fog_scroll_velocity = vec2(0.02, 0.015);
uniform float fog_strength : hint_range(0.0, 1.0) = 1.0;

void fragment() {
    // fog mask
	vec4 base = texture(TEXTURE, UV);
    float mask = base.r;

    // compute scrolling UVs to wrap automatically
    vec2 scroll_uv = fract(UV + fog_scroll_velocity * TIME);

    vec4 fog = texture(fog_tex, scroll_uv);

    // apply fog overlay texture to black pixels
    float blend = 1.0 - mask;
    vec4 result = mix(vec4(0.0), fog, blend * fog_strength);

    COLOR = result;
}
