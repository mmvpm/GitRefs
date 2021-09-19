module GitRefs.Program 

open System
open GitRefs

[<EntryPoint>]
let main _ =
    printf "Enter path to project directory: "
    let projectDirectory = Console.ReadLine() + "/.git"
    let refs = Refs.getAllRefs projectDirectory
    for ref in refs do
        printfn $"{ref}"
    0