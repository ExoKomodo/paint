module Paint.Scene.PaintScene

open Paint.UI.Canvas
open Paint.UI.CommandPanel
open System
open Womb
open Womb.Graphics

let createUI config =
  match Paint.UI.Canvas.create with
  | Some(canvas) ->
    match Paint.UI.CommandPanel.create with
    | Some(commandPanel) ->
      match Paint.Brushes.LineBrush.create {
        Start=0f, 0.5f;
        End=1f, 1f;
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
