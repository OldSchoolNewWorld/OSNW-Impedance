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

End Namespace ' GeometryTests
