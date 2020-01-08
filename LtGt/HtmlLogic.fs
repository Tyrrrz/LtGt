namespace LtGt

open System
open System.Collections.Generic
open System.Runtime.CompilerServices
open System.Text
open System.Xml.Linq

// F# API
[<AutoOpen>]
module HtmlLogic =

    // ** Primitives

    /// Tries to cast a node to an element.
    let tryAsElement (node : HtmlNode) =
        match node with
        | :? HtmlElement as x -> Some x
        | _ -> None

    /// Tries to find an attribute by name.
    let tryAttribute name (element : HtmlElement) =
        element.Attributes
        |> Seq.tryFind (fun x -> String.ordinalEqualsCI x.Name name)

    /// Tries to find an attribute by name and get its value.
    let tryAttributeValue name = tryAttribute name >> Option.map (fun x -> x.Value)

    /// Tries to get the value of the "id" attribute.
    let tryId = tryAttributeValue "id"

    /// Tries to get the value of the "class" attribute.
    let tryClassName = tryAttributeValue "class"

    /// Gets the value of the "class" attribute as a list of space-separated elements.
    let classNames =
        tryClassName
        >> Option.map (String.split ' ' >> List.ofArray)
        >> Option.defaultValue List.empty

    /// Checks whether an element has specified tag name.
    let tagNameMatches name (element : HtmlElement) =
        String.ordinalEqualsCI element.TagName name

    /// Checks whether an element has specified value of "id" attribute.
    let idMatches id = tryId >> Option.exists (String.ordinalEquals id)

    /// Checks whether the class name of an element matches specified class name.
    /// This function works by splitting both class names by space and checking if the element contains all individual
    /// classes in the list.
    let classNameMatches className element =
        let targetClassNames = className |> String.split ' '
        let sourceClassNames = element |> classNames
        targetClassNames |> Seq.forall (fun x -> sourceClassNames |> Seq.exists (String.ordinalEquals x))

    // ** Hierarchical navigation

    /// Gets all of the node's ancestors, from immediate parent to the root node.
    let rec ancestors (node : HtmlNode) = seq {
        if not <| isNull node.Parent then
            yield node.Parent
            yield! ancestors node.Parent
    }

    /// Gets all of the node's siblings.
    let siblings (node : HtmlNode) = seq {
        if not <| isNull node.Parent then
            yield! node.Parent.Children |> Seq.filter (fun x -> x <> node)
    }

    /// Gets all of the node's siblings that appear before it in the DOM.
    let rec previousSiblings (node : HtmlNode) = seq {
        if not <| isNull node.Previous then
            yield node.Previous
            yield! previousSiblings node.Previous
    }

    /// Gets all of the node's siblings that appear after it in the DOM.
    let rec nextSiblings (node : HtmlNode) = seq {
        if not <| isNull node.Next then
            yield node.Next
            yield! nextSiblings node.Next
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
            | _ -> yield child
    }

    /// Gets all descendant elements (i.e. children and children of children recursively).
    let descendantElements container =
        container
        |> descendants
        |> filterElements

    // ** Basic selectors

    /// Tries to find the first descendant element by the value of its "id" attribute.
    let tryElementById id = descendantElements >> Seq.tryFind (idMatches id)

    /// Gets all descendant elements that are matched by the specified tag name.
    let elementsByTagName name = descendantElements >> Seq.filter (tagNameMatches name)

    /// Gets all descendant elements that are matched by the specified class name.
    let elementsByClassName className = descendantElements >> Seq.filter (classNameMatches className)

    // ** Misc

    let rec private appendInnerText (node : HtmlNode) (buffer : StringBuilder) =
        let isHidden (element : HtmlElement) =
            tagNameMatches "br" element ||
            tagNameMatches "script" element ||
            tagNameMatches "style" element ||
            tagNameMatches "select" element ||
            tagNameMatches "canvas" element ||
            tagNameMatches "video" element ||
            tagNameMatches "iframe" element

        let shouldPrependLine (element : HtmlElement) =
            buffer.Length <> 0 && (
                tagNameMatches "br" element ||
                tagNameMatches "p" element ||
                tagNameMatches "caption" element ||
                tagNameMatches "div" element ||
                tagNameMatches "li" element)

        match node with
        | :? HtmlText as a -> buffer.Append a.Content
        | :? HtmlCData as a -> buffer.Append a.Content
        | :? HtmlElement as a ->
            if shouldPrependLine a then
                do buffer.AppendLine() |> ignore

            if isHidden a then
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

    let rec private appendHtml (entity : HtmlEntity) depth (buffer : StringBuilder) =
        match entity with

        | :? HtmlDeclaration as x ->
            buffer.Append (sprintf "<!%s>" x.Content)

        | :? HtmlAttribute as x ->
            if String.IsNullOrWhiteSpace x.Value then
                buffer.Append x.Name
            else
                buffer.Append (sprintf "%s=\"%s\"" x.Name (htmlEncode x.Value))

        | :? HtmlText as x ->
            if x.Parent |> tryAsElement |> Option.exists (fun a -> isRawTextElementTagName a.TagName) then
                buffer.Append x.Content
            else
                buffer.Append (htmlEncode x.Content)

        | :? HtmlComment as x ->
            buffer.Append (sprintf "<!-- %s -->" x.Content)

        | :? HtmlCData as x ->
            buffer.Append (sprintf "<![CDATA[%s]]>" x.Content)

        | :? HtmlElement as x ->
            do buffer.Append('<').Append(x.TagName) |> ignore

            for attribute in x.Attributes do
                buffer.Append ' ' |> appendHtml attribute depth |> ignore
            do buffer.Append '>' |> ignore

            if not (isVoidElementTagName x.TagName) || not (x.Children |> Seq.isEmpty) then
                for child in x.Children do
                    buffer.AppendLineIndented(depth + 1) |> appendHtml child (depth + 1) |> ignore
                do buffer.AppendLineIndented(depth).Append("</").Append(x.TagName).Append('>') |> ignore

            buffer

        | :? HtmlDocument as x ->
            do buffer |> appendHtml x.Declaration depth |> ignore
            do buffer.AppendLine() |> ignore

            for child in x.Children do
                do buffer |> appendHtml child depth |> ignore
                do buffer.AppendLine() |> ignore

            buffer

        | _ -> failwith "Unmatched entity."

    /// Renders an entity as HTML code.
    let toHtml entity =
        StringBuilder()
        |> appendHtml entity 0
        |> string

    /// Converts an entity to its equivalent LINQ2XML representation.
    let rec toXObject (entity : HtmlEntity) : XObject =
        match entity with
        | :? HtmlDeclaration as x -> upcast XComment(x.Content)

        | :? HtmlAttribute as x ->
            upcast XAttribute(XName.Get(x.Name),
               x.Value |> Option.ofObj |> Option.defaultValue "")

        | :? HtmlText as x -> upcast XText(x.Content)
        | :? HtmlComment as x -> upcast XComment(x.Content)
        | :? HtmlCData as x -> upcast XCData(x.Content)

        | :? HtmlElement as x ->
            upcast XElement(XName.Get(x.TagName),
                x.Attributes |> Seq.map toXObject |> Array.ofSeq,
                x.Children |> Seq.map toXObject |> Array.ofSeq)

        | :? HtmlDocument as x ->
            upcast XDocument(x.Children |> Seq.map toXObject |> Array.ofSeq)

        | _ -> failwith "Unmatched entity."

    /// Creates a deep copy of an entity.
    let rec clone (entity : HtmlEntity) : HtmlEntity =
        match entity with
        | :? HtmlDeclaration as x -> upcast HtmlDeclaration(x.Content)
        | :? HtmlAttribute as x -> upcast HtmlAttribute(x.Name, x.Value)
        | :? HtmlText as x -> upcast HtmlText(x.Content)
        | :? HtmlComment as x -> upcast HtmlComment(x.Content)
        | :? HtmlCData as x -> upcast HtmlCData(x.Content)

        | :? HtmlElement as x ->
            upcast HtmlElement(x.TagName,
                x.Attributes |> Seq.map clone |> Seq.cast |> Seq.toArray,
                x.Children |> Seq.map clone |> Seq.cast |> Seq.toArray)

        | :? HtmlDocument as x ->
            upcast HtmlDocument(x.Declaration |> clone :?> HtmlDeclaration,
                x.Children |> Seq.map clone |> Seq.cast<HtmlNode> |> Seq.toArray)

        | _ -> failwith "Unmatched entity."

// C# API
[<Extension>]
module HtmlLogicExtensions =

    // ** Primitives

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

    /// Gets the value of the "class" attribute as a list of space-separated elements.
    [<Extension>]
    let GetClassNames self =
        self
        |> classNames
        :> IReadOnlyList<string>

    /// Checks whether an element has specified tag name.
    [<Extension>]
    let TagNameMatches (self, name) =
        self
        |> tagNameMatches name

    /// Checks whether an element has specified value of "id" attribute.
    [<Extension>]
    let IdMatches (self, id) =
        self
        |> idMatches id

    /// Checks whether the class name of an element matches specified class name.
    /// This function works by splitting both class names by space and checking if the element contains all individual
    /// classes in the list.
    [<Extension>]
    let ClassNameMatches (self, className) =
        self
        |> classNameMatches className

    // ** Hierarchical navigation

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

    // ** Basic selectors

    /// Gets the first descendant element by the value of its "id" attribute or returns null if not found.
    [<Extension>]
    let GetElementById (self, id) =
        self
        |> tryElementById id
        |> Option.toObj

    /// Gets all descendant elements that are matched by the specified tag name.
    [<Extension>]
    let GetElementsByTagName (self, name) =
        self
        |> elementsByTagName name

    /// Gets all descendant elements that are matched by the specified class name.
    [<Extension>]
    let GetElementsByClassName (self, className) =
        self
        |> elementsByClassName className

    // ** Misc

    /// Gets inner text.
    [<Extension>]
    let GetInnerText self =
        self
        |> innerText

    /// Renders an entity as HTML code.
    [<Extension>]
    let ToHtml self =
        self
        |> toHtml

    /// Converts an entity to its equivalent LINQ2XML representation.
    [<Extension>]
    let ToXObject self =
        self
        |> toXObject

    /// Creates a deep copy of an entity.
    [<Extension>]
    let Clone self =
        self
        |> clone