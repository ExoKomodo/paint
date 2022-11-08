#version 330 core
layout (location = 0) in vec3 position;

vec4 mapPosition3D(vec3 positionToMap, float zoom);

const float zoom = 0.9;

void main()
{
    gl_Position = mapPosition3D(position, zoom);
}
