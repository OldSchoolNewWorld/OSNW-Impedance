﻿'TODO:
' Can a tuning design be selected based solely on an impedance, without consideration
' of capacitance, inductance, or frequency?
'   It would not find capacitance/inductance values but maybe it would have the
'   ability to determine the reactance values needed, which could then be used
'   to select the component values for a specified frequency.
' Reject infinity for admittance and susceptance?
' Finish AddShuntImpedance() and AddParallelAdmittance() when Admittance is
'   accessible.
' Add AddSeriesAdmittance() when Admittance is accessible?????
' Add De/Serialization to Admittance?????
'   "the strings should be generated and parsed by using the conventions of the invariant culture."
'   REF: Serialize and deserialize numeric data
'   https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-globalization-numberformatinfo#serialize-and-deserialize-numeric-data
' Add tests of failures for bad inputs.
' Allow both i and j to match the .NET result? Add tests for both i and j.
'   Wait, where does .NET indicate anything about allowing "j" for Complex aside from "Format a complex
'     number"? Complex only has ToString() and TryFormat() - nothing about standard form.
' Provide for conversion to arbtitrary impedances (Rtarget, Xtarget) vs. only characteristic impedances?

Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Diagnostics.CodeAnalysis
Imports System.Globalization
Imports System.Text.Json.Serialization

' REF: A Practical Introduction to Impedance Matching.
' https://picture.iczhiku.com/resource/eetop/shkgQUqJkAUQZBXx.pdf

' REF: Electrical impedance
' https://en.wikipedia.org/wiki/Electrical_impedance

' REF: Standing wave ratio
' https://en.wikipedia.org/wiki/Standing_wave_ratio

' REF: Impedance matching
' https://en.wikipedia.org/wiki/Impedance_matching

' Spread Spectrum Scene
' http://www.sss-mag.com/smith.html


' FROM OLD YTT CODE AND .NET Source:
'    <SerializableAttribute()>
''' <summary>
''' Represents an electrical impedance with resistance (R) and reactance (X).
''' An electrical impedance (Z) is a number of the standard form Z=R+Xj or
''' Z=R+jX, where:
'''   Z is the impedance (ohms);
'''   R is the resistance (ohms);
'''   X is the reactance (ohms); and
'''   j^2 = −1, the imaginary unit.
''' </summary>
Public Structure Impedance
    Implements IEquatable(Of Impedance), IFormattable
    ' BASED ON .NET SOURCE:
    ' Implements IEquatable(Of Impedance),
    '     IFormattable,
    '     INumberBase(Of Impedance),
    '     ISignedNumber(Of Impedance),
    '     IUtf8SpanFormattable
    ' FROM OLD YTT CODE:
    '        Implements IEquatable(Of Ytt.Util.Electrical.Impedance), IFormattable

    ' DEV: An Impedance is essentially a complex number with some cosmetic
    ' differences:
    '   'i' is replaced by 'j' in the standard form.
    '   The Real component is represented by Resistance.
    '   The Imaginary component is represented by Reactance.
    ' Since System.Numerics.Complex is represented as a structure, it cannot be
    ' inherited. Given that, Impedance is created as a structure which uses
    ' familiar terminology but relies on Complex for most of its work.

    Const MSGCHNV As System.String = "Cannot have a negative value."
    Const MSGCHZV As System.String = "Cannot have a zero value."
    Const MSGCHIV As System.String = "Cannot have an infinite value."
    '    Const MSGVMBGTZ As System.String = "Must be a positive, non-zero value."
    Const MSGNOSTR As System.String = "Cannot be Null/Nothing."

#Region "Fields and Properties"

    ''' <summary>
    ''' Gets the resistance (R) component, in ohms, of the current instance.
    ''' </summary>
    Private ReadOnly m_Resistance As System.Double

    ''' <summary>
    ''' Gets the reactance (X) component, in ohms, of the current instance.
    ''' </summary>
    Private ReadOnly m_Reactance As System.Double

    ''' <summary>
    ''' Gets the resistance (R) component, in ohms, of the current instance.
    ''' </summary>
    Public ReadOnly Property Resistance As System.Double
        ' Do not rename (binary serialization).
        Get
            Return Me.m_Resistance
        End Get
    End Property

    ''' <summary>
    ''' Gets the reactance (X) component, in ohms, of the current instance.
    ''' </summary>
    Public ReadOnly Property Reactance As System.Double
        ' Do not rename (binary serialization).
        Get
            Return Me.m_Reactance
        End Get
    End Property

#End Region ' "Fields and Properties"

    ''' <summary>
    '''  This is for some internal conveniences. It provides easy access to
    '''  <c>Complex</c> functionality.
    ''' </summary>
    ''' <returns>A new instance of the <see cref="System.Numerics.Complex"/>
    ''' structure.</returns>
    Friend Function ToComplex() As System.Numerics.Complex
        Return New System.Numerics.Complex(Me.Resistance, Me.Reactance)
    End Function

    ''' <summary>
    ''' Returns a value that represents this instance as its equivalent
    ''' <see cref="Admittance"/>.
    ''' </summary>
    ''' <returns>The equivalent <c>Admittance</c> value of the current
    ''' instance.</returns>
    Public Function ToAdmittance() As Admittance
        Dim ComplexRecip As System.Numerics.Complex =
            System.Numerics.Complex.Reciprocal(Me.ToComplex())
        Return New Admittance(ComplexRecip.Real, ComplexRecip.Imaginary)
    End Function

#Region "System.ValueType Implementations"

    ' public override bool Equals([NotNullWhen(true)] object? obj)
    ' {
    '     return obj is Complex other && Equals(other);
    ' }
    ''' <summary>
    ''' Determines whether the specified object is equal to the current object.
    ''' </summary>
    ''' <param name="obj">The object to compare with the current object.</param>
    ''' <returns><c>True</c> if the specified object is equal to the current
    ''' object; otherwise, <c>False</c>.</returns>
    Public Overrides Function Equals(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal obj As System.Object) As System.Boolean

        '' This may have to be changed to determine equality within some
        '' reasonable bounds. See 
        '' <see href="https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers"/>
        Return (TypeOf obj Is Impedance) AndAlso
            DirectCast(Me, IEquatable(Of Impedance)).Equals(
            DirectCast(obj, Impedance))
    End Function ' Equals

    ' public bool Equals(Complex value)
    ' {
    '     return m_real.Equals(value.m_real) && m_imaginary.Equals(value.m_imaginary);
    ' }
    ''' <summary>
    ''' Returns a value that indicates whether this instance and the specified
    ''' <see cref="Impedance"/> have the same component values.
    ''' </summary>
    ''' <param name="value">The <c>Impedance</c> to compare.</param>
    ''' <returns><c>True</c> if this instance and a specified <c>Impedance</c>
    ''' have the same component values.</returns>
    Public Overloads Function Equals(ByVal value As Impedance) _
        As System.Boolean _
        Implements System.IEquatable(Of Impedance).Equals

        ' This may have to be changed to determine equality within some
        ' reasonable bounds. See 
        ' <see href="https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers"/>
        Return Me.ToComplex().Equals(value.ToComplex())
    End Function ' Equals

    ''' <summary>
    ''' Serves as the default hash function.
    ''' </summary>
    ''' <returns>A hash code for the current object.</returns>
    Public Overrides Function GetHashCode() As System.Int32
        Return Me.ToComplex.GetHashCode
    End Function

#End Region ' "System.ValueType Implementations"

#Region "Operator Implementations"

    ''' <summary>
    ''' Returns a value that indicates whether two <c>Impedance</c>s are equal.
    ''' </summary>
    ''' <param name="left">The first <c>Impedance</c> to compare.</param>
    ''' <param name="right">The second <c>Impedance</c> to compare.</param>
    ''' <returns><c>True</c>> if the left and right parameters have the same
    ''' value; otherwise, <c>False</c>.</returns>
    Public Shared Operator =(ByVal left As Impedance,
                             ByVal right As Impedance) As System.Boolean
        Return left.Equals(right)
    End Operator

    ''' <summary>
    ''' Returns a value that indicates whether two <c>Impedance</c>s are not
    ''' equal.
    ''' </summary>
    ''' <param name="left">The first <c>Impedance</c> to compare.</param>
    ''' <param name="right">The second <c>Impedance</c> to compare.</param>
    ''' <returns><c>True</c>> if left and right are not equal; otherwise,
    ''' <c>False</c>.</returns>
    Public Shared Operator <>(ByVal left As Impedance,
                              ByVal right As Impedance) As System.Boolean
        Return Not left = right
    End Operator

    ''' <summary>
    ''' Returns the result of the addition of two <c>Impedance</c>s.
    ''' </summary>
    ''' <param name="impedance1">The first <c>Impedance</c> to add.</param>
    ''' <param name="impedance2">The first <c>Impedance</c> to add.</param>
    ''' <returns>The result of the addition.</returns>
    Public Shared Operator +(impedance1 As Impedance,
                             impedance2 As Impedance) As Impedance
        ' No input checking. impedance1 and impedance2 are presumed to have been
        ' checked when created.
        Dim TotalC As System.Numerics.Complex =
            impedance1.ToComplex + impedance2.ToComplex
        Return New Impedance(TotalC.Real, TotalC.Imaginary)
    End Operator

#End Region ' "Operator Implementations"

#Region "TryFormat Implementations"

    '
    '
    ' Are these needed ??????????????
    '
    '

#End Region ' "TryFormat Implementations"

#Region "ToString Implementations"

    ' As of when recorded, System.Numerics.Complex in .NET 8.0 and .NET 9.0 have
    ' the implementations below.
    '   They optionally specify a format string, an IFormatProvider, or both.
    '   All cases eventually call the full case, which assigns defaults as
    '     needed.
    '   Create similar implementations herein.

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
            System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat
                )>
            ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As System.String

        Return Me.ToComplex.ToString(format, provider).Replace(CHARI, CHARJ)
    End Function ' ToString

    Private Function IFormattable_ToString(
        ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As String Implements IFormattable.ToString

        Return Me.ToString(format, provider)
    End Function ' IFormattable_ToString

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

        Return Me.ToComplex.ToString(format).Replace(CHARI, CHARJ)
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

        Return Me.ToComplex.ToString(provider).Replace(CHARI, CHARJ)
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
        Return Me.ToComplex.ToString().Replace(CHARI, CHARJ)
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
    ''' <param name="format">A standard or custom numeric format string -or- A
    ''' null reference (Nothing in Visual Basic) to use the default format
    ''' defined for the type of the IFormattable implementation.</param>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information -or- A null reference (Nothing in Visual Basic)
    ''' to obtain the numeric format information from the current locale setting
    ''' of the operating system.</param>
    ''' <returns>The current Impedance expressed in the specified standard
    ''' form.</returns>
    Public Function ToStandardString(
        ByVal standardizationStyle As StandardizationStyles,
        <StringSyntax(
            System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat)>
            ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As System.String

        Return Me.ToComplex.ToStandardString(standardizationStyle, format,
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

        Return Me.ToComplex.ToStandardString(
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

        Return Me.ToComplex.ToStandardString(standardizationStyle,
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

        Return Me.ToComplex.ToStandardString(standardizationStyle,
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
        Return Me.ToComplex.ToStandardString().Replace(CHARI, CHARJ)
    End Function ' ToStandardString

#End Region ' "ToStandardString Implementations"

#Region "Parsing Implementations"

    ' As of when recorded, these Complex signatures match in .NET 8.0 and
    '   .NET 9.0.
    '
    '   public static bool TryParse(ReadOnlySpan<char> s, NumberStyles Style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);
    '   public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles Style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);

    ' public static bool TryParse(ReadOnlySpan<char> s, NumberStyles Style, IFormatProvider? provider, out Complex result)
    ' public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles Style, IFormatProvider? provider, out Complex result)
    ''' <summary>
    ''' Attempts to convert the standard form representation of an Impedance
    ''' to its <see cref="OSNW.Numerics.Impedance"/> equivalent using the
    ''' specified layout format, numeric format, and culture-specific format
    ''' information.
    ''' </summary>
    ''' <param name="s">Specifies the standard form string to be parsed.</param>
    ''' <param name="standardizationStyle">Specifies the layout formats
    ''' permitted in numeric string arguments that are passed to the TryParse
    ''' method of the <c>Impedance</c> numeric type.</param>
    ''' <param name="style">Determines the styles permitted in numeric string
    ''' arguments that are passed to the TryParse method of the <c>Impedance</c>
    ''' numeric type.</param>
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

        If Not GetCharCount(s, "j"c).Equals(1) Then
            result = New OSNW.Numerics.Impedance
            Return False
        End If
        Dim Cplx As New System.Numerics.Complex
        If OSNW.Numerics.ComplexExtensions.TryParseStandard(
            s.Replace(CHARJ, CHARI), standardizationStyle, style, provider, Cplx) Then

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

        Return TryParseStandard(s, standardizationStyle,
                                DEFAULTCOMPLEXNUMBERSTYLE, provider, result)
    End Function ' TryParseStandard

#End Region ' "Parsing Implementations"

#Region "De/Serialization"

    ''' <summary>
    ''' Serializes a <see cref="Impedance"/> structure to a JSON-formatted
    ''' string, optionally using formatting specified by
    ''' <paramref name="jsonOptions"/>.
    ''' </summary>
    ''' <param name="jsonString">Specifies the target string.</param>
    ''' <param name="jsonOptions">Optional. Specifies serialization options.
    ''' Default is <c>Nothing</c>.</param>
    ''' <returns><c>True</c> if the serialized export succeeds; otherwise,
    ''' <c>False</c>. Also returns the serialized result in
    ''' <paramref name="jsonString"/>.</returns>
    ''' <remarks>This function does not use a specific culture for numbers; it
    ''' always uses the <see cref="CultureInfo.InvariantCulture"/> culture for
    ''' serialization.</remarks>
    Public Function SerializeJSONString(ByRef jsonString As System.String,
        Optional jsonOptions _
            As System.Text.Json.JsonSerializerOptions = Nothing) _
        As System.Boolean

        ' Input checking.
        If jsonString Is Nothing Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentNullException(NameOf(jsonString), MSGNOSTR)
        End If

        ' REF: Serialize and deserialize numeric data
        ' https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-globalization-numberformatinfo#serialize-and-deserialize-numeric-data
        ' When numeric data is serialized in string format and later
        ' deserialized and parsed, the strings should be generated and parsed by
        ' using the conventions of the invariant culture.

        ' Ref: How to write .NET objects as JSON (serialize)
        ' https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to
        jsonString = If(IsNothing(jsonOptions),
            System.Text.Json.JsonSerializer.Serialize(Me),
            System.Text.Json.JsonSerializer.Serialize(Me, jsonOptions))

        ' On getting this far,
        Return True

    End Function ' SerializeJSONString

    ''' <summary>
    ''' Deserializes the JSON-formatted string specified by
    ''' <paramref name="jsonString"/> into an <see cref="Impedance"/> specified
    ''' by <paramref name="impedanceOut"/>.
    ''' </summary>
    ''' <param name="jsonString">Specifies the JSON-formatted string to be
    ''' deserialized.</param>
    ''' <param name="impedanceOut">Specifies the <see cref="Impedance"/> into
    ''' which <paramref name="jsonString"/> is to be serialized.</param>
    ''' <returns><c>True</c> if the deserialized import succeeds; otherwise,
    ''' <c>False</c> and also returns the deserialized result in
    ''' <paramref name="impedanceOut"/>.</returns>
    Public Shared Function DeserializeJSONString(jsonString As System.String,
        ByRef impedanceOut As Impedance) As System.Boolean

        If System.String.IsNullOrWhiteSpace(jsonString) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentNullException(
                NameOf(jsonString), MSGNOSTR)
        End If

        ' Ref: How to read JSON as .NET objects (deserialize)
        ' https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/deserialization

        ' REF: Serialize and deserialize numeric data
        ' https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-globalization-numberformatinfo#serialize-and-deserialize-numeric-data

        ' REF: Use immutable types and properties
        ' https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/immutability

        impedanceOut = System.Text.Json.JsonSerializer.Deserialize(
            Of Impedance)(jsonString)

        ' On getting this far,
        Return True

    End Function ' DeserializeJSONString

#End Region '  "De/Serialization"

#Region "Other Instance Methods"

    ''' <summary>
    ''' Calculates the voltage reflection coeffient (Gamma) for this instance
    ''' based on the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance, in ohms.</param>
    ''' <returns>The voltage reflection coeffient for the current instance based
    ''' on the specified characteristic impedance.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' When <paramref name="z0"/> is not a positive, non-zero value.
    ''' </exception>
    Public Function VoltageReflectionCoefficient(ByVal z0 As System.Double) _
        As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHNV)
        ElseIf z0.Equals(0.0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHZV)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        ' Ref: https://en.wikipedia.org/wiki/Standing_wave_ratio#Relationship_to_the_reflection_coefficient
        Dim MeAsComplex As System.Numerics.Complex = Me.ToComplex()
        Return (MeAsComplex - z0) / (MeAsComplex + z0)

    End Function ' VoltageReflectionCoefficient

    ''' <summary>
    ''' Calculates the voltage standing wave ratio for this instance based on
    ''' the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance, in ohms.</param>
    ''' <returns>The voltage standing wave ratio for the current instance at the
    ''' specified characteristic impedance.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' When <paramref name="z0"/> is not a positive, non-zero value.
    ''' </exception>
    Public Function VSWR(ByVal z0 As System.Double) As System.Double

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHNV)
        ElseIf z0.Equals(0.0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHZV)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        ' Ref:
        ' https://www.antenna-theory.com/definitions/vswr.php
        ' https://www.antenna-theory.com/definitions/vswr-calculator.php
        ' https://www.microwaves101.com/encyclopedias/voltage-standing-wave-ratio-vswr

        'Dim Gamma As System.Numerics.Complex = Me.VoltageReflectionCoefficient(z0)
        'Dim AbsGamma As System.Double = System.Numerics.Complex.Abs(Gamma)
        Dim AbsGamma As System.Double =
            System.Numerics.Complex.Abs(Me.VoltageReflectionCoefficient(z0))
        Return (1.0 + AbsGamma) / (1.0 - AbsGamma)

    End Function ' VSWR

    ''' <summary>
    ''' Calculates the power reflection coeffient for this instance based on the
    ''' specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance, in ohms.</param>
    ''' <returns>The power reflection coeffient for the current instance at the
    ''' specified characteristic impedance.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' When <paramref name="z0"/> is not a positive, non-zero value.
    ''' </exception>
    Public Function PowerReflectionCoefficient(ByVal z0 As System.Double) _
        As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHNV)
        ElseIf z0.Equals(0.0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHZV)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim VRC As System.Numerics.Complex = Me.VoltageReflectionCoefficient(z0)
        Return VRC * VRC

    End Function ' PowerReflectionCoefficient

    ''' <summary>
    ''' Adds two <c>Impedance</c>s in series.
    ''' </summary>
    ''' <param name="loadZ">The impedance of the load.</param>
    ''' <param name="addZ">The impedance of the added component.</param>
    ''' <returns>The input impedance of the combined load.</returns>
    ''' <remarks>
    ''' <code>
    '''         o----- addZ -----o
    '''                          :
    '''   ~Source                loadZ
    '''                          :
    '''         o----------------o
    ''' </code>
    ''' </remarks>
    Public Shared Function AddSeriesImpedance(
        ByVal loadZ As Impedance, ByVal addZ As Impedance) As Impedance

        ' No input checking. loadZ and addZ are presumed to have been checked
        ' when created.
        Return loadZ + addZ
    End Function ' AddSeriesImpedance

    ''' <summary>
    ''' Adds an <c>Impedance</c> in parallel with a load <c>Impedance</c> and
    ''' returns the result.
    ''' </summary>
    ''' <param name="loadZ">The impedance of the load.</param>
    ''' <param name="addZ">The impedance of the added component.</param>
    ''' <returns>The input impedance of the combined load.</returns>
    ''' <remarks>
    ''' <code>
    '''         o-------o-------o
    '''                 |       :
    '''   ~Source     addZ      loadZ
    '''                 |       :
    '''         o-------o-------o
    ''' </code>
    ''' </remarks>
    Public Shared Function AddShuntImpedance(
        ByVal loadZ As Impedance, ByVal addZ As Impedance) As Impedance

        ' No input checking. loadZ and addZ are presumed to have been checked
        ' when created.
        Return (loadZ.ToAdmittance + addZ.ToAdmittance).ToImpedance
    End Function ' AddShuntImpedance

    '''' <summary>
    '''' Adds an <c>Admittance</c> in parallel with a load <c>Impedance</c> and
    '''' returns the result.
    '''' </summary>
    '''' <param name="loadZ">The impedance of the load.</param>
    '''' <param name="addY">The admittance of the added component.</param>
    '''' <returns>The input impedance of the combined load.</returns>
    '''' <remarks>
    '''' <code>
    ''''         o-------o-------o
    ''''                 |       :
    ''''   ~Source     addY      loadZ
    ''''                 |       :
    ''''         o-------o-------o
    ''''  </code>
    ''''  </remarks>
    'Public Shared Function AddParallelAdmittance(
    '    ByVal loadZ As Impedance, ByVal addY As Admittance) As Impedance

    '    ' No input checking. loadZ and addY are presumed to have been checked
    '    ' when created.
    '    Return (loadZ.ToAdmittance + addY).ToImpedance
    'End Function ' AddParallelAdmittance

#End Region ' "Other Instance Methods"

#Region "Constructors"

    ''' <summary>
    ''' Initializes a new instance of the <c>Impedance</c> structure using the
    ''' specified  <paramref name="resistance"/> (R) and
    ''' <paramref name="reactance"/> (X) values.
    ''' </summary>
    ''' <param name="resistance">Specifies the value of the resistance component
    ''' of the current instance in ohms.</param>
    ''' <param name="reactance">Specifies the value of the reactance component
    ''' of the current instance in ohms.</param>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' When <paramref name="resistance"/> is negative.
    ''' </exception>
    ''' <remarks>
    ''' An exception is thrown if <paramref name="resistance"/> is negative.
    ''' <para>
    ''' No real electrical component can have zero resistance, or its reciprocal
    ''' - infinite admittance. Nor can the opposite case exist. Some theoretical
    ''' calculations, such as choosing a component to resonate a circuit, use
    ''' pure reactances. When necessary, use a very small
    ''' <paramref name="resistance"/>, such as
    ''' <see cref="System.Double.Epsilon"/>, to avoid <c>NaN</c>> results in
    ''' other calculations.
    ''' </para>
    ''' </remarks>
    <JsonConstructor> ' See Use immutable types and properties.
    Public Sub New(ByVal resistance As System.Double,
                   ByVal reactance As System.Double)

        ' REF: Use immutable types and properties
        ' https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/immutability

        ' Input checking.
        ' Leave one consolidated test for now. The version below was based on
        ' considering whether special cases may exist where some of the
        ' rejections may need to be allowed.
        If resistance < 0.0 OrElse Double.IsInfinity(resistance) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(resistance))
        End If
        'If resistance < 0.0 Then
        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ArgumentOutOfRangeException(NameOf(resistance),
        '                                                 MSGCHNV)
        'ElseIf resistance.Equals(0.0) Then
        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ArgumentOutOfRangeException(NameOf(resistance),
        '                                                 MSGCHZV)
        'ElseIf Double.IsInfinity(resistance) Then
        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ArgumentOutOfRangeException(NameOf(resistance),
        '                                                 MSGCHIV)
        'End If

        '        Me.AsComplex = New System.Numerics.Complex(resistance, reactance)
        Me.m_Resistance = resistance
        Me.m_Reactance = reactance

    End Sub ' New

#End Region ' "Constructors"

End Structure ' Impedance
