open System.Reflection
open BenchmarkDotNet.Running

[<EntryPoint>]
let main args =
    BenchmarkRunner.Run(Assembly.GetExecutingAssembly()) |> ignore
    0