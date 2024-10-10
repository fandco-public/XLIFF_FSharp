namespace XLIFF_Library

open System

open System.IO
open System.Reflection
open System.Xml
open System.Xml.Linq
open System.Xml.Schema

module XLIFF_v1_2 =

    type XLIFF_v1_2_Class() =


        // INIT Schema Set
        let schemaPathsForXLIFFv1_2: string list = [ @"Schemas\v1.2\xliff-core-1.2-transitional.xsd";
                                                     @"Schemas\v1.2\xml.xsd"
                                                     ]
        let schemaSetForXLIFFv1_2 = new System.Xml.Schema.XmlSchemaSet()
        let assemblyLocation = Assembly.GetExecutingAssembly().Location
        //let baseDir = AppDomain.CurrentDomain.BaseDirectory
        let libraryDirectory = System.IO.Path.GetDirectoryName(assemblyLocation)
        //let libraryDirectory = ""

        do for path in schemaPathsForXLIFFv1_2 do
            let schemaPath = Path.Combine(libraryDirectory, path)
            schemaSetForXLIFFv1_2.Add(null, schemaPath) |> ignore

        // MEMBERS

        member val ready : bool = false
        member val documentForXLIFFv1_2 : System.Xml.Linq.XDocument = new System.Xml.Linq.XDocument() with get, set

        // READ

        member this.ReadXliffFile(xmlPath : string) : unit =
            printfn "Reading XLIFF file %s\n\n" xmlPath

            // Function to handle validation events
            let validationEventHandler (e: ValidationEventArgs): unit =
                if e.Severity = XmlSeverityType.Warning then
                    printfn "\nWarning: %s at line %d, position %d" e.Message e.Exception.LineNumber e.Exception.LinePosition
                elif e.Severity = XmlSeverityType.Error then
                    printfn "\nError: %s at line %d, position %d" e.Message e.Exception.LineNumber e.Exception.LinePosition
                else printfn "\nValidation event: %s" e.Message
                ()

            // Create a reader settings object
            let settings = new XmlReaderSettings()
            settings.Schemas <- schemaSetForXLIFFv1_2
            settings.ValidationType <- ValidationType.Schema

            // Create an XmlReader object with the settings and validate
            use reader = XmlReader.Create(xmlPath, settings)
            this.documentForXLIFFv1_2 <- XDocument.Load(reader)
            reader.Close()
            printfn "XML document is valid."


        member this.ReadResxOrReswFile(file) =
            printfn "Reading RESX/W file %s" file
            failwith "Not implemented"


        // WRITE

        member this.WriteXliffFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!"))

            printfn "Writing file %s" file
            failwith "Not implemented"


        member this.WriteResxOrReswFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Writing file %s" file
            failwith "Not implemented"


        // CHECK/VALIDATE/VERIFY/PARSE

        member this.CheckXliffFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Checking file %s" file
            failwith "Not implemented"


        member this.CheckResxOrReswFile(file) =
            if not this.ready then
                raise (new System.Exception("Not ready!")) |> ignore

            printfn "Checking file %s" file
            failwith "Not implemented"



