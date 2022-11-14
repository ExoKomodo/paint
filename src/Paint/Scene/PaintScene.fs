module Paint.Scene.PaintScene

open Paint.Brushes
open Paint.Graphics
open Paint.UI
open System.Numerics
open Womb
open Womb.Graphics

let createUI config =
  match Canvas.create with
  | Some(canvas) ->
    match CommandPanel.create with
    | Some(commandPanel) ->
      match LineBrush.create {
        Start=(LineBrush.pointNew2D 0.0f 0.0f);
        End=(LineBrush.pointNew2D 0.4f 0.3f);
      } with
      | Some(lineBrush) ->
        (config, Some(canvas), Some(commandPanel), Some(lineBrush))
      | None ->
        Logging.fail "Failed to create line brush"
        (config, Some(canvas), Some(commandPanel), None)
    | None ->
      Logging.fail "Failed to create command panel"
      (config, Some(canvas), None, None)
  | None ->
    Logging.fail "Failed to create canvas"
    (config, None, None, None)

let draw config viewMatrix projectionMatrix canvas commandPanel lineBrushes =
  let scale = Vector3.One * 1.0f
  let rotation = Vector3.UnitZ * 0.0f
  
  // Draw canvas
  Paint.Graphics.drawTransformedShadedObject
    viewMatrix
    projectionMatrix
    canvas
    scale
    rotation
    (new Vector3(0.5f, 0.5f, 0.0f))
  
  // Draw objects on canvas
  List.map
    (
      fun lineBrush ->
        Paint.Graphics.drawTransformedShadedLine
          viewMatrix
          projectionMatrix
          lineBrush
          scale
          rotation
          (new Vector3(0.1f, 0.2f, 0.0f))
    )
    lineBrushes |> ignore

  // Draw UI elements
  Paint.Graphics.drawTransformedShadedObject
    viewMatrix
    projectionMatrix
    commandPanel
    scale
    rotation
    (new Vector3(0.075f, 0.5f, 0.0f))
