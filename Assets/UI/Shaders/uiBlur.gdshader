shader_type canvas_item;

uniform float blur_amount : hint_range(0.0, 10.0) = 2.0;
uniform float mix_amount : hint_range(0.0, 1.0) = 0.5;
uniform float saturation : hint_range(0.0, 2.0) = 1.2;
uniform float grain_amount : hint_range(0.0, 0.1) = 0.02;
uniform vec4 color_over : source_color;
// The BackBufferCopy will feed this texture
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture, filter_linear_mipmap;

float noise(vec2 uv) {
    return fract(sin(dot(uv.xy, vec2(12.9898, 78.233))) * 43758.5453123);
}

void fragment() {
    // Sampling with SCREEN_UV ensures we grab the exact pixels
    // located behind this specific fragment on the screen.
    vec4 blurred = textureLod(SCREEN_TEXTURE, SCREEN_UV, blur_amount);

    float luminance = dot(blurred.rgb, vec3(0.2126, 0.7152, 0.0722));
    blurred.rgb = mix(vec3(luminance), blurred.rgb, saturation);

    vec4 fin = mix(blurred, color_over, mix_amount);

    // Grain stays static relative to the screen
    float n = (noise(SCREEN_UV) - 0.5) * grain_amount;
    fin.rgb += n;

    COLOR = vec4(fin.rgb, 1.0);
}