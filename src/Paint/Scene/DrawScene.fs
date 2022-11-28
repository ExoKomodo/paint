module Paint.Scene.DrawScene

open Paint.Brushes
open Paint.Brushes.Types
open Paint.State
open Paint.UI
open System.Numerics
open Womb.Graphics
open Womb.Graphics.Types
open Womb.Logging
open Womb.Types

let createUI config =
  match Canvas.create() with
  | Some canvas ->
    match CommandPanel.create() with
    | Some commandPanel ->
      match LineBrush.create() with
      | Some lineBrush ->
        (config, Some canvas, Some commandPanel, Some lineBrush)
      | None ->
        fail "Failed to create line brush"
        (config, Some canvas, Some commandPanel, None)
    | None ->
      fail "Failed to create command panel"
      (config, Some canvas, None, None)
  | None ->
    fail "Failed to create canvas"
    (config, None, None, None)

let draw (config:Config<GameState>) viewMatrix projectionMatrix =
  let state = config.State
  let scale = Vector3.One * 1.0f
  let rotation = Vector3.UnitZ * 0.0f
  
  // Draw canvas
  match state.DrawScene.Canvas with
  | Some canvas ->
    Primitives.ShadedObject.Draw
      config
      viewMatrix
      projectionMatrix
      canvas.Primitive
      scale
      rotation
      (new Vector3(0.5f, 0.5f, 0.0f))
      []
  | None -> fail "Canvas is None"
  
  // Draw lines on canvas
  List.map
    (
      fun lineBrush ->
        // Cause line to follow mouse for one point
        let lineBrush =
          { lineBrush with
              End = config.Mouse.Position }
        Primitives.ShadedObject.Draw
          config
          viewMatrix
          projectionMatrix
          lineBrush.Primitive
          scale
          rotation
          (new Vector3(0.5f, 0.5f, 0.0f))
          [
            Vector2Uniform("start", lineBrush.Start);
            Vector2Uniform("end", lineBrush.End);
          ]
    )
    state.DrawScene.LineBrushes |> ignore

  // Draw UI elements on top
  match state.DrawScene.CommandPanel with
  | Some commandPanel ->
    Primitives.ShadedObject.Draw
      config
      viewMatrix
      projectionMatrix
      commandPanel.Primitive
      scale
      rotation
      (new Vector3(0.075f, 0.5f, 0.0f))
      []
  | None -> fail "Command Panel is None"
