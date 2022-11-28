module Paint.Handlers

open Paint.Brushes
open Paint.Brushes.Types
open Paint.State
open SDL2Bindings
open Womb
open Womb.Types

let private addLineBrushPoint (config:Config<GameState>) : Config<GameState> =
  let drawSceneState = config.State.DrawScene

  let lines =
    match drawSceneState.LineBrushes with
    | [] ->
        match LineBrush.createWithStart config.Mouse.Position with
        | Some line -> [line]
        | None -> []
    | line :: _ when line.End.IsSome ->
        match LineBrush.createWithStart config.Mouse.Position with
        | Some line -> line :: drawSceneState.LineBrushes
        | None -> drawSceneState.LineBrushes
    | line :: lines ->
      (
        { line with
            End = Some config.Mouse.Position }
      ) :: lines
  { config with
      State =
        { config.State with
            DrawScene =
              { config.State.DrawScene with
                  LineBrushes = lines }}}

let help (config:Config<GameState>) : Config<GameState> =
  Logging.info "
<ESC>   Quit the game
<SPACE> Add line brush anchor point (Once to create start point. twice to create end point and show line)
<F12>   Show debug menu
<H>     Show this help screen
  "
  config

let handleKeyUp (config:Config<GameState>) (event:SDL.SDL_Event) : Config<GameState> =
  match event.key.keysym.sym with
  | SDL.SDL_Keycode.SDLK_ESCAPE -> config.StopHandler config
  | SDL.SDL_Keycode.SDLK_SPACE -> addLineBrushPoint config
  | SDL.SDL_Keycode.SDLK_h -> help config
  | SDL.SDL_Keycode.SDLK_F12 ->
    { config with
        State =
          { config.State with
              DebugScene =
                { config.State.DebugScene with
                    IsEnabled = not config.State.DebugScene.IsEnabled } } }
  | _ -> config
