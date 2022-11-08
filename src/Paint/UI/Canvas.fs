module Paint.UI.Canvas

open Womb
open Womb.Graphics
open System

let create =
  let canvas = 
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPath = "Resources/Shaders/UI/Canvas/fragment.glsl"
          VertexShaderPath = "Resources/Shaders/UI/vertex.glsl" }
      [|
        // bottom left
        0.1f; 0f; 0.0f;
        // shared top left
        0.1f; 1f; 0.0f;
        // shared bottom right
        1f; 0f; 0.0f;
        // top right
        1f; 1f; 0.0f;
      |]
      [|
        0u; 1u; 2u; // first triangle vertex order as array indices
        1u; 2u; 3u; // second triangle vertex order as array indices
      |]

  match Display.compileShader canvas.VertexShaderPath canvas.FragmentShaderPath with
  | Some(shader) -> 
      Some(
        { canvas with
            Shader = shader
            VertexData = Primitives.VertexObjectData.From canvas.Vertices canvas.Indices }
      )
  | None ->
      Logging.fail "Failed to compile canvas shader"
      None
