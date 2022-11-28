module Paint.Debug.Types

open System.Numerics
open Womb.Graphics

type Mouse =
  { Primitive: Primitives.ShadedObject
    Position: Vector2 }
