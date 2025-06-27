Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Diagnostics.CodeAnalysis

''' <summary>
''' Represents an electrical impedance with resistance (R) and reactance (X).
''' An electrical Impedance (Z) is a number of the standard form Z=R+jX or R+Xj,
''' where R and X are real numbers, and j is the imaginary unit, with the
''' property j^2 = -1.
''' </summary>
Public Structure Impedance
    '    Implements IEquatable(Of Impedance)

    ' DEV: An Impedance is essentially a complex number with some cosmetic
    ' differences:
    '   'i' is replaced by 'j' in the standard form.
    '   The Real component is represented by Resistance.
    '   The Imaginary component is represented by Reactance.
    ' Since System.Numerics.Complex is represented as a structure, it cannot be
    ' inherited. Given that, Impedance is created as a structure which uses
    ' familiar terminology but relies on Complex for most of its work.

    ' Use the "has a ..." approach to expose the desired features of a
    ' System.Numerics.Complex.
    ' Do not rename (binary serialization). ??????????????????????????????
    ''' <summary>
    ''' Gets the Impedance as a complex number.
    ''' </summary>
    Private ReadOnly AsComplex As System.Numerics.Complex

    ''' <summary>
    ''' Gets the resistance component of the Impedance.
    ''' </summary>
    Public ReadOnly Property Resistance As System.Double
        Get
            Return Me.AsComplex.Real
        End Get
    End Property

    ''' <summary>
    ''' Gets the reactance component of the Impedance.
    ''' </summary>
    Public ReadOnly Property Reactance As System.Double
        Get
            Return Me.AsComplex.Imaginary
        End Get
    End Property

    ''' <summary>
    ''' Initializes a new instance of the Impedance structure using the
    ''' specified resistance (R) and reactance (X) values.
    ''' </summary>
    ''' <param name="r">Specifies the value of the resistance component of the
    ''' Impedance.</param>
    ''' <param name="x">Specifies the value of the reactance component of the
    ''' Impedance.</param>
    Public Sub New(r As System.Double, x As System.Double)
        Me.AsComplex = New System.Numerics.Complex(r, x)
    End Sub

    'Public Function ToComplex() As System.Numerics.Complex
    '    Return New System.Numerics.Complex(Me.Resistance, Me.Reactance)
    'End Function

    'Public Overloads Function Equals(other As Impedance) As System.Boolean
    '    Implements IEquatable(Of Impedance).Equals

    '    '        Return Resistance.Equals(other.Resistance) AndAlso Reactance.Equals(other.Reactance)
    '    Return Me.AsComplex.Equals(other.ToComplex())
    'End Function

#Region "ToString Implementations"

    ' As of when recorded, System.Numerics.Complex in .NET 8.0 and .NET 9.0 have
    ' the implementations below.
    '   They optionally specify a format string, an IFormatProvider, or both.
    '   All cases eventually call the full case, which assigns defaults as
    '     needed.
    '   Create similar extensions herein.

    ' public override string ToString() => ToString(null, null);
    ' public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => ToString(format, null);
    ' public string ToString(IFormatProvider? provider) => ToString(null, provider);
    ' public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider)
    ' {
    '     // $"<{m_real.ToString(format, provider)}; {m_imaginary.ToString(format, provider)}>";
    '     var handler = new DefaultInterpolatedStringHandler(4, 2, provider, stackalloc char[512]);
    '     handler.AppendLiteral("<");
    '     handler.AppendFormatted(m_real, format);
    '     handler.AppendLiteral("; ");
    '     handler.AppendFormatted(m_imaginary, format);
    '     handler.AppendLiteral(">");
    '     return handler.ToStringAndClear();
    ' }

    '    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider)
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent string
    ''' representation in Cartesian form by using the specified numeric format
    ''' and culture-specific format information for its resistance and reactance
    ''' parts.
    ''' </summary>
    ''' <param name="format">A standard or custom numeric format string.</param>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information.</param>
    ''' <returns>The current Impedance expressed in Cartesian form.</returns>
    Public Overloads Function ToString(
        <StringSyntax(
            System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat)>
            ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As System.String

        Return Me.AsComplex.ToString(format, provider).Replace(CHARI, CHARJ)
    End Function ' ToString

    '    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent string
    ''' representation in Cartesian form by using the specified numeric format
    ''' information, and using the default culture-specific format information,
    ''' for its resistance and reactance parts.
    ''' </summary>
    ''' <param name="format">A standard or custom numeric format string.</param>
    ''' <returns>The current Impedance expressed in Cartesian form.</returns>
    Public Overloads Function ToString(
        <StringSyntax(StringSyntaxAttribute.NumericFormat)>
            ByVal format As System.String) _
        As System.String

        Return Me.AsComplex.ToString(format).Replace(CHARI, CHARJ)
    End Function ' ToString

    '    public string ToString(IFormatProvider? provider)
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent string
    ''' representation in Cartesian form by using the specified culture-specific
    ''' format information, and using the default numeric format, for its
    ''' resistance and reactance parts.
    ''' </summary>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information.</param>
    ''' <returns>The current Impedance expressed in Cartesian form.</returns>
    Public Overloads Function ToString(
        ByVal provider As System.IFormatProvider) As System.String

        Return Me.AsComplex.ToString(provider).Replace(CHARI, CHARJ)
    End Function ' ToString

    '    public override string ToString()
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent string
    ''' representation in Cartesian form by using the default numeric format and
    ''' culture-specific format information for its resistance and reactance
    ''' parts.
    ''' </summary>
    ''' <returns>The current Impedance expressed in Cartesian form.</returns>
    Public Overrides Function ToString() As System.String
        Return Me.AsComplex.ToString().Replace(CHARI, CHARJ)
    End Function ' ToString

#End Region ' "ToString Implementations"

#Region "ToStandardString Implementations"

    ' System.Numerics.Complex in .NET 8.0 and .NET 9.0 have these:
    '   They optionally specify a format string, an IFormatProvider, or both.
    '   All cases eventually call the full case, which will assign defaults as
    '   needed.
    '   Create similar extensions below.

    ' public override string ToString() => ToString(null, null);
    ' public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => ToString(format, null);
    ' public string ToString(IFormatProvider? provider) => ToString(null, provider);
    ' public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider)
    ' {
    '     // $"<{m_real.ToString(format, provider)}; {m_imaginary.ToString(format, provider)}>";
    '     var handler = new DefaultInterpolatedStringHandler(4, 2, provider, stackalloc char[512]);
    '     handler.AppendLiteral("<");
    '     handler.AppendFormatted(m_real, format);
    '     handler.AppendLiteral("; ");
    '     handler.AppendFormatted(m_imaginary, format);
    '     handler.AppendLiteral(">");
    '     return handler.ToStringAndClear();
    ' }

    '    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider)
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent
    ''' string representation in standard form by using the specified layout
    ''' format, numeric format, and culture-specific format information for its
    ''' real and imaginary parts.
    ''' </summary>
    ''' <param name="standardizationStyle">Specifies the 
    ''' <see cref="StandardizationStyles"/> to be used to generate the standard
    ''' form string.</param>
    ''' <param name="format">A standard or custom numeric format
    ''' string.</param>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information.</param>
    ''' <returns>The current Impedance expressed in standard form.</returns>
    Public Function ToStandardString(
        ByVal standardizationStyle As StandardizationStyles,
        <StringSyntax(
            System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat)>
            ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As System.String

        Return Me.AsComplex.ToStandardString(standardizationStyle, format,
                                           provider).Replace(CHARI, CHARJ)
    End Function ' ToStandardString

    '    public override string ToString()
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent
    ''' string representation in standard form by using the specified layout
    ''' format information, and using the default numeric format and
    ''' culture-specific format for its real and imaginary parts.
    ''' </summary>
    ''' <param name="standardizationStyle">Specifies the 
    ''' <see cref="StandardizationStyles"/> to be used to generate the standard
    ''' form string.</param>
    ''' <returns>The current Impedance expressed in standard form.</returns>
    Public Function ToStandardString(
        ByVal standardizationStyle As StandardizationStyles) _
        As System.String

        Return Me.AsComplex.ToStandardString(
            standardizationStyle).Replace(CHARI, CHARJ)
    End Function ' ToStandardString

    '    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent
    ''' string representation in standard form by using the specified layout
    ''' format and numeric format information, and using the default
    ''' culture-specific format information, for its real and imaginary parts.
    ''' </summary>
    ''' <param name="standardizationStyle">Specifies the 
    ''' <see cref="StandardizationStyles"/> to be used to generate the standard
    ''' form string.</param>
    ''' <param name="format">A standard or custom numeric format string.</param>
    ''' <returns>The current Impedance expressed in standard form.</returns>
    Public Function ToStandardString(
        ByVal standardizationStyle As StandardizationStyles,
        <StringSyntax(StringSyntaxAttribute.NumericFormat)>
            ByVal format As System.String) _
        As System.String

        Return Me.AsComplex.ToStandardString(standardizationStyle,
                                           format).Replace(CHARI, CHARJ)
    End Function ' ToStandardString

    '    public string ToString(IFormatProvider? provider)
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent
    ''' string representation in standard form by using the specified layout
    ''' format and culture-specific format information, and using the
    ''' default numeric format, for its real and imaginary parts.
    ''' </summary>
    ''' <param name="standardizationStyle">Specifies the 
    ''' <see cref="StandardizationStyles"/> to be used to generate the standard
    ''' form string.</param>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information.</param>
    ''' <returns>The current Impedance expressed in standard form.</returns>
    Public Function ToStandardString(
        ByVal standardizationStyle As StandardizationStyles,
        ByVal provider As System.IFormatProvider) _
        As System.String

        Return Me.AsComplex.ToStandardString(standardizationStyle,
                                           provider).Replace(CHARI, CHARJ)
    End Function ' ToStandardString

    '    public override string ToString()
    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent
    ''' string representation in standard form by using the default layout
    ''' format, numeric format, and culture-specific format information for its
    ''' real and imaginary parts.
    ''' </summary>
    ''' <returns>The current Impedance expressed in standard form.</returns>
    Public Function ToStandardString() As System.String
        Return Me.AsComplex.ToStandardString().Replace(CHARI, CHARJ)
    End Function ' ToStandardString

#End Region ' "ToStandardString Implementations"

#Region "TryFormat Implementations"

#Region "Parsing Implementations"

    ' As of when recorded, these Complex signatures match in .NET 8.0 and
    '   .NET 9.0.
    '
    '   public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);
    '   public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);

    ' public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex result)
    ' public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex result)
    ''' <summary>
    ''' Attempts to convert the standard form representation of an Impedance
    ''' to its <see cref="OSNW.Numerics.Impedance"/> equivalent using the
    ''' specified layout format, numeric format, and culture-specific format
    ''' information.
    ''' </summary>
    ''' <param name="s">Specifies the standard form string to be parsed.</param>
    ''' <param name="standardizationStyle">Specifies the layout formats
    ''' permitted in numeric string arguments that are passed to the TryParse
    ''' method of the <c>OSNW.Numerics.Impedance</c> numeric type.</param>
    ''' <param name="style">Determines the styles permitted in numeric string
    ''' arguments that are passed to the TryParse method of the
    ''' <c>OSNW.Numerics.Impedance</c> numeric type.</param>
    ''' <param name="provider">Provides a mechanism for retrieving an object to
    ''' control formatting.</param>
    ''' <param name="result">Returns the <c>OSNW.Numerics.Impedance</c>
    ''' represented by <paramref name="s"/>.</param>
    ''' <returns>Returns <c>True</c> if the conversion succeeds; otherwise,
    ''' <c>False</c>.</returns>
    Public Shared Function TryParseStandard(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal s As System.String,
        ByVal standardizationStyle As StandardizationStyles,
        ByVal style As System.Globalization.NumberStyles,
        ByVal provider As System.IFormatProvider,
        ByRef result As OSNW.Numerics.Impedance) _
        As System.Boolean

        Dim Cplx As New System.Numerics.Complex
        If OSNW.Numerics.ComplexExtensions.TryParseStandard(
            s.Replace(CHARJ, CHARI), standardizationStyle, style, provider,
            Cplx) Then

            result = New Impedance(Cplx.Real, Cplx.Imaginary)
            Return True
        Else
            result = New OSNW.Numerics.Impedance
            Return False
        End If

    End Function ' TryParseStandard

    ' public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result)
    ' public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result)
    ''' <summary>
    ''' Attempts to convert the standard form representation of an Impedance
    ''' to its <see cref="OSNW.Numerics.Impedance"/> equivalent using the
    ''' specified layout format and culture-specific format information.
    ''' </summary>
    ''' <param name="s">Specifies the standard form string to be parsed.</param>
    ''' <param name="standardizationStyle">Specifies the layout formats
    ''' permitted in numeric string arguments that are passed to the TryParse
    ''' method of the <c>OSNW.Numerics.Impedance</c> numeric type.</param>
    ''' <param name="provider">Provides a mechanism for retrieving an object to
    ''' control formatting.</param>
    ''' <param name="result">Returns the <c>OSNW.Numerics.Impedance</c>
    ''' represented by <paramref name="s"/>.</param>
    ''' <returns>Returns <c>True</c> if the conversion succeeds; otherwise,
    ''' <c>False</c>.</returns>
    Public Shared Function TryParseStandard(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal s As System.String,
        ByVal standardizationStyle As StandardizationStyles,
        ByVal provider As System.IFormatProvider,
        ByRef result As OSNW.Numerics.Impedance) _
        As System.Boolean

        Return TryParseStandard(
            s, standardizationStyle, DEFAULTCOMPLEXSTYLE, provider, result)
    End Function ' TryParseStandard

#End Region ' "Parsing Implementations"

#End Region ' "TryFormat Implementations"

End Structure ' Impedance
