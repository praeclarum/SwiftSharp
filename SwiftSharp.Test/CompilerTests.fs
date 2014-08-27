namespace SwiftSharp.Test

open System
open NUnit.Framework

open SwiftSharp
open SwiftSharp.SwiftCompiler

[<TestFixture>]
type CompilerTests () =

    let compileFile path =
        let config =
            {
                InputUrls = ["TestFiles/" + path + ".swift"]
                OutputPath = "TestFiles/" + path + ".dll"
                References =
                    [
                        "/Developer/MonoTouch/usr/lib/mono/2.1/mscorlib.dll"
                        "/Developer/MonoTouch/usr/lib/mono/2.1/monotouch.dll"
                    ]
            }
        compile config

    [<Test>]
    member x.SODAClient() =
        let r = compileFile "SODAClient"
        let lastType = r |> List.rev |> List.head
        Assert.AreEqual ("SODAClient", lastType.Name)
