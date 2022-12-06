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
        let (alpha, start, _end) =
          match lineBrush with
          | { End = Some _end; } -> (lineBrush.Color.W, lineBrush.Start * viewport, _end * viewport)
          | { End = None; } -> (0.1f, lineBrush.Start * viewport, config.Mouse.Position)
        Primitives.ShadedObject.Draw
          config
          viewMatrix
          projectionMatrix
          lineBrush.Primitive
          scale
          rotation
          (new Vector3(0.5f, 0.5f, 0.0f))
          [
            Vector2Uniform("in_start", start);
            Vector2Uniform("in_end", _end);
            Vector4Uniform(
              "in_color",
              new Vector4(lineBrush.Color.X, lineBrush.Color.Y, lineBrush.Color.Z, alpha)
            );
          ]
    )
    state.DrawScene.LineBrushes |> ignore
  
  // Draw circles on canvas
  List.map
    (
      fun circleBrush ->
        let (alpha, radiusPoint, center) =
          match circleBrush with
          | { RadiusPoint = Some radiusPoint; } -> (circleBrush.Color.W, radiusPoint * viewport, circleBrush.Center * viewport)
          | { RadiusPoint = None; } -> (0.1f, config.Mouse.Position, circleBrush.Center * viewport)
        Primitives.ShadedObject.Draw
          config
          viewMatrix
          projectionMatrix
          circleBrush.Primitive
          scale
          rotation
          (new Vector3(0.5f, 0.5f, 0.0f))
          [
            Vector2Uniform("in_center", center);
            Vector1Uniform("in_radius", Vector2.Distance(radiusPoint, center));
            Vector4Uniform(
              "in_color",
              new Vector4(circleBrush.Color.X, circleBrush.Color.Y, circleBrush.Color.Z, alpha)
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
