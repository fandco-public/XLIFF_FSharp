namespace XLIFF_Library

module XLIFF_v1_2 =

    type XLIFF_v1_2_Class() =

        member val ready : bool = false
        member val document : System.Xml.XmlDocument


        // READ

        member this.ReadXliffFile(file : string) =
            printfn "Reading XLIFF file %s" file
            raise (new System.Exception("Not implemented")) |> ignore


        member this.ReadResxOrReswFile(file) =
            printfn "Reading RESX/W file %s" file
            failwith "Not implemented" |> ignore


        // WRITE

        member this.WriteXliffFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Writing file %s" file
            failwith "Not implemented" |> ignore


        member this.WriteResxOrReswFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Writing file %s" file
            failwith "Not implemented" |> ignore


        // CHECK/VALIDATE/VERIFY/PARSE

        member this.CheckXliffFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Checking file %s" file
            failwith "Not implemented" |> ignore


        member this.CheckResxOrReswFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Checking file %s" file
            failwith "Not implemented" |> ignore



