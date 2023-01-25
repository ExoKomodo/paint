#version 330 core

#define RADIUS 10.0f

// TODO: Pass in position
// TODO: Pass in size of button canvas
uniform vec4 in_color;
uniform vec4 in_icon_color;
uniform vec2 in_viewport;

out vec4 out_frag_color;

vec4 drawCircle(vec2 center, float radius, vec4 color);
bool isZero(vec4 x);
float map(float a1, float a2, float b1, float b2, float s);

void main()
{
  out_frag_color = in_color;
	// TODO: Doesn't size exactly right
	float radius = in_viewport.x / 80f;

  vec4 color = drawCircle(
		(
      vec2(
        map(-1, 1, 0, 1, -0.85),
        map(-1, 1, 0, 1, 0.55)
      ) * in_viewport
    ),
		radius,
		in_icon_color
	);
	if (!isZero(color)) {
		out_frag_color = color;
		return;
	}
} 
