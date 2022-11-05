module Paint.Board

open Womb
open Womb.Graphics
open System

let createBoard =
  Primitives.ShadedObject.From
    { Primitives.ShadedObject.Default with
        FragmentShaderPath = "Resources/Shaders/Board/fragment.glsl"
        VertexShaderPath = "Resources/Shaders/Board/vertex.glsl" }
    [|
      // bottom left
      -1f; -1f; 0.0f;
      // shared top left
      -1f; 1f; 0.0f;
      // shared bottom right
      1f; -1f; 0.0f;
      // top right
      1f; 1f; 0.0f;
    |]
    [|
      0u; 1u; 2u; // first triangle vertex order as array indices
      1u; 2u; 3u; // second triangle vertex order as array indices
    |]
