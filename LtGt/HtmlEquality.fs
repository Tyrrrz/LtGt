namespace LtGt

open System.Collections.Generic

type HtmlEntityEqualityComparer() =
    let rec equals (a : HtmlEntity) (b : HtmlEntity) =
        match a, b with
        | null, null -> true

        | (:? HtmlDeclaration as x1), (:? HtmlDeclaration as x2) ->
            ordinalEqualsCI x1.Value x2.Value

        | (:? HtmlAttribute as x1), (:? HtmlAttribute as x2) ->
            ordinalEqualsCI x1.Name x2.Name && x1.Value = x2.Value

        | (:? HtmlText as x1), (:? HtmlText as x2) ->
            x1.Value = x2.Value

        | (:? HtmlComment as x1), (:? HtmlComment as x2) ->
            x1.Value = x2.Value

        | (:? HtmlElement as x1), (:? HtmlElement as x2) ->
            ordinalEqualsCI x1.Name x2.Name &&
            Seq.zip x1.Attributes x2.Attributes |> Seq.map (fun (x1a, x2a) -> equals x1a x2a) |> Seq.fold (&&) true &&
            Seq.zip x1.Children x2.Children |> Seq.map (fun (x1c, x2c) -> equals x1c x2c) |> Seq.fold (&&) true

        | (:? HtmlDocument as x1), (:? HtmlDocument as x2) ->
            equals x1.Declaration x2.Declaration &&
            Seq.zip x1.Children x2.Children |> Seq.map (fun (x1c, x2c) -> equals x1c x2c) |> Seq.fold (&&) true

        | _ -> false

    let (<*>) h0 h = Microsoft.FSharp.Core.Operators.(*) h0 23 + h

    let rec hash (a : HtmlEntity) =
        match a with
        | :? HtmlDeclaration as x -> ordinalHashCI x.Value

        | :? HtmlAttribute as x -> ordinalHashCI x.Name <*> ordinalHash x.Value

        | :? HtmlText as x -> ordinalHash x.Value

        | :? HtmlComment as x -> ordinalHash x.Value

        | :? HtmlElement as x ->
            ordinalHashCI x.Name <*>
            Seq.fold (<*>) 17 (x.Attributes |> Seq.map hash) <*>
            Seq.fold (<*>) 17 (x.Children |> Seq.map hash)

        | :? HtmlDocument as x ->
            hash x.Declaration <*>
            Seq.fold (<*>) 17 (x.Children |> Seq.map hash)

        | _ -> 0


    interface IEqualityComparer<HtmlEntity> with
        member self.Equals(a, b) = equals a b

        member self.GetHashCode(a) = hash a

    static member Instance = HtmlEntityEqualityComparer() :> IEqualityComparer<HtmlEntity>