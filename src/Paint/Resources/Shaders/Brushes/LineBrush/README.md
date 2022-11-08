[Reference](https://stackoverflow.com/questions/14486291/how-to-draw-line-in-opengl)

Vertex Shader
```glsl
#version 330 core
layout (location = 0) in vec3 position;

uniform mat4 MVP;

void main() {
  gl_Position = MVP * vec4(
		position.x,
		position.y,
		position.z,
		1.0
	);
}
```

Fragment Shader
```glsl
#version 330 core

out vec4 FragColor;
uniform vec3 color;

void main()
{
  FragColor = vec4(
		color,
		1.0f
	);
}
```
