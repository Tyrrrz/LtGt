namespace LtGt

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Text

// F# API
[<AutoOpen>]
module HtmlLogic =

    // -- Attributes

    /// Tries to find an attribute by name.
    let tryAttribute name (element : HtmlElement) =
        element.Attributes
        |> Seq.tryFind (fun x -> String.ordinalEqualsCI x.Name name)

    /// Tries to find an attribute by name and get its value.
    let tryAttributeValue name element =
        element
        |> tryAttribute name
        |> Option.map (fun x -> x.Value)

    // -- Elements

    /// Tries to cast a node to an element.
    let tryAsElement (node : HtmlNode) =
        match node with
        | :? HtmlElement as x -> Some x
        | _ -> None

    /// Tries to get the value of the "id" attribute.
    let tryId element =
        element
        |> tryAttributeValue "id"

    /// Tries to get the value of the "class" attribute.
    let tryClassName element =
        element
        |> tryAttributeValue "class"

    /// Tries to get the value of the "href" attribute.
    let tryHref element =
        element
        |> tryAttributeValue "href"

    /// Tries to get the value of the "src" attribute.
    let trySrc element =
        element
        |> tryAttributeValue "src"

    /// Checks whether an element has specified tag name.
    /// This takes into account case.
    let nameMatches name (element : HtmlElement) =
        String.ordinalEqualsCI element.Name name

    /// Checks whether an element has specified value of "id" attribute.
    /// This takes into account case.
    let idMatches id element =
        element
        |> tryId
        |> Option.exists (String.ordinalEquals id)

    /// Gets the value of the "class" attribute as a list of space-separated elements.
    let classNames element =
        element
        |> tryClassName
        |> Option.map (String.split ' ')
        |> Option.defaultValue Array.empty

    /// Checks whether the class name of an element matches specified class name.
    /// This function works by splitting both class names by space and checking if the element contains all individual
    /// elements in the list.
    let classNameMatches className element =
        let targetClassNames = className |> String.split ' '
        let sourceClassNames = element |> classNames

        targetClassNames
        |> Seq.forall (fun x -> sourceClassNames |> Seq.contains x)

    // -- Containers

    let rec private appendInnerText (node : HtmlNode) (buffer : StringBuilder) =
        let isFirstNode = buffer.Length = 0

        let isEmpty element =
            nameMatches "script" element ||
            nameMatches "style" element ||
            nameMatches "select" element ||
            nameMatches "canvas" element ||
            nameMatches "video" element ||
            nameMatches "iframe" element

        let shouldPrependLine element =
            not isFirstNode && (
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

    /// Gets inner text.
    let innerText container =
        StringBuilder()
        |> appendInnerText container
        |> string

    /// Gets all of the node's ancestors, from immediate parent to the root node.
    let rec ancestors (node : HtmlNode) = seq {
        if not (isNull node.Parent) then
            yield node.Parent
            yield! node.Parent |> ancestors
    }

    /// Gets all of the node's siblings.
    let siblings (node : HtmlNode) = seq {
        if not (isNull node.Parent) then
            yield! node.Parent.Children |> Seq.filter (fun x -> x <> node)
    }

    /// Gets all of the node's siblings that appear before it in the DOM.
    let rec previousSiblings (node : HtmlNode) = seq {
        if not (isNull node.Previous) then
            yield node.Previous
            yield! node.Previous |> previousSiblings
    }

    /// Gets all of the node's siblings that appear after it in the DOM.
    let rec nextSiblings (node : HtmlNode) = seq {
        if not (isNull node.Next) then
            yield node.Next
            yield! node.Next |> nextSiblings
    }

    /// Filters a sequence of nodes by elements.
    let filterElements (nodes : HtmlNode seq) =
        nodes
        |> Seq.filter (fun x -> x :? HtmlElement)
        |> Seq.cast<HtmlElement>

    /// Gets all descendant nodes (i.e. children and children of children recursively).
    let rec descendants (container : HtmlContainer) = seq {
        for child in container.Children do
            match child with

            | :? HtmlContainer as x ->
                yield child
                yield! descendants x

            | _ ->
                yield child
    }

    /// Gets all descendant elements (i.e. children and children of children recursively).
    let descendantElements container =
        container
        |> descendants
        |> filterElements

    /// Tries to find the first descendant element by the value of its "id" attribute.
    let tryElementById id container =
        container
        |> descendantElements
        |> Seq.tryFind (idMatches id)

    /// Gets all descendant elements that are matched by the specified tag name.
    let elementsByName name container =
        container
        |> descendantElements
        |> Seq.filter (nameMatches name)

    /// Gets all descendant elements that are matched by the specified class name.
    let elementsByClassName className container =
        container
        |> descendantElements
        |> Seq.filter (classNameMatches className)

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

    // -- Entities

    /// Renders an entity as HTML code.
    let toHtml entity =
        StringBuilder()
        |> appendHtml entity 0
        |> string

// C# API
[<Extension>]
module HtmlLogicExtensions =

    // -- Attributes

    /// Gets an attribute by name or returns null if not found.
    [<Extension>]
    let GetAttribute (self, name) =
        self
        |> tryAttribute name
        |> Option.toObj

    /// Gets the value of an attribute by its name or returns null if attribute is not found.
    [<Extension>]
    let GetAttributeValue (self, name) =
        self
        |> tryAttributeValue name
        |> Option.toObj

    // -- Elements

    /// Gets the value of the "id" attribute or returns null if it's not set.
    [<Extension>]
    let GetId self =
        self
        |> tryId
        |> Option.toObj

    /// Gets the value of the "class" attribute or returns null if it's not set.
    [<Extension>]
    let GetClassName self =
        self
        |> tryClassName
        |> Option.toObj

    /// Gets the value of the "href" attribute or returns null if it's not set.
    [<Extension>]
    let GetHref self =
        self
        |> tryHref
        |> Option.toObj

    /// Gets the value of the "src" attribute or returns null if it's not set.
    [<Extension>]
    let GetSrc self =
        self
        |> trySrc
        |> Option.toObj

    /// Checks whether an element has specified tag name.
    /// This takes into account case.
    [<Extension>]
    let MatchesName (self, name) =
        self
        |> nameMatches name

    /// Checks whether an element has specified value of "id" attribute.
    /// This takes into account case.
    [<Extension>]
    let MatchesId (self, id) =
        self
        |> idMatches id

    /// Gets the value of the "class" attribute as a list of space-separated elements.
    [<Extension>]
    let GetClassNames self =
        self
        |> classNames
        :> IReadOnlyList<string>

    /// Checks whether the class name of an element matches specified class name.
    /// This function works by splitting both class names by space and checking if the element contains all individual
    /// elements in the list.
    [<Extension>]
    let MatchesClassName (self, className) =
        self
        |> classNameMatches className

    // -- Containers

    /// Gets inner text.
    [<Extension>]
    let GetInnerText self =
        self
        |> innerText

    /// Gets all of the node's ancestors, from immediate parent to the root node.
    [<Extension>]
    let GetAncestors (self : HtmlNode) =
        self
        |> ancestors

    /// Gets all of the node's siblings.
    [<Extension>]
    let GetSiblings (self : HtmlNode) =
        self
        |> siblings

    /// Gets all of the node's siblings that appear before it in the DOM.
    [<Extension>]
    let GetPreviousSiblings (self : HtmlNode) =
        self
        |> previousSiblings

    /// Gets all of the node's siblings that appear after it in the DOM.
    [<Extension>]
    let GetNextSiblings (self : HtmlNode) =
        self
        |> nextSiblings

    /// Gets all descendant nodes (i.e. children and children of children recursively).
    [<Extension>]
    let rec GetDescendants self =
        self
        |> descendants

    /// Gets all descendant elements (i.e. children and children of children recursively).
    [<Extension>]
    let GetDescendantElements self =
        self
        |> descendantElements

    /// Gets the first descendant element by the value of its "id" attribute or returns null if not found.
    [<Extension>]
    let GetElementById (self, id) =
        self
        |> tryElementById id
        |> Option.toObj

    /// Gets all descendant elements that are matched by the specified tag name.
    [<Extension>]
    let GetElementsByName (self, name) =
        self
        |> elementsByName name

    /// Gets all descendant elements that are matched by the specified class name.
    [<Extension>]
    let GetElementsByClassName (self, className) =
        self
        |> elementsByClassName className

    // -- Entities

    /// Renders an entity as HTML code.
    [<Extension>]
    let ToHtml self =
        self
        |> toHtml