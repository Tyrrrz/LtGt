namespace LtGt

[<AutoOpen>]
module internal HtmlRules =

    /// List of element tag names that belong to "raw text elements" category.
    /// These elements can only contain text inside and that text doesn't need to be encoded.
    let rawTextElementTagNames = [
        "script"
        "style"
    ]

    /// List of element tag names that belong to "void elements" category.
    /// These elements can't have children and don't need a closing tag.
    let voidElementTagNames = [
        "meta"
        "link"
        "img"
        "br"
        "input"
        "hr"
        "area"
        "base"
        "col"
        "embed"
        "param"
        "source"
        "track"
        "wbr"
    ]

    /// Checks whether the specified element tag name belongs to "raw text elements" category.
    let isRawTextElementTagName name = rawTextElementTagNames |> Seq.exists (String.ordinalEqualsCI name)

    /// Checks whether the specified element tag name belongs to "void elements" category.
    let isVoidElementTagName name = voidElementTagNames |> Seq.exists (String.ordinalEqualsCI name)