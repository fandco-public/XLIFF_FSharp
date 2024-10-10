module Tests

open System
open Xunit
open XLIFF_Library.XLIFF_v1_2

let xliff_samples = [ 
                      @"Samples\OpenPasswordGenerator.ar-SA.xlf";
                      @"Samples\OpenPasswordGenerator.de-DE.xlf";
                      @"Samples\OpenPasswordGenerator.fr-FR.xlf";
                      @"Samples\OpenPasswordGenerator.gu-IN.xlf";
                      @"Samples\OpenPasswordGenerator.ja-JP.xlf";
                      @"Samples\OpenPasswordGenerator.nl-NL.xlf";
                      @"Samples\OpenPasswordGenerator.ru-RU.xlf";
                      @"Samples\Sample_AlmostEverything_1.2_strict.xlf"; 
                      @"Samples\Sample_AlmostEverything_1.2_transitional.xlf"
                      ]

let executeAndCheckForExceptions action = 
    try
        action()
    with
    | ex -> Assert.True(false, ex.Message)

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
    let action = fun () -> instance.ReadXliffFile(xliff_samples.[index])
    executeAndCheckForExceptions action

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
    let action = fun () -> instance.ReadResxOrReswFile("file")
    executeAndCheckForExceptions action

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
    executeAndCheckForExceptions action

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
    executeAndCheckForExceptions action

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
    executeAndCheckForExceptions action

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
    executeAndCheckForExceptions action




