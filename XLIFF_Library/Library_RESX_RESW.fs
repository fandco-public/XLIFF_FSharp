namespace XLIFF_Library

open System
open System.IO
open System.Reflection
open System.Xml
open System.Xml.Linq
open System.Xml.Schema

open Library_XML


module RESX_RESW = 

    type RESX_Class() = 

        inherit XML_Class( None)

        // READ

        member this.ReadResxFile(xmlPath : string) : unit =
            printfn "Reading XLIFF file %s\n\n" xmlPath
            this.Read(xmlPath)

        // CHECK/VALIDATE/VERIFY/PARSE

        member this.CheckResxFile(file) =
            //if not this.readyProperty then
            //    raise (new System.Exception("Not ready!"))
            this.Check(file)


        // WRITE

        member this.WriteResxFile(file) =
            match this.Problems with
            | Some _ -> raise (new System.Exception("Not ready!"))
            | None -> this.Write(file)

