module Tests

open System
open Xunit
open XLIFF_Library.XLIFF_v1_2

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

//let executeAndCheckForExceptions action : bool = 
//    try
//        action(); true
//    with
//    | ex -> Assert.True(false, ex.Message); false 

// TEST XLIFF_v1_2_Class construction
[<Fact>]
let ``XLIFF_v1_2_Class construction with no data`` () =
    let instance = XLIFF_v1_2_Class()
    Assert.False(instance.ready)

// TEST XLIFF_v1_2_Class
[<Theory>]
[<InlineData(0)>]
[<InlineData(1)>]
[<InlineData(2)>]
[<InlineData(3)>]
[<InlineData(4)>]
[<InlineData(5)>]
[<InlineData(6)>]
[<InlineData(7)>]
let ``XLIFF_v1_2_Class ReadXliffFile`` (index:int) =
    let instance = XLIFF_v1_2_Class()
    let action = fun () -> instance.ReadXliffFile(xliff_samples.[index]);
                           null <> instance.documentForXLIFFv1_2.Root
    try
        let result = action()
        Assert.True(result)
    with 
    | ex -> Assert.True(false, ex.Message)


[<Theory>]
[<InlineData(0)>]
let ``XLIFF_v1_2_Class ReadXliffFile foo element`` (index:int) =
    let instance = XLIFF_v1_2_Class()
    let action = fun () -> instance.ReadXliffFile(xliff_broken_samples.[index]);
                           null <> instance.documentForXLIFFv1_2.Root // if not exception then should have a value - i.e. false
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
let ``XLIFF_v1_2_Class WriteXliffFile with no data`` (index:int) =
    let instance = XLIFF_v1_2_Class()
    let action = fun () -> instance.WriteXliffFile("file")
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
let ``XLIFF_v1_2_Class CheckResxOrReswFile with no data`` (index:int) =
    let instance = XLIFF_v1_2_Class()
    let action = fun () -> instance.CheckResxOrReswFile("file")
    try
        let result = action()
        Assert.True(result)
    with 
    | ex -> Assert.True(false, ex.Message)




