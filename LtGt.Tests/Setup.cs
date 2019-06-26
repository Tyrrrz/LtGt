using LtGt.Models;
using NUnit.Framework;

namespace LtGt.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void ConfigureFormatters()
        {
            TestContext.AddFormatter<HtmlNode>(n => HtmlRenderer.Default.RenderNode((HtmlNode) n));
        }
    }
}