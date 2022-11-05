open Argu
open Paint.Board
open Paint.UI.CommandPanel
open Womb
open Womb.Graphics
open System

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

let mutable private boardPrimitive = Primitives.ShadedObject.Default
let mutable private commandPanelPrimitive = Primitives.ShadedObject.Default

let private initHandler config =
  boardPrimitive <- createBoard
  commandPanelPrimitive <- createCommandPanel
  match Display.compileShader boardPrimitive.VertexShaderPath boardPrimitive.FragmentShaderPath with
  | Some(boardShader) -> 
      boardPrimitive <-
        { boardPrimitive with
            Shader = boardShader
            VertexData = Primitives.VertexObjectData.From boardPrimitive.Vertices boardPrimitive.Indices }
      match Display.compileShader commandPanelPrimitive.VertexShaderPath commandPanelPrimitive.FragmentShaderPath with
      | Some(commandPanelShader) -> 
          commandPanelPrimitive <-
            { commandPanelPrimitive with
                Shader = commandPanelShader
                VertexData = Primitives.VertexObjectData.From commandPanelPrimitive.Vertices commandPanelPrimitive.Indices }
          config
      | None ->
          Logging.fail "Failed to compile command panel shader"
          config
  | None ->
      Logging.fail "Failed to compile board shader"
      config

let private drawHandler config =
  let config = Display.clear config
  Primitives.drawShadedObject boardPrimitive
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
