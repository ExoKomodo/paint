#version 330 core

#define RADIUS 20.0f
#define START vec3(400, 300, 1)

out vec4 FragColor;

uniform vec2 mouse;

void main()
{
  // Draw two points
	if (distance(gl_FragCoord.xy, START.xy) <= RADIUS) {
		FragColor = vec4(
			0.0f,
			1.0f,
			0.3f,
			1.0f
		);
		return;
	}
	// Attach mouse to one point
	if (distance(gl_FragCoord.xy, mouse.xy) <= RADIUS) {
		FragColor = vec4(
			0.0f,
			1.0f,
			0.3f,
			1.0f
		);
		return;
	}
	// Draw line between them
	// TODO: Understand this
	vec2 ab = mouse.xy - START.xy;
	vec2 ac = gl_FragCoord.xy - START.xy;

	float _dot = dot(ab, ac) / length(ab); // = length(p13) * cos(angle)
	vec2 p4 = START.xy + normalize(ab) * _dot;
	if (length(p4 - gl_FragCoord.xy) < 10/* * sin01(iTime * 4.0 + length(p4 - p1)* 0.02)*/
				&& length(p4 - START.xy) <= length(ab)
				&& length(p4 - mouse.xy) <= length(ab)) {
		FragColor = vec4(0.0, 1.0, 0.0, 1.0);
		return;
	}
	discard;
} 
