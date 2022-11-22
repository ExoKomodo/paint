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

vec4 drawCircle(vec2 center, float radius, vec4 color) {
  if (distance(gl_FragCoord.xy, center) <= radius) {
		return color;
	}
  return VEC4_ZERO;
}

vec4 drawLine(vec2 start, vec2 end, float thickness, vec4 color) {
  vec2 ab = end.xy - start.xy;
	vec2 ac = gl_FragCoord.xy - start.xy;

	float _dot = dot(ab, ac) / length(ab); // = length(p13) * cos(angle)
	vec2 p4 = start.xy + normalize(ab) * _dot;
	if (
    length(p4 - gl_FragCoord.xy) < thickness &&
    length(p4 - start.xy) <= length(ab) &&
    length(p4 - end.xy) <= length(ab)
  ) {
		return vec4(0.0, 1.0, 0.0, 1.0);
	}
  return VEC4_ZERO;
}
