namespace LtGt

[<AutoOpen>]
module internal HtmlRules =

    let rawTextElementNames = [
        "script"
        "style"
    ]

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

    let isRawTextElementName name =
        rawTextElementNames
        |> Seq.exists (String.ordinalEqualsCI name)

    let isVoidElementName name =
        voidElementNames
        |> Seq.exists (String.ordinalEqualsCI name)