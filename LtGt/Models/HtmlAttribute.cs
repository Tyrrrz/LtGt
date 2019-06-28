using System;
using LtGt.Internal;

namespace LtGt.Models
{
    /// <summary>
    /// Represents an attribute entity in HTML document object model.
    /// </summary>
    public class HtmlAttribute : HtmlEntity, IEquatable<HtmlAttribute>
    {
        /// <summary>
        /// Name of this attribute node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Value of this attribute node.
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute(string name, string value)
        {
            Name = name.GuardNotNull(nameof(name));
            Value = value; // value can be null
        }

        /// <summary>
        /// Initializes an instance of <see cref="HtmlAttribute"/>.
        /// </summary>
        public HtmlAttribute(string name)
            : this(name, null)
        {
        }

        /// <inheritdoc />
        public bool Equals(HtmlAttribute other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return string.Equals(Name, other.Name) &&
                   string.Equals(Value, other.Value);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((HtmlAttribute) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode() => HashCodeBuilder.Combine(Name, Value);

        /// <inheritdoc />
        public override string ToString() => !Value.IsNullOrWhiteSpace() ? $"{Name}=\"{Value}\"" : Name;
    }
}