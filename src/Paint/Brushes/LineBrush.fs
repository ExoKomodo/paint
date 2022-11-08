module Paint.Brushes.LineBrush

open Womb
open Womb.Graphics

type Point = single * single * single

type LineBrushData = {
  Start: Point;
  End: Point;
}

let pointNew2D x y = Point(x, y, 1.0f)

let create (data:LineBrushData) =
  let startX, startY, startZ = data.Start
  let endX, endY, endZ = data.End
  let lineBrush = (
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPath = "Resources/Shaders/Brushes/LineBrush/fragment.glsl"
          VertexShaderPath = "Resources/Shaders/Brushes/LineBrush/vertex.glsl" }
      [|
        startX; startY; startZ;
        endX; endY; endZ;
      |]
      [|
        0u; 1u;
      |]
  )

  match Display.compileShader lineBrush.VertexShaderPath lineBrush.FragmentShaderPath with
  | Some(shader) -> 
      Some(
        { lineBrush with
            Shader = shader
            VertexData = Primitives.VertexObjectData.From lineBrush.Vertices lineBrush.Indices }
      )
  | None ->
      Logging.fail "Failed to compile line brush shader"
      None
