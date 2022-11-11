open Argu
open System
open Womb
open Womb.Graphics
open System.Numerics

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

let mutable private canvasPrimitive = Primitives.ShadedObject.Default
let mutable private commandPanelPrimitive = Primitives.ShadedObject.Default
let mutable private lineBrushPrimitive = Primitives.ShadedObject.Default

let private initHandler config =
  match Paint.Scene.PaintScene.createUI config with
  | (newConfig, Some(canvas), Some(commandPanel), Some(lineBrush)) ->
    canvasPrimitive <- canvas
    commandPanelPrimitive <- commandPanel
    lineBrushPrimitive <- lineBrush
    newConfig
  | (newConfig, Some(canvas), Some(commandPanel), None) ->
    Logging.fail "Successfully created UI canvas and Command Panel but failed to create Line Brush for Paint Scene"
    canvasPrimitive <- canvas
    commandPanelPrimitive <- commandPanel
    newConfig
  | (newConfig, Some(canvas), None, None) ->
    Logging.fail "Successfully created UI canvas but failed to create UI Command Panel for Paint Scene"
    canvasPrimitive <- canvas
    newConfig
  | _ ->
    Logging.fail "Failed to create UI for Paint Scene"
    config

let private drawHandler config =
  let config = Display.clear config
  let scale = Vector3.One * 2.0f
  let rotation = Vector3.UnitZ * 0.0f
  let translation = new Vector3(0.1f, 0.0f, 0.0f)
  
  // Draw canvas
  Paint.Graphics.drawTransformedShadedObject
    canvasPrimitive
    scale
    rotation
    translation
  
  // Draw objects on canvas
  let objectOffset = new Vector3(0.0f, 0.0f, 0.0f)
  Paint.Graphics.drawTransformedShadedLine
    lineBrushPrimitive
    scale
    rotation
    (translation + objectOffset)

  // Draw UI elements
  Paint.Graphics.drawTransformedShadedObject
    commandPanelPrimitive
    scale
    rotation
    (new Vector3(-0.1f, 0.0f, 0.0f))

  Display.swap config

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
      (Some initHandler)
      (Some drawHandler) ).ExitCode
