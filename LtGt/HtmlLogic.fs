﻿namespace LtGt

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
    let inline GetId(self) =
        TryGetAttributeValue (self, "id")
        |> Option.toObj

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
        let elementClassNames = GetClassNames (self)

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
                yield! GetDescendants(x)

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
        |> Seq.tryFind (fun x -> String.ordinalEquals (GetId x) id)
        |> Option.toObj

    [<Extension>]
    let inline GetElementsByTagName(self, name) =
        GetDescendantElements (self)
        |> Seq.filter (fun x -> String.ordinalEqualsCI x.Name name)

    [<Extension>]
    let inline GetElementsByClassName(self, className) =
        GetDescendantElements (self)
        |> Seq.filter (fun x -> MatchesClassName(x, className))

    let inline private AppendInnerText (self : HtmlContainer) (buffer : StringBuilder) =
        // TODO: implement
        buffer

    [<Extension>]
    let inline GetInnerText(self) =
        StringBuilder()
        |> AppendInnerText self
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