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
      (config, Some(canvas), Some(commandPanel))
    | None ->
      Logging.fail "Failed to create command panel"
      (config, Some(canvas), None)
  | None ->
    Logging.fail "Failed to create canvas"
    (config, None, None)
