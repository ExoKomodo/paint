#version 330 core

#define RADIUS 20.0f
#define START vec2(100, 140)

out vec4 FragColor;

uniform vec2 mouse;

void main()
{
  // Draw two points
	// Attach mouse to one point
	// Draw line between them?
	// Draw points for cross and dot products?
	if (
		distance(gl_FragCoord.xy, START) > RADIUS &&
		distance(gl_FragCoord.xy, mouse) > RADIUS
	) {
    discard;
  }
  FragColor = vec4(
    0.0f,
    1.0f,
    0.3f,
    1.0f
  );
} 
