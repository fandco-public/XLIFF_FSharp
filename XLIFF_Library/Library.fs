namespace XLIFF_Library

module XLIFF_v1_2 =

    type XLIFF_v1_2_Class() =
        member this.Hello(name) =
            printfn "Hello %s" name

        member this.ThrowsException() =
            raise (new System.Exception("Throws exception!")) |> ignore

        member this.ParseXliffFile(file) =
            printfn "Parsing file %s" file

    let hello name =
        printfn "Hello %s" name
