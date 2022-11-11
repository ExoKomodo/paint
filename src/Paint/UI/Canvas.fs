module Paint.UI.Canvas

open Womb
open Womb.Graphics
open System

let create =
  let canvas = 
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPaths = ["Resources/Shaders/UI/Canvas/fragment.glsl"]
          VertexShaderPaths = [
            "Resources/Shaders/Lib/map.glsl";
            "Resources/Shaders/UI/Canvas/vertex.glsl";
          ]
      }
      [|
        // bottom left
        0f; 0.0f; 0.0f;
        // shared top left
        0f; 0.8f; 0.0f;
        // shared bottom right
        0.5f; 0.0f; 0.0f;
        // top right
        0.5f; 0.8f; 0.0f;
      |]
      [|
        0u; 1u; 2u; // first triangle vertex order as array indices
        1u; 2u; 3u; // second triangle vertex order as array indices
      |]

  match (
    Display.compileShader
      canvas.VertexShaderPaths
      canvas.FragmentShaderPaths
   ) with
  | Some(shader) -> 
      Some(
        { canvas with
            Shader = shader
            VertexData = Primitives.VertexObjectData.From canvas.Vertices canvas.Indices }
      )
  | None ->
      Logging.fail "Failed to compile canvas shader"
      None
