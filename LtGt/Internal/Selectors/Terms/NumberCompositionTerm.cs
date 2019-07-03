namespace LtGt.Internal.Selectors.Terms
{
    internal class NumberCompositionTerm
    {
        public int Multiplier { get; }

        public int Constant { get; }

        public NumberCompositionTerm(int multiplier = 1, int constant = 0)
        {
            Multiplier = multiplier;
            Constant = constant;
        }

        public bool Check(int value) => (value - Constant) % Multiplier == 0;

        public override string ToString()
        {
            if (Multiplier != 1 && Constant > 0)
                return $"{Multiplier}n + {Constant}";

            if (Multiplier != 1 && Constant < 0)
                return $"{Multiplier}n - {-1 * Constant}";

            if (Multiplier != 1)
                return $"{Multiplier}n";

            if (Constant > 0)
                return $"n + {Constant}";

            if (Constant < 0)
                return $"n - {-1 * Constant}";

            return "n";
        }
    }
}