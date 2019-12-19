namespace LtGt

open System
open System.Runtime.CompilerServices

[<AutoOpen; Extension>]
module CssSelectorLogic =

    let private evaluateAttrOp a pattern value =
        match a with
        | Equals -> String.ordinalEqualsCI pattern value
        | StartsWith -> value.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)
        | EndsWith -> value.EndsWith(pattern, StringComparison.OrdinalIgnoreCase)
        | Contains -> value.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0
        | WhiteSpaceSeparatedContains -> value |> String.split ' ' |> Seq.exists (String.ordinalEqualsCI pattern)
        | HyphenSeparatedStartsWith -> value |> String.split '-' |> Seq.tryHead |> Option.exists (String.ordinalEqualsCI pattern)

    let private evaluateFormula f value =
        match f with
        | MultiplierAndConstant (multiplier, constant) -> value % multiplier = constant
        | OnlyMultiplier multiplier -> value % multiplier = 0
        | OnlyConstant constant -> value = constant

    let rec private evaluateSelector s (element : HtmlElement) =
        match s with

        // Primitive selectors

        | Any -> true
        | ByType name -> String.ordinalEqualsCI name element.Name
        | ByClass name -> MatchesClassName (element, name)
        | ById id -> String.ordinalEquals id (GetId element)

        // Attribute selectors

        | ByAttribute name ->
            TryGetAttribute (element, name)
            |> Option.isSome

        | ByAttributeValue (name, op, pattern) ->
            TryGetAttributeValue (element, name)
            |> Option.exists (evaluateAttrOp op pattern)

        // Hierarchical selectors

        | Root -> element.Parent |> Option.ofObj |> Option.bind TryElement |> Option.isNone
        | Empty -> element.Children |> Seq.isEmpty

        | OnlyChild ->
            element
            |> GetSiblings
            |> Seq.isEmpty

        | FirstChild ->
            element.Previous
            |> isNull

        | LastChild ->
            element.Next
            |> isNull

        | NthChild formula ->
            element
            |> GetPreviousSiblings
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | NthLastChild formula ->
            element
            |> GetNextSiblings
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | OnlyOfType ->
            element
            |> GetSiblings
            |> FilterElements
            |> Seq.filter (fun x -> String.ordinalEqualsCI x.Name element.Name)
            |> Seq.isEmpty

        | FirstOfType ->
            element
            |> GetPreviousSiblings
            |> FilterElements
            |> Seq.filter (fun x -> String.ordinalEqualsCI x.Name element.Name)
            |> Seq.isEmpty

        | LastOfType ->
            element
            |> GetNextSiblings
            |> FilterElements
            |> Seq.filter (fun x -> String.ordinalEqualsCI x.Name element.Name)
            |> Seq.isEmpty

        | NthOfType formula ->
            element
            |> GetPreviousSiblings
            |> FilterElements
            |> Seq.filter (fun x -> String.ordinalEqualsCI x.Name element.Name)
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | NthLastOfType formula ->
            element
            |> GetNextSiblings
            |> FilterElements
            |> Seq.filter (fun x -> String.ordinalEqualsCI x.Name element.Name)
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        // Hierarchical combinators

        | Descendant (ancestorSelector, childSelector) ->
            GetAncestors element
            |> Seq.cast
            |> FilterElements
            |> Seq.exists (evaluateSelector ancestorSelector)
            && evaluateSelector childSelector element

        | Child (parentSelector, childSelector) ->
            element.Parent
            |> TryElement
            |> Option.exists (evaluateSelector parentSelector)
            && evaluateSelector childSelector element

        | Sibling (previousSelector, targetSelector) ->
            element.Previous
            |> TryElement
            |> Option.exists (evaluateSelector previousSelector)
            && evaluateSelector targetSelector element

        | SubsequentSibling (previousSelector, targetSelector) ->
            element
            |> GetPreviousSiblings
            |> FilterElements
            |> Seq.exists (evaluateSelector previousSelector)
            && evaluateSelector targetSelector element

        // Supreme combinators

        | Not selector -> evaluateSelector selector element |> not
        | Group selectors -> selectors |> Seq.forall (fun x -> evaluateSelector x element)

    [<Extension>]
    let QuerySelectorAll (self, query) =
        let selector = CssSelector.Parse query
        GetDescendantElements(self)
        |> Seq.filter (evaluateSelector selector)