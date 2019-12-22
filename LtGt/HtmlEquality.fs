namespace LtGt

open System.Collections.Generic

// F# API
[<AutoOpen>]
module HtmlEquality =

    /// Diffuses hash.
    let inline private (<*>) a b = Microsoft.FSharp.Core.Operators.(*) a 23 + b

    /// Compares two HTML entities and returns whether they are logically equal.
    let rec htmlEquals (a : HtmlEntity) (b : HtmlEntity) =
        match a, b with

        | null, null -> true

        | (:? HtmlDeclaration as x1), (:? HtmlDeclaration as x2) ->
            String.ordinalEqualsCI x1.Content x2.Content

        | (:? HtmlAttribute as x1), (:? HtmlAttribute as x2) ->
            String.ordinalEqualsCI x1.Name x2.Name &&
            String.ordinalEquals x1.Value x2.Value

        | (:? HtmlText as x1), (:? HtmlText as x2) ->
            String.ordinalEquals x1.Content x2.Content

        | (:? HtmlComment as x1), (:? HtmlComment as x2) ->
            String.ordinalEquals x1.Content x2.Content

        | (:? HtmlCData as x1), (:? HtmlCData as x2) ->
            String.ordinalEquals x1.Content x2.Content

        | (:? HtmlElement as x1), (:? HtmlElement as x2) ->
            String.ordinalEqualsCI x1.TagName x2.TagName &&
            x1.Attributes.Count = x2.Attributes.Count &&
            x1.Children.Count = x2.Children.Count &&
            Seq.zip x1.Attributes x2.Attributes |> Seq.map (fun (x1a, x2a) -> htmlEquals x1a x2a) |> Seq.fold (&&) true &&
            Seq.zip x1.Children x2.Children |> Seq.map (fun (x1c, x2c) -> htmlEquals x1c x2c) |> Seq.fold (&&) true

        | (:? HtmlDocument as x1), (:? HtmlDocument as x2) ->
            htmlEquals x1.Declaration x2.Declaration &&
            x1.Children.Count = x2.Children.Count &&
            Seq.zip x1.Children x2.Children |> Seq.map (fun (x1c, x2c) -> htmlEquals x1c x2c) |> Seq.fold (&&) true

        | _ -> false

    /// Calculates hashcode for specified HTML entity.
    let rec htmlHash (e : HtmlEntity) =
        match e with

        | :? HtmlDeclaration as x -> String.ordinalHashCI x.Content

        | :? HtmlAttribute as x -> String.ordinalHashCI x.Name <*> String.ordinalHash x.Value

        | :? HtmlText as x -> String.ordinalHash x.Content

        | :? HtmlComment as x -> String.ordinalHash x.Content

        | :? HtmlCData as x -> String.ordinalHash x.Content

        | :? HtmlElement as x ->
            String.ordinalHashCI x.TagName <*>
            (x.Attributes |> Seq.map htmlHash |> Seq.fold (<*>) 17) <*>
            (x.Children |> Seq.map htmlHash |> Seq.fold (<*>) 17)

        | :? HtmlDocument as x ->
            htmlHash x.Declaration <*> (x.Children |> Seq.map htmlHash |> Seq.fold (<*>) 17)

        | _ -> 0

// C# API
type HtmlEntityEqualityComparer() =

    interface IEqualityComparer<HtmlEntity> with
        member self.Equals(a, b) = htmlEquals a b

        member self.GetHashCode(a) = htmlHash a

    static member Instance = HtmlEntityEqualityComparer() :> IEqualityComparer<HtmlEntity>