module Paint.UI.Types

open System.Numerics
open Womb.Graphics

type Canvas =
  { Primitive: Primitives.ShadedObject }

type CommandPanel =
  { Primitive: Primitives.ShadedObject }

type DebugMouse =
  { Primitive: Primitives.ShadedObject
    Position: Vector2 }

