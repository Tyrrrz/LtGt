namespace LtGt

open System.Runtime.CompilerServices

[<AutoOpen; Extension>]
module CssSelectorLogic =

    [<Extension>]
    let QuerySelectorAll (self : HtmlContainer, query : string) =
        if (ordinalEquals query "*") then
            GetDescendantElements(self)
        else
            let selector = CssSelector.Parse query
            GetDescendantElements(self)
            |> Seq.filter selector.Evaluate