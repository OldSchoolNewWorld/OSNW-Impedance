Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Public Module Math

#Region "Constants"

    ''' <summary>
    ''' This sets a practical limit on the precision of equality detection in
    ''' graphics operations. It is intended to prevent issues arising from
    ''' floating point precision limitations. This should account for
    ''' indistinguishable, sub-pixel, differences on any current monitor or
    ''' printer. A smaller value DEcreases the liklihood of detecting equality;
    ''' a larger value INcreases the liklihood of detecting equality.
    ''' </summary>
    Public Const GRAPHICTOLERANCE As System.Double = 0.0001

    'Public Const PI As System.Double = System.Double.Pi
    Public Const HALFPI As System.Double = System.Double.Pi / 2.0

    Public Const MSGCHIV As System.String = "Cannot have an infinite value."
    Public Const MSGCHNV As System.String = "Cannot have a negative value."
    Public Const MSGCHZV As System.String = "Cannot have a zero value."
    'Public Const MSGFGPXPY As System.String = "Failure getting PlotX, PlotY."
    'Public Const MSGFIXEDSIZEVIOLATION As System.String =
    '    "cannot modify the fixed-size ImageImpedanceList."
    'Public Const MSGIIC As System.String = "Invalid intersection count."
    'Public Const MSGNOSTR As System.String = "Cannot be Null/Nothing."
    'Public Const MSGTDNRT As String = " transformation did not reach target."
    Public Const MSGUEEZ As System.String = MSGCHZV & " Use EqualEnoughZero()."
    Public Const MSGVMBGTE1 As System.String =
        "Must be greater than or equal to 1."
    Public Const MSGVMBGTZ As System.String =
        "Must be a positive, non-zero value."

#End Region ' Constants

#Region "EqualEnough Implementations"

    ' REF: Precision and complex numbers
    ' https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers

    ' REF: Random ASCII – tech blog of Bruce Dawson
    ' https://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/

    ''' <summary>
    ''' Check for reasonable equality when using floating point values. A
    ''' difference of less than or equal to <paramref name="maxDiff"/> is
    ''' considered to establish equality.
    ''' </summary>
    ''' <param name="otherVal">Specifies the value to be compared to
    ''' <paramref name="refVal"/>.</param>
    ''' <param name="refVal">Specifies the reference value to which
    ''' <paramref name="otherVal"/> is compared.</param>
    ''' <param name="maxDiff">Specifies the maximum difference that satisfies
    ''' equality.</param>
    ''' <returns><c>True</c> if the values are reasonably close in value;
    ''' otherwise, <c>False</c>.</returns>
    ''' <remarks>
    ''' This does the comparison based on an absolute numeric difference. The
    ''' control value is <paramref name="maxDiff"/>. Select
    ''' <paramref name="maxDiff"/> such that it is a good representation of
    ''' zero, relative to other known or expected values.</remarks>
    Public Function EqualEnoughAbsolute(ByVal otherVal As System.Double,
        ByVal refVal As System.Double, ByVal maxDiff As System.Double) _
        As System.Boolean

        ' No input checking.
        Return System.Math.Abs(otherVal - refVal) <= maxDiff
    End Function ' EqualEnoughAbsolute

    ''' <summary>
    ''' Check for reasonable equality to zero when using floating point values.
    ''' Any value less than or equal to <paramref name="zeroTolerance"/> from
    ''' zero is considered to be equal to zero.
    ''' </summary>
    ''' <param name="value">Specifies the value to be compared to zero.</param>
    ''' <param name="zeroTolerance">Specifies the maximum offset from zero which
    ''' is assumed to represent zero.</param>
    ''' <returns><c>True</c> if <paramref name="value"/> is reasonably close to
    ''' zero; otherwise, <c>False</c>.</returns>
    ''' <remarks>Use this when an actual zero reference would cause a failure in
    ''' <see cref="EqualEnough(System.Double, System.Double, System.Double)"/>.
    ''' Select <paramref name="zeroTolerance"/> such that it is a good
    ''' representation of zero relative to other known or expected
    ''' values.</remarks>
    Public Function EqualEnoughZero(ByVal value As System.Double,
        ByVal zeroTolerance As System.Double) As System.Boolean

        ' No input checking.
        Return System.Math.Abs(value) <= System.Math.Abs(zeroTolerance)
    End Function ' EqualEnoughZero

    ''' <summary>
    ''' Check for reasonable equality, within a specified ratio, when using
    ''' floating point values.
    ''' </summary>
    ''' <param name="otherVal">Specifies the value to be compared to
    ''' <paramref name="refVal"/>.</param>
    ''' <param name="refVal">Specifies the reference value to which
    ''' <paramref name="otherVal"/> is compared.</param>
    ''' <param name="ratio">Specifies the maximum ratio of the values which is
    ''' assumed to represent equality.</param>
    ''' <returns><c>True</c> if the values are reasonably close in value;
    ''' otherwise, <c>False</c>.</returns>
    ''' <exception cref="System.ArgumentOutOfRangeException">When either
    ''' compared value is zero.</exception>
    ''' <remarks>
    ''' This does the comparison based on scale, not on an absolute numeric
    ''' difference. The control value is <paramref name="ratio"/> multiplied
    ''' by <paramref name="refVal"/>, to determine the minimum difference that
    ''' excludes equality.<br/>
    ''' There is no way to scale a comparison to zero. When a zero reference
    ''' would cause a failure here, use
    ''' <see cref="EqualEnoughZero(System.Double, System.Double)"/>.
    ''' </remarks>
    Public Function EqualEnough(ByVal otherVal As System.Double,
        ByVal refVal As System.Double, ByVal ratio As System.Double) _
        As System.Boolean

        ' Input checking.
        If refVal.Equals(0.0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(refVal), MSGUEEZ)
        End If
        If otherVal.Equals(0.0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(otherVal), MSGUEEZ)
        End If

        Return System.Math.Abs(otherVal - refVal) <
            System.Math.Abs(ratio * refVal)

    End Function ' EqualEnough

#End Region ' "EqualEnough Implementations"

    ''' <summary>
    ''' Computes the distance between two points in a 3D space.
    ''' </summary>
    ''' <param name="x1">Specifies the X-coordinate of one point.</param>
    ''' <param name="y1">Specifies the Y-coordinate of one point.</param>
    ''' <param name="z1">Specifies the Z-coordinate of one point.</param>
    ''' <param name="x2">Specifies the X-coordinate of the other point.</param>
    ''' <param name="y2">Specifies the Y-coordinate of the other point.</param>
    ''' <param name="z2">Specifies the Z-coordinate of the other point.</param>
    ''' <returns>The distance between the two points.</returns>
    Public Function Distance3D(ByVal x1 As System.Double,
        ByVal y1 As System.Double, ByVal z1 As System.Double,
        ByVal x2 As System.Double, ByVal y2 As System.Double,
        ByVal z2 As System.Double) As System.Double

        ' Based on the Pythagorean theorem.
        Dim DeltaX As System.Double = x2 - x1
        Dim DeltaY As System.Double = y2 - y1
        Dim DeltaZ As System.Double = z2 - z1
        Return System.Math.Sqrt(
            (DeltaX * DeltaX) + (DeltaY * DeltaY) + (DeltaZ * DeltaZ))
    End Function ' Distance3D

    ''' <summary>
    ''' Attempts to solve the "a*x^2 + b*x + c = 0" quadratic equation for real
    ''' solutions.
    ''' </summary>
    ''' <param name="a">Specifies the <paramref name="a"/> in the well-known
    ''' formula.</param>
    ''' <param name="b">Specifies the <paramref name="b"/> in the well-known
    ''' formula.</param>
    ''' <param name="c">Specifies the <paramref name="c"/> in the well-known
    ''' formula.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' When valid, also returns the results in <paramref name="x1"/> and
    ''' <paramref name="x2"/>.</returns>
    Public Function TryQuadratic(ByVal a As System.Double,
        ByVal b As System.Double, ByVal c As System.Double,
        ByRef x1 As System.Double, ByRef x2 As System.Double) As System.Boolean

        ' Input checking.
        Dim Discriminant As System.Double = b * b - 4.0 * a * c
        If a.Equals(0.0) OrElse Discriminant < 0.0 Then
            ' Not a quadratic equation.
            x1 = Double.NaN
            x2 = Double.NaN
            Return False
        End If

        Dim DiscRoot As System.Double = System.Math.Sqrt(Discriminant)
        Dim A2 As System.Double = 2.0 * a
        x1 = (-b + DiscRoot) / A2
        x2 = (-b - DiscRoot) / A2
        Return True

    End Function ' TryQuadratic

    '''' Suspended XML comments for suspended code:
    '''' <exception cref="System.ArgumentOutOfRangeException">
    '''' Thrown when <paramref name="circleX"/>, <paramref name="circleY"/>,
    '''' <paramref name="circleR"/>, <paramref name="lineM"/>, or
    '''' <paramref name="lineB"/> is infinite.
    '''' </exception>
    '''' <exception cref="System.ArgumentOutOfRangeException">
    '''' Thrown when <paramref name="circleR"/> is less than or equal to zero.
    '''' </exception>
    ''' <summary>
    ''' Attempts to solve where a line intersects a circle, given the center
    ''' coordinates and radius of the circle, along with the slope and
    ''' Y-intercept of the line.
    ''' </summary>
    ''' <param name="circleX">Specifies the X-coordinate of the center of the
    ''' circle.</param>
    ''' <param name="circleY">Specifies the Y-coordinate of the center of the
    ''' circle.</param>
    ''' <param name="circleR">Specifies the radius of the circle.</param>
    ''' <param name="lineM">Specifies the slope of the line.</param>
    ''' <param name="lineB">Specifies the Y-intercept of the line.</param>
    ''' <param name="intersect1X">Specifies the X-coordinate of one
    ''' intersection.</param>
    ''' <param name="intersect1Y">Specifies the Y-coordinate of one
    ''' intersection.</param>
    ''' <param name="intersect2X">Specifies the X-coordinate of the other
    ''' intersection.</param>
    ''' <param name="intersect2Y">Specifies the Y-coordinate of the other
    ''' intersection.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' When valid, also returns the results in <paramref name="intersect1X"/>,
    ''' <paramref name="intersect1Y"/>, <paramref name="intersect2X"/>, and
    ''' <paramref name="intersect2Y"/>.</returns>
    ''' <remarks>
    ''' A vertical line (infinite slope) will not have a Y-intercept, except
    ''' when that line passes through the circle center, a case which would have
    ''' infinite common points.
    ''' <br/>
    ''' To avoid throwing an exception, <c>False</c> is returned
    ''' when <paramref name="circleX"/>, <paramref name="circleY"/>,
    ''' <paramref name="circleR"/>, <paramref name="lineM"/>, or
    ''' <paramref name="lineB"/> is infinite,
    ''' or
    ''' when <paramref name="circleR"/> is less than or equal to zero.
    ''' </remarks>
    Public Function TryCircleLineIntersection(
        ByVal circleX As System.Double, ByVal circleY As System.Double,
        ByVal circleR As System.Double, ByVal lineM As System.Double,
        ByVal lineB As System.Double, ByRef intersect1X As System.Double,
        ByRef intersect1Y As System.Double, ByRef intersect2X As System.Double,
        ByRef intersect2Y As System.Double) As System.Boolean

        ' Input checking.
        ' Suspended to avoid exceptions:
        '        If System.Double.IsInfinity(circleX) OrElse
        '            System.Double.IsInfinity(circleY) OrElse
        '            System.Double.IsInfinity(circleR) OrElse
        '            System.Double.IsInfinity(lineM) OrElse
        '            System.Double.IsInfinity(lineB) Then
        '            'Dim CaughtBy As System.Reflection.MethodBase =
        '            '    System.Reflection.MethodBase.GetCurrentMethod
        '            Throw New System.ArgumentOutOfRangeException(
        '                $"Arguments to {NameOf(TryCircleLineIntersection)} {MSGCHIV}")
        '        End If
        '        If circleR <= 0.0 Then
        '            'Dim CaughtBy As System.Reflection.MethodBase =
        '            '    System.Reflection.MethodBase.GetCurrentMethod
        '            Throw New System.ArgumentOutOfRangeException(
        '                NameOf(circleR), MSGVMBGTZ)
        '        End If
        If System.Double.IsInfinity(circleX) OrElse
            System.Double.IsInfinity(circleY) OrElse
            System.Double.IsInfinity(circleR) OrElse
            System.Double.IsInfinity(lineM) OrElse
            System.Double.IsInfinity(lineB) OrElse
            circleR <= 0.0 Then
            Return False
        End If

        ' The derivation follows:
        ' Standard form of a circle and a line.
        ' (X - circleX)^2 + (Y - circleY)^2 = circleR^2
        ' Y = lineM * X + B

        ' Localize parameters, for one point of intersection.
        ' (intersect1X - circleX)^2 + (intersect1Y - circleY)^2 = circleR^2
        ' y1 = lineM * intersect1X + lineB

        ' A point at the intersection of the circle and the line conforms to
        ' both equations.
        ' (intersect1X - circleX)^2
        '     + ((lineM * intersect1X + lineB)- circleY)^2 = circleR^2
        ' (intersect1X - circleX)^2
        '     + (lineM * intersect1X + lineB - circleY)^2 = circleR^2

        ' Rewrite for visibility.
        ' (intersect1X - circleX)^2
        ' + ((lineM * intersect1X) + lineB - circleY)^2
        ' = circleR^2

        ' Expand the squares.
        ' intersect1X^2 - (2 * circleX * intersect1X) + circleX^2
        ' + (lineM * intersect1X)
        '     * ((lineM * intersect1X) + lineB - circleY)
        ' + lineB * ((lineM * intersect1X) + lineB - circleY)
        ' - circleY * ((lineM * intersect1X) + lineB - circleY)
        ' = circleR^2

        ' Distribute the multiplications.
        ' intersect1X^2 -2*circleX*intersect1X + circleX^2
        ' + (lineM*intersect1X*lineM*intersect1X + lineM*intersect1X*lineB
        '     - lineM*intersect1X*circleY)
        ' + (lineB*lineM*intersect1X + lineB*lineB - lineB*circleY)
        ' - (circleY*lineM*intersect1X + circleY*lineB - circleY*circleY)
        ' = circleR^2

        ' Normalize terms.
        ' intersect1X^2 -2*circleX*intersect1X + circleX^2
        ' + lineM^2*intersect1X^2 + lineM*lineB*intersect1X
        '     - lineM*circleY*intersect1X
        ' + lineB*lineM*intersect1X + lineB^2 - lineB*circleY
        ' - circleY*lineM*intersect1X - circleY*lineB + circleY*circleY
        ' = circleR^2

        ' Gather like terms. Arrange for quadratic formula.
        ' intersect1X^2 + (lineM^2)*intersect1X^2
        ' -(2*circleX)*intersect1X + (2*lineM*lineB)*intersect1X
        '     - (2*lineM*circleY)*intersect1X
        ' + circleX^2 + lineB^2 - 2*lineB*circleY + circleY^2 - circleR^2
        ' = 0

        ' Extract X terms.
        ' (1 + (lineM^2))*intersect1X^2
        ' + (lineM*(lineB - circleY) - circleX)*2*intersect1X
        ' circleX^2 + lineB*(lineB - 2*circleY) + circleY^2 - circleR^2
        ' = 0

        ' Set up for the quadratic formula.
        ' a = (1 + (lineM^2))
        ' b = (lineM*(lineB - circleY) - circleX)*2
        ' c = circleX^2 + lineB*(lineB - 2*circleY) + circleY^2 - circleR^2

        ' Implementation:

        Dim a As System.Double = 1 + (lineM ^ 2)
        Dim b As System.Double = 2 * (lineM * (lineB - circleY) - circleX)
        Dim c As System.Double = circleX ^ 2 + lineB * (lineB - 2 * circleY) +
                                 circleY ^ 2 - circleR ^ 2
        If Not TryQuadratic(a, b, c, intersect1X, intersect2X) Then
            intersect1X = System.Double.NaN
            intersect1Y = System.Double.NaN
            intersect2X = System.Double.NaN
            intersect2Y = System.Double.NaN
            Return False
        End If

        ' Substitute into "y = mx + b".
        intersect1Y = lineM * intersect1X + lineB
        intersect2Y = lineM * intersect2X + lineB
        Return True

    End Function ' TryCircleLineIntersection

    '''' Suspended XML comments for suspended code:
    '''' <exception cref="System.ArgumentOutOfRangeException">
    '''' Thrown when <paramref name="circleX"/>, <paramref name="circleY"/>,
    '''' <paramref name="circleR"/>, <paramref name="lineX1"/>,
    '''' <paramref name="lineX2"/>, <paramref name="lineY1"/>, or
    '''' <paramref name="lineY2"/> is infinite.
    '''' </exception>
    '''' <exception cref="System.ArgumentOutOfRangeException">
    '''' Thrown when <paramref name="circleR"/> is less than or equal to zero.
    '''' </exception>
    ''' <summary>
    ''' Attempts to solve where a line intersects a circle, given the center
    ''' coordinates and radius of the circle, along with the coordinates of two
    ''' points on the line..
    ''' </summary>
    ''' <param name="circleX">Specifies the X-coordinate of the center of the
    ''' circle.</param>
    ''' <param name="circleY">Specifies the Y-coordinate of the center of the
    ''' circle.</param>
    ''' <param name="circleR">Specifies the radius of the circle.</param>
    ''' <param name="lineX1">Specifies the X-coordinate of the first point on
    ''' the line.</param>
    ''' <param name="lineY1">Specifies the Y-coordinate of the first point on
    ''' the line.</param>
    ''' <param name="lineX2">Specifies the X-coordinate of the second point on
    ''' the line.</param>
    ''' <param name="lineY2">Specifies the Y-coordinate of the second point on
    ''' the line.</param>
    ''' <param name="intersect1X">Specifies the X-coordinate of one
    ''' intersection.</param>
    ''' <param name="intersect1Y">Specifies the Y-coordinate of one
    ''' intersection.</param>
    ''' <param name="intersect2X">Specifies the X-coordinate of the other
    ''' intersection.</param>
    ''' <param name="intersect2Y">Specifies the Y-coordinate of the other
    ''' intersection.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' When valid, also returns the results in <paramref name="intersect1X"/>,
    ''' <paramref name="intersect1Y"/>, <paramref name="intersect2X"/>, and
    ''' <paramref name="intersect2Y"/>.</returns>
    ''' <remarks>
    ''' To avoid throwing an exception, <c>False</c> is returned
    ''' when <paramref name="circleX"/>, <paramref name="circleY"/>,
    ''' <paramref name="circleR"/>, <paramref name="lineX1"/>,
    ''' or <paramref name="lineY1"/>, or <paramref name="lineY2"/> is infinite,
    ''' or
    ''' when <paramref name="circleR"/> is less than or equal to zero.
    ''' </remarks>
    Public Function TryCircleLineIntersection(ByVal circleX As System.Double,
        ByVal circleY As System.Double, ByVal circleR As System.Double,
        ByVal lineX1 As System.Double, ByVal lineY1 As System.Double,
        ByVal lineX2 As System.Double, ByVal lineY2 As System.Double,
        ByRef intersect1X As System.Double, ByRef intersect1Y As System.Double,
        ByRef intersect2X As System.Double,
        ByRef intersect2Y As System.Double) As System.Boolean

        ' Input checking.
        ' Suspended to avoid exceptions:
        '       If System.Double.IsInfinity(circleX) OrElse
        '           System.Double.IsInfinity(circleY) OrElse
        '           System.Double.IsInfinity(circleR) OrElse
        '           System.Double.IsInfinity(lineX1) OrElse
        '           System.Double.IsInfinity(lineY1) OrElse
        '           System.Double.IsInfinity(lineX2) OrElse
        '           System.Double.IsInfinity(lineY2) Then
        '           'Dim CaughtBy As System.Reflection.MethodBase =
        '           '    System.Reflection.MethodBase.GetCurrentMethod
        '           Throw New System.ArgumentOutOfRangeException(
        '               $"Arguments to {NameOf(TryCircleLineIntersection)} {MSGCHIV}")
        '       End If
        '        If circleR <= 0.0 Then
        '            'Dim CaughtBy As System.Reflection.MethodBase =
        '            '    System.Reflection.MethodBase.GetCurrentMethod
        '            Throw New System.ArgumentOutOfRangeException(
        '                NameOf(circleR), MSGVMBGTZ)
        '        End If
        If System.Double.IsInfinity(circleX) OrElse
            System.Double.IsInfinity(circleY) OrElse
            System.Double.IsInfinity(circleR) OrElse
            System.Double.IsInfinity(lineX1) OrElse
            System.Double.IsInfinity(lineY1) OrElse
            System.Double.IsInfinity(lineX2) OrElse
            System.Double.IsInfinity(lineY2) OrElse
            circleR <= 0.0 Then
            Return False
        End If

        ' Check for a vertical line.
        Dim DeltaX As System.Double = lineX2 - lineX1
        If DeltaX.Equals(0.0) Then
            ' Vertical line; X = lineX1.

            ' Can there be an intersection?
            If System.Math.Abs(lineX1 - circleX) > circleR Then
                ' No intersection possible.
                intersect1X = System.Double.NaN
                intersect1Y = System.Double.NaN
                intersect2X = System.Double.NaN
                intersect2Y = System.Double.NaN
                Return False
            End If

            ' The derivation follows:
            ' Standard form of a circle and a line.
            ' (X - h)^2 + (Y - k)^2 = r^2

            ' Substitute parameters into well-known circle equation.
            ' (X - circleX)^2 + (Y - circleY)^2 = circleR^2
            ' (Y - circleY)^2 = circleR^2 - (X - circleX)^2
            ' Y - circleY = sqrt(circleR^2 - (X - circleX)^2)
            ' Y = circleY + sqrt(circleR^2 - (X - circleX)^2)
            ' Y = circleY + sqrt(circleR^2 - (lineX1 - circleX)^2)

            ' Get the Y values.
            ' Root = sqrt(circleR^2 - (lineX1 - circleX)^2)
            ' intersect1Y = circleY + Root
            ' intersect2Y = circleY - Root

            Dim Minus As System.Double = lineX1 - circleX
            Dim Root As System.Double =
                System.Math.Sqrt((circleR * circleR) - (Minus * Minus))
            intersect1Y = circleY + Root
            intersect2Y = circleY - Root
            intersect1X = lineX1
            intersect2X = lineX1 ' Yes, the same assignment.
            Return True

        End If ' Vertical line.

        ' On getting here, the line is not vertical.

        ' Get the slope of the line.
        ' M = (Y2 - Y1) / (X2 - X1); generic slope.
        Dim lineM As System.Double = (lineY2 - lineY1) / DeltaX

        ' Get the equation for the line.
        ' Y = M*X + B; Standard form line.
        ' B = Y - M*X; Solve for the Y-intercept.
        Dim lineB As System.Double = lineY1 - lineM * lineX1

        Return TryCircleLineIntersection(circleX, circleY, circleR, lineM,
            lineB, intersect1X, intersect1Y, intersect2X, intersect2Y)

    End Function ' TryCircleLineIntersection

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="x1">xxxxxxxxxx</param>
    ''' <param name="y1">xxxxxxxxxx</param>
    ''' <param name="r1">xxxxxxxxxx</param>
    ''' <param name="int2X">xxxxxxxxxx</param>
    ''' <param name="y2">xxxxxxxxxx</param>
    ''' <param name="r2">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx
    ''' </returns>
    ''' <remarks>
    ''' Tangent circles will have only one intersection. When both circles
    ''' specify the same circle, they are considered not to intersect.
    ''' </remarks>
    Public Function CirclesIntersect(ByVal x1 As System.Double,
        ByVal y1 As System.Double, ByVal r1 As System.Double,
        ByVal int2X As System.Double, ByVal y2 As System.Double,
        ByVal r2 As System.Double) As System.Boolean

        ' Input checking.
        If (r1 < 0.0) OrElse (r2 < 0.0) Then
            Return False
        End If

        ' Check for solvability.
        Dim CtrSeparation As System.Double =
            System.Double.Hypot(int2X - x1, y2 - y1)
        If CtrSeparation > (r1 + r2) Then
            ' Two isolated circles.
            Return False
        ElseIf CtrSeparation < System.Math.Abs(r2 - r1) Then
            ' One inside the other.
            Return False
        ElseIf int2X.Equals(x1) AndAlso y2.Equals(y1) Then
            ' They are concentric, with either zero or infinite common points.
            ' The second case is consirered not to be intersecting.
            Return False
        End If
        Return True

    End Function ' CirclesIntersect

    Public Function TryCircleIntersection(ByVal centerX1 As System.Double,
        ByVal centerY1 As System.Double, ByVal radius1 As System.Double,
        ByVal centerint2X As System.Double, ByVal centerY2 As System.Double,
        ByVal radius2 As System.Double, ByRef intersect1X As System.Double,
        ByRef intersect1Y As System.Double, ByRef intersect2X As System.Double,
        ByRef intersect2Y As System.Double) As System.Boolean

        If Not CirclesIntersect(centerX1, centerY1, radius1,
            centerint2X, centerY2, radius2) Then
            intersect1X = Double.NaN
            intersect1Y = Double.NaN
            intersect2X = Double.NaN
            intersect2Y = Double.NaN
            Return False
        End If

        intersect1X = 999.99
        intersect1Y = 999.99
        intersect2X = 999.99
        intersect2Y = 999.99



        '        xxxx




        Return False ' Until implemented.

    End Function ' TryCircleIntersection

End Module ' Math

' Partial Public Structure Math
' 
'     ''' <summary>
'     ''' A structure to define two circles in order to consider their overlap relationship.
'     ''' </summary>
'     ''' <remarks>
'     ''' 
'     ''' Check the status of CirclesIntersect before reading the intersection points!
'     ''' 
'     ''' The following possibilities have been identified for two circles:
'     '''   They may be separate and not intersect.
'     '''   They may be externally tangent and intersect at one point.
'     '''   They may intersect at two points.
'     '''   A small circle may be internally tangent to a large circle and intersect at one point.
'     '''   A small circle may be inside the other with no intersecting points.
'     '''   They may describe the same circle. CirclesIntersect returns <c>False</c> for this case.
'     ''' 
'     ''' </remarks>
'     Public Structure IntersectingCircleData
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Creates a new Ytt.Util.Math.IntersectingCircleData.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Sub New(ByVal circle1 As Ytt.Util.Math.CircleData, ByVal circle2 As Ytt.Util.Math.CircleData)
'         ''' </declaration>
'         ''' <param name="circle1">Describes one circle.</param>
'         ''' <param name="circle2">Describes one circle.</param>
'         ''' <remarks></remarks>
'         Public Sub New(ByVal circle1 As Ytt.Util.Math.CircleData, ByVal circle2 As Ytt.Util.Math.CircleData)
'             Me.New(circle1.CenterX, circle1.CenterY, circle1.Radius,
'                    circle2.CenterX, circle2.CenterY, circle2.Radius)
'         End Sub ' New
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Creates a new Ytt.Util.Math.IntersectingCircleData.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Sub New(ByVal centerX1 As System.Double, ByVal centerY1 As System.Double, ByVal radius1 As System.Double,
'         ''' ByVal centerint2X As System.Double, ByVal centerY2 As System.Double, ByVal radius2 As System.Double)
'         ''' </declaration>
'         ''' <param name="centerX1">The X coordinate of the center of one circle.</param>
'         ''' <param name="centerY1">The Y coordinate of the center of one circle.</param>
'         ''' <param name="radius1">The radius of one circle. Cannot be negative.</param>
'         ''' <param name="centerint2X">The X coordinate of the center of the other circle.</param>
'         ''' <param name="centerY2">The Y coordinate of the center of the other circle.</param>
'         ''' <param name="radius2">The radius of the other circle. Cannot be negative.</param>
'         ''' <exception cref="System.ArgumentOutOfRangeException">
'         ''' Thrown when <paramref name="radius1"/> or <paramref name="radius1"/> is negative.
'         ''' </exception>
'         ''' <remarks></remarks>
'         Public Sub New(ByVal centerX1 As System.Double, ByVal centerY1 As System.Double, ByVal radius1 As System.Double,
'                        ByVal centerint2X As System.Double, ByVal centerY2 As System.Double, ByVal radius2 As System.Double)
' 
'             ' References:
'             ' http://paulbourke.net/geometry/circlesphere/
'             ' http://paulbourke.net/geometry/2circle/
'             ' http://paulbourke.net/geometry/2circle/tvoght.c
' 
'             ' Input checking.
'             If (radius1 < 0.0) Then
'                 Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                 Dim Ex = Ytt.Util.RunTime.NewValueCannotBeNegativeException(ProcName, radius1, "radius1")
'                 Ex.Source = ProcName
'                 Ex.Data.Add("centerX1", centerX1)
'                 Ex.Data.Add("centerY1", centerY1)
'                 Ex.Data.Add("radius1", radius1)
'                 Ex.Data.Add("centerint2X", centerint2X)
'                 Ex.Data.Add("centerY2", centerY2)
'                 Ex.Data.Add("radius2", radius2)
'                 Throw Ex
'             End If
'             If (radius2 < 0.0) Then
'                 Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                 Dim Ex = Ytt.Util.RunTime.NewValueCannotBeNegativeException(ProcName, radius2, "radius2")
'                 Ex.Source = ProcName
'                 Ex.Data.Add("centerX1", centerX1)
'                 Ex.Data.Add("centerY1", centerY1)
'                 Ex.Data.Add("radius1", radius1)
'                 Ex.Data.Add("centerint2X", centerint2X)
'                 Ex.Data.Add("centerY2", centerY2)
'                 Ex.Data.Add("radius2", radius2)
'                 Throw Ex
'             End If
' 
'             With Me
'                 .m_X1 = centerX1 : .m_Y1 = centerY1 : .m_R1 = radius1
'                 .m_int2X = centerint2X : .m_Y2 = centerY2 : .m_R2 = radius2
'             End With
' 
'             ' DeltaX and DeltaY are the vertical and horizontal distances between the circle centers.
'             Dim DeltaX = (m_int2X - m_X1)
'             Dim DeltaY = (m_Y2 - m_Y1)
' 
'             ' Determine the straight-line distance between the centers. 
'             Dim CenterSeparation = Ytt.Util.Math.Hypotenuse(DeltaX, DeltaY)
' 
'             ' Check for solvability.
'             Me.m_CirclesIntersect = True ' For now.
'             If ((Me.m_int2X = Me.m_X1) AndAlso (Me.m_Y2 = Me.m_Y1) AndAlso (Me.m_R2 = Me.m_R1)) Then
'                 ' They are both the same.
'                 Me.m_CirclesIntersect = False
'             ElseIf (CenterSeparation > (m_R1 + m_R2)) Then
'                 ' Two isolated circles.
'                 Me.m_CirclesIntersect = False
'             ElseIf (CenterSeparation < System.Math.Abs(m_R1 - m_R2)) Then
'                 ' One inside the other.
'                 Me.m_CirclesIntersect = False
'             End If
' 
'             If Me.m_CirclesIntersect Then
' 
' 
'                 ' "point 3" is the point where the line through the circle
'                 ' intersection points crosses the line between the circle
'                 ' centers.  
' 
'                 Dim A, H, Rx, Ry, X3, Y3 As System.Double
' 
'                 ' Determine the distance from point 1 to point 3. 
'                 A = ((m_R1 * m_R1) - (m_R2 * m_R2) + (CenterSeparation * CenterSeparation)) / (2.0 * CenterSeparation)
' 
'                 ' Determine the coordinates of point 3. 
'                 Dim OnceACS = (A / CenterSeparation)
'                 X3 = (m_X1 + (DeltaX * OnceACS))
'                 Y3 = (m_Y1 + (DeltaY * OnceACS))
' 
'                 ' Determine the distance from point 3 to either of the
'                 ' intersection points.
'                 H = System.Math.Sqrt((m_R1 * m_R1) - (A * A))
' 
'                 ' Now determine the offsets of the intersection points from point 3.
'                 Dim OnceHCS = (H / CenterSeparation)
'                 Rx = (-DeltaY * OnceHCS)
'                 Ry = (DeltaX * OnceHCS)
' 
'                 ' Determine the absolute intersection points. 
'                 m_Intersect1X = X3 + Rx
'                 m_Intersect2X = X3 - Rx
'                 m_Intersect1Y = Y3 + Ry
'                 m_Intersect2Y = Y3 - Ry
' 
'             End If ' Me.m_CirclesIntersect
' 
'         End Sub ' New
' 
'         ''' <summary>
'         ''' Returns <c>True</c> if the circles intersect. Otherwise <c>False</c>.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public ReadOnly Property CirclesIntersect As System.Boolean
'         ''' </declaration>
'         ''' <value><c>True</c> if the circles intersect. Otherwise <c>False</c>.</value>
'         ''' <remarks></remarks>
'         Public ReadOnly Property CirclesIntersect As System.Boolean
'             Get
'                 Return Me.m_CirclesIntersect
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The X coordinate of the first intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public ReadOnly Property Intersect1X As System.Double
'         ''' </declaration>
'         ''' <value>The X coordinate of the first intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect1X As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect1X
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The Y coordinate of the first intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' The Y coordinate of the first intersection.
'         ''' </declaration>
'         ''' <value>The Y coordinate of the first intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect1Y As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect1Y
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The X coordinate of the second intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' The X coordinate of the second intersection.
'         ''' </declaration>
'         ''' <value>The X coordinate of the second intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect2X As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect2X
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The Y coordinate of the second intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' The Y coordinate of the second intersection.
'         ''' </declaration>
'         ''' <value>The Y coordinate of the second intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect2Y As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect2Y
'             End Get
'         End Property
' 
'         Private Shared Function GetCheckFirstException(ByVal sourceProcName As System.String) As System.ApplicationException
'             Dim S1 = "Check the status of CirclesIntersect before reading the intersection points."
'             Dim S2 = "Circles do not intersect."
'             Return New System.ApplicationException(Ytt.Util.RunTime.FormattedExceptionString(S1, sourceProcName, S2))
'         End Function
' 
'         Private m_CirclesIntersect As System.Boolean
' 
'         ' Center and radius of 1st circle.
'         Private m_X1 As System.Double
'         Private m_Y1 As System.Double
'         Private m_R1 As System.Double
'         ' Center and radius of 2nd circle.
'         Private m_int2X As System.Double
'         Private m_Y2 As System.Double
'         Private m_R2 As System.Double
' 
'         ' 1st intersection point.
'         Private m_Intersect1X, m_Intersect1Y As System.Double
'         ' 2nd intersection point.
'         Private m_Intersect2X, m_Intersect2Y As System.Double
' 
'     End Structure ' IntersectingCircleData
' 
' End Structure ' Math
