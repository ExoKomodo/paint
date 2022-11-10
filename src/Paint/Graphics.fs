module Paint.Graphics

#nowarn "9" // Unverifiable IL due to fixed expression and NativePtr library usage

open System.Numerics
open System.Text
open Womb.Backends.OpenGL.Api
open Womb.Backends.OpenGL.Api.Constants
open Womb.Graphics.Primitives
open Womb.Logging

let private glGetUniformLocationEasy (program:uint) (name:string) =
  use namePtr = fixed Encoding.ASCII.GetBytes(name) in
    glGetUniformLocation program namePtr

let private glUniformMatrix4fvEasy location count (value:Matrix4x4) =
  let buffer = [|
    value.M11; value.M21; value.M31; value.M41;
    value.M12; value.M22; value.M32; value.M42;
    value.M13; value.M23; value.M33; value.M43;
    value.M14; value.M24; value.M34; value.M44;
  |]
  use bufPtr = fixed buffer in
    glUniformMatrix4fv location count false bufPtr

let drawShadedLine (primitive:ShadedObject) =
  glUseProgram primitive.Shader
  glBindVertexArray primitive.VertexData.VAO
  glBindBuffer
    GL_ELEMENT_ARRAY_BUFFER
    primitive.VertexData.EBO
  glDrawArrays
    GL_LINES
    0
    primitive.Indices.Length

let drawTransformedShadedObject (primitive:ShadedObject) (scale:Vector3) (rotation:Vector3) (translation:Vector3) =
  let shader = primitive.Shader
  glUseProgram shader
  let mvpUniform = glGetUniformLocationEasy shader "mvp"
  
  let model = Matrix4x4.CreateTranslation(translation) * Matrix4x4.CreateScale(scale)
  let view = Matrix4x4.CreateLookAt(
    new Vector3(0f, 0f, -1f),
    new Vector3(0f, 0f, 0f),
    Vector3.UnitY
  )
  let projection = Matrix4x4.CreateOrthographic(1.0f, 1.0f, -1.0f, 1.0f)
  glUniformMatrix4fvEasy mvpUniform 1 (projection * view * model)
  glBindVertexArray primitive.VertexData.VAO
  glBindBuffer
    GL_ELEMENT_ARRAY_BUFFER
    primitive.VertexData.EBO
  glDrawElements
    GL_TRIANGLES
    primitive.Indices.Length
    GL_UNSIGNED_INT
    GL_NULL
