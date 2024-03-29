open Argu
open Paint.Scene
open Paint.Types
open System
open System.Numerics
open Womb
open Womb.Graphics
open Womb.Types

let help config =
  Logging.info "
<ESC>   Quit the game
<C>     Add circle brush anchor point (Down to create center point. Up to calculate radius.)
<SPACE> Add line brush anchor point (Down to create start point. Up to anchor end point.)
<F12>   Show debug menu
  "
  config

let private initDebugScene config =
  match DebugScene.createUI config with
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

let private initDrawScene config =
  match DrawScene.createUI config with
  | (config, Some canvas, Some commandPanel, Some circleButton, Some lineButton) ->
      { config with
          State =
            { config.State with
                DrawScene =
                  { config.State.DrawScene with
                      Canvas = Some canvas
                      CircleButton = Some circleButton
                      LineButton = Some lineButton
                      CommandPanel = Some commandPanel
                      LineBrushes = list.Empty } } }
  | (config, Some canvas, Some commandPanel, Some circleButton, None) ->
      Logging.fail "Successfully created UI canvas and command panel, but failed to create UI and line button for Draw Scene"
      config
  | (config, Some canvas, Some commandPanel, None, None) ->
      Logging.fail "Successfully created UI canvas and command panel, but failed to create UI, circle button, and line button for Draw Scene"
      config
  | (config, Some canvas, None, None, None) ->
      Logging.fail "Successfully created UI canvas but failed to create UI Command Panel, circle button, and line button for Draw Scene"
      config
  | _ ->
      Logging.fail "Failed to create UI for Draw Scene"
      config

let private initHandler config =
  initDebugScene config
    |> initDrawScene
    |> help

let private calculateMatrices cameraPosition cameraTarget =
  let viewMatrix = Matrix4x4.CreateLookAt(
    cameraPosition,
    cameraTarget,
    Vector3.UnitY
  )
  let projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(-1f, 1f, -1f, 1f, 0f, 1f)
  (viewMatrix, projectionMatrix)

let private loopHandler config =
  let cameraPosition = new Vector3(0f, 0f, 1f)
  let cameraTarget = new Vector3(0f, 0f, 0f)
  let (viewMatrix, projectionMatrix) = calculateMatrices cameraPosition cameraTarget

  let displayConfig = Engine.Internals.drawBegin config.DisplayConfig
  let config = 
    DrawScene.draw
      config
      viewMatrix
      projectionMatrix

  if config.State.DebugScene.IsEnabled then
    DebugScene.draw
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

  let width = parsedArgs.GetResult(Width, CliArguments.DefaultWidth)
  let height = parsedArgs.GetResult(Height, CliArguments.DefaultHeight)

  ( Game.play
      "Paint"
      width
      height
      (GameState.Default())
      (Some initHandler)
      (Some DrawSceneHandlers.handleEvent)
      (Some loopHandler) ).ExitCode
