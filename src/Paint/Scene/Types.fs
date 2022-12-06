module Paint.Scene.Types

open Paint.Brushes.Types
open Paint.UI.Types

[<NoComparison>]
type DebugSceneState =
  { Mouse: option<DebugMouse>;
    IsEnabled: bool; }
  
    static member Default () = {
      Mouse = None
      IsEnabled = false }

[<NoComparison>]
type DrawSceneState =
  { Canvas: option<Canvas>;
    CommandPanel: option<CommandPanel>;
    CircleBrushes: list<CircleBrush>;
    LineBrushes: list<LineBrush>; }
  
    static member Default () = {
      Canvas = None
      CommandPanel = None
      CircleBrushes = List.Empty
      LineBrushes = List.Empty }
