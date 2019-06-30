namespace LtGt.Internal
{
    internal static class HashCode
    {
        public static int Combine(params object[] objects)
        {
            unchecked
            {
                var code = 17;

                foreach (var o in objects)
                    code = code * 23 + (o?.GetHashCode() ?? 0);

                return code;
            }
        }
    }
}