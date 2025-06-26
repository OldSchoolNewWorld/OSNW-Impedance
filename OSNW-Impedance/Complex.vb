Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization

''' <summary>
''' This module contains extension methods for the
''' <see cref="System.Numerics.Complex"/> structure, providing additional
''' functionality such as parsing and standard form string representation.
''' </summary>
''' <remarks>
''' The module is designed to extend the <see cref="System.Numerics.Complex"/>
''' structure with methods that allow for converting complex numbers to a
''' standard form string and parsing complex numbers from standard form strings.
''' It includes methods for both default and custom standardization styles,
''' allowing for flexibility in how complex numbers are represented as strings.
''' </remarks>
Public Module ComplexExtensions

    ' REF: Extension Methods not Recognized
    ' https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-extend-a-type-with-extension-methods
    ' The module needs to be specified as Public.

    Friend Const CHARI As System.Char = "i"c
    Friend Const CHARJ As System.Char = "j"c
    Private Const CHARPLUS As System.Char = "+"c
    Private Const CHARMINUS As System.Char = "-"c
    Private Const CHARUPPERE As System.Char = "E"c
    Private Const CHARSEMI As System.Char = ";"c
    Private Const CHARSPACE As System.Char = " "c

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
    ''' Used to specify the layout style to use when converting a complex number
    ''' to its standard form representation.
    ''' </summary>
    ''' <remarks>
    ''' The default is <see cref="StandardizationStyles.None"/>.
    ''' </remarks>>
    <System.FlagsAttribute>
    Public Enum StandardizationStyles As System.Int32

        ' REF: Enum Design
        ' https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/enum

        ''' <summary>
        ''' Default: Use the A+iB sequence and the closed form, without spaces
        ''' before and after the sign of the imaginary component. There is no
        ''' parsing enforcement unless EnforceSequence and/or EnforceSpacing are
        ''' set.
        ''' </summary>
        ''' <remarks>
        ''' This is the default value for the enumeration.
        ''' </remarks>
        None = 0

        ''' <summary>
        ''' Use the A+Bi sequence. There is no parsing enforcement unless
        ''' EnforceSequence is set.
        ''' </summary>
        ''' <remarks>
        ''' This is the default value for the enumeration.
        ''' </remarks>
        ABi = 1

        ''' <summary>
        ''' Use the open (A + iB or A + Bi) form, with spaces before and after
        ''' the sign of the imaginary component. There is no parsing enforcement unless
        ''' EnforceSpacing is set.
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

        ' Add some convenience/shorthand values.

        ''' <summary>
        ''' Use the A+iB sequence and the closed form, without spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless EnforceSequence and/or EnforceSpacing are set.
        ''' </summary>
        ClosedAiB = None ' 0

        ''' <summary>
        ''' Use the A+Bi sequence and the closed form, without spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless EnforceSequence and/or EnforceSpacing are set.
        ''' </summary>
        ClosedABi = ABi ' 1

        ''' <summary>
        ''' Use the A + iB sequence and the open form, with spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless EnforceSequence and/or EnforceSpacing are set.
        ''' </summary>
        OpenAiB = Open ' 2

        ''' <summary>
        ''' Use the A + Bi sequence and the open form, with spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless EnforceSequence and/or EnforceSpacing are set.
        ''' </summary>
        OpenABi = Open Or ABi ' 3

        ''' <summary>
        ''' EnforceBoth, when applied in general, enforces both the selected
        ''' sequence and closed/open form when parsing.
        ''' <c>EnforcedClosedAiB</c> is a shortcut that has the same value, and
        ''' is intended to enforce both the use of the A+iB sequence and
        ''' the closed form, without spaces before and after the sign of the
        ''' imaginary component when parsing.
        ''' </summary>
        EnforceBoth = EnforceSequence Or EnforceSpacing ' 12
        EnforcedClosedAiB = EnforceBoth ' 12

        ''' <summary>
        ''' Enforce both the use of the A+Bi sequence and the closed form,
        ''' without spaces before and after the sign of the imaginary component
        ''' when parsing.
        ''' </summary>
        EnforcedClosedABi = ClosedABi Or EnforceBoth ' 13

        ''' <summary>
        ''' Enforce both the use of the A + iB sequence and the open form, with
        ''' spaces before and after the sign of the imaginary component when
        ''' parsing.
        ''' </summary>
        EnforcedOpenAiB = OpenAiB Or EnforceBoth ' 14

        ''' <summary>
        ''' Enforce both the use of the A + Bi sequence and the open form, with
        ''' spaces before and after the sign of the imaginary component when
        ''' parsing.
        ''' </summary>
        EnforcedOpenABi = OpenABi Or EnforceBoth ' 15

    End Enum ' StandardizationStyle

    'Public Const DefaultStandardizationStyle As StandardizationStyle =
    '    StandardizationStyle.ClosedAIB
    ''' <summary>
    ''' The default standard form is A+iB sequence without spaces, but no
    ''' enforcement of either option.
    ''' </summary>
    Public Const DefaultStandardizationStyle As StandardizationStyles = Nothing

End Module ' ComplexExtensions
