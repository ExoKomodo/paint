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
  let projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, 1f, 0f, 1f, 0f, 1f)
  (viewMatrix, projectionMatrix)

let private loopHandler config =
  let cameraPosition = new Vector3(0f, 0f, 1f)
  let cameraTarget = new Vector3(0f, 0f, 0f)
  let (viewMatrix, projectionMatrix) = calculateMatrices cameraPosition cameraTarget

  let displayConfig = Engine.Internals.drawBegin config.DisplayConfig
  DrawScene.draw
    config
    viewMatrix
    projectionMatrix

  if config.State.DebugScene.IsEnabled then
    DebugScene.draw
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
