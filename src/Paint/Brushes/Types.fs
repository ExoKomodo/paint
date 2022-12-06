module Paint.Brushes.Types

open Womb.Graphics

[<NoComparison>]
type CircleBrush =
  { Primitive: Primitives.ShadedObject;
    Color: single * single * single * single;
    Center: single * single;
    RadiusPoint: option<single * single>; }

[<NoComparison>]
type LineBrush =
  { Primitive: Primitives.ShadedObject;
    Color: single * single * single * single;
    Start: single * single;
    End: option<single * single>; }
