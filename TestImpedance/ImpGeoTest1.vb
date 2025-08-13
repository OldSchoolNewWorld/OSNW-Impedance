Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit
Imports OsnwImpd = OSNW.Numerics.Impedance

Namespace GeometryTests



    Public Class TestRadiusRX

        <Theory>
        <InlineData(4.0, 8.0, 4.0, 1.0, 3.0, 0.5)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 2.0, 2.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1.0, 1.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 2.0, 4.0 / 3.0)>
        <InlineData(4.0, 8.0, 4.0, 1.0, 1 / 3.0, 1.5)>
        Sub RadiusRX_Succeeds(ByVal gridCenterX As System.Double,
            ByVal gridCenterY As System.Double,
            ByVal gridDiameter As System.Double, ByVal z0 As System.Double,
            testRes As System.Double, expectRad As System.Double)

            Dim SmithCirc As New SmithMainCircle(gridCenterX, gridCenterY, gridDiameter, z0)
            Dim RadiusAns As System.Double = SmithCirc.RadiusRX(testRes)
            Assert.Equal(expectRad, RadiusAns)
        End Sub

    End Class ' TestRadiusRX

End Namespace ' GeometryTests
