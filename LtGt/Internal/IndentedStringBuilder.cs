using System.Text;

namespace LtGt.Internal
{
    internal class IndentedStringBuilder
    {
        private readonly StringBuilder _internalBuilder = new StringBuilder();

        public int Depth { get; private set; }

        public IndentedStringBuilder Append(string value)
        {
            _internalBuilder.Append(value);
            return this;
        }

        public IndentedStringBuilder Append(char value)
        {
            _internalBuilder.Append(value);
            return this;
        }

        public IndentedStringBuilder AppendLine()
        {
            _internalBuilder.AppendLine();
            _internalBuilder.Append(' ', Depth * 2);
            return this;
        }

        public IndentedStringBuilder IncreaseIndent()
        {
            Depth++;
            return this;
        }

        public IndentedStringBuilder DecreaseIndent()
        {
            Depth--;
            return this;
        }

        public override string ToString() => _internalBuilder.ToString();
    }
}