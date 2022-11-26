module Paint.Lib.Graphics

open Womb
open Womb.Graphics

let createQuad vertexPaths fragmentPaths vertices indices =
  match (
    Display.compileShader
      vertexPaths
      fragmentPaths
   ) with
  | Some(shader) -> 
      Some(
        Primitives.ShadedObject.Quad(
          Primitives.ShadedObjectContext.From vertices indices,
          shader
        )
      )
  | None ->
      Logging.fail $"Failed to compile quad shaders:\n{vertexPaths}\n{fragmentPaths}"
      None
