module Paint.Scene.PaintScene

open Paint.Board
open Paint.UI.Board
open Paint.UI.CommandPanel
open System
open Womb
open Womb.Graphics

let create config =
  match Paint.Board.create with
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
