module Paint.State

open Womb.Graphics

type DebugSceneState =
  { Mouse: Primitives.ShadedObject;
    IsEnabled: bool; }
  
    static member Default = {
      Mouse = Primitives.ShadedObject.Default
      IsEnabled = false }

type DrawSceneState =
  { Canvas: Primitives.ShadedObject;
    CommandPanel: Primitives.ShadedObject;
    LineBrushes: list<Primitives.ShadedObject>; }
  
    static member Default = {
      Canvas = Primitives.ShadedObject.Default
      CommandPanel = Primitives.ShadedObject.Default
      LineBrushes = List.Empty }


type GameState =
  { DebugScene: DebugSceneState
    DrawScene: DrawSceneState }
  
    static member Default = {
      DebugScene = DebugSceneState.Default
      DrawScene = DrawSceneState.Default }

