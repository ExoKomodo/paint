module Paint.Scene.DebugScene

open Paint.Brushes
open Paint.Debug
open Paint.State
open Paint.UI
open System.Numerics
open Womb
open Womb.Graphics
open Womb.Logging
open Womb.Types

let createUI config =
  match Mouse.create with
  | Some(mouse) ->
      (config, Some(mouse))
  | None ->
    fail "Failed to create mouse"
    (config, None)

let draw (config:Config<GameState>) viewMatrix projectionMatrix =
  let state = config.State
  let scale = Vector3.One * 1.0f
  let rotation = Vector3.UnitZ * 0.0f

  debug $"Mouse: {config.Mouse.Position}"
  
  // Draw mouse
  Primitives.ShadedObject.Draw
    config
    viewMatrix
    projectionMatrix
    state.DebugScene.Mouse
    scale
    rotation
    Vector3.Zero
