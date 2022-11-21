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
	// Draw point for cross product
	vec3 _cross = cross(START, vec3(mouse.xy, 1));
	if (distance(gl_FragCoord.xy, _cross.xy) <= RADIUS) {
		FragColor = vec4(
			1.0f,
			0.0f,
			0.3f,
			1.0f
		);
		return;
	}
	// Draw line between them?
	discard;
} 
