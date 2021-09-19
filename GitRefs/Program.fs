module GitRefs.Program 

open GitRefs

[<EntryPoint>]
let main argv =
    let projectDir = if Array.isEmpty argv then "." else argv.[0]
    let gitDir = $"{projectDir}/.git"
    let refs = Refs.getAllRefs gitDir
    for ref in refs do
        printfn $"{ref}"
    0