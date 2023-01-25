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
  let transform = Transform.Default()
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths transform width height with
  | Some primitive -> { Primitive = primitive } |> Some
  | None -> None

