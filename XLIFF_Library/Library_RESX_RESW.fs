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

        member this.ReadDontValidateResx(xmlPath : string) : unit =
            printfn "Reading XLIFF file %s\n\n" xmlPath
            this.ReadDontValidate(xmlPath)

        // VALIDATE

        member this.ReadAndValidateResx(file) =
            //if not this.readyProperty then
            //    raise (new System.Exception("Not ready!"))
            this.ReadAndValidate(file)


        // WRITE

        member this.Write(file) =
            match this.Problems with
            | Some _ -> raise (new System.Exception("Not ready!"))
            | None -> base.Write(file)

