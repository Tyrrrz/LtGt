using NUnit.Framework;

namespace LtGt.Tests
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void ConfigureFormatters()
        {
            TestContext.AddFormatter<HtmlEntity>(e => ((HtmlEntity) e).ToHtml());
        }
    }
}