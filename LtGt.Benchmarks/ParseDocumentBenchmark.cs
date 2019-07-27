using System.Net.Http;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using LtGt.Models;

namespace LtGt.Benchmarks
{
    [CoreJob]
    [RankColumn]
    public class ParseDocumentBenchmark
    {
        private string _source;

        [GlobalSetup]
        public async Task Setup()
        {
            using (var httpClient = new HttpClient())
                _source = await httpClient.GetStringAsync("https://youtube.com/watch?v=9bZkp7q19f0");
        }

        [Benchmark(Description = "LtGt", Baseline = true)]
        public HtmlDocument ParseWithLtGt() => HtmlParser.Default.ParseDocument(_source);

        [Benchmark(Description = "AngleSharp")]
        public AngleSharp.Html.Dom.IHtmlDocument ParseWithAngleSharp() => new AngleSharp.Html.Parser.HtmlParser().ParseDocument(_source);

        [Benchmark(Description = "HtmlAgilityPack")]
        public HtmlAgilityPack.HtmlDocument ParseWithHtmlAgilityPack()
        {
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(_source);

            return document;
        }
    }
}