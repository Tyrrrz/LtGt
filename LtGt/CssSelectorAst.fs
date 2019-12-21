namespace LtGt

type internal AttributeValueOperator =
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
    | ByName of string
    | ById of string
    | ByClassName of string
    | ByAttribute of string
    | ByAttributeValue of string * AttributeValueOperator * string
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