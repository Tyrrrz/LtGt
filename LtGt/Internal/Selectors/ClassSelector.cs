using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class ClassSelector : Selector
    {
        public string ClassName { get; }

        public ClassSelector(string className)
        {
            ClassName = className;
        }

        public override bool Matches(HtmlElement element) => element.MatchesClassName(ClassName);

        public override string ToString() => $".{ClassName}";
    }
}