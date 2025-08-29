Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit

Namespace GeometryTests

    Public Class TestGetRadiusR

        Const INF As Double = Double.PositiveInfinity

        '<InlineData(GridX, GridY, Radius,   Z0,     R, RadiusR)> ' Model
        '<InlineData(  4.0,   5.0,    2.0,  1.0,     R, RadiusR)> ' Base circle
        '
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0)> ' B: Open circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1.5)> ' D:
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 1.5)> ' E: NormZ 1/3 + j1/3
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' F: On R=Z0 circle
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 2 / 3.0)> ' G:
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 2 / 3.0)> ' H: NormZ 2 + j1/2
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1.5)> ' I:
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.5)> ' K:
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4 / 3.0)> ' L:
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 2 / 3.0)> ' M:
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4 / 3.0)> ' N: On G=Y0 circle
        Sub GetRadiusR_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testR As Double, expectRadR As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
            Assert.Equal(expectRadR, RadiusAns)
        End Sub

        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0)> ' A: Short circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0)> ' C: Perimeter
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0)> ' NormR<=0
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

        '<InlineData(GridX, GridY, Radius,   Z0,      X, RadiusX)> ' Model
        '<InlineData(  4.0,   5.0,    2.0,  1.0,      X, RadiusX)> ' Base circle
        ' <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 2.0)> ' C: Perimeter
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 2.0)> ' R=Z0 circle above line
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999)> ' R=Z0 circle below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0)> ' Inside R=Z0 circle below line
        <InlineData(4.0, 5.0, 2.0, 50.0, 25.0, 4.0)> ' Inside R=Z0 circle below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 999)> ' G=Y0 circle above line
        <InlineData(4.0, 5.0, 2.0, 1.0, -1 / 2.0, 4.0)> ' G=Y0 circle below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.4, 999)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.8, 2.0)> ' Bottom remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 6.0)> ' D:
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 6.0)> ' E: NormZ 1/3 + j1/3
        <InlineData(4.0, 5.0, 2.0, 1.0, -1 / 3.0, 6.0)> ' L:
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 1.0)> ' M:
        Sub GetRadiusX_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testX As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

        '<InlineData(4.0, 5.0, 2.0, 1.0, X, RadiusX)> ' Outside of circle
        '<InlineData(4.0, 5.0, 2.0, 1.0, X, RadiusX)> ' NormR<=0
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0, INF)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0, INF)> ' B: Open circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0, INF)> ' J: Center point
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0, INF)> ' Inside R=Z0 circle on line
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0, INF)> ' Inside G=Y0 circle

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
        Sub GetRadiusG_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testG As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius, z0)
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
        <InlineData(4.0, 8.0, 2.0, 1.0, 2.0, 2.0 / 3.0)> ' VSWR=3:1
        <InlineData(4.0, 8.0, 2.0, 1.0, 3.0, 1.0)> ' VSWR=2:1
        Sub GetRadiusV_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testV As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
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

        ''<InlineData(GridX, GridY, Radius,   Z0,     R,      X,      GridX,  GridY)>
        ''<InlineData(  4.0,   5.0,    2.0,  1.0,     R,      X,      GridX,  GridY)>
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1 / 3.0, 3.1765, 5.7059)>
        '<InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 25.0, 3.1765, 5.7059)> ' NormZ 1/3 + j1/3
        '<InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 4.4, 5.8)> ' On R=Z0 circle
        '<InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 1 / 2.0, 4.703, 5.2162)>
        '<InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 25.0, 4.703, 5.2162)> ' NormZ 2 + j1/2
        '<InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 0.0, 3.0, 5.0)>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0, 4.0, 5.0)> ' Center point
        '<InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.0, 5.0, 5.0)>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 3.0, 3.4588, 4.4353)>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 2.0, -2.0, 5.077, 4.385)>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 2.0, 3.6, 4.2)> ' On G=Y0 circle






        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0, 4.0, 5.0)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 4.4, 5.8)> ' R=Z0 circle above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, -2.0, 5.0, 4.0)> ' R=Z0 circle below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 1 / 2.0, 4.703, 5.2162)> ' Inside R=Z0 circle below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.0, 5.0, 5.0)> ' Inside R=Z0 circle on line
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 25.0, 4.703, 5.2162)> ' Inside R=Z0 circle below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 1 / 2.0, 3.6, 5.8)> ' G=Y0 circle above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 2.0, 3.6, 4.2)> ' G=Y0 circle below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 0.0, 3.0, 5.0)> ' Inside G=Y0 circle
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 1.4, 4.5882, 6.6471)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, -0.8, 3.8462, 3.7692)> ' Bottom remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1 / 3.0, 3.1765, 5.7059)> ' D:
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 25.0, 3.1765, 5.7059)> ' E: NormZ 1/3 + j1/3
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 3.0, 3.4588, 4.4353)> ' L:
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, -2.0, 5.077, 4.385)> ' M:
        Sub TryGetXYPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

            ' This loose precision may be needed due to the use of PointF in GetPlotXY.
            Const Precision As Double = 0.001

            Dim GridX, GridY As Double
            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)

            If Not SmithCirc.GetPlotXY(testR, testX, GridX, GridY) Then
                Assert.True(False)
            End If

            Assert.Equal(expectPlotX, GridX, Precision)
            Assert.Equal(expectPlotY, GridY, Precision)

        End Sub



        '<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, X, GridX, GridY)> ' NormR<=0
        '<InlineData(4.0, 5.0, 2.0, 1.0, R, X, 2.5, 6.5)> ' Outside of circle
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0, 0.0, 2.0, 5.0)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0, 6.0, 5.0)> ' B: Open circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0, 1 / 2.0, 2.8, 6.6)> ' C: Perimeter


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
        <InlineData(4.0, 5.0, 4.0, 1, 3.0, 5.0, 1 / 3.0, 0.0)> ' NormR on resonance line left.
        <InlineData(4.0, 5.0, 4.0, 1, 5.0, 5.0, 3.0, 0.0)> ' NormR on resonance line right.
        <InlineData(4.0, 5.0, 4.0, 1, 4.4, 5.8, 1.0, 1.0)> ' NormR above resonance line right.
        <InlineData(4.0, 5.0, 4.0, 1, 4.703, 5.218, 2.0, 1 / 2.0)> ' NormR above resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 4.0, 5.0, 1.0, 0.0)> ' NormR at center point.
        <InlineData(4.0, 5.0, 4.0, 1, 5.077, 4.385, 2.0, -2.0)> ' NormR below resonance line.
        <InlineData(4.0, 5.0, 4.0, 1, 3.175, 5.7, 1 / 3.0, 1 / 3.0)>
        <InlineData(4.0, 5.0, 4.0, 1, 3.4541, 4.4368, 1 / 2.0, -1 / 3.0)>
        Public Sub GetZFromPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridDiameter As Double, z0 As Double,
            plotX As Double, plotY As Double,
            expectR As Double, expectX As Double)

            Const Precision As Double = 0.01

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
            Assert.Equal(expectR, ZAns.Resistance, Precision)
            Assert.Equal(expectX, ZAns.Reactance, Precision)

        End Sub

        <Fact>
        Public Sub GetZFromPlot_BadInput_Fails()
            ' Try GetZFromPlot with point outside circle.
            Dim SmithCirc As New SmithMainCircle(4, 5, 4, 1)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim ZAns As Impedance = SmithCirc.GetZFromPlot(2.5, 6.5)
                End Sub)
        End Sub

    End Class ' TestGetZFromPlot

    Public Class TestGetYFromPlot

        '<InlineData(GridX, GridY, Radius,   Z0,   G,       B,  GridX,  GridY)> ' Model
        '<InlineData(  4.0,   5.0,    2.0,  1.0,   G,       B,  GridX,  GridY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.5, -1.5, 3.1765, 5.7059)>
        <InlineData(4.0, 5.0, 2.0, 75.0, 0.0133333333, -0.0133333333, 3.1765, 5.7059)> ' NormZ 1/3 + j1/3
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.5, -0.5, 4.4, 5.8)> ' On R=Z0 circle
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.48, -0.15, 4.703, 5.2162)>
        <InlineData(4.0, 5.0, 2.0, 50.0, 0.016, -0.008, 4.703, 5.2162)> ' NormZ 2 + j1/2
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0, 4.0, 5.0)> ' Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.35, 0.0, 5.0, 5.0)>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.4, 0.9, 3.4588, 4.4353)>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.25, 0.25, 5.077, 4.385)>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 3.6, 4.2)> ' On G=Y0 circle
        Public Sub GetYFromPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectG As Double, expectB As Double,
            plotX As Double, plotY As Double)

            Const Precision As Double = 0.1

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim YAns As Admittance = SmithCirc.GetYFromPlot(plotX, plotY)
            Assert.Equal(expectG, YAns.Conductance, Precision)
            Assert.Equal(expectB, YAns.Susceptance, Precision)

        End Sub

        <Fact>
        Public Sub GetYFromPlot_BadInput_Fails()
            ' Try GetYFromPlot with point outside circle.
            Dim SmithCirc As New SmithMainCircle(4, 5, 4, 1)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
            Sub()
                ' Code that throws the exception
                Dim ZAns As Admittance = SmithCirc.GetYFromPlot(2.5, 6.5)
            End Sub)
        End Sub

    End Class ' TestGetYFromPlot

End Namespace ' GeometryTests
