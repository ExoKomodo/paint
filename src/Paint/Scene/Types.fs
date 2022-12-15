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
    CircleButton: option<Button>;
    LineButton: option<Button>;
    LineBrushes: list<LineBrush>;
    ActiveBrush: Brush; }
  
    static member Default () = {
      Canvas = None
      CommandPanel = None
      CircleBrushes = List.Empty
      CircleButton = None
      LineButton = None
      LineBrushes = List.Empty
      ActiveBrush = Circle  }
