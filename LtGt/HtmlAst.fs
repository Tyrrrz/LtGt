namespace LtGt

open System
open System.Collections.Generic

// ---------------------------------
// Abstractions
// ---------------------------------

type [<AbstractClass; AllowNullLiteral>] HtmlEntity() =
    abstract member Clone : unit -> HtmlEntity

and [<AbstractClass; AllowNullLiteral>] HtmlNode() =
    inherit HtmlEntity()

    let mutable parent : HtmlContainer = null
    let mutable previous : HtmlNode = null
    let mutable next : HtmlNode = null

    member self.Parent
        with public get() = parent
        and internal set value = parent <- value

    member self.Previous
        with public get() = previous
        and internal set value = previous <- value

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

// ---------------------------------
// Implementations
// ---------------------------------

type [<AllowNullLiteral>] HtmlDeclaration(value : string) =
    inherit HtmlEntity()

    member self.Value = value

    new(other : HtmlDeclaration) = HtmlDeclaration(other.Value)

    override self.Clone() = upcast HtmlDeclaration(self)

type [<AllowNullLiteral>] HtmlAttribute(name : string, value : string) =
    inherit HtmlEntity()

    member self.Name = name
    member self.Value = value

    new(name) = HtmlAttribute(name, null)
    new(other : HtmlAttribute) = HtmlAttribute(other.Name, other.Value)

    override self.Clone() = upcast HtmlAttribute(self)

type [<AllowNullLiteral>] HtmlText(value : string) =
    inherit HtmlNode()

    member self.Value = value

    new(other : HtmlText) = HtmlText(other.Value)

    override self.Clone() = upcast HtmlText(self)

type [<AllowNullLiteral>] HtmlComment(value : string) =
    inherit HtmlNode()

    member self.Value = value

    new(other : HtmlComment) = HtmlComment(other.Value)

    override self.Clone() = upcast HtmlComment(self)

type [<AllowNullLiteral>] HtmlElement(name : string, attributes : IReadOnlyList<HtmlAttribute>, children : IReadOnlyList<HtmlNode>) =
    inherit HtmlContainer(children)

    member self.Name = name
    member self.Attributes = attributes

    // temp hack until mutability is in place
    new(name, [<ParamArray>] content : HtmlEntity[]) =
        let attributes = content |> Seq.filter (fun x -> x :? HtmlAttribute) |> Seq.cast<HtmlAttribute> |> Seq.toArray
        let children = content |> Seq.filter (fun x -> x :? HtmlNode) |> Seq.cast<HtmlNode> |> Seq.toArray
        HtmlElement(name, attributes, children)

    new(other : HtmlElement) =
        HtmlElement(other.Name,
                    other.Attributes |> Seq.map (fun x -> x.Clone() :?> HtmlAttribute) |> Seq.toArray,
                    other.Children |> Seq.map (fun x -> x.Clone() :?> HtmlNode) |> Seq.toArray)

    override self.Clone() = upcast HtmlElement(self)

type [<AllowNullLiteral>] HtmlDocument(declaration : HtmlDeclaration, children : IReadOnlyList<HtmlNode>) =
    inherit HtmlContainer(children)

    member self.Declaration = declaration

     // temp hack until mutability is in place
    new(declaration, [<ParamArray>] children : HtmlNode[]) =
        HtmlDocument(declaration, children :> IReadOnlyList<HtmlNode>)

    new(other : HtmlDocument) =
        HtmlDocument(other.Declaration,
                     other.Children |> Seq.map (fun x -> x.Clone() :?> HtmlNode) |> Seq.toArray)

    override self.Clone() = upcast HtmlDocument(self)