module Paint.Brushes.Types

open System.Numerics
open Womb.Graphics

type CircleBrush =
  { Primitive: Primitives.ShadedObject
    Color: Vector4
    Center: Vector2
    RadiusPoint: option<Vector2> }

type LineBrush =
  { Primitive: Primitives.ShadedObject
    Color: Vector4
    Start: Vector2
    End: option<Vector2> }
