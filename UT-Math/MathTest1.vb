Imports Xunit
Imports OsnwCircle2D = OSNW.Math.D2.Circle
Imports OsnwEllipse2D = OSNW.Math.D2.Ellipse

' DEV: For comprehensive tests of numeric values, consider:
'   Infinity. Check expected result.
'   Just above positive limit. Fail when requiring <= limit.
'   At positive limit. Pass/fail as appropriate for the < or <= requirement.
'   Just below positive limit. Pass when requiring < limit.
'   Positive sweet spot. Only if needed for initial development of the basic
'     functionality.
'   Positive edge cases. Check expected result.
'   Near-zero positive. Maybe Epsilon?
'   Zero. Check expected result.
'   Near-zero negative. Maybe -Epsilon?
'   Negative edge cases. Check expected result.
'   Negative sweet spot. Only if needed for initial development of the basic
'     functionality.
'   Just above negative limit. Pass when requiring > or >= limit.
'   At negative limit. Pass/fail as appropriate for the requirement.
'   Just below negative limit. Fail when requiring >= limit.
'   Negative infinity. Check expected result.
'   NaN. Check expected result.

Namespace NumericTests

    Public Class EqualityTests

#Region "TestEqualEnoughAbsolute"

        <Theory>
        <InlineData(30.0, 0.002, 30.0 - 0.001)> ' Passes with Diff=-0.0010000000000012221
        <InlineData(30.0, 0.002, 30.0 + 0.001)> ' Passes with Diff=0.0010000000000012221
        Public Sub EqualEnoughAbsolute_NormalValues_Succeeds(refVal As Double, tolerance As Double,
                                                             otherVal As Double)
            Dim Result As System.Boolean = OSNW.Math.EqualEnoughAbsolute(refVal, tolerance, otherVal)
            Assert.True(Result)
        End Sub

        <Theory>
        <InlineData(30.0, Double.PositiveInfinity, 40.0)> ' Passes
        Public Sub EqualEnoughAbsolute_AbnormalValues_AlsoSucceeds(refVal As Double, tolerance As Double,
                                                                   otherVal As Double)
            Dim Result As System.Boolean = OSNW.Math.EqualEnoughAbsolute(refVal, tolerance, otherVal)
            Assert.True(Result)
        End Sub

        <Theory>
        <InlineData(30.0, 0.001, 30.0 - 0.002)>
        <InlineData(30.0, 0.001, 30.0 + 0.002)>
        <InlineData(30.0, Double.NegativeInfinity, 30.0)>
        <InlineData(30.0, Double.NaN, 30.0)>
        <InlineData(Double.PositiveInfinity, 0.001, Double.PositiveInfinity)>
        <InlineData(Double.NegativeInfinity, 0.001, Double.NegativeInfinity)>
        <InlineData(Double.NaN, 0.001, Double.NaN)>
        Public Sub EqualEnoughAbsolute_BadValues_Fails(refVal As Double, tolerance As Double,
                                                       otherVal As Double)
            Dim Result As System.Boolean = OSNW.Math.EqualEnoughAbsolute(refVal, tolerance, otherVal)
            Assert.False(Result)
        End Sub

#End Region ' "TestEqualEnoughAbsolute"

#Region "TestEqualEnoughZero"

        <Theory>
        <InlineData(0.001, 0.001)>
        <InlineData(-0.001, 0.001)>
        Public Sub EqualEnoughZero_NormalValues_Succeeds(value As Double, tolerance As Double)
            Assert.True(OSNW.Math.EqualEnoughZero(value, tolerance))
        End Sub

        <Theory>
        <InlineData(Double.NaN, 0.001)>
        <InlineData(-0.0011, 0.001)>
        <InlineData(0.0011, 0.001)>
        Public Sub EqualEnoughZero_BadValues_Fails(value As Double, tolerance As Double)
            Assert.False(OSNW.Math.EqualEnoughZero(value, tolerance))
        End Sub

#End Region ' "TestEqualEnoughZero"

#Region "TestEqualEnough"

        <Theory>
        <InlineData(1000.0, 0.001, 999)>
        <InlineData(1000.0, 0.001, 1001)>
        <InlineData(-1000.0, 0.001, -999)>
        <InlineData(-1000.0, 0.001, -1001)>
        Public Sub EqualEnough_GoodValues_Succeeds(ByVal refVal As System.Double,
            ByVal ratio As System.Double, ByVal otherVal As System.Double)

            Assert.True(OSNW.Math.EqualEnough(refVal, ratio, otherVal))
        End Sub

        <Theory>
        <InlineData(1000.0, 0.001, 998.999)>
        <InlineData(1000.0, 0.001, 1001.001)>
        <InlineData(1000.0, 0.001, Double.NaN)>
        <InlineData(Double.NaN, 0.001, Double.NaN)>
        Public Sub EqualEnough_BadValues_Fails(ByVal refVal As System.Double,
            ByVal ratio As System.Double, ByVal otherVal As System.Double)

            Assert.False(OSNW.Math.EqualEnough(refVal, ratio, otherVal))
        End Sub

        <Theory>
        <InlineData(0.0, 0.001, 1001)>
        <InlineData(1000.0, 0.001, 0.0)>
        Public Sub EqualEnough_Zero_Fails(ByVal refVal As System.Double,
            ByVal ratio As System.Double, ByVal otherVal As System.Double)

            Try
                ' Code that throws the exception.
                Dim B As Boolean = OSNW.Math.EqualEnough(refVal, ratio, otherVal)
            Catch ex As Exception
                Assert.True(True)
                Exit Sub
            End Try
            Assert.True(False, "Did not fail.")

        End Sub

#End Region ' "TestEqualEnough"

    End Class ' EqualityTests

    Public Class GeometricMeanTests

        ' Test data found at:
        ' https://www.statisticshowto.com/geometric-mean/

        <Theory>
        <InlineData(3.3019, {2.0, 3.0, 6.0})>
        <InlineData(6.81, {4.0, 8.0, 3.0, 9.0, 17.0})>
        <InlineData(0.3528, {1 / 2.0, 1 / 4.0, 1 / 5.0, 9 / 72.0, 7 / 4.0})>
        Sub GeometricMean_GoodValues_Succeeds(ByVal expect As Double,
                                              ByVal ParamArray values As Double())

            Dim Tolerance As Double = 0.001
            Dim M As Double = OSNW.Math.GeometricMean(values)
            Assert.True(OSNW.Math.EqualEnough(expect, Tolerance, M))
        End Sub

        <Theory>
        <InlineData({})> ' Empty.
        <InlineData({2.0, -3.0, 6.0})> ' Negative.
        <InlineData({2.0, 0.0, 6.0})> ' Zero.
        Sub GeometricMean_BadValues_Fails(ByVal ParamArray values As Double())
            Dim M As Double = OSNW.Math.GeometricMean(values)
            Assert.True(Double.IsNaN(M))
        End Sub

    End Class ' GeometricMeanTests

    Public Class MinMaxTests

#Region "TestMaxValue"

        <Fact>
        Sub MaxValue_InlineArray_Succeeds()
            Assert.True(OSNW.Math.MaxValue({1, 3, 4, 5, 2}).Equals(5))
        End Sub

        <Fact>
        Sub MaxValue_PassedArray_Succeeds()
            Dim Values As Double() = {1, 3, 4, 5, 2}
            Assert.True(OSNW.Math.MaxValue(Values).Equals(5))
        End Sub

        <Fact>
        Sub MaxValue_Negative_Succeeds()
            Assert.True(OSNW.Math.MaxValue({1, 3, 4, -5, 2}).Equals(4))
        End Sub

#End Region ' "TestMaxValue"

#Region "TestMaxMagnitude"

        <Fact>
        Sub MaxMagnitude_InlineArray_Succeeds()
            Assert.True(OSNW.Math.MaxMagnitude({1, 3, 5, 4, 2}).Equals(5))
        End Sub

        <Fact>
        Sub MaxMagnitude_Negative_Succeeds()
            Assert.True(OSNW.Math.MaxMagnitude({1, 3, -5, 4, 2}).Equals(5))
        End Sub

        <Fact>
        Sub MaxMagnitude_PassedArray_Succeeds()
            Dim Val1 As Double = 1
            Dim Val2 As Double = 3
            Dim Val3 As Double = -5
            Dim Val4 As Double = 4
            Dim Val5 As Double = 2
            Dim Values As Double() = {Val1, Val2, Val3, Val4, Val5}
            Assert.True(OSNW.Math.MaxMagnitude(Values).Equals(5))
        End Sub

#End Region ' "TestMaxMagnitude"

#Region "TestMinValue"

        <Fact>
        Sub MinValue_InlineArray_Succeeds()
            Assert.True(OSNW.Math.MinValue({2, 3, 1, -4, 5}).Equals(-4))
        End Sub

        <Fact>
        Sub MinValue_PassedArray_Succeeds()
            Dim Values As Double() = {2, 3, 1, -4, 5}
            Assert.True(OSNW.Math.MinValue(Values).Equals(-4))
        End Sub

#End Region ' "TestMinValue"

#Region "TestMinMagnitude"

        <Fact>
        Sub MinMagnitude_InlineArray_Succeeds()
            Assert.True(OSNW.Math.MinMagnitude({2, 3, 1, -4, 5}).Equals(1))
        End Sub

        <Fact>
        Sub MinMagnitude_PassedArray_Succeeds()
            Dim Val1 As Double = 2
            Dim Val2 As Double = 3
            Dim Val3 As Double = 1
            Dim Val4 As Double = -4
            Dim Val5 As Double = 5
            Dim Values As Double() = {Val1, Val2, Val3, Val4, Val5}
            Assert.True(OSNW.Math.MinMagnitude(Values).Equals(1))
        End Sub

        <Fact>
        Sub MinMagnitude_Negative_Succeeds()
            Assert.True(OSNW.Math.MinMagnitude({5, 3, 1, -4, 2}).Equals(1))
        End Sub

#End Region ' "TestMinMagnitude"

    End Class ' MinMaxTests

    Public Class RoundToTests

        <Theory>
        <InlineData(1.0, 17.0 / 2.0, 9.0, System.MidpointRounding.AwayFromZero)>
        <InlineData(1.0, -17.0 / 2.0, -9.0, System.MidpointRounding.AwayFromZero)>
        <InlineData(1.0, 8.5, 8.0, System.MidpointRounding.ToZero)>
        <InlineData(1.0, -8.5, -8.0, System.MidpointRounding.ToZero)>
        <InlineData(1.0, 8.5, 9.0, System.MidpointRounding.ToPositiveInfinity)>
        <InlineData(1.0, -8.5, -8.0, System.MidpointRounding.ToPositiveInfinity)>
        <InlineData(1.0, 8.5, 8.0, System.MidpointRounding.ToNegativeInfinity)>
        <InlineData(1.0, -8.5, -9.0, System.MidpointRounding.ToNegativeInfinity)>
        <InlineData(1.0, 8.5, 8.0, System.MidpointRounding.ToEven)>
        <InlineData(1.0, 7.5, 8.0, System.MidpointRounding.ToEven)>
        <InlineData(1.0, -8.5, -8.0, System.MidpointRounding.ToEven)>
        <InlineData(1.0, -7.5, -8.0, System.MidpointRounding.ToEven)>
        <InlineData(5, 28.0, 30.0)> ' Default rounding.
        <InlineData(5, 32.0, 30.0)> ' Default rounding.
        Public Sub RoundTo_NormalValues_Succeeds(nearest As Double, value As Double, expected As Double,
            Optional mode As System.MidpointRounding = OSNW.Math.OSNWDFLTMPR)

            Dim Result As System.Double = OSNW.Math.RoundTo(nearest, value, mode)
            Assert.True(OSNW.Math.EqualEnough(expected, 0.001, Result))
        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, 0.001, Double.PositiveInfinity)>
        <InlineData(Double.NaN, 0.001, Double.NaN)>
        Public Sub RoundTo_AbnormalValues_Fails(nearest As Double, value As Double, expected As Double,
            Optional mode As System.MidpointRounding = OSNW.Math.OSNWDFLTMPR)

            Dim Result As System.Double = OSNW.Math.RoundTo(nearest, value, mode)
            Assert.False(OSNW.Math.EqualEnough(expected, 0.001, Result))
        End Sub

        <Theory>
        <InlineData(Double.NegativeInfinity, Double.NegativeInfinity)>
        <InlineData(0, 32.0)> ' Zero for nearest.
        <InlineData(-5, -32.0)> ' Negative for nearest.
        Public Sub RoundTo_BadValues_Fails(nearest As Double, value As Double,
            Optional mode As System.MidpointRounding = OSNW.Math.OSNWDFLTMPR)

            Try
                ' Code that throws the exception.
                Dim Result As System.Double = OSNW.Math.RoundTo(nearest, value, mode)
            Catch ex As Exception
                Assert.True(True)
                Exit Sub
            End Try
            Assert.True(False, "Did not fail.")
        End Sub

    End Class ' RoundToTests

End Namespace ' NumericTests    

    Namespace GeometricTests

    ' XXXXXXXXXX MOVE THIS TO A GROUP FOR A PARABOLA. XXXXXXXXXX
    Public Class TestTryQuadratic

        ' TRY TO FIND MORE SETS OF VALUES THAT TRIGGER THE ALTERNATE APPROACH.
        <Theory>
        <InlineData(1.0, -3.0, 2.0, 1.0, 2.0, True)> ' Two real roots.
        <InlineData(1.0 / 2, -3.0, 5.0 / 2, 1.0, 5.0, True)> ' Two real roots.
        <InlineData(1.0, -1634.0, 2.0, 1.633_998_776E3, 0.001224, True)> ' Alternate.
        Sub TryQuadratic_GoodInput_Succeeds(
            ByVal a As System.Double, ByVal b As System.Double, ByVal c As System.Double,
            ByRef expectX0 As System.Double, ByRef expectX1 As System.Double, expectSuccess As Boolean)

            Dim X0, X1 As System.Double
            Dim Success As Boolean = OSNW.Math.TryQuadratic(a, b, c, X0, X1)
            Assert.Equal(expectSuccess, Success)
            If Success Then
                Assert.True((OSNW.Math.EqualEnough(X0, 0.001, expectX0) AndAlso
                                 OSNW.Math.EqualEnough(X1, 0.001, expectX1)) OrElse
                            (OSNW.Math.EqualEnough(X0, 0.001, expectX1) AndAlso
                                 OSNW.Math.EqualEnough(X1, 0.001, expectX0)))
                Dim ZeroTol As Double =
                    OSNW.Math.DFLTEQUALITYTOLERANCE * OSNW.Math.MaxMagnitude({a, b, c})
                Dim Y0 As Double = a * X0 ^ 2 + b * X0 + c
                Dim Y1 As Double = a * X1 ^ 2 + b * X1 + c

                '817 + 816.998_776_0 = 1.633_998_776e3
                '817 - 816.998_776_0=1.224e-3

                Assert.True(OSNW.Math.EqualEnoughZero(Y0, ZeroTol) AndAlso
                            OSNW.Math.EqualEnoughZero(Y1, ZeroTol))
            End If
        End Sub

        <Theory>
        <InlineData(0.0, -3.0, 2.0)> ' a=Zero.
        <InlineData(1.0, -3.0, 99.0)> ' Negative discriminant.
        Sub TryQuadratic_BadInput_Fails(
             ByVal a As System.Double, ByVal b As System.Double, ByVal c As System.Double)

            Dim x0, x1 As System.Double
            Dim Success As Boolean = OSNW.Math.TryQuadratic(a, b, c, x0, x1)
            Assert.False(Success)
        End Sub

    End Class ' TestTryQuadratic

    ' XXXXXXXXXX MOVE THIS TO A GROUP FOR AN ELLIPSE. XXXXXXXXXX
    Public Class TestNewEllipse

        <Fact>
        Public Sub NewEllipse_WorksOK()

            ' NOT A GOOD TEST.
            ' JUST A WAY TO WALK DATA THROUGH DEBUGGING.
            Dim Ellipse As New OsnwEllipse2D(4, 2, 3, 6, 30 * Double.Pi / 180)

        End Sub

    End Class

    ' XXXXXXXXXX MOVE THIS TO A GROUP FOR CROSSOVER TESTS? XXXXXXXXXX
#Region "CrossoverTests"

    Public Class TestTryCircleLineIntersections

        Const POSINF As Double = Double.PositiveInfinity

        '' Suspend due to allowing NaN and infinity.
        '<Theory>
        '<InlineData(POSINF, 6.75, 1.5, (6.25 - 6.75) / (3 - 1.75), 7.45)>
        '<InlineData(1.75, POSINF, 1.5, (6.25 - 6.75) / (3 - 1.75), 7.45)>
        '<InlineData(1.75, 6.75, POSINF, (6.25 - 6.75) / (3 - 1.75), 7.45)>
        '<InlineData(1.75, 6.75, 1.5, POSINF, 7.45)>
        '<InlineData(1.75, 6.75, 1.5, (6.25 - 6.75) / (3 - 1.75), POSINF)>
        '<InlineData(1.75, 6.75, 1.5, (6.25 - 6.75) / (3 - 1.75), -1.5)>
        'Sub TryCircleLineIntersectionsLine_BadInput_Fails(circleX As Double, circleY As Double,
        '    circleR As Double, lineM As Double, lineB As Double)

        '    Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

        '    Assert.False(OsnwCircle2D.TryCircleLineIntersections(circleX, circleY, circleR, lineM, lineB,
        '        Intersect1X, Intersect1Y, Intersect2X, Intersect2Y))

        'End Sub

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

        '' Suspend due to allowing NaN and infinity.
        '<Theory>
        '<InlineData(POSINF, 6.75, 1.5, 3.1698, 7.2381, 2.4427, 5.4205)>
        '<InlineData(1.75, POSINF, 1.5, 3.1698, 7.2381, 2.4427, 5.4205)>
        '<InlineData(1.75, 6.75, POSINF, 3.1698, 7.2381, 2.4427, 5.4205)>
        '<InlineData(1.75, 6.75, 1.5, POSINF, 7.2381, 2.4427, 5.4205)>
        '<InlineData(1.75, 6.75, 1.5, 3.1698, POSINF, 2.4427, 5.4205)>
        '<InlineData(1.75, 6.75, 1.5, 3.1698, 7.2381, POSINF, 5.4205)>
        '<InlineData(1.75, 6.75, 1.5, 3.1698, 7.2381, 2.4427, POSINF)>
        '<InlineData(1.75, 6.75, -1.5, 3.1698, 7.2381, 2.4427, 5.4205)>
        'Sub TryCircleLineIntersectionsPoints_BadInput_Fails(
        '     circleX As Double, circleY As Double, circleR As Double,
        '     lineX1 As Double, lineY1 As Double, lineX2 As Double, lineY2 As Double)

        '    Dim Intersect1X, Intersect1Y, Intersect2X, Intersect2Y As Double

        '    Assert.False(OsnwCircle2D.TryCircleLineIntersections(circleX, circleY, circleR, lineX1, lineY1,
        '        lineX2, lineY2, Intersect1X, Intersect1Y, Intersect2X, Intersect2Y))

        'End Sub

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

#End Region ' "CrossoverTests"

    ' XXXXXXXXXX MOVE THIS TO A GROUP FOR CIRCLE TESTS? XXXXXXXXXX
    Public Class TestTryCircleCircleIntersections

        <Theory>
        <InlineData(1.75, 6.75, -1.5, 3, 6.25, 1)> ' Negative radius.
        <InlineData(1.75, 6.75, 1.5, 3, 6.25, -1)> ' Negative radius.
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
        Sub TryCircleCircleIntersectionsPoints_Hint_Succeeds(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double,
            expecti1x As System.Double, expecti1y As System.Double,
            expecti2x As System.Double, expecti2y As System.Double)

            Const TOLERANCE As Double = 0.001

            ' Run the in-progress tests, supplying the expected results.
            Dim Out1x As System.Double = expecti1x
            Dim Out1y As System.Double = expecti1y
            Dim Out2x As System.Double = expecti2x
            Dim Out2y As System.Double = expecti2y

            If Not OsnwCircle2D.TryCircleCircleIntersections(x1, y1, r1, x2, y2, r2, Out1x, Out1y, Out2x, Out2y) Then
                ' It failed internally.
                Assert.True(False)
            Else
                ' It thinks all went ok; check actual results.
                Assert.Equal(expecti1x, Out1x, TOLERANCE * expecti1x)
                Assert.Equal(expecti1y, Out1y, TOLERANCE * expecti1y)
                Assert.Equal(expecti2x, Out2x, TOLERANCE * expecti2x)
                Assert.Equal(expecti2y, Out2y, TOLERANCE * expecti2y)
            End If

        End Sub

        <Theory>
        <InlineData(1.75, 6.75, 1.5, 3, 6.25, 1, 3.1692, 7.2356, 2.4428, 5.4196)> ' General overlap case.
        <InlineData(2.5, 6.25, 1.5, 3.5, 6.25, 1, 3.625, 7.242, 3.625, 5.2576)> ' Horizontal overlap circles.
        <InlineData(2.5, 6.25, 1.5, 3, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal inside-tangent circles.
        <InlineData(2.5, 6.25, 1.5, 5, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal outside-tangent circles.
        Sub TryCircleCircleIntersectionsPoints_NoHint_Succeeds(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double,
            expecti1x As System.Double, expecti1y As System.Double,
            expecti2x As System.Double, expecti2y As System.Double)

            Const TOLERANCE As Double = 0.001

            ' Skip in-progress tests, not supplying the expected results.
            Dim Out1x As System.Double
            Dim Out1y As System.Double
            Dim Out2x As System.Double
            Dim Out2y As System.Double

            If Not OsnwCircle2D.TryCircleCircleIntersections(x1, y1, r1, x2, y2, r2, Out1x, Out1y, Out2x, Out2y) Then
                ' It failed internally.
                Assert.True(False)
            Else
                ' It thinks all went ok; check actual results.
                Assert.Equal(expecti1x, Out1x, TOLERANCE * expecti1x)
                Assert.Equal(expecti1y, Out1y, TOLERANCE * expecti1y)
                Assert.Equal(expecti2x, Out2x, TOLERANCE * expecti2x)
                Assert.Equal(expecti2y, Out2y, TOLERANCE * expecti2y)
            End If

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
        Sub TryCircleCircleIntersectionsCircles_Hint_Succeeds(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double,
            expecti1x As System.Double, expecti1y As System.Double,
            expecti2x As System.Double, expecti2y As System.Double)

            Const TOLERANCE As Double = 0.001

            Dim Circle1 As New OsnwCircle2D(x1, y1, r1)
            Dim Circle2 As New OsnwCircle2D(x2, y2, r2)

            ' Run the in-progress tests, supplying the expected results.
            Dim Out1x As System.Double = expecti1x
            Dim Out1y As System.Double = expecti1y
            Dim Out2x As System.Double = expecti2x
            Dim Out2y As System.Double = expecti2y

            If Not OsnwCircle2D.TryCircleCircleIntersections(Circle1, Circle2, Out1x, Out1y, Out2x, Out2y) Then
                ' It failed internally.
                Assert.True(False)
            Else
                ' It thinks all went ok; check actual results.
                Assert.Equal(expecti1x, Out1x, TOLERANCE * expecti1x)
                Assert.Equal(expecti1y, Out1y, TOLERANCE * expecti1y)
                Assert.Equal(expecti2x, Out2x, TOLERANCE * expecti2x)
                Assert.Equal(expecti2y, Out2y, TOLERANCE * expecti2y)
            End If

        End Sub

        <Theory>
        <InlineData(1.75, 6.75, 1.5, 3, 6.25, 1, 3.1692, 7.2356, 2.4428, 5.4196)> ' General overlap case.
        <InlineData(2.5, 6.25, 1.5, 3.5, 6.25, 1, 3.625, 7.242, 3.625, 5.2576)> ' Horizontal overlap circles.
        <InlineData(2.5, 6.25, 1.5, 3, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal Inside tangent circles.
        <InlineData(2.5, 6.25, 1.5, 5, 6.25, 1, 4, 6.25, 4, 6.25)> ' Horizontal Outside tangent circles.
        Sub TryCircleCircleIntersectionsCircles_NoHint_Succeeds(
            x1 As System.Double, y1 As System.Double, r1 As System.Double,
            x2 As System.Double, y2 As System.Double, r2 As System.Double,
            expecti1x As System.Double, expecti1y As System.Double,
            expecti2x As System.Double, expecti2y As System.Double)

            Const TOLERANCE As Double = 0.001

            Dim Circle1 As New OsnwCircle2D(x1, y1, r1)
            Dim Circle2 As New OsnwCircle2D(x2, y2, r2)

            ' Skip in-progress tests, not supplying the expected results.
            Dim Out1x As System.Double
            Dim Out1y As System.Double
            Dim Out2x As System.Double
            Dim Out2y As System.Double

            If Not OsnwCircle2D.TryCircleCircleIntersections(Circle1, Circle2, Out1x, Out1y, Out2x, Out2y) Then
                ' It failed internally.
                Assert.True(False)
            Else
                ' It thinks all went ok; check the actual results.
                Assert.Equal(expecti1x, Out1x, TOLERANCE * expecti1x)
                Assert.Equal(expecti1y, Out1y, TOLERANCE * expecti1y)
                Assert.Equal(expecti2x, Out2x, TOLERANCE * expecti2x)
                Assert.Equal(expecti2y, Out2y, TOLERANCE * expecti2y)
            End If

        End Sub

    End Class ' TryCircleCircleIntersectionsTests

End Namespace ' GeometricTests
