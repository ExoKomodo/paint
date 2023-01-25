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
        match Button.create "CircleButton" (-0.85f, 0.55f, 0f) with
        | Some circleButton ->
            match Button.create "LineButton" (-0.85f, 0.45f, 0f) with
            | Some lineButton ->
                (config, Some canvas, Some commandPanel, Some circleButton, Some lineButton)
            | None ->
                fail "Failed to create line button"
                (config, Some canvas, Some commandPanel, None, None)
        | None ->
            fail "Failed to create circle button"
            (config, Some canvas, Some commandPanel, None, None)
    | None ->
        fail "Failed to create command panel"
        (config, Some canvas, None, None, None)
  | None ->
      fail "Failed to create canvas"
      (config, None, None, None, None)

let private drawCanvas viewMatrix projectionMatrix (config:Config<GameState>) =
  match config.State.DrawScene.Canvas with
  | Some canvas ->
      Primitives.ShadedObject.Draw
        config
        viewMatrix
        projectionMatrix
        canvas.Primitive
        []
      config
  | None ->
      fail "Canvas is None"
      config

let private drawLines viewMatrix projectionMatrix (viewport:Vector2) (config:Config<GameState>) =
  List.map
    (
      fun (lineBrush:Paint.Brushes.Types.LineBrush) ->
        let curried_map = Womb.Lib.Math.map -1f 1f 0f 1f
        let (a, start, _end) =
          match lineBrush with
          | { End = Some _end; } ->
              let (startX, startY) = lineBrush.Start
              let (endX, endY) = _end
              let (r, g, b, a) = lineBrush.Color
              (a, new Vector2(curried_map startX, curried_map startY) * viewport, new Vector2(curried_map endX, curried_map endY) * viewport)
          | { End = None; } ->
              let (startX, startY) = lineBrush.Start
              let (mouseX, mouseY) = config.Mouse.Position
              (0.1f, new Vector2(curried_map startX, curried_map startY) * viewport, new Vector2(mouseX, mouseY))
        let (r, g, b, _) = lineBrush.Color
        Primitives.ShadedObject.Draw
          config
          viewMatrix
          projectionMatrix
          lineBrush.Primitive
          [
            Vector2Uniform("in_start", (start.X, start.Y));
            Vector2Uniform("in_end", (_end.X, _end.Y));
            Vector4Uniform(
              "in_color",
              (r, g, b, a)
            );
          ]
    )
    config.State.DrawScene.LineBrushes |> ignore
  config

let private drawCircles viewMatrix projectionMatrix (viewport:Vector2) (config:Config<GameState>) =
  List.map
    (
      fun circleBrush ->
        let curried_map = Womb.Lib.Math.map -1f 1f 0f 1f
        let (a, radiusPoint, center) =
          match circleBrush with
          | { RadiusPoint = Some radiusPoint; } ->
              let (_, _, _, a) = circleBrush.Color
              let (radiusX, radiusY) = radiusPoint
              let (centerX, centerY) = circleBrush.Center
              (a, new Vector2(curried_map radiusX, curried_map radiusY) * viewport, new Vector2(curried_map centerX, curried_map centerY) * viewport)
          | { RadiusPoint = None; } ->
              let (centerX, centerY) = circleBrush.Center
              let (mouseX, mouseY) = config.Mouse.Position
              (0.1f, new Vector2(mouseX, mouseY), new Vector2(curried_map centerX, curried_map centerY) * viewport)
        let (r, g, b, _) = circleBrush.Color
        Primitives.ShadedObject.Draw
          config
          viewMatrix
          projectionMatrix
          circleBrush.Primitive
          [
            Vector2Uniform("in_center", (center.X, center.Y));
            Vector1Uniform("in_radius", Vector2.Distance(radiusPoint, center));
            Vector4Uniform(
              "in_color",
              (r, g, b, a)
            );
          ]
    )
    config.State.DrawScene.CircleBrushes |> ignore
  config

let private drawUIElements viewMatrix projectionMatrix (viewport:Vector2) (config:Config<GameState>) =
  match config.State.DrawScene.CommandPanel with
  | Some commandPanel ->
      Primitives.ShadedObject.Draw
        config
        viewMatrix
        projectionMatrix
        commandPanel.Primitive
        []
  | None -> fail "Command Panel is None"
  
  let selectedButton =
    match config.State.DrawScene.ActiveBrush with
    | Circle -> "CircleButton"
    | Line -> "LineButton"

  List.map
    (
      fun (buttonOpt:option<Types.Button>) ->
        match buttonOpt with
        | Some button ->
            let idleColor = Womb.Lib.Types.Vector4(1.0f, 0.0f, 0.0f, 1.0f)
            let hoverColor = Womb.Lib.Types.Vector4(1.0f, 0.0f, 1.0f, 1.0f)
            let selectedColor = Womb.Lib.Types.Vector4(0.0f, 0.0f, 1.0f, 1.0f)
            let color =
              match Primitives.ShadedObject.Contains button.Primitive (config.VirtualMousePosition()), selectedButton = button.Name with
              | None, true -> selectedColor
              | Some _, _ -> hoverColor
              | None, _ -> idleColor

            Primitives.ShadedObject.Draw
              config
              viewMatrix
              projectionMatrix
              button.Primitive
              [
                Vector4Uniform("in_color", color);
                Vector4Uniform("in_icon_color", (0.0f, 1.0f, 0.0f, 1.0f))
              ]
        | None -> fail "Button is None"
    ) [config.State.DrawScene.CircleButton; config.State.DrawScene.LineButton] |> ignore
  config

let draw (config:Config<GameState>) viewMatrix projectionMatrix =
  let viewport = new Vector2(
    config.DisplayConfig.Width |> single,
    config.DisplayConfig.Height |> single
  )  
  drawCanvas viewMatrix projectionMatrix config |>
    drawUIElements viewMatrix projectionMatrix viewport |>
    drawLines viewMatrix projectionMatrix viewport |>
    drawCircles viewMatrix projectionMatrix viewport
