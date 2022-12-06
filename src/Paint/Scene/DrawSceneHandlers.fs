module Paint.Scene.DrawSceneHandlers

open Paint.Brushes
open Paint.Brushes.Types
open Paint.Types
open SDL2Bindings
open Womb.Types

let private addCirclePoint (config:Config<GameState>) : Config<GameState> =
  let drawSceneState = config.State.DrawScene

  let circles =
    match drawSceneState.CircleBrushes with
    | [] ->
        match CircleBrush.createWithCenter (config.VirtualMousePosition()) with
        | Some circle -> [circle]
        | None -> []
    | circle :: _ when circle.RadiusPoint.IsSome ->
        match CircleBrush.createWithCenter (config.VirtualMousePosition()) with
        | Some circle -> circle :: drawSceneState.CircleBrushes
        | None -> drawSceneState.CircleBrushes
    | circle :: circles ->
      (
        { circle with
            RadiusPoint = config.VirtualMousePosition() |> Some }
      ) :: circles
  { config with
      State =
        { config.State with
            DrawScene =
              { config.State.DrawScene with
                  CircleBrushes = circles }}}

let private addLineBrushPoint (config:Config<GameState>) : Config<GameState> =
  let drawSceneState = config.State.DrawScene

  let lines =
    match drawSceneState.LineBrushes with
    | [] ->
        match LineBrush.createWithStart (config.VirtualMousePosition()) with
        | Some line -> [line]
        | None -> []
    | line :: _ when line.End.IsSome ->
        match LineBrush.createWithStart (config.VirtualMousePosition()) with
        | Some line -> line :: drawSceneState.LineBrushes
        | None -> drawSceneState.LineBrushes
    | line :: lines ->
      (
        { line with
            End = config.VirtualMousePosition() |> Some }
      ) :: lines
  { config with
      State =
        { config.State with
            DrawScene =
              { config.State.DrawScene with
                  LineBrushes = lines }}}

let handleKeyUp (config:Config<GameState>) (event:SDL.SDL_Event) : Config<GameState> =
  match event.key.keysym.sym with
  | SDL.SDL_Keycode.SDLK_ESCAPE -> config.StopHandler config
  | SDL.SDL_Keycode.SDLK_SPACE -> addLineBrushPoint config
  | SDL.SDL_Keycode.SDLK_c -> addCirclePoint config
  | SDL.SDL_Keycode.SDLK_F12 ->
    { config with
        State =
          { config.State with
              DebugScene =
                { config.State.DebugScene with
                    IsEnabled = not config.State.DebugScene.IsEnabled } } }
  | _ -> config
