namespace LtGt

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Text

// F# API
[<AutoOpen>]
module HtmlLogic =

    let rec ancestors (node : HtmlNode) = seq {
        if not (isNull node.Parent) then
            yield node.Parent
            yield! node.Parent |> ancestors
    }

    let siblings (node : HtmlNode) = seq {
        if not (isNull node.Parent) then
            yield! node.Parent.Children |> Seq.filter (fun x -> x <> node)
    }

    let rec previousSiblings (node : HtmlNode) = seq {
        if not (isNull node.Previous) then
            yield node.Previous
            yield! node.Previous |> previousSiblings
    }

    let rec nextSiblings (node : HtmlNode) = seq {
        if not (isNull node.Next) then
            yield node.Next
            yield! node.Next |> nextSiblings
    }

    let tryAttribute name (element : HtmlElement) =
        element.Attributes
        |> Seq.tryFind (fun x -> String.ordinalEqualsCI x.Name name)

    let tryAttributeValue name element =
        element
        |> tryAttribute name
        |> Option.map (fun x -> x.Value)

    let tryAsElement (node : HtmlNode) =
        match node with
        | :? HtmlElement as x -> Some x
        | _ -> None

    let filterElements (nodes : HtmlNode seq) =
        nodes
        |> Seq.filter (fun x -> x :? HtmlElement)
        |> Seq.cast<HtmlElement>

    let nameMatches name (element : HtmlElement) =
        String.ordinalEqualsCI element.Name name

    let tryId element =
        element
        |> tryAttributeValue "id"

    let idMatches id element =
        element
        |> tryId
        |> Option.exists (String.ordinalEquals id)

    let tryClassName element =
        element
        |> tryAttributeValue "class"

    let classNames element =
        element
        |> tryClassName
        |> Option.map (String.split ' ')
        |> Option.defaultValue Array.empty

    let classNameMatches className element =
        let targetClassNames = className |> String.split ' '
        let sourceClassNames = element |> classNames

        targetClassNames
        |> Seq.forall (fun x -> sourceClassNames |> Seq.contains x)

    let rec private appendInnerText (node : HtmlNode) (buffer : StringBuilder) =
        let isEmpty element =
            nameMatches "script" element ||
            nameMatches "style" element ||
            nameMatches "select" element ||
            nameMatches "canvas" element ||
            nameMatches "video" element ||
            nameMatches "iframe" element

        let shouldPrependLine element =
            buffer.Length > 0 && (
                nameMatches "p" element ||
                nameMatches "caption" element ||
                nameMatches "div" element ||
                nameMatches "li" element)

        match node with
        | :? HtmlText as a -> buffer.Append a.Value
        | :? HtmlElement as a ->
            if a |> nameMatches "br" then
                buffer.AppendLine()
            else
                if shouldPrependLine a then
                    do buffer.AppendLine() |> ignore

                if isEmpty a then
                    buffer
                else
                    a.Children |> Seq.iter (fun x -> appendInnerText x buffer |> ignore)
                    buffer
        | _ -> buffer

    let innerText container =
        StringBuilder()
        |> appendInnerText container
        |> string

    let rec private appendHtml (entity : HtmlEntity) depth (buffer : StringBuilder) =
        let appendLine d (b : StringBuilder) = b.AppendLine().Append(' ', d * 2)

        match entity with
        | :? HtmlDeclaration as x ->
            buffer.Append (sprintf "<!%s>" x.Value)

        | :? HtmlAttribute as x ->
            if String.IsNullOrWhiteSpace x.Value then
                buffer.Append x.Name
            else
                buffer.Append (sprintf "%s=\"%s\"" x.Name (htmlEncode x.Value))

        | :? HtmlText as x ->
            if x.Parent |> tryAsElement |> Option.exists (fun a -> isRawTextElementName a.Name) then
                buffer.Append x.Value
            else
                buffer.Append (htmlEncode x.Value)

        | :? HtmlComment as x ->
            buffer.Append (sprintf "<!-- %s -->" x.Value)

        | :? HtmlElement as x ->
            do buffer.Append('<').Append(x.Name) |> ignore

            for attribute in x.Attributes do
                buffer.Append ' ' |> appendHtml attribute depth |> ignore

            do buffer.Append '>' |> ignore

            if (isVoidElementName x.Name && x.Children |> Seq.isEmpty) |> not then

                let innerDepth = depth + 1

                for child in x.Children do
                    appendLine innerDepth buffer |> appendHtml child innerDepth |> ignore

                do (appendLine depth buffer).Append("</").Append(x.Name).Append('>') |> ignore

            buffer

        | :? HtmlDocument as x ->
            do buffer |> appendHtml x.Declaration depth |> ignore
            do buffer.AppendLine() |> ignore

            for child in x.Children do
                do buffer |> appendHtml child depth |> ignore
                do buffer.AppendLine() |> ignore

            buffer

        | _ -> buffer

    let toHtml entity =
        StringBuilder()
        |> appendHtml entity 0
        |> string

    let tryHref element =
        element
        |> tryAttributeValue "href"

    let trySrc element =
        element
        |> tryAttributeValue "src"

    let rec descendants (container : HtmlContainer) = seq {
        for child in container.Children do
            match child with

            | :? HtmlContainer as x ->
                yield child
                yield! descendants x

            | _ ->
                yield child
    }

    let descendantElements container =
        container
        |> descendants
        |> filterElements

    let tryElementById id container =
        container
        |> descendantElements
        |> Seq.tryFind (idMatches id)

    let elementsByName name container =
        container
        |> descendantElements
        |> Seq.filter (nameMatches name)

    let elementsByClassName className container =
        container
        |> descendantElements
        |> Seq.filter (classNameMatches className)

    let tryHead document =
        document
        |> elementsByName "head"
        |> Seq.tryHead

    let tryTitle document =
        document
        |> tryHead
        |> Option.map innerText

    let tryBody document =
        document
        |> elementsByName "body"
        |> Seq.tryHead

// C# API
[<Extension>]
module HtmlLogicExtensions =

    [<Extension>]
    let GetAncestors (self : HtmlNode) =
        self
        |> ancestors

    [<Extension>]
    let GetSiblings (self : HtmlNode) =
        self
        |> siblings

    [<Extension>]
    let GetPreviousSiblings (self : HtmlNode) =
        self
        |> previousSiblings

    [<Extension>]
    let GetNextSiblings (self : HtmlNode) =
        self
        |> nextSiblings

    [<Extension>]
    let GetAttribute (self, name) =
        self
        |> tryAttribute name
        |> Option.toObj

    [<Extension>]
    let MatchesName (self, name) =
        self
        |> nameMatches name

    [<Extension>]
    let GetId self =
        self
        |> tryId
        |> Option.toObj

    [<Extension>]
    let MatchesId (self, id) =
        self
        |> idMatches id

    [<Extension>]
    let GetClassName self =
        self
        |> tryClassName
        |> Option.toObj

    [<Extension>]
    let GetClassNames self =
        self
        |> classNames
        :> IReadOnlyList<string>

    [<Extension>]
    let MatchesClassName (self, className) =
        self
        |> classNameMatches className

    [<Extension>]
    let GetHref self =
        self
        |> tryHref
        |> Option.toObj

    [<Extension>]
    let GetSrc self =
        self
        |> trySrc
        |> Option.toObj

    [<Extension>]
    let rec GetDescendants self =
        self
        |> descendants

    [<Extension>]
    let GetDescendantElements self =
        self
        |> descendantElements

    [<Extension>]
    let GetElementById (self, id) =
        self
        |> tryElementById id
        |> Option.toObj

    [<Extension>]
    let GetElementsByTagName (self, name) =
        self
        |> elementsByName name

    [<Extension>]
    let GetElementsByClassName (self, className) =
        self
        |> elementsByClassName className

    [<Extension>]
    let GetInnerText self =
        self
        |> innerText

    [<Extension>]
    let GetHead self =
        self
        |> tryHead
        |> Option.toObj

    [<Extension>]
    let GetBody self =
        self
        |> tryBody
        |> Option.toObj

    [<Extension>]
    let GetTitle self =
        self
        |> tryTitle
        |> Option.toObj

    [<Extension>]
    let ToHtml self =
        self
        |> toHtml