module Paint.UI.CommandPanel

open Womb
open Womb.Graphics
open System

let createCommandPanel =
  Primitives.ShadedObject.From
    { Primitives.ShadedObject.Default with
        FragmentShaderPath = "Resources/Shaders/UI/CommandPanel/fragment.glsl"
        VertexShaderPath = "Resources/Shaders/UI/CommandPanel/vertex.glsl" }
    [|
      // bottom left
      0f; -1f; 0.0f;
      // shared top left
      0f; 1f; 0.0f;
      // shared bottom right
      0.1f; -1f; 0.0f;
      // top right
      0.1f; 1f; 0.0f;
    |]
    [|
      0u; 1u; 2u; // first triangle vertex order as array indices
      1u; 2u; 3u; // second triangle vertex order as array indices
    |]
