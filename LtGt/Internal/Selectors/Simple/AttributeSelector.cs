using LtGt.Internal.Selectors.Simple.StringOperators;
using LtGt.Models;

namespace LtGt.Internal.Selectors.Simple
{
    internal class AttributeSelector : Selector
    {
        public string AttributeName { get; }

        public string AttributeValue { get; }

        public StringMatchOperator AttributeValueMatchOperator { get; }

        public AttributeSelector(string attributeName, string attributeValue, StringMatchOperator attributeValueMatchOperator)
        {
            AttributeName = attributeName;
            AttributeValue = attributeValue;
            AttributeValueMatchOperator = attributeValueMatchOperator;
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

            if (AttributeValueMatchOperator != null && !AttributeValueMatchOperator.Matches(attribute.Value, AttributeValue))
                return false;

            return true;
        }

        public override string ToString() => AttributeValueMatchOperator != null
            ? $"[{AttributeName}{AttributeValueMatchOperator}=\"{AttributeValue}\"]"
            : $"[{AttributeName}]";
    }
}