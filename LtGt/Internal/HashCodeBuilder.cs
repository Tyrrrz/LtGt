using System.Collections.Generic;
using System.Linq;

namespace LtGt.Internal
{
    internal partial class HashCodeBuilder
    {
        private int _code = 17;

        public HashCodeBuilder Add(int hashCode)
        {
            unchecked
            {
                _code = _code * 23 + hashCode;
            }

            return this;
        }

        public HashCodeBuilder Add(IEnumerable<int> hashCodes)
        {
            foreach (var hashCode in hashCodes)
                Add(hashCode);

            return this;
        }

        public HashCodeBuilder Add<T>(T obj, IEqualityComparer<T> comparer) =>
            Add(GetHashCodeSafe(obj, comparer));

        public HashCodeBuilder AddMany<T>(IEnumerable<T> objs, IEqualityComparer<T> comparer) =>
            Add(objs.Select(o => GetHashCodeSafe(o, comparer)));

        public int Build() => _code;
    }

    internal partial class HashCodeBuilder
    {
        private static int GetHashCodeSafe<T>(T obj, IEqualityComparer<T> comparer) => obj != null ? comparer.GetHashCode(obj) : 0;
    }
}