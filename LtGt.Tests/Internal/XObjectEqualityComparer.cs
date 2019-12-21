using System.Collections.Generic;
using System.Xml.Linq;

namespace LtGt.Tests.Internal
{
    internal class XObjectEqualityComparer : IEqualityComparer<XObject>
    {
        public static XObjectEqualityComparer Instance { get; } = new XObjectEqualityComparer();

        public bool Equals(XObject x, XObject y)
        {
            if (x is null && y is null)
                return true;

            if (x is null)
                return false;

            if (x is XAttribute xa && y is XAttribute ya)
                return xa.Name == ya.Name && xa.Value == ya.Value;

            if (x is XNode xn && y is XNode yn)
                return XNode.EqualityComparer.Equals(xn, yn);

            return false;
        }

        public int GetHashCode(XObject obj)
        {
            if (obj is XAttribute attr)
                return attr.GetHashCode();

            if (obj is XNode node)
                return XNode.EqualityComparer.GetHashCode(node);

            return obj.GetHashCode();
        }
    }
}