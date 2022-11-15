open Argu
open Paint.State
open System
open System.Numerics
open Womb
open Womb.Graphics

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

let private initHandler (configState:Engine.Internals.Config * GameState) =
  let (config, state) = configState
  match Paint.Scene.DrawScene.createUI config with
  | (newConfig, Some(canvas), Some(commandPanel), Some(lineBrush)) ->
    ( newConfig,
      { GameState.Default with
          Canvas = canvas
          CommandPanel = commandPanel
          LineBrushes = [lineBrush] })
  | (newConfig, Some(canvas), Some(commandPanel), None) ->
    Logging.fail "Successfully created UI canvas and Command Panel but failed to create Line Brush for Paint Scene"
    (newConfig, state)
  | (newConfig, Some(canvas), None, None) ->
    Logging.fail "Successfully created UI canvas but failed to create UI Command Panel for Paint Scene"
    (newConfig, state)
  | _ ->
    Logging.fail "Failed to create UI for Paint Scene"
    (config, state)

let private calculateMatrices cameraPosition cameraTarget =
  let viewMatrix = Matrix4x4.CreateLookAt(
    cameraPosition,
    cameraTarget,
    Vector3.UnitY
  )
  let projectionMatrix = Matrix4x4.CreateOrthographicOffCenter(0f, 1f, 0f, 1f, 0f, 1f)
  (viewMatrix, projectionMatrix)

let private drawHandler (configState:Display.Config * GameState) =
  let (config, state) = configState
  let cameraPosition = new Vector3(0f, 0f, 1f)
  let cameraTarget = new Vector3(0f, 0f, 0f)
  let (viewMatrix, projectionMatrix) = calculateMatrices cameraPosition cameraTarget

  let clearedConfig = Display.clear config
  Paint.Scene.DrawScene.draw
    (clearedConfig, state)
    viewMatrix
    projectionMatrix
  (Display.swap clearedConfig, state)

[<EntryPoint>]
let main argv =
  let errorHandler =
    ProcessExiter(
      colorizer=function
        | ErrorCode.HelpText -> None
        | _ -> Some ConsoleColor.Red )
  let parsedArgs =
    ArgumentParser.Create<CliArguments>(programName="Paint", errorHandler=errorHandler).Parse argv

  let width : uint = parsedArgs.GetResult(Width, DEFAULT_WIDTH)
  let height : uint = parsedArgs.GetResult(Height, DEFAULT_HEIGHT)

  let (config, _) = (
    Game.play
      "Paint"
      width
      height
      GameState.Default
      (Some initHandler)
      (Some drawHandler) )
  config.ExitCode
