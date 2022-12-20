module Paint.UI.Types

open Womb.Graphics

type Button =
  { Primitive: Primitives.ShadedObject
    Name: string }

type Canvas =
  { Primitive: Primitives.ShadedObject }

type CommandPanel =
  { Primitive: Primitives.ShadedObject }

[<NoComparison>]
type DebugMouse =
  { Primitive: Primitives.ShadedObject }
