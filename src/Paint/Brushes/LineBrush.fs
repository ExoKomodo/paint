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
  let (width, height) = 1.6f, 1.2f
  let transform = Transform.Default()
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths transform width height with
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
