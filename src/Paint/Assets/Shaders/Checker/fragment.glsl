#version 330 core

out vec4 out_frag_color;

uniform vec2 in_viewport;

void main()
{
  float scanline_height = in_viewport / 12f;
  if (int(gl_FragCoord.y / scanline_height) % 2 == 0)
  {
    out_frag_color = vec4(
      1.0f,
      0.0f,
      0.0f,
      1.0f
    );
  }
  else
  {
    out_frag_color = vec4(
      0.0f,
      1.0f,
      0.0f,
      1.0f
    );
  }
} 