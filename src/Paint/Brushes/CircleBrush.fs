module Paint.Brushes.CircleBrush

open System.Numerics
open Paint.Brushes.Types
open Womb.Graphics

let private _create center : option<CircleBrush> =
  let fragmentPaths = [
    "Resources/Shaders/Lib/helpers.glsl";
    "Resources/Shaders/Brushes/CircleBrush/fragment.glsl";
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
        Color = new Vector4(1.0f, 0.0f, 0.3f, 0.4f)
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
