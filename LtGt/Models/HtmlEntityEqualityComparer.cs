using System;
using System.Collections.Generic;
using System.Linq;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Comparer that can compare two <see cref="HtmlEntity"/>s for equality.
    /// </summary>
    public partial class HtmlEntityEqualityComparer : IEqualityComparer<HtmlEntity>
    {
        private bool Equals(HtmlDeclaration x, HtmlDeclaration y) =>
            StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name) &&
            StringComparer.OrdinalIgnoreCase.Equals(x.Value, y.Value);

        private bool Equals(HtmlAttribute x, HtmlAttribute y) =>
            StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name) &&
            StringComparer.Ordinal.Equals(x.Value, y.Value);

        private bool Equals(HtmlComment x, HtmlComment y) =>
            StringComparer.Ordinal.Equals(x.Value, y.Value);

        private bool Equals(HtmlText x, HtmlText y) =>
            StringComparer.Ordinal.Equals(x.Value, y.Value);

        private bool Equals(HtmlElement x, HtmlElement y) =>
            StringComparer.OrdinalIgnoreCase.Equals(x.Name, y.Name) &&
            x.Attributes.SequenceEqual(y.Attributes, this) &&
            x.Children.SequenceEqual(y.Children, this);

        private bool Equals(HtmlDocument x, HtmlDocument y) =>
            Equals(x.Declaration, y.Declaration) &&
            x.Children.SequenceEqual(y.Children, this);

        /// <inheritdoc />
        public bool Equals(HtmlEntity entity1, HtmlEntity entity2)
        {
            if (ReferenceEquals(entity1, entity2))
                return true;

            if (entity1 is null || entity2 is null)
                return false;

            if (entity1.GetType() != entity2.GetType())
                return false;

            if (entity1 is HtmlDeclaration declaration1 && entity2 is HtmlDeclaration declaration2)
                return Equals(declaration1, declaration2);

            if (entity1 is HtmlAttribute attribute1 && entity2 is HtmlAttribute attribute2)
                return Equals(attribute1, attribute2);

            if (entity1 is HtmlComment comment1 && entity2 is HtmlComment comment2)
                return Equals(comment1, comment2);

            if (entity1 is HtmlText text1 && entity2 is HtmlText text2)
                return Equals(text1, text2);

            if (entity1 is HtmlElement element1 && entity2 is HtmlElement element2)
                return Equals(element1, element2);

            if (entity1 is HtmlDocument document1 && entity2 is HtmlDocument document2)
                return Equals(document1, document2);

            throw new InvalidOperationException($"Unknown entity type [{entity1.GetType().Name}].");
        }

        private int GetHashCode(HtmlDeclaration declaration) => new HashCodeBuilder()
            .Add(declaration.Name, StringComparer.OrdinalIgnoreCase)
            .Add(declaration.Value, StringComparer.OrdinalIgnoreCase)
            .Build();

        private int GetHashCode(HtmlAttribute attribute) => new HashCodeBuilder()
            .Add(attribute.Name, StringComparer.OrdinalIgnoreCase)
            .Add(attribute.Value, StringComparer.Ordinal)
            .Build();

        private int GetHashCode(HtmlComment comment) => new HashCodeBuilder()
            .Add(comment.Value, StringComparer.Ordinal)
            .Build();

        private int GetHashCode(HtmlText text) => new HashCodeBuilder()
            .Add(text.Value, StringComparer.Ordinal)
            .Build();

        private int GetHashCode(HtmlElement element) => new HashCodeBuilder()
            .Add(element.Name, StringComparer.OrdinalIgnoreCase)
            .AddMany(element.Attributes, this)
            .AddMany(element.Children, this)
            .Build();

        private int GetHashCode(HtmlDocument document) => new HashCodeBuilder()
            .Add(document.Declaration, this)
            .AddMany(document.Children, this)
            .Build();

        /// <inheritdoc />
        public int GetHashCode(HtmlEntity obj)
        {
            obj.GuardNotNull(nameof(obj));

            if (obj is HtmlDeclaration declaration)
                return GetHashCode(declaration);

            if (obj is HtmlAttribute attribute)
                return GetHashCode(attribute);

            if (obj is HtmlComment comment)
                return GetHashCode(comment);

            if (obj is HtmlText text)
                return GetHashCode(text);

            if (obj is HtmlElement element)
                return GetHashCode(element);

            if (obj is HtmlDocument document)
                return GetHashCode(document);

            throw new InvalidOperationException($"Unknown entity type [{obj.GetType().Name}].");
        }
    }

    public partial class HtmlEntityEqualityComparer
    {
        /// <summary>
        /// Default instance of <see cref="HtmlEntityEqualityComparer"/>.
        /// </summary>
        public static HtmlEntityEqualityComparer Default { get; } = new HtmlEntityEqualityComparer();
    }
}