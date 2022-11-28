module Paint.Brushes.LineBrush

open Paint.Brushes.Types
open System.Numerics
open Womb.Graphics

let create () : option<LineBrush> =
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
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths vertices indices with
  | Some primitive ->
      { Primitive = primitive
        Start = new Vector2(400f, 300f)
        End = Vector2.Zero } |> Some
  | None -> None
