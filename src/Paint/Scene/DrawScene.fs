module Paint.Scene.DrawScene

open Paint.Brushes
open Paint.Lib
open Paint.State
open Paint.UI
open System.Numerics
open Womb
open Womb.Backends.OpenGL.Api
open Womb.Backends.OpenGL.Api.Constants
open Womb.Graphics
open Womb.Logging
open Womb.Types

let createUI config =
  match Canvas.create with
  | Some(canvas) ->
    match CommandPanel.create with
    | Some(commandPanel) ->
      match LineBrush.create {
        Start=(LineBrush.pointNew2D 0.0f 0.0f);
        End=(LineBrush.pointNew2D 0.4f 0.3f);
      } with
      | Some(lineBrush) ->
        (config, Some(canvas), Some(commandPanel), Some(lineBrush))
      | None ->
        fail "Failed to create line brush"
        (config, Some(canvas), Some(commandPanel), None)
    | None ->
      fail "Failed to create command panel"
      (config, Some(canvas), None, None)
  | None ->
    fail "Failed to create canvas"
    (config, None, None, None)

let private drawShadedLine (config:Config<GameState>) viewMatrix projectionMatrix (primitive:Primitives.ShadedObject) (scale:Vector3) (rotation:Vector3) (translation:Vector3) (line:LineBrush.Data) =
  let shader = primitive.Shader
  glUseProgram shader

  let scaleMatrix = Matrix4x4.CreateScale(scale)
  let rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z)
  let translationMatrix = Matrix4x4.CreateTranslation(translation)
  let modelMatrix = scaleMatrix * rotationMatrix * translationMatrix

  let mvp = modelMatrix * viewMatrix * projectionMatrix
  glUniformMatrix4fvEasy
    (glGetUniformLocationEasy shader "mvp")
    1
    mvp
  
  glUniform2f
    (glGetUniformLocationEasy shader "mouse")
    config.Mouse.Position.X
    config.Mouse.Position.Y

  // let (x, y, z) = line.Start
  // glUniform3f
  //   (glGetUniformLocationEasy shader "start")
  //   x y z

  // let (x, y, z) = line.End
  // glUniform3f
  //   (glGetUniformLocationEasy shader "end")
  //   x y z
  
  glBindVertexArray primitive.VertexData.VAO
  glBindBuffer
    GL_ELEMENT_ARRAY_BUFFER
    primitive.VertexData.EBO
  glDrawElements
    GL_TRIANGLES
    primitive.Indices.Length
    GL_UNSIGNED_INT
    GL_NULL

let draw (config:Config<GameState>) viewMatrix projectionMatrix =
  let state = config.State
  let scale = Vector3.One * 1.0f
  let rotation = Vector3.UnitZ * 0.0f
  
  // Draw canvas
  Primitives.drawShadedObjectWithMvp
    config
    viewMatrix
    projectionMatrix
    state.DrawScene.Canvas
    scale
    rotation
    (new Vector3(0.5f, 0.5f, 0.0f))
  
  // Draw objects on canvas
  List.map
    (
      fun lineBrush ->
        drawShadedLine
          config
          viewMatrix
          projectionMatrix
          lineBrush
          scale
          rotation
          (new Vector3(0.5f, 0.5f, 0.0f))
          { Start = (LineBrush.pointNew2D 0.0f 0.0f)
            End = (LineBrush.pointNew2D 0.4f 0.3f) }

    )
    state.DrawScene.LineBrushes |> ignore

  // Draw UI elements
  Primitives.drawShadedObjectWithMvp
    config
    viewMatrix
    projectionMatrix
    state.DrawScene.CommandPanel
    scale
    rotation
    (new Vector3(0.075f, 0.5f, 0.0f))
