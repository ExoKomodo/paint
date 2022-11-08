module Paint.Scene.PaintScene

open Paint.Brushes
open Paint.UI
open Womb

let createUI config =
  match Canvas.create with
  | Some(canvas) ->
    match CommandPanel.create with
    | Some(commandPanel) ->
      match LineBrush.create {
        Start=(LineBrush.pointNew2D 0f 0.5f);
        End=(LineBrush.pointNew2D 0.5f 0.75f);
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
