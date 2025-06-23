Imports System.Globalization
Imports System.Numerics
Imports System.Runtime.CompilerServices

Public Module ComplexExtensions

    'Private Const CHARI As System.Char = "i"c
    Private Const CHARPLUS As System.Char = "+"c
    Private Const CHARMINUS As System.Char = "-"c
    'Private Const CHARUPPERE As System.Char = "E"c
    Private Const CHARSEMI As System.Char = ";"c
    'Private Const CHARSPACE As System.Char = " "c

    ' private const NumberStyles DefaultNumberStyle = NumberStyles.Float | NumberStyles.AllowThousands;
    ''' <summary>
    ''' The style to use in TryParseStandard() to provide support for
    ''' <see cref="System.Globalization.CultureInfo"/>s that include commas for
    ''' thousands.
    ''' </summary>
    Public Const COMPLEXSTYLE As System.Globalization.NumberStyles =
        NumberStyles.Float Or NumberStyles.AllowThousands

    ' private const NumberStyles InvalidNumberStyles = ~(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite
    '                                                  | NumberStyles.AllowLeadingSign | NumberStyles.AllowTrailingSign
    '                                                  | NumberStyles.AllowParentheses | NumberStyles.AllowDecimalPoint
    '                                                  | NumberStyles.AllowThousands | NumberStyles.AllowExponent
    '                                                  | NumberStyles.AllowCurrencySymbol | NumberStyles.AllowHexSpecifier);
    Private Const InvalidNumberStyles _
        As System.Globalization.NumberStyles = Not (
        NumberStyles.AllowLeadingWhite Or NumberStyles.AllowTrailingWhite Or
        NumberStyles.AllowLeadingSign Or NumberStyles.AllowTrailingSign Or
        NumberStyles.AllowParentheses Or NumberStyles.AllowDecimalPoint Or
        NumberStyles.AllowThousands Or NumberStyles.AllowExponent Or
        NumberStyles.AllowCurrencySymbol Or NumberStyles.AllowHexSpecifier)

    ''' <summary>
    ''' Specifies the style to use when converting a complex number to a
    ''' standard form representation.
    ''' </summary>
    ''' <remarks>
    ''' The default is <see cref="StandardizationStyle.ClosedAIB"/>.
    ''' </remarks>>
    <FlagsAttribute>
    Public Enum StandardizationStyle As System.Int32

        ''' <summary>
        ''' Use the A+iB sequence and the open form, without spaces before and
        ''' after the sign of the imaginary component.
        ''' </summary>
        ClosedAIB = 0

        ''' <summary>
        ''' Use the A+Bi sequence.
        ''' </summary>
        ABI = 1

        ''' <summary>
        ''' Use the open (A + iB) form, with spaces before and after the sign of
        ''' the imaginary component.
        ''' </summary>
        Open = 2

        ''' <summary>
        ''' Enforce the use of the designated sequence for parsing. If not set,
        ''' either sequence is allowed. Enforcement does not apply for
        ''' ToStandardString().
        ''' </summary>
        EnforceSequence = 4

        ''' <summary>
        ''' Enforce the use of the designated spacing for parsing. If not set,
        ''' either form is allowed. Enforcement does not apply for
        ''' ToStandardString().
        ''' </summary>
        EnforceSpacing = 8

    End Enum ' StandardizationStyle

    ''' <summary>
    ''' The default standard form is A+iB sequence without spaces, but no
    ''' enforcement of either option.
    ''' </summary>
    Public Const DefaultStandardizationStyle As StandardizationStyle =
        StandardizationStyle.ClosedAIB






    '' THIS WAS JUST EARLY TINKERING WITH STRUCTURAL QUESTIONS. IT IS A VERY
    '' RUDIMENTARY IMPLEMENTATION WITH A SINGLE FORMAT FOR THE RESULT.
    '''' <summary>
    '''' Returns a standard form string that represents the current object.
    '''' </summary>
    '''' <param name="complex">The complex number to convert.</param>
    '''' <returns>A string representation of the complex number in the form "A+iB" or "A-iB".</returns>
    '<Extension()>
    'Public Function ToStandardString(complex As System.Numerics.Complex)
    '    Return If(complex.Imaginary < 0.0,
    '        $"{complex.Real}-i{Math.Abs(complex.Imaginary)}",
    '        $"{complex.Real}+i{complex.Imaginary}")
    'End Function ' ToStandardString

End Module ' ComplexExtensions
