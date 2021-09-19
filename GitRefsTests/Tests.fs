module Tests

open Xunit
open GitRefs

open System.IO
open System.Reflection

let gitTestDir =
    let location = Assembly.GetExecutingAssembly().Location
    Path.Combine(Path.GetDirectoryName(location), "GitTestDir")

let expectedPrimalRefs = [|
    { path = "refs/heads/master"
      commitId = "46e07a0dc62d699cd4eed6d8a1199874a80af275" };
    { path = "refs/remotes/origin/master"
      commitId = "46e07a0dc62d699cb4eed6d8a1189874a80af275" }
|]

let expectedPackedRefs = [|
    { path = "refs/remotes/origin/master"
      commitId = "a55239cdab4c8711e67dd180924bced1d17fea1b" }
|]

let expectedAllRefs = Array.concat [|
    expectedPrimalRefs
    expectedPackedRefs
|]

[<Fact>]
let ``getPrimalRefs should return only primal refs`` () =
    let actual = Refs.getPrimalRefs gitTestDir $"{gitTestDir}/refs/"
    Assert.Equal<Ref>(expectedPrimalRefs, actual)

[<Fact>]
let ``getPackedRefs should return only packed refs`` () =
    let actual = Refs.getPackedRefs $"{gitTestDir}/packed-refs"
    Assert.Equal<Ref>(expectedPackedRefs, actual)

[<Fact>]
let ``getAllRefs should return all refs`` () =
    let actual = Refs.getAllRefs gitTestDir
    Assert.Equal<Ref>(expectedAllRefs, actual)