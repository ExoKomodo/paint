module Paint.UI.Button

open Paint.UI.Types
open Womb.Graphics

let create (): option<Button> =
  let fragmentPaths = ["Assets/Shaders/UI/Button/fragment.glsl"]
  let vertexPaths = ["Assets/Shaders/Common/vertex.glsl"]
  let (width, height) = 0.05f, 0.05f
  let vertices = [|
    // bottom left
    -width; -height; 0.0f;
    // shared top left
    -width; height; 0.0f;
    // shared bottom right
    width; -height; 0.0f;
    // top right
    width; height; 0.0f;
  |]
  let indices = [|
    0u; 1u; 2u; // first triangle vertex order as array indices
    1u; 2u; 3u; // second triangle vertex order as array indices
  |]
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths vertices indices with
  | Some primitive -> Some { Primitive = primitive }
  | None -> None
