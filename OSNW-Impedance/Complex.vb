Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization

' REF: System.Numerics.Complex struct
' https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md
' https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-numerics-complex

' REF: Format a complex number
' https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-numerics-complex#format-a-complex-number

' REF: .NET Framework System.Numerics.Complex
' 1 https://github.com/microsoft/referencesource/blob/main/System.Numerics/System/Numerics/Complex.cs

' REF: .NET 8.0 System.Numerics.Complex
' https://learn.microsoft.com/en-us/dotnet/api/system.numerics.complex?view=net-8.0
' 2 https://github.com/dotnet/runtime/blob/5535e31a712343a63f5d7d796cd874e563e5ac14/src/libraries/System.Runtime.Numerics/src/System/Numerics/Complex.cs

' REF: .NET 9.0 System.Numerics.Complex
' https://learn.microsoft.com/en-us/dotnet/api/system.numerics.complex?view=net-9.0
' 3 https://github.com/dotnet/runtime/blob/9d5a6a9aa463d6d10b0b0ba6d5982cc82f363dc3/src/libraries/System.Runtime.Numerics/src/System/Numerics/Complex.cs

' REF: .NET 10.0 System.Numerics.Complex
' https://learn.microsoft.com/en-us/dotnet/api/system.numerics.complex?view=net-10.0
' 4 https://github.com/dotnet/dotnet/blob/c22dcd0c7a78d095a94d20e59ec0271b9924c82c/src/runtime/src/libraries/System.Runtime.Numerics/src/System/Numerics/Complex.cs

' The module needs to be specified as Public.
' REF: Extension Methods not Recognized
' https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-extend-a-type-with-extension-methods
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

    Private Const COMPLEXMINLEN As System.Int32 = 4 ' #+#i or #+i#
    Friend Const CHARI As System.Char = "i"c
    Friend Const CHARJ As System.Char = "j"c
    Private Const CHARPLUS As System.Char = "+"c
    Private Const CHARMINUS As System.Char = "-"c
    Private Const CHARUPPERE As System.Char = "E"c
    Private Const CHARSEMI As System.Char = ";"c
    Private Const CHARSPACE As System.Char = " "c

    ' REF: Mystery of The French Thousands Separator
    ' https://haacked.com/archive/2020/05/17/french-thousand-separator-mystery/
    ' Public allows use for unit testing.
    ''' <summary>
    ''' The narrow no-break space character, used in some cultures as a
    ''' thousands separator.
    ''' </summary>
    ''' <remarks>
    ''' This character is used in some cultures, such as French, as a
    ''' thousands separator. It is defined as Unicode character U+202F.
    ''' </remarks>
    Public Const CHARNNBSP As String = ChrW(&H202F) ' NARROW NO-BREAK SPACE

    ' private const NumberStyles DefaultNumberStyle = NumberStyles.Float | NumberStyles.AllowThousands;
    ''' <summary>
    ''' The numeric style to use in TryParseStandard() to provide support for
    ''' <see cref="System.Globalization.CultureInfo"/>s that include commas for
    ''' thousands.
    ''' </summary>
    Friend Const DEFAULTCOMPLEXNUMBERSTYLE As System.Globalization.NumberStyles =
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
    ''' The layout style to use when converting a complex number to its standard
    ''' form representation.
    ''' </summary>
    ''' <remarks>
    ''' The default is <c>StandardizationStyles.None</c>.
    ''' </remarks>>
    <System.FlagsAttribute>
    Public Enum StandardizationStyles As System.Int32

        ' REF: Enum Design
        ' https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/enum

        ''' <summary>
        ''' Default: Use the A+Bi sequence and the closed form, without spaces
        ''' before and after the sign of the imaginary component. There is no
        ''' parsing enforcement unless EnforceSequence and/or EnforceSpacing are
        ''' set.
        ''' </summary>
        ''' <remarks>
        ''' This is the default value for the enumeration.
        ''' </remarks>
        None = 0

        ''' <summary>
        ''' Use the A+iB sequence. There is no parsing enforcement unless
        ''' EnforceSequence is set.
        ''' </summary>
        ''' <remarks>
        ''' This is the default value for the enumeration.
        ''' </remarks>
        AiB = 1

        ''' <summary>
        ''' Use the open (A + Bi or A + iB) form, with spaces before and after
        ''' the sign of the imaginary component. There is no parsing enforcement
        ''' unless <c>EnforceSpacing</c> is set.
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
        ''' Use the A+Bi sequence and the closed form, without spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless <c>EnforceSequence</c> and/or
        ''' <c>EnforceSpacing</c> are set.
        ''' </summary>
        ClosedABi = None ' 0

        ''' <summary>
        ''' Use the A+iB sequence and the closed form, without spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless <c>EnforceSequence</c> and/or
        ''' <c>EnforceSpacing</c> are set.
        ''' </summary>
        ClosedAiB = AiB ' 1

        ''' <summary>
        ''' Use the A + Bi sequence and the open form, with spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless <c>EnforceSequence</c> and/or
        ''' <c>EnforceSpacing</c> are set.
        ''' </summary>
        OpenABi = Open ' 2

        ''' <summary>
        ''' Use the A + iB sequence and the open form, with spaces before and
        ''' after the sign of the imaginary component. There is no parsing
        ''' enforcement unless <c>EnforceSequence</c> and/or
        ''' <c>EnforceSpacing</c> are set.
        ''' </summary>
        OpenAiB = Open Or AiB ' 3

        ''' <summary>
        ''' EnforceBoth, when applied in general, enforces both the selected
        ''' sequence and closed/open form when parsing.
        ''' <c>EnforcedClosedABi</c> is a shortcut that has the same value, and
        ''' is intended to enforce both the use of the A+Bi sequence and
        ''' the closed form, without spaces before and after the sign of the
        ''' imaginary component when parsing.
        ''' </summary>
        EnforceBoth = EnforceSequence Or EnforceSpacing ' 12
        EnforcedClosedABi = EnforceBoth ' 12

        ''' <summary>
        ''' Enforce both the use of the A+Bi sequence and the closed form,
        ''' without spaces before and after the sign of the imaginary component
        ''' when parsing.
        ''' </summary>
        EnforcedClosedAiB = ClosedAiB Or EnforceBoth ' 13

        ''' <summary>
        ''' Enforce both the use of the A + Bi sequence and the open form, with
        ''' spaces before and after the sign of the imaginary component when
        ''' parsing.
        ''' </summary>
        EnforcedOpenABi = OpenABi Or EnforceBoth ' 14

        ''' <summary>
        ''' Enforce both the use of the A + iB sequence and the open form, with
        ''' spaces before and after the sign of the imaginary component when
        ''' parsing.
        ''' </summary>
        EnforcedOpenAiB = OpenAiB Or EnforceBoth ' 15

    End Enum ' StandardizationStyles

    '''' <summary>
    '''' The default standard form is A+Bi sequence without spaces, but with no
    '''' enforcement of either option.
    '''' </summary>
    Private Const DEFAULTSTANDARDIZATIONSTYLE As StandardizationStyles =
        StandardizationStyles.None

    ' Some cultures use a comma as a decimal, or as a thousands, separator.
    ' French may include narrow no-break space as a thousands separator.
    ' The open form includes spaces.
    Private Function GetValidComplexChars() As System.String
        Const VALIDCOMPLEXCHARS As System.String = "1234567890.+-iEe ,"
        Return VALIDCOMPLEXCHARS & CHARNNBSP ' (narrow no-break space)
    End Function ' GetValidComplexChars

End Module ' ComplexExtensions
