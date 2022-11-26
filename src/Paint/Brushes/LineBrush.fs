module Paint.Brushes.LineBrush

open Womb
open Womb.Backends.OpenGL.Api.Constants
open Womb.Graphics

type Point = single * single * single

type Data = {
  Start: Point;
  End: option<Point>;
}

let pointNew2D x y = Point(x, y, 0.0f)

let create () =
  let lineBrush = (
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPaths = [
            "Resources/Shaders/Lib/helpers.glsl";
            "Resources/Shaders/Brushes/LineBrush/fragment.glsl";
          ]
          VertexShaderPaths = ["Resources/Shaders/Common/vertex.glsl"]
      }
      [|
        // bottom left
        -0.5f; -0.5f; 0.0f;
        // shared top left
        -0.5f; 0.5f; 0.0f;
        // shared bottom right
        0.5f; -0.5f; 0.0f;
        // top right
        0.5f; 0.5f; 0.0f;
      |]
      [|
        0u; 1u; 2u; // first triangle vertex order as array indices
        1u; 2u; 3u; // second triangle vertex order as array indices
      |]
  )

  match (
    Display.compileShader lineBrush.VertexShaderPaths lineBrush.FragmentShaderPaths
   ) with
  | Some(shader) -> 
      Some(
        { lineBrush with
            Shader = shader
            Context = Primitives.ShadedObjectContext.From lineBrush.Context.Vertices lineBrush.Context.Indices }
      )
  | None ->
      Logging.fail "Failed to compile line brush shader"
      None
