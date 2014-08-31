namespace SwiftSharp.Test

open System
open System.IO
open NUnit.Framework

open SwiftSharp
open SwiftSharp.SwiftParser
open SwiftSharp.SwiftCompiler

type BookTests () =

    member this.Test (name : string, code : string) =
        let ast = 
            match parseCode code with
            | Some x -> x
            | _ -> failwith (sprintf "Parse returned nothing for %A" code)
        Assert.Greater (ast.Length, 0)
        let config =
            {
                OutputPath = name + ".exe"
                References = [ typeof<String>.Assembly.Location ]
            }
        let asm = compile [ast] config
        let types = asm.GetTypes ()
        Assert.Greater (types.Length, 0)

        asm.Save (config.OutputPath)
