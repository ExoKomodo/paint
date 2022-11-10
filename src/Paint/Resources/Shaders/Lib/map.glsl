#version 330 core

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

vec4 mapPosition3D(vec3 positionToMap, float zoom) {
    const float originalMinCoord = 0.0;
    const float originalMaxCoord = 1.0;
    const float newMinCoord = -1.0;
    const float newMaxCoord = 1.0;

    return vec4(
        zoom * vec3(
            map(
                positionToMap.x,
                originalMinCoord,
                originalMaxCoord,
                newMinCoord,
                newMaxCoord
            ),
            map(
                positionToMap.y,
                originalMinCoord,
                originalMaxCoord,
                newMinCoord,
                newMaxCoord
            ),
            map(
                positionToMap.z,
                originalMinCoord,
                originalMaxCoord,
                newMinCoord,
                newMaxCoord
            )
        ),
        1.0
    );
}

vec4 mapPosition3DTransformed(vec3 positionToMap, mat4 transform) {
    const float originalMinCoord = 0.0;
    const float originalMaxCoord = 1.0;
    const float newMinCoord = -1.0;
    const float newMaxCoord = 1.0;

    vec4 mappedPosition = vec4(
        vec3(
            map(
                positionToMap.x,
                originalMinCoord,
                originalMaxCoord,
                newMinCoord,
                newMaxCoord
            ),
            map(
                positionToMap.y,
                originalMinCoord,
                originalMaxCoord,
                newMinCoord,
                newMaxCoord
            ),
            map(
                positionToMap.z,
                originalMinCoord,
                originalMaxCoord,
                newMinCoord,
                newMaxCoord
            )
        ),
        1.0
    );
    return transform * mappedPosition;
}