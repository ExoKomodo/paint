module Paint.Lib.Graphics

#nowarn "9" // Unverifiable IL due to fixed expression and NativePtr library usage

open System.Numerics
open System.Text
open Womb.Backends.OpenGL.Api
open Womb.Backends.OpenGL.Api.Constants
open Womb.Graphics.Primitives
open Womb.Logging

let private _glGetUniformLocationEasy (program:uint) (name:string) =
  use namePtr = fixed Encoding.ASCII.GetBytes(name) in
    glGetUniformLocation program namePtr

let private _glUniformMatrix4fvEasy location count (value:Matrix4x4) =
  let buffer = [|
    value.M11; value.M12; value.M13; value.M14;
    value.M21; value.M22; value.M23; value.M24;
    value.M31; value.M32; value.M33; value.M34;
    value.M41; value.M42; value.M43; value.M44;
  |]
  use bufPtr = fixed buffer in
    glUniformMatrix4fv location count false bufPtr

let private _useMvpShader shader (viewMatrix:Matrix4x4) (projectionMatrix:Matrix4x4) (scale:Vector3) (rotation:Vector3) (translation:Vector3) =
  glUseProgram shader
  let mvpUniform = _glGetUniformLocationEasy shader "mvp"

  let scaleMatrix = Matrix4x4.CreateScale(scale)
  let rotationMatrix = Matrix4x4.CreateFromYawPitchRoll(rotation.Y, rotation.X, rotation.Z)
  let translationMatrix = Matrix4x4.CreateTranslation(translation)
  let modelMatrix = scaleMatrix * rotationMatrix * translationMatrix

  let mvp = modelMatrix * viewMatrix * projectionMatrix
  _glUniformMatrix4fvEasy mvpUniform 1 mvp

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

let drawTransformedShadedLine (viewMatrix:Matrix4x4) (projectionMatrix:Matrix4x4) (primitive:ShadedObject) (scale:Vector3) (rotation:Vector3) (translation:Vector3) =
  let shader = primitive.Shader
  _useMvpShader shader viewMatrix projectionMatrix scale rotation translation

  glBindVertexArray primitive.VertexData.VAO
  glBindBuffer
    GL_ELEMENT_ARRAY_BUFFER
    primitive.VertexData.EBO
  glDrawArrays
    GL_LINES
    0
    primitive.Indices.Length

let drawTransformedShadedObject (viewMatrix:Matrix4x4) (projectionMatrix:Matrix4x4) (primitive:ShadedObject) (scale:Vector3) (rotation:Vector3) (translation:Vector3) =
  let shader = primitive.Shader
  _useMvpShader shader viewMatrix projectionMatrix scale rotation translation
  
  glBindVertexArray primitive.VertexData.VAO
  glBindBuffer
    GL_ELEMENT_ARRAY_BUFFER
    primitive.VertexData.EBO
  glDrawElements
    GL_TRIANGLES
    primitive.Indices.Length
    GL_UNSIGNED_INT
    GL_NULL