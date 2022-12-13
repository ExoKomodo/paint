module Paint.UI.Canvas

open Paint.UI.Types
open Womb.Graphics
open Womb.Lib.Types

let create (): option<Canvas> =
  let fragmentPaths = ["Assets/Shaders/UI/Canvas/fragment.glsl"]
  let vertexPaths = ["Assets/Shaders/Common/vertex.glsl"]
  let (width, height) = 0.4f, 0.3f
  let vertices = [|
    // bottom left
    -width / 2.0f; -height / 2.0f; 0.0f;
    // shared top left
    -width / 2.0f; height / 2.0f; 0.0f;
    // shared bottom right
    width / 2.0f; -height / 2.0f; 0.0f;
    // top right
    width / 2.0f; height / 2.0f; 0.0f;
  |]
  let indices = [|
    0u; 1u; 2u; // first triangle vertex order as array indices
    1u; 2u; 3u; // second triangle vertex order as array indices
  |]

  let transform =
    { Transform.Default() with
        Scale = 4f, 4f, 4f }
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths vertices indices transform with
  | Some primitive -> Some { Primitive = primitive }
  | None -> None
