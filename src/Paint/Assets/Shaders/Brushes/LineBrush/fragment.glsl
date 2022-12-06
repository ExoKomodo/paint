#version 330 core

out vec4 out_frag_color;

uniform vec2 in_start;
uniform vec2 in_end;
uniform vec4 in_color;

#define RADIUS 20.0f

vec4 drawCircle(vec2 center, float radius, vec4 color);
vec4 drawLine(vec2 in_start, vec2 in_end, float thickness, vec4 color);
bool isZero(vec4 x);

void main()
{
	out_frag_color = vec4(0, 0, 0, 0);
	
	vec4 color = drawCircle(
		in_start,
		RADIUS,
		in_color
	);
	if (!isZero(color)) {
		out_frag_color = color;
		return;
	}
	color = drawCircle(
		in_end,
		RADIUS,
		in_color
	);
	if (!isZero(color)) {
		out_frag_color = color;
		return;
	}

	color = drawLine(
		in_start,
		in_end,
		RADIUS,
		in_color
	);
	if (!isZero(color)) {
		out_frag_color = color;
		return;
	}
} 
