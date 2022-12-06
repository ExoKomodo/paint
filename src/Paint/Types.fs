module Paint.Types

open Paint.Scene.Types

type GameState =
  { DebugScene: DebugSceneState
    DrawScene: DrawSceneState }
  
    static member Default = {
      DebugScene = DebugSceneState.Default
      DrawScene = DrawSceneState.Default }
