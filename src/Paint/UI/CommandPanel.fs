module Paint.UI.CommandPanel

open Paint.UI.Types
open Womb.Graphics
open Womb.Lib.Types

let create (): option<CommandPanel> =
  let fragmentPaths = ["Assets/Shaders/UI/CommandPanel/fragment.glsl"]
  let vertexPaths = ["Assets/Shaders/Common/vertex.glsl"]
  let (width, height) = 0.1f, 1.2f

  let transform =
    { Transform.Default() with
        Translation = -1f + (1.5f * width), 0f, 0f}
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths transform width height with
  | Some primitive -> Some { Primitive = primitive }
  | None -> None
