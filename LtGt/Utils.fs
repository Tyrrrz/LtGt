namespace LtGt

open System

[<AutoOpen>]
module internal Utils =

    let inline (<&>) f g = fun x -> f x && g x

    module String =

        let inline ordinalEquals a b = String.Equals(a, b, StringComparison.Ordinal)

        let inline ordinalEqualsCI a b = String.Equals(a, b, StringComparison.OrdinalIgnoreCase)

        let inline ordinalHash a =
            match a with
            | null -> 0
            | _ -> StringComparer.Ordinal.GetHashCode(a)

        let inline ordinalHashCI a =
            match a with
            | null -> 0
            | _ -> StringComparer.OrdinalIgnoreCase.GetHashCode(a)

        let inline trim (s : string) =
            s.Trim()

        let inline split (c : char) (s : string) =
            s.Split([| c |], StringSplitOptions.RemoveEmptyEntries)

    let inline htmlDecode str = Net.WebUtility.HtmlDecode str