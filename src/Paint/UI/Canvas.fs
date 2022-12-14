module Paint.UI.Canvas

open Paint.UI.Types
open Womb.Graphics
open Womb.Lib.Types

let create (): option<Canvas> =
  let fragmentPaths = ["Assets/Shaders/UI/Canvas/fragment.glsl"]
  let vertexPaths = ["Assets/Shaders/Common/vertex.glsl"]
  let (width, height) = 1.6f, 1.2f
  let transform = Transform.Default()
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths transform width height with
  | Some primitive -> Some { Primitive = primitive }
  | None -> None
