#version 330 core

out vec4 FragColor;

uniform vec3 start;
uniform vec3 end;

void main() {
	// https://byjus.com/maths/two-point-form/
	// Draw two points
	// Draw line between them
	// Attach mouse to one point
	// Draw points for cross and dot products

	if (false) {
		discard;
	}
	FragColor = vec4(
		0.0f,
		1.0f,
		0.3f,
		1.0f
	);
}
