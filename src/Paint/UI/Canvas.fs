module Paint.UI.Canvas

open Paint.Lib.Graphics

let create () =
  let fragmentPaths = ["Resources/Shaders/UI/Canvas/fragment.glsl"]
  let vertexPaths = ["Resources/Shaders/Common/vertex.glsl"]
  let vertices = [|
    // bottom left
    -0.4f; -0.3f; 0.0f;
    // shared top left
    -0.4f; 0.3f; 0.0f;
    // shared bottom right
    0.4f; -0.3f; 0.0f;
    // top right
    0.4f; 0.3f; 0.0f;
  |]
  let indices = [|
    0u; 1u; 2u; // first triangle vertex order as array indices
    1u; 2u; 3u; // second triangle vertex order as array indices
  |]
  createQuad vertexPaths fragmentPaths vertices indices
