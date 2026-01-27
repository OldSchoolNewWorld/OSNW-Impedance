Imports System
Imports Xunit
Imports OsnwCircle2D = OSNW.Math.Circle2D




Namespace EqualityTests

End Namespace ' EqualityTests
'xxxxxxxxxxxxxxxxxxxx




Namespace MathTests

    Public Class TestTryCircleLineIntersections

        <Theory>
        <InlineData(Double.PositiveInfinity, 6.75, 1.5, (6.25 - 6.75) / (3 - 1.75), 7.45)>
        <InlineData(1.75, Double.PositiveInfinity, 1.5, (6.25 - 6.75) / (3 - 1.75), 7.45)>
        <InlineData(1.75, 6.75, Double.PositiveInfinity, (6.25 - 6.75) / (3 - 1.75), 7.45)>
        <InlineData(1.75, 6.75, 1.5, Double.PositiveInfinity, 7.45)>
        <InlineData(1.75, 6.75, 1.5, (6.25 - 6.75) / (3 - 1.75), Double.PositiveInfinity)>
        <InlineData(1.75, 6.75, 1.5, (6.25 - 6.75) / (3 - 1.75), -1.5)>
        Sub TryCircleLineIntersectionsLine_BadInput_Fails(circleX As Double, circleY As Double, circleR As Double,
                                                          lineM As Double, lineB As Double)

            Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

            Assert.False(OsnwCircle2D.TryCircleLineIntersections(circleX, circleY, circleR, lineM, lineB,
                                                                 Intersect1X, Intersect1Y, Intersect2X, Intersect2Y))

        End Sub

        ''' <summary>
        ''' Tests TryCircleLineIntersections with a line defined by its slope and Y-intercept.<br/>
        ''' </summary>
        <Theory>
        <InlineData(1.75, 6.75, 1.5, (6.25 - 6.75) / (3 - 1.75), 7.45, 0.3558, 7.3065, 3.1428, 6.1934)>
        <InlineData(3.0, 6.25, 1.0, (6.25 - 6.75) / (3 - 1.75), 7.45, 2.0703, 6.621, 3.9293, 5.8801)>
        Sub TryCircleLineIntersectionsLine_GoodInput_Succeeds(circleX As Double, circleY As Double,
            circleR As Double, lineM As Double, lineB As Double, expect1X As Double, expect1Y As Double,
            expect2X As Double, expect2Y As Double)

            Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

            If Not OsnwCircle2D.TryCircleLineIntersections(circleX, circleY, circleR, lineM, lineB,
                                                           Intersect1X, Intersect1Y, Intersect2X, Intersect2Y) Then
                Assert.True(False)
            End If

            ' These may need to be swapped depending on order of calculation.
            Assert.Equal(Intersect2X, expect1X, 0.01)
            Assert.Equal(Intersect2Y, expect1Y, 0.01)
            Assert.Equal(Intersect1X, expect2X, 0.01)
            Assert.Equal(Intersect1Y, expect2Y, 0.01)

        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, 6.75, 1.5, 3.1698, 7.2381, 2.4427, 5.4205)>
        <InlineData(1.75, Double.PositiveInfinity, 1.5, 3.1698, 7.2381, 2.4427, 5.4205)>
        <InlineData(1.75, 6.75, Double.PositiveInfinity, 3.1698, 7.2381, 2.4427, 5.4205)>
        <InlineData(1.75, 6.75, 1.5, Double.PositiveInfinity, 7.2381, 2.4427, 5.4205)>
        <InlineData(1.75, 6.75, 1.5, 3.1698, Double.PositiveInfinity, 2.4427, 5.4205)>
        <InlineData(1.75, 6.75, 1.5, 3.1698, 7.2381, Double.PositiveInfinity, 5.4205)>
        <InlineData(1.75, 6.75, 1.5, 3.1698, 7.2381, 2.4427, Double.PositiveInfinity)>
        <InlineData(1.75, 6.75, -1.5, 3.1698, 7.2381, 2.4427, 5.4205)>
        Sub TryCircleLineIntersectionsPoints_BadInput_Fails(
             circleX As Double, circleY As Double, circleR As Double,
             lineX1 As Double, lineY1 As Double,
             lineX2 As Double, lineY2 As Double)

            Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

            Assert.False(OsnwCircle2D.TryCircleLineIntersections(circleX, circleY, circleR, lineX1, lineY1,
                lineX2, lineY2, Intersect1X, Intersect1Y, Intersect2X, Intersect2Y))

        End Sub

        ''' <summary>
        ''' Tests TryCircleLineIntersections with a line defined by two points.<br/>
        ''' </summary>
        <Theory>
        <InlineData(1.75, 6.75, 1.5,
                    3.1698, 7.2381,
                    2.4427, 5.4205,
                    3.1698, 7.2381,
                    2.4427, 5.4205)> ' Line ends on circle.
        <InlineData(1.75, 6.75, 1.5,
                    0.5, 7.875,
                    2.25, 7.875,
                    2.7422, 7.875,
                    0.7578, 7.875)> ' Horizontal line.
        <InlineData(1.75, 6.75, 1.5,
                    1, 8.25,
                    1, 5.25,
                    1, 8.049,
                    1, 5.451)> ' Vertical line.
        <InlineData(1.75, 6.75, 1.5,
                    1, 5.25,
                    1, 8.25,
                    1, 8.049,
                    1, 5.451)> ' Vertical line reversed.
        Sub TryCircleLineIntersectionsPoints_GoodInput_Succeeds(circleX As Double, circleY As Double,
            circleR As Double, lineX1 As Double, lineY1 As Double, lineX2 As Double, lineY2 As Double,
            expect1X As Double, expect1Y As Double, expect2X As Double, expect2Y As Double)

            Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

            If Not OsnwCircle2D.TryCircleLineIntersections(circleX, circleY, circleR, lineX1, lineY1,
                lineX2, lineY2, Intersect1X, Intersect1Y, Intersect2X, Intersect2Y) Then
                Assert.True(False)
            End If

            ' These may need to be swapped depending on order of calculation.
            Assert.Equal(expect1X, Intersect1X, 0.01)
            Assert.Equal(expect1Y, Intersect1Y, 0.01)
            Assert.Equal(expect2X, Intersect2X, 0.01)
            Assert.Equal(expect2Y, Intersect2Y, 0.01)

        End Sub

    End Class ' TryCircleLineIntersectionsTests

    Public Class TestTryCircleCircleIntersections

        <Theory>
        <InlineData(2.5, 6.25, 1.5, 2.75, 6.25, 1)> ' Inside, no overlap.
        <InlineData(2.5, 6.25, 1.5, 5.5, 6.25, 1)> ' Outside, no overlap.
        <InlineData(2.5, 6.25, 1.5, 2.5, 6.25, 1)> ' Concentric circles.
        Sub TryCircleCircleIntersectionsPoints_BadInput_Fails(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double)

            Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

            Assert.False(OsnwCircle2D.TryCircleCircleIntersections(
                x1, y1, r1,
                x2, y2, r2,
                Intersect1X, Intersect1Y,
                Intersect2X, Intersect2Y))

        End Sub

        <Theory>
        <InlineData(1.75, 6.75, 1.5, 3, 6.25, 1, 3.1692, 7.2356, 2.4428, 5.4196)> ' General overlap case.
        <InlineData(2.5, 6.25, 1.5, 3.5, 6.25, 1, 3.625, 7.242, 3.625, 5.2576)> ' Horizontal overlap circles.
        <InlineData(2.5, 6.25, 1.5, 3, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal inside-tangent circles.
        <InlineData(2.5, 6.25, 1.5, 5, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal outside-tangent circles.
        Sub TryCircleCircleIntersectionsPoints_GoodInput_Succeeds(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double,
            expecti1x As System.Double, expecti1y As System.Double,
            expecti2x As System.Double, expecti2y As System.Double)

            Const TOLERANCE As Double = 0.001
            Dim i1x As System.Double = expecti1x
            Dim i1y As System.Double = expecti1y
            Dim i2x As System.Double = expecti2x
            Dim i2y As System.Double = expecti2y

            If Not OsnwCircle2D.TryCircleCircleIntersections(
                x1, y1, r1,
                x2, y2, r2,
                i1x, i1y,
                i2x, i2y) Then

                Assert.True(False)
            End If

            Assert.Equal(expecti1x, i1x, TOLERANCE * expecti1x)
            Assert.Equal(expecti1y, i1y, TOLERANCE * expecti1y)
            Assert.Equal(expecti2x, i2x, TOLERANCE * expecti2x)
            Assert.Equal(expecti2y, i2y, TOLERANCE * expecti2y)

        End Sub

        <Theory>
        <InlineData(2.5, 6.25, 1.5, 2.75, 6.25, 1)> ' Inside, no overlap.
        <InlineData(2.5, 6.25, 1.5, 5.5, 6.25, 1)> ' Outside, no overlap.
        <InlineData(2.5, 6.25, 1.5, 2.5, 6.25, 1)> ' Concentric circles.
        Sub TryCircleCircleIntersectionsCircles_BadInput_Fails(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double)

            Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

            Assert.False(OsnwCircle2D.TryCircleCircleIntersections(
                x1, y1, r1,
                x2, y2, r2,
                Intersect1X, Intersect1Y,
                Intersect2X, Intersect2Y))

        End Sub

        <Theory>
        <InlineData(1.75, 6.75, 1.5, 3, 6.25, 1, 3.1692, 7.2356, 2.4428, 5.4196)> ' General overlap case.
        <InlineData(2.5, 6.25, 1.5, 3.5, 6.25, 1, 3.625, 7.242, 3.625, 5.2576)> ' Horizontal overlap circles.
        <InlineData(2.5, 6.25, 1.5, 3, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal Inside tangent circles.
        <InlineData(2.5, 6.25, 1.5, 5, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal Outside tangent circles.
        Sub TryCircleCircleIntersectionsCircles_GoodInput_Succeeds(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double,
            expecti1x As System.Double, expecti1y As System.Double,
            expecti2x As System.Double, expecti2y As System.Double)

            Const TOLERANCE As Double = 0.001
            Dim i1x As System.Double = expecti1x
            Dim i1y As System.Double = expecti1y
            Dim i2x As System.Double = expecti2x
            Dim i2y As System.Double = expecti2y

            Dim Circle1 As New OsnwCircle2D(x1, y1, r1)
            Dim Circle2 As New OsnwCircle2D(x2, y2, r2)

            If Not OsnwCircle2D.TryCircleCircleIntersections(
                Circle1, Circle2,
                i1x, i1y, i2x, i2y) Then

                Assert.True(False)
            End If

            Assert.Equal(expecti1x, i1x, TOLERANCE * expecti1x)
            Assert.Equal(expecti1y, i1y, TOLERANCE * expecti1y)
            Assert.Equal(expecti2x, i2x, TOLERANCE * expecti2x)
            Assert.Equal(expecti2y, i2y, TOLERANCE * expecti2y)

        End Sub

    End Class ' TryCircleCircleIntersectionsTests

End Namespace ' MathTests
