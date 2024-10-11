module Tests.RESX_RESW

    open System
    open System.IO
    open System.Reflection
    open Xunit
    open XLIFF_Library.RESX_RESW
    open System.Xml.Linq
    open System.Linq
    open Microsoft.FSharp.Core

    let resx_samples = [ 
                          @"Samples\RESX\Resources.de.resx";
                          @"Samples\RESX\Resources.en.resx";
                          @"Samples\RESX\Resources.es.resx";
                          @"Samples\RESX\Resources.fr.resx";
                          @"Samples\RESX\Resources.hi.resx";
                          @"Samples\RESX\Resources.hu.resx";
                          @"Samples\RESX\Resources.zh-hans.resx";
                          @"Samples\RESX\Resources.zh-hant.resx";
                          ]

    let resx_error_samples = [
                                @"Samples\RESX\Resources.de.xsd_has_name-error.resx";
                                @"Samples\RESX\Resources.de.body_has_name-error.resx";
                                ]

    let resw_samples = [ 
                          @"Samples\RESW\de\Resources.resw";
                          @"Samples\RESW\en\Resources.resw";
                          @"Samples\RESW\es\Resources.resw";
                          @"Samples\RESW\fr\Resources.resw";
                          @"Samples\RESW\hi\Resources.resw";
                          @"Samples\RESW\hu\Resources.resw";
                          @"Samples\RESW\zh-hans\Resources.resw";
                          @"Samples\RESW\zh-hant\Resources.resw";
                          ]

    //let resx_broken_samples = [
    //                            @"Samples\v1.2\OpenPasswordGenerator.ar-SA_broken.xlf";
    //                            ]


    // TEST XLIFF_v1_2_Class construction

    [<Fact>]
    let ``RESX_Class construction with no data`` () =
        try
            let instance = RESX_Class()
            match instance.Problems with
            | None -> Assert.True(true)
            | Some _ -> Assert.True(false)
        with
        | ex -> Assert.True(false, ex.Message)


    // TEST XLIFF_v1_2_Class

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    let ``RESX_Class ReadDontValidateResxe has ERROR`` (index:int) =
        let instance = RESX_Class()
        let action = fun () -> instance.ReadDontValidateResx(resx_error_samples.[index]);
                               null <> instance.Document.Root // if not exception then should have a value - i.e. false
        try
            let result = action()
            Assert.False(result) // supposed to be null when XML doesn't pass verification
        with 
        | ex -> let action_match = ex.Message.Contains(@"NOT FOUND")
                Assert.True(action_match, resx_error_samples.[index] + " passed error case")


    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    [<InlineData(7)>]
    let ``RESX_Class ReadDontValidateResx`` (index:int) =
        let instance = RESX_Class()
        // CHOOSE between definitions of 'action'
        let action = fun () -> instance.ReadDontValidateResx(resx_samples.[index]);
                               null <> instance.Document.Root // if not exception then should have a value - i.e. false
        //let action = fun () -> instance.ReadResxFile(resx_samples.[index]);
        //                       null <> instance.Document.Root
        try
            let result = action()
            Assert.True(result)
        with 
        | ex -> Assert.True(false, ex.Message)


    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    [<InlineData(7)>]
    let ``RESX_Class Write with data`` (index:int) =

        let getRoot (doc: XDocument) : XElement = doc.Root
            
        let getRootName (doc: XDocument) : string = 
            getRoot doc |> fun root -> root.Name.LocalName

        let getElementName (element: XElement) : string = element.Name.LocalName

        let checkElement (pDoc: XDocument) (pNs: XNamespace) (pElementName: string) : option<XElement> =
            // Attempt to find the first offurrence of the specified element
            let elementName = pNs + pElementName

            let maybeElements = pDoc.Descendants(elementName)

            if maybeElements.FirstOrDefault() = null then
                None  // Element does not exist
            else
                let result = 
                        maybeElements
                        |> Seq.head
                Some result


        let checkElementAndAttribute (pDoc: XDocument) (pNs: XNamespace) (pElementName: string) (pAttributeName: string) : option<XAttribute> =
            // Attempt to find the first occurrence of the specified element and its specified attribute

            let elementName = pNs + pElementName

            let maybeElements = pDoc.Descendants(elementName)  
            
            if maybeElements.FirstOrDefault() = null then
                None  // Element does not exist
            else
                let result =
                    maybeElements
                    |> Seq.map (fun element -> 
                                    // Check if the attribute exists on the found element
                                    let attribute = element.Attribute(pAttributeName)
                                    if isNull attribute then
                                        None  // Attribute does not exist
                                    else
                                        Some attribute  // Return the value of the attribute
                                )
                    // return the first value
                    |> Seq.head
                result

        let assemblyLocation = Assembly.GetExecutingAssembly().Location
        let libraryDirectory = System.IO.Path.GetDirectoryName( assemblyLocation)

        let instance = RESX_Class()
        let readAction = fun () -> instance.ReadDontValidateResx(resx_samples.[index])
        readAction()

        let foo_rootName = getRootName instance.Document

        let foo_root_ns = instance.Document.Root.GetDefaultNamespace()
        let foo_name_ns = instance.Document.Root.Name.Namespace

        let foo_allElements = instance.Document.Descendants()

        let foo_firstElement = checkElement instance.Document foo_root_ns "trans-unit"
        let foo_allAttributes = match foo_firstElement with 
                                | None -> Seq.empty
                                | Some elem -> elem.Attributes()

        //// WORKS - look in Output -> Tests
        //foo_allElements
        //|> Seq.iter (fun elem -> 
        //    printfn "Element Name: %s" elem.Name.LocalName
        //    if not (String.IsNullOrWhiteSpace( elem.Value)) then
        //        printfn "Content: %s" elem.Value
        //    )

        let foo_xliff_descendant = checkElementAndAttribute instance.Document foo_name_ns "xliff" "xmlns"
        match foo_xliff_descendant with
        | Some elem -> printfn "xliff: %s" elem.Value
        | None -> printfn "xliff: null"

        let foo_element = checkElementAndAttribute instance.Document foo_name_ns resx_samples.[index] "xmlns"
        let foo_name = getElementName instance.Document.Root

        match foo_element with
        | Some elem -> printfn "tool: %s" elem.Value
        | None -> printfn "tool: null"

        let foo2_element = checkElementAndAttribute instance.Document foo_name_ns resx_samples.[index] "tool-id"
        match foo2_element with
        | Some elem -> printfn "tool-id: %s" elem.Value
        | None -> printfn "tool-id: null"

        let temp = resx_samples
        let output_xml = Path.GetFileName(resx_samples.[index])
        let writeFilePath = Path.Combine( @"C:/Workspace/Projects/XLIFF_FSharp/Test_XLIFF_Library/TEST_OUTPUT/WriteXliffFile_with_data_" + output_xml)
        let writeAction = fun () -> instance.Write(writeFilePath);
        try
            writeAction()
            // test that xliff_samples.[index] and C:\ERASEME\eraseme.index.xml are the same
            let result = System.IO.File.ReadAllText(resx_samples.[index]) = System.IO.File.ReadAllText(writeFilePath)
            Assert.True(result)
        with 
        | ex -> Assert.True(false, ex.Message)


    [<Theory>]
    [<InlineData(0)>]
    let ``RESX_Class Write with no data`` (index:int) =
        let instance = RESX_Class()
        let action = fun () -> instance.Write(resx_samples.[index])
        try
            let result = action()
            Assert.True(false, "Expected an exception when running RESX_Class WriteResxFile with no data")
        with 
        | ex -> Assert.True(true, "RESX_Class with no data is supposed to throw a (" + ex.Message + ") exception.")


    // FIX THESE TESTS

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    [<InlineData(7)>]
    [<InlineData(8)>]
    let ``RESX_Class ReadAndValidateResx with no data`` (index:int) =
        let instance = RESX_Class()
        let action = fun () -> instance.ReadAndValidateResx(resx_samples.[index])
        try
            match action() with 
            | None -> Assert.True(true)
            | Some _ -> Assert.True(false)
        with 
        | ex -> Assert.True(false, ex.Message)


    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    [<InlineData(5)>]
    [<InlineData(6)>]
    [<InlineData(7)>]
    let ``RESX_Class ReadAndValidateResx with data`` (index:int) =
        let instance = RESX_Class()
        let action = fun () -> instance.ReadAndValidateResx(resx_samples.[index])
        try
            match action() with 
            | None -> Assert.True(true)
            | Some _ -> Assert.True(false)
        with 
        | ex -> Assert.True(false, ex.Message)
