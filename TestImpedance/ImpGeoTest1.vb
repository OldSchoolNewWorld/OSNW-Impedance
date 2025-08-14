Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit
Imports OsnwImpd = OSNW.Numerics.Impedance

Namespace GeometryTests

    Public Class TestRadiusR

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0 / 2.0, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0 / 3.0, 1.5)>
        Sub RadiusR_Succeeds(ByVal gridCenterX As System.Double,
            ByVal gridCenterY As System.Double,
            ByVal gridDiameter As System.Double, ByVal z0 As System.Double,
            testRes As System.Double, expectRad As System.Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As System.Double = SmithCirc.RadiusR(testRes)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

    End Class ' TestRadiusR

    Public Class TestRadiusX

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0 / 2.0, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -1.0 / 3.0, 1.5)>
        Sub RadiusX_Succeeds(ByVal gridCenterX As System.Double,
            ByVal gridCenterY As System.Double,
            ByVal gridDiameter As System.Double, ByVal z0 As System.Double,
            testRes As System.Double, expectRad As System.Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As System.Double = SmithCirc.RadiusX(testRes)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

    End Class ' TestRadiusX

    Public Class TestRadiusG

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 2.0, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 3.0, 1.5)>
        Sub RadiusG_Succeeds(ByVal gridCenterX As System.Double,
            ByVal gridCenterY As System.Double,
            ByVal gridDiameter As System.Double, ByVal z0 As System.Double,
            testCond As System.Double, expectRad As System.Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As System.Double = SmithCirc.RadiusG(testCond)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

    End Class ' TestRadiusG

    Public Class TestRadiusB

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, -1 / 2.0, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 3.0, 1.5)>
        Sub RadiusB_Succeeds(ByVal gridCenterX As System.Double,
            ByVal gridCenterY As System.Double,
            ByVal gridDiameter As System.Double, ByVal z0 As System.Double,
            testCond As System.Double, expectRad As System.Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As System.Double = SmithCirc.RadiusB(testCond)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

    End Class ' TestRadiusB

    Public Class TestRadiusV

        Const Precision As Double = 0.000001

        '        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 3.0, 1.5)>
        '        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 2.0, 4.0 / 3.0)>
        '        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 1.0)>
        Sub RadiusV_Succeeds(ByVal gridCenterX As System.Double,
            ByVal gridCenterY As System.Double,
            ByVal gridDiameter As System.Double, ByVal z0 As System.Double,
            testV As System.Double, expectRad As System.Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As System.Double = SmithCirc.RadiusV(testV)
            Assert.Equal(expectRad, RadiusAns, Precision)
        End Sub

    End Class ' TestRadiusV

End Namespace ' GeometryTests
