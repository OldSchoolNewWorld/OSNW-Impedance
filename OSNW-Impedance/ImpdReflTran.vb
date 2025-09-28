Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off
Imports System.Data

Partial Public Structure Impedance

#Region "Voltage Reflection"

    ''' <summary>
    ''' Calculates the complex voltage reflection coefficient (Gamma) when the
    ''' specified <paramref name="zLoad"/> <c>Impedance</c> is connected to the
    ''' specified <paramref name="zSource"/> <c>Impedance</c>.
    ''' </summary>
    ''' <param name="zSource">Specifies the impedance of the source.</param>
    ''' <param name="zLoad">Specifies the impedance of the load.</param>
    ''' <returns>The complex voltage reflection coefficient.</returns>
    ''' <remarks>The voltage reflection coefficient (Gamma) is a scalar value
    ''' with no dimension.</remarks>
    Public Shared Function VoltageReflectionComplexCoefficient(
        ByVal zSource As Impedance, ByVal zLoad As Impedance) _
        As System.Numerics.Complex

        ' REF: Reflection and Transmission Coefficients Explained
        ' https://www.rfwireless-world.com/terminology/reflection-and-transmission-coefficients
        ' has the numerator shown as "Zload - Zsource".

        ' REF: Mathematical Construction and Properties of the Smith Chart
        ' https://www.allaboutcircuits.com/technical-articles/mathematical-construction-and-properties-of-the-smith-chart/
        ' has mostly the same but with the numerator shown as "Zsource - Zload".

        Dim LoadCplx As System.Numerics.Complex = zLoad.ToComplex
        Dim SourceCplx As System.Numerics.Complex = zSource.ToComplex
        Return (LoadCplx - SourceCplx) / (LoadCplx + SourceCplx)

    End Function ' VoltageReflectionComplexCoefficient

    ''' <summary>
    ''' Calculates the complex voltage reflection coefficient (Gamma) when this
    ''' instance is connected to the specified
    ''' <paramref name="zSource"/> <c>Impedance</c>.
    ''' </summary>
    ''' <param name="zSource">Specifies the impedance of the source.</param>
    ''' <returns>The complex voltage reflection coefficient.</returns>
    ''' <remarks>The voltage reflection coefficient (Gamma) is a scalar value
    ''' with no dimension.</remarks>
    Public Function VoltageReflectionComplexCoefficient(ByVal zSource As Impedance) _
        As System.Numerics.Complex

        Return Impedance.VoltageReflectionComplexCoefficient(zSource, Me)
    End Function ' VoltageReflectionComplexCoefficient

    ''' <summary>
    ''' Calculates the complex voltage reflection coefficient (Gamma) when this
    ''' instance is connected to the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance, in ohms.</param>
    ''' <returns>The complex voltage reflection coefficient for the current
    ''' instance, based on the specified characteristic impedance.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>The voltage reflection coefficient (Gamma) is a scalar value
    ''' with no dimension.</remarks>
    Public Function VoltageReflectionComplexCoefficient(ByVal z0 As System.Double) _
        As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim SourceImp As New Impedance(z0, 0.0)
        Return Impedance.VoltageReflectionComplexCoefficient(SourceImp, Me)

    End Function ' VoltageReflectionComplexCoefficient

    ''' <summary>
    ''' Calculates the voltage reflection coefficient (Gamma) when this instance
    ''' is connected to the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance, in ohms.</param>
    ''' <returns>The voltage reflection coefficient for the current instance based
    ''' on the specified characteristic impedance.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>The voltage reflection coefficient (Gamma) is a scalar value
    ''' with no dimension.</remarks>
    Public Function VoltageReflectionCoefficient(ByVal z0 As System.Double) _
        As System.Double

        ' The underlying formula that was used here returns a Complex. This
        ' routine uses Complex.Magnitude() to return a Double, to match what is
        ' shown at the bottom of a Smith Chart.

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim SourceImp As New Impedance(z0, 0.0)
        Dim VRCC As System.Numerics.Complex =
            Impedance.VoltageReflectionComplexCoefficient(SourceImp, Me)
        Return VRCC.Magnitude

    End Function ' VoltageReflectionCoefficient

#End Region ' "Voltage Reflection"

#Region "Power Reflection"

    ''' <summary>
    ''' Calculates the complex power reflection coefficient (COMMON NAME???)
    ''' when the specified <paramref name="zLoad"/> <c>Impedance</c> is
    ''' connected to the specified <paramref name="zSource"/> <c>Impedance</c>.
    ''' </summary>
    ''' <param name="zSource">Specifies the impedance of the source.</param>
    ''' <param name="zLoad">Specifies the impedance of the load.</param>
    ''' <returns>The complex power reflection coefficient.</returns>
    ''' <remarks>The power reflection coefficient is a scalar value with no
    ''' dimension.</remarks>
    Public Shared Function PowerReflectionComplexCoefficient(
        ByVal zSource As Impedance, ByVal zLoad As Impedance) _
        As System.Numerics.Complex

        Dim Gamma As System.Numerics.Complex =
            Impedance.VoltageReflectionComplexCoefficient(zSource, zLoad)
        Return Gamma * Gamma
    End Function ' PowerReflectionComplexCoefficient

    ''' <summary>
    ''' Calculates the complex power reflection coefficient when this instance
    ''' is connected to the specified <paramref name="zSource"/>
    ''' <c>Impedance</c>.
    ''' </summary>
    ''' <param name="zSource">Specifies the impedance of the source.</param>
    ''' <returns>The complex power reflection coefficient.</returns>
    ''' <remarks>The power reflection coefficient is a scalar value with no
    ''' dimension.</remarks>
    Public Function PowerReflectionComplexCoefficient(ByVal zSource As Impedance) _
        As System.Numerics.Complex

        Dim Gamma As System.Numerics.Complex =
            Me.VoltageReflectionComplexCoefficient(zSource)
        Return Gamma * Gamma
    End Function ' PowerReflectionComplexCoefficient

    ''' <summary>
    ''' Calculates the complex power reflection coefficient when this instance
    ''' is connected to the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance, in ohms.</param>
    ''' <returns>The complex power reflection coefficient for the current
    ''' instance, based on the specified characteristic impedance.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>The power reflection coefficient is a scalar value with no
    ''' dimension.</remarks>
    Public Function PowerReflectionComplexCoefficient(ByVal z0 As System.Double) _
        As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim Gamma As System.Numerics.Complex =
            Me.VoltageReflectionComplexCoefficient(z0)
        Return Gamma * Gamma

    End Function ' PowerReflectionComplexCoefficient

    ''' <summary>
    ''' Calculates the power reflection coefficient when this instance is
    ''' connected to the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance, in ohms.</param>
    ''' <returns>The complex power reflection coefficient for the current
    ''' instance, based on the specified characteristic impedance.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>The power reflection coefficient is a scalar value with no
    ''' dimension.</remarks>
    Public Function PowerReflectionCoefficient(ByVal z0 As System.Double) _
        As System.Double

        ' The underlying formula that was used here returns a Complex. This
        ' routine uses Complex.Magnitude() to return a Double, to match what is
        ' shown at the bottom of a Smith Chart.

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim Gamma As System.Numerics.Complex =
            Me.VoltageReflectionComplexCoefficient(z0)
        Return (Gamma * Gamma).Magnitude

    End Function ' PowerReflectionCoefficient

#End Region ' "Power Reflection"

#Region "Voltage Transmission"

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="zSource">xxxxxxxxxx</param>
    ''' <param name="zLoad">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function VoltageTransmissionComplexCoefficient(
        ByVal zSource As Impedance, ByVal zLoad As Impedance) _
        As System.Numerics.Complex

        ' REF: Reflection and Transmission Coefficients Explained
        ' https://www.rfwireless-world.com/terminology/reflection-and-transmission-coefficients

        '        
        Dim LoadCplx As System.Numerics.Complex = zLoad.ToComplex
        Dim SourceCplx As System.Numerics.Complex = zSource.ToComplex
        Return 2.0 * LoadCplx / (LoadCplx + SourceCplx)

    End Function ' VoltageTransmissionComplexCoefficient

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="zSource">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Function VoltageTransmissionComplexCoefficient(ByVal zSource As Impedance) _
        As System.Numerics.Complex

        Return Impedance.VoltageTransmissionComplexCoefficient(zSource, Me)
    End Function ' VoltageTransmissionComplexCoefficient

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="z0">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    Public Function VoltageTransmissionComplexCoefficient(ByVal z0 As System.Double) _
        As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim SourceImp As New Impedance(z0, 0.0)
        Return Impedance.VoltageTransmissionComplexCoefficient(SourceImp, Me)

    End Function ' VoltageTransmissionComplexCoefficient

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="z0">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    Public Function VoltageTransmissionCoefficient(ByVal z0 As System.Double) _
        As System.Double

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim SourceImp As New Impedance(z0, 0.0)
        Dim VTCC As System.Numerics.Complex =
            Impedance.VoltageTransmissionComplexCoefficient(SourceImp, Me)
        Return VTCC.Magnitude

    End Function ' VoltageTransmissionCoefficient

#End Region ' "Voltage Transmission"

#Region "Power Transmission"

    ''' <summary>
    ''' Returns the complex power transmission coefficient when
    ''' <paramref name="zLoad"/> is connected to <paramref name="zSource"/>.
    ''' </summary>
    ''' <param name="zSource">Specifies the impedance of the source to which
    ''' <paramref name="zLoad"/> is connected .</param>
    ''' <param name="zLoad">Specifies the impedance of the load connected to
    ''' <paramref name="zSource"/>.</param>
    ''' <returns>
    ''' The complex power transmission coefficient when
    ''' <paramref name="zLoad"/> is connected to <paramref name="zSource"/>.
    ''' </returns>
    ''' <remarks>
    ''' NOTE: This function only matches the "TRANSM.COEFF.P" ruler on a Smith
    ''' Chart "for Zload= real, i.e. the case of a step in characteristic
    ''' impedance of the coaxial line."
    ''' </remarks>
    Public Shared Function PowerTransmissionComplexCoefficient(
        ByVal zSource As Impedance, ByVal zLoad As Impedance) _
        As System.Numerics.Complex

        ' REF: C1-Navigation_smith2.pdf
        ' https://indico.cern.ch/event/216963/sessions/35851/attachments/347577/484627/C1-Navigation_smith2.pdf
        ' Slide 8 says "This ruler is only valid for Zload= real, i.e. the case
        ' of a step in characteristic impedance of the coaxial line." That
        ' appears to explain why the normal set of test cases had so many
        ' failures when trying to match against the "TRANSM.COEFF.P" ruler on a
        ' Smith Chart.
        ' At least for now, it is being assumed that the formula itself is valid
        ' for all impedances and that the restriction is only with regard the
        ' the Smith Chart showing only a special case.

        ' REF: Google search AI results from search pattern:
        '     impedance power transmission coefficient formula
        ' T = 4*Z2*Z1 / (Z1 + Z2)^2

        'Dim CSource As System.Numerics.Complex = zSource.ToComplex
        'Dim CLoad As System.Numerics.Complex = zLoad.ToComplex
        'Dim Sum As System.Numerics.Complex = CSource + CLoad
        'Dim Num As System.Numerics.Complex = 4.0 * CSource * CLoad
        'Dim Den As System.Numerics.Complex = Sum * Sum
        'Dim PTCC As System.Numerics.Complex = Num / Den
        'Return PTCC

        Dim CSource As System.Numerics.Complex = zSource.ToComplex
        Dim CLoad As System.Numerics.Complex = zLoad.ToComplex
        Dim Sum As System.Numerics.Complex = CSource + CLoad
        Return 4.0 * CSource * CLoad / (Sum * Sum)

    End Function ' PowerTransmissionComplexCoefficient

    ''' <summary>
    ''' Returns the complex power transmission coefficient when this instance is
    ''' connected to <paramref name="zSource"/>.
    ''' </summary>
    ''' <param name="zSource">Specifies the impedance of the source to which
    ''' this instance is connected .</param>
    ''' <returns>
    ''' The complex power transmission coefficient when this instance is
    ''' connected to <paramref name="zSource"/>.
    ''' </returns>
    ''' <remarks>See
    ''' <see cref="PowerTransmissionComplexCoefficient(Impedance, Impedance)"/>
    ''' regarding comparison to Smith Chart results.</remarks>
    Public Function PowerTransmissionCoefficient(ByVal zSource As Impedance) _
        As System.Numerics.Complex

        Return Impedance.PowerTransmissionComplexCoefficient(zSource, Me)
    End Function ' PowerTransmissionComplexCoefficient

    ''' <summary>
    ''' Returns the complex power transmission coefficient when this instance is
    ''' connected to a source matching the specified characteristic impedance,
    ''' <paramref name="z0"/>.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance source to which
    ''' this instance is connected.</param>
    ''' xxxx
    ''' <returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' The complex power transmission coefficient when this instance is
    ''' connected to a source matching the specified characteristic impedance
    ''' </returns>
    ''' <remarks>See
    ''' <see cref="PowerTransmissionComplexCoefficient(Impedance, Impedance)"/>
    ''' regarding comparison to Smith Chart results.</remarks>
    Public Function PowerTransmissionComplexCoefficient(ByVal z0 As System.Double) _
        As System.Numerics.Complex

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Return Impedance.PowerTransmissionComplexCoefficient(
            New Impedance(z0, 0.0), Me)

    End Function ' PowerTransmissionComplexCoefficient

    ''' <summary>
    ''' Returns the magnitude of the complex power transmission coefficient when
    ''' this instance is connected to a source matching the specified
    ''' characteristic impedance, <paramref name="z0"/>.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which this
    ''' instance is connected.</param>
    ''' <returns>
    ''' The magnitude of the complex power transmission coefficient when
    ''' this instance is connected to a source matching the specified
    ''' characteristic impedance <paramref name="z0"/>.
    ''' </returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>See
    ''' <see cref="PowerTransmissionComplexCoefficient(Impedance, Impedance)"/>
    ''' regarding comparison to Smith Chart results.</remarks>
    Public Function PowerTransmissionCoefficient(ByVal z0 As System.Double) _
        As System.Double

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Transmission.MethodBase =
            '    System.Transmission.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim SourceImp As New Impedance(z0, 0.0)
        Dim PTCC As System.Numerics.Complex =
            Impedance.PowerTransmissionComplexCoefficient(SourceImp, Me)
        Return PTCC.Magnitude

    End Function ' PowerTransmissionCoefficient

#End Region ' "Power Transmission"

#Region "Angle of Reflection"

    ' ARE THE AngleOfReflection AND AngleOfTransmission ROUTINES UNIQUE, OR
    ' SHARED, FOR VOLTAGE AND POWER? CURRENT?
    ' IS THERE A MATHEMATICAL FORMULA TO USE THAT MAY BE BETTER THAN DOING THE GEOMETRY?

    ''' <summary>
    ''' Returns the angle of reflection, in radians, when this instance is
    ''' connected to a source with the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance.</param>
    ''' <returns>
    ''' The angle of reflection, in radians, when this instance is
    ''' connected to a source with the specified characteristic impedance.
    ''' </returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>
    ''' Results range from PI to almost -PI. Inductive reactances return
    ''' positive values that rotate CCW, and capacitive reactances return
    ''' negative values that rotate CW, from the open circuit point.
    ''' A plot at the matched center point has no reflection, but returns 0.0.
    ''' </remarks>
    Public Function AngleOfReflectionRadians(ByVal z0 As System.Double) _
        As System.Double

        Const PI As System.Double = System.Double.Pi
        Const HALFPI As System.Double = System.Double.Pi / 2.0

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.

        Dim PlotX As System.Double
        Dim PlotY As System.Double
        If Not MainCirc.GetPlotXY(Me.Resistance, Me.Reactance,
                                  PlotX, PlotY) Then
            Throw New ApplicationException(MSGFGPXPY)
        End If

        Dim Opposite As System.Double = PlotY - MainCirc.GridCenterY
        Dim Adjacent As System.Double = PlotX - MainCirc.GridCenterX
        Dim TanAlpha As System.Double
        Dim RadAngle As System.Double

        If PlotX > MainCirc.GridCenterX Then
            ' Right side.
            TanAlpha = Opposite / Adjacent
            RadAngle = System.Math.Atan(TanAlpha)
            Return RadAngle
        ElseIf PlotX < MainCirc.GridCenterX Then
            ' Left side.
            TanAlpha = Opposite / Adjacent
            RadAngle = System.Math.Atan(TanAlpha)
            If Opposite < 0.0 Then
                ' Below the resonance line.
                Return RadAngle - PI
            Else
                ' On or above the resonance line.
                Return PI + RadAngle
            End If
        Else
            ' Vertical will have zero as the adjacent side.
            If PlotY > MainCirc.GridCenterY Then
                ' Above the resonance line.
                Return HALFPI
            ElseIf PlotY < MainCirc.GridCenterY Then
                ' Below the resonance line.
                Return -HALFPI
            Else
                ' On the resonance line, at the center.
                ' MATCHED, SO NO REFLECTION. SHOULD THIS HAVE *ANY* VALUE? NOT
                ' 90 OR -90 DEGREES, SO USE ZERO FOR NOW.
                Return 0.0
            End If
        End If

    End Function ' AngleOfReflectionRadians

    ''' <summary>
    ''' Returns the angle of reflection, in degrees, when this instance is
    ''' connected to a source with the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance.</param>
    ''' <returns>
    ''' The angle of reflection, in degrees, when this instance is
    ''' connected to a source with the specified characteristic impedance.
    ''' </returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>
    ''' Results range from 180 to almost -180. Inductive reactances return
    ''' positive values that rotate CCW, and capacitive reactances return
    ''' negative values that rotate CW, from the open circuit point.
    ''' A plot at the matched center point has no reflection, but returns 0.0.
    ''' A standard Smith Chart is marked with the angles shown in degrees.
    ''' </remarks>
    Public Function AngleOfReflection(ByVal z0 As System.Double) _
        As System.Double

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Return Me.AngleOfReflectionRadians(z0) * 180.0 / System.Math.PI

    End Function ' AngleOfReflection

#End Region ' "Angle of Reflection"

#Region "Angle of Transmission"

    ' ARE THE AngleOfReflection AND AngleOfTransmission ROUTINES UNIQUE, OR
    ' SHARED, FOR VOLTAGE AND POWER? CURRENT?
    ' IS THERE A MATHEMATICAL FORMULA TO USE THAT MAY BE BETTER THAN DOING THE GEOMETRY?

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    Public Function AngleOfTransmissionRadians(ByVal z0 As System.Double) As System.Double

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.

        Dim PlotX As System.Double
        Dim PlotY As System.Double
        If Not MainCirc.GetPlotXY(Me.Resistance, Me.Reactance,
                                  PlotX, PlotY) Then
            Throw New ApplicationException(MSGFGPXPY)
        End If

        Dim Opposite As System.Double = PlotY - MainCirc.GridCenterY
        Dim Adjacent As System.Double = PlotX - MainCirc.GridLeftEdgeX
        Dim TanAlpha As System.Double
        Dim RadAngle As System.Double

        If PlotY < MainCirc.GridCenterY Then
            TanAlpha = Opposite / Adjacent
            RadAngle = System.Math.Atan(TanAlpha)
            Return RadAngle
        ElseIf PlotY > MainCirc.GridCenterY Then
            TanAlpha = Opposite / Adjacent
            RadAngle = System.Math.Atan(TanAlpha)
            Return RadAngle
        Else
            TanAlpha = Opposite / Adjacent
            RadAngle = System.Math.Atan(TanAlpha)
            Return RadAngle
        End If

    End Function ' AngleOfTransmissionRadians

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <remarks>An original Smith Chart is marked with the angles shown in
    ''' degrees.</remarks>
    Public Function AngleOfTransmission(ByVal z0 As System.Double) As System.Double

        ' Input checking.
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        Return Me.AngleOfTransmissionRadians(z0) * 180.0 / System.Math.PI

    End Function ' AngleOfTransmission

#End Region ' "Angle of Transmission"

#Region "VSWR"

    ' NEED/WANT TO ADD MULTIPLE VERSIONS AS DONE WITH VoltageReflectionComplexCoefficient
    ' AND PowerReflectionComplexCoefficient?
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
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
        ElseIf Double.IsInfinity(z0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
        End If

        ' REF:
        ' https://www.antenna-theory.com/definitions/vswr.php
        ' https://www.antenna-theory.com/definitions/vswr-calculator.php
        ' https://www.microwaves101.com/encyclopedias/voltage-standing-wave-ratio-vswr

        'Dim Gamma As System.Numerics.Complex = Me.VoltageReflectionComplexCoefficient(z0)
        'Dim AbsGamma As System.Double = System.Numerics.Complex.Abs(Gamma)
        Dim AbsGamma As System.Double =
            System.Numerics.Complex.Abs(Me.VoltageReflectionComplexCoefficient(z0))
        Return (1.0 + AbsGamma) / (1.0 - AbsGamma)

    End Function ' VSWR

#End Region ' "VSWR"

End Structure ' Impedance
