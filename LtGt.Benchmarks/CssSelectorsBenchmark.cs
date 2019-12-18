using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace LtGt.Benchmarks
{
    [CoreJob]
    [RankColumn]
    public class CssSelectorsBenchmark
    {
        private HtmlDocument _ltGtDocument;
        private AngleSharp.Html.Dom.IHtmlDocument _angleSharpDocument;

        [GlobalSetup]
        public async Task Setup()
        {
            using var httpClient = new HttpClient();

            var source = await httpClient.GetStringAsync("https://youtube.com/watch?v=9bZkp7q19f0");

            _ltGtDocument = Html.ParseDocument(source);
            _angleSharpDocument = new AngleSharp.Html.Parser.HtmlParser().ParseDocument(source);
        }

        private const string CssSelector = "div#player";

        [Benchmark(Description = "LtGt", Baseline = true)]
        public IReadOnlyList<HtmlElement> SelectWithLtGt() => _ltGtDocument.QuerySelectorAll(CssSelector).ToArray();

        [Benchmark(Description = "AngleSharp")]
        public IReadOnlyList<AngleSharp.Dom.IElement> SelectWithAngleSharp() => _angleSharpDocument.QuerySelectorAll(CssSelector).ToArray();
    }
}