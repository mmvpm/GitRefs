namespace GitRefs

type Ref = {
    path: string
    commitId: string
}

module Refs =
    open System.IO

    let readRef (gitDir: string) (fileName: string) : Ref [] =
        if (File.Exists fileName && not <| fileName.EndsWith "HEAD") then
             [| {
                 path = fileName.Remove(0, gitDir.Length + 1).Trim()
                 commitId = (File.ReadAllText fileName).Trim()
             } |]
        else
            [||]
    
    let rec getPrimalRefs (gitDir: string) (searchDir: string) : Ref [] =
        let renameSlash = fun (s: string) -> s.Replace ("\\", "/")
        let files = Directory.GetFiles searchDir |> Array.map renameSlash
        let dirs = Directory.GetDirectories searchDir |> Array.map renameSlash
        Array.concat [|
            Array.collect (readRef gitDir) files
            Array.collect (getPrimalRefs gitDir) dirs
        |]

    let getPackedRefs (fileName: string) : Ref [] =
        let createRef (line: string) : Ref [] =
            match line.Split " " with
            | [| commitId; branchName |] -> [| {
                     path = branchName.Trim()
                     commitId = commitId.Trim()
                 } |]
            | _ -> [||]

        if File.Exists fileName then
            File.ReadAllLines fileName
            |> Array.filter (fun line -> not <| line.StartsWith "#") // filter comments
            |> Array.collect createRef
        else
            [||]

    let getAllRefs (gitDir: string) : Ref [] =
        Array.concat [|
            getPrimalRefs gitDir $"{gitDir}/refs/"
            getPackedRefs $"{gitDir}/packed-refs"
        |]