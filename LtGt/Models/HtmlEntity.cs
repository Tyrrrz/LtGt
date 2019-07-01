namespace LtGt.Models
{
    /// <summary>
    /// Represents an abstract entity in HTML document object model.
    /// </summary>
    public abstract class HtmlEntity
    {
        /// <summary>
        /// Comparer that can be used to compare values of two <see cref="HtmlEntity"/>s.
        /// </summary>
        public static HtmlEntityEqualityComparer EqualityComparer => HtmlEntityEqualityComparer.Default;
    }
}