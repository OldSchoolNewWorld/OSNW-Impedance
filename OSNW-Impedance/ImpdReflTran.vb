Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

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



    ' xxxxxxxxxx NO EXPECTED RESULTS KNOWN FOR THESE YET. xxxxxxxxxx
    ' xxxxxxxxxx NO TEST SET UP FOR THESE YET. xxxxxxxxxx



    ' xxxxxxxxxxxxxxxxxxxxxxxx
    ' REF: Reflection and Transmission Coefficients Explained
    ' https://www.rfwireless-world.com/terminology/reflection-and-transmission-coefficients
    ' T = (2.0 * Zl) / (Zl + Zs)
    'xxxxxxxxxxxxxxxxxxxxxxxx



    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="zSource">xxxxxxxxxx</param>
    ''' <param name="zLoad">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function VoltageTransmissionCoefficient(
        ByVal zSource As Impedance, ByVal zLoad As Impedance) _
        As System.Numerics.Complex

        ' REF: Reflection and Transmission Coefficients Explained
        ' https://www.rfwireless-world.com/terminology/reflection-and-transmission-coefficients

        Dim LoadCplx As System.Numerics.Complex = zLoad.ToComplex
        Dim SourceCplx As System.Numerics.Complex = zSource.ToComplex
        Return 2.0 * LoadCplx / (LoadCplx + SourceCplx)

    End Function ' VoltageTransmissionCoefficient

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="zSource">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Function VoltageTransmissionCoefficient(ByVal zSource As Impedance) _
        As System.Numerics.Complex

        Return Impedance.VoltageTransmissionCoefficient(zSource, Me)
    End Function ' VoltageTransmissionCoefficient

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="z0">xxxxxxxxxx</param>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <returns>xxxxxxxxxx</returns>
    Public Function VoltageTransmissionCoefficient(ByVal z0 As System.Double) _
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

        '        Return Impedance.VoltageTransmissionCoefficient(New Impedance(z0, 0.0), Me)
        Dim SourceImp As New Impedance(z0, 0.0)
        Return Impedance.VoltageTransmissionCoefficient(SourceImp, Me)

    End Function ' VoltageTransmissionCoefficient

    '''' <summary>
    '''' xxxxxxxxxx
    '''' </summary>
    '''' <param name="z0">xxxxxxxxxx</param>
    '''' <exception cref="System.ArgumentOutOfRangeException">When
    '''' <paramref name="z0"/> is not a positive, non-zero value or is
    '''' infinite.</exception>
    '''' <returns>xxxxxxxxxx</returns>
    'Public Function VoltageTransmissionCoefficient(ByVal z0 As System.Double) _
    '    As System.Double

    '    ' Input checking.
    '    If z0 <= 0.0 Then
    '        'Dim CaughtBy As System.Transmission.MethodBase =
    '        '    System.Transmission.MethodBase.GetCurrentMethod
    '        Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGVMBGTZ)
    '    ElseIf Double.IsInfinity(z0) Then
    '        'Dim CaughtBy As System.Transmission.MethodBase =
    '        '    System.Transmission.MethodBase.GetCurrentMethod
    '        Throw New System.ArgumentOutOfRangeException(NameOf(z0), MSGCHIV)
    '    End If

    '    '        Return Impedance.VoltageTransmissionCoefficient(New Impedance(z0, 0.0), Me)
    '    Dim SourceImp As New Impedance(z0, 0.0)
    '    Return Impedance.VoltageTransmissionCoefficient(SourceImp, Me).Real

    'End Function ' VoltageTransmissionCoefficient

#End Region ' "Voltage Transmission"

#Region "Power Transmission"

    ' xxxxxxxxxxxxxxxxxxxxxxxx
    ' REF: Reflection and Transmission Coefficients Explained
    ' https://www.rfwireless-world.com/terminology/reflection-and-transmission-coefficients
    ' T = (2.0 * Zl) / (Zl + Zs)
    'xxxxxxxxxxxxxxxxxxxxxxxx

    ' xxxxxxxxxx NO TEST SET UP FOR THIS YET. xxxxxxxxxx
    ' xxxxxxxxxx NO EXPECTED RESULTS KNOWN FOR THIS YET. xxxxxxxxxx
    '
    '
    '
    '
    '



    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="zSource">xxxxxxxxxx</param>
    ''' <param name="zLoad">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function PowerTransmissionCoefficient(
        ByVal zSource As Impedance, ByVal zLoad As Impedance) _
        As System.Numerics.Complex

        ' REF: Reflection and Transmission Coefficients Explained
        ' https://www.rfwireless-world.com/terminology/reflection-and-transmission-coefficients

        Dim LoadCplx As System.Numerics.Complex = zLoad.ToComplex
        Dim SourceCplx As System.Numerics.Complex = zSource.ToComplex
        Return 2.0 * LoadCplx / (LoadCplx + SourceCplx)

    End Function ' PowerTransmissionCoefficient

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="zSource">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Function PowerTransmissionCoefficient(ByVal zSource As Impedance) _
        As System.Numerics.Complex

        Return Impedance.PowerTransmissionCoefficient(zSource, Me)
    End Function ' PowerTransmissionCoefficient

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="z0">xxxxxxxxxx</param>
    ''' <exception cref="System.ArgumentOutOfRangeException">When
    ''' <paramref name="z0"/> is not a positive, non-zero value or is
    ''' infinite.</exception>
    ''' <returns>xxxxxxxxxx</returns>
    Public Function PowerTransmissionCoefficient(ByVal z0 As System.Double) _
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

        '        Return Impedance.PowerTransmissionCoefficient(New Impedance(z0, 0.0), Me)
        Dim SourceImp As New Impedance(z0, 0.0)
        Return Impedance.PowerTransmissionCoefficient(SourceImp, Me)

    End Function ' PowerTransmissionCoefficient

#End Region ' "Power Transmission"

#Region "Angle of Reflection"

    ' ARE THE AngleOfReflection AND AngleOfTransmission ROUTINES UNIQUE, OR
    ' SHARED, FOR VOLTAGE AND POWER?

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    Public Function AngleOfReflectionRadians(ByVal z0 As System.Double) _
        As System.Double

        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.

        Dim PlotX As System.Double
        Dim PlotY As System.Double
        If Not MainCirc.GetPlotXY(Me.Resistance, Me.Reactance,
                                  PlotX, PlotY) Then
            Throw New ApplicationException("Failure getting PlotX, PlotY")
        End If

        Dim Opposite As System.Double = PlotY - MainCirc.GridCenterY
        Dim Adjacent As System.Double = PlotX - MainCirc.GridCenterX
        Dim TanAlpha As System.Double
        Dim RadAngle As System.Double

        If PlotX < MainCirc.GridCenterX Then
            ' Left side.
            TanAlpha = Opposite / Adjacent
            RadAngle = System.Math.Atan(TanAlpha)
            If PlotY > MainCirc.GridCenterY Then
                Return System.Math.PI + RadAngle
            ElseIf PlotY < MainCirc.GridCenterY Then
                Return -System.Math.PI + RadAngle
            Else
                ' On resonance line, left of center.
                Return System.Math.PI
            End If
        ElseIf PlotX > MainCirc.GridCenterX Then
            ' Right side.
            TanAlpha = Opposite / Adjacent
            RadAngle = System.Math.Atan(TanAlpha)
            Return RadAngle
        Else
            ' Vertical will have zero as the adjacent side.
            If PlotY > MainCirc.GridCenterY Then
                ' Above the resonance line.
                Return System.Math.PI / 2.0
            ElseIf PlotY < MainCirc.GridCenterY Then
                ' Below the resonance line.
                Return -System.Math.PI / 2.0
            Else
                ' On the resonance line, at the center.
                ' MATCHED, SO NO REFLECTION. SHOULD THIS HAVE *ANY* VALUE? NOT
                ' 90 OR -90 DEGREES, SO USE ZERO FOR NOW.
                Return 0.0
            End If
        End If

    End Function ' AngleOfReflectionRadians

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>An original Smith Chart is marked with the angles shown in
    ''' degrees.</remarks>
    Public Function AngleOfReflection(ByVal z0 As System.Double) _
        As System.Double

        Return Me.AngleOfReflectionRadians(z0) * 180.0 / System.Math.PI
    End Function ' AngleOfReflection

#End Region ' "Angle of Reflection"

#Region "Angle of Transmission"

    ' ARE THE AngleOfReflection AND AngleOfTransmission ROUTINES UNIQUE, OR
    ' SHARED, FOR VOLTAGE AND POWER?

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    Public Function AngleOfTransmissionRadians(ByVal z0 As System.Double) As System.Double

        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.

        Dim PlotX As System.Double
        Dim PlotY As System.Double
        If Not MainCirc.GetPlotXY(Me.Resistance, Me.Reactance,
                                  PlotX, PlotY) Then
            Throw New ApplicationException("Failure getting PlotX, PlotY")
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
    ''' <remarks>An original Smith Chart is marked with the angles shown in
    ''' degrees.</remarks>
    Public Function AngleOfTransmission(ByVal z0 As System.Double) As System.Double
        Return Me.AngleOfTransmissionRadians(z0) * 180.0 / System.Math.PI
    End Function ' AngleOfTransmission

#End Region ' "Angle of Transmission"









    ' NEED/WANT ADD MULTIPLE VERSIONS AS DONE WITH VoltageReflectionComplexCoefficient
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

End Structure ' Impedance
