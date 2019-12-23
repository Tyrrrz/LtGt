namespace LtGt

open System
open System.Collections.Generic

// Abstractions

type [<AbstractClass; AllowNullLiteral>] HtmlEntity() = class end

and [<AbstractClass; AllowNullLiteral>] HtmlNode() =
    inherit HtmlEntity()

    let mutable parent : HtmlContainer = null
    let mutable previous : HtmlNode = null
    let mutable next : HtmlNode = null

    /// Parent container of this node.
    /// This property can be null.
    member self.Parent
        with public get() = parent
        and internal set value = parent <- value

    /// Previous node.
    /// This property can be null.
    member self.Previous
        with public get() = previous
        and internal set value = previous <- value

    /// Next node.
    /// This property can be null.
    member self.Next
        with public get() = next
        and internal set value = next <- value

and [<AbstractClass; AllowNullLiteral>] HtmlContainer(children : IReadOnlyList<HtmlNode>) as self =
    inherit HtmlNode()

    do children |> Seq.iteri (fun i node ->
        node.Parent <- self
        node.Previous <- children |> Seq.tryItem (i - 1) |> Option.toObj
        node.Next <- children |> Seq.tryItem (i + 1) |> Option.toObj
    )

    member self.Children = children

// Implementations

type [<AllowNullLiteral>] HtmlDeclaration(content : string) =
    inherit HtmlEntity()

    member self.Content = content

and [<AllowNullLiteral>] HtmlAttribute(name : string, value : string) =
    inherit HtmlEntity()

    let mutable parent : HtmlElement = null
    let mutable previous : HtmlAttribute = null
    let mutable next : HtmlAttribute = null

    member self.Name = name
    member self.Value = value

    /// Parent element that contains this attribute.
    /// This property can be null.
    member self.Parent
        with public get() = parent
        and internal set value = parent <- value

    /// Previous attribute.
    /// This property can be null.
    member self.Previous
        with public get() = previous
        and internal set value = previous <- value

    /// Next attribute.
    /// This property can be null.
    member self.Next
        with public get() = next
        and internal set value = next <- value

    new(name) = HtmlAttribute(name, null)

and [<AllowNullLiteral>] HtmlText(content : string) =
    inherit HtmlNode()

    member self.Content = content

and [<AllowNullLiteral>] HtmlComment(content : string) =
    inherit HtmlNode()

    member self.Content = content

and [<AllowNullLiteral>] HtmlCData(content : string) =
    inherit HtmlNode()

    member self.Content = content

and [<AllowNullLiteral>] HtmlElement(tagName : string,
                                     attributes : IReadOnlyList<HtmlAttribute>,
                                     children : IReadOnlyList<HtmlNode>) as self =
    inherit HtmlContainer(children)

    do attributes |> Seq.iteri (fun i node ->
        node.Parent <- self
        node.Previous <- attributes |> Seq.tryItem (i - 1) |> Option.toObj
        node.Next <- attributes |> Seq.tryItem (i + 1) |> Option.toObj
    )

    member self.TagName = tagName
    member self.Attributes = attributes

    new(name, attr1, attr2, attr3, attr4, [<ParamArray>] children : HtmlNode[]) =
        HtmlElement(name, [ attr1; attr2; attr3; attr4 ], children)

    new(name, attr1, attr2, attr3, [<ParamArray>] children : HtmlNode[]) =
        HtmlElement(name, [ attr1; attr2; attr3 ], children)

    new(name, attr1, attr2, [<ParamArray>] children : HtmlNode[]) =
        HtmlElement(name, [ attr1; attr2 ], children)

    new(name, attribute : HtmlAttribute, [<ParamArray>] children : HtmlNode[]) =
        HtmlElement(name, [ attribute ], children)

    new(name, [<ParamArray>] children : HtmlNode[]) =
        HtmlElement(name, Array.empty, children)

and [<AllowNullLiteral>] HtmlDocument(declaration : HtmlDeclaration,
                                      children : IReadOnlyList<HtmlNode>) =
    inherit HtmlContainer(children)

    member self.Declaration = declaration

    new(declaration, [<ParamArray>] children : HtmlNode[]) =
        HtmlDocument(declaration, children :> IReadOnlyList<HtmlNode>)