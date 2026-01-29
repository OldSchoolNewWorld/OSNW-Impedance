Imports System.Reflection.Metadata
Imports System.Xml
Imports OSNW.Math

Partial Public Module Math

    ''' <summary>
    ''' A base class that represents the geometry of a generic circle, with a
    ''' center and radius, for use on a Cartesian grid. Dimensions are in
    ''' generic "units".
    ''' </summary>
    Public Class Circle2D

#Region "Fields and Properties"

        Private m_CenterX As System.Double
        ''' <summary>
        ''' Represents the X-coordinate of the center of the <c>Circle2D</c>, on
        ''' a Cartesian grid. Dimensions are in generic "units".
        ''' </summary>
        Public Property CenterX As System.Double
            Get
                Return Me.m_CenterX
            End Get
            Set(value As System.Double)
                Me.m_CenterX = value
            End Set
        End Property

        Private m_CenterY As System.Double
        ''' <summary>
        ''' Represents the Y-coordinate of the center of the <c>Circle2D</c>, on
        ''' a Cartesian grid. Dimensions are in generic "units".
        ''' </summary>
        Public Property CenterY As System.Double
            Get
                Return Me.m_CenterY
            End Get
            Set(value As System.Double)
                Me.m_CenterY = value
            End Set
        End Property

        Private m_Radius As System.Double
        ''' <summary>
        ''' Represents the radius of the <c>Circle2D</c>, on a Cartesian grid.
        ''' Dimensions are in generic "units".
        ''' </summary>
        Public Property Radius As System.Double
            Get
                Return Me.m_Radius
            End Get
            Set(value As System.Double)

                ' Input checking.
                ' A zero radius seems useless, but may be valid in some unusual
                ' case.
                If value < 0.0 Then
                    'Dim CaughtBy As System.Reflection.MethodBase =
                    '    System.Reflection.MethodBase.GetCurrentMethod
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(value), OSNW.Math.MSGCHNV)
                End If

                Me.m_Radius = value

            End Set
        End Property

        ''' <summary>
        ''' Represents the diameter of the <c>Circle2D</c>, on a Cartesian grid.
        ''' Dimensions are in generic "units".
        ''' </summary>
        Public Property Diameter As System.Double
            ' DEV: Being functionally redundant, this may need to be excluded
            ' from any serialization process.
            Get
                Return Me.Radius * 2.0
            End Get
            Set(value As System.Double)

                ' Input checking.
                ' A zero radius seems useless, but may be valid in some unusual
                ' case.
                If value < 0.0 Then
                    'Dim CaughtBy As System.Reflection.MethodBase =
                    '    System.Reflection.MethodBase.GetCurrentMethod
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(value), OSNW.Math.MSGCHNV)
                End If

                Me.Radius = value / 2.0

            End Set
        End Property

#End Region ' "Fields and Properties"

#Region "Constructors"

        ''' <summary>
        ''' A default constructor that creates a new instance of the
        ''' <c>Circle2D</c> class with default center coordinates and radius.
        ''' </summary>
        ''' <remarks>
        ''' A default constructor is required to allow inheritance.
        ''' </remarks>
        Public Sub New()
            With Me
                '.m_CenterX = 0.0
                '.m_CenterY = 0.0
                .m_Radius = 1.0 ' Default to a unit circle.
            End With
        End Sub ' New

        ''' <summary>
        ''' Creates a new instance of the <c>Circle2D</c> class with the
        ''' specified center coordinates and radius.
        ''' </summary>
        ''' <param name="centerX"> Specifies the X-coordinate of the center of
        ''' the <c>Circle2D</c>, on a Cartesian grid. Dimensions are in generic
        ''' "units".</param>
        ''' <param name="centerY"> Specifies the Y-coordinate of the center of
        ''' the <c>Circle2D</c>, on a Cartesian grid. Dimensions are in generic
        ''' "units".</param>
        ''' <param name="radius">Specifies the radius of the <c>Circle2D</c>, on
        ''' a Cartesian grid. Dimensions are in generic "units".</param>
        Public Sub New(ByVal centerX As System.Double,
                       ByVal centerY As System.Double,
                       ByVal radius As System.Double)

            ' Input checking.
            ' A zero radius seems useless, but may be valid in some unusual
            ' case.
            If radius < 0.0 Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                        NameOf(radius), OSNW.Math.MSGCHNV)
            End If

            With Me
                .m_CenterX = centerX
                .m_CenterY = centerY
                .m_Radius = radius
            End With

        End Sub ' New

        ''' <summary>
        ''' Creates a new instance of the <c>Circle2D</c> class with the
        ''' specified center coordinates and radius.
        ''' </summary>
        ''' <param name="center"> Specifies the center point of the
        ''' <c>Circle2D</c>, on a Cartesian grid. Dimensions are in generic
        ''' "units".</param>
        ''' <param name="radius">Specifies the radius of the <c>Circle2D</c>, on
        ''' a Cartesian grid. Dimensions are in generic "units".</param>
        Public Sub New(ByVal center As Point2D,
                       ByVal radius As System.Double)

            ' Input checking.
            ' A zero radius seems useless, but may be valid in some unusual
            ' case.
            If radius < 0.0 Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                        NameOf(radius), OSNW.Math.MSGCHNV)
            End If

            With Me
                .m_CenterX = center.X
                .m_CenterY = center.Y
                .m_Radius = radius
            End With

        End Sub ' New

#End Region ' "Constructors"

#Region "Methods"

        '''' Suspended XML comments for suspended code:
        '''' <exception cref="System.ArgumentOutOfRangeException">
        '''' Thrown when <paramref name="circleX"/>, <paramref name="circleY"/>,
        '''' <paramref name="circleR"/>, <paramref name="lineM"/>, or
        '''' <paramref name="lineB"/> is infinite.
        '''' </exception>
        '''' <exception cref="System.ArgumentOutOfRangeException"> Thrown when
        '''' <paramref name="circleR"/> is less than or equal to zero.
        '''' </exception>
        ''' <summary>
        ''' Attempts to solve where a line intersects a circle, given the center
        ''' coordinates and radius of the circle, along with the slope and
        ''' Y-intercept of the line.
        ''' </summary>
        ''' <param name="circleX">Specifies the X-coordinate of the center of
        ''' the circle.</param>
        ''' <param name="circleY">Specifies the Y-coordinate of the center of
        ''' the circle.</param>
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
        ''' <returns><c>True</c> if the process succeeds; otherwise,
        ''' <c>False</c>.
        ''' When valid, also returns the results in
        ''' <paramref name="intersect1X"/>, <paramref name="intersect1Y"/>,
        ''' <paramref name="intersect2X"/>, and
        ''' <paramref name="intersect2Y"/>.</returns>
        ''' <remarks>
        ''' A vertical line (infinite slope) will not have a Y-intercept, except
        ''' when that line passes through the circle center, a case which would
        ''' have infinite common points.
        ''' <br/>
        ''' To avoid throwing an exception, <c>False</c> is returned
        ''' when <paramref name="circleX"/>, <paramref name="circleY"/>,
        ''' <paramref name="circleR"/>, <paramref name="lineM"/>, or
        ''' <paramref name="lineB"/> is infinite,
        ''' or
        ''' when <paramref name="circleR"/> is less than or equal to zero.
        ''' </remarks>
        Public Shared Function TryCircleLineIntersections(
            ByVal circleX As System.Double, ByVal circleY As System.Double,
            ByVal circleR As System.Double, ByVal lineM As System.Double,
            ByVal lineB As System.Double, ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double,
            ByRef intersect2X As System.Double,
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
            '                $"Arguments to {NameOf(TryCircleLineIntersections) {MSGCHIV")
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
            Dim c As System.Double = circleX ^ 2 +
                lineB * (lineB - 2 * circleY) + circleY ^ 2 - circleR ^ 2
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

        End Function ' TryCircleLineIntersections

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
        ''' coordinates and radius of the circle, along with the coordinates of
        ''' two points on the line.
        ''' </summary>
        ''' <param name="circleX">Specifies the X-coordinate of the center of
        ''' the circle.</param>
        ''' <param name="circleY">Specifies the Y-coordinate of the center of
        ''' the circle.</param>
        ''' <param name="circleR">Specifies the radius of the circle.</param>
        ''' <param name="lineX1">Specifies the X-coordinate of the first point
        ''' on the line.</param>
        ''' <param name="lineY1">Specifies the Y-coordinate of the first point
        ''' on the line.</param>
        ''' <param name="lineX2">Specifies the X-coordinate of the second point
        ''' on the line.</param>
        ''' <param name="lineY2">Specifies the Y-coordinate of the second point
        ''' on the line.</param>
        ''' <param name="intersect1X">Specifies the X-coordinate of one
        ''' intersection.</param>
        ''' <param name="intersect1Y">Specifies the Y-coordinate of one
        ''' intersection.</param>
        ''' <param name="intersect2X">Specifies the X-coordinate of the other
        ''' intersection.</param>
        ''' <param name="intersect2Y">Specifies the Y-coordinate of the other
        ''' intersection.</param>
        ''' <returns>
        ''' <c>True</c> if the process succeeds; otherwise, <c>False</c>. When
        ''' valid, also returns the results in <paramref name="intersect1X"/>,
        ''' <paramref name="intersect1Y"/>, <paramref name="intersect2X"/>, and
        ''' <paramref name="intersect2Y"/>.
        ''' </returns>
        ''' <remarks>
        ''' To avoid throwing an exception, <c>False</c> is returned
        ''' when <paramref name="circleX"/>, <paramref name="circleY"/>,
        ''' <paramref name="circleR"/>, <paramref name="lineX1"/>,
        ''' or <paramref name="lineY1"/>, or <paramref name="lineY2"/> is
        ''' infinite,
        ''' or
        ''' when <paramref name="circleR"/> is less than or equal to zero.
        ''' </remarks>
        Public Shared Function TryCircleLineIntersections(
            ByVal circleX As System.Double,
            ByVal circleY As System.Double, ByVal circleR As System.Double,
            ByVal lineX1 As System.Double, ByVal lineY1 As System.Double,
            ByVal lineX2 As System.Double, ByVal lineY2 As System.Double,
            ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double,
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
            '               $"Arguments to {NameOf(TryCircleLineIntersections) {MSGCHIV")
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

            Return TryCircleLineIntersections(circleX, circleY, circleR, lineM,
                lineB, intersect1X, intersect1Y, intersect2X, intersect2Y)

        End Function ' TryCircleLineIntersections

        ''' <summary>
        ''' Determines whether two circles intersect, given their center
        ''' coordinates and radii.
        ''' </summary>
        ''' <param name="x1">Specifies the X-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="y1">Specifies the Y-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="r1">Specifies the radius of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="x2">Specifies the X-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="y2">Specifies the Y-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="r2">Specifies the radius of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <returns><c>True</c> if the circles intersect; otherwise,
        ''' <c>False</c>.</returns>
        ''' <remarks>
        ''' Tangent circles will have only one intersection. Concentric circles
        ''' will have either zero or infinite common points. The second case is
        ''' considered not to be intersecting. A negative radius will return
        ''' <c>False</c>, to avoid an exception.
        ''' </remarks>
        Public Shared Function CirclesIntersect(ByVal x1 As System.Double,
            ByVal y1 As System.Double, ByVal r1 As System.Double,
            ByVal x2 As System.Double, ByVal y2 As System.Double,
            ByVal r2 As System.Double) As System.Boolean

            ' Input checking.
            If (r1 < 0.0) OrElse (r2 < 0.0) Then
                Return False ' To avoid an exception.
            End If

            ' Check for solvability.
            Dim CtrSeparation As System.Double =
            System.Double.Hypot(x2 - x1, y2 - y1)
            If CtrSeparation > (r1 + r2) Then
                ' Two isolated circles.
                Return False
            ElseIf CtrSeparation < System.Math.Abs(r2 - r1) Then
                ' One inside the other.
                Return False
            ElseIf x2.Equals(x1) AndAlso y2.Equals(y1) Then
                ' They are concentric, with either zero or infinite common points.
                ' The second case is considered not to be intersecting.
                Return False
            End If
            Return True

        End Function ' CirclesIntersect

        ''' <summary>
        ''' Determines whether two circles intersect.
        ''' </summary>
        ''' <param name="circle1">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle2"/>.</param>
        ''' <param name="circle2">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle1"/>.</param>
        ''' <returns><c>True</c> if the circles intersect; otherwise,
        ''' <c>False</c>.</returns>
        Public Shared Function CirclesIntersect(ByVal circle1 As Circle2D,
                ByVal circle2 As Circle2D) As System.Boolean

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx
            Return CirclesIntersect(
                circle1.CenterX, circle1.CenterY, circle1.Radius,
                circle2.CenterX, circle2.CenterY, circle2.Radius)
        End Function ' CirclesIntersect

        ''' <summary>
        ''' Calculates the intersection points of two circles defined by their
        ''' center coordinates and radii.
        ''' </summary>
        ''' <param name="c1X">Specifies the X-coordinate of circle1.</param>
        ''' <param name="c1Y">Specifies the Y-coordinate of circle1.</param>
        ''' <param name="c1R">Specifies the radius of circle1.</param>
        ''' <param name="c2X">Specifies the X-coordinate of circle2.</param>
        ''' <param name="c2Y">Specifies the Y-coordinate of circle2.</param>
        ''' <param name="c2R">Specifies the radius of circle1.</param>
        ''' <returns>A list of 0, 1, or 2 intersection points as
        ''' <see cref="OSNW.Math.Point2D"/> structure(s).</returns>
        ''' <exception cref="ArgumentOutOfRangeException">when either radius is
        ''' less than or equal to zero.</exception>
        ''' <remarks>
        ''' If there are no intersection points, an empty list is returned. If
        ''' the circles are tangent to each other, a list with one intersection
        ''' point is returned. If the circles intersect at two points, a list
        ''' with both points is returned.
        ''' </remarks>
        Public Shared Function GetIntersections(ByVal c1X As System.Double,
                ByVal c1Y As System.Double, ByVal c1R As System.Double,
                ByVal c2X As System.Double, ByVal c2Y As System.Double,
                ByVal c2R As System.Double) _
                As System.Collections.Generic.List(Of Point2D)

            ' DEV: This is the worker for the related routines.

            ' Input checking.
            If c1R <= 0 OrElse c2R <= 0 Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Dim ErrMsg As System.String = String.Format(
                    "{0}={1}, {2}={3}", NameOf(c1R), c1R, NameOf(c2R), c2R)
                Throw New System.ArgumentOutOfRangeException(
                    ErrMsg, OSNW.Math.MSGVMBGTZ)
            End If

            Dim Intersections _
                As New System.Collections.Generic.List(Of Point2D)

            ' Concentric circles would have either zero or infinite intersecting
            ' points.
            If OSNW.Math.EqualEnough(
                    c1X, c2X, OSNW.Math.DFLTGRAPHICTOLERANCE) AndAlso
                OSNW.Math.EqualEnough(
                    c1Y, c2Y, OSNW.Math.DFLTGRAPHICTOLERANCE) Then

                Return Intersections ' Still empty.
            End If

            ' Calculate the distance between the centers of the circles.
            Dim DeltaX As System.Double = c2X - c1X
            Dim DeltaY As System.Double = c2Y - c1Y
            Dim DeltaCtr As System.Double =
                System.Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY)

            ' Check if circles are too far apart or if one is contained within,
            ' but not tangent to, the other.
            If DeltaCtr > (c1R + c2R) OrElse
                DeltaCtr < System.Math.Abs(c1R - c2R) Then

                Return Intersections ' Still empty.
            End If

            ' On getting this far, the circles are neither isolated nor have one
            ' separately contained within the other. There should now be either
            ' one or two intersections.

            ' Check if the circles are outside-tangent to each other.
            If OSNW.Math.EqualEnough(c1R + c2R, DeltaCtr,
                                     OSNW.Math.DFLTGRAPHICTOLERANCE) Then
                ' One intersection point.
                Dim C1Frac As System.Double = c1R / DeltaCtr
                Intersections.Add(New Point2D(
                                  c1X + C1Frac * DeltaX, c1Y + C1Frac * DeltaY))
                Return Intersections
            End If

            ' Check if the circles are inside-tangent to each other.
            ' Two circles of the same radius cannot be inside-tangent to each
            ' other.
            If Not OSNW.Math.EqualEnough(
                c1R, c2R, OSNW.Math.DFLTGRAPHICTOLERANCE) Then

                If OSNW.Math.EqualEnough(System.Math.Abs(c1R - c2R), DeltaCtr,
                                         OSNW.Math.DFLTGRAPHICTOLERANCE) Then
                    ' They are inside-tangent.
                    If c1R > c2R Then
                        Dim C1Frac As System.Double = c1R / DeltaCtr
                        Intersections.Add(New Point2D(
                                              c1X + (C1Frac * DeltaX),
                                              c1Y + (C1Frac * DeltaY)))
                        Return Intersections
                    Else
                        Dim C2Frac As System.Double = c2R / DeltaCtr
                        Intersections.Add(New Point2D(
                                              c2X + (C2Frac * -DeltaX),
                                              c2Y + (C2Frac * -DeltaY)))
                        Return Intersections
                    End If
                End If
            End If

            ' (The initial version of) the sequence below was generated by
            ' Visual Studio AI.

            ' Calculate two intersection points.
            Dim OnceA As System.Double =
                (c1R * c1R - c2R * c2R + DeltaCtr * DeltaCtr) / (2 * DeltaCtr)
            Dim OnceH As System.Double = System.Math.Sqrt(c1R * c1R - OnceA * OnceA)
            Dim X0 As System.Double = c1X + OnceA * (DeltaX / DeltaCtr)
            Dim Y0 As System.Double = c1Y + OnceA * (DeltaY / DeltaCtr)

            ' Two intersection points.
            Dim intersection1 As New Point2D(
                X0 + OnceH * (DeltaY / DeltaCtr),
                Y0 - OnceH * (DeltaX / DeltaCtr))
            Dim intersection2 As New Point2D(
                X0 - OnceH * (DeltaY / DeltaCtr),
                Y0 + OnceH * (DeltaX / DeltaCtr))
            Intersections.Add(intersection1)
            Intersections.Add(intersection2)
            Return Intersections

        End Function ' GetIntersections

        ''' <summary>
        ''' Calculates the intersection points between <paramref name="circle1"/>
        ''' and <paramref name="circle2"/>.
        ''' </summary>
        ''' <param name="circle1">Specifies the first circle.</param>
        ''' <param name="circle2">Specifies the second circle.</param>
        ''' <returns>A list of intersection points as
        ''' <see cref="OSNW.Math.Point2D"/> objects.</returns>
        Public Shared Function GetIntersections(
            ByVal circle1 As Circle2D, ByVal circle2 As Circle2D) _
            As System.Collections.Generic.List(Of Point2D)

            Return GetIntersections(
                circle1.CenterX, circle1.CenterY, circle1.Radius,
                circle2.CenterX, circle2.CenterY, circle2.Radius)
        End Function ' GetIntersections

        ''' <summary>
        ''' Calculates the intersection points between the current instance and
        ''' <paramref name="otherCircle"/>.
        ''' </summary>
        ''' <param name="otherCircle">Specifies the other circle with which to find
        ''' intersections.</param>
        ''' <returns>A list of intersection points as
        ''' <see cref="OSNW.Math.Point2D"/> objects.</returns>
        Public Function GetIntersections(ByVal otherCircle As Circle2D) _
            As System.Collections.Generic.List(Of Point2D)

            Return GetIntersections(Me, otherCircle)
        End Function ' GetIntersections

        ''' <summary>
        ''' Attempts to determine where two circles intersect, given their
        ''' center coordinates and radii.
        ''' </summary>
        ''' <param name="circle1X">Specifies the X-coordinate of the first
        ''' circle to consider for intersection with the second circle.</param>
        ''' <param name="circle1Y">Specifies the Y-coordinate of the first
        ''' circle to consider for intersection with the second circle.</param>
        ''' <param name="circle1R">Specifies the radius of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="circle2X">Specifies the X-coordinate of the second
        ''' circle to consider for intersection with the first circle.</param>
        ''' <param name="circle2Y">Specifies the Y-coordinate of the second
        ''' circle to consider for intersection with the first circle.</param>
        ''' <param name="circle2R">Specifies the radius of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="intersect1X">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect1Y">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect2X">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <param name="intersect2Y">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <returns><c>True</c> if the intersections are found;
        ''' otherwise, <c>False</c>.
        ''' When valid, also returns the results in
        ''' <paramref name="intersect1X"/>, <paramref name="intersect1Y"/>,
        ''' <paramref name="intersect2X"/>, and
        ''' <paramref name="intersect2Y"/>.</returns>
        ''' <remarks>
        ''' A negative radius, or circles that do not intersect, will return
        ''' <c>False</c>, to avoid an exception.
        ''' Concentric circles will have either zero or infinite common points;
        ''' the second case is considered to NOT be intersecting.
        ''' Tangent circles will have two identical (or nearly so) intersections.
        ''' </remarks>
        Public Shared Function TryCircleCircleIntersections(
            ByVal circle1X As System.Double, ByVal circle1Y As System.Double,
            ByVal circle1R As System.Double, ByVal circle2X As System.Double,
            ByVal circle2Y As System.Double, ByVal circle2R As System.Double,
            ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double,
            ByRef intersect2X As System.Double,
            ByRef intersect2Y As System.Double) _
            As System.Boolean

            ' Input checking.
            ' A zero radius seems useless, but may be valid in some unusual
            ' case.
            ' Non-intersecting circles fail.
            ' Concentric circles (same center), will have either zero or
            ' infinite common points; the second case is considered to not be
            ' intersecting.
            If circle1R < 0.0 OrElse circle2R < 0.0 _
                OrElse Not CirclesIntersect(circle1X, circle1Y, circle1R,
                                            circle2X, circle2Y, circle2R) _
                OrElse circle2X.Equals(circle1X) _
                       AndAlso circle2Y.Equals(circle1Y) Then

                intersect1X = Double.NaN
                intersect1Y = Double.NaN
                intersect2X = Double.NaN
                intersect2Y = Double.NaN
                Return False 'To avoid an exception.
            End If

            ' THESE ARE TEMP STUFF TO ALLOW IN-PROGRESS TESTING.
            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            Const VERIFYTOLERANCE As System.Double = 0.001
            Const VERIFYTOLERANCE0 As System.Double = 0.001
            ' Any values sent as a predicted result indicate a request for
            ' in-line tests.
            Dim TestMode As System.Boolean = Not (
                intersect1X.Equals(0.0) AndAlso intersect1Y.Equals(0.0) AndAlso
                intersect2X.Equals(0.0) AndAlso intersect2Y.Equals(0.0))
            ' Variables to allow the derivations to be verified.
            Dim H1 As System.Double = circle1X
            Dim H2 As System.Double = circle2X
            Dim K1 As System.Double = circle1Y
            Dim K2 As System.Double = circle2Y
            Dim R1 As System.Double = circle1R
            Dim R2 As System.Double = circle2R
            Dim H12 As System.Double = H1 * H1
            Dim H22 As System.Double = H2 * H2
            Dim K12 As System.Double = K1 * K1
            Dim K22 As System.Double = K2 * K2
            Dim R12 As System.Double = R1 * R1
            Dim R22 As System.Double = R2 * R2
            ' Set X and Y to match expected valid results.
            Dim X As System.Double = intersect1X ' Expected result.
            Dim Y As System.Double = intersect1Y ' Expected result.
            Dim Left As System.Double ' Interim verification value.
            Dim Right As System.Double ' Interim verification value.
            ' END OF TESTING VALUES.

            ' Use these to substitute parameter names for clarity, squaring, and
            ' reuse.
            Dim circle1X2 As System.Double = circle1X * circle1X
            Dim circle2X2 As System.Double = circle2X * circle2X
            Dim circle1Y2 As System.Double = circle1Y * circle1Y
            Dim circle2Y2 As System.Double = circle2Y * circle2Y
            Dim circle1R2 As System.Double = circle1R * circle1R
            Dim circle2R2 As System.Double = circle2R * circle2R
            Dim DiffX As System.Double
            Dim DiffY As System.Double
            Dim SumRXY As System.Double

            If circle2Y.Equals(circle1Y) Then
                ' Special case. The circles share the center Y-coordinate.

                ' The derivation follows:
                ' DEV: Square brackets, braces, and split lines are used below
                ' for visual clarity across the various steps. "h", "k", and
                ' squared values are carried through the derivation, in keeping
                ' with the standard form. The actual parameters, and
                ' multiplication vs. squaring, are substituted in the
                ' implementation.

                ' Standard form of a circle.
                ' [(X - h)^2] + [(Y - k)^2] = r^2
                ' Localize parameters, for one generalized (X, Y) intersection.
                ' [(X - h1)^2] + [(Y - k1)^2] = r1^2
                ' [(X - h2)^2] + [(Y - k2)^2] = r2^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = ((X - H1) ^ 2) + ((Y - K1) ^ 2)
                    Right = R1 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                    Left = ((X - H2) ^ 2) + ((Y - K2) ^ 2)
                    Right = R2 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Since k2=k1, in this special case.
                ' [(X - h1)^2] + [(Y - k1)^2] = r1^2
                ' [(X - h2)^2] + [(Y - k1)^2] = r2^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = ((X - H1) ^ 2) + ((Y - K1) ^ 2)
                    Right = R1 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                    Left = ((X - H2) ^ 2) + ((Y - K1) ^ 2)
                    Right = R2 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Subtract the second equation from the first.
                ' [(X - h1)^2] + [(Y - k1)^2]
                ' - [(X - h2)^2] - [(Y - k1)^2]
                ' = r1^2 - r2^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = ((X - H1) ^ 2) + ((Y - K1) ^ 2) _
                        - ((X - H2) ^ 2) - ((Y - K1) ^ 2)
                    Right = R1 ^ 2 - R2 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Handle the cancellation.
                ' [(X - h1)^2] - [(X - h2)^2]
                ' = r1^2 - r2^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = ((X - H1) ^ 2) - ((X - H2) ^ 2)
                    Right = R1 ^ 2 - R2 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Expand the squares.
                ' [X^2 - (2*h1*X) + h1^2]
                ' - [X^2 - (2*h2*X) + h2^2]
                ' = r1^2 - r2^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = (X ^ 2 - (2 * H1 * X) + H1 ^ 2) _
                        - (X ^ 2 - (2 * H2 * X) + H2 ^ 2)
                    Right = R1 ^ 2 - R2 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Handle negation.
                ' X^2 - (2*h1*X) + h1^2
                ' - X^2 + (2*h2*X) - h2^2
                ' = r1^2 - r2^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = X ^ 2 - (2 * H1 * X) + H1 ^ 2 _
                        - X ^ 2 + (2 * H2 * X) - H2 ^ 2
                    Right = R1 ^ 2 - R2 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Handle the cancellation.
                ' - (2*h1*X) + h1^2
                ' + (2*h2*X) - h2^2
                ' = r1^2 - r2^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = -(2 * H1 * X) + H1 ^ 2 _
                        + (2 * H2 * X) - H2 ^ 2
                    Right = R1 ^ 2 - R2 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Gather like terms. Extract the common 2 and X.
                ' 2*(h2 - h1)*X
                ' = r1^2 - r2^2 + h2^2 - h1^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = 2 * (H2 - H1) * X
                    Right = R1 ^ 2 - R2 ^ 2 + H2 ^ 2 - H1 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Solve for X.
                ' *** This is part of the solution for both intersections. ***
                ' X = (r1^2 - r2^2 + h2^2 - h1^2) / (2*(h2 - h1))

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = X
                    Right = (R1 ^ 2 - R2 ^ 2 + H2 ^ 2 - H1 ^ 2) / (2 * (H2 - H1))
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Solve the standard form of a circle for Y, for one circle.
                ' [(X - h)^2] + [(Y - k)^2] = r^2
                ' [(X - h1)^2] + [(Y - k1)^2] = r1^2

                ' Expand the squares.
                ' (X^2 -2*h1*X + h1^2) + (Y^2 -2*k1*Y + k1^2) = r1^2
                ' X^2 -2*h1*X + h1^2 + Y^2 -2*k1*Y + k1^2 = r1^2

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = X ^ 2 - 2 * H1 * X + H1 ^ 2 + Y ^ 2 - 2 * K1 * Y + K1 ^ 2
                    Right = R1 ^ 2
                    If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                        Return False
                    End If
                End If

                ' Arrange for the quadratic formula. X, having been solved
                ' above, may now be treated as one of the constants.
                ' Y^2
                ' - 2*k1*Y
                ' + X^2 - 2*h1*X + h1^2 + k1^2 - r1^2
                ' = 0

                ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
                ' ARE GOOD.
                If TestMode Then
                    Left = Y ^ 2 - 2 * K1 * Y + X ^ 2 - 2 * H1 * X + H1 ^ 2 + K1 ^ 2 - R1 ^ 2
                    If Not OSNW.Math.EqualEnoughZero(Left, VERIFYTOLERANCE0 * circle1R) Then
                        Return False
                    End If
                End If

                ' Set up for the quadratic formula; then use that to get the two
                ' Y-coordinates, still treating X as one of the constants.
                ' a = 1
                ' b = -2*k1
                ' c = X^2 -2*h1*X + h1^2 + k1^2 - r1^2

                ' Implementation:

                intersect1X = (circle1R2 - circle2R2 + circle2X2 - circle1X2) _
                              / (2 * (circle2X - circle1X))
                Dim intersect1x2 As System.Double = intersect1X * intersect1X

                Dim a As System.Double = 1
                Dim b As System.Double = -2 * circle1Y
                Dim c As System.Double =
                    intersect1x2 - 2 * circle1X * intersect1X _
                    + circle1X2 + circle1Y2 - circle1R2
                If Not TryQuadratic(a, b, c, intersect1Y, intersect2Y) Then
                    intersect1X = System.Double.NaN
                    intersect1Y = System.Double.NaN
                    intersect2X = System.Double.NaN
                    intersect2Y = System.Double.NaN
                    Return False
                End If

                ' On getting here, the intersection points have been found.
                intersect2X = intersect1X
                Return True

            End If

            ' On getting here, the circles do not share the center Y-coordinate.
            ' The derivation follows:
            ' DEV: Square brackets, braces, and split lines are used below for
            ' visual clarity across the various steps. "h", "k", and squared
            ' values are carried through the derivation, in keeping with the
            ' standard form; the actual parameters, and multiplication vs.
            ' squaring, are substituted in the implementation.

            ' REF: How can I find the points at which two circles intersect?
            ' https://math.stackexchange.com/questions/256100/how-can-i-find-the-points-at-which-two-circles-intersect

            ' Standard form of a circle.
            ' [(X - h)^2] + [(Y - k)^2] = r^2

            ' Localize parameters, for a generic point (X, Y) of intersection.
            ' [(X - h1)^2] + [(Y - k1)^2] = r1^2
            ' [(X - h2)^2] + [(Y - k2)^2] = r2^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = ((X - H1) ^ 2) + ((Y - K1) ^ 2)
                Right = R1 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
                Left = ((X - H2) ^ 2) + ((Y - K2) ^ 2)
                Right = R2 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Subtract the second equation from the first.
            ' [(X - h1)^2] + [(Y - k1)^2]
            ' - {[(X - h2)^2] + [(Y - k2)^2]}
            ' = r1^2 - r2^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = ((X - H1) ^ 2) + ((Y - K1) ^ 2) _
                    - (((X - H2) ^ 2) + ((Y - K2) ^ 2))
                Right = R1 ^ 2 - R2 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Handle negations.
            ' [(X - h1)^2] + [(Y - k1)^2]
            ' - [(X - h2)^2] - [(Y - k2)^2]
            ' = r1^2 - r2^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = ((X - H1) ^ 2) + ((Y - K1) ^ 2) _
                    - ((X - H2) ^ 2) - ((Y - K2) ^ 2)
                Right = R1 ^ 2 - R2 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Expand the squares.
            ' [X^2 - (2*h1*X) + h1^2]
            ' + [Y^2 - (2*k1*Y) + k1^2]
            ' - [X^2 - (2*h2*X) + h2^2]
            ' - [Y^2 - (2*k2*Y) + k2^2]
            ' = r1^2 - r2^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = (X ^ 2 - (2 * H1 * X) + H1 ^ 2) _
                    + (Y ^ 2 - (2 * K1 * Y) + K1 ^ 2) _
                    - (X ^ 2 - (2 * H2 * X) + H2 ^ 2) _
                    - (Y ^ 2 - (2 * K2 * Y) + K2 ^ 2)
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Simplify and rearrange, in steps:

            ' Handle negations.
            ' X^2 - (2*h1*X) + h1^2
            ' + Y^2 - (2*k1*Y) + k1^2
            ' - X^2 + (2*h2*X) - h2^2
            ' - Y^2 + (2*k2*Y) - k2^2
            ' = r1^2 - r2^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = X ^ 2 - (2 * H1 * X) + H1 ^ 2 _
                    + Y ^ 2 - (2 * K1 * Y) + K1 ^ 2 _
                    - X ^ 2 + (2 * H2 * X) - H2 ^ 2 _
                    - Y ^ 2 + (2 * K2 * Y) - K2 ^ 2
                Right = R1 ^ 2 - R2 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Handle the cancellations.
            ' - (2*h1*X) + h1^2
            ' - (2*k1*Y) + k1^2
            ' + (2*h2*X) - h2^2
            ' + (2*k2*Y) - k2^2
            ' = r1^2 - r2^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = -(2 * H1 * X) + H1 ^ 2 _
                    - (2 * K1 * Y) + K1 ^ 2 _
                    + (2 * H2 * X) - H2 ^ 2 _
                 + (2 * K2 * Y) - K2 ^ 2
                Right = R1 ^ 2 - R2 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Gather like terms.
            ' (2*h2*X) - (2*h1*X)
            ' + (2*k2*Y) - (2*k1*Y)
            ' = r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = (2 * H2 * X) - (2 * H1 * X) _
                    + (2 * K2 * Y) - (2 * K1 * Y)
                Right = R1 ^ 2 - R2 ^ 2 + H2 ^ 2 - H1 ^ 2 + K2 ^ 2 - K1 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Extract X and Y terms. Extract common 2s. Those arrange a standard
            ' "aX + bY = c" form of a linear equation.
            ' 2*(h2 - h1)*X
            ' + 2*(k2 - k1)*Y
            ' = r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = 2 * (H2 - H1) * X _
                    + 2 * (K2 - K1) * Y
                Right = R1 ^ 2 - R2 ^ 2 + H2 ^ 2 - H1 ^ 2 + K2 ^ 2 - K1 ^ 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Division by 2 yields the "aX + bY = c" standard form of a linear
            ' equation.
            ' (h2 - h1)*X + (k2 - k1)*Y
            ' = (r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2)/2

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = (H2 - H1) * X + (K2 - K1) * Y
                Right = (R1 ^ 2 - R2 ^ 2 + H2 ^ 2 - H1 ^ 2 + K2 ^ 2 - K1 ^ 2) / 2
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Arrange the "aX + bY = c" standard form of a linear equation into
            ' the "Y = mX + b" slope-intercept form of a line.
            ' Y = {[(r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2)/2] - [(h2 - h1)*X]}
            '     /
            '     (k2 - k1)
            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            If TestMode Then
                Left = Y
                Right = (((R1 ^ 2 - R2 ^ 2 + H2 ^ 2 - H1 ^ 2 + K2 ^ 2 - K1 ^ 2) / 2) - ((H2 - H1) * X)) _
                     / (K2 - K1)
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If

            ' Substitute parameter names for clarity, squaring, and reuse.
            ' DiffH = h2 - h1
            ' DiffK = k2 - k1
            ' SumRHK = r12 - r22 + h22 - h12 + k22 - k12

            ' Y = ((SumRHK/2) - (DiffH*X)) / DiffK

            ' THESE TESTS CAN BE REMOVED AFTER All IN-PROGRESS VERIFICATIONS
            ' ARE GOOD.
            Dim DiffH As System.Double = H2 - H1
            Dim DiffK As System.Double = K2 - K1
            Dim SumRHK As System.Double = R12 - R22 + H22 - H12 + K22 - K12
            If TestMode Then
                Left = Y
                Right = ((SumRHK / 2) - (DiffH * X)) / DiffK
                If Not OSNW.Math.EqualEnough(Left, Right, VERIFYTOLERANCE) Then
                    Return False
                End If
            End If
            ' REWORKED TO HERE.

            ' Implementation:

            ' Substitute parameter names for clarity, squaring, and reuse.
            DiffX = circle2X - circle1X
            DiffY = circle2Y - circle1Y
            SumRXY = circle1R2 - circle2R2 + circle2X2 - circle1X2 _
                + circle2Y2 - circle1Y2
            Dim LineM As System.Double = -DiffX / DiffY
            Dim LineB As System.Double = SumRXY / (2 * DiffY)
            Return TryCircleLineIntersections(
                circle1X, circle1Y, circle1R, LineM, LineB,
                intersect1X, intersect1Y, intersect2X, intersect2Y)

        End Function ' TryCircleCircleIntersections

        ''' <summary>
        ''' Attempts to determine where two circles intersect.
        ''' </summary>
        ''' <param name="circle1">Specifies the first circle to consider for
        ''' intersection with the second circle.</param>
        ''' <param name="circle2">Specifies the second circle to consider
        ''' for intersection with the first circle.</param>
        ''' <param name="intersect1X">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect1Y">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect2X">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <param name="intersect2Y">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <returns><c>True</c> if the intersections are found;
        ''' otherwise, <c>False</c>.
        ''' When valid, also returns the results in
        ''' <paramref name="intersect1X"/>, <paramref name="intersect1Y"/>,
        ''' <paramref name="intersect2X"/>, and
        ''' <paramref name="intersect2Y"/>.</returns>
        ''' <remarks>
        ''' Concentric circles will have either zero or infinite common points;
        ''' the second case is considered to NOT be intersecting.
        ''' Tangent circles will have two identical (or nearly so) intersections.
        ''' </remarks>
        Public Shared Function TryCircleCircleIntersections(
            ByVal circle1 As OSNW.Circle2D, ByVal circle2 As OSNW.Circle2D,
            ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double,
            ByRef intersect2X As System.Double,
            ByRef intersect2Y As System.Double) _
            As System.Boolean

            ' No input checking. circle1 and circle2 are presumed to have been
            ' checked when created.

            Return TryCircleCircleIntersections(circle1.CenterX,
                circle1.CenterY, circle1.Radius, circle2.CenterX,
                circle2.CenterY, circle2.Radius, intersect1X, intersect1Y, intersect2X, intersect2Y)

        End Function ' TryCircleCircleIntersections

        ''' <summary>
        ''' Converts the value of the current Circle2D to its equivalent string
        ''' representation in Cartesian form, using the default numeric format and
        ''' culture-specific format information for its parts.
        ''' </summary>
        ''' <returns>The current Circle2D expressed in Cartesian form.</returns>
        Public Overrides Function ToString() As System.String
            Return $"Center: ({Me.CenterX}, {Me.CenterY}), Radius: {Me.Radius}"
        End Function ' ToString

#End Region ' "Methods"

    End Class ' Circle2D

End Module ' Math
