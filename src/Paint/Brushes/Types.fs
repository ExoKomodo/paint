module Paint.Brushes.Types

open System.Numerics
open Womb.Graphics

type LineBrush =
  { Primitive: Primitives.ShadedObject
    Start: Vector2 
    End: Vector2 }
