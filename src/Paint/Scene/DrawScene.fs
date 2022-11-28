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
        (config, Some canvas, Some commandPanel)
    | None ->
      fail "Failed to create command panel"
      (config, Some canvas, None)
  | None ->
    fail "Failed to create canvas"
    (config, None, None)

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
        match lineBrush with
        | { Primitive = Some primitive; End = Some _end; } ->
          Primitives.ShadedObject.Draw
            config
            viewMatrix
            projectionMatrix
            primitive
            scale
            rotation
            (new Vector3(0.5f, 0.5f, 0.0f))
            [
              Vector2Uniform("start", lineBrush.Start);
              Vector2Uniform("end", _end);
              Vector4Uniform("line_color", lineBrush.Color);
            ]
        | _ -> ()
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
