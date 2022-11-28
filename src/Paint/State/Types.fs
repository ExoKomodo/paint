module Paint.State

open Paint.Brushes.Types
open Paint.Debug.Types
open Paint.UI.Types
open Womb.Graphics

type DebugSceneState =
  { Mouse: option<Mouse>;
    IsEnabled: bool; }
  
    static member Default = {
      Mouse = None
      IsEnabled = false }

type DrawSceneState =
  { Canvas: option<Canvas>;
    CommandPanel: option<CommandPanel>;
    LineBrushes: list<LineBrush>; }
  
    static member Default = {
      Canvas = None
      CommandPanel = None
      LineBrushes = List.Empty }


type GameState =
  { DebugScene: DebugSceneState
    DrawScene: DrawSceneState }
  
    static member Default = {
      DebugScene = DebugSceneState.Default
      DrawScene = DrawSceneState.Default }

