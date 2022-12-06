module Paint.Types

open Argu
open Paint.Scene.Types

type CliArguments =
  | [<Unique>][<EqualsAssignmentOrSpaced>] Width of width:uint
  | [<Unique>][<EqualsAssignmentOrSpaced>] Height of height:uint

  static member DefaultHeight = 600u
  static member DefaultWidth = 800u

  interface IArgParserTemplate with
    member s.Usage =
      match s with
      | Width _ -> $"set the initial display width (default: {CliArguments.DefaultWidth})"
      | Height _ -> $"set the initial display height (default: {CliArguments.DefaultHeight})"

type GameState =
  { DebugScene: DebugSceneState
    DrawScene: DrawSceneState }
  
    static member Default = {
      DebugScene = DebugSceneState.Default
      DrawScene = DrawSceneState.Default }
