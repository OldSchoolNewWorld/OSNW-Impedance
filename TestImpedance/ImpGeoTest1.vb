Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit

Namespace GeometryTests

    Public Class TestGetRadiusR

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)> ' NormR=3
        <InlineData(4.0, 8.0, 4.0, 50.0, 100.0, 2.0 / 3.0)> ' NormR=2
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)> ' NormR=1
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0 / 3.0)> ' NormR=1/2
        <InlineData(4.0, 8.0, 4.0, 75.0, 25.0, 1.5)> ' NormR=1/3
        Sub GetRadiusR_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testR As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.0)> ' NormR=0
        <InlineData(4.0, 8.0, 4.0, 1.0, -2.0)> ' NormR<=0
        Sub GetRadiusR_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testR As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
                End Sub)
        End Sub

    End Class ' TestGetRadiusR

    Public Class TestGetRadiusX

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 2.0 / 3.0)> ' NormX=3
        <InlineData(4.0, 8.0, 4.0, 50.0, 100.0, 1.0)> ' NormX=2
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 2.0)> ' NormX=1
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0)> ' NormX=1/2
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0 / 3.0, 6.0)> ' NormX=1/3
        <InlineData(4.0, 8.0, 4.0, 75.0, -25.0, 6.0)> ' NormX=-1/3
        <InlineData(4.0, 8.0, 4.0, 50.0, -25.0, 4.0)> ' NormX=-1/2
        <InlineData(4.0, 8.0, 4.0, 1.0, -1.0, 2.0)> ' NormX=-1
        <InlineData(4.0, 8.0, 4.0, 50.0, -100.0, 1.0)> ' NormX=-1/2
        <InlineData(4.0, 8.0, 4.0, 1.0, -3.0, 2.0 / 3.0)> ' NormX=-1/3
        Sub GetRadiusX_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testX As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.0)> ' NormX=0
        Sub GetRadiusX_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testX As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
                End Sub)
        End Sub

    End Class ' TestGetRadiusX

    Public Class TestGetRadiusG

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)> ' NormG=3
        <InlineData(4.0, 8.0, 4.0, 50.0, 2.0 * (1.0 / 50.0), 2.0 / 3.0)> ' NormG=2
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)> ' NormG=1
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0 / 3.0)> ' NormG=1/2
        <InlineData(4.0, 8.0, 4.0, 75.0, (1.0 / 75.0) / 3.0, 1.5)> ' NormG=1/3
        Sub GetRadiusG_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testG As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.0)> ' NormG=0
        <InlineData(4.0, 8.0, 4.0, 1.0, -2.0)> ' NormG=<0
        Sub GetRadiusG_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testG As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
                End Sub)
        End Sub

    End Class ' TestGetRadiusG

    Public Class TestGetRadiusB

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 2.0 / 3.0)> ' NormB=3
        <InlineData(4.0, 8.0, 4.0, 50.0, 2.0 * (1.0 / 50.0), 1.0)> ' NormB=2
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 2.0)> ' NormB=1
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0)> ' NormB=1/2
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0 / 3.0, 6.0)> ' NormB=1/3
        <InlineData(4.0, 8.0, 4.0, 75.0, -((1.0 / 75.0) / 3.0), 6.0)> ' NormB=-1/3
        <InlineData(4.0, 8.0, 4.0, 50.0, -((1.0 / 50.0) / 2.0), 4.0)> ' NormB=-1/2
        <InlineData(4.0, 8.0, 4.0, 1.0, -1.0, 2.0)> ' NormB=-1
        <InlineData(4.0, 8.0, 4.0, 50.0, -2.0 * (1.0 / 50.0), 1.0)> ' NormB=-2
        <InlineData(4.0, 8.0, 4.0, 1.0, -3.0, 2.0 / 3.0)> ' NormB=-3
        Sub GetRadiusB_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testB As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.0)> ' NormB=0
        Sub GetRadiusB_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testB As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
                End Sub)
        End Sub

    End Class ' TestGetRadiusB

    Public Class TestGetRadiusV

        Const Precision As Double = 0.000001

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 2.0, 2.0 / 3.0)> ' VSWR=3:1
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 1.0)> ' VSWR=2:1
        Sub GetRadiusV_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testV As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusV(testV)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, -1.0)> ' VSWR=<0
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 2.0)> ' VSWR<1
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0)> ' VSWR=1
        Sub GetRadiusV_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testV As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim RadiusAns As Double = SmithCirc.GetRadiusV(testV)
                End Sub)
        End Sub

    End Class ' TestGetRadiusV

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
        Sub GetIntersections_OneIntersections_Succeeds(
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
        Sub GetIntersections_Inside_NoIntersections(
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
        Sub GetIntersections_Outside_NoIntersections(
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

        <Theory>
        <InlineData(4.0, 5.0, 4.0, 1, 1, 1, 4.4, 5.8)> ' NormR above resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 1, 2, 5.0, 6.0)> ' NormR above resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 1, 0, 4.0, 5.0)> ' NormR at center point.
        <InlineData(4.0, 5.0, 4.0, 1, 3, 0, 5.0, 5.0)> ' NormR on resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 2, -2, 5.075, 4.385)> ' NormRBelow resonance line.
        Sub TryGetXYPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridDiameter As Double, z0 As Double,
            testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

            ' This loose precision may be needed due to the use of PointF in GetPlotXY.
            Const Precision As Double = 0.1

            Dim GridX, GridY As Double
            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)

            If Not SmithCirc.GetPlotXY(testR, testX, GridX, GridY) Then
                Assert.True(False)
            End If

            Assert.Equal(expectPlotX, GridX, Precision)
            Assert.Equal(expectPlotY, GridY, Precision)

        End Sub

        <Theory>
        <InlineData(4.0, 5.0, 4.0, 1.0, 0.0)> ' NormR=0
        <InlineData(4.0, 5.0, 4.0, 1.0, -2.0)> ' NormR<=0
        Public Sub TryGetPlotXY_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                               z0 As Double, testR As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
            Sub()
                ' Code that throws the exception
                Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
            End Sub)
        End Sub

    End Class ' TestGetPlotXY

    Public Class TestGetZFromPlot

        <Theory>
        <InlineData(4.0, 5.0, 4.0, 1, 4.4, 5.8, 1, 1)> ' NormR above resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 4.703, 5.218, 1, 2)> ' NormR above resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 4.0, 5.0, 1, 0)> ' NormR at center point.
        <InlineData(4.0, 5.0, 4.0, 1, 5.0, 5.0, 3, 0)> ' NormR on resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 5.075, 4.385, 2, -2)> ' NormRBelow resonance line.
        Public Sub GetZFromPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridDiameter As Double, z0 As Double,
            plotX As Double, plotY As Double,
            expectR As Double, expectX As Double)
            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
            Assert.Equal(expectR, ZAns.Resistance)
            Assert.Equal(expectX, ZAns.Reactance)
        End Sub

        <Fact>
        Public Sub GetZFromPlot_BadInput_Fails()
            '
            ' TRY GetZFromPlot WITH POINT OUTSIDE CIRCLE
            '
            '
            '
            Assert.True(False)
        End Sub

    End Class ' TestGetZFromPlot

End Namespace ' GeometryTests
