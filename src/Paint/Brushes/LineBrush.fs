module Paint.Brushes.LineBrush

open Paint.Brushes.Types
open Womb.Graphics
open Womb.Lib.Types

let private _create start : option<LineBrush> =
  let fragmentPaths = [
    "Assets/Shaders/Lib/helpers.glsl";
    "Assets/Shaders/Brushes/LineBrush/fragment.glsl";
  ]
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
  | Some primitive ->
      { Primitive = primitive
        Color = (0.0f, 1.0f, 0.3f, 0.4f)
        Start = start
        End = None } |> Some
  | None -> None

let createWithStart start : option<LineBrush> = _create start

let create start _end : option<LineBrush> =
  match _create start with
  | Some lineBrush ->
      { lineBrush with
          End = _end } |> Some
  | None -> None
