Imports OSNW.Numerics
Imports Xunit

Namespace ReflectionTests

    Public Class TestVoltageReflectionCoefficient

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(     Z0,        R,       X,    VRC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 1.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, 1.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D: At the center.
        <InlineData(1.0, 1.0, 1.0, 0.4472)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1.0, -2.0, 0.7071)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 2.0, 1 / 2.0, 0.3676)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 0.3676)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.5)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 2.0, -2.0, 0.62)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.4472)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.4472)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.5423)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 0.5423)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.5)> ' M: Inside G=Y0 circle, on line.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.3911)> ' N: Inside G=Y0 circle, below line.
        <InlineData(1.0, 0.2, 1.4, 0.8745)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 0.8745)> ' O2: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 0.62)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 0.62)> ' P2: In the bottom center. Z0=50.
        Public Sub VoltageReflectionCoefficient_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectVRC As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsVRC As Double = Impd.VoltageReflectionCoefficient(z0)
            Assert.Equal(expectVRC, AnsVRC, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,    VRC)> ' Model
        <Theory>
        <InlineData(1.0, INF, 0.0000, 1.0)> ' C: At the open circuit point on the right.
        <InlineData(1.0, -0.0345, 0.4138, 1.0)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub VoltageReflectionCoefficient_BadInput_Fails1(
            z0 As Double, r As Double, x As Double, expectVRC As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Impd As New Impedance(r, x)
                    Dim AnsVRC As Double = Impd.VoltageReflectionCoefficient(z0)
                    Assert.Equal(expectVRC, AnsVRC, Precision)
                End Sub)
        End Sub

        ''<InlineData(     Z0,        R,       X,    VRC)> ' Model
        '<Theory>
        '<InlineData(1.0, INF, 0.0000, 1.0)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, -0.0345, 0.4138, 1.0)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Public Sub VoltageReflectionCoefficient_BadInput_Fails2(
        '    z0 As Double, r As Double, x As Double, expectVRC As Double)

        '    Try
        '        ' Code that throws the exception.
        '        Dim Impd As New Impedance(r, x)
        '        Dim AnsVRC As Double = Impd.VoltageReflectionCoefficient(z0)
        '        Assert.Equal(expectVRC, AnsVRC, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestVoltageReflectionCoefficient

    Public Class TestPowerReflectionCoefficient

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(     Z0,        R,       X,    PRC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 1.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, 1.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D: At the center.
        <InlineData(1.0, 1.0, 1.0, 0.2)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1.0, -2.0, 0.5)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 2.0, 1 / 2.0, 0.1351)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 0.1351)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.25)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 2.0, -2.0, 0.3846)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.2)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.2)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.2941)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 0.2941)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.25)> ' M: Inside G=Y0 circle, on line.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.1529)> ' N: Inside G=Y0 circle, below line.
        <InlineData(1.0, 0.2, 1.4, 0.7647)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 0.7647)> ' O2: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 0.3846)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 0.3846)> ' P2: In the bottom center. Z0=50.
        Public Sub PowerReflectionCoefficient_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectPRC As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsPRC As Double = Impd.PowerReflectionCoefficient(z0)
            Assert.Equal(expectPRC, AnsPRC, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,    PRC)> ' Model
        <Theory>
        <InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
        <InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub PowerReflectionCoefficient_BadInput_Fails1(
            z0 As Double, r As Double, x As Double, expectPRC As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Impd As New Impedance(r, x)
                    Dim AnsPRC As Double = Impd.PowerReflectionCoefficient(z0)
                    Assert.Equal(expectPRC, AnsPRC, Precision)
                End Sub)
        End Sub

        ''<InlineData(     Z0,        R,       X,    PRC)> ' Model
        '<Theory>
        '<InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Public Sub PowerReflectionCoefficient_BadInput_Fails2(
        '    z0 As Double, r As Double, x As Double, expectPRC As Double)

        '    Try
        '        ' Code that throws the exception.
        '        Dim Impd As New Impedance(r, x)
        '        Dim AnsPRC As Double = Impd.PowerReflectionCoefficient(z0)
        '        Assert.Equal(expectPRC, AnsPRC, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestPowerReflectionCoefficient

    Public Class TestVoltageTransmissionCoefficient

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(     Z0,        R,       X,    VTC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, 0.8944)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 1.0)> ' D: At the center.
        <InlineData(1.0, 1.0, 1.0, 1.2649)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1.0, -2.0, 1.5811)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 2.0, 1 / 2.0, 1.3557)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 1.3557)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 1.5)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 2.0, -2.0, 1.5689)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.8944)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.8944)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.686)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 0.686)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.5)> ' M: Inside G=Y0 circle, on line.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.7822)> ' N: Inside G=Y0 circle, below line.
        <InlineData(1.0, 0.2, 1.4, 1.534)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 1.534)> ' O2: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 1.1094)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 1.1094)> ' P2: In the bottom center. Z0=50.
        Public Sub VoltageTransmissionCoefficient_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectVTC As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsVTC As Double = Impd.VoltageTransmissionCoefficient(z0)
            Assert.Equal(expectVTC, AnsVTC, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,    VTC)> ' Model
        <Theory>
        <InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
        <InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub VoltageTransmissionCoefficient_BadInput_Fails1(
            z0 As Double, r As Double, x As Double, expectVTC As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Impd As New Impedance(r, x)
                    Dim AnsVTC As Double = Impd.VoltageTransmissionCoefficient(z0)
                    Assert.Equal(expectVTC, AnsVTC, Precision)
                End Sub)
        End Sub

        ''<InlineData(     Z0,        R,       X,    VTC)> ' Model
        '<Theory>
        '<InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Public Sub VoltageTransmissionCoefficient_BadInput_Fails2(
        '    z0 As Double, r As Double, x As Double, expectVTC As Double)

        '    Try
        '        ' Code that throws the exception.
        '        Dim Impd As New Impedance(r, x)
        '        Dim AnsVTC As Double = Impd.VoltageTransmissionCoefficient(z0)
        '        Assert.Equal(expectVTC, AnsVTC, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestVoltageTransmissionCoefficient

    Public Class TestPowerTransmissionCoefficient

        ' NOTE: There are no tests of bad inputs. See
        ' PowerTransmissionComplexCoefficient(Impedance, Impedance) regarding
        ' comparison to Smith Chart results. This only tests the data sets that conform.

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        ' These are not excluded because they all result in errors. They are not tested due to the note above.

        '<InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, 0.0000, 1 / 2.0, 0.0000)> ' B: Anywhere else on the perimeter. R=0.0.
        '<InlineData(1.0, 1.0, 1.0, 0.8)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        '<InlineData(1.0, 1.0, -2.0, 0.5)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        '<InlineData(1.0, 2.0, 1 / 2.0, 0.8649)> ' G1: Inside R=Z0 circle, above resonance line.
        '<InlineData(50.0, 100.0, 25.0, 0.8649)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        '<InlineData(1.0, 2.0, -2.0, 0.6154)> ' I: Inside R=Z0 circle, below resonance line.
        '<InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.8)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        '<InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.8)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        '<InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.7059)> ' L1: Inside G=Y0 circle, above resonance line.
        '<InlineData(75.0, 25.0, 25.0, 0.7059)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        '<InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.8471)> ' N: Inside G=Y0 circle, below line.
        '<InlineData(1.0, 0.2, 1.4, 0.2353)> ' O1: In the top center.
        '<InlineData(50.0, 10.0, 70.0, 0.2353)> ' O2: In the top center. Z0=50.
        '<InlineData(1.0, 0.4, -0.8, 0.6154)> ' P1: In the bottom center.
        '<InlineData(50.0, 20.0, -40.0, 0.6154)> ' P2: In the bottom center. Z0=50.
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.

        '<InlineData(     Z0,        R,       X,    PTC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 1.0, 0.0000, 1.0)> ' D: At the center.
        <InlineData(1.0, 3.0, 0.0000, 0.75)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.75)> ' M: Inside G=Y0 circle, on line.
        Public Sub PowerTransmissionCoefficient_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectPTC As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsPTC As Double = Impd.PowerTransmissionCoefficient(z0)
            Assert.Equal(expectPTC, AnsPTC, Precision)
        End Sub

    End Class ' TestPowerTransmissionCoefficient

    Public Class TestAngleOfReflection

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(     Z0,        R,       X,       AOR)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D: At the center.
        <InlineData(1.0, 1.0, 1.0, 63.4349)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1.0, -2.0, -45.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 2.0, 1 / 2.0, 17.1027)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 17.1027)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.0000)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 2.0, -2.0, -29.7449)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 116.5651)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, -116.5651)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 139.3987)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 139.3987)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 180.0)> ' M: Inside G=Y0 circle, on line.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, -133.7811)> ' N: Inside G=Y0 circle, below line.
        <InlineData(1.0, 0.2, 1.4, 70.34617)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 70.34617)> ' O2: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, -97.125)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, -97.125)> ' P2: In the bottom center. Z0=50.
        Public Sub AngleOfReflection_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectAOR As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsAOR As Double = Impd.AngleOfReflection(z0)
            Assert.Equal(expectAOR, AnsAOR, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,       AOR)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, INF)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, INF, 0.0000, INF)> ' C: At the open circuit point on the right.
        <InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub AngleOfReflection_BadInput_Fails1(
            z0 As Double, r As Double, x As Double, expectAOR As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Impd As New Impedance(r, x)
                    Dim AnsAOR As Double = Impd.AngleOfReflection(z0)
                    Assert.Equal(expectAOR, AnsAOR, Precision)
                End Sub)
        End Sub

        ''<InlineData(     Z0,        R,       X,       AOR)> ' Model
        '<Theory>
        '<InlineData(1.0, 0.0000, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(1.0, 0.0000, 1 / 2.0, INF)> ' B: Anywhere else on the perimeter. R=0.0.
        '<InlineData(1.0, INF, 0.0000, INF)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Public Sub AngleOfReflection_BadInput_Fails2(
        '    z0 As Double, r As Double, x As Double, expectAOR As Double)

        '    Try
        '        ' Code that throws the exception.
        '        Dim Impd As New Impedance(r, x)
        '        Dim AnsAOR As Double = Impd.AngleOfReflection(z0)
        '        Assert.Equal(expectAOR, AnsAOR, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestAngleOfReflection

    Public Class TestAngleOfTransmission

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(     Z0,        R,       X,      AOT)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D: At the center.
        <InlineData(1.0, 1.0, 1.0, 18.435)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1.0, -2.0, -18.435)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 2.0, 1 / 2.0, 4.5739)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 4.5739)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.0000)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 2.0, -2.0, -11.3099)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 26.5651)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, -26.5651)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 30.9638)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 30.9638)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.0000)> ' M: Inside G=Y0 circle, on line.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, -21.1613)> ' N: Inside G=Y0 circle, below line.
        <InlineData(1.0, 0.2, 1.4, 32.4712)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 32.4712)> ' O2: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, -33.6901)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, -33.6901)> ' P2: In the bottom center. Z0=50.
        Public Sub AngleOfTransmission_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectAOT As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsAOT As Double = Impd.AngleOfTransmission(z0)
            Assert.Equal(expectAOT, AnsAOT, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,      AOT)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 2.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, 2.8)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, INF, 0.0000, 6.0)> ' C: At the open circuit point on the right.
        <InlineData(1.0, -0.0345, 0.4138, 2.5)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub AngleOfTransmission_BadInput_Fails1(
         z0 As Double, r As Double, x As Double, expectAOT As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Impd As New Impedance(r, x)
                    Dim AnsAOT As Double = Impd.AngleOfTransmission(z0)
                    Assert.Equal(expectAOT, AnsAOT, Precision)
                End Sub)

        End Sub

        ''<InlineData(     Z0,        R,       X,      AOT)> ' Model
        '<Theory>
        '<InlineData(1.0, 0.0000, 0.0000, 2.0)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(1.0, 0.0000, 1 / 2.0, 2.8)> ' B: Anywhere else on the perimeter. R=0.0.
        '<InlineData(1.0, INF, 0.0000, 6.0)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, -0.0345, 0.4138, 2.5)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Public Sub AngleOfTransmission_BadInput_Fails2(
        ' z0 As Double, r As Double, x As Double, expectAOT As Double)

        '    Try
        '        ' Code that throws the exception.
        '        Dim Impd As New Impedance(r, x)
        '        Dim AnsAOT As Double = Impd.AngleOfTransmission(z0)
        '        Assert.Equal(expectAOT, AnsAOT, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestAngleOfTransmission

    Public Class TestVSWR

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(     Z0,        R,       X,    VSWR)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, INF)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 1.0)> ' D: At the center.
        <InlineData(1.0, 1.0, 1.0, 2.618)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1.0, -2.0, 5.8284)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 2.0, 1 / 2.0, 2.1626)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 2.1626)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 3.0)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 2.0, -2.0, 4.2656)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 2.618)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 2.618)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 3.3699)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 3.3699)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 3.0)> ' M: Inside G=Y0 circle, on line.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 2.2845)> ' N: Inside G=Y0 circle, below line.
        <InlineData(1.0, 0.2, 1.4, 14.933)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 14.933)> ' O2: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 4.2656)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 4.2656)> ' P2: In the bottom center. Z0=50.
        Public Sub VSWR_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectVSWR As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsVWSR As Double = Impd.VSWR(z0)
            Assert.Equal(expectVSWR, AnsVWSR, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,    VSWR)> ' Model
        <Theory>
        <InlineData(1.0, INF, 0.0000, INF)> ' C: At the open circuit point on the right.
        <InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        Sub VSWR_BadInput_Fails1(
            z0 As Double, r As Double, x As Double, expectVSWR As Double)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Impd As New Impedance(r, x)
                    Dim AnsVWSR As Double = Impd.VSWR(z0)
                    Assert.Equal(expectVSWR, AnsVWSR, Precision)
                End Sub)
        End Sub

        ''<InlineData(     Z0,        R,       X,    VSWR)> ' Model
        '<Theory>
        '<InlineData(1.0, INF, 0.0000, INF)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Sub VSWR_BadInput_Fails2(
        '    z0 As Double, r As Double, x As Double, expectVSWR As Double)
        '    Try
        '        ' Code that throws the exception.
        '        Dim Impd As New Impedance(r, x)
        '        Dim AnsVWSR As Double = Impd.VSWR(z0)
        '        Assert.Equal(expectVSWR, AnsVWSR, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestVSWR

End Namespace ' ReflectionTests
