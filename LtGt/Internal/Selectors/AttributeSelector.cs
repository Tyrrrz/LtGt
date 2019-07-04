using LtGt.Internal.Selectors.Terms;
using LtGt.Models;

namespace LtGt.Internal.Selectors
{
    internal class AttributeSelector : Selector
    {
        public string AttributeName { get; }

        public string AttributeValue { get; }

        public StringComparisonTerm StringComparisonTerm { get; }

        public AttributeSelector(string attributeName, string attributeValue, StringComparisonTerm stringComparisonTerm)
        {
            AttributeName = attributeName;
            AttributeValue = attributeValue;
            StringComparisonTerm = stringComparisonTerm;
        }

        public AttributeSelector(string attributeName)
            : this(attributeName, null, null)
        {
        }

        public override bool Matches(HtmlElement element)
        {
            var attribute = element.GetAttribute(AttributeName);

            if (attribute == null)
                return false;

            if (StringComparisonTerm != null && !StringComparisonTerm.Matches(attribute.Value, AttributeValue))
                return false;

            return true;
        }

        public override string ToString() => StringComparisonTerm != null
            ? $"[{AttributeName}{StringComparisonTerm}=\"{AttributeValue}\"]"
            : $"[{AttributeName}]";
    }
}