module Paint.Scene.DrawScene

open Paint.Brushes.Types
open Paint.Types
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
  let viewport = new Vector2(
    config.DisplayConfig.Width |> single,
    config.DisplayConfig.Height |> single
  )
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
        let (a, start, _end) =
          match lineBrush with
          | { End = Some _end; } ->
              let (startX, startY) = lineBrush.Start
              let (endX, endY) = _end
              let (r, g, b, a) = lineBrush.Color
              (a, new Vector2(startX, startY) * viewport, new Vector2(endX, endY) * viewport)
          | { End = None; } ->
              let (startX, startY) = lineBrush.Start
              let (mouseX, mouseY) = config.Mouse.Position
              (0.1f, new Vector2(startX, startY) * viewport, new Vector2(mouseX, mouseY))
        let (r, g, b, _) = lineBrush.Color
        Primitives.ShadedObject.Draw
          config
          viewMatrix
          projectionMatrix
          lineBrush.Primitive
          scale
          rotation
          (new Vector3(0.5f, 0.5f, 0.0f))
          [
            Vector2Uniform("in_start", (start.X, start.Y));
            Vector2Uniform("in_end", (_end.X, _end.Y));
            Vector4Uniform(
              "in_color",
              (r, g, b, a)
            );
          ]
    )
    state.DrawScene.LineBrushes |> ignore
  
  // Draw circles on canvas
  List.map
    (
      fun circleBrush ->
        let (a, radiusPoint, center) =
          match circleBrush with
          | { RadiusPoint = Some radiusPoint; } ->
              let (_, _, _, a) = circleBrush.Color
              let (radiusX, radiusY) = radiusPoint
              let (centerX, centerY) = circleBrush.Center
              (a, new Vector2(radiusX, radiusY) * viewport, new Vector2(centerX, centerY) * viewport)
          | { RadiusPoint = None; } ->
              let (centerX, centerY) = circleBrush.Center
              let (mouseX, mouseY) = config.Mouse.Position
              (0.1f, new Vector2(mouseX, mouseY), new Vector2(centerX, centerY) * viewport)
        let (r, g, b, _) = circleBrush.Color
        Primitives.ShadedObject.Draw
          config
          viewMatrix
          projectionMatrix
          circleBrush.Primitive
          scale
          rotation
          (new Vector3(0.5f, 0.5f, 0.0f))
          [
            Vector2Uniform("in_center", (center.X, center.Y));
            Vector1Uniform("in_radius", Vector2.Distance(radiusPoint, center));
            Vector4Uniform(
              "in_color",
              (r, g, b, a)
            );
          ]
    )
    state.DrawScene.CircleBrushes |> ignore

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
