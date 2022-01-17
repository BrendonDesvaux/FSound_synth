﻿module Synthe.tests

open NUnit.Framework
open waves


[<SetUp>]
let Setup () =
    ()

[<Test>]
let sineWave() =
   let result = sinWave.Length
   Assert.AreEqual(120000, result)
[<Test>]
let triangle() =
   let result = triangle.Length
   Assert.AreEqual(120000, result)

[<Test>]
let squarre() =
   let result = squareWave.Length
   Assert.AreEqual(120000, result)

[<Test>]
let Sawtooth() =
   let result = sawWave.Length
   Assert.AreEqual(120000, result)
   