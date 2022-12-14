module Paint.Brushes.CircleBrush

open Paint.Brushes.Types
open Womb.Graphics
open Womb.Lib.Types

let private _create center : option<CircleBrush> =
  let fragmentPaths = [
    "Assets/Shaders/Lib/helpers.glsl";
    "Assets/Shaders/Brushes/CircleBrush/fragment.glsl";
  ]
  let vertexPaths = ["Assets/Shaders/Common/vertex.glsl"]
  let (width, height) = 1.6f, 1.2f
  let transform = Transform.Default()
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths transform width height with
  | Some primitive ->
      { Primitive = primitive
        Color = (1.0f, 0.0f, 0.3f, 0.4f)
        Center = center
        RadiusPoint = None } |> Some
  | None -> None

let createWithCenter center : option<CircleBrush> = _create center

let create center radiusPoint : option<CircleBrush> =
  match _create center with
  | Some circle ->
      { circle with
          Center = center
          RadiusPoint = radiusPoint } |> Some
  | None -> None
