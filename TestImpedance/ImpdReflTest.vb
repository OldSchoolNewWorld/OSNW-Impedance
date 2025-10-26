Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit

Namespace ReflectionTests

    Public Class TestVoltageReflectionCoefficient

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(     Z0,        R,         X,    VRC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 1.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, 1.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000, 0.0000)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 1.0, 1.0, 0.4472)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, 50.0, 0.4472)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1.0, -2.0, 0.7071)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, -100.0, 0.7071)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 2.0, 1 / 2.0, 0.3676)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 0.3676)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.5)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000, 0.5)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 2.0, -2.0, 0.62)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(50.0, 100.0, -100.0, 0.62)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.4472)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, 25.0, 0.4472)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.4472)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, -25.0, 0.4472)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.5423)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 0.5423)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.5)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000, 0.5)> ' M75: Inside G=Y0 circle, on line. Z0=75.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.3911)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(75.0, 37.5, -25.0, 0.3911)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        <InlineData(1.0, 0.2, 1.4, 0.8745)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 0.8745)> ' O50: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 0.62)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 0.62)> ' P50: In the bottom center. Z0=50.
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

        '<InlineData(     Z0,        R,         X,    PRC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 1.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, 1.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000, 0.0000)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 1.0, 1.0, 0.2)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, 50.0, 0.2)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1.0, -2.0, 0.5)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, -100.0, 0.5)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 2.0, 1 / 2.0, 0.1351)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 0.1351)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.25)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000, 0.25)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 2.0, -2.0, 0.3846)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(50.0, 100.0, -100.0, 0.3846)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.2)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, 25.0, 0.2)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.2)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, -25.0, 0.2)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.2941)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 0.2941)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.25)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000, 0.25)> ' M75: Inside G=Y0 circle, on line. Z0=75.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.1529)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(75.0, 37.5, -25.0, 0.1529)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        <InlineData(1.0, 0.2, 1.4, 0.7647)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 0.7647)> ' O50: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 0.3846)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 0.3846)> ' P50: In the bottom center. Z0=50.
        Public Sub PowerReflectionCoefficient_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectPRC As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsPRC As Double = Impd.PowerReflectionCoefficient(z0)
            Assert.Equal(expectPRC, AnsPRC, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,    PRC)> ' Model
        <Theory>
        <InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        <InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
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
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        '<InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
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

        '<InlineData(1.0, INF, 0.0000, 999)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(1.0, -2.0, 999, 999)> ' R: NormR<=0. Invalid.
        '<InlineData(     Z0,        R,         X,    VTC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, 0.8944)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 1.0)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000, 1.0)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 1.0, 1.0, 1.2649)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, 50.0, 1.2649)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1.0, -2.0, 1.5811)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, -100.0, 1.5811)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 2.0, 1 / 2.0, 1.3557)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 1.3557)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 1.5)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000, 1.5)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 2.0, -2.0, 1.5689)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(50.0, 100.0, -100.0, 1.5689)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.8944)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, 25.0, 0.8944)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.8944)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, -25.0, 0.8944)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.686)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 0.686)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.5)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000, 0.5)> ' M75: Inside G=Y0 circle, on line. Z0=75.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.7822)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(75.0, 37.5, -25.0, 0.7822)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        <InlineData(1.0, 0.2, 1.4, 1.534)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 1.534)> ' O50: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 1.1094)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 1.1094)> ' P50: In the bottom center. Z0=50.
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
        '<InlineData(1.0, 0.0000, 1 / 2.0, 0.0000)> ' B: Anywhere else on the perimeter. R=0.0.
        '<InlineData(1.0, 1.0, 1.0, 0.8)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        '<InlineData(50.0, 50.0, 50.0, 0.8)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
        '<InlineData(1.0, 1.0, -2.0, 0.5)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        '<InlineData(50.0, 50.0, -100.0, 0.5)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
        '<InlineData(1.0, 2.0, 1 / 2.0, 0.8649)> ' G1: Inside R=Z0 circle, above resonance line.
        '<InlineData(50.0, 100.0, 25.0, 0.8649)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        '<InlineData(1.0, 2.0, -2.0, 0.6154)> ' I1: Inside R=Z0 circle, below resonance line.
        '<InlineData(50.0, 100.0, -100.0, 0.6154)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        '<InlineData(1.0, 1 / 2.0, 1 / 2.0, 0.8)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        '<InlineData(50.0, 25.0, 25.0, 0.8)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
        '<InlineData(1.0, 1 / 2.0, -1 / 2.0, 0.8)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        '<InlineData(50.0, 25.0, -25.0, 0.8)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
        '<InlineData(1.0, 1 / 3.0, 1 / 3.0, 0.7059)> ' L1: Inside G=Y0 circle, above resonance line.
        '<InlineData(75.0, 25.0, 25.0, 0.7059)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        '<InlineData(1.0, 1 / 2.0, -1 / 3.0, 0.8471)> ' N1: Inside G=Y0 circle, below line.
        '<InlineData(75.0, 37.5, -25.0, 0.8471)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        '<InlineData(1.0, 0.2, 1.4, 0.2353)> ' O1: In the top center.
        '<InlineData(50.0, 10.0, 70.0, 0.2353)> ' O50: In the top center. Z0=50.
        '<InlineData(1.0, 0.4, -0.8, 0.6154)> ' P1: In the bottom center.
        '<InlineData(50.0, 20.0, -40.0, 0.6154)> ' P50: In the bottom center. Z0=50.
        '<InlineData(1.0, -0.0345, 0.4138, 999)> ' Q: Outside of main circle. Invalid.

        '<InlineData(Z0, R, X, PTC)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 1.0, 0.0000, 1.0)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000, 1.0)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 3.0, 0.0000, 0.75)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000, 0.75)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.75)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000, 0.75)> ' M75: Inside G=Y0 circle, on line. Z0=75.
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

        '<InlineData(     Z0,        R,         X,       AOR)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000, 0.0000)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 1.0, 1.0, 63.4349)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, 50.0, 63.4349)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1.0, -2.0, -45.0)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, -100.0, -45.0)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 2.0, 1 / 2.0, 17.1027)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 17.1027)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.0000)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000, 0.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 2.0, -2.0, -29.7449)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(50.0, 100.0, -100.0, -29.7449)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 116.5651)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, 25.0, 116.5651)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, -116.5651)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, -25.0, -116.5651)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 139.3987)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 139.3987)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 180.0)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000, 180.0)> ' M75: Inside G=Y0 circle, on line. Z0=75.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, -133.7811)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(75.0, 37.5, -25.0, -133.7811)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        <InlineData(1.0, 0.2, 1.4, 70.34617)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 70.34617)> ' O50: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, -97.125)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, -97.125)> ' P50: In the bottom center. Z0=50.
        Public Sub AngleOfReflection_GoodInput_Succeeds(
            z0 As Double, r As Double, x As Double, expectAOR As Double)

            Dim Impd As New Impedance(r, x)
            Dim AnsAOR As Double = Impd.AngleOfReflection(z0)
            Assert.Equal(expectAOR, AnsAOR, Precision)
        End Sub

        '<InlineData(     Z0,        R,       X,       AOR)> ' Model
        <Theory>
        <InlineData(1.0, INF, 0.0000, INF)> ' C: At the open circuit point on the right.
        <InlineData(1.0, 0.0000, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, INF)> ' B: Anywhere else on the perimeter. R=0.0.
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
        '<InlineData(1.0, INF, 0.0000, INF)> ' C: At the open circuit point on the right.
        '<InlineData(1.0, 0.0000, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(1.0, 0.0000, 1 / 2.0, INF)> ' B: Anywhere else on the perimeter. R=0.0.
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

        '<InlineData(     Z0,        R,         X,      AOT)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000, 0.0000)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000, 0.0000)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 1.0, 1.0, 18.435)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, 50.0, 18.435)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1.0, -2.0, -18.435)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, -100.0, -18.435)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 2.0, 1 / 2.0, 4.5739)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 4.5739)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 0.0000)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000, 0.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 2.0, -2.0, -11.3099)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(50.0, 100.0, -100.0, -11.3099)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 26.5651)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, 25.0, 26.5651)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, -26.5651)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, -25.0, -26.5651)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 30.9638)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 30.9638)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 0.0000)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000, 0.0000)> ' M75: Inside G=Y0 circle, on line. Z0=75.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, -21.1613)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(75.0, 37.5, -25.0, -21.1613)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        <InlineData(1.0, 0.2, 1.4, 32.4712)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 32.4712)> ' O50: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, -33.6901)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, -33.6901)> ' P50: In the bottom center. Z0=50.
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

        '<InlineData(     Z0,        R,         X,    VSWR)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0, INF)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 1.0, 0.0000, 1.0)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000, 1.0)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 1.0, 1.0, 2.618)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, 50.0, 2.618)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1.0, -2.0, 5.8284)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 50.0, -100.0, 5.8284)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 2.0, 1 / 2.0, 2.1626)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0, 2.1626)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000, 3.0)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000, 3.0)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 2.0, -2.0, 4.2656)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(50.0, 100.0, -100.0, 4.2656)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 2.618)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, 25.0, 2.618)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 2.618)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(50.0, 25.0, -25.0, 2.618)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 3.3699)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0, 3.3699)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000, 3.0)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000, 3.0)> ' M75: Inside G=Y0 circle, on line. Z0=75.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 2.2845)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(75.0, 37.5, -25.0, 2.2845)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        <InlineData(1.0, 0.2, 1.4, 14.933)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0, 14.933)> ' O50: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8, 4.2656)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0, 4.2656)> ' P50: In the bottom center. Z0=50.
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
