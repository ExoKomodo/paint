open Argu
open System
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

let mutable private canvasPrimitive = Primitives.ShadedObject.Default
let mutable private commandPanelPrimitive = Primitives.ShadedObject.Default

let private initHandler config =
  match Paint.Scene.PaintScene.createUI config with
  | (newConfig, Some(canvas), Some(commandPanel)) ->
    canvasPrimitive <- canvas
    commandPanelPrimitive <- commandPanel
    newConfig
  | (newConfig, Some(canvas), None) ->
    Logging.fail "Successfully create UI canvas but failed to create UI Command Panel for Paint Scene"
    canvasPrimitive <- canvas
    newConfig
  | _ ->
    Logging.fail "Failed to create UI for Paint Scene"
    config

let private drawHandler config =
  let config = Display.clear config
  Primitives.drawShadedObject canvasPrimitive
  Primitives.drawShadedObject commandPanelPrimitive
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
