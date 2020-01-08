namespace LtGt

open System.Runtime.CompilerServices

module CssSelector =

    let private evaluateAttrOp op pattern value =
        match op with
        | Equals -> String.ordinalEqualsCI value pattern
        | StartsWith -> String.ordinalStartsWithCI value pattern
        | EndsWith -> String.ordinalEndsWithCI value pattern
        | Contains -> String.ordinalContainsCI value pattern

        | WhiteSpaceSeparatedContains ->
            value
            |> String.split ' '
            |> Array.exists (String.ordinalEqualsCI pattern)

        | HyphenSeparatedStartsWith ->
            value
            |> String.split '-'
            |> Array.tryHead
            |> Option.exists (String.ordinalEqualsCI pattern)

    let private evaluateFormula f value =
        match f with
        | MultiplierAndConstant (multiplier, constant) -> value % multiplier = constant
        | OnlyMultiplier multiplier -> value % multiplier = 0
        | OnlyConstant constant -> value = constant

    let rec private evaluateSelector selector (element : HtmlElement) =
        match selector with
        | Any -> true
        | ByTagName name -> element |> Html.tagNameMatches name
        | ById id -> element |> Html.idMatches id
        | ByClassName className -> element |> Html.classNameMatches className

        | ByAttribute name ->
            element
            |> Html.tryAttribute name
            |> Option.isSome

        | ByAttributeValue (name, op, pattern) ->
            element
            |> Html.tryAttributeValue name
            |> Option.exists (evaluateAttrOp op pattern)

        | Root ->
            element.Parent
            |> Option.ofObj
            |> Option.bind Html.tryAsElement
            |> Option.isNone

        | Empty -> element.Children |> Seq.isEmpty
        | OnlyChild -> element |> Html.siblings |> Seq.isEmpty
        | FirstChild -> isNull <| element.Previous
        | LastChild -> isNull <| element.Next

        | NthChild formula ->
            element
            |> Html.previousSiblings
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | NthLastChild formula ->
            element
            |> Html.nextSiblings
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | OnlyOfType ->
            element
            |> Html.siblings
            |> Html.filterElements
            |> Seq.filter (Html.tagNameMatches element.TagName)
            |> Seq.isEmpty

        | FirstOfType ->
            element
            |> Html.previousSiblings
            |> Html.filterElements
            |> Seq.filter (Html.tagNameMatches element.TagName)
            |> Seq.isEmpty

        | LastOfType ->
            element
            |> Html.nextSiblings
            |> Html.filterElements
            |> Seq.filter (Html.tagNameMatches element.TagName)
            |> Seq.isEmpty

        | NthOfType formula ->
            element
            |> Html.previousSiblings
            |> Html.filterElements
            |> Seq.filter (Html.tagNameMatches element.TagName)
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | NthLastOfType formula ->
            element
            |> Html.nextSiblings
            |> Html.filterElements
            |> Seq.filter (Html.tagNameMatches element.TagName)
            |> Seq.length
            |> fun x -> x + 1
            |> evaluateFormula formula

        | Descendant (ancestorSelector, childSelector) ->
            element
            |> Html.ancestors
            |> Seq.cast
            |> Html.filterElements
            |> Seq.exists (evaluateSelector ancestorSelector)
            && evaluateSelector childSelector element

        | Child (parentSelector, childSelector) ->
            element.Parent
            |> Html.tryAsElement
            |> Option.exists (evaluateSelector parentSelector)
            && evaluateSelector childSelector element

        | Sibling (previousSelector, targetSelector) ->
            element.Previous
            |> Html.tryAsElement
            |> Option.exists (evaluateSelector previousSelector)
            && evaluateSelector targetSelector element

        | SubsequentSibling (previousSelector, targetSelector) ->
            element
            |> Html.previousSiblings
            |> Html.filterElements
            |> Seq.exists (evaluateSelector previousSelector)
            && evaluateSelector targetSelector element

        | Not selector -> not <| evaluateSelector selector element
        | Group selectors -> selectors |> Seq.forall (fun x -> evaluateSelector x element)

    /// Gets all descendant elements that are matched by the specified CSS selector.
    let queryElements query container =
        // This never fails, just returns nothing in case of an error
        match ParsingUtils.runWithResult CssSelectorGrammar.selectorFull query with
        | Ok selector ->
            container
            |> Html.descendantElements
            |> Seq.filter (evaluateSelector selector)
        | Error _ -> Seq.empty

// Extensions for usage from C#
[<Extension>]
module CssSelectorExtensions =

    /// Gets all descendant elements that are matched by the specified CSS selector.
    [<Extension>]
    let QueryElements (self, query) = self |> CssSelector.queryElements query