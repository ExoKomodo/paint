module Paint.UI.Board

open Womb
open Womb.Graphics
open System

let create =
  let board = 
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPath = "Resources/Shaders/UI/Board/fragment.glsl"
          VertexShaderPath = "Resources/Shaders/UI/Board/vertex.glsl" }
      [|
        // bottom left
        -1f; -1f; 0.0f;
        // shared top left
        -1f; 1f; 0.0f;
        // shared bottom right
        1f; -1f; 0.0f;
        // top right
        1f; 1f; 0.0f;
      |]
      [|
        0u; 1u; 2u; // first triangle vertex order as array indices
        1u; 2u; 3u; // second triangle vertex order as array indices
      |]

  match Display.compileShader board.VertexShaderPath board.FragmentShaderPath with
  | Some(shader) -> 
      Some(
        { board with
            Shader = shader
            VertexData = Primitives.VertexObjectData.From board.Vertices board.Indices }
      )
  | None ->
      Logging.fail "Failed to compile board shader"
      None
