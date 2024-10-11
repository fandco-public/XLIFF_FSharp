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

        let mutable ready : bool = false;


        // MEMBERS

        member val document : System.Xml.Linq.XDocument = new System.Xml.Linq.XDocument() with get, set

    
        // ACCESS FIELD

        member this.readyProperty
            with get() = ready
            and set(pReady : bool) = ready <- pReady


        // READ

        member this.Read(xmlPath : string) : unit =
            printfn "Reading XLIFF file %s\n\n" xmlPath

            // Function to handle validation events
            let validationEventHandler (e: ValidationEventArgs): unit =
                if e.Severity = XmlSeverityType.Warning then
                    printfn "\nWarning: %s at line %d, position %d" e.Message e.Exception.LineNumber e.Exception.LinePosition
                elif e.Severity = XmlSeverityType.Error then
                    printfn "\nError: %s at line %d, position %d" e.Message e.Exception.LineNumber e.Exception.LinePosition
                else printfn "\nValidation event: %s" e.Message
                ()

            // Create a validating reader settings object
            let settingsValidating = new XmlReaderSettings()
            settingsValidating.ValidationFlags <- XmlSchemaValidationFlags.ReportValidationWarnings 
                                                  ||| XmlSchemaValidationFlags.ProcessInlineSchema 
                                                  ||| XmlSchemaValidationFlags.ProcessSchemaLocation
            settingsValidating.Schemas <- schemaSet
            settingsValidating.ValidationType <- ValidationType.Schema
            settingsValidating.IgnoreWhitespace <- false
            settingsValidating.IgnoreComments <- false
            settingsValidating.IgnoreProcessingInstructions <- true // seems like a security risk
            settingsValidating.ConformanceLevel <- ConformanceLevel.Document

            // Create an XmlReader object with the settings and validate
            use readerValidating = XmlReader.Create(xmlPath, settingsValidating)
            this.document <- XDocument.Load(readerValidating)
            this.readyProperty <- true
            readerValidating.Close()
            printfn "XML document is valid."

            // Create a plain reader settings object
            let settingsPlain = new XmlReaderSettings()
            settingsValidating.ValidationFlags <- XmlSchemaValidationFlags.ReportValidationWarnings 
                                                  ||| XmlSchemaValidationFlags.ProcessInlineSchema 
                                                  ||| XmlSchemaValidationFlags.ProcessSchemaLocation
            settingsPlain.Schemas <- schemaSet
            settingsPlain.ValidationType <- ValidationType.None
            settingsPlain.IgnoreWhitespace <- false  // leave original file untouched
            settingsPlain.IgnoreComments <- true // leave original file untouched
            settingsPlain.IgnoreProcessingInstructions <- true // seems like a security risk 
            settingsPlain.ConformanceLevel <- ConformanceLevel.Document

            // Create an XmlReader object with plain settings and read
            use readerPlain = XmlReader.Create(xmlPath, settingsPlain)
            this.document <- XDocument.Load(readerPlain)
            this.readyProperty <- true
            readerPlain.Close()
            printfn "XML document is loaded."


        // WRITE

        member this.Write(filePath) =
            if not this.readyProperty then
                raise (new System.Exception("Not ready!")) |> ignore

            // test is this.document is empty
            if this.document.Root = null then
                raise (new System.Exception("No data to write!")) |> ignore

            // test if filePath is empty
            if String.IsNullOrEmpty(filePath) then
                raise (new System.Exception("No file path!")) |> ignore

            // check if directory exists
            if not (Directory.Exists(Path.GetDirectoryName(filePath))) then
                Directory.CreateDirectory(filePath) |> ignore

            // write this.document to filePath
            this.document.Save(filePath)





