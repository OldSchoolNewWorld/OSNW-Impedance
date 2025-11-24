Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

' This document contains a structure that represents an admittance. It is
' included as a convenience and to allow the use of a familiar concept. At a
' certain level, an admittance is just a different way to represent an
' impedance. A derived project could be reworked to use impedances directly,
' which may offer some performance improvments at the expense of readability. A
' limited set of convenience features has been implemented.

Imports System.Diagnostics.CodeAnalysis
Imports System.Text.Json.Serialization
Imports OSNW.Numerics.ComplexExtensions

''' <summary>
''' Represents an electrical admittance with conductance (G) and
''' susceptance (B).
''' An electrical admittance (Y) is a number of the standard form Y=G+Bj or
''' Y=G+jB, where:
'''   Y is the admittance (siemens);
'''   G is the conductance (siemens);
'''   B is the susceptance (siemens); and
'''   j^2 = -1, the imaginary unit.
''' </summary>
Public Structure Admittance
    Implements IEquatable(Of Admittance), IFormattable

    Const MSGCHNV As System.String = Impedance.MSGCHNV
    Const MSGCHZV As System.String = Impedance.MSGCHZV
    Const MSGCHIV As System.String = Impedance.MSGCHIV

#Region "Fields and Properties"

    ''' <summary>
    ''' Gets the conductance (G) component, in siemens, of the current instance.
    ''' </summary>
    Private ReadOnly m_Conductance As System.Double

    ''' <summary>
    ''' Gets the conductance (G) component, in siemens, of the current instance.
    ''' </summary>
    Public ReadOnly Property Conductance As System.Double
        ' Do not rename (binary serialization).
        Get
            Return Me.m_Conductance
        End Get
    End Property

    ''' <summary>
    ''' Gets the susceptance (B) component, in siemens, of the current instance.
    ''' </summary>
    Private ReadOnly m_Susceptance As System.Double

    ''' <summary>
    ''' Gets the susceptance (B) component, in siemens, of the current instance.
    ''' </summary>
    Public ReadOnly Property Susceptance As System.Double
        ' Do not rename (binary serialization).
        Get
            Return Me.m_Susceptance
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
        Return New System.Numerics.Complex(Me.Conductance, Me.Susceptance)
    End Function

    ''' <summary>
    ''' Returns a value that represents this instance as its equivalent
    ''' <see cref="Impedance"/>.
    ''' </summary>
    ''' <returns>The equivalent <c>Impedance</c> value of the current
    ''' instance.</returns>
    Public Function ToImpedance() As Impedance
        Dim ComplexRecip As System.Numerics.Complex =
            System.Numerics.Complex.Reciprocal(Me.ToComplex())
        Return New Impedance(ComplexRecip.Real, ComplexRecip.Imaginary)
    End Function

#Region "Conversions"

#End Region ' "Conversions"

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

        ' This may have to be changed to determine equality within some
        ' reasonable bounds. See 
        ' <see href="https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers"/>
        Return (TypeOf obj Is Admittance) AndAlso
            DirectCast(Me, IEquatable(Of Admittance)).Equals(
            DirectCast(obj, Admittance))
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
    Public Overloads Function Equals(ByVal value As Admittance) As System.Boolean _
        Implements System.IEquatable(Of Admittance).Equals

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
    ''' Returns a value that indicates whether two <c>Admittance</c>s are equal.
    ''' </summary>
    ''' <param name="left">The first <c>Admittance</c> to compare.</param>
    ''' <param name="right">The second <c>Admittance</c> to compare.</param>
    ''' <returns><c>True</c>> if the left and right parameters have the same
    ''' value; otherwise, <c>False</c>.</returns>
    Public Shared Operator =(ByVal left As Admittance,
                             ByVal right As Admittance) As System.Boolean

        Return left.Equals(right)
    End Operator

    ''' <summary>
    ''' Returns a value that indicates whether two <c>Admittance</c>s are not
    ''' equal.
    ''' </summary>
    ''' <param name="left">The first <c>Admittance</c> to compare.</param>
    ''' <param name="right">The second <c>Admittance</c> to compare.</param>
    ''' <returns><c>True</c>> if left and right are not equal; otherwise,
    ''' <c>False</c>.</returns>
    Public Shared Operator <>(ByVal left As Admittance,
                              ByVal right As Admittance) As System.Boolean

        Return Not left = right
    End Operator

    ''' <summary>
    ''' Returns the result of the addition of two <c>Admittance</c>s.
    ''' </summary>
    ''' <param name="admittance1">The first <c>Admittance</c> to add.</param>
    ''' <param name="admittance2">The first <c>Admittance</c> to add.</param>
    ''' <returns>The result of the addition.</returns>
    Public Shared Operator +(admittance1 As Admittance,
                             admittance2 As Admittance) As Admittance
        ' No input checking. impedance1 and impedance2 are presumed to have been
        ' checked when created.
        Dim TotalC As System.Numerics.Complex =
            admittance1.ToComplex + admittance2.ToComplex
        Return New Admittance(TotalC.Real, TotalC.Imaginary)
    End Operator

#End Region ' "Operator Implementations"

#Region "TryFormat Implementations"

    '
    '
    ' Need these ??????????????
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
    ''' Converts the value of the current Admittance to its equivalent string
    ''' representation in Cartesian form, using the specified numeric format and
    ''' culture-specific format information for its resistance and reactance
    ''' parts.
    ''' </summary>
    ''' <param name="format">A standard or custom numeric format string.</param>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information.</param>
    ''' <returns>The current Admittance expressed in Cartesian form.</returns>
    Public Overloads Function ToString(
        <StringSyntax(
            System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat
                )>
            ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As System.String

        '        Return Me.AsComplex.ToString(format, provider).Replace(CHARI, CHARJ)
        Return Me.ToImpedance().ToString(format, provider)
    End Function ' ToString

    Private Function IFormattable_ToString(
        ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As String Implements IFormattable.ToString

        Return ToString(format, provider)
    End Function ' IFormattable_ToString

    '    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    ''' <summary>
    ''' Converts the value of the current Admittance to its equivalent string
    ''' representation in Cartesian form, using the specified numeric format
    ''' information, and using the default culture-specific format information,
    ''' for its resistance and reactance parts.
    ''' </summary>
    ''' <param name="format">A standard or custom numeric format string.</param>
    ''' <returns>The current Admittance expressed in Cartesian form.</returns>
    Public Overloads Function ToString(
        <StringSyntax(StringSyntaxAttribute.NumericFormat)>
            ByVal format As System.String) _
        As System.String

        Return Me.ToComplex.ToString(format).Replace(CHARI, CHARJ)
    End Function ' ToString

    '    public string ToString(IFormatProvider? provider)
    ''' <summary>
    ''' Converts the value of the current Admittance to its equivalent string
    ''' representation in Cartesian form, using the specified culture-specific
    ''' format information, and using the default numeric format, for its
    ''' resistance and reactance parts.
    ''' </summary>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information.</param>
    ''' <returns>The current Admittance expressed in Cartesian form.</returns>
    Public Overloads Function ToString(
        ByVal provider As System.IFormatProvider) As System.String

        Return Me.ToComplex.ToString(provider).Replace(CHARI, CHARJ)
    End Function ' ToString

    '    public override string ToString()
    ''' <summary>
    ''' Converts the value of the current Admittance to its equivalent string
    ''' representation in Cartesian form, using the default numeric format and
    ''' culture-specific format information for its resistance and reactance
    ''' parts.
    ''' </summary>
    ''' <returns>The current Admittance expressed in Cartesian form.</returns>
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
    ''' Converts the value of the current <c>Admittance</c> to its equivalent
    ''' string representation in standard form, using the specified layout
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
    ''' <returns>The current <c>Admittance</c> expressed in the specified standard
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
    ''' Converts the value of the current <c>Admittance</c> to its equivalent
    ''' string representation in standard form, using the specified layout
    ''' format information, and using the default numeric format and
    ''' culture-specific format for its real and imaginary parts.
    ''' </summary>
    ''' <param name="standardizationStyle">Specifies the 
    ''' <see cref="StandardizationStyles"/> to be used to generate the standard
    ''' form string.</param>
    ''' <returns>The current <c>Admittance</c> expressed in standard form.</returns>
    Public Function ToStandardString(
        ByVal standardizationStyle As StandardizationStyles) _
        As System.String

        Return Me.ToComplex.ToStandardString(
            standardizationStyle).Replace(CHARI, CHARJ)
    End Function ' ToStandardString

    '    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format)
    ''' <summary>
    ''' Converts the value of the current <c>Admittance</c> to its equivalent
    ''' string representation in standard form, using the specified layout
    ''' format and numeric format information, and using the default
    ''' culture-specific format information, for its real and imaginary parts.
    ''' </summary>
    ''' <param name="standardizationStyle">Specifies the 
    ''' <see cref="StandardizationStyles"/> to be used to generate the standard
    ''' form string.</param>
    ''' <param name="format">A standard or custom numeric format string.</param>
    ''' <returns>The current <c>Admittance</c> expressed in standard form.</returns>
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
    ''' Converts the value of the current <c>Admittance</c> to its equivalent
    ''' string representation in standard form, using the specified layout
    ''' format and culture-specific format information, and using the
    ''' default numeric format, for its real and imaginary parts.
    ''' </summary>
    ''' <param name="standardizationStyle">Specifies the 
    ''' <see cref="StandardizationStyles"/> to be used to generate the standard
    ''' form string.</param>
    ''' <param name="provider">An object that supplies culture-specific
    ''' formatting information.</param>
    ''' <returns>The current <c>Admittance</c> expressed in standard form.</returns>
    Public Function ToStandardString(
        ByVal standardizationStyle As StandardizationStyles,
        ByVal provider As System.IFormatProvider) _
        As System.String

        Return Me.ToComplex.ToStandardString(standardizationStyle,
                                             provider).Replace(CHARI, CHARJ)
    End Function ' ToStandardString

    '    public override string ToString()
    ''' <summary>
    ''' Converts the value of the current <c>Admittance</c> to its equivalent
    ''' string representation in standard form, using the default layout format,
    ''' numeric format, and culture-specific format information for its real and
    ''' imaginary parts.
    ''' </summary>
    ''' <returns>The current <c>Admittance</c> expressed in standard form.</returns>
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
    ''' Attempts to convert the standard form representation of an impedance to
    ''' its <see cref="Impedance"/> equivalent using the specified layout
    ''' format, numeric format, and culture-specific format information.
    ''' </summary>
    ''' <param name="s">Specifies the standard form string to be parsed.</param>
    ''' <param name="standardizationStyle">Specifies the layout formats
    ''' permitted in numeric string arguments that are passed to the TryParse
    ''' method of the <c>Impedance</c> numeric type.</param>
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
        ByRef result As OSNW.Numerics.Admittance) _
        As System.Boolean

        If Not GetCharCount(s, "j"c).Equals(1) Then
            result = New OSNW.Numerics.Admittance
            Return False
        End If
        Dim Cplx As New System.Numerics.Complex
        If OSNW.Numerics.ComplexExtensions.TryParseStandard(
            s.Replace(CHARJ, CHARI), standardizationStyle, style, provider,
            Cplx) Then

            result = New Admittance(Cplx.Real, Cplx.Imaginary)
            Return True
        Else
            result = New OSNW.Numerics.Admittance
            Return False
        End If
    End Function ' TryParseStandard

    ' public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result)
    ' public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result)
    ''' <summary>
    ''' Attempts to convert the standard form representation of an impedance to
    ''' its <see cref="Impedance"/> equivalent using the specified layout format
    ''' and culture-specific format information.
    ''' </summary>
    ''' <param name="s">Specifies the standard form string to be parsed.</param>
    ''' <param name="standardizationStyle">Specifies the layout formats
    ''' permitted in numeric string arguments that are passed to the TryParse
    ''' method of the <c>Impedance</c> numeric type.</param>
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
        ByRef result As OSNW.Numerics.Admittance) _
        As System.Boolean

        Return TryParseStandard(s, standardizationStyle,
                                DEFAULTCOMPLEXNUMBERSTYLE, provider, result)
    End Function ' TryParseStandard

#End Region ' "Parsing Implementations"

#Region "Constructors"

    ''' <summary>
    ''' Initializes a new instance of the <c>Admittance</c> structure using the
    ''' specified  <paramref name="conductance"/> (G) and
    ''' <paramref name="susceptance"/> (B) values.
    ''' </summary>
    ''' <param name="conductance">Specifies the value of the conductance
    ''' component of the current instance in siemens.</param>
    ''' <param name="susceptance">Specifies the value of the susceptance
    ''' component of the current instance in siemens.</param>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' When <paramref name="conductance"/> is negative.
    ''' </exception>
    ''' <remarks>
    ''' An exception is thrown if <paramref name="conductance"/> is negative.
    ''' <para>
    ''' No real electrical component can have zero conductance, or its reciprocal
    ''' - infinite resistance. Nor can the opposite case exist. Some theoretical
    ''' calculations, such as choosing a component to resonate a circuit, use
    ''' pure reactances. When necessary, use a very small
    ''' <paramref name="conductance"/>, such as
    ''' <see cref="System.Double.Epsilon"/>, to avoid <c>NaN</c>> results in
    ''' other calculations.
    ''' </para>
    ''' </remarks>
    <JsonConstructor> ' See Use immutable types and properties.
    Public Sub New(ByVal conductance As System.Double,
                   ByVal susceptance As System.Double)

        ' REF: Use immutable types and properties
        ' https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/immutability

        ' Input checking.
        ' Leave one consolidated test for now. The version below was based on
        ' considering whether special cases may exist where some of the
        ' rejections may need to be allowed.
        If conductance < 0.0 OrElse Double.IsInfinity(conductance) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(conductance))
        End If
        'If conductance < 0.0 Then
        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ArgumentOutOfRangeException(NameOf(conductance),
        '                                                 MSGCHNV)
        'ElseIf Impedance.EqualEnoughZero(conductance, TOLERANCE) Then
        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ArgumentOutOfRangeException(NameOf(conductance),
        '                                                 MSGCHZV)
        'ElseIf Double.IsInfinity(conductance) Then
        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ArgumentOutOfRangeException(NameOf(conductance),
        '                                                 MSGCHIV)
        'End If

        '        Me.AsComplex = New System.Numerics.Complex(resistance, reactance)
        Me.m_Conductance = conductance
        Me.m_Susceptance = susceptance

    End Sub ' New

#End Region ' "Constructors"

End Structure ' Admittance
