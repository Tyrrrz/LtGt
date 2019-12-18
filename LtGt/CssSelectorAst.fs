namespace LtGt

open System

type internal AttributeValueEqualityOperator =
    | Equals
    | StartsWith
    | EndsWith
    | Contains
    | WhiteSpaceSeparatedContains
    | HyphenSeparatedStartsWith

    member self.Evaluate (pattern : string) (value : string) =
        match self with
        | Equals -> value.Equals(pattern, StringComparison.OrdinalIgnoreCase)
        | StartsWith -> value.StartsWith(pattern, StringComparison.OrdinalIgnoreCase)
        | EndsWith -> value.EndsWith(pattern, StringComparison.OrdinalIgnoreCase)
        | Contains -> value.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) >= 0
        | WhiteSpaceSeparatedContains -> value.Split(' ') |> Seq.exists (fun x -> x.Equals(pattern, StringComparison.OrdinalIgnoreCase))
        | HyphenSeparatedStartsWith -> value.Split('-') |> Seq.tryHead |> Option.exists (fun x -> x.Equals(pattern, StringComparison.OrdinalIgnoreCase))

type internal NumberFormula =
    | MultiplierAndConstant of int * int
    | OnlyMultiplier of int
    | OnlyConstant of int

    member self.Evaluate (value : int) =
        match self with
        | MultiplierAndConstant (multiplier, constant) -> value % multiplier = constant
        | OnlyMultiplier multiplier -> value % multiplier = 0
        | OnlyConstant constant -> value = constant

type internal Selector =
    | None
    | Any
    | ByType of string
    | ByClass of string
    | ById of string
    | ByAttribute of string
    | ByAttributeValue of string * AttributeValueEqualityOperator * string
    | Root
    | Empty
    | OnlyChild
    | FirstChild
    | LastChild
    | NthChild of NumberFormula
    | NthLastChild of NumberFormula
    | OnlyOfType
    | FirstOfType
    | LastOfType
    | NthOfType of NumberFormula
    | NthLastOfType of NumberFormula
    | Descendant of Selector * Selector
    | Child of Selector * Selector
    | Sibling of Selector * Selector
    | SubsequentSibling of Selector * Selector
    | Not of Selector
    | Group of Selector list

    member self.Evaluate (element : HtmlElement) =
        match self with

        // Primitive selectors
        | None -> false
        | Any -> true

        | ByType name -> ordinalEqualsCI name element.Name
        | ByClass name -> ordinalEqualsCI name (GetClassName element)
        | ById id -> ordinalEquals id (GetId element)

        // Attribute selectors
        | ByAttribute name ->
            TryGetAttribute (element, name)
            |> Option.isSome

        | ByAttributeValue (name, op, pattern) ->
            TryGetAttributeValue (element, name)
            |> Option.exists (fun x -> op.Evaluate pattern x)

        // Hierarchical selectors
        | Root -> element.Parent |> isNull
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
            |> formula.Evaluate

        | NthLastChild formula ->
            element
            |> GetNextSiblings
            |> Seq.length
            |> fun x -> x + 1
            |> formula.Evaluate

        | OnlyOfType ->
            element
            |> GetSiblings
            |> FilterElements
            |> Seq.filter (fun x -> ordinalEqualsCI x.Name element.Name)
            |> Seq.isEmpty

        | FirstOfType ->
            element
            |> GetPreviousSiblings
            |> FilterElements
            |> Seq.filter (fun x -> ordinalEqualsCI x.Name element.Name)
            |> Seq.isEmpty

        | LastOfType ->
            element
            |> GetNextSiblings
            |> FilterElements
            |> Seq.filter (fun x -> ordinalEqualsCI x.Name element.Name)
            |> Seq.isEmpty

        | NthOfType formula ->
            element
            |> GetPreviousSiblings
            |> FilterElements
            |> Seq.filter (fun x -> ordinalEqualsCI x.Name element.Name)
            |> Seq.length
            |> fun x -> x + 1
            |> formula.Evaluate

        | NthLastOfType formula ->
            element
            |> GetNextSiblings
            |> FilterElements
            |> Seq.filter (fun x -> ordinalEqualsCI x.Name element.Name)
            |> Seq.length
            |> fun x -> x + 1
            |> formula.Evaluate

        // Hierarchical combinators
        | Descendant (ancestorSelector, childSelector) ->
            GetAncestors element
            |> Seq.cast
            |> FilterElements
            |> Seq.exists (fun x -> ancestorSelector.Evaluate x)
            && childSelector.Evaluate element

        | Child (parentSelector, childSelector) ->
            element.Parent
            |> TryElement
            |> Option.exists (fun x -> parentSelector.Evaluate x)
            && childSelector.Evaluate element

        | Sibling (previousSelector, targetSelector) ->
            element.Previous
            |> TryElement
            |> Option.exists (fun x -> previousSelector.Evaluate x)
            && targetSelector.Evaluate element

        | SubsequentSibling (previousSelector, targetSelector) ->
            element
            |> GetPreviousSiblings
            |> FilterElements
            |> Seq.exists (fun x -> previousSelector.Evaluate x)
            && targetSelector.Evaluate element

        // Supreme combinators
        | Not selector -> selector.Evaluate element |> not
        | Group selectors -> selectors |> Seq.forall (fun x -> x.Evaluate element)