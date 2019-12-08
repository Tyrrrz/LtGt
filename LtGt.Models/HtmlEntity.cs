using System;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract entity in HTML document object model.
    /// </summary>
    public abstract partial class HtmlEntity
    {
        /// <summary>
        /// Clones this <see cref="HtmlEntity"/>.
        /// </summary>
        public abstract HtmlEntity Clone();
    }

#if !NETSTANDARD1_0
    public abstract partial class HtmlEntity : ICloneable
    {
        /// <inheritdoc />
        object ICloneable.Clone() => Clone();
    }
#endif

    public abstract partial class HtmlEntity
    {
        /// <summary>
        /// Comparer that can be used to compare values of two <see cref="HtmlEntity"/>s.
        /// </summary>
        public static HtmlEntityEqualityComparer EqualityComparer => HtmlEntityEqualityComparer.Default;
    }
}