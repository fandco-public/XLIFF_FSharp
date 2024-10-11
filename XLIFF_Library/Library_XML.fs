namespace XLIFF_Library

open System

open System.IO
open System.Reflection
open System.Xml
open System.Xml.Linq
open System.Xml.Schema

module Library_XML = 

    type XML_Class( pSchemaPaths: string list option) = 


        // INIT Schema Set

        let xmlSchemaFile = "Schemas\XML\xml.xsd"
        let schemaPaths = 
            match pSchemaPaths with
            | None -> [xmlSchemaFile]
            | Some schemaPaths -> xmlSchemaFile :: pSchemaPaths.Value

        let schemaSet = new System.Xml.Schema.XmlSchemaSet()
        let assemblyLocation = Assembly.GetExecutingAssembly().Location
        let libraryDirectory = System.IO.Path.GetDirectoryName(assemblyLocation)
        do for path in schemaPaths do
            let schemaPath = Path.Combine(libraryDirectory, path)
            schemaSet.Add(null, schemaPath) |> ignore


        // MEMBERS

        member val Document : System.Xml.Linq.XDocument = new System.Xml.Linq.XDocument() with get, set

        member val Warnings : string list option = None with get, set

        member val Errors : string list option = None with get, set

        member val OtherValidationEvents : string list option = None with get, set

        // Expose SchemaSet as a property
        member this.SchemaSet = schemaSet


        // Function to handle validation events
        member this.ValidationEventHandler (e: ValidationEventArgs): unit =
            if e.Severity = XmlSeverityType.Warning then
                let warningMessage = sprintf "\nWarning: %s at line %d, position %d" e.Message e.Exception.LineNumber e.Exception.LinePosition
                this.Warnings <- 
                    match this.Warnings with
                    | None -> Some [warningMessage]
                    | Some warnings -> Some (warningMessage :: warnings)

            elif e.Severity = XmlSeverityType.Error then
                let errorMessage = sprintf "\nError: %s at line %d, position %d" e.Message e.Exception.LineNumber e.Exception.LinePosition
                this.Errors <- 
                    match this.Errors with
                    | None -> Some [errorMessage]
                    | Some errors -> Some (errorMessage :: errors)
                
            else let otherMessage = sprintf "\nValidation event: %s" e.Message
                 this.OtherValidationEvents <- 
                    match this.OtherValidationEvents with
                    | None -> Some [otherMessage]
                    | Some events -> Some (otherMessage :: events)
            ()


        // Were there errors, warning, or other events?
        member this.Problems = 
            let countElements (lst: string list option) =
                match lst with
                | None -> 0
                | Some l -> List.length l

            let warningsCount = countElements this.Warnings
            let errorsCount = countElements this.Errors
            let otherEventsCount = countElements this.OtherValidationEvents

            if (warningsCount + errorsCount + otherEventsCount) > 0 
               then Some (warningsCount + errorsCount + otherEventsCount)
               else None


        // Define a function to validate the RESX file
        member this.Check (xmlPath: string) : int option =                

            this.Read(xmlPath)
            this.Problems


        // READ

        member this.Read(xmlPath : string) : unit =

            // Create XmlReaderSettings with schema validation
            let settings = XmlReaderSettings()
            settings.ValidationType <- ValidationType.Schema
            settings.ValidationFlags <- XmlSchemaValidationFlags.ProcessInlineSchema
                                        ||| XmlSchemaValidationFlags.ProcessSchemaLocation
                                        ||| XmlSchemaValidationFlags.ReportValidationWarnings
            settings.Schemas.Add(this.SchemaSet)
        
            // Add a validation event handler to capture errors
            settings.ValidationEventHandler.Add( this.ValidationEventHandler)
        
            // Create an XmlReader for the RESX file
            use xmlReader = XmlReader.Create(xmlPath, settings)
        
            // Parse the RESX file
            XDocument.Load(xmlReader) |> ignore
            xmlReader.Close()
            printfn "XML document is loaded."


    



        // WRITE

        member this.Write(filePath) =
            
            if  this.Check(filePath) <> None then
                raise (new System.Exception("Not ready!")) |> ignore

            // test is this.Document is empty
            if this.Document.Root = null then
                raise (new System.Exception("No data to write!")) |> ignore

            // test if filePath is empty
            if String.IsNullOrEmpty(filePath) then
                raise (new System.Exception("No file path!")) |> ignore

            // check if directory exists
            if not (Directory.Exists(Path.GetDirectoryName(filePath))) then
                Directory.CreateDirectory(filePath) |> ignore

            // write this.Document to filePath
            this.Document.Save(filePath)





