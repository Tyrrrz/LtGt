using Tyrrrz.Extensions;

namespace LtGt.Tests
{
    public static class TestData
    {
        public static string GetTestDocumentHtml() =>
            typeof(TestData).Assembly.GetManifestResourceString($"{typeof(TestData).Namespace}.Resources.TestDocument.html");
    }
}