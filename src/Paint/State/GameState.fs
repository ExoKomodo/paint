module Paint.State

open Womb.Graphics

type GameState =
  { Canvas: Primitives.ShadedObject;
    CommandPanel: Primitives.ShadedObject;
    LineBrushes: list<Primitives.ShadedObject>;
    IsDebug: bool }
  
    static member Default = {
      Canvas = Primitives.ShadedObject.Default
      CommandPanel = Primitives.ShadedObject.Default
      LineBrushes = List.Empty
      IsDebug = false }
