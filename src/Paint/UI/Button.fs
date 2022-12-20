module Paint.UI.Button

open Paint.UI.Types
open Womb.Graphics
open Womb.Lib.Types

let create (name:string) (translation:Vector3): option<Button> =
  let fragmentPaths = [
    $"Assets/Shaders/UI/{name}/fragment.glsl";
  ]
  let vertexPaths = ["Assets/Shaders/Common/vertex.glsl"]
  let (width, height) = 0.08f, 0.08f
  let transform =
    { Transform.Default() with
        Translation = translation }
  match Primitives.ShadedObject.CreateQuad vertexPaths fragmentPaths transform width height with
  | Some primitive ->
      { Primitive = primitive
        Name = name } |> Some
  | None -> None
