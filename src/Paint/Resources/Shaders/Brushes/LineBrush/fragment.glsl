#version 330 core

out vec4 FragColor;

uniform vec2 start;
uniform vec2 end;

#define RADIUS 20.0f

vec4 drawCircle(vec2 center, float radius, vec4 color);
vec4 drawLine(vec2 start, vec2 end, float thickness, vec4 color);
bool isZero(vec4 x);

void main()
{
	vec4 color = drawCircle(
		start,
		RADIUS,
		vec4(0.0f, 1.0f, 0.3f, 1.0f)
	);
	if (!isZero(color)) {
		FragColor = color;
		return;
	}
	color = drawCircle(
		end,
		RADIUS,
		vec4(0.0f, 1.0f, 0.3f, 1.0f)
	);
	if (!isZero(color)) {
		FragColor = color;
		return;
	}

	color = drawLine(
		start,
		end,
		RADIUS,
		vec4(0.0f, 1.0f, 0.0f, 1.0f)
	);
	if (!isZero(color)) {
		FragColor = color;
		return;
	}

	discard;
} 
