namespace LtGt

open System.Collections.Generic

type HtmlEntityEqualityComparer() =
    let rec equals (a : HtmlEntity) (b : HtmlEntity) =
        match a, b with
        | null, null -> true

        | (:? HtmlDeclaration as x1), (:? HtmlDeclaration as x2) ->
            String.ordinalEqualsCI x1.Value x2.Value

        | (:? HtmlAttribute as x1), (:? HtmlAttribute as x2) ->
            String.ordinalEqualsCI x1.Name x2.Name &&
            String.ordinalEquals x1.Value x2.Value

        | (:? HtmlText as x1), (:? HtmlText as x2) ->
            String.ordinalEquals x1.Value x2.Value

        | (:? HtmlComment as x1), (:? HtmlComment as x2) ->
            String.ordinalEquals x1.Value x2.Value

        | (:? HtmlElement as x1), (:? HtmlElement as x2) ->
            String.ordinalEqualsCI x1.Name x2.Name &&
            Seq.zip x1.Attributes x2.Attributes |> Seq.map (fun (x1a, x2a) -> equals x1a x2a) |> Seq.fold (&&) true &&
            Seq.zip x1.Children x2.Children |> Seq.map (fun (x1c, x2c) -> equals x1c x2c) |> Seq.fold (&&) true

        | (:? HtmlDocument as x1), (:? HtmlDocument as x2) ->
            equals x1.Declaration x2.Declaration &&
            Seq.zip x1.Children x2.Children |> Seq.map (fun (x1c, x2c) -> equals x1c x2c) |> Seq.fold (&&) true

        | _ -> false

    let (<*>) a b = Microsoft.FSharp.Core.Operators.(*) a 23 + b

    let rec hash (a : HtmlEntity) =
        match a with
        | :? HtmlDeclaration as x -> String.ordinalHashCI x.Value

        | :? HtmlAttribute as x -> String.ordinalHashCI x.Name <*> String.ordinalHash x.Value

        | :? HtmlText as x -> String.ordinalHash x.Value

        | :? HtmlComment as x -> String.ordinalHash x.Value

        | :? HtmlElement as x ->
            String.ordinalHashCI x.Name <*>
            (x.Attributes |> Seq.map hash |> Seq.fold (<*>) 17) <*>
            (x.Children |> Seq.map hash |> Seq.fold (<*>) 17)

        | :? HtmlDocument as x ->
            hash x.Declaration <*> (x.Children |> Seq.map hash |> Seq.fold (<*>) 17)

        | _ -> 0

    interface IEqualityComparer<HtmlEntity> with
        member self.Equals(a, b) = equals a b

        member self.GetHashCode(a) = hash a

    static member Instance = HtmlEntityEqualityComparer() :> IEqualityComparer<HtmlEntity>