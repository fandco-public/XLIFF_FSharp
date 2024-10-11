namespace XLIFF_Library

open System
open System.IO
open System.Reflection
open System.Xml
open System.Xml.Linq
open System.Xml.Schema

open Library_XML


module XLIFF_v1_2 =


    // XLIFF v1.2

    type XLIFF_v1_2_Class() =
        inherit XML_Class( Some [ @"Schemas\v1.2\xliff-core-1.2-transitional.xsd"; ])

        // READ

        member this.ReadXliffFile(xmlPath : string) : unit =
            printfn "Reading XLIFF file %s\n\n" xmlPath
            this.Read(xmlPath)


        member this.ReadResxOrReswFile(file) =
            printfn "Reading RESX/W file %s" file
            failwith "Not implemented"


        // WRITE

        member this.WriteXliffFile(file) =
            if not this.readyProperty then
                raise (new System.Exception("Not ready!"))
            this.Write(file)


        member this.WriteResxOrReswFile(file) =
            if not this.readyProperty then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Writing file %s" file
            failwith "Not implemented"


        // CHECK/VALIDATE/VERIFY/PARSE

        member this.CheckXliffFile(file) =
            if not this.readyProperty then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Checking file %s" file
            failwith "Not implemented"


        member this.CheckResxOrReswFile(file) =
            if not this.readyProperty then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Checking file %s" file
            failwith "Not implemented"



