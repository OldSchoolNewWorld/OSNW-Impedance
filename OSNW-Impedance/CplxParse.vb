Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Runtime.InteropServices

Partial Public Module ComplexExtensions

    ' As of when recorded, these Complex signatures match in .NET 8.0 and
    '   .NET 9.0.
    '
    '   public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);
    '   public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);

    ' For these emulations,
    '   Examine the string for a valid standard form.
    '   Extract the component strings.
    '   Create a string matching Complex.ToString().
    '   Use Complex.TryParse() to parse the string.

End Module ' ComplexExtensions
