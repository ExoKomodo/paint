module Paint.Scene.DrawScene

open Paint.Brushes
open Paint.Lib
open Paint.State
open Paint.UI
open System.Numerics
open Womb
open Womb.Graphics
open Womb.Logging

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
        fail "Failed to create line brush"
        (config, Some(canvas), Some(commandPanel), None)
    | None ->
      fail "Failed to create command panel"
      (config, Some(canvas), None, None)
  | None ->
    fail "Failed to create canvas"
    (config, None, None, None)

let draw (config:Config<GameState>) viewMatrix projectionMatrix =
  let state = config.State
  let scale = Vector3.One * 1.0f
  let rotation = Vector3.UnitZ * 0.0f
  
  // Draw canvas
  Primitives.drawShadedObjectWithMvp
    viewMatrix
    projectionMatrix
    state.Canvas
    scale
    rotation
    (new Vector3(0.5f, 0.5f, 0.0f))
  
  // Draw objects on canvas
  List.map
    (
      fun lineBrush ->
        Primitives.drawShadedLineWithMvp
          viewMatrix
          projectionMatrix
          lineBrush
          scale
          rotation
          (new Vector3(0.1f, 0.2f, 0.0f))
    )
    state.LineBrushes |> ignore

  // Draw UI elements
  Primitives.drawShadedObjectWithMvp
    viewMatrix
    projectionMatrix
    state.CommandPanel
    scale
    rotation
    (new Vector3(0.075f, 0.5f, 0.0f))
