Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates

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
                ' A zero value is useless, but possibly valid.
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
                ' A zero value is useless, but possibly valid.
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
            ' A zero value is useless, but possibly valid.
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
            ' A zero value is useless, but possibly valid.
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
        Public Shared Function TryCircleLineIntersection(
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
            '                $"Arguments to {NameOf(TryCircleLineIntersection) {MSGCHIV")
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
        Public Shared Function TryCircleLineIntersection(
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
            '               $"Arguments to {NameOf(TryCircleLineIntersection) {MSGCHIV")
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
        ''' xxxxxxxxxx
        ''' </summary>
        ''' <param name="x1">xxxxxxxxxx</param>
        ''' <param name="y1">xxxxxxxxxx</param>
        ''' <param name="r1">xxxxxxxxxx</param>
        ''' <param name="x2">xxxxxxxxxx</param>
        ''' <param name="y2">xxxxxxxxxx</param>
        ''' <param name="r2">xxxxxxxxxx</param>
        ''' <param name="i1x">xxxxxxxxxx</param>
        ''' <param name="i1y">xxxxxxxxxx</param>
        ''' <param name="i2x">xxxxxxxxxx</param>
        ''' <param name="i2y">xxxxxxxxxx</param>
        ''' <returns>xxxxxxxxxx</returns>
        ''' <remarks> A negative radius will return <c>False</c>, to avoid an
        ''' exception.</remarks>
        Public Shared Function TryCirclesIntersection(
            ByVal x1 As System.Double, ByVal y1 As System.Double,
            ByVal r1 As System.Double, ByVal x2 As System.Double,
            ByVal y2 As System.Double, ByVal r2 As System.Double,
            ByRef i1x As System.Double, ByRef i1y As System.Double,
            ByRef i2x As System.Double, ByRef i2y As System.Double) _
            As System.Boolean

            ' Input checking.
            ' A zero value is useless, but possibly valid.
            If r1 < 0.0 OrElse r2 < 0.0 Then
                i1x = Double.NaN
                i1y = Double.NaN
                i2x = Double.NaN
                i2y = Double.NaN
                Return False 'To avoid an exception.
            End If

            If Not CirclesIntersect(x1, y1, r1, x2, y2, r2) Then
                i1x = Double.NaN
                i1y = Double.NaN
                i2x = Double.NaN
                i2y = Double.NaN
                Return False
            End If

            ' The derivation follows:

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            '' USE KNOWN VALUES TO TEST THIS ROUTINE BEFORE COMPLETING IT.
            'Dim Factor As System.Double = 0.01
            'Dim h1 As System.Double = x1
            'Dim k1 As System.Double = y1
            'Dim h2 As System.Double = x2
            'Dim k2 As System.Double = y2
            'Dim Left1, Right1 As System.Double ' 1st intersection.
            'Dim Left2, Right2 As System.Double ' 2nd intersection.

            ' REF: How can I find the points at which two circles intersect?
            ' https://math.stackexchange.com/questions/256100/how-can-i-find-the-points-at-which-two-circles-intersect

            ' Standard form of a circle.
            ' [(X - h)^2] + [(Y - k)^2] = r^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            '' Circle1.
            'Left1 = ((i1x - h1) ^ 2) + ((i1y - k1) ^ 2)
            'Right1 = r1 ^ 2
            'Left2 = ((i2x - h1) ^ 2) + ((i2y - k1) ^ 2)
            'Right2 = r1 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If
            '' Circle2.
            'Left1 = ((i1x - h2) ^ 2) + ((i1y - k2) ^ 2)
            'Right1 = r2 ^ 2
            'Left2 = ((i2x - h2) ^ 2) + ((i2y - k2) ^ 2)
            'Right2 = r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Localize parameters, for one point of intersection.
            ' [(X - h1)^2] + [(Y - k1)^2] = r1^2
            ' [(X - h2)^2] + [(Y - k2)^2] = r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = (i1x - h1) ^ 2 + (i1y - k1) ^ 2
            'Right1 = r1 ^ 2
            'Left2 = (i2x - h2) ^ 2 + (i2y - k2) ^ 2
            'Right2 = r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Subtract the second equation from the first.
            ' [(X - h1)^2] + [(Y - k1)^2] - {[(X - h2)^2] + [(Y - k2)^2]} = r1^2 - r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = (i1x - h1) ^ 2 + (i1y - k1) ^ 2 - ((i1x - h2) ^ 2 + (i1y - k2) ^ 2)
            'Right1 = r1 ^ 2 - r2 ^ 2
            'Left2 = (i2x - h1) ^ 2 + (i2y - k1) ^ 2 - ((i2x - h2) ^ 2 + (i2y - k2) ^ 2)
            'Right2 = r1 ^ 2 - r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Handle negations.
            ' [(X - h1)^2] + [(Y - k1)^2]
            ' - [(X - h2)^2] - [(Y - k2)^2]
            ' = r1^2 - r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = (i1x - h1) ^ 2 + (i1y - k1) ^ 2 _
            '    - (i1x - h2) ^ 2 - (i1y - k2) ^ 2
            'Right1 = r1 ^ 2 - r2 ^ 2
            'Left2 = (i2x - h1) ^ 2 + (i2y - k1) ^ 2 _
            '    - (i2x - h2) ^ 2 - (i2y - k2) ^ 2
            'Right2 = r1 ^ 2 - r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Expand squares.
            ' [X^2 - (2*h1*X) + h1^2] + [Y^2 - (2*k1*Y) + k1^2]
            ' - [X^2 - (2*h2*X) + h2^2] - [Y^2 - (2*k2*Y) + k2^2]
            ' = r1^2 - r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = (i1x ^ 2 - (2 * h1 * i1x) + h1 ^ 2) + (i1y ^ 2 - (2 * k1 * i1y) + k1 ^ 2) _
            '    - (i1x ^ 2 - (2 * h2 * i1x) + h2 ^ 2) - (i1y ^ 2 - (2 * k2 * i1y) + k2 ^ 2)
            'Right1 = r1 ^ 2 - r2 ^ 2
            'Left2 = (i2x ^ 2 - (2 * h1 * i2x) + h1 ^ 2) + (i2y ^ 2 - (2 * k1 * i2y) + k1 ^ 2) _
            '    - (i2x ^ 2 - (2 * h2 * i2x) + h2 ^ 2) - (i2y ^ 2 - (2 * k2 * i2y) + k2 ^ 2)
            'Right2 = r1 ^ 2 - r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Simplify and rearrange, in steps:

            ' Handle negations.
            ' X^2 - (2*h1*X) + h1^2
            ' + Y^2 - (2*k1*Y) + k1^2
            ' - X^2 + (2*h2*X) - h2^2
            ' - Y^2 + (2*k2*Y) - k2^2
            ' = r1^2 - r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = i1x ^ 2 - (2 * h1 * i1x) + h1 ^ 2 _
            '    + i1y ^ 2 - (2 * k1 * i1y) + k1 ^ 2 _
            '    - i1x ^ 2 + (2 * h2 * i1x) - h2 ^ 2 _
            '    - i1y ^ 2 + (2 * k2 * i1y) - k2 ^ 2
            'Right1 = r1 ^ 2 - r2 ^ 2
            'Left2 = i2x ^ 2 - (2 * h1 * i2x) + h1 ^ 2 _
            '    + i2y ^ 2 - (2 * k1 * i2y) + k1 ^ 2 _
            '    - i2x ^ 2 + (2 * h2 * i2x) - h2 ^ 2 _
            '    - i2y ^ 2 + (2 * k2 * i2y) - k2 ^ 2
            'Right2 = r1 ^ 2 - r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Handle cancellations.
            ' - (2*h1*X) + h1^2
            ' - (2*k1*Y) + k1^2
            ' + (2*h2*X) - h2^2
            ' + (2*k2*Y) - k2^2
            ' = r1^2 - r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = -(2 * h1 * i1x) + h1 ^ 2 _
            ' - (2 * k1 * i1y) + k1 ^ 2 _
            ' + (2 * h2 * i1x) - h2 ^ 2 _
            ' + (2 * k2 * i1y) - k2 ^ 2
            'Right1 = r1 ^ 2 - r2 ^ 2
            'Left2 = -(2 * h1 * i2x) + h1 ^ 2 _
            ' - (2 * k1 * i2y) + k1 ^ 2 _
            ' + (2 * h2 * i2x) - h2 ^ 2 _
            ' + (2 * k2 * i2y) - k2 ^ 2
            'Right2 = r1 ^ 2 - r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Gather like terms.
            ' (2*h2*X) - (2*h1*X)
            ' + (2*k2*Y) - (2*k1*Y)
            ' + h1^2 - h2^2 + k1^2 - k2^2
            ' = r1^2 - r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = (2 * h2 * i1x) - (2 * h1 * i1x) _
            '    + (2 * k2 * i1y) - (2 * k1 * i1y) _
            '    + h1 ^ 2 - h2 ^ 2 + k1 ^ 2 - k2 ^ 2
            'Right1 = r1 ^ 2 - r2 ^ 2
            'Left2 = (2 * h2 * i2x) - (2 * h1 * i2x) _
            '    + (2 * k2 * i2y) - (2 * k1 * i2y) _
            '    + h1 ^ 2 - h2 ^ 2 + k1 ^ 2 - k2 ^ 2
            'Right2 = r1 ^ 2 - r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Extract X and Y terms.
            ' 2*(h2 - h1)*X
            ' + 2*(k2 - k1)*Y
            ' + h1^2 - h2^2 + k1^2 - k2^2
            ' = r1^2 - r2^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = 2 * (h2 - h1) * i1x _
            '    + 2 * (k2 - k1) * i1y _
            '    + h1 ^ 2 - h2 ^ 2 + k1 ^ 2 - k2 ^ 2
            'Right1 = r1 ^ 2 - r2 ^ 2
            'Left2 = 2 * (h2 - h1) * i2x _
            '    + 2 * (k2 - k1) * i2y _
            '    + h1 ^ 2 - h2 ^ 2 + k1 ^ 2 - k2 ^ 2
            'Right2 = r1 ^ 2 - r2 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Arrange for the general "aX + bY = c" form of a linear equation.
            ' 2*(h2 - h1)*X
            ' + 2*(k2 - k1)*Y
            ' = r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2

            '' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            'Left1 = 2 * (h2 - h1) * i1x + 2 * (k2 - k1) * i1y
            'Right1 = r1 ^ 2 - r2 ^ 2 + h2 ^ 2 - h1 ^ 2 + k2 ^ 2 - k1 ^ 2
            'Left2 = 2 * (h2 - h1) * i2x + 2 * (k2 - k1) * i2y
            'Right2 = r1 ^ 2 - r2 ^ 2 + h2 ^ 2 - h1 ^ 2 + k2 ^ 2 - k1 ^ 2
            'If Not (EqualEnough(Left1, Right1, Factor) AndAlso
            '    EqualEnough(Left2, Right2, Factor)) Then
            '    Return False
            'End If

            ' Arrange the "aX + bY = c" form of a linear equation for the
            ' slope-intercept form of a line.
            ' aX + bY = c
            ' bY = c - aX
            ' Y = (c - aX) / b
            ' Y = (c / b) - (a / b)X
            ' Y = (-a / b)X + (c / b) 

            ' Substitute from the "aX + bY = c" form.
            ' Y = (-(2*(h2 - h1)) / (2*(k2 - k1)))X
            '     + ((r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2) / (2*(k2 - k1))) 

            '            Return TryCircleLineIntersection(
            '                x1, y1, r1,
            '                (-(2 * (h2 - h1)) / (2 * (k2 - k1))),
            '                ((r1 ^ 2 - r2 ^ 2 + h2 ^ 2 - h1 ^ 2 + k2 ^ 2 - k1 ^ 2) / (2 * (k2 - k1))),
            '                i1x, i1y, i2x, i2y)

            Return TryCircleLineIntersection(
                x1, y1, r1,
                (-(2 * (x2 - x1)) / (2 * (y2 - y1))),
                ((r1 ^ 2 - r2 ^ 2 + x2 ^ 2 - x1 ^ 2 + y2 ^ 2 - y1 ^ 2) / (2 * (y2 - y1))),
                i1x, i1y, i2x, i2y)

        End Function ' TryCirclesIntersection

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
