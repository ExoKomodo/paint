module Paint.Debug.Mouse

open Womb
open Womb.Graphics
open System

let create =
  let canvas = 
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPaths = ["Resources/Shaders/Debug/Mouse/fragment.glsl"]
          VertexShaderPaths = []
      }
      [||]
      [||]

  match (
    Display.compileShader
      canvas.VertexShaderPaths
      canvas.FragmentShaderPaths
   ) with
  | Some(shader) -> 
      Some(
        { canvas with
            Shader = shader }
      )
  | None ->
      Logging.fail "Failed to compile debug mouse shader"
      None
