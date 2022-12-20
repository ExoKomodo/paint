#version 330 core

#define VEC2_ZERO vec2(0, 0)
#define VEC3_ZERO vec3(0, 0, 0)
#define VEC4_ZERO vec4(0, 0, 0, 0)

bool isZero(vec2 x) {
  return x == VEC2_ZERO;
}

bool isZero(vec3 x) {
  return x == VEC3_ZERO;
}

bool isZero(vec4 x) {
  return x == VEC4_ZERO;
}

float map(float a1, float a2, float b1, float b2, float s) {
  return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
}

vec4 drawCircle(vec2 center, float radius, vec4 color) {
  if (distance(gl_FragCoord.xy, center) <= radius) {
		return color;
	}
  return VEC4_ZERO;
}

vec4 drawLine(vec2 start, vec2 end, float thickness, vec4 color) {
  vec2 a_to_b = end.xy - start.xy;
	vec2 a_to_c = gl_FragCoord.xy - start.xy;

	float _dot = dot(a_to_b, a_to_c) / length(a_to_b); // = length(p13) * cos(angle)
	vec2 closest_point = start.xy + normalize(a_to_b) * _dot;
  float a_to_b_length = length(a_to_b);
	if (
    distance(closest_point, gl_FragCoord.xy) < thickness &&
    distance(closest_point, start.xy) <= a_to_b_length &&
    distance(closest_point, end.xy) <= a_to_b_length
  ) {
		return color;
	}
  return VEC4_ZERO;
}
