Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit

Namespace GeometryTests

    Public Class TestGetRadiusR

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R, RadiusR)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000)> ' C: At the open circuit point on the right.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' D: At the center.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 2.0 / 3)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 2.0 / 3)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.5)> ' H: Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 2.0 / 3)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0 / 3)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0 / 3)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1.5)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 1.5)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1.5)> ' M: Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0 / 3)> ' N: Inside G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 5.0 / 3)> ' O: In the top remainder.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, 1.4286)> ' P: In the bottom remainder.
        Sub GetRadiusR_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testR As Double, expectRadR As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
            Assert.Equal(expectRadR, RadiusAns, Precision)
        End Sub

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R, RadiusR)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 2.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 2.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.0345, 999)> ' Q: Outside of main circle. Invalid.
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999)> ' R: NormR<=0. Invalid.
        Sub GetRadiusR_BadInput_Fails1(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                       z0 As Double, testR As Double, expectRadR As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
                    Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
                    Assert.Equal(expectRadR, RadiusAns, Precision)
                End Sub)
        End Sub

        ''<InlineData(ChartX, ChartY, ChartRad,      Z0,        R, RadiusR)> ' Model
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 2.0)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 2.0)> ' B: Anywhere else on the perimeter. R=0.0.
        '<InlineData(4.0, 5.0, 2.0, 1.0, -0.0345, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999)> ' R: NormR<=0. Invalid.
        'Sub GetRadiusR_BadInput_Fails2(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
        '                               z0 As Double, testR As Double, expectRadR As Double)
        '    Try
        '        ' Code that throws the exception
        '        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
        '        Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
        '        Assert.Equal(expectRadR, RadiusAns, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetRadiusR

    Public Class TestGetRadiusX

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       X, RadiusX)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 2.0)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 1.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 50.0, 25.0, 4.0)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 1.0)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, -1 / 2.0, 4.0)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 6.0)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 6.0)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(4.0, 5.0, 2.0, 1.0, -1 / 3.0, 6.0)> ' N: Inside G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.4, 1.4286)> ' O: In the top remainder.
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.8, 2.5)> ' P: In the bottom remainder.
        Sub GetRadiusX_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testX As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999)> ' R: NormR<=0. Invalid.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       X, RadiusX)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' C: At the open circuit point on the right.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' D: At the center.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' H: Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' M: Inside G=Y0 circle, on line
        Sub GetRadiusX_BadInput_Fails1(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                       z0 As Double, testX As Double, expectRad As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
                    Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
                    Assert.Equal(expectRad, RadiusAns, Precision)
                End Sub)
        End Sub

        ''<InlineData(4.0, 5.0, 2.0, 1.0, 0.4138, 999)> ' Q: Outside of main circle. Invalid.
        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999)> ' R: NormR<=0. Invalid.
        ''<InlineData(ChartX, ChartY, ChartRad,      Z0,       X, RadiusX)> ' Model
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' C: At the open circuit point on the right.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' D: At the center.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' H: Inside R=Z0 circle, on line
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' M: Inside G=Y0 circle, on line
        'Sub GetRadiusX_BadInput_Fails2(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
        '                               z0 As Double, testX As Double, expectRad As Double)
        '    Try
        '        ' Code that throws the exception
        '        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
        '        Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
        '        Assert.Equal(expectRad, RadiusAns, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetRadiusX

    Public Class TestGetRadiusG

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       G, RadiusG)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' D: At the center.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.5, 4.0 / 3)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 5.0 / 3)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 8 / 17.0, 1.36)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 50.0, 4 / 425.0, 1.36)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0 / 3, 1.5)> ' H: Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.25, 1.6)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.5, 0.8)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 75.0, 0.02, 0.8)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.5)> ' M: Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 18 / 13.0, 0.8387)> ' N: Inside G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.1, 1.8182)> ' O: In the top remainder.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.5, 4.0 / 3)> ' P: In the bottom remainder.
        Sub GetRadiusG_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testG As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       G, RadiusG)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 2.0)> ' C: At the open circuit point on the right.
        Sub GetRadiusG_BadInput_Fails1(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                       z0 As Double, testG As Double, expectRad As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
                    Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
                    Assert.Equal(expectRad, RadiusAns, Precision)
                End Sub)
        End Sub

        ''<InlineData(ChartX, ChartY, ChartRad,      Z0,       G, RadiusG)> ' Model
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Sub GetRadiusG_BadInput_Fails2(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
        '                               z0 As Double, testG As Double, expectRad As Double)
        '    Try
        '        ' Code that throws the exception
        '        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
        '        Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
        '        Assert.Equal(expectRad, RadiusAns, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetRadiusG

    Public Class TestGetRadiusB

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        B, RadiusB)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 1.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.5, 4.0)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, 5.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, -2 / 17.0, 17.0)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 50.0, -1 / 425.0, 17.0)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.25, 8.0)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(4.0, 5.0, 2.0, 1.0, -1.0, 2.0)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 2.0)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, -1.5, 1.3333)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 75.0, -0.02, 1.3333)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(4.0, 5.0, 2.0, 1.0, 12 / 13.0, 2.1666)> ' N: Inside G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.7, 2.8571)> ' O: In the top remainder.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 2.0)> ' P: In the bottom remainder.
        Sub GetRadiusB_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testB As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        B, RadiusB)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' C: At the open circuit point on the right.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' D: At the center.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' H: Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, INF)> ' M: Inside G=Y0 circle, on line
        Sub GetRadiusB_BadInput_Fails1(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                       z0 As Double, testB As Double, expectRad As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
                    Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
                    Assert.Equal(expectRad, RadiusAns, Precision)
                End Sub)
        End Sub

        ''<InlineData(ChartX, ChartY, ChartRad,      Z0,        B, RadiusB)> ' Model
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999)> ' Q: Outside of main circle. Invalid.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999)> ' R: NormR<=0. Invalid.
        'Sub GetRadiusB_BadInput_Fails2(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
        '                               z0 As Double, testB As Double, expectRad As Double)
        '    Try
        '        ' Code that throws the exception
        '        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
        '        Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
        '        Assert.Equal(expectRad, RadiusAns, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetRadiusB

    Public Class TestGetIntersections

        <Theory>
        <InlineData(2.0, 8.0, 1.5, 3.5, 9.0, 1, 3.5, 8, 2.5769, 9.3846)>
        <InlineData(3.5, 9.0, 1, 2.0, 8.0, 1.5, 2.5769, 9.3846, 3.5, 8)>
        Sub GetIntersections_TwoIntersections_Succeeds(
            c1X As Double, c1Y As Double, c1R As Double,
            c2X As Double, c2Y As Double, c2R As Double,
            expect1x As Double, expect1y As Double,
            expect2x As Double, expect2y As Double)

            ' Testing via GetIntersections(otherCircle As GenericCircle) exercises all three overloads.
            ' NOTE: The order in which the circles are specified dictates the order in which the intersections are stored.
            Dim C1 As New GenericCircle(c1X, c1Y, c1R)
            Dim C2 As New GenericCircle(c2X, c2Y, c2R)
            Dim Intersections As System.Collections.Generic.List(Of System.Drawing.PointF) = C1.GetIntersections(C2)
            Assert.Equal(2, Intersections.Count)
            Assert.Equal(expect1x, Intersections(0).X, 0.01)
            Assert.Equal(expect1y, Intersections(0).Y, 0.01)
            Assert.Equal(expect2x, Intersections(1).X, 0.01)
            Assert.Equal(expect2y, Intersections(1).Y, 0.01)

        End Sub

        <Theory>
        <InlineData(2.0, 8.0, 1.5, 4.5, 8.0, 1, 3.5, 8, 3.5, 8)> ' Outside tangent.
        <InlineData(4.5, 8.0, 1, 2.0, 8.0, 1.5, 3.5, 8, 3.5, 8)> ' Outside tangent.
        <InlineData(2.0, 8.0, 1.5, 2.0, 5.5, 1, 2.0, 6.5, 2.0, 6.5)> ' Outside tangent.
        <InlineData(2.0, 5.5, 1, 2.0, 8.0, 1.5, 2.0, 6.5, 2.0, 6.5)> ' Outside tangent.
        <InlineData(2.0, 8.0, 1.5, 3.0, 8.0, 0.5, 3.5, 8, 3.5, 8)> ' Inside tangent.
        Sub GetIntersections_OneIntersection_Succeeds(
            c1X As Double, c1Y As Double, c1R As Double,
            c2X As Double, c2Y As Double, c2R As Double,
            expect1x As Double, expect1y As Double,
            expect2x As Double, expect2y As Double)

            ' Testing via GetIntersections(otherCircle As GenericCircle) exercises all three overloads.
            Dim C1 As New GenericCircle(c1X, c1Y, c1R)
            Dim C2 As New GenericCircle(c2X, c2Y, c2R)
            Dim Intersections As System.Collections.Generic.List(Of System.Drawing.PointF) = C1.GetIntersections(C2)
            Assert.Equal(2, Intersections.Count)
            Assert.Equal(expect1x, Intersections(0).X, 0.01)
            Assert.Equal(expect1y, Intersections(0).Y, 0.01)
            Assert.Equal(expect2x, Intersections(1).X, 0.01)
            Assert.Equal(expect2y, Intersections(1).Y, 0.01)
        End Sub

        <Theory>
        <InlineData(2.0, 8.0, 1.5, 2.5, 8.5, 0.5)>
        Sub GetIntersections_Inside_NoIntersections_Succeeds(
            c1X As Double, c1Y As Double, c1R As Double,
            c2X As Double, c2Y As Double, c2R As Double)

            ' Testing via GetIntersections(otherCircle As GenericCircle) exercises all three overloads.
            Dim C1 As New GenericCircle(c1X, c1Y, c1R)
            Dim C2 As New GenericCircle(c2X, c2Y, c2R)
            Dim Intersections As System.Collections.Generic.List(Of System.Drawing.PointF) = C1.GetIntersections(C2)
            Assert.Equal(0, Intersections.Count)
        End Sub

        <Theory>
        <InlineData(2.0, 8.0, 1.5, 4.0, 8.5, 0.5)>
        Sub GetIntersections_Outside_NoIntersections_Succeeds(
            c1X As Double, c1Y As Double, c1R As Double,
            c2X As Double, c2Y As Double, c2R As Double)

            ' Testing via GetIntersections(otherCircle As GenericCircle) exercises all three overloads.
            Dim C1 As New GenericCircle(c1X, c1Y, c1R)
            Dim C2 As New GenericCircle(c2X, c2Y, c2R)
            Dim Intersections As System.Collections.Generic.List(Of System.Drawing.PointF) = C1.GetIntersections(C2)
            Assert.Equal(0, Intersections.Count)
        End Sub

    End Class ' TestGetIntersections

    Public Class TestGetPlotXY

        Const Precision As Double = 0.0005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000, 6.0, 5.0)> ' C: At the open circuit point on the right.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0000, 4.0, 5.0)> ' D: At the center.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 4.4, 5.8)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, -2.0, 5.0, 4.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 1 / 2.0, 4.7027, 5.2162)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 25.0, 4.7027, 5.2162)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.0000, 5.0, 5.0)> ' H: Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, -2.0, 5.077, 4.3846)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 1 / 2.0, 3.6, 5.8)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 2.0, 3.6, 4.2)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1 / 3.0, 3.1765, 5.7059)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 25.0, 3.1765, 5.7059)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 0.0000, 3.0, 5.0)> ' M: Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 3.0, 3.4588, 4.4353)> ' N: Inside G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 1.4, 4.5882, 6.6471)> ' O: In the top remainder.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, -0.8, 3.8462, 3.7692)> ' P: In the bottom remainder.
        Sub TryGetPlotXY_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

            Dim GridX, GridY As Double
            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)

            If Not SmithCirc.GetPlotXY(testR, testX, GridX, GridY) Then
                Assert.True(False)
            End If

            Assert.Equal(expectPlotX, GridX, Precision)
            Assert.Equal(expectPlotY, GridY, Precision)

        End Sub

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 1 / 2.0, 2.8, 6.6)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.0345, 0.4138, 2.5, 6.5)> ' Q: Outside of main circle. Invalid.
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub TryGetPlotXY_BadInput_Fails1(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
            Sub()
                ' Code that throws the exception

                Dim GridX, GridY As Double
                Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)

                If Not SmithCirc.GetPlotXY(testR, testX, GridX, GridY) Then
                    Assert.True(False)
                End If

                Assert.Equal(expectPlotX, GridX, Precision)
                Assert.Equal(expectPlotY, GridY, Precision)

                Assert.True(False, "Did not fail")

            End Sub)
        End Sub

        ''<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 1 / 2.0, 2.8, 6.6)> ' B: Anywhere else on the perimeter. R=0.0.
        '<InlineData(4.0, 5.0, 2.0, 1.0, -0.0345, 0.4138, 2.5, 6.5)> ' Q: Outside of main circle. Invalid.
        '<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, 999, 999)> ' R: NormR<=0. Invalid.
        'Public Sub TryGetPlotXY_BadInput_Fails2(
        '    gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
        '    testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

        '    Try
        '        ' Code that throws the exception

        '        Dim GridX, GridY As Double
        '        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)

        '        If Not SmithCirc.GetPlotXY(testR, testX, GridX, GridY) Then
        '            Assert.True(False)
        '        End If

        '        Assert.Equal(expectPlotX, GridX, Precision)
        '        Assert.Equal(expectPlotY, GridY, Precision)

        '        Assert.True(False, "Did not fail")

        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetPlotXY

    Public Class TestGetZFromPlot

        ' The reduced precision here is due to the use of floating point values
        ' and the number of calculations in GetZFromPlot.
        '        Const Precision As Double = 0.0005
        '        Const Precision As Double = 0.001
        Const Precision As Double = 0.005
        Const INF As Double = Double.PositiveInfinity

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 1 / 2.0, 2.8, 6.6)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0000, 4.0, 5.0)> ' D: At the center.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 4.4, 5.8)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, -2.0, 5.0, 4.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 1 / 2.0, 4.7027, 5.2162)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 25.0, 4.7027, 5.2162)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.0000, 5.0, 5.0)> ' H: Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, -2.0, 5.077, 4.3846)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 1 / 2.0, 3.6, 5.8)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 2.0, 3.6, 4.2)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1 / 3.0, 3.1765, 5.7059)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 25.0, 3.1765, 5.7059)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 0.0000, 3.0, 5.0)> ' M: Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 3.0, 3.4588, 4.4353)> ' N: Inside G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 1.4, 4.5882, 6.6471)> ' O: In the top remainder.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, -0.8, 3.8462, 3.7692)> ' P: In the bottom remainder.
        Public Sub GetZFromPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectR As Double, expectX As Double,
            plotX As Double, plotY As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
            Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
            Assert.Equal(expectR, ZAns.Resistance, Precision)
            Assert.Equal(expectX, ZAns.Reactance, Precision)
        End Sub

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000, 6.0, 5.0)> ' C: At the open circuit point on the right.
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.0345, 0.4138, 2.5, 6.5)> ' Q: Outside of main circle. Invalid.
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub GetZFromPlot_BadInput_Fails1(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectR As Double, expectX As Double,
            plotX As Double, plotY As Double)
            ' Try GetZFromPlot with point outside circle.
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
                    Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
                    Assert.Equal(expectR, ZAns.Resistance, Precision)
                    Assert.Equal(expectX, ZAns.Reactance, Precision)
                End Sub)
        End Sub

        ''<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000, 6.0, 5.0)> ' C: At the open circuit point on the right.
        '<InlineData(4.0, 5.0, 2.0, 1.0, -0.0345, 0.4138, 2.5, 6.5)> ' Q: Outside of main circle. Invalid.
        '<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, 999, 999)> ' R: NormR<=0. Invalid.
        'Public Sub GetZFromPlot_BadInput_Fails2(
        '    gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
        '    expectR As Double, expectX As Double,
        '    plotX As Double, plotY As Double)

        '    Try
        '        ' Code that throws the exception
        '        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
        '        Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
        '        Assert.Equal(expectR, ZAns.Resistance, Precision)
        '        Assert.Equal(expectX, ZAns.Reactance, Precision)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")

        'End Sub

    End Class ' TestGetZFromPlot

    Public Class TestGetYFromPlot

        Const Precision As Double = 0.0005

        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: At the short circuit point. Omit - covered by B.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 6.0, 5.0)> ' C: At the open circuit point on the right.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 2.5, 6.5)> ' Q: Outside of main circle. Invalid.
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 999, 999)> ' R: NormR<=0. Invalid.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       G,        B,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, -2.0, 2.8, 6.6)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0000, 4.0, 5.0)> ' D: At the center.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.5, -0.5, 4.4, 5.8)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 0.4, 5.0, 4.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 8 / 17.0, -2 / 17.0, 4.7027, 5.2162)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 50.0, 4 / 425.0, -1 / 425.0, 4.7027, 5.2162)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0 / 3, 0.0000, 5.0, 5.0)> ' H: Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.25, 0.25, 5.077, 4.3846)> ' I: Inside R=Z0 circle, below resonance line.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, -1.0, 3.6, 5.8)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 3.6, 4.2)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.5, -1.5, 3.1765, 5.7059)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(4.0, 5.0, 2.0, 75.0, 0.02, -0.02, 3.1765, 5.7059)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.0000, 3.0, 5.0)> ' M: Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 18 / 13.0, 12 / 13.0, 3.4588, 4.4353)> ' N: Inside G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.1, -0.7, 4.5882, 6.6471)> ' O: In the top remainder.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.5, 1.0, 3.8462, 3.7692)> ' P: In the bottom remainder.
        Public Sub GetYFromPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectG As Double, expectB As Double,
            plotX As Double, plotY As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim YAns As Admittance = SmithCirc.GetYFromPlot(plotX, plotY)
            Assert.Equal(expectG, YAns.Conductance, Precision)
            Assert.Equal(expectB, YAns.Susceptance, Precision)
        End Sub

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       G,        B,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 6.0, 5.0)> ' C: At the open circuit point on the right.
        <InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 2.5, 6.5)> ' Q: Outside of main circle. Invalid.
        <InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub GetYFromPlot_BadInput_Fails1(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectG As Double, expectB As Double,
            plotX As Double, plotY As Double)

            ' Try GetYFromPlot with point outside circle.
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
            Sub()
                ' Code that throws the exception
                Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
                Dim YAns As Admittance = SmithCirc.GetYFromPlot(plotX, plotY)
                Assert.Equal(expectG, YAns.Conductance, Precision)
                Assert.Equal(expectB, YAns.Susceptance, Precision)
            End Sub)
        End Sub

        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       G,        B,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 6.0, 5.0)> ' C: At the open circuit point on the right.
        <InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 2.5, 6.5)> ' Q: Outside of main circle. Invalid.
        <InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 999, 999)> ' R: NormR<=0. Invalid.
        Public Sub GetYFromPlot_BadInput_Fails2(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectG As Double, expectB As Double,
            plotX As Double, plotY As Double)

            Try
                ' Code that throws the exception
                Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
                Dim YAns As Admittance = SmithCirc.GetYFromPlot(plotX, plotY)
                Assert.Equal(expectG, YAns.Conductance, Precision)
                Assert.Equal(expectB, YAns.Susceptance, Precision)
            Catch ex As Exception
                Assert.True(True)
                Exit Sub
            End Try
            Assert.True(False, "Did not fail")
        End Sub

    End Class ' TestGetYFromPlot

End Namespace ' GeometryTests
