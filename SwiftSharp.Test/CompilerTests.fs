namespace SwiftSharp.Test

open System
open System.IO
open NUnit.Framework

open SwiftSharp
open SwiftSharp.SwiftCompiler

[<TestFixture>]
type CompilerTests () =

    let compileFile path =
        let config =
            {
                OutputPath = Path.GetFullPath ("TestFiles/" + path + ".dll")
                References =
                    [
                        "/Developer/MonoTouch/usr/lib/mono/2.1/mscorlib.dll"
                        "/Developer/MonoTouch/usr/lib/mono/2.1/monotouch.dll"
                    ]
            }
        compileUrls [Path.GetFullPath ("TestFiles/" + path + ".swift")] config

    [<Test>]
    member x.SODAClient() =
        let a = (compileFile "SODAClient")
        let r = a.GetTypes ()
        let lastType = r.[r.Length - 1]
        Assert.AreEqual ("SODAClient", lastType.Name)
