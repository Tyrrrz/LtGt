namespace LtGt

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Text

[<AutoOpen; Extension>]
module HtmlLogic =

    let inline internal TryElement (self : HtmlNode) =
        match self with
        | :? HtmlElement as x -> Some x
        | _ -> None

    let inline internal TryGetAttribute (self : HtmlElement, name) =
        self.Attributes
        |> Seq.tryFind (fun x -> String.ordinalEqualsCI x.Name name)

    let inline internal TryGetAttributeValue (self, name) =
        TryGetAttribute (self, name)
        |> Option.map (fun x -> x.Value)

    let inline internal FilterElements (self : HtmlNode seq) =
        self
        |> Seq.filter (fun x -> x :? HtmlElement)
        |> Seq.cast<HtmlElement>

    [<Extension>]
    let inline GetAttribute (self, name) =
        TryGetAttribute (self, name)
        |> Option.toObj

    [<Extension>]
    let inline MatchesName(self : HtmlElement, name) =
        String.ordinalEqualsCI self.Name name

    [<Extension>]
    let inline GetId(self) =
        TryGetAttributeValue (self, "id")
        |> Option.toObj

    [<Extension>]
    let inline MatchesId(self : HtmlElement, id) =
        String.ordinalEquals (GetId self) id

    [<Extension>]
    let inline GetClassName(self) =
        TryGetAttributeValue (self, "class")
        |> Option.toObj

    [<Extension>]
    let inline GetClassNames(self) =
        TryGetAttributeValue (self, "class")
        |> Option.map (String.split ' ')
        |> Option.defaultValue Array.empty
        :> IReadOnlyList<string>

    [<Extension>]
    let inline MatchesClassName(self, className) =
        let classNames = className |> String.split ' '
        let elementClassNames = GetClassNames self

        classNames
        |> Seq.forall (fun x -> elementClassNames |> Seq.contains x)

    [<Extension>]
    let inline GetHref(self) =
        TryGetAttributeValue (self, "href")
        |> Option.toObj

    [<Extension>]
    let inline GetSrc(self) =
        TryGetAttributeValue (self, "src")
        |> Option.toObj

    [<Extension>]
    let rec GetDescendants(self : HtmlContainer) = seq {
        for child in self.Children do
            match child with

            | :? HtmlContainer as x ->
                yield child
                yield! GetDescendants x

            | _ ->
                yield child
    }

    [<Extension>]
    let inline GetDescendantElements(self) =
        GetDescendants (self)
        |> FilterElements

    [<Extension>]
    let inline GetElementById(self : HtmlContainer, id) =
        GetDescendantElements (self)
        |> Seq.tryFind (fun x -> MatchesId(x, id))
        |> Option.toObj

    [<Extension>]
    let inline GetElementsByTagName(self, name) =
        GetDescendantElements (self)
        |> Seq.filter (fun x -> MatchesName(x, name))

    [<Extension>]
    let inline GetElementsByClassName(self, className) =
        GetDescendantElements (self)
        |> Seq.filter (fun x -> MatchesClassName(x, className))

    let rec private appendInnerText (node : HtmlNode) (buffer : StringBuilder) =
        let isEmpty (element:HtmlElement) =
            MatchesName (element, "script") ||
            MatchesName (element, "style") ||
            MatchesName (element, "select") ||
            MatchesName (element, "canvas") ||
            MatchesName (element, "video") ||
            MatchesName (element, "iframe")

        let shouldPrependLine (element:HtmlElement) =
            buffer.Length > 0 && (
                MatchesName (element, "p") ||
                MatchesName (element, "caption") ||
                MatchesName (element, "div") ||
                MatchesName (element, "li"))

        match node with
        | :? HtmlText as a -> buffer.Append a.Value
        | :? HtmlElement as a ->
            if MatchesName (a, "br") then
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

    [<Extension>]
    let GetInnerText(self) =
        StringBuilder()
        |> appendInnerText self
        |> string

    [<Extension>]
    let inline GetHead(self) =
        GetElementsByTagName (self, "head")
        |> Seq.tryHead
        |> Option.toObj

    [<Extension>]
    let inline GetBody(self) =
        GetElementsByTagName (self, "body")
        |> Seq.tryHead
        |> Option.toObj

    [<Extension>]
    let inline GetTitle(self) =
        GetElementsByTagName (self, "head")
        |> Seq.tryHead
        |> Option.map GetInnerText
        |> Option.toObj

    [<Extension>]
    let rec GetAncestors(self : HtmlNode) = seq {
        if not (isNull self.Parent) then
            yield self.Parent
            yield! GetAncestors(self.Parent)
    }

    [<Extension>]
    let inline GetSiblings(self : HtmlNode) = seq {
        if not (isNull self.Parent) then
            yield! self.Parent.Children |> Seq.filter (fun x -> x <> self)
    }

    [<Extension>]
    let rec GetPreviousSiblings(self : HtmlNode) = seq {
        if not (isNull self.Previous) then
            yield self.Previous
            yield! GetPreviousSiblings(self.Previous)
    }

    [<Extension>]
    let rec GetNextSiblings(self : HtmlNode) = seq {
        if not (isNull self.Next) then
            yield self.Next
            yield! GetNextSiblings(self.Next)
    }

    let rec appendHtml (entity : HtmlEntity) depth (buffer : StringBuilder) =
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
            if TryElement x.Parent |> Option.exists (fun a -> isRawTextElementName a.Name) then
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

    [<Extension>]
    let ToHtml(self : HtmlEntity) =
        StringBuilder()
        |> appendHtml self 0
        |> string