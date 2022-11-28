module Paint.Brushes.Types

open System.Numerics
open Womb.Graphics

type LineBrush =
  { Primitive: option<Primitives.ShadedObject>
    Color: Vector4
    Start: Vector2
    End: option<Vector2> }
