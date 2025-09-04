Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit

Namespace GeometryTests

    Public Class TestGetRadiusR

        Const INF As Double = Double.PositiveInfinity

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,      G,       B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Model
        ' <Theory>
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      999,     999,    999,     999,    2.5,    6.5, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Outside of circle
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,  -2.0000,     999,    999,     999,  GridX,  GridY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' NormR<=0
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,  0.0000, 0.0000,  0.0000, 2.0000, 5.0000,  2.0000,     INF,  0.0000,     INF,     INF)> ' A: Short circuit
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,   1/2.0, 0.0000, -2.0000,    2.8,    6.6,  2.0000,  4.0000,     INF,   1.000,     INF)> ' C: Perimeter
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      INF,  0.0000, 0.0000,  0.0000,    6.0, 5.0000,  0.0000,     INF,  2.0000,     INF,     INF)> ' B: Open circuit
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  0.0000, 1.0000,  0.0000, 4.0000, 5.0000,  1.0000,     INF,  1.0000,     INF,  1.0000)> ' J: Center point
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  1.0000, 0.5000, -0.5000,    4.4,    5.8,  1.0000,  2.0000,   4.0/3,  4.0000,  2.6180)> ' On R=Z0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000, -2.0000, 0.2000,  0.4000, 5.0000, 4.0000,  1.0000,  1.0000,   5.0/3,  5.0000,  5.8284)> ' On R=Z0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000,   1/2.0, 0.4706, -0.1176, 4.7027, 5.2162,   2/3.0,  4.0000,  1.3600, 17.0000,  2.1626)> ' Q1: Inside R=Z0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000, 50.0000, 100.0000, 25.0000, 0.0094, -0.0024, 4.7027, 5.2162,   2/3.0,  4.0000,  1.3605, 16.6667,  2.1626)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   3.0000,  0.0000,  1.0/3,  0.0000, 5.0000, 5.0000,     0.5,     INF,  1.5000,     999,  3.0000)> ' Inside R=Z0 circle, on line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000, -2.0000, 0.2500,  0.2500,  5.077,  4.385,   2.0/3,  1.0000,   1.600,  8.0000,  4.2656)> ' M: Inside R=Z0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,   1/2.0, 1.0000,  -1.000,    3.6,    5.8,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/2.0, 1.0000,  1.0000,    3.6,    4.2,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,  0.0000, 3.0000,  0.0000, 3.0000, 5.0000,     1.5,     INF,  0.5000,     999,  3.0000)> ' Inside G=Y0 circle, on line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,   1/3.0, 1.5000, -1.5000,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D1: Inside G=Y0, above line
        '<InlineData(4.0000, 5.0000,   2.0000, 75.0000,  25.0000, 25.0000, 0.0200, -0.0200,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D2: NormZ 1/3 + j1/3, Z0=75
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/3.0, 1.3846,  0.9231, 3.4588, 4.4353,   4/3.0,  6.0000,  0.8387,  2.2500,  2.2845)> ' L: Inside G=Y0, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.2000,  1.4000, 0.1000, -0.7000  4.5882, 6.6471,   5/3.0,  1.4286,  1.8182,  2.8571, 14.9330)> ' Top remainder
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.4000, -0.8000, 0.5000,  1.0000,  3.845,   3.75,  1.4286,     2.5,   4.0/3,  2.0000,  4.2656)> ' Bottom remainder

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R, RadiusR)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000)> ' B: Open circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' On R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' On R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 2 / 3.0)> ' Q1: Inside R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 2 / 3.0)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.5)> ' Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 2.0 / 3)> ' M: Inside R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4 / 3.0)> ' G=Y0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4 / 3.0)> ' G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1.5)> ' Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1.5)> ' D1: Inside G=Y0, above line
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 1.5)> ' D2: NormZ 1/3 + j1/3, Z0=75
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4 / 3.0)> ' L: Inside G=Y0, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 5 / 3.0)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, 1.4286)> ' Bottom remainder
        Sub GetRadiusR_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testR As Double, expectRadR As Double)

            Const Precision As Double = 0.0005

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
            Assert.Equal(expectRadR, RadiusAns, Precision)
        End Sub

        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' C: Perimeter
        'Sub GetRadiusR_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testR As Double)

        '    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '    Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '        Sub()
        '            ' Code that throws the exception
        '            Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
        '        End Sub)
        'End Sub

        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999, RadiusR)> ' Outside of circle
        ''<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, RadiusR)> ' NormR<=0
        ''
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' C: Perimeter
        'Sub GetRadiusR_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testR As Double)
        '    Try
        '        Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '            Sub()
        '                ' Code that throws the exception
        '                Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '                Dim RadiusAns As Double = SmithCirc.GetRadiusR(testR)
        '            End Sub)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetRadiusR

    Public Class TestGetRadiusX

        Const INF As Double = Double.PositiveInfinity


        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,      G,       B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Model
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,        R,       X,      G,       B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Base circle
        ' <Theory>
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      999,     999,    999,     999,    2.5,    6.5, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Outside of circle
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,  -2.0000,     999,    999,     999,  GridX,  GridY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' NormR<=0
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,  0.0000, 0.0000,  0.0000, 2.0000, 5.0000,  2.0000,     INF,  0.0000,     INF,     INF)> ' A: Short circuit
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,   1/2.0, 0.0000, -2.0000,    2.8,    6.6,  2.0000,  4.0000,     INF,   1.000,     INF)> ' C: Perimeter
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      INF,  0.0000, 0.0000,  0.0000,    6.0, 5.0000,  0.0000,     INF,  2.0000,     INF,     INF)> ' B: Open circuit
        '
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  0.0000, 1.0000,  0.0000, 4.0000, 5.0000,  1.0000,     INF,  1.0000,     INF,  1.0000)> ' J: Center point
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  1.0000, 0.5000, -0.5000,    4.4,    5.8,  1.0000,  2.0000,   4.0/3,  4.0000,  2.6180)> ' On R=Z0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000, -2.0000, 0.2000,  0.4000, 5.0000, 4.0000,  1.0000,  1.0000,   5.0/3,  5.0000,  5.8284)> ' On R=Z0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000,   1/2.0, 0.4706, -0.1176, 4.7027, 5.2162,   2/3.0,  4.0000,  1.3600, 17.0000,  2.1626)> ' Q1: Inside R=Z0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000, 50.0000, 100.0000, 25.0000, 0.0094, -0.0024, 4.7027, 5.2162,   2/3.0,  4.0000,  1.3605, 16.6667,  2.1626)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   3.0000,  0.0000,  1.0/3,  0.0000, 5.0000, 5.0000,     0.5,     INF,  1.5000,     999,  3.0000)> ' Inside R=Z0 circle, on line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000, -2.0000, 0.2500,  0.2500,  5.077,  4.385,   2.0/3,  1.0000,   1.600,  8.0000,  4.2656)> ' M: Inside R=Z0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,   1/2.0, 1.0000,  -1.000,    3.6,    5.8,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/2.0, 1.0000,  1.0000,    3.6,    4.2,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,  0.0000, 3.0000,  0.0000, 3.0000, 5.0000,     1.5,     INF,  0.5000,     999,  3.0000)> ' Inside G=Y0 circle, on line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,   1/3.0, 1.5000, -1.5000,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D1: Inside G=Y0, above line
        '<InlineData(4.0000, 5.0000,   2.0000, 75.0000,  25.0000, 25.0000, 0.0200, -0.0200,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D2: NormZ 1/3 + j1/3, Z0=75
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/3.0, 1.3846,  0.9231, 3.4588, 4.4353,   4/3.0,  6.0000,  0.8387,  2.2500,  2.2845)> ' L: Inside G=Y0, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.2000,  1.4000, 0.1000, -0.7000  4.5882, 6.6471,   5/3.0,  1.4286,  1.8182,  2.8571, 14.9330)> ' Top remainder
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.4000, -0.8000, 0.5000,  1.0000,  3.845,   3.75,  1.4286,     2.5,   4.0/3,  2.0000,  4.2656)> ' Bottom remainder


        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       X, RadiusX)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0)> ' C: Perimeter
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 2.0)> ' On R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 1.0)> ' On R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0)> ' Q1: Inside R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 50.0, 25.0, 4.0)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 1.0)> ' M: Inside R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 4.0)> ' G=Y0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, -1 / 2.0, 4.0)> ' G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 6.0)> ' D1: Inside G=Y0, above line
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 6.0)> ' D2: NormZ 1/3 + j1/3, Z0=75
        <InlineData(4.0, 5.0, 2.0, 1.0, -1 / 3.0, 6.0)> ' L: Inside G=Y0, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.4, 1.4286)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.8, 2.5)> ' Bottom remainder
        Sub GetRadiusX_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testX As Double, expectRad As Double)

            Const Precision As Double = 0.0005

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub


        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999)> ' Outside of circle
        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999)> ' NormR<=0
        ''
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' B: Open circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' J: Center point
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' Inside R=Z0 circle, on line
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' Inside G=Y0 circle, on line
        'Sub GetRadiusX_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testX As Double)

        '    Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '        Sub()
        '            ' Code that throws the exception
        '            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '            Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
        '        End Sub)
        'End Sub

        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, RadiusR)> ' Outside of circle
        '<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, RadiusR)> ' NormR<=0
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' B: Open circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' J: Center point
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' C: Perimeter
        'Sub GetRadiusX_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testX As Double)
        '    Try
        '        Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '            Sub()
        '                ' Code that throws the exception
        '                Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '                Dim RadiusAns As Double = SmithCirc.GetRadiusX(testX)
        '            End Sub)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetRadiusX

    Public Class TestGetRadiusG

        Const INF As Double = Double.PositiveInfinity

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 5, 2.0 / 6)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 2, 2.0 / 3)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0 / 3)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.2, 10.0 / 6)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 3, 1.5)>
        Sub GetRadiusG_EasyValues_Succeed(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testG As Double, expectRad As Double)

            Const Precision As Double = 0.0005

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,      G, RadiusG)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.5, 4.0 / 3)> ' On R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 5.0 / 3)> ' On R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4706, 1.36)> ' Q1: Inside R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 50.0, 0.0094, 1.3605)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0 / 3, 1.5)> ' Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.25, 1.6)> ' M: Inside R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' G=Y0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0)> ' G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.5)> ' Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.5, 0.8)> ' D1: Inside G=Y0, above line
        <InlineData(4.0, 5.0, 2.0, 75.0, 0.02, 0.8)> ' D2: NormZ 1/3 + j1/3, Z0=75
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.3846, 0.8387)> ' L: Inside G=Y0, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.1, 1.8182)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.5, 4.0 / 3)> ' Bottom remainder
        Sub GetRadiusG_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testG As Double, expectRad As Double)

            Const Precision As Double = 0.0005

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999, RadiusG)> ' Outside of circle
        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999, RadiusG)> ' NormR<=0
        ''
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' C: Perimeter
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' B: Open circuit
        'Sub GetRadiusG_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testG As Double)

        '    Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '        Sub()
        '            ' Code that throws the exception
        '            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '            Dim RadiusAns As Double = SmithCirc.GetRadiusX(testG)
        '        End Sub)
        'End Sub

        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999, RadiusG)> ' Outside of circle
        ''<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, RadiusG)> ' NormR<=0
        ''
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' B: Open circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' J: Center point
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' C: Perimeter
        'Sub GetRadiusG_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testG As Double)
        '    Try
        '        Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '            Sub()
        '                ' Code that throws the exception
        '                Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '                Dim RadiusAns As Double = SmithCirc.GetRadiusG(testG)
        '            End Sub)
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestGetRadiusG

    Public Class TestGetRadiusB

        Const INF As Double = Double.PositiveInfinity


        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,      G,       B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Model
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,        R,       X,      G,       B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Base circle
        ' <Theory>
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      999,     999,    999,     999,    2.5,    6.5, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Outside of circle
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,  -2.0000,     999,    999,     999,  GridX,  GridY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' NormR<=0
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,  0.0000, 0.0000,  0.0000, 2.0000, 5.0000,  2.0000,     INF,  0.0000,     INF,     INF)> ' A: Short circuit
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,   1/2.0, 0.0000, -2.0000,    2.8,    6.6,  2.0000,  4.0000,     INF,   1.000,     INF)> ' C: Perimeter
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      INF,  0.0000, 0.0000,  0.0000,    6.0, 5.0000,  0.0000,     INF,  2.0000,     INF,     INF)> ' B: Open circuit
        '
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  0.0000, 1.0000,  0.0000, 4.0000, 5.0000,  1.0000,     INF,  1.0000,     INF,  1.0000)> ' J: Center point
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  1.0000, 0.5000, -0.5000,    4.4,    5.8,  1.0000,  2.0000,   4.0/3,  4.0000,  2.6180)> ' On R=Z0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000, -2.0000, 0.2000,  0.4000, 5.0000, 4.0000,  1.0000,  1.0000,   5.0/3,  5.0000,  5.8284)> ' On R=Z0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000,   1/2.0, 0.4706, -0.1176, 4.7027, 5.2162,   2/3.0,  4.0000,  1.3600, 17.0000,  2.1626)> ' Q1: Inside R=Z0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000, 50.0000, 100.0000, 25.0000, 0.0094, -0.0024, 4.7027, 5.2162,   2/3.0,  4.0000,  1.3605, 16.6667,  2.1626)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   3.0000,  0.0000,  1.0/3,  0.0000, 5.0000, 5.0000,     0.5,     INF,  1.5000,     999,  3.0000)> ' Inside R=Z0 circle, on line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000, -2.0000, 0.2500,  0.2500,  5.077,  4.385,   2.0/3,  1.0000,   1.600,  8.0000,  4.2656)> ' M: Inside R=Z0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,   1/2.0, 1.0000,  -1.000,    3.6,    5.8,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, above line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/2.0, 1.0000,  1.0000,    3.6,    4.2,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,  0.0000, 3.0000,  0.0000, 3.0000, 5.0000,     1.5,     INF,  0.5000,     999,  3.0000)> ' Inside G=Y0 circle, on line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,   1/3.0, 1.5000, -1.5000,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D1: Inside G=Y0, above line
        '<InlineData(4.0000, 5.0000,   2.0000, 75.0000,  25.0000, 25.0000, 0.0200, -0.0200,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D2: NormZ 1/3 + j1/3, Z0=75
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/3.0, 1.3846,  0.9231, 3.4588, 4.4353,   4/3.0,  6.0000,  0.8387,  2.2500,  2.2845)> ' L: Inside G=Y0, below line
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.2000,  1.4000, 0.1000, -0.7000  4.5882, 6.6471,   5/3.0,  1.4286,  1.8182,  2.8571, 14.9330)> ' Top remainder
        '<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.4000, -0.8000, 0.5000,  1.0000,  3.845,   3.75,  1.4286,     2.5,   4.0/3,  2.0000,  4.2656)> ' Bottom remainder


        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,       B, RadiusB)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 1.0)> ' C: Perimeter
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.5, 4.0)> ' On R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, 5.0)> ' On R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.1176, 17.0)> ' Q1: Inside R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 50.0, -0.0024, 16.6667)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.25, 8.0)> ' M: Inside R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, -1.0, 2.0)> ' G=Y0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 2.0)> ' G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, -1.5, 1.3333)> ' D1: Inside G=Y0, above line
        <InlineData(4.0, 5.0, 2.0, 75.0, -0.02, 1.3333)> ' D2: NormZ 1/3 + j1/3, Z0=75
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.9231, 2.25)> ' L: Inside G=Y0, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, -0.7, 2.8571)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 2.0)> ' Bottom remainder
        Sub GetRadiusB_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testB As Double, expectRad As Double)

            '            Const Precision As Double = 0.0005
            Const Precision As Double = 0.1

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        '<Theory>
        '<InlineData(4.0, 8.0, 4.0, 1.0, 0.0)> ' NormB=0
        'Sub GetRadiusB_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testB As Double)

        '    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '    Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '        Sub()
        '            ' Code that throws the exception
        '            Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
        '        End Sub)
        'End Sub

        '<InlineData(4.0, 5.0, 2.0, 1.0, 999)> ' Outside of circle
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999)> ' NormR<=0
        '<InlineData(4.0, 8.0, 4.0, 1.0, 0.0)> ' NormB=0
        '
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' A: Short circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' B: Open circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000)> ' Inside G=Y0 circle, on line
        Sub GetRadiusB_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testB As Double)

            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
                    Dim RadiusAns As Double = SmithCirc.GetRadiusB(testB)
                End Sub)
        End Sub

    End Class ' TestGetRadiusB

    Public Class TestGetRadiusV

        Const INF As Double = Double.PositiveInfinity
        Const Precision As Double = 0.0005

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,    VSWR, RadiusV)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0000)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.618, 0.8944)> ' On R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 5.8284, 1.4142)> ' On R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.1626, 0.7352)> ' Q1: Inside R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 50.0, 2.1626, 0.7352)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 1.0)> ' Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 4.2656, 1.2404)> ' M: Inside R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.618, 0.8944)> ' G=Y0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.618, 0.8944)> ' G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 1.0)> ' Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.3699, 1.0846)> ' D1: Inside G=Y0, above line
        <InlineData(4.0, 5.0, 2.0, 75.0, 3.3699, 1.0846)> ' D2: NormZ 1/3 + j1/3, Z0=75
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.2845, 0.7822)> ' L: Inside G=Y0, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 14.933, 1.7489)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 4.2656, 1.2404)> ' Bottom remainder
        Sub GetRadiusV_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridRadius As Double,
                                          z0 As Double, testV As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusV(testV)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

        ''<InlineData(4.0, 5.0, 2.0, 1.0, VSWR)> ' Outside of circle
        ''<InlineData(4.0, 5.0, 2.0, 1.0, VSWR)> ' NormR<=0
        ''
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, INF)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, INF)> ' C: Perimeter
        '<InlineData(4.0, 5.0, 2.0, 1.0, INF)> ' B: Open circuit
        'Sub GetRadiusV_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
        '                              z0 As Double, testV As Double)

        '    Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '        Sub()
        '            ' Code that throws the exception
        '            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
        '            Dim RadiusAns As Double = SmithCirc.GetRadiusV(testV)
        '        End Sub)
        'End Sub

        '<InlineData(4.0, 5.0, 2.0, 1.0, VSWR)> ' Outside of circle
        '<InlineData(4.0, 5.0, 2.0, 1.0, VSWR)> ' NormR<=0
        '
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, INF)> ' A: Short circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, INF)> ' C: Perimeter
        <InlineData(4.0, 5.0, 2.0, 1.0, INF)> ' B: Open circuit
        Sub GetRadiusV_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testV As Double)
            Try
                Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                    Sub()
                        ' Code that throws the exception
                        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
                        Dim RadiusAns As Double = SmithCirc.GetRadiusV(testV)
                    End Sub)
            Catch ex As Exception
                Assert.True(True)
                Exit Sub
            End Try
            Assert.True(False, "Did not fail")
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

        Const INF As Double = Double.PositiveInfinity

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0000, 4.0, 5.0)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 4.4, 5.8)> ' On R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, -2.0, 5.0, 4.0)> ' On R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 1 / 2.0, 4.7027, 5.2162)> ' Q1: Inside R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 25.0, 4.7027, 5.2162)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.0000, 5.0, 5.0)> ' Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, -2.0, 5.077, 4.385)> ' M: Inside R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 1 / 2.0, 3.6, 5.8)> ' G=Y0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 2.0, 3.6, 4.2)> ' G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 0.0000, 3.0, 5.0)> ' Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1 / 3.0, 3.1765, 5.7059)> ' D1: Inside G=Y0, above line
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 25.0, 3.1765, 5.7059)> ' D2: NormZ 1/3 + j1/3, Z0=75
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 3.0, 3.4588, 4.4353)> ' L: Inside G=Y0, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 1.4, 4.5882, 6.6471)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, -0.8, 3.8462, 3.7692)> ' Bottom remainder
        Sub TryGetPlotXY_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

            Const Precision As Double = 0.0005

            Dim GridX, GridY As Double
            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)

            If Not SmithCirc.GetPlotXY(testR, testX, GridX, GridY) Then
                Assert.True(False)
            End If

            Assert.Equal(expectPlotX, GridX, Precision)
            Assert.Equal(expectPlotY, GridY, Precision)

        End Sub

        ''<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, GridX, GridY)> ' NormR<=0
        ''<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 2.5, 6.5)> ' Outside of circle
        ''<InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000, 6.0, 5.0)> ' B: Open circuit
        ''
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 1 / 2.0, 2.8, 6.6)> ' C: Perimeter
        'Public Sub TryGetPlotXY_BadInput_Fails(
        '    gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
        '    testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

        '    Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '    Sub()
        '        ' Code that throws the exception
        '        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
        '        Dim DidIt As Boolean = SmithCirc.GetPlotXY(testR, testX, gridCenterX, gridCenterY)
        '    End Sub)
        'End Sub

        '<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, GridX, GridY)> ' NormR<=0
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: Short circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 1 / 2.0, 2.8, 6.6)> ' C: Perimeter
        '
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000, 6.0, 5.0)> ' B: Open circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 2.5, 6.5)> ' Outside of circle
        Public Sub TryGetPlotXY_BadInput_Fails(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            testR As Double, testX As Double, expectPlotX As Double, expectPlotY As Double)

            Try
                Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                    Sub()
                        ' Code that throws the exception
                        Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
                        '                        Dim RadiusAns As Double = SmithCirc.GetPlotXY(testR, testX, gridCenterX, gridCenterY)
                        Dim DidIt As Boolean = SmithCirc.GetPlotXY(testR, testX, gridCenterX, gridCenterY)
                    End Sub)
            Catch ex As Exception
                Assert.True(True)
                Exit Sub
            End Try
            Assert.True(False, "Did not fail")
        End Sub

    End Class ' TestGetPlotXY

    Public Class TestGetZFromPlot

        Const INF As Double = Double.PositiveInfinity

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY
        ' NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,  PlotX,  PlotY)> ' Model
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 1 / 2.0, 2.8, 6.6)> ' C: Perimeter
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 0.0000, 4.0, 5.0)> ' J: Center point
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, 1.0, 4.4, 5.8)> ' On R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1.0, -2.0, 5.0, 4.0)> ' On R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, 1 / 2.0, 4.7027, 5.2162)> ' Q1: Inside R=Z0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 50.0, 100.0, 25.0, 4.7027, 5.2162)> ' Q2: Inside R=Z0 circle, above line, Z0=50
        <InlineData(4.0, 5.0, 2.0, 1.0, 3.0, 0.0000, 5.0, 5.0)> ' Inside R=Z0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 2.0, -2.0, 5.077, 4.385)> ' M: Inside R=Z0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, 1 / 2.0, 3.6, 5.8)> ' G=Y0 circle, above line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 2.0, 3.6, 4.2)> ' G=Y0 circle, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 0.0000, 3.0, 5.0)> ' Inside G=Y0 circle, on line
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 3.0, 1 / 3.0, 3.1765, 5.7059)> ' D1: Inside G=Y0, above line
        <InlineData(4.0, 5.0, 2.0, 75.0, 25.0, 25.0, 3.1765, 5.7059)> ' D2: NormZ 1/3 + j1/3, Z0=75
        <InlineData(4.0, 5.0, 2.0, 1.0, 1 / 2.0, -1 / 3.0, 3.4588, 4.4353)> ' L: Inside G=Y0, below line
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.2, 1.4, 4.5882, 6.6471)> ' Top remainder
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.4, -0.8, 3.8462, 3.7692)> ' Bottom remainder
        Public Sub GetZFromPlot_GoodInput_Succeeds(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectR As Double, expectX As Double,
            plotX As Double, plotY As Double)

            '            Const Precision As Double = 0.01
            '   THE LOOSE PRECISION HERE SEEMS TO BE A RESULT OF THE USE OF FLOATING POINT VALUES.
            Const Precision As Double = 0.05

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
            Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
            Assert.Equal(expectR, ZAns.Resistance, Precision)
            Assert.Equal(expectX, ZAns.Reactance, Precision)

        End Sub

        ''<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, GridX, GridY)> ' NormR<=0
        ''
        '<Theory>
        '<InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 2.5, 6.5)> ' Outside of circle
        '<InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000, 6.0, 5.0)> ' B: Open circuit
        '<InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: Short circuit
        'Public Sub GetZFromPlot_BadInput_Fails(
        '    gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
        '    expectR As Double, expectX As Double,
        '    plotX As Double, plotY As Double)

        '    Const Precision As Double = 0.0005

        '    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2.0, z0)
        '    Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
        '    Assert.Equal(expectR, ZAns.Resistance, Precision)
        '    Assert.Equal(expectX, ZAns.Reactance, Precision)

        'End Sub

        '<InlineData(4.0, 5.0, 2.0, 1.0, -2.0, 999, GridX, GridY)> ' NormR<=0
        '
        <Theory>
        <InlineData(4.0, 5.0, 2.0, 1.0, 999, 999, 2.5, 6.5)> ' Outside of circle
        <InlineData(4.0, 5.0, 2.0, 1.0, INF, 0.0000, 6.0, 5.0)> ' B: Open circuit
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.0000, 0.0000, 2.0, 5.0)> ' A: Short circuit
        Public Sub GetZFromPlot_BadInput_Fails(
            gridCenterX As Double, gridCenterY As Double, gridRadius As Double, z0 As Double,
            expectR As Double, expectX As Double,
            plotX As Double, plotY As Double)
            ' Try GetZFromPlot with point outside circle.
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridRadius * 2, z0)
                    Dim ZAns As Impedance = SmithCirc.GetZFromPlot(plotX, plotY)
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
        <InlineData(4.0, 5.0, 2.0, 1.0, 0.48, -0.15, 4.7027, 5.2162)>
        <InlineData(4.0, 5.0, 2.0, 50.0, 0.016, -0.008, 4.7027, 5.2162)> ' NormZ 2 + j1/2
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
