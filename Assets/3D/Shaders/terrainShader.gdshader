shader_type spatial;

uniform vec4 albedo_colour : source_color = vec4(1.0, 1.0, 1.0, 1.0);

uniform sampler2D texture_albedo : source_color;
uniform sampler2D texture_roughness : hint_roughness_r;
uniform sampler2D texture_normal : filter_linear_mipmap;
uniform float normal_strength : hint_range(0.0, 2.0) = 1.0;
uniform float roughness_strength : hint_range(0.0, 2.0) = 1.0;

// voronoi settings
uniform float base_scale : hint_range(0.1, 10000) = 10.0;
uniform float scale : hint_range(0.1, 10000.0) = 1.0;
uniform float voronoi_random : hint_range(0.0, 1.0, 0.1) = 0.5;

#define BASE_SCALE base_scale
#define VORONOI_SCALE scale
#define VORONOI_ROTATION PI * 2.0
#define VORONOI_SCALE_RANDOM voronoi_random
#include "res://addons/voronoi_scatter/voronoi.gdshaderinc"

void fragment() {
	VORONOI_INIT
	ALBEDO = VORONOI_TEXTURE_SAMPLE(texture_albedo).rgb * albedo_colour.rgb;
	ROUGHNESS = VORONOI_TEXTURE_SAMPLE(texture_roughness).r * roughness_strength;
	NORMAL_MAP = VORONOI_NORMAL_TEXTURE_SAMPLE(texture_normal) * normal_strength;
}