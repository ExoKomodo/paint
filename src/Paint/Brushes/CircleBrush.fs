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
