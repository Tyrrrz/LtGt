using LtGt.Models;

namespace LtGt.Internal.Selectors.Simple
{
    internal class ClassNameSelector : Selector
    {
        public string ClassName { get; }

        public ClassNameSelector(string className)
        {
            ClassName = className;
        }

        public override bool Matches(HtmlElement element) => element.MatchesClassName(ClassName);

        public override string ToString() => $".{ClassName}";
    }
}