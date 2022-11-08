#version 330 core
layout (location = 0) in vec2 position;   // the position variable has attribute position 0

const float zoom = 0.9;

float map(float value, float low1, float high1, float low2, float high2) {
    return (
        low2 +
        (
            value -
            low1
        ) * (
            high2 -
            low2
        ) / (
            high1 -
            low1
        )
    );
}

vec4 mapPositionToOpenGL(vec2 positionToMap) {
    const float originalMinCoord = 0.0;
    const float originalMaxCoord = 1.0;
    const float openGlMinCoord = -1.0;
    const float openGlMaxCoord = 1.0;

    return vec4(
        vec2(
            map(
                positionToMap.x,
                originalMinCoord,
                originalMaxCoord,
                openGlMinCoord,
                openGlMaxCoord
            ),
            map(
                positionToMap.y,
                originalMinCoord,
                originalMaxCoord,
                openGlMinCoord,
                openGlMaxCoord
            )
        ) * zoom,
        1.0,
        1.0
    );
}

void main()
{
    gl_Position = mapPositionToOpenGL(position);
}
