Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Diagnostics.CodeAnalysis
Imports System.Threading
Imports System.Threading.Tasks.Dataflow

' FROM OLD YTT CODE AND .NET SOURCE:
'    <SerializableAttribute()>
''' <summary>
''' Represents an electrical impedance with resistance (R) and reactance (X).
''' An electrical Impedance (Z) is a number of the standard form Z=R+jX or R+Xj,
''' where R and X are real numbers, and j is the imaginary unit, with the
''' property j^2 = -1.
''' </summary>
Public Structure Impedance
    Implements IEquatable(Of Impedance)
    ' FROM .NET SOURCE:
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

#Region "Fields and Properties"

    ' Use the "has a ..." approach to expose the desired features of a
    ' System.Numerics.Complex.
    ' Do not rename (binary serialization). ??????????????????????????????
    ''' <summary>
    ''' Gets the Impedance as a complex number.
    ''' </summary>
    Private ReadOnly m_Complex As System.Numerics.Complex

    ''' <summary>
    ''' Gets the resistance component, in ohms, of the current <c>Impedance</c>
    ''' instance.
    ''' </summary>
    Public ReadOnly Property Resistance As System.Double
        Get
            Return Me.m_Complex.Real
        End Get
    End Property

    ''' <summary>
    ''' Gets the reactance component, in ohms, of the current <c>Impedance</c>
    ''' instance.
    ''' </summary>
    Public ReadOnly Property Reactance As System.Double
        Get
            Return Me.m_Complex.Imaginary
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

    '        ''' <summary>
    '        ''' Returns a value that represents this instance as its equivalent <see cref="Admittance"/>.
    '        ''' </summary>
    '        ''' <returns>The equivalent admittance value of this instance.</returns>
    '        Public Function ToAdmittance() As Ytt.Util.Electrical.Admittance
    '            Dim ComplexRecip As System.Numerics.Complex = System.Numerics.Complex.Reciprocal(Me.ToComplex())
    '            Return New Ytt.Util.Electrical.Admittance(ComplexRecip.Real, ComplexRecip.Imaginary)
    '        End Function

#Region "System.ValueType Implementations"

    ' public override bool Equals([NotNullWhen(true)] object? obj)
    ' {
    '     return obj is Complex other && Equals(other);
    ' }
    'Public Overrides Function Equals(obj As Object) As Boolean
    '    Return (TypeOf obj Is Impedance) AndAlso
    '        DirectCast(Me, IEquatable(Of Impedance)).Equals(
    '        DirectCast(obj, Impedance))
    'End Function
    ''' <summary>
    ''' Determines whether the specified object is equal to the current object.
    ''' </summary>
    ''' <param name="obj">The object to compare with the current object.</param>
    ''' <returns><c>True</c> if the specified object is equal to the current
    ''' object; otherwise, <c>False</c>.</returns>
    Public Overrides Function Equals(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal obj As System.Object) As System.Boolean

        Return (TypeOf obj Is Impedance) AndAlso
            DirectCast(Me, IEquatable(Of Impedance)).Equals(
            DirectCast(obj, Impedance))
    End Function

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
    ''' <remarks>This may have to be changed to determine equality within some
    ''' reasonable bounds. See 
    ''' <see href="https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers"/>
    ''' </remarks>
    Public Overloads Function Equals(ByVal value As Impedance) As System.Boolean _
        Implements System.IEquatable(Of Impedance).Equals

        Return Me.ToComplex().Equals(value.ToComplex())
    End Function

    ''' <summary>
    ''' Serves as the default hash function.
    ''' </summary>
    ''' <returns>A hash code for the current object.</returns>
    Public Overrides Function GetHashCode() As System.Int32
        Return Me.ToComplex.GetHashCode
    End Function

#End Region ' "System.ValueType Implementations"


#Region "IEquatable Implementations"

    Public Shared Operator =(left As Impedance, right As Impedance) As Boolean
        Return left.Equals(right)
    End Operator

    Public Shared Operator <>(left As Impedance, right As Impedance) As Boolean
        Return Not left = right
    End Operator

#End Region ' "IEquatable Implementations"

#Region "TryFormat Implementations"

    '
    '
    '
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
            System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.NumericFormat)>
            ByVal format As System.String,
        ByVal provider As System.IFormatProvider) _
        As System.String

        Return Me.m_Complex.ToString(format, provider).Replace(CHARI, CHARJ)
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

        Return Me.m_Complex.ToString(format).Replace(CHARI, CHARJ)
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

        Return Me.m_Complex.ToString(provider).Replace(CHARI, CHARJ)
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
        Return Me.m_Complex.ToString().Replace(CHARI, CHARJ)
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

        Return Me.m_Complex.ToStandardString(standardizationStyle, format,
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

        Return Me.m_Complex.ToStandardString(
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

        Return Me.m_Complex.ToStandardString(standardizationStyle,
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

        Return Me.m_Complex.ToStandardString(standardizationStyle,
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
        Return Me.m_Complex.ToStandardString().Replace(CHARI, CHARJ)
    End Function ' ToStandardString

#End Region ' "ToStandardString Implementations"

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

#Region "Other Instance Methods"

    ''' <summary>
    ''' Calculates the voltage reflection coeffient for this instance based on
    ''' the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">The characteristic impedance.</param>
    ''' <returns>The voltage reflection coeffient.</returns>
    ''' <exception cref="System.ArgumentNullException">
    '''   Thrown when any parameter is <c>Nothing</c>.
    ''' </exception>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' Thrown when <paramref name="z0"/> is not a positive, non-zero value.
    ''' </exception>
    Public Function VoltageReflectionCoefficient(ByVal z0 As System.Double) As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0))
        End If

        ' Ref: https://en.wikipedia.org/wiki/Standing_wave_ratio

        Dim MeAsComplex As System.Numerics.Complex = Me.ToComplex()
        Return (MeAsComplex - z0) / (MeAsComplex + z0)

    End Function ' VoltageReflectionCoefficient

    ''' <summary>
    ''' Calculates the voltage standing wave ratio for this instance based on
    ''' the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">The characteristic impedance in ohms.</param>
    ''' <returns>The voltage standing wave ratio.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' Thrown when <paramref name="z0"/> is not a positive, non-zero value.
    ''' </exception>
    Public Function VSWR(ByVal z0 As System.Double) As System.Double

        ' Input checking.
        If z0 <= 0.0 Then
            Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0))
        End If

        ' Ref:
        ' https://www.antenna-theory.com/definitions/vswr.php
        ' https://www.antenna-theory.com/definitions/vswr-calculator.php
        ' https://www.microwaves101.com/encyclopedias/voltage-standing-wave-ratio-vswr

        'Dim Gamma As System.Numerics.Complex = Me.VoltageReflectionCoefficient(z0)
        ''            Internal calls to Ytt.Util.Electrical.AbsComplex were replaced by direct calls to System.Numerics.Complex.Abs
        ''            Dim AbsGamma As System.Double = Ytt.Util.Electrical.AbsComplex(Gamma)
        'Dim AbsGamma As System.Double = System.Numerics.Complex.Abs(Gamma)
        'Return (1.0 + AbsGamma) / (1.0 - AbsGamma)

        Dim AbsGamma As System.Double =
            System.Numerics.Complex.Abs(Me.VoltageReflectionCoefficient(z0))
        Return (1.0 + AbsGamma) / (1.0 - AbsGamma)

    End Function ' VSWR

    ''' <summary>
    ''' Calculates the power reflection coeffient for this instance based on the
    ''' specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">The characteristic impedance in ohms.</param>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' Thrown when <paramref name="z0"/> is not a positive, non-zero value.
    ''' </exception>
    ''' <returns>The voltage reflection coeffient.</returns>
    Public Function PowerReflectionCoefficient(ByVal z0 As System.Double) As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0))
        End If

        Dim VRC As System.Numerics.Complex = Me.VoltageReflectionCoefficient(z0)
        Return System.Numerics.Complex.Multiply(VRC, VRC)

    End Function ' PowerReflectionCoefficient

#End Region ' "Other Instance Methods"

#Region "Constructors"

    ''' <summary>
    ''' Initializes a new instance of the <c>Impedance</c> structure using the
    ''' specified  <paramref name="resistance"/> (R) and
    ''' <paramref name="reactance"/> (X) values.
    ''' </summary>
    ''' <param name="resistance">Specifies the value of the resistance component of the
    ''' Impedance in ohms.</param>
    ''' <param name="reactance">Specifies the value of the reactance component of the
    ''' Impedance in ohms.</param>
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
    Public Sub New(ByVal resistance As System.Double,
                   ByVal reactance As System.Double)

        ' Input checking.
        If resistance < 0.0 Then
            Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(resistance))
        End If

        Me.m_Complex = New System.Numerics.Complex(resistance, reactance)

    End Sub ' New

#End Region ' "Constructors"

End Structure ' Impedance
