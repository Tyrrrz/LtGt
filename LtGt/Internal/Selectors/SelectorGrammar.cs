﻿using System.Globalization;
using System.Linq;
using LtGt.Internal.Selectors.Combinators;
using LtGt.Internal.Selectors.Terms;
using Sprache;

namespace LtGt.Internal.Selectors
{
    internal static class SelectorGrammar
    {
        /* Special character */

        private static readonly Parser<char> SpecialCharacter =
            Parse.Chars(' ', '.', '#', ':', '[', ']', '(', ')', '>', '+', '~', '*', '^', '$', '|', '=');

        /* String comparison term */

        private static readonly Parser<StringComparisonTerm> StringComparisonTerm =
            Parse.Char('~').Return(new StringComparisonTerm(StringComparisonStrategy.ContainsWithinWhiteSpaceSeparated))
                .Or(Parse.Char('^').Return(new StringComparisonTerm(StringComparisonStrategy.StartsWith)))
                .Or(Parse.Char('$').Return(new StringComparisonTerm(StringComparisonStrategy.EndsWith)))
                .Or(Parse.Char('*').Return(new StringComparisonTerm(StringComparisonStrategy.Contains)))
                .Or(Parse.Char('|').Return(new StringComparisonTerm(StringComparisonStrategy.StartsWithHyphenSeparated)));

        /* Number composition term */

        // 2n+1
        private static readonly Parser<NumberCompositionTerm> FormulaWithConstantNumberCompositionTerm =
            from multiplierSign in Parse.Chars('+', '-').Once().Text().Optional()
            from multiplierAbs in Parse.Digit.AtLeastOnce().Text()
            from n in Parse.IgnoreCase('n')
            from constantSign in Parse.Chars('+', '-').Once().Text()
            from constantAbs in Parse.Digit.AtLeastOnce().Text()
            let multiplier = int.Parse(multiplierSign.GetOrDefault() + multiplierAbs, CultureInfo.InvariantCulture)
            let constant = int.Parse(constantSign + constantAbs, CultureInfo.InvariantCulture)
            select new NumberCompositionTerm(multiplier, constant);

        // 2n
        private static readonly Parser<NumberCompositionTerm> FormulaWithoutConstantNumberCompositionTerm =
            from multiplierSign in Parse.Chars('+', '-').Once().Text().Optional()
            from multiplierAbs in Parse.Digit.AtLeastOnce().Text()
            from n in Parse.IgnoreCase('n')
            let multiplier = int.Parse(multiplierSign.GetOrDefault() + multiplierAbs, CultureInfo.InvariantCulture)
            select new NumberCompositionTerm(multiplier);

        // 2
        private static readonly Parser<NumberCompositionTerm> ValueNumberCompositionTerm =
            from constantAbs in Parse.Digit.AtLeastOnce().Text()
            let constant = int.Parse(constantAbs, CultureInfo.InvariantCulture)
            select new NumberCompositionTerm(0, constant);

        // odd
        private static readonly Parser<NumberCompositionTerm> OddNumberCompositionTerm =
            Parse.IgnoreCase("odd").Return(new NumberCompositionTerm(2, 1));

        // even
        private static readonly Parser<NumberCompositionTerm> EvenNumberCompositionTerm =
            Parse.IgnoreCase("even").Return(new NumberCompositionTerm(2));

        private static readonly Parser<NumberCompositionTerm> NumberCompositionTerm =
            FormulaWithConstantNumberCompositionTerm
                .Or(FormulaWithoutConstantNumberCompositionTerm)
                .Or(ValueNumberCompositionTerm)
                .Or(OddNumberCompositionTerm)
                .Or(EvenNumberCompositionTerm);

        /* Any selector */

        private static readonly Parser<AnySelector> AnySelector = Parse.Char('*').Return(new AnySelector());

        /* Type selector */

        private static readonly Parser<TypeSelector> TypeSelector =
            Parse.LetterOrDigit.AtLeastOnce().Text().Select(n => new TypeSelector(n));

        /* Class name selector */

        private static readonly Parser<ClassNameSelector> ClassNameSelector =
            from dot in Parse.Char('.')
            from className in Parse.AnyChar.Except(SpecialCharacter).AtLeastOnce().Text()
            select new ClassNameSelector(className);

        /* Element ID selector */

        private static readonly Parser<IdSelector> IdSelector =
            from pound in Parse.Char('#')
            from id in Parse.AnyChar.Except(SpecialCharacter).AtLeastOnce().Text()
            select new IdSelector(id);

        /* Attribute selector */

        // [id="main"]
        private static readonly Parser<AttributeSelector> NormalAttributeSelector =
            from openBrace in Parse.Char('[')
            from name in Parse.AnyChar.Except(SpecialCharacter).AtLeastOnce().Text()
            from matchOperator in StringComparisonTerm.Optional().Select(o => o.GetOrElse(new StringComparisonTerm()))
            from eq in Parse.Char('=')
            from openQuote in Parse.Chars('"', '\'')
            from value in Parse.CharExcept(openQuote).Many().Text()
            from closeQuote in Parse.Char(openQuote)
            from closeBrace in Parse.Char(']')
            select new AttributeSelector(name, value, matchOperator);

        // [id]
        private static readonly Parser<AttributeSelector> ValuelessAttributeSelector =
            from open in Parse.Char('[')
            from name in Parse.AnyChar.Except(SpecialCharacter).AtLeastOnce().Text()
            from close in Parse.Char(']')
            select new AttributeSelector(name);

        private static readonly Parser<AttributeSelector> AttributeSelector =
            NormalAttributeSelector
                .Or(ValuelessAttributeSelector);

        /* Root selector */

        private static readonly Parser<RootSelector> RootSelector =
            Parse.IgnoreCase(":root").Return(new RootSelector());

        /* Child selectors */

        private static readonly Parser<OnlyChildSelector> OnlyChildSelector =
            Parse.IgnoreCase(":only-child").Return(new OnlyChildSelector());

        private static readonly Parser<FirstChildSelector> FirstChildSelector =
            Parse.IgnoreCase(":first-child").Return(new FirstChildSelector());

        private static readonly Parser<LastChildSelector> LastChildSelector =
            Parse.IgnoreCase(":last-child").Return(new LastChildSelector());

        private static readonly Parser<NthChildSelector> NthChildSelector =
            from name in Parse.IgnoreCase(":nth-child")
            from open in Parse.Char('(')
            from indexDescriptor in NumberCompositionTerm
            from close in Parse.Char(')')
            select new NthChildSelector(indexDescriptor);

        private static readonly Parser<NthLastChildSelector> NthLastChildSelector =
            from name in Parse.IgnoreCase(":nth-last-child")
            from open in Parse.Char('(')
            from indexDescriptor in NumberCompositionTerm
            from close in Parse.Char(')')
            select new NthLastChildSelector(indexDescriptor);

        /* Type selectors */

        private static readonly Parser<OnlyOfTypeSelector> OnlyOfTypeSelector =
            Parse.IgnoreCase(":only-of-type").Return(new OnlyOfTypeSelector());

        private static readonly Parser<FirstOfTypeSelector> FirstOfTypeSelector =
            Parse.IgnoreCase(":first-of-type").Return(new FirstOfTypeSelector());

        private static readonly Parser<LastOfTypeSelector> LastOfTypeSelector =
            Parse.IgnoreCase(":last-of-type").Return(new LastOfTypeSelector());

        private static readonly Parser<NthOfTypeSelector> NthOfTypeSelector =
            from name in Parse.IgnoreCase(":nth-of-type")
            from open in Parse.Char('(')
            from indexDescriptor in NumberCompositionTerm
            from close in Parse.Char(')')
            select new NthOfTypeSelector(indexDescriptor);

        private static readonly Parser<NthLastOfTypeSelector> NthLastOfTypeSelector =
            from name in Parse.IgnoreCase(":nth-last-of-type")
            from open in Parse.Char('(')
            from indexDescriptor in NumberCompositionTerm
            from close in Parse.Char(')')
            select new NthLastOfTypeSelector(indexDescriptor);

        /* Empty selector */

        private static readonly Parser<EmptySelector> EmptySelector =
            Parse.IgnoreCase(":empty").Return(new EmptySelector());

        /* Stand-alone selector */

        private static readonly Parser<Selector> StandaloneSelector =
            AnySelector
                .Or<Selector>(TypeSelector)
                .Or(ClassNameSelector)
                .Or(IdSelector)
                .Or(AttributeSelector)
                .Or(RootSelector)
                .Or(OnlyChildSelector)
                .Or(FirstChildSelector)
                .Or(LastChildSelector)
                .Or(OnlyOfTypeSelector)
                .Or(FirstOfTypeSelector)
                .Or(LastOfTypeSelector)
                .Or(NthChildSelector)
                .Or(NthLastChildSelector)
                .Or(NthOfTypeSelector)
                .Or(NthLastOfTypeSelector)
                .Or(EmptySelector);

        /* Not selector */

        private static readonly Parser<NotSelector> NotSelector =
            from name in Parse.IgnoreCase(":not")
            from open in Parse.Char('(')
            from targetSelector in GroupCombinator
            from close in Parse.Char(')')
            select new NotSelector(targetSelector);

        /* Group combinator */

        private static readonly Parser<GroupCombinator> GroupCombinator =
            StandaloneSelector
                .Or(NotSelector)
                .AtLeastOnce().Select(s => new GroupCombinator(s.ToArray()));

        /* Descendant combinator */

        private static readonly Parser<DescendantCombinator> DescendantCombinator =
            from parentSelector in GroupCombinator
            from space in Parse.WhiteSpace
            from targetSelector in GroupCombinator
            select new DescendantCombinator(parentSelector, targetSelector);

        /* Child combinator */

        private static readonly Parser<ChildCombinator> ChildCombinator =
            from parentSelector in GroupCombinator
            from gt in Parse.Char('>').Token()
            from targetSelector in GroupCombinator
            select new ChildCombinator(parentSelector, targetSelector);

        /* Sibling combinator */

        private static readonly Parser<SiblingCombinator> SiblingCombinator =
            from previousSelector in GroupCombinator
            from plus in Parse.Char('+').Token()
            from targetSelector in GroupCombinator
            select new SiblingCombinator(previousSelector, targetSelector);

        /* Subsequent sibling combinator */

        private static readonly Parser<SubsequentSiblingCombinator> SubsequentSiblingCombinator =
            from previousSelector in GroupCombinator
            from tilde in Parse.Char('~').Token()
            from targetSelector in GroupCombinator
            select new SubsequentSiblingCombinator(previousSelector, targetSelector);

        /* Selector */

        public static readonly Parser<Selector> Selector =
            DescendantCombinator
                .Or<Selector>(ChildCombinator)
                .Or(SiblingCombinator)
                .Or(SubsequentSiblingCombinator)
                .Or(GroupCombinator);
    }
}