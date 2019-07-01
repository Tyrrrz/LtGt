using System.Linq;
using LtGt.Internal.Selectors.Contextual;
using LtGt.Internal.Selectors.Simple;
using LtGt.Internal.Selectors.Simple.StringOperators;
using Sprache;

namespace LtGt.Internal.Selectors
{
    internal static class SelectorGrammar
    {
        private static readonly Parser<string> Name = Parse.LetterOrDigit.AtLeastOnce().Text();

        private static readonly Parser<NameSelector> NameSelector = Name.Select(n => new NameSelector(n));

        private static readonly Parser<string> ClassName =
            from leading in Parse.Letter.Or(Parse.Char('_')).Once().Text()
            from trailing in Parse.LetterOrDigit.Or(Parse.Chars('_', '-')).Many().Text()
            select leading + trailing;

        private static readonly Parser<ClassNameSelector> ClassNameSelector =
            from dot in Parse.Char('.')
            from className in ClassName
            select new ClassNameSelector(className);

        private static readonly Parser<string> Id = Parse.AnyChar.Except(Parse.WhiteSpace.Or(Parse.Chars('.', '#', ':', '['))).AtLeastOnce().Text();

        private static readonly Parser<IdSelector> IdSelector =
            from pound in Parse.Char('#')
            from id in Id
            select new IdSelector(id);

        private static readonly Parser<string> AttributeName =
            Parse.CharExcept(
                c => char.IsWhiteSpace(c) || c == '~' || c == '^' || c == '$' || c == '*' || c == '|' || c == '=' || c == '\'' || c == '"' || c == ']',
                "invalid attribute name characters")
                .AtLeastOnce().Text();

        private static readonly Parser<StringMatchOperator> StringMatchOperator =
            Parse.Char('~').Return(new WhiteSpaceSeparatedContainsStringMatchOperator()).Or<StringMatchOperator>(
                Parse.Char('^').Return(new StartsWithStringMatchOperator())).Or(
                Parse.Char('$').Return(new EndsWithStringMatchOperator())).Or(
                Parse.Char('*').Return(new ContainsStringMatchOperator())).Or(
                Parse.Char('|').Return(new HyphenSeparatedStartsWithStringMatchOperator()));

        private static readonly Parser<AttributeSelector> NormalAttributeSelector =
            from openBrace in Parse.Char('[')
            from name in AttributeName
            from matchOperator in StringMatchOperator.Optional().Select(o => o.GetOrElse(new EqualsStringMatchOperator()))
            from eq in Parse.Char('=')
            from openQuote in Parse.Chars('"', '\'')
            from value in Parse.CharExcept(openQuote).Many().Text()
            from closeQuote in Parse.Char(openQuote)
            from closeBrace in Parse.Char(']')
            select new AttributeSelector(name, value, matchOperator);

        private static readonly Parser<AttributeSelector> ValuelessAttributeSelector =
            from open in Parse.Char('[')
            from name in AttributeName
            from close in Parse.Char(']')
            select new AttributeSelector(name);

        private static readonly Parser<AttributeSelector> AttributeSelector = NormalAttributeSelector.Or(ValuelessAttributeSelector);

        private static readonly Parser<CombinedSelector> CombinedSelector =
            NameSelector.Or<Selector>(ClassNameSelector).Or(IdSelector).Or(AttributeSelector)
                .XAtLeastOnce()
                .Select(s => new CombinedSelector(s.ToArray()));

        private static readonly Parser<DescendantSelector> DescendantSelector =
            from parentSelector in CombinedSelector
            from space in Parse.WhiteSpace
            from targetSelector in CombinedSelector
            select new DescendantSelector(parentSelector, targetSelector);

        private static readonly Parser<ChildSelector> ChildSelector =
            from parentSelector in CombinedSelector
            from gt in Parse.Char('>').Token()
            from targetSelector in CombinedSelector
            select new ChildSelector(parentSelector, targetSelector);

        private static readonly Parser<OnlyOfTypeSelector> OnlyOfTypeSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":only-of-type")
            select new OnlyOfTypeSelector(targetSelector);

        private static readonly Parser<FirstOfTypeSelector> FirstOfTypeSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":first-of-type")
            select new FirstOfTypeSelector(targetSelector);

        private static readonly Parser<LastOfTypeSelector> LastOfTypeSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":last-of-type")
            select new LastOfTypeSelector(targetSelector);

        private static readonly Parser<NthOfTypeSelector> NthOfTypeSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":nth-of-type")
            from open in Parse.Char('(')
            from index in Parse.Digit.AtLeastOnce().Text().Select(int.Parse)
            from close in Parse.Char(')')
            select new NthOfTypeSelector(targetSelector, index);

        private static readonly Parser<NthLastOfTypeSelector> NthLastOfTypeSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":nth-last-of-type")
            from open in Parse.Char('(')
            from index in Parse.Digit.AtLeastOnce().Text().Select(int.Parse)
            from close in Parse.Char(')')
            select new NthLastOfTypeSelector(targetSelector, index);

        private static readonly Parser<OnlyChildSelector> OnlyChildSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":only-child")
            select new OnlyChildSelector(targetSelector);

        private static readonly Parser<FirstChildSelector> FirstChildSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":first-child")
            select new FirstChildSelector(targetSelector);

        private static readonly Parser<LastChildSelector> LastChildSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":last-child")
            select new LastChildSelector(targetSelector);

        private static readonly Parser<NthChildSelector> NthChildSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":nth-child")
            from open in Parse.Char('(')
            from index in Parse.Digit.AtLeastOnce().Text().Select(int.Parse)
            from close in Parse.Char(')')
            select new NthChildSelector(targetSelector, index);

        private static readonly Parser<NthLastChildSelector> NthLastChildSelector =
            from targetSelector in CombinedSelector
            from name in Parse.IgnoreCase(":nth-last-child")
            from open in Parse.Char('(')
            from index in Parse.Digit.AtLeastOnce().Text().Select(int.Parse)
            from close in Parse.Char(')')
            select new NthLastChildSelector(targetSelector, index);

        private static readonly Parser<SiblingSelector> SiblingSelector =
            from previousSelector in CombinedSelector
            from plus in Parse.Char('+').Token()
            from targetSelector in CombinedSelector
            select new SiblingSelector(previousSelector, targetSelector);

        private static readonly Parser<SubsequentSiblingSelector> SubsequentSiblingSelector =
            from previousSelector in CombinedSelector
            from tilde in Parse.Char('~').Token()
            from targetSelector in CombinedSelector
            select new SubsequentSiblingSelector(previousSelector, targetSelector);

        public static readonly Parser<Selector> Selector = ChildSelector.Or<Selector>(SiblingSelector).Or(OnlyOfTypeSelector)
            .Or(NthOfTypeSelector).Or(NthLastOfTypeSelector).Or(FirstChildSelector).Or(LastChildSelector).Or(FirstOfTypeSelector)
            .Or(LastOfTypeSelector).Or(OnlyChildSelector).Or(SubsequentSiblingSelector).Or(DescendantSelector).Or(NthChildSelector)
            .Or(NthLastChildSelector).Or(CombinedSelector);
    }
}