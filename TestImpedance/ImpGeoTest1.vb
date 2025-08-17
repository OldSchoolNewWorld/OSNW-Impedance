Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit
Imports OsnwImpd = OSNW.Numerics.Impedance

Namespace GeometryTests

    Public Class TestGetRadiusR

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 50.0, 100.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 75.0, 25.0, 1.5)>
        Sub GetRadiusR_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testRes As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusR(testRes)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -2.0, 2.0 / 3.0)>
        Sub GetRadiusR_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testRes As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim RadiusAns As Double = SmithCirc.GetRadiusR(testRes)
                End Sub)
        End Sub

    End Class ' TestGetRadiusR

    Public Class TestGetRadiusX

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 2.0 / 3.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 50.0, 100.0, 1.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 2.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0 / 3.0, 6.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0)> ' Est
        <InlineData(4.0, 8.0, 4.0, 75.0, 25.0, 6.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -0.5, 4.0)> ' Est
        <InlineData(4.0, 8.0, 4.0, 75.0, -25.0, 6.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -1.0, 2.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 1.0, -1.0, 2.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 50.0, -100.0, 1.0)> ' Meas
        <InlineData(4.0, 8.0, 4.0, 1.0, -3.0, 2.0 / 3.0)> ' Meas
        Sub GetRadiusX_GoodInput_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                          z0 As Double, testReact As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusX(testReact)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.0, 0.5)>
        Sub GetRadiusX_BadInput_Fails(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                      z0 As Double, testRes As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim RadiusAns As Double = SmithCirc.GetRadiusX(testRes)
                End Sub)
        End Sub

    End Class ' TestGetRadiusX

    Public Class TestGetRadiusG

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 50.0, 100.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 0.5, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 75.0, 25.0, 1.5)>
        Sub GetRadiusG_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                z0 As Double, testCond As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusG(testCond)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

    End Class ' TestGetRadiusG

    Public Class TestGetRadiusB

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -1 / 2.0, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 3.0, 1.5)>
        Sub GetRadiusB_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                                z0 As Double, testCond As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.GetRadiusB(testCond)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

    End Class ' TestGetRadiusB

    Public Class TestRadiusV

        Const Precision As Double = 0.000001

        '        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 3.0, 1.5)>
        '        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 2.0, 4.0 / 3.0)>
        '        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 1.0)>
        Sub RadiusV_Succeeds(gridCenterX As Double, gridCenterY As Double, gridDiameter As Double,
                             z0 As Double, testV As Double, expectRad As Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As Double = SmithCirc.RadiusV(testV)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

    End Class ' TestRadiusV

End Namespace ' GeometryTests
