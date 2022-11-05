#version 330 core
layout (location = 0) in vec3 position;   // the position variable has attribute position 0

const float commonScale = 0.9;
const float xScale = commonScale;
const float yScale = commonScale;

void main()
{
    gl_Position = vec4(
        (
            position.x -
            (
                1.0 - (
                    (
                        1.0 -
                        commonScale
                    ) /
                    2.0
                )
            )
        ) * xScale,
        position.y * yScale,
        position.z,
        1.0
    );
}
