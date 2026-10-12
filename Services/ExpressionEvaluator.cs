
using System.Globalization;

namespace Make10Game.Services;

public static class ExpressionEvaluator
{
    public static bool TryEvaluate(
        string expression,
        int[] availableNumbers,
        out decimal result,
        out string error)
    {
        result = 0;
        error = "";

        if (string.IsNullOrWhiteSpace(expression))
        {
            error = "Expression cannot be empty.";
            return false;
        }

        try
        {
            var parser = new Parser(expression);
            result = parser.ParseExpression();

            if (!parser.IsAtEnd)
            {
                error = "Unable to parse part of the expression. Check the parentheses and operators.";
                return false;
            }

            var usedNumbers = parser.UsedNumbers;

            if (!SameNumbers(usedNumbers, availableNumbers))
            {
                error = "You must use each of the four provided numbers exactly once. Do not use any other numbers.";
                return false;
            }

            return true;
        }
        catch (DivideByZeroException)
        {
            error = "Division by zero is not allowed.";
            return false;
        }
        catch (FormatException)
        {
            error = "Invalid expression format. Example: (1 + 2) * (3 + 4)";
            return false;
        }
        catch (OverflowException)
        {
            error = "The calculation result exceeds the supported numeric range.";
            return false;
        }
    }

    private static bool SameNumbers(List<decimal> used, int[] available)
    {
        if (used.Count != available.Length)
            return false;

        var expected = available.Select(n => (decimal)n).OrderBy(n => n).ToArray();
        var actual = used.OrderBy(n => n).ToArray();

        return expected.SequenceEqual(actual);
    }

    private sealed class Parser
    {
        private readonly string _text;
        private int _position;

        public List<decimal> UsedNumbers { get; } = new();
        public bool IsAtEnd => _position >= _text.Length;

        public Parser(string text)
        {
            _text = text;
        }

        public decimal ParseExpression()
        {
            decimal value = ParseTerm();

            while (true)
            {
                SkipSpaces();

                if (Match('+'))
                    value += ParseTerm();
                else if (Match('-'))
                    value -= ParseTerm();
                else
                    return value;
            }
        }

        private decimal ParseTerm()
        {
            decimal value = ParseFactor();

            while (true)
            {
                SkipSpaces();

                if (Match('*'))
                    value *= ParseFactor();
                else if (Match('/'))
                    value /= ParseFactor();
                else
                    return value;
            }
        }

        private decimal ParseFactor()
        {
            SkipSpaces();

            if (Match('+'))
                return ParseFactor();

            if (Match('-'))
                return -ParseFactor();

            if (Match('('))
            {
                decimal value = ParseExpression();
                SkipSpaces();

                if (!Match(')'))
                    throw new FormatException();

                return value;
            }

            return ParseNumber();
        }

        private decimal ParseNumber()
        {
            SkipSpaces();
            int start = _position;

            while (_position < _text.Length &&
                   char.IsDigit(_text[_position]))
            {
                _position++;
            }

            if (start == _position)
                throw new FormatException();

            string token = _text[start.._position];

            if (!decimal.TryParse(
                    token,
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out decimal number))
            {
                throw new FormatException();
            }

            UsedNumbers.Add(number);
            return number;
        }

        private bool Match(char expected)
        {
            if (_position < _text.Length &&
                _text[_position] == expected)
            {
                _position++;
                return true;
            }

            return false;
        }

        private void SkipSpaces()
        {
            while (_position < _text.Length &&
                   char.IsWhiteSpace(_text[_position]))
            {
                _position++;
            }
        }
    }
}
