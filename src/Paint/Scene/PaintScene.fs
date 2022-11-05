module Paint.Scene.PaintScene

open Paint.UI.Board
open Paint.UI.CommandPanel
open System
open Womb
open Womb.Graphics

let createUI config =
  match Paint.UI.Board.create with
  | Some(board) ->
    match Paint.UI.CommandPanel.create with
    | Some(commandPanel) ->
      (config, Some(board), Some(commandPanel))
    | None ->
      Logging.fail "Failed to create command panel"
      (config, Some(board), None)
  | None ->
    Logging.fail "Failed to create board"
    (config, None, None)
