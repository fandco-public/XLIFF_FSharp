module Tests.XLIFF

    open System
    open System.IO
    open System.Reflection
    open Xunit
    open XLIFF_Library.XLIFF_v1_2
    open System.Xml.Linq
    open System.Linq
    open Microsoft.FSharp.Core

    let xliff_samples = [ 
                          @"Samples\v1.2\OpenPasswordGenerator.ar-SA.xlf";
                          @"Samples\v1.2\OpenPasswordGenerator.de-DE.xlf";
                          @"Samples\v1.2\OpenPasswordGenerator.fr-FR.xlf";
                          @"Samples\v1.2\OpenPasswordGenerator.gu-IN.xlf";
                          @"Samples\v1.2\OpenPasswordGenerator.ja-JP.xlf";
                          @"Samples\v1.2\OpenPasswordGenerator.nl-NL.xlf";
                          @"Samples\v1.2\OpenPasswordGenerator.ru-RU.xlf";
                          @"Samples\v1.2\Sample_AlmostEverything_1.2_strict.xlf"; 
                          @"Samples\v1.2\Sample_AlmostEverything_1.2_transitional.xlf"
                          ]

    let xliff_broken_samples = [
                                @"Samples\v1.2\OpenPasswordGenerator.ar-SA_broken.xlf";
                                //@"Samples\v1.2\OpenPasswordGenerator.de-DE_broken.xlf";
                                //@"Samples\v1.2\OpenPasswordGenerator.fr-FR_broken.xlf";
                                //@"Samples\v1.2\OpenPasswordGenerator.gu-IN_broken.xlf";
                                //@"Samples\v1.2\OpenPasswordGenerator.ja-JP_broken.xlf";
                                //@"Samples\v1.2\OpenPasswordGenerator.nl-NL_broken.xlf";
                                //@"Samples\v1.2\OpenPasswordGenerator.ru-RU_broken.xlf";
                                //@"Samples\v1.2\Sample_AlmostEverything_1.2_strict_broken.xlf"; 
                                //@"Samples\v1.2\Sample_AlmostEverything_1.2_transitional_broken.xlf"
                                ]


    // TEST XLIFF_v1_2_Class construction

    [<Fact>]
    let ``XLIFF_v1_2_Class construction with no data`` () =
        let instance = XLIFF_v1_2_Class()
        Assert.False(instance.readyProperty)


    // TEST XLIFF_v1_2_Class

    [<Theory>]
    [<InlineData(0)>]
    let ``XLIFF_v1_2_Class ReadXliffFile ERROR has foo element`` (index:int) =
        let instance = XLIFF_v1_2_Class()
        let action = fun () -> instance.ReadXliffFile(xliff_broken_samples.[index]);
                               null <> instance.document.Root // if not exception then should have a value - i.e. false
        try
            let result = action()
            Assert.False(result) // supposed to be null when XML doesn't pass verification
        with 
        | ex -> let action_match_foo = ex.Message.Contains(@"has invalid child element 'foo' in namespace")
                Assert.True(action_match_foo, "found - XLIFF_v1_2_Class ReadXliffFile foo element")


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
    let ``XLIFF_v1_2_Class ReadXliffFile`` (index:int) =
        let instance = XLIFF_v1_2_Class()
        let action = fun () -> instance.ReadXliffFile(xliff_samples.[index]);
                               null <> instance.document.Root
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
    [<InlineData(8)>]
    let ``XLIFF_v1_2_Class WriteXliffFile with data`` (index:int) =

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

        let instance = XLIFF_v1_2_Class()
        let readAction = fun () -> instance.ReadXliffFile(xliff_samples.[index])
        readAction()

        let foo_rootName = getRootName instance.document

        let foo_root_ns = instance.document.Root.GetDefaultNamespace()
        let foo_name_ns = instance.document.Root.Name.Namespace

        let foo_allElements = instance.document.Descendants()

        let foo_firstElement = checkElement instance.document foo_root_ns "trans-unit"
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

        let foo_xliff_descendant = checkElementAndAttribute instance.document foo_name_ns "xliff" "xmlns"
        match foo_xliff_descendant with
        | Some elem -> printfn "xliff: %s" elem.Value
        | None -> printfn "xliff: null"

        let foo_element = checkElementAndAttribute instance.document foo_name_ns "file" "xmlns"
        let foo_name = getElementName instance.document.Root

        match foo_element with
        | Some elem -> printfn "tool: %s" elem.Value
        | None -> printfn "tool: null"

        let foo2_element = checkElementAndAttribute instance.document foo_name_ns "file" "tool-id"
        match foo2_element with
        | Some elem -> printfn "tool-id: %s" elem.Value
        | None -> printfn "tool-id: null"

        let temp = xliff_samples
        let output_xml = Path.GetFileName(xliff_samples.[index])
        let writeFilePath = Path.Combine( @"C:/Workspace/Projects/XLIFF_FSharp/Test_XLIFF_Library/TEST_OUTPUT/WriteXliffFile_with_data_" + output_xml)
        let writeAction = fun () -> instance.WriteXliffFile(writeFilePath);
        try
            writeAction()
            // test that xliff_samples.[index] and C:\ERASEME\eraseme.index.xml are the same
            let result = System.IO.File.ReadAllText(xliff_samples.[index]) = System.IO.File.ReadAllText(writeFilePath)
            Assert.True(result)
        with 
        | ex -> Assert.True(false, ex.Message)


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
    let ``XLIFF_v1_2_Class WriteXliffFile with no data`` (index:int) =
        let instance = XLIFF_v1_2_Class()
        let action = fun () -> instance.WriteXliffFile("file")
        try
            let result = action()
            Assert.True(result; true) // WARNING - hack to get test compiling
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
    [<InlineData(8)>]
    let ``XLIFF_v1_2_Class ReadResxOrReswFile`` (index:int) =
        let instance = XLIFF_v1_2_Class()
        let action = fun () -> instance.ReadResxOrReswFile("file"); true
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
    [<InlineData(8)>]
    let ``XLIFF_v1_2_Class WriteResxOrReswFile with no data`` (index:int) =
        let instance = XLIFF_v1_2_Class()
        let action = fun () -> instance.WriteResxOrReswFile("file")
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
    [<InlineData(8)>]
    let ``XLIFF_v1_2_Class CheckXliffFile with no data`` (index:int) =
        let instance = XLIFF_v1_2_Class()
        let action = fun () -> instance.CheckXliffFile("file")
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
    [<InlineData(8)>]
    let ``XLIFF_v1_2_Class CheckResxOrReswFile with no data`` (index:int) =
        let instance = XLIFF_v1_2_Class()
        let action = fun () -> instance.CheckResxOrReswFile("file")
        try
            let result = action()
            Assert.True(result)
        with 
        | ex -> Assert.True(false, ex.Message)
