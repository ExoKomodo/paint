#version 330 core

#define RADIUS 10.0f

out vec4 FragColor;

uniform vec2 mouse;

void main()
{
  if (distance(gl_FragCoord.xy, mouse) > RADIUS) {
    discard;
  }
  FragColor = vec4(
    0.0f,
    0.3f,
    1.0f,
    1.0f
  );
} 
