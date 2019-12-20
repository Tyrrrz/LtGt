namespace LtGt.Benchmarks

open System.Net.Http
open BenchmarkDotNet.Attributes
open FSharp.Control.Tasks.V2
open LtGt

[<AutoOpen>]
module private Helpers =

    let parseDocumentWithLtGt source =
        Html.ParseDocument source

    let parseDocumentWithAngleSharp (source : string) =
        AngleSharp.Html.Parser.HtmlParser().ParseDocument(source)

    let parseDocumentWithHtmlAgilityPack (source : string) =
        let document = HtmlAgilityPack.HtmlDocument()
        document.LoadHtml source
        document

[<SimpleJob>]
type ParseDocumentBenchmark() =

    let mutable _source : string = null

    [<GlobalSetup>]
    member self.Setup() = task {
        use client = new HttpClient()
        let! source = client.GetStringAsync("https://youtube.com/watch?v=9bZkp7q19f0")
        _source <- source
    }

    [<Benchmark(Description = "LtGt", Baseline = true)>]
    member self.LtGt() = parseDocumentWithLtGt _source

    [<Benchmark(Description = "AngleSharp")>]
    member self.AngleSharp() = parseDocumentWithAngleSharp _source

    [<Benchmark(Description = "HtmlAgilityPack")>]
    member self.HtmlAgilityPack() = parseDocumentWithHtmlAgilityPack _source

[<SimpleJob>]
type BasicSelectorsBenchmark() =

    let mutable _ltGtDocument : HtmlDocument = null
    let mutable _angleSharpDocument : AngleSharp.Html.Dom.IHtmlDocument = null
    let mutable _htmlAgilityPackDocument : HtmlAgilityPack.HtmlDocument = null

    [<GlobalSetup>]
    member self.Setup() = task {
        use client = new HttpClient()
        let! source = client.GetStringAsync("https://youtube.com/watch?v=9bZkp7q19f0")

        _ltGtDocument <- parseDocumentWithLtGt source
        _angleSharpDocument <- parseDocumentWithAngleSharp source
        _htmlAgilityPackDocument <- parseDocumentWithHtmlAgilityPack source
    }

    [<Benchmark(Description = "LtGt", Baseline = true)>]
    member self.LtGt() = _ltGtDocument |> tryElementById "player"

    [<Benchmark(Description = "AngleSharp")>]
    member self.AngleSharp() = _angleSharpDocument.GetElementById("player")

    [<Benchmark(Description = "HtmlAgilityPack")>]
    member self.HtmlAgilityPack() = _htmlAgilityPackDocument.GetElementbyId("player")

[<SimpleJob>]
type CssSelectorsBenchmark() =

    let mutable _ltGtDocument : HtmlDocument = null
    let mutable _angleSharpDocument : AngleSharp.Html.Dom.IHtmlDocument = null

    [<GlobalSetup>]
    member self.Setup() = task {
        use client = new HttpClient()
        let! source = client.GetStringAsync("https://youtube.com/watch?v=9bZkp7q19f0")

        _ltGtDocument <- parseDocumentWithLtGt source
        _angleSharpDocument <- parseDocumentWithAngleSharp source
    }

    [<Benchmark(Description = "LtGt", Baseline = true)>]
    member self.LtGt() = _ltGtDocument |> queryElements "div#player" |> Seq.toArray

    [<Benchmark(Description = "AngleSharp")>]
    member self.AngleSharp() = _angleSharpDocument.QuerySelectorAll("div#player") |> Seq.toArray