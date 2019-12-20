namespace LtGt

open System

[<AutoOpen>]
module internal Utils =

    /// Combines two predicates using AND.
    let inline (<&>) f g = fun x -> f x && g x

    module String =

        /// Checks that two strings are equal according to ordinal comparison rules.
        let inline ordinalEquals a b = String.Equals(a, b, StringComparison.Ordinal)

        /// Checks that two strings are equal according to ordinal comparison rules, disregarding differences in case.
        let inline ordinalEqualsCI a b = String.Equals(a, b, StringComparison.OrdinalIgnoreCase)

        /// Calculates hashcode of a string using ordinal comparer.
        let inline ordinalHash s =
            match s with
            | null -> 0
            | _ -> StringComparer.Ordinal.GetHashCode(s)

        /// Calculates hashcode of a string using ordinal comparer which ignores differences in case.
        let inline ordinalHashCI s =
            match s with
            | null -> 0
            | _ -> StringComparer.OrdinalIgnoreCase.GetHashCode(s)

        /// Trims whitespace in a string.
        let inline trim (s : string) =
            s.Trim()

        /// Splits a string using specified char separator.
        let inline split (c : char) (s : string) =
            s.Split([| c |], StringSplitOptions.RemoveEmptyEntries)

    /// Encodes reserved HTML characters in a string.
    let inline htmlEncode s = Net.WebUtility.HtmlEncode s

    /// Decodes reserved HTML characters in a string.
    let inline htmlDecode s = Net.WebUtility.HtmlDecode s