module Paint.Debug.Mouse

open Paint.Debug.Types
open System.Numerics
open Womb.Graphics

let create (): option<Mouse> =
  let fragmentPaths = [
    "Resources/Shaders/Lib/helpers.glsl";
    "Resources/Shaders/Debug/Mouse/fragment.glsl";
  ]
  let vertexPaths = ["Resources/Shaders/Common/vertex.glsl"]
  let vertices = [|
    // bottom left
    0.0f; 0.0f; 0.0f;
    // shared top left
    0.0f; 1.0f; 0.0f;
    // shared bottom right
    1.0f; 0.0f; 0.0f;
    // top right
    1.0f; 1.0f; 0.0f;
  |]
  let indices = [|
    0u; 1u; 2u; // first triangle vertex order as array indices
    1u; 2u; 3u; // second triangle vertex order as array indices
  |]
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths vertices indices with
  | Some primitive ->
    { Primitive = primitive
      Position = Vector2.Zero } |> Some
  | None -> None
