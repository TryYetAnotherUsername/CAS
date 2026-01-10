shader_type spatial;


// --- VORONOI SCATTERING CONFIGURATION (Required Defines for Plugin) ---
// The plugin MUST read these values from #define statements.
#define BASE_SCALE 10.0 // The overall tiling rate of the large-scale texture space. (Controls scatter detail)
#define VORONOI_SCALE 1.5 // The scale of the sub-texture inside each Voronoi cell.
#define VORONOI_ROTATION 6.283185307 // Max rotation (PI * 2.0 radians = 360 degrees).
#define VORONOI_SCALE_RANDOM 0.5 // Max random scale variation (0.5 means scale ranges from 0.5 to 1.5).

// --- INCLUDE THE VORONOI LOGIC FROM THE PLUGIN ---
#include "res://Addons/voronoi_scatter/voronoi.gdshaderinc"

// --- UNIFORMS (Runtime Controls) ---
uniform sampler2D texture_albedo : source_color; // Essential hint for color space
uniform sampler2D texture_normal; // Texture containing the normal map data.
uniform sampler2D texture_roughness; // Texture containing roughness data.

// Uniform for blending color (RGB) and Alpha control (A).
uniform vec4 blend_color : source_color = vec4(1.0, 1.0, 1.0, 1.0); // Color picker for blend tint/alpha

// Dedicated slider for blend strength (0.0 = no blend, 1.0 = full effect)
uniform float blend_amount : hint_range(0.0, 1.0, 0.01) = 0.0;

// Uniform for Normal Map Strength: 0.0 is flat, 1.0 is full strength.
uniform float normal_strength : hint_range(0.0, 1.0, 0.01) = 1.0;

// The Scaling Parameter (Tiling Multiplier) - Applied in vertex()
uniform float global_tiling_scale : hint_range(0.1, 100, 0.05) = 1.0;

// Blending Mode Selector
// 0 = Linear Blend (Mix)
// 1 = Darken (Multiply)
uniform int blend_mode : hint_range(0, 1) = 0;

// =========================================================================
// --- VERTEX SHADER IMPLEMENTATION ---
// =========================================================================

void vertex() {
    // Safely apply the scaling to the UV coordinates here,
    // as UV is writable in the vertex function.
    UV *= global_tiling_scale;
}

// =========================================================================
// --- FRAGMENT SHADER IMPLEMENTATION ---
// =========================================================================

void fragment() {
	// The UVs arriving here are already scaled by global_tiling_scale

	// 1. Initialize Voronoi scattering (calls the logic from the included file)
	// This uses the constant #DEFINEs for its scatter logic.
	VORONOI_INIT

	// 2. Sample Albedo (Color and Alpha) using the scattered UVs
	vec4 sampled_albedo_vec4 = VORONOI_TEXTURE_SAMPLE(texture_albedo);
	vec3 sampled_albedo_rgb = sampled_albedo_vec4.rgb;
    float sampled_alpha = sampled_albedo_vec4.a;

    // Temporary variables for the final blended color and alpha
    vec3 final_albedo_rgb;
    float final_alpha;

    // 3. Blending Logic: Choose mode based on 'blend_mode' uniform
    if (blend_mode == 0) {
        // Mode 0: Linear Blend (Mix)
        final_albedo_rgb = mix(sampled_albedo_rgb, blend_color.rgb, blend_amount);
    } else { // blend_mode == 1
        // Mode 1: Darken (Multiply)

        // Color Blending
        vec3 multiplied_albedo_rgb = sampled_albedo_rgb * blend_color.rgb;
        final_albedo_rgb = mix(sampled_albedo_rgb, multiplied_albedo_rgb, blend_amount);

        // Alpha Blending
        float multiplied_alpha = sampled_alpha * blend_color.a;
        final_alpha = mix(sampled_alpha, multiplied_alpha, blend_amount);
    }

    // Set the final color and transparency
    ALBEDO = final_albedo_rgb;

	// 4. Sample Normal Map and apply rotation
	NORMAL_MAP = VORONOI_NORMAL_TEXTURE_SAMPLE(texture_normal);

    // Apply the strength control to the normal map effect.
    NORMAL_MAP_DEPTH = normal_strength;

    // 5. Sample Roughness
	ROUGHNESS = VORONOI_TEXTURE_SAMPLE(texture_roughness).r;
}