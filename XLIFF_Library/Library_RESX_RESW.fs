namespace XLIFF_Library

open System
open System.IO
open System.Reflection
open System.Xml
open System.Xml.Linq
open System.Xml.Schema

open Library_XML


module Library_RESX_RESW = 

    type RESX_Class() = 

        inherit XML_Class( None)

        // READ

        member this.ReadResxFile(xmlPath : string) : unit =
            printfn "Reading XLIFF file %s\n\n" xmlPath
            this.Read(xmlPath)


        // WRITE

        member this.WriteResxFile(file) =
            if not this.readyProperty then
                raise (new System.Exception("Not ready!"))
            this.Write(file)

