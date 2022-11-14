module Paint.UI.CommandPanel

open Womb
open Womb.Backends.OpenGL.Api.Constants
open Womb.Graphics
open System

let create =
  let commandPanel =
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPaths = ["Resources/Shaders/UI/CommandPanel/fragment.glsl"]
          VertexShaderPaths = ["Resources/Shaders/Common/vertex.glsl"]
      }
      [|
        // bottom left
        0.0f; 0.0f; 0.0f;
        // shared top left
        0.0f; 0.3f; 0.0f;
        // shared bottom right
        0.025f; 0.0f; 0.0f;
        // top right
        0.025f; 0.3f; 0.0f;
      |]
      [|
        0u; 1u; 2u; // first triangle vertex order as array indices
        1u; 2u; 3u; // second triangle vertex order as array indices
      |]

  match (
    Display.compileShader
      commandPanel.VertexShaderPaths
      commandPanel.FragmentShaderPaths
  ) with
  | Some(shader) -> 
      Some(
        { commandPanel with
            Shader = shader
            VertexData = Primitives.VertexObjectData.From commandPanel.Vertices commandPanel.Indices }
      )
  | None ->
      Logging.fail "Failed to compile command panel shader"
      None
