module Paint.Scene.DrawSceneHandlers

open Paint.Brushes
open Paint.Brushes.Types
open Paint.Scene.Types
open Paint.Types
open SDL2Bindings
open Womb.Types

let private addCirclePoint config =
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

let private addLineBrushPoint config =
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


let private handleKeyDown config (event:SDL.SDL_Event) =
  match event.key.repeat with
  | 0uy ->
    match event.key.keysym.sym with
    | SDL.SDL_Keycode.SDLK_SPACE -> addLineBrushPoint config
    | SDL.SDL_Keycode.SDLK_c -> addCirclePoint config
    | _ -> config
  | _ -> config

let private handleKeyUp config (event:SDL.SDL_Event) =
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

let private handleMouseDown config (event:SDL.SDL_Event) =
  match event.button.button |> uint32 with
  | SDL.SDL_BUTTON_LEFT ->
      let drawScene = config.State.DrawScene
      match drawScene.Canvas, drawScene.CircleButton, drawScene.LineButton with
      | Some canvas, Some circleButton, Some lineButton ->
          match
            (
              Womb.Graphics.Primitives.ShadedObject.Contains canvas.Primitive (config.VirtualMousePosition()),
              Womb.Graphics.Primitives.ShadedObject.Contains circleButton.Primitive (config.VirtualMousePosition()),
              Womb.Graphics.Primitives.ShadedObject.Contains lineButton.Primitive (config.VirtualMousePosition())
            )
          with
          | (Some _, _, _) ->
              match config.State.DrawScene.ActiveBrush with
              | Circle -> addCirclePoint config
              | Line -> addLineBrushPoint config
          | (None, Some _, None) ->
              { config with
                  State =
                    { config.State with
                        DrawScene =
                          { config.State.DrawScene with
                              ActiveBrush = Circle } } }
          | (None, None, Some _) ->
              { config with
                  State =
                    { config.State with
                        DrawScene =
                          { config.State.DrawScene with
                              ActiveBrush = Line } } }
          | _ -> config
      | _, _, _ ->
          Womb.Logging.fail "Canvas, Circle, and Line must all be present"
          config
  | _ -> config

let private handleMouseUp config (event:SDL.SDL_Event) =
  match event.button.button |> uint32 with
  | SDL.SDL_BUTTON_LEFT ->
      let drawScene = config.State.DrawScene
      match drawScene.Canvas, drawScene.CircleButton, drawScene.LineButton with
      | Some canvas, Some circleButton, Some lineButton ->
          match
            (
              Womb.Graphics.Primitives.ShadedObject.Contains circleButton.Primitive (config.VirtualMousePosition()),
              Womb.Graphics.Primitives.ShadedObject.Contains lineButton.Primitive (config.VirtualMousePosition())
            )
          with
          | (Some _, None) ->
              { config with
                  State =
                    { config.State with
                        DrawScene =
                          { config.State.DrawScene with
                              ActiveBrush = Circle } } }
          | (None, Some _) ->
              { config with
                  State =
                    { config.State with
                        DrawScene =
                          { config.State.DrawScene with
                              ActiveBrush = Line } } }
          | _ ->
            match config.State.DrawScene.ActiveBrush with
              | Circle -> addCirclePoint config
              | Line -> addLineBrushPoint config
      | _, _, _ ->
          Womb.Logging.fail "Canvas, Circle, and Line must all be present"
          config
  | _ -> config

let handleEvent config (event:SDL.SDL_Event) =
  match event.typeFSharp with
  | SDL.SDL_EventType.SDL_KEYUP -> handleKeyUp config event
  | SDL.SDL_EventType.SDL_KEYDOWN -> handleKeyDown config event
  | SDL.SDL_EventType.SDL_MOUSEBUTTONUP -> handleMouseUp config event
  | SDL.SDL_EventType.SDL_MOUSEBUTTONDOWN -> handleMouseDown config event
  | _ -> config
