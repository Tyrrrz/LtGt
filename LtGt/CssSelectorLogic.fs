namespace LtGt

open System.Runtime.CompilerServices

[<AutoOpen; Extension>]
module CssSelectorLogic =

    [<Extension>]
    let QuerySelectorAll (self, query) =
        let selector = CssSelector.Parse query
        GetDescendantElements(self)
        |> Seq.filter selector.Evaluate