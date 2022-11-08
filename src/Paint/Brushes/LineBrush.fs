module Paint.Brushes.LineBrush

open Womb
open Womb.Graphics
open System

type Point = single * single

type LineBrushData = {
  Start: Point;
  End: Point;
}

let create (data:LineBrushData) =
  let startX, startY = data.Start
  let endX, endY = data.End
  let lineBrush = (
    Primitives.ShadedObject.From
      { Primitives.ShadedObject.Default with
          FragmentShaderPath = "Resources/Shaders/Brushes/LineBrush/fragment.glsl"
          VertexShaderPath = "Resources/Shaders/Brushes/LineBrush/vertex.glsl" }
      [|
        startX; startY; 1f;
        endX; endY; 1f;
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
