#version 330 core

#define RADIUS 1.0f

// TODO: Pass in position
// TODO: Pass in size of button canvas
uniform vec4 in_color;
uniform vec4 in_icon_color;
uniform vec2 in_viewport;

out vec4 out_frag_color;

vec4 drawCircle(vec2 center, float radius, vec4 color);
vec4 drawLine(vec2 in_start, vec2 in_end, float thickness, vec4 color);
bool isZero(vec4 x);
float map(float a1, float a2, float b1, float b2, float s);

void main()
{
  out_frag_color = in_color;
  vec2 start = vec2(
    map(-1.0f, 1.0f, 0.0f, 1.0f, -0.89f),
    map(-1.0f, 1.0f, 0.0f, 1.0f, 0.41f)
   ) * in_viewport;
   vec2 end = vec2(
    map(-1.0f, 1.0f, 0.0f, 1.0f, -0.81f),
    map(-1.0f, 1.0f, 0.0f, 1.0f, 0.49f)
   ) * in_viewport;

  vec4 color = drawCircle(
		start,
		RADIUS,
		in_icon_color
	);
	if (!isZero(color)) {
		out_frag_color = color;
		return;
	}
	color = drawCircle(
		end,
		RADIUS,
		in_icon_color
	);
	if (!isZero(color)) {
		out_frag_color = color;
		return;
	}

	color = drawLine(
		start,
		end,
		RADIUS,
		in_icon_color
	);
	if (!isZero(color)) {
		out_frag_color = color;
		return;
	}
} 
