module Paint.State

open Paint.Brushes.Types
open Paint.UI.Types

type DebugSceneState =
  { Mouse: option<DebugMouse>;
    IsEnabled: bool; }
  
    static member Default = {
      Mouse = None
      IsEnabled = false }

type DrawSceneState =
  { Canvas: option<Canvas>;
    CommandPanel: option<CommandPanel>;
    CircleBrushes: list<CircleBrush>;
    LineBrushes: list<LineBrush>; }
  
    static member Default = {
      Canvas = None
      CommandPanel = None
      CircleBrushes = List.Empty
      LineBrushes = List.Empty }


type GameState =
  { DebugScene: DebugSceneState
    DrawScene: DrawSceneState }
  
    static member Default = {
      DebugScene = DebugSceneState.Default
      DrawScene = DrawSceneState.Default }

