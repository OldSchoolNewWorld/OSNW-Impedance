Imports System.Data
Imports OSNW.Math
Imports Xunit
Imports OsnwPoint2D = OSNW.Math.D2.Point

Namespace GeometricTests

    Public Class PointTests

        Const Tolerance As Double = 0.001
        Const RAD390d As Double = 13.0 * PId / 6.0 ' 6.8067 (30+360)
        Const RAD480d As Double = 8.0 * PId / 3.0 ' 8.3776 (120+360)
        Const RAD600d As Double = 10.0 * PId / 3.0 ' 10.4720 (240+360)
        Const RAD675d As Double = 15.0 * PId / 4.0 ' 11.7810 (315+360)

#Region "TestNewPoint"

        <Fact>
        Public Sub New_Default_Succeeds()
            Dim P As New OsnwPoint2D()
            Assert.Equal(OsnwPoint2D.DFLTX, P.X)
            Assert.Equal(OsnwPoint2D.DFLTY, P.Y)
        End Sub

        <Theory>
        <InlineData(3.0, 4.0)>
        <InlineData(-3.0, 4.0)>
        <InlineData(3.0, -4.0)>
        <InlineData(-3.0, -4.0)>
        Public Sub New_GoodValues_Succeeds(ByVal x As Double, ByVal y As Double)
            Dim P As New OsnwPoint2D(x, y)
            Assert.Equal(x, P.X)
            Assert.Equal(y, P.Y)
        End Sub

        <Theory>
        <InlineData(Double.NaN, 4.0)>
        <InlineData(3.0, Double.NaN)>
        Public Sub New_NaN_AlsoSucceeds(ByVal x As Double, ByVal y As Double)
            Dim P As New OsnwPoint2D(x, y)
            Assert.Equal(x, P.X)
            Assert.Equal(y, P.Y)
        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, 4.0)>
        <InlineData(Double.NegativeInfinity, 4.0)>
        <InlineData(3.0, Double.PositiveInfinity)>
        <InlineData(3.0, Double.NegativeInfinity)>
        Public Sub New_Infinity_AlsoSucceeds(ByVal x As Double, ByVal y As Double)
            Dim P As New OsnwPoint2D(x, y)
            Assert.Equal(x, P.X)
            Assert.Equal(y, P.Y)
        End Sub

#End Region ' "TestNewPoint"

#Region "TestDistance"

        <Theory>
        <InlineData(3.0, 4.0, 5.0, 6.0, 2.8284)>
        <InlineData(5.0, 6.0, 3.0, 4.0, 2.8284)>
        Public Sub Distance_Coordinates_Succeeds(x0 As Double, y0 As Double, x1 As Double, y1 As Double,
                                                 expected As Double)
            Assert.Equal(expected, OsnwPoint2D.Distance(x0, y0, x1, y1), Tolerance)
        End Sub

        <Theory>
        <InlineData(3.0, 4.0, 5.0, 6.0, 2.8284)>
        <InlineData(5.0, 6.0, 3.0, 4.0, 2.8284)>
        Public Sub Distance_Points_Succeeds(x0 As Double, y0 As Double, x1 As Double, y1 As Double,
                                            expected As Double)
            Dim P0 As New OsnwPoint2D(x0, y0)
            Dim P1 As New OsnwPoint2D(x1, y1)
            Assert.Equal(expected, OsnwPoint2D.Distance(P0, P1), Tolerance)
        End Sub

        <Theory>
        <InlineData(3.0, 4.0, 5.0, 6.0, 2.8284)>
        <InlineData(5.0, 6.0, 3.0, 4.0, 2.8284)>
        Public Sub Distance_Other_Succeeds(x0 As Double, y0 As Double, x1 As Double, y1 As Double,
                                           expected As Double)
            Dim P0 As New OsnwPoint2D(x0, y0)
            Dim P1 As New OsnwPoint2D(x1, y1)
            Assert.Equal(expected, P0.Distance(P1), Tolerance)
        End Sub

#End Region ' "TestDistance"

#Region "TestRotatedAround"

        <Theory>
        <InlineData(RAD030d, 5.0, 4.0, 3.0, 4.0, 4.7321, 5.0)> ' CCW 30.
        <InlineData(-RAD030d, 5.0, 4.0, 3.0, 4.0, 4.7321, 3.0)> ' CW 30.
        <InlineData(RAD390d, 5.0, 4.0, 3.0, 4.0, 4.7321, 5.0)> ' CCW 30+360.
        <InlineData(-RAD390d, 5.0, 4.0, 3.0, 4.0, 4.7321, 3.0)> ' CW 30+360.
        <InlineData(RAD120d, 5.0, 4.0, 3.0, 4.0, 2.0, 5.7321)>
        <InlineData(-RAD120d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 5.7321)>
        <InlineData(RAD480d, 5.0, 4.0, 3.0, 4.0, 2.0, 5.7321)>
        <InlineData(-RAD480d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 5.7321)>
        <InlineData(RAD240d, 5.0, 4.0, 3.0, 4.0, 2.0, 2.2679)>
        <InlineData(-RAD240d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 2.2679)>
        <InlineData(RAD600d, 5.0, 4.0, 3.0, 4.0, 2.0, 2.2679)>
        <InlineData(-RAD600d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 2.2679)>
        <InlineData(RAD315d, 5.0, 4.0, 3.0, 4.0, 4.4142, 2.5858)>
        <InlineData(-RAD315d, 5.0, 4.0, 3.0, 4.0, 4.4142, 8.0 - 2.5858)>
        <InlineData(RAD675d, 5.0, 4.0, 3.0, 4.0, 4.4142, 2.5858)>
        <InlineData(-RAD675d, 5.0, 4.0, 3.0, 4.0, 4.4142, 8.0 - 2.5858)>
        Public Sub RotatedAround_Coords_Succeeds(angle As Double, movingX As Double, movingY As Double,
            aroundX As Double, aroundY As Double, expectedX As Double, expectedY As Double)

            Dim Moving As New OsnwPoint2D(movingX, movingY)

            Dim ResultP As OsnwPoint2D = Moving.RotatedAround(angle, aroundX, aroundY)

            Assert.Equal(expectedX, ResultP.X, Tolerance)
            Assert.Equal(expectedY, ResultP.Y, Tolerance)

        End Sub

        <Theory>
        <InlineData(RAD030d, 5.0, 4.0, 3.0, 4.0, 4.7321, 5.0)> ' CCW 30.
        <InlineData(-RAD030d, 5.0, 4.0, 3.0, 4.0, 4.7321, 3.0)> ' CW 30.
        <InlineData(RAD390d, 5.0, 4.0, 3.0, 4.0, 4.7321, 5.0)> ' CCW 30+360.
        <InlineData(-RAD390d, 5.0, 4.0, 3.0, 4.0, 4.7321, 3.0)> ' CW 30+360.
        <InlineData(RAD120d, 5.0, 4.0, 3.0, 4.0, 2.0, 5.7321)>
        <InlineData(-RAD120d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 5.7321)>
        <InlineData(RAD480d, 5.0, 4.0, 3.0, 4.0, 2.0, 5.7321)>
        <InlineData(-RAD480d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 5.7321)>
        <InlineData(RAD240d, 5.0, 4.0, 3.0, 4.0, 2.0, 2.2679)>
        <InlineData(-RAD240d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 2.2679)>
        <InlineData(RAD600d, 5.0, 4.0, 3.0, 4.0, 2.0, 2.2679)>
        <InlineData(-RAD600d, 5.0, 4.0, 3.0, 4.0, 2.0, 8.0 - 2.2679)>
        <InlineData(RAD315d, 5.0, 4.0, 3.0, 4.0, 4.4142, 2.5858)>
        <InlineData(-RAD315d, 5.0, 4.0, 3.0, 4.0, 4.4142, 8.0 - 2.5858)>
        <InlineData(RAD675d, 5.0, 4.0, 3.0, 4.0, 4.4142, 2.5858)>
        <InlineData(-RAD675d, 5.0, 4.0, 3.0, 4.0, 4.4142, 8.0 - 2.5858)>
        Public Sub RotatedAround_Point_Succeeds(angle As Double, movingX As Double, movingY As Double,
            aroundX As Double, aroundY As Double, expectedX As Double, expectedY As Double)

            Dim Moving As New OsnwPoint2D(movingX, movingY)
            Dim Around As New OsnwPoint2D(aroundX, aroundY)

            Dim ResultP As OsnwPoint2D = Moving.RotatedAround(angle, Around)

            Assert.Equal(expectedX, ResultP.X, Tolerance)
            Assert.Equal(expectedY, ResultP.Y, Tolerance)

        End Sub

#End Region ' "TestRotatedAround"

#Region "TestShifted"

#End Region ' "TestShifted"

        <Theory>
        <InlineData(3.0, 4.0, 2.0, 3.0, 5.0, 7.0)>
        <InlineData(3.0, 4.0, -2.0, 3.0, 1.0, 7.0)> ' -X.
        <InlineData(3.0, 4.0, 2.0, -3.0, 5.0, 1.0)> ' -Y.
        Public Sub Shifted_GoodValues_Succeeds(movingX As Double, movingY As Double,
            shiftX As System.Double, shiftY As System.Double, expectedX As Double, expectedY As Double)

            Dim Moving As New OsnwPoint2D(movingX, movingY)

            Dim NewPoint As D2.Point = Moving.Shifted(shiftX, shiftY)

            Assert.Equal(expectedX, NewPoint.X, Tolerance)
            Assert.Equal(expectedY, NewPoint.Y, Tolerance)

        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, 4.0, 2.0, 3.0, Double.PositiveInfinity, 7.0)>
        <InlineData(Double.NegativeInfinity, 4.0, -2.0, 3.0, Double.NegativeInfinity, 7.0)>
        <InlineData(Double.NaN, 4.0, 2.0, -3.0, Double.NaN, 1.0)>
        <InlineData(3.0, Double.PositiveInfinity, 2.0, 3.0, 5.0, Double.PositiveInfinity)>
        <InlineData(3.0, Double.NegativeInfinity, -2.0, 3.0, 1.0, Double.NegativeInfinity)>
        <InlineData(3.0, Double.NaN, 2.0, -3.0, 5.0, Double.NaN)>
        <InlineData(3.0, 4.0, Double.PositiveInfinity, 3.0, Double.PositiveInfinity, 7.0)>
        <InlineData(3.0, 4.0, -Double.NegativeInfinity, 3.0, -Double.NegativeInfinity, 7.0)>
        <InlineData(3.0, 4.0, Double.NaN, -3.0, Double.NaN, 1.0)>
        <InlineData(3.0, 4.0, 2.0, Double.PositiveInfinity, 5.0, Double.PositiveInfinity)>
        <InlineData(3.0, 4.0, -2.0, Double.NegativeInfinity, 1.0, Double.NegativeInfinity)>
        <InlineData(3.0, 4.0, 2.0, -Double.NaN, 5.0, -Double.NaN)>
        Public Sub Shifted_AbnormalValues_AlsoSucceeds(movingX As Double, movingY As Double,
            shiftX As System.Double, shiftY As System.Double, expectedX As Double, expectedY As Double)

            Dim Moving As New OsnwPoint2D(movingX, movingY)

            Dim NewPoint As D2.Point = Moving.Shifted(shiftX, shiftY)

            Assert.Equal(expectedX, NewPoint.X, Tolerance)
            Assert.Equal(expectedY, NewPoint.Y, Tolerance)

        End Sub

    End Class ' PointTests

End Namespace ' GeometricTests
