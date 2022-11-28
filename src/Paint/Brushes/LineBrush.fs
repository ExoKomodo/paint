module Paint.Brushes.LineBrush

open System.Numerics
open Paint.Brushes.Types
open Womb.Graphics

let private _create start : option<LineBrush> =
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
      { Primitive = Some primitive
        Color = new Vector4(0.0f, 1.0f, 0.3f, 0.3f)
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
