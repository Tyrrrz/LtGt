﻿using System;

namespace LtGt.Internal
{
    internal static class Guards
    {
        public static T GuardNotNull<T>(this T o, string argName = null) where T : class =>
            o ?? throw new ArgumentNullException(argName);
    }
}