module Paint.Debug.Mouse

open Womb
open Womb.Graphics
open System

let create =
  let mouse = 
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPaths = [
            "Resources/Shaders/Lib/helpers.glsl";
            "Resources/Shaders/Debug/Mouse/fragment.glsl";
          ]
          VertexShaderPaths = ["Resources/Shaders/Common/vertex.glsl"]
      }
      [|
        // bottom left
        0.0f; 0.0f; 0.0f;
        // shared top left
        0.0f; 1.0f; 0.0f;
        // shared bottom right
        1.0f; 0.0f; 0.0f;
        // top right
        1.0f; 1.0f; 0.0f;
      |]
      [|
        0u; 1u; 2u; // first triangle vertex order as array indices
        1u; 2u; 3u; // second triangle vertex order as array indices
      |]

  match (
    Display.compileShader
      mouse.VertexShaderPaths
      mouse.FragmentShaderPaths
   ) with
  | Some(shader) -> 
      Some(
        { mouse with
            Shader = shader
            Context = Primitives.ShadedObjectContext.From mouse.Context.Vertices mouse.Context.Indices }
      )
  | None ->
      Logging.fail "Failed to compile debug mouse shader"
      None
