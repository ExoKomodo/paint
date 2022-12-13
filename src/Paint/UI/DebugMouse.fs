module Paint.UI.DebugMouse

open Womb.Lib.Types
open Paint.UI.Types
open Womb.Graphics

let create (): option<DebugMouse> =
  let fragmentPaths = [
    "Assets/Shaders/Lib/helpers.glsl";
    "Assets/Shaders/Debug/Mouse/fragment.glsl";
  ]
  let vertexPaths = ["Assets/Shaders/Common/vertex.glsl"]
  let (width, height) = 2.0f, 2.0f
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
  let transform = Transform.Default()
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths vertices indices transform with
  | Some primitive -> { Primitive = primitive } |> Some
  | None -> None

