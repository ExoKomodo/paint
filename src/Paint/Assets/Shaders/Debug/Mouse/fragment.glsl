#version 330 core

out vec4 out_frag_color;

uniform vec2 in_mouse;
uniform vec2 in_viewport;

vec4 drawCircle(vec2 center, float radius, vec4 color);
vec4 drawLine(vec2 start, vec2 end, float thickness, vec4 color);
bool isZero(vec2 x);
bool isZero(vec3 x);
bool isZero(vec4 x);

void main()
{
  float radius = in_viewport.y / 60f;
  out_frag_color = vec4(0, 0, 0, 0);
  vec4 color = drawCircle(
    in_mouse.xy,
    radius,
    vec4(0.0f, 0.3f, 1.0f, 1.0f)
  );
  if (!isZero(color)) {
    out_frag_color = color;
    return;
  }
} 
