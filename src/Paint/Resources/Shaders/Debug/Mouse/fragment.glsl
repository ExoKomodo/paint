#version 330 core

#define RADIUS 10.0f

out vec4 FragColor;

uniform vec2 mouse;

void main()
{
  if (distance(gl_FragCoord.xy, mouse) < RADIUS) {
    FragColor = vec4(
      1.0f,
      1.0f,
      1.0f,
      1.0f
    );
  } else {
    FragColor = vec4(
      0.0f,
      0.0f,
      0.0f,
      0.0f
    );
  }
} 
