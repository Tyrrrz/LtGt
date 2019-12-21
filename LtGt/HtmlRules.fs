namespace LtGt

[<AutoOpen>]
module internal HtmlRules =

    /// List of element names that belong to "raw text elements" category.
    /// These elements can only contain text inside and that text doesn't need to be encoded.
    let rawTextElementNames = [
        "script"
        "style"
    ]

    /// List of element names that belong to "void elements" category.
    /// These elements can't have children and don't need a closing tag.
    let voidElementNames = [
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

    /// Checks whether the specified element name belongs to "raw text elements" category.
    let isRawTextElementName name =
        rawTextElementNames
        |> Seq.exists (String.ordinalEqualsCI name)

    /// Checks whether the specified element name belongs to "void elements" category.
    let isVoidElementName name =
        voidElementNames
        |> Seq.exists (String.ordinalEqualsCI name)