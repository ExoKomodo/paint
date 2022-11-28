#version 330 core

out vec4 FragColor;

uniform vec2 in_center;
uniform float in_radius;
uniform vec4 in_color;

vec4 drawCircle(vec2 center, float radius, vec4 color);
bool isZero(vec4 x);

void main()
{
	FragColor = vec4(0, 0, 0, 0);
	
	vec4 color = drawCircle(
		in_center,
		in_radius,
		in_color
	);
	if (!isZero(color)) {
		FragColor = color;
		return;
	}
} 
