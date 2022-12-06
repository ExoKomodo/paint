open Argu
open Paint.Scene
open Paint.State
open System
open System.Numerics
open Womb
open Womb.Graphics
open Womb.Types

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

let help (config:Config<GameState>) : Config<GameState> =
  Logging.info "
<ESC>   Quit the game
<C>     Add circle brush anchor point (Once to create center point. Twice to calculate radius.)
<SPACE> Add line brush anchor point (Once to create start point. Twice to anchor end point.)
<F12>   Show debug menu
  "
  config

let private initDebugScene (config:Config<GameState>) =
  match Paint.Scene.DebugScene.createUI config with
  | (config, Some mouse) ->
    { config with
        State =
          { config.State with
              DebugScene =
                { config.State.DebugScene with
                    Mouse = Some mouse } } }
  | _ ->
    Logging.fail "Failed to create UI for Debug Scene"
    config

let private initDrawScene (config:Config<GameState>) =
  match Paint.Scene.DrawScene.createUI config with
  | (config, Some canvas, Some commandPanel) ->
    { config with
        State =
          { config.State with
              DrawScene =
                { config.State.DrawScene with
                    Canvas = Some canvas
                    CommandPanel = Some commandPanel
                    LineBrushes = list.Empty } } }
  | (config, Some canvas, None) ->
    Logging.fail "Successfully created UI canvas but failed to create UI Command Panel for Draw Scene"
    config
  | _ ->
    Logging.fail "Failed to create UI for Draw Scene"
    config

let private initHandler (config:Config<GameState>) =
  initDebugScene config
    |> initDrawScene
    |> help

let private calculateMatrices cameraPosition cameraTarget =
  let viewMatrix = Matrix4x4.CreateLookAt(
    cameraPosition,
    cameraTarget,
    Vector3.UnitY
  )
  let projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, 1f, 0f, 1f, 0f, 1f)
  (viewMatrix, projectionMatrix)

let private drawHandler (config:Config<GameState>) =
  let cameraPosition = new Vector3(0f, 0f, 1f)
  let cameraTarget = new Vector3(0f, 0f, 0f)
  let (viewMatrix, projectionMatrix) = calculateMatrices cameraPosition cameraTarget

  let displayConfig = Engine.Internals.drawBegin config.DisplayConfig
  Paint.Scene.DrawScene.draw
    config
    viewMatrix
    projectionMatrix

  if config.State.DebugScene.IsEnabled then
    Paint.Scene.DebugScene.draw
      config
      viewMatrix
      projectionMatrix

  match config.State.DrawScene.Canvas with
  | Some canvas ->
      let vertices = [|
        // bottom left
        -0.4f; -0.3f; 0.0f;
        // shared top left
        -0.4f; 0.3f; 0.0f;
        // shared bottom right
        0.4f; -0.3f; 0.0f;
        // top right
        0.4f; 0.3f; 0.0f;
      |]
      { config with
          DisplayConfig = Engine.Internals.drawEnd displayConfig
          State =
            { config.State with
                DrawScene =
                  { config.State.DrawScene with
                      Canvas = Some { Primitive = Primitives.ShadedObject.UpdateVertices vertices canvas.Primitive } } } }
  | None -> config

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
      (Some DrawSceneHandlers.handleKeyUp)
      (Some drawHandler) ).ExitCode
