open Argu
open Paint.State
open SDL2Bindings
open System
open System.Numerics
open Womb
open Womb.Graphics
open Womb.Input

let DEFAULT_WIDTH = 800u
let DEFAULT_HEIGHT = 600u

type CliArguments =
  | [<Unique>][<EqualsAssignmentOrSpaced>] Width of width:uint
  | [<Unique>][<EqualsAssignmentOrSpaced>] Height of height:uint

  interface IArgParserTemplate with
    member s.Usage =
      match s with
      | Width _ -> $"set the initial display width (default: %d{DEFAULT_WIDTH})"
      | Height _ -> $"set the initial display height (default: %d{DEFAULT_HEIGHT})"

let private handleKeyUp (config:Config<GameState>) (event:SDL.SDL_Event) : Config<GameState> =
  match event.key.keysym.sym with
  | SDL.SDL_Keycode.SDLK_ESCAPE -> config.StopHandler config
  | SDL.SDL_Keycode.SDLK_F12 ->
    { config with
        State =
          { config.State with
              IsDebug = not config.State.IsDebug }}
  | _ -> config

let private initHandler (config:Config<GameState>) =
  match Paint.Scene.DrawScene.createUI config with
  | (config, Some(canvas), Some(commandPanel), Some(lineBrush)) ->
    { config with
        State =
          { GameState.Default with
              Canvas = canvas
              CommandPanel = commandPanel
              LineBrushes = [lineBrush] } }
  | (config, Some(canvas), Some(commandPanel), None) ->
    Logging.fail "Successfully created UI canvas and Command Panel but failed to create Line Brush for Paint Scene"
    config
  | (config, Some(canvas), None, None) ->
    Logging.fail "Successfully created UI canvas but failed to create UI Command Panel for Paint Scene"
    config
  | _ ->
    Logging.fail "Failed to create UI for Paint Scene"
    config

let private calculateMatrices cameraPosition cameraTarget =
  let viewMatrix = Matrix4x4.CreateLookAt(
    cameraPosition,
    cameraTarget,
    Vector3.UnitY
  )
  let projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, 1f, 0f, 1f, 0f, 1f)
  (viewMatrix, projectionMatrix)

let private drawHandler (config:Config<GameState>) =
  Logging.debug_if config.State.IsDebug $"Config {config}"
  let cameraPosition = new Vector3(0f, 0f, 1f)
  let cameraTarget = new Vector3(0f, 0f, 0f)
  let (viewMatrix, projectionMatrix) = calculateMatrices cameraPosition cameraTarget

  let displayConfig = Engine.Internals.drawBegin config.DisplayConfig
  Paint.Scene.DrawScene.draw
    config
    viewMatrix
    projectionMatrix
  { config with
      DisplayConfig = Engine.Internals.drawEnd displayConfig }

[<EntryPoint>]
let main argv =
  let errorHandler =
    ProcessExiter(
      colorizer=function
        | ErrorCode.HelpText -> None
        | _ -> Some ConsoleColor.Red )
  let parsedArgs =
    ArgumentParser.Create<CliArguments>(programName="Paint", errorHandler=errorHandler).Parse argv

  let width = parsedArgs.GetResult(Width, DEFAULT_WIDTH)
  let height = parsedArgs.GetResult(Height, DEFAULT_HEIGHT)

  ( Game.play
      "Paint"
      width
      height
      GameState.Default
      (Some initHandler)
      (Some handleKeyUp)
      (Some drawHandler) ).ExitCode
