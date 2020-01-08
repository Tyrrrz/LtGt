namespace LtGt

open System
open System.Text

[<AutoOpen>]
module internal Utils =

    /// Combines two predicates using AND.
    let inline (<&>) f g = fun x -> f x && g x

    module String =

        /// Checks that two strings are equal according to ordinal comparison rules.
        let ordinalEquals a b = String.Equals(a, b, StringComparison.Ordinal)

        /// Checks that two strings are equal according to ordinal comparison rules, disregarding differences in case.
        let ordinalEqualsCI a b = String.Equals(a, b, StringComparison.OrdinalIgnoreCase)

        /// Calculates hashcode of a string using ordinal comparer.
        let ordinalHash s =
            match s with
            | null -> 0
            | _ -> StringComparer.Ordinal.GetHashCode(s)

        /// Calculates hashcode of a string using ordinal comparer which ignores differences in case.
        let ordinalHashCI s =
            match s with
            | null -> 0
            | _ -> StringComparer.OrdinalIgnoreCase.GetHashCode(s)

        /// Checks that first string starts with second string according to ordinal comparison rules, disregarding differences in case.
        let ordinalStartsWithCI (a : string) (b : string) = a.StartsWith(b, StringComparison.OrdinalIgnoreCase)

        /// Checks that first string ends with second string according to ordinal comparison rules, disregarding differences in case.
        let ordinalEndsWithCI (a : string) (b : string) = a.EndsWith(b, StringComparison.OrdinalIgnoreCase)

        /// Checks that first string contains second string according to ordinal comparison rules, disregarding differences in case.
        let ordinalContainsCI (a : string) (b : string) = a.IndexOf(b, StringComparison.OrdinalIgnoreCase) >= 0

        /// Trims whitespace in a string.
        let trim (s : string) =
            s.Trim()

        /// Splits a string using specified char separator.
        let split (c : char) (s : string) =
            s.Split([| c |], StringSplitOptions.RemoveEmptyEntries)

    /// Encodes reserved HTML characters in a string.
    let htmlEncode = Net.WebUtility.HtmlEncode

    /// Decodes reserved HTML characters in a string.
    let htmlDecode = Net.WebUtility.HtmlDecode

    type StringBuilder with
        member self.AppendLineIndented depth = self.AppendLine().Append(' ', depth * 2)