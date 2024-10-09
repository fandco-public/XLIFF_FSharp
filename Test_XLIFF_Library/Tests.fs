module Tests

open System
open Xunit
open XLIFF_Library.XLIFF_v1_2

//[<Fact>]
//let ``My test`` () =
//    Assert.True(true)

[<Fact>]
let ``Hello method should not throw an exception`` () =
    let instance = XLIFF_v1_2_Class()
    let action = fun () -> instance.Hello("World")
    action    

[<Fact>]
let ``ThrowsException method should throw an exception`` () = 
    let instance = XLIFF_v1_2_Class()
    let action = fun () -> instance.ThrowsException()
    let ex : System.Exception = Assert.Throws<System.Exception>(action)
    Assert.Equal("Throws exception!", ex.Message)
