module Paint.Brushes.LineBrush

open Womb.Graphics

type Point = single * single * single

type Data = {
  Start: Point;
  End: option<Point>;
}

let pointNew2D x y = Point(x, y, 0.0f)

let create () =
  let fragmentPaths = [
    "Resources/Shaders/Lib/helpers.glsl";
    "Resources/Shaders/Brushes/LineBrush/fragment.glsl";
  ]
  let vertexPaths = ["Resources/Shaders/Common/vertex.glsl"]
  let vertices = [|
    // bottom left
    -0.5f; -0.5f; 0.0f;
    // shared top left
    -0.5f; 0.5f; 0.0f;
    // shared bottom right
    0.5f; -0.5f; 0.0f;
    // top right
    0.5f; 0.5f; 0.0f;
  |]
  let indices = [|
    0u; 1u; 2u; // first triangle vertex order as array indices
    1u; 2u; 3u; // second triangle vertex order as array indices
  |]
  Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths vertices indices
