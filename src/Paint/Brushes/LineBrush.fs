module Paint.Brushes.LineBrush

open Womb
open Womb.Backends.OpenGL.Api.Constants
open Womb.Graphics

type Point = single * single * single

type LineBrushData = {
  Start: Point;
  End: Point;
}

let pointNew2D x y = Point(x, y, 0.0f)

let create (data:LineBrushData) =
  let startX, startY, startZ = data.Start
  let endX, endY, endZ = data.End
  let lineBrush = (
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPaths = ["Resources/Shaders/Brushes/LineBrush/fragment.glsl"]
          VertexShaderPaths = ["Resources/Shaders/Common/vertex.glsl"]
      }
      [|
        startX; startY; startZ;
        endX; endY; endZ;
      |]
      [|
        0u; 1u;
      |]
  )

  match (
    Display.compileShader lineBrush.VertexShaderPaths lineBrush.FragmentShaderPaths
   ) with
  | Some(shader) -> 
      Some(
        { lineBrush with
            Shader = shader
            VertexData = Primitives.VertexObjectData.From lineBrush.Vertices lineBrush.Indices }
      )
  | None ->
      Logging.fail "Failed to compile line brush shader"
      None
