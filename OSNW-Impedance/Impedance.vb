'TODO:
' Move EqualEnough routines to a new class derived from System.Double, in its own solution?
'   Both Single and Double?
'   Implement at least some as extensions?
' Convert from building an array, to building a list, of suggested solutions.
'   https://learn.microsoft.com/en-us/dotnet/standard/collections/
' Create looped test of tangency with reversed checks?
' Should infinity be allowed or rejected for admittance and susceptance inputs?
' Add De/Serialization to Admittance?????
'   "the strings should be generated and parsed by using the conventions of the invariant culture."
'   REF: Serialize and deserialize numeric data
'   https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-globalization-numberformatinfo#serialize-and-deserialize-numeric-data
' Add De/Serialization to solution suggestions?????
' Allow both "i" and "j" to match the .NET result? Add tests for both i and j.
'   Wait, where does .NET indicate anything about allowing "j" for Complex aside from "Format a complex
'     number"? Complex only has ToString() and TryFormat() - nothing about standard form.

Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Diagnostics.CodeAnalysis
Imports System.Globalization
Imports System.Text.Json.Serialization

' REF: Electrical impedance
' https://en.wikipedia.org/wiki/Electrical_impedance

' REF: Standing wave ratio
' https://en.wikipedia.org/wiki/Standing_wave_ratio

' REF: Formulas of Impedances in AC Circuits
' https://www.mathforengineers.com/AC-circuits/formulas-of-impedances-in-ac-circuits.html

' REF: A Practical Introduction to Impedance Matching.
' https://picture.iczhiku.com/resource/eetop/shkgQUqJkAUQZBXx.pdf

' REF: Impedance matching
' https://en.wikipedia.org/wiki/Impedance_matching

' Impedance Matching and Smith Chart Impedance
' https://www.analog.com/en/resources/technical-articles/impedance-matching-and-smith-chart-impedance-maxim-integrated.html?gated=1751854195363

' REF: Spread Spectrum Scene
' http://www.sss-mag.com/smith.html

' REF: Advanced Accelerator Physics Course 2013
' https://indico.cern.ch/event/216963/sessions/35851/attachments/347577/

'   C1-Navigation_smith2.pdf
'   https://indico.cern.ch/event/216963/sessions/35851/attachments/347577/484627/C1-Navigation_smith2.pdf

'   C1-Smith_Chart_Aarhus_CAS_2010_caspers_version_20_September_2010.pdf
'   https://indico.cern.ch/event/216963/sessions/35851/attachments/347577/484626/C1-Smith_Chart_Aarhus_CAS_2010_caspers_version_20_September_2010.pdf

'   C1-S_Parameter_in_CAS_Ebeltoft_CASPERS_26.1.2011.pdf
'   https://indico.cern.ch/event/216963/sessions/35851/attachments/347577/484630/C1-S_Parameter_in_CAS_Ebeltoft_CASPERS_26.1.2011.pdf

' Smith-Chart - University of Utah
' https://my.ece.utah.edu/~ece5321/ZY_chart.pdf
'
' NORMALIZED IMPEDANCE AND ADMITTANCE COORDINATES
' https://mtt.org/app/uploads/2023/08/ZY_color_smith_chart.pdf

' Chapter 2-2 The Smith Chart - University of Florida
' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

' Microsoft Word - The Smith Chart.doc
' https://ittc.ku.edu/~jstiles/723/handouts/The%20Smith%20Chart.pdf

' Smith Chart Table of Contents
' http://www.antenna-theory.com/tutorial/smith/chart.php

' FROM OLD YTT CODE AND .NET Source:
'    <SerializableAttribute()>
''' <summary>
''' Represents an electrical impedance with resistance (R) and reactance (X).
''' An electrical impedance (Z) is a number of the standard form Z=R+Xj or
''' Z=R+jX, where:
'''   Z is the impedance (ohms);
'''   R is the resistance (ohms);
'''   X is the reactance (ohms); and
'''   j^2 = -1, the imaginary unit.
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
    '     Implements IEquatable(Of Ytt.Util.Electrical.Impedance), IFormattable

    ' DEV: An Impedance is essentially a complex number with some cosmetic
    ' differences:
    '   'i' is replaced by 'j' in the standard form.
    '   The Real component is represented by Resistance.
    '   The Imaginary component is represented by Reactance.
    ' Since System.Numerics.Complex is represented as a structure, it cannot be
    ' inherited. Given that, Impedance is created as a structure which uses
    ' familiar terminology but relies on Complex for some of its work.

    ''' <summary>
    ''' This sets a practical limit on the precision of equality detection in
    ''' mathematical operations related to impedances. It is intended to prevent
    ''' issues arising from minor inequalities due to floating point precision
    ''' limitations. A smaller value DEcreases the liklihood of detecting
    ''' equality; a larger value INcreases the liklihood of detecting equality.
    ''' </summary>
    ''' <remarks>
    ''' This is intended to be used as a factor to be multiplied by some
    ''' practical reference value.<br/>
    ''' <example>
    ''' This example uses the <c>DFLTIMPDTOLERANCE</c> value to determine if two
    ''' impedances are close enough to be treated as being equal. In the case of
    ''' <see cref="EqualEnough(Double, Double, Double)"/>, the reference value
    ''' is the "refVal" parameter.
    ''' <code>
    ''' Public Shared Sub EqualityTest
    '''
    '''     Dim Tol As System.Double = Impedance.DFLTIMPDTOLERANCE
    '''     Dim Z1 As New OSNW.Numerics.Impedance(50.0, 25.0)
    '''     Dim Z2 As New OSNW.Numerics.Impedance(50.000049, 25)
    '''
    '''     if Impedance.EqualEnough(Z2.Resistance, Z1.Resistance, Tol) AndAlso
    '''         Impedance.EqualEnough(Z2.Reactance, Z1.Reactance, Tol)) then
    '''         '
    '''         ' Code for a match.
    '''         '
    '''     else
    '''         '
    '''         ' Code for a mismatch.
    '''         '
    '''     end if
    '''
    ''' End Sub
    ''' </code></example>
    ''' </remarks>
    Const DFLTIMPDTOLERANCE As System.Double = 0.000_001

    ''' <summary>
    ''' This sets a practical limit on the precision of zero detection in
    ''' mathematical operations related to impedances. It is intended to prevent
    ''' issues arising from floating point precision limitations. A smaller value
    ''' DEcreases the liklihood of zero detection; a larger value INcreases the
    ''' liklihood of zero detection.
    ''' </summary>
    ''' <remarks>
    ''' This is intended to be used as a factor to be multiplied by some
    ''' practical reference value. Since zeroes cannot be compared based on a
    ''' ratio of two values, some other value is needed to establish a
    ''' reasonable closeness to zero.<br/>
    ''' <example>
    ''' This example shows how to use <c>DFLTIMPDTOLERANCE0</c> in conjunction
    ''' with <see cref="EqualEnoughZero(System.Double, System.Double)"/> to
    ''' determine if two reactances are close enough to be considered equal.
    ''' <code>
    ''' Dim Z0 As System.Double = 50.0
    ''' Dim SourceX As System.Double = SourceZ.Reactance
    ''' Dim LoadX As System.Double = LoadZ.Reactance
    ''' If EqualEnoughZero(SourceX - LoadX, DFLTIMPDTOLERANCE0 * Z0) Then
    '''     '
    '''     ' Code for when reactances are considered to already match.
    '''     '
    ''' ElseIf
    '''     '
    '''     ' Code for when reactances are not considered to already match.
    '''     '
    ''' End If
    ''' </code></example>
    ''' </remarks>
    Const DFLTIMPDTOLERANCE0 As System.Double = 0.000_001

    ''' <summary>
    ''' This sets a practical limit on the precision of equality detection in
    ''' graphics operations. It is intended to prevent issues arising from
    ''' floating point precision limitations. This should account for
    ''' indistinguishable, sub-pixel, differences on any current monitor or
    ''' printer. A smaller value DEcreases the liklihood of detecting equality;
    ''' a larger value INcreases the liklihood of detecting equality.
    ''' </summary>
    Const GRAPHICTOLERANCE As System.Double = 0.0001

    Public Const PI As System.Double = System.Double.Pi
    Public Const HALFPI As System.Double = System.Double.Pi / 2.0

    Public Const MSGCHIV As System.String = "Cannot have an infinite value."
    Public Const MSGCHNV As System.String = "Cannot have a negative value."
    Public Const MSGCHZV As System.String = "Cannot have a zero value."
    Public Const MSGFGPXPY As System.String = "Failure getting PlotX, PlotY."
    Public Const MSGFIXEDSIZEVIOLATION As System.String =
        "cannot modify the fixed-size ImageImpedanceList."
    Public Const MSGIIC As System.String = "Invalid intersection count."
    Public Const MSGNOSTR As System.String = "Cannot be Null/Nothing."
    Public Const MSGTDNRT As String = " transformation did not reach target."
    Public Const MSGUEEZ As System.String = MSGCHZV & " Use EqualEnoughZero()."
    Public Const MSGVMBGTZ As System.String =
        "Must be a positive, non-zero value."
    Public Const MSGVMBGTE0 As System.String =
        "Must be greater than or equal to 0."
    Public Const MSGVMBGTE1 As System.String =
        "Must be greater than or equal to 1."

#Region "Fields and Properties"

    ''' <summary>
    ''' Gets the resistance (R) component, in ohms, of the current instance.
    ''' </summary>
    Private ReadOnly m_Resistance As System.Double

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
    Private ReadOnly m_Reactance As System.Double

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

#Region "Conversions"

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

#End Region ' "Conversions"

#Region "System.ValueType Implementations"

#Region "EqualEnough Implementations"

    ' REF: Precision and complex numbers
    ' https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers

    ' REF: Random ASCII – tech blog of Bruce Dawson
    ' https://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/

    ''' <summary>
    ''' Check for reasonable equality when using floating point values. A
    ''' difference of less than or equal to <paramref name="maxDiff"/> is
    ''' considered to establish equality.
    ''' </summary>
    ''' <param name="otherVal">Specifies the value to be compared to
    ''' <paramref name="refVal"/>.</param>
    ''' <param name="refVal">Specifies the reference value to which
    ''' <paramref name="otherVal"/> is compared.</param>
    ''' <param name="maxDiff">Specifies the maximum difference that satisfies
    ''' equality.</param>
    ''' <returns><c>True</c> if the values are reasonably close in value;
    ''' otherwise, <c>False</c>.</returns>
    ''' <remarks>
    ''' This does the comparison based on an absolute numeric difference. The
    ''' control value is <paramref name="maxDiff"/>. Select
    ''' <paramref name="maxDiff"/> such that it is a good representation of
    ''' zero, relative to other known or expected values.</remarks>
    Public Shared Function EqualEnoughAbsolute(ByVal otherVal As System.Double,
        ByVal refVal As System.Double, ByVal maxDiff As System.Double) _
        As System.Boolean

        ' No input checking.
        Return System.Math.Abs(otherVal - refVal) <= maxDiff
    End Function ' EqualEnoughAbsolute

    ''' <summary>
    ''' Check for reasonable equality to zero when using floating point values.
    ''' Any value less than or equal to <paramref name="zeroTolerance"/> from
    ''' zero is considered to be equal to zero.
    ''' </summary>
    ''' <param name="value">Specifies the value to be compared to zero.</param>
    ''' <param name="zeroTolerance">Specifies the maximum offset from zero which
    ''' is assumed to represent zero.</param>
    ''' <returns><c>True</c> if <paramref name="value"/> is reasonably close to
    ''' zero; otherwise, <c>False</c>.</returns>
    ''' <remarks>Use this when an actual zero reference would cause a failure in
    ''' <see cref="EqualEnough(System.Double, System.Double, System.Double)"/>.
    ''' Select <paramref name="zeroTolerance"/> such that it is a good
    ''' representation of zero relative to other known or expected
    ''' values.</remarks>
    Public Shared Function EqualEnoughZero(ByVal value As System.Double,
        ByVal zeroTolerance As System.Double) As System.Boolean

        ' No input checking.
        Return System.Math.Abs(value) <= System.Math.Abs(zeroTolerance)
    End Function ' EqualEnoughZero

    ''' <summary>
    ''' Check for reasonable equality, within a specified ratio, when using
    ''' floating point values.
    ''' </summary>
    ''' <param name="otherVal">Specifies the value to be compared to
    ''' <paramref name="refVal"/>.</param>
    ''' <param name="refVal">Specifies the reference value to which
    ''' <paramref name="otherVal"/> is compared.</param>
    ''' <param name="ratio">Specifies the maximum ratio of the values which is
    ''' assumed to represent equality.</param>
    ''' <returns><c>True</c> if the values are reasonably close in value;
    ''' otherwise, <c>False</c>.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When either
    ''' compared value is zero.</exception>
    ''' <remarks>
    ''' This does the comparison based on scale, not on an absolute numeric
    ''' difference. The control value is <paramref name="ratio"/> multiplied
    ''' by <paramref name="refVal"/>, to determine the minimum difference that
    ''' excludes equality.<br/>
    ''' There is no way to scale a comparison to zero. When a zero reference
    ''' would cause a failure here, use
    ''' <see cref="EqualEnoughZero(System.Double, System.Double)"/>.
    ''' </remarks>
    Public Shared Function EqualEnough(ByVal otherVal As System.Double,
        ByVal refVal As System.Double, ByVal ratio As System.Double) _
        As System.Boolean

        ' Input checking.
        If refVal.Equals(0.0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(refVal), MSGUEEZ)
        End If
        If otherVal.Equals(0.0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(otherVal), MSGUEEZ)
        End If

        Return System.Math.Abs(otherVal - refVal) <
            System.Math.Abs(ratio * refVal)

    End Function ' EqualEnough

    ''' <summary>
    ''' Check for reasonable equality of two impedances, based on the
    ''' characteristic impedance of the system. A difference of less than or
    ''' equal to <see cref="DFLTIMPDTOLERANCE"/> multiplied by
    ''' <paramref name="z0"/> is considered to establish equality.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance of the
    ''' system.</param>
    ''' <param name="otherVal">Specifies the value to be compared to
    ''' <paramref name="refVal"/>.</param>
    ''' <param name="refVal">Specifies the reference value to which
    ''' <paramref name="otherVal"/> is compared.</param>
    ''' <returns><c>True</c> if the values are reasonably close in value;
    ''' otherwise, <c>False</c>.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is zero or negative.</exception>"
    ''' <remarks>
    ''' This attempts to do the comparison based on scale, not on an absolute
    ''' numeric difference. When either reactance is nearly zero, both
    ''' reactances must be nearly zero to be considered equal.
    ''' The control value is <see cref="DFLTIMPDTOLERANCE"/> multiplied by
    ''' <paramref name="z0"/>, to determine the minimum difference that
    ''' excludes equality.<br/>
    ''' There is no way to scale a comparison to zero. When a zero reference
    ''' would cause a failure here, this uses <see cref="EqualEnoughZero"/>.
    ''' </remarks>
    Public Shared Function EqualEnough(ByVal z0 As System.Double,
        ByVal otherVal As Impedance, ByVal refVal As Impedance) _
        As System.Boolean

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(z0), Impedance.MSGVMBGTZ)
        End If
        ' otherVal and refVal are presumed to have been checked when created.

        Dim NearlyZero As System.Double = DFLTIMPDTOLERANCE0 * z0

        If Not EqualEnoughAbsolute(otherVal.Resistance, refVal.Resistance,
                                   NearlyZero) Then
            Return False
        End If
        If EqualEnoughZero(otherVal.Reactance, NearlyZero) OrElse
            EqualEnoughZero(refVal.Reactance, NearlyZero) Then

            ' There is a zero; both reactances must be nearly zero.
            Return EqualEnoughZero(otherVal.Reactance, NearlyZero) AndAlso
                EqualEnoughZero(refVal.Reactance, NearlyZero)
        Else
            ' Not zero; reactances must match.
            Return EqualEnough(otherVal.Reactance, refVal.Reactance,
                               DFLTIMPDTOLERANCE)
        End If

    End Function ' EqualEnough

#End Region ' "EqualEnough Implementations"

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
    ''' <remarks><seealso cref="EqualEnough(Double, Impedance, Impedance)"/></remarks>
    Public Overrides Function Equals(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal obj As System.Object) As System.Boolean

        ' This may have to be changed to determine equality within some
        ' reasonable bounds. See 
        ' <see href="https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers"/>
        ' That is now available via EqualEnough(value, refVal) above.
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
        ' That is now available via EqualEnough(value, refVal) above.
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
    ''' <returns><c>True</c> if left and right are not equal; otherwise,
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

    ''' <summary>
    ''' Returns the result of the subtraction of one <c>Impedance</c>
    ''' (<paramref name="subtrahend"/>) from another
    ''' (<paramref name="minuend"/>).
    ''' </summary>
    ''' <param name="minuend"> the number from which another number is being subtracted</param>
    ''' <param name="subtrahend">denote the number being subtracted from another</param>
    ''' <returns></returns>
    Public Shared Operator -(minuend As Impedance,
                             subtrahend As Impedance) As Impedance
        ' No input checking. impedance1 and impedance2 are presumed to have been
        ' checked when created.
        Dim RemainderC As System.Numerics.Complex =
            minuend.ToComplex - subtrahend.ToComplex
        Return New Impedance(RemainderC.Real, RemainderC.Imaginary)
    End Operator

    ''' <summary>
    ''' Returns the result of the division of one <c>Impedance</c>
    ''' (<paramref name="numerator"/>) by another
    ''' (<paramref name="denominator"/>).
    ''' </summary>
    ''' <returns>The result of the division.</returns>
    Public Shared Operator /(numerator As Impedance,
                             denominator As Impedance) As Impedance
        ' No input checking. numerator and denominator are presumed to have been
        ' checked when created.
        Dim QuotientC As System.Numerics.Complex =
            numerator.ToComplex / denominator.ToComplex
        Return New Impedance(QuotientC.Real, QuotientC.Imaginary)
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
    ''' representation in Cartesian form, using the specified numeric format and
    ''' culture-specific format information for its resistance and reactance
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
    ''' representation in Cartesian form, using the specified numeric format
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
    ''' representation in Cartesian form, using the specified culture-specific
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
    ''' representation in Cartesian form, using the default numeric format and
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
    ''' string representation in standard form, using the specified layout
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
    ''' string representation in standard form, using the specified layout
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
    ''' string representation in standard form, using the specified layout
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
    ''' string representation in standard form, using the default layout format,
    ''' numeric format, and culture-specific format information for its real and
    ''' imaginary parts.
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

        ' REF: How to write .NET objects as JSON (serialize)
        ' https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/how-to
        If IsNothing(jsonOptions) Then
            jsonString = System.Text.Json.JsonSerializer.Serialize(Me)
        Else
            jsonString = System.Text.Json.JsonSerializer.Serialize(Me, jsonOptions)
        End If

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

        ' REF: How to read JSON as .NET objects (deserialize)
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

#Region "Other Shared Methods"

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
        Return (loadZ.ToAdmittance() + addZ.ToAdmittance()).ToImpedance
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
    '    Return (loadZ.ToAdmittance() + addY).ToImpedance
    'End Function ' AddParallelAdmittance

#End Region ' "Other Shared Methods"

#Region "Other Instance Methods"

    ''' <summary>
    ''' Determines whether <see cref="Resistance"/> and <see cref="Reactance"/>
    ''' both equal 0.0.
    ''' </summary>
    ''' <returns><c>True</c> if <see cref="Resistance"/> and
    ''' <see cref="Reactance"/> both equal 0.0; otherwise,
    ''' <c>False</c>.</returns>
    Public Function IsZero() As System.Boolean
        Return Resistance.Equals(0.0) AndAlso Reactance.Equals(0.0)
    End Function ' IsZero

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
        ' rejections may need to be allowed. Work with pure reactances would
        ' need to allow for R=0.
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
        'ElseIf Impedance.EqualEnoughZero(resistance, TOLERANCE) Then
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
