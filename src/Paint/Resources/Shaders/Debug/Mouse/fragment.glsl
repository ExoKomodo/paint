#version 330 core

out vec4 FragColor;

uniform vec2 mouse;

#define RADIUS 10.0f

vec4 drawCircle(vec2 center, float radius, vec4 color);
vec4 drawLine(vec2 start, vec2 end, float thickness, vec4 color);
bool isZero(vec2 x);
bool isZero(vec3 x);
bool isZero(vec4 x);

void main()
{
  FragColor = vec4(0, 0, 0, 0);
  vec4 color = drawCircle(
    mouse.xy,
    RADIUS,
    vec4(0.0f, 0.3f, 1.0f, 1.0f)
  );
  if (!isZero(color)) {
    FragColor = color;
    return;
  }
} 
