module Paint.UI.Types

open Womb.Graphics

type Button =
  { Primitive: Primitives.ShadedObject }

type Canvas =
  { Primitive: Primitives.ShadedObject }

type CommandPanel =
  { Primitive: Primitives.ShadedObject }

[<NoComparison>]
type DebugMouse =
  { Primitive: Primitives.ShadedObject
    Position: single * single }
