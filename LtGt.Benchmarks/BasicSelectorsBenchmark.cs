using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using LtGt.Models;

namespace LtGt.Benchmarks
{
    [CoreJob]
    [RankColumn]
    public class BasicSelectorsBenchmark
    {
        private HtmlDocument _ltGtDocument;
        private AngleSharp.Html.Dom.IHtmlDocument _angleSharpDocument;
        private HtmlAgilityPack.HtmlDocument _htmlAgilityPackDocument;

        [GlobalSetup]
        public async Task Setup()
        {
            using (var httpClient = new HttpClient())
            {
                var source = await httpClient.GetStringAsync("https://youtube.com/watch?v=9bZkp7q19f0");

                _ltGtDocument = HtmlParser.Default.ParseDocument(source);

                _angleSharpDocument = new AngleSharp.Html.Parser.HtmlParser().ParseDocument(source);

                _htmlAgilityPackDocument = new HtmlAgilityPack.HtmlDocument();
                _htmlAgilityPackDocument.LoadHtml(source);
            }
        }

        private const string ElementId = "player";

        [Benchmark(Description = "LtGt", Baseline = true)]
        public HtmlElement SelectWithLtGt() => _ltGtDocument.GetElementById(ElementId);

        [Benchmark(Description = "AngleSharp")]
        public AngleSharp.Dom.IElement SelectWithAngleSharp() => _angleSharpDocument.GetElementById(ElementId);

        [Benchmark(Description = "HtmlAgilityPack")]
        public HtmlAgilityPack.HtmlNode SelectWithHtmlAgilityPack() => _htmlAgilityPackDocument.GetElementbyId(ElementId);
    }
}