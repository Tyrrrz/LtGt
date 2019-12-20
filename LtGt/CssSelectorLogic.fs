namespace LtGt

open System
open System.Runtime.CompilerServices

// F# API
[<AutoOpen>]
module CssSelectorLogic =

    let private evaluateAttrOp op pattern value =
        match op with
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

    let rec private evaluateSelector selector (element : HtmlElement) =
        match selector with

        // Primitive selectors

        | Any -> true
        | ByType name -> element |> nameMatches name
        | ByClass className -> element |> classNameMatches className
        | ById id -> element |> idMatches id

        // Attribute selectors

        | ByAttribute name ->
            element
            |> tryAttribute name
            |> Option.isSome

        | ByAttributeValue (name, op, pattern) ->
            element
            |> tryAttributeValue name
            |> Option.exists (evaluateAttrOp op pattern)

        // Hierarchical selectors

        | Root ->
            element.Parent
            |> Option.ofObj
            |> Option.bind tryAsElement
            |> Option.isNone

        | Empty -> element.Children |> Seq.isEmpty

        | OnlyChild ->
            element
            |> siblings
            |> Seq.isEmpty

        | FirstChild ->
            element.Previous
            |> isNull

        | LastChild ->
            element.Next
            |> isNull

        | NthChild formula ->
            element
            |> previousSiblings
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | NthLastChild formula ->
            element
            |> nextSiblings
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | OnlyOfType ->
            element
            |> siblings
            |> filterElements
            |> Seq.filter (nameMatches element.Name)
            |> Seq.isEmpty

        | FirstOfType ->
            element
            |> previousSiblings
            |> filterElements
            |> Seq.filter (nameMatches element.Name)
            |> Seq.isEmpty

        | LastOfType ->
            element
            |> nextSiblings
            |> filterElements
            |> Seq.filter (nameMatches element.Name)
            |> Seq.isEmpty

        | NthOfType formula ->
            element
            |> previousSiblings
            |> filterElements
            |> Seq.filter (nameMatches element.Name)
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | NthLastOfType formula ->
            element
            |> nextSiblings
            |> filterElements
            |> Seq.filter (nameMatches element.Name)
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        // Hierarchical combinators

        | Descendant (ancestorSelector, childSelector) ->
            element
            |> ancestors
            |> Seq.cast
            |> filterElements
            |> Seq.exists (evaluateSelector ancestorSelector)
            && evaluateSelector childSelector element

        | Child (parentSelector, childSelector) ->
            element.Parent
            |> tryAsElement
            |> Option.exists (evaluateSelector parentSelector)
            && evaluateSelector childSelector element

        | Sibling (previousSelector, targetSelector) ->
            element.Previous
            |> tryAsElement
            |> Option.exists (evaluateSelector previousSelector)
            && evaluateSelector targetSelector element

        | SubsequentSibling (previousSelector, targetSelector) ->
            element
            |> previousSiblings
            |> filterElements
            |> Seq.exists (evaluateSelector previousSelector)
            && evaluateSelector targetSelector element

        // Supreme combinators

        | Not selector -> evaluateSelector selector element |> not
        | Group selectors -> selectors |> Seq.forall (fun x -> evaluateSelector x element)

    let queryElements query container =
        let selector = CssSelector.Parse query
        container
        |> descendantElements
        |> Seq.filter (evaluateSelector selector)

// C# API
[<Extension>]
module CssSelectorLogicExtensions =

    [<Extension>]
    let QueryElements (self, query) =
        self
        |> queryElements query