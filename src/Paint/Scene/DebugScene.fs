module Paint.Scene.DebugScene

open Paint.Types
open Paint.UI
open Womb.Graphics
open Womb.Logging
open Womb.Types

let createUI config =
  match DebugMouse.create() with
  | Some mouse ->
      (config, Some mouse)
  | None ->
    fail "Failed to create mouse"
    (config, None)

let draw (config:Config<GameState>) viewMatrix projectionMatrix =
  let state = config.State

  debug $"Mouse: {config.Mouse.Position}"

  // Draw mouse
  match state.DebugScene.Mouse with
  | Some mouse -> 
      Primitives.ShadedObject.Draw
        config
        viewMatrix
        projectionMatrix
        mouse.Primitive
        []
  | None -> ()
