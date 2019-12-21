namespace LtGt

type internal AttributeValueEqualityOperator =
    | Equals
    | StartsWith
    | EndsWith
    | Contains
    | WhiteSpaceSeparatedContains
    | HyphenSeparatedStartsWith

type internal NumberFormula =
    | MultiplierAndConstant of int * int
    | OnlyMultiplier of int
    | OnlyConstant of int

type internal Selector =
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