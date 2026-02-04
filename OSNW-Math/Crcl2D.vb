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
        ''' <exception cref="System.ArgumentOutOfRangeException"> Thrown when
        ''' the value is infinite.</exception>"
        Public Property CenterX As System.Double
            Get
                Return Me.m_CenterX
            End Get
            Set(value As System.Double)

                ' Input checking.
                If System.Double.IsInfinity(value) Then
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(CenterX), MSGCHIV)
                End If

                Me.m_CenterX = value

            End Set
        End Property

        Private m_CenterY As System.Double
        ''' <summary>
        ''' Represents the Y-coordinate of the center of the <c>Circle2D</c>, on
        ''' a Cartesian grid. Dimensions are in generic "units".
        ''' </summary>
        ''' <exception cref="System.ArgumentOutOfRangeException"> Thrown when
        ''' the value is infinite.</exception>"
        Public Property CenterY As System.Double
            Get
                Return Me.m_CenterY
            End Get
            Set(value As System.Double)

                ' Input checking.
                If System.Double.IsInfinity(value) Then
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(CenterY), MSGCHIV)
                End If

                Me.m_CenterY = value

            End Set
        End Property

        Private m_Radius As System.Double
        ''' <summary>
        ''' Represents the radius of the <c>Circle2D</c>, on a Cartesian grid.
        ''' Dimensions are in generic "units".
        ''' </summary>
        ''' <exception cref="System.ArgumentOutOfRangeException"> Thrown when
        ''' the value is infinite or negative.</exception>"
        Public Property Radius As System.Double
            Get
                Return Me.m_Radius
            End Get
            Set(value As System.Double)

                ' Input checking.
                If System.Double.IsInfinity(value) Then
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(Radius), MSGCHIV)
                End If
                ' A zero radius seems useless, but may be valid in some specific
                ' case.
                If value < 0.0 Then
                    'Dim CaughtBy As System.Reflection.MethodBase =
                    '    System.Reflection.MethodBase.GetCurrentMethod
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(Radius), OSNW.Math.MSGCHNV)
                End If

                Me.m_Radius = value

            End Set
        End Property

        ''' <summary>
        ''' Represents the diameter of the <c>Circle2D</c>, on a Cartesian grid.
        ''' Dimensions are in generic "units".
        ''' </summary>
        ''' <exception cref="System.ArgumentOutOfRangeException"> Thrown when
        ''' the value is infinite or negative.</exception>"
        Public Property Diameter As System.Double
            ' DEV: Being functionally redundant, this may need to be excluded
            ' from any serialization process.
            Get
                Return Me.Radius * 2.0
            End Get
            Set(value As System.Double)

                ' Input checking.
                If System.Double.IsInfinity(value) Then
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(Diameter), MSGCHIV)
                End If
                ' A zero radius seems useless, but may be valid in some specific
                ' case.
                If value < 0.0 Then
                    'Dim CaughtBy As System.Reflection.MethodBase =
                    '    System.Reflection.MethodBase.GetCurrentMethod
                    Throw New System.ArgumentOutOfRangeException(
                        NameOf(Diameter), OSNW.Math.MSGCHNV)
                End If

                Me.Radius = value / 2.0

            End Set
        End Property

#End Region ' "Fields and Properties"

#Region "Constructors"

        ''' <summary>
        ''' A default constructor that creates a new instance of the
        ''' <c>Circle2D</c> class with default center coordinates and radius.
        ''' The default is a unit circle centered at the origin.
        ''' </summary>
        Public Sub New()
            ' A default constructor is required to allow inheritance.
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
        ''' <exception cref="System.ArgumentOutOfRangeException"> Thrown when
        ''' any parameter is infinite.</exception>
        ''' <exception cref="System.ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="radius"/> is negative.
        ''' </exception>"
        Public Sub New(ByVal centerX As System.Double,
                       ByVal centerY As System.Double,
                       ByVal radius As System.Double)

            ' Input checking.
            If System.Double.IsInfinity(centerX) OrElse
                System.Double.IsInfinity(centerY) OrElse
                System.Double.IsInfinity(radius) Then

                Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                    $"Arguments to {NameOf(CaughtBy)} {MSGCHIV}")
            End If
            ' A zero radius seems useless, but may be valid in some specific
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
        ''' <exception cref="System.ArgumentOutOfRangeException">
        ''' Thrown when any parameter is infinite.
        ''' </exception>
        ''' <exception cref="System.ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="radius"/> is negative.
        ''' </exception>"
        Public Sub New(ByVal center As Point2D,
                       ByVal radius As System.Double)

            ' Input checking.
            If System.Double.IsInfinity(center.X) OrElse
                System.Double.IsInfinity(center.Y) OrElse
                System.Double.IsInfinity(radius) Then
                Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                    $"Arguments to {NameOf(CaughtBy)} {MSGCHIV}")
            End If
            ' A zero radius seems useless, but may be valid in some specific
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
        '''' ''''''''''''''''''''''''''''''''''
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
        ''' <param name="intersect0X">Specifies the X-coordinate of one
        ''' intersection.</param>
        ''' <param name="intersect0Y">Specifies the Y-coordinate of one
        ''' intersection.</param>
        ''' <param name="intersect1X">Specifies the X-coordinate of the other
        ''' intersection.</param>
        ''' <param name="intersect1Y">Specifies the Y-coordinate of the other
        ''' intersection.</param>
        ''' <returns><c>True</c> if the process succeeds; otherwise,
        ''' <c>False</c>.
        ''' When valid, also returns the results in
        ''' <paramref name="intersect0X"/>, <paramref name="intersect0Y"/>,
        ''' <paramref name="intersect1X"/>, and
        ''' <paramref name="intersect1Y"/>.</returns>
        ''' <remarks>
        ''' A vertical line (infinite slope) will not have a Y-intercept, except
        ''' when that line passes through the circle center - a case which would
        ''' have infinite common points.
        ''' <br/>
        ''' To avoid throwing an exception, <c>False</c> is returned
        ''' when any of <paramref name="circleX"/>, <paramref name="circleY"/>,
        ''' <paramref name="circleR"/>, <paramref name="lineM"/>,
        ''' <paramref name="lineB"/>, or
        ''' <paramref name="circleR"/> is infinite,
        ''' or
        ''' when <paramref name="circleR"/> is negative.
        ''' </remarks>
        Public Shared Function TryCircleLineIntersections(
            ByVal circleX As System.Double, ByVal circleY As System.Double,
            ByVal circleR As System.Double, ByVal lineM As System.Double,
            ByVal lineB As System.Double, ByRef intersect0X As System.Double,
            ByRef intersect0Y As System.Double,
            ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double) As System.Boolean

            ' Suspended to avoid exceptions:
            'If System.Double.IsInfinity(circleX) OrElse
            '    System.Double.IsInfinity(circleY) OrElse
            '    System.Double.IsInfinity(circleR) OrElse
            '    System.Double.IsInfinity(lineM) OrElse
            '    System.Double.IsInfinity(lineB) Then
            '    'Dim CaughtBy As System.Reflection.MethodBase =
            '    '    System.Reflection.MethodBase.GetCurrentMethod
            '    Throw New System.ArgumentOutOfRangeException(
            '        $"Arguments to {NameOf(TryCircleLineIntersections)} {MSGCHIV}")
            'End If
            '' A zero radius seems useless, but may be valid in some specific
            '' case.
            'If circleR < 0.0 Then
            '    'Dim CaughtBy As System.Reflection.MethodBase =
            '    '    System.Reflection.MethodBase.GetCurrentMethod
            '    Throw New System.ArgumentOutOfRangeException(
            '            NameOf(circleR), OSNW.Math.MSGCHNV)
            'End If
            ' ''''''''''''''''''''''''''''''''''
            ' Input checking.
            If System.Double.IsInfinity(circleX) OrElse
                System.Double.IsInfinity(circleY) OrElse
                System.Double.IsInfinity(circleR) OrElse
                System.Double.IsInfinity(lineM) OrElse
                System.Double.IsInfinity(lineB) OrElse
                System.Double.IsInfinity(circleR) OrElse
                circleR < 0.0 Then
                Return False
            End If

            ' The derivation follows:
            ' DEV: Any square brackets, braces, and split lines below are used
            ' for visual clarity across the various steps. "h", "k", "r", "m",
            ' "b", and squared values are carried through the derivation, in
            ' keeping with the standard form. The actual parameters, and
            ' multiplication vs. squaring, are substituted in the
            ' implementation.

            ' Standard form of a circle and a line.
            ' (X - h)^2 + (Y - k)^2 = r^2
            ' Y = m*X + b

            ' A point at the intersection of the circle and the line conforms to
            ' both equations. Substitute the second equation into the first.
            ' (X - h)^2 + (m*X + b - k)^2 = r^2

            ' Expand the squares.
            ' X^2 - 2*h*X + h^2
            ' + m*X*(m*X + b - k)
            ' + b*(m*X + b - k)
            ' - k*(m*X + b - k)
            ' = r^2

            ' Distribute the multiplications.
            ' X^2 - 2*h*X + h^2
            ' + m^2*X^2 + m*b*X - m*k*X
            ' + m*b*X + b^2 - b*k
            ' - m*k*X - b*k + k^2 - r^2
            ' = 0

            ' Gather like terms.
            ' X^2 + m^2*X^2
            ' + (2*m*b)*X - (2*h)*X - (2*m*k)*X
            ' + h^2 + k^2 - r^2 + b^2 - 2*b*k
            ' = 0

            ' Rework the second row.
            ' + (2*m*b)*X - (2*h)*X - (2*m*k)*X
            ' Extract the common X.
            ' + [(2*m*b) - (2*h) - (2*m*k)]*X
            ' Extract the common X.
            ' + 2*[(m*b) - (h) - (m*k)]*X
            ' Rearrange.
            ' + 2*[(m*b) - (m*k) - (h)]*X
            ' Extract the common m.
            ' + 2*[m*{(b) - (k)} - (h)]*X

            ' Use the reworked the second row.
            ' (1 + m^2)*X^2
            ' + 2*(m*((b) - (k)) - (h))*X
            ' + h^2 + k^2 - r^2 + b^2 - 2*b*k
            ' = 0

            ' Implementation:

            ' Force multiplication vs. raising to a power.
            Dim CircleX2 As System.Double = circleX * circleX
            Dim CircleY2 As System.Double = circleY * circleY
            Dim CircleR2 As System.Double = circleR * circleR
            Dim LineM2 As System.Double = lineM * lineM
            Dim LineB2 As System.Double = lineB * lineB

            ' Set up and use "a*x^2 + b*x + c = 0" terms in the quadratic
            ' formula.
            Dim QuadA As Double = 1 + LineM2
            Dim QuadB As Double = 2 * ((lineM * (lineB - circleY)) - circleX)
            Dim QuadC As Double = CircleX2 + CircleY2 - CircleR2 + LineB2 -
                2 * lineB * circleY
            If Not TryQuadratic(
                QuadA, QuadB, QuadC, intersect0X, intersect1X) Then

                intersect0X = System.Double.NaN
                intersect0Y = System.Double.NaN
                intersect1X = System.Double.NaN
                intersect1Y = System.Double.NaN
                Return False
            End If

            ' Substitute into "y = mx + b".
            intersect0Y = lineM * intersect0X + lineB
            intersect1Y = lineM * intersect1X + lineB
            Return True

        End Function ' TryCircleLineIntersections

        '''' Suspended XML comments for suspended code:
        '''' <exception cref="System.ArgumentOutOfRangeException">
        '''' Thrown when <paramref name="circleX"/>, <paramref name="circleY"/>,
        '''' <paramref name="circleR"/>, <paramref name="lineX1"/>,
        '''' <paramref name="lineX2"/>, <paramref name="lineY1"/>, or
        '''' <paramref name="lineY2"/> is infinite.
        '''' </exception>
        '''' <exception cref="System.ArgumentOutOfRangeException">Thrown when
        '''' <paramref name="circleR"/> is less than or equal to zero.
        '''' </exception>
        '''' ''''''''''''''''''''''''''''''''''
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
        ''' <param name="line1X">Specifies the X-coordinate of the first point
        ''' on the line.</param>
        ''' <param name="line1Y">Specifies the Y-coordinate of the first point
        ''' on the line.</param>
        ''' <param name="line2X">Specifies the X-coordinate of the second point
        ''' on the line.</param>
        ''' <param name="line2Y">Specifies the Y-coordinate of the second point
        ''' on the line.</param>
        ''' <param name="intersect0X">Specifies the X-coordinate of one
        ''' intersection.</param>
        ''' <param name="intersect0Y">Specifies the Y-coordinate of one
        ''' intersection.</param>
        ''' <param name="intersect1X">Specifies the X-coordinate of the other
        ''' intersection.</param>
        ''' <param name="intersect1Y">Specifies the Y-coordinate of the other
        ''' intersection.</param>
        ''' <returns>
        ''' <c>True</c> if the process succeeds; otherwise, <c>False</c>. When
        ''' valid, also returns the results in <paramref name="intersect0X"/>,
        ''' <paramref name="intersect0Y"/>, <paramref name="intersect1X"/>, and
        ''' <paramref name="intersect1Y"/>.
        ''' </returns>
        ''' <remarks>
        ''' To avoid throwing an exception, <c>False</c> is returned
        ''' when <paramref name="circleX"/>, <paramref name="circleY"/>,
        ''' <paramref name="circleR"/>, <paramref name="line1X"/>,
        ''' or <paramref name="line1Y"/>, or <paramref name="line2Y"/> is
        ''' infinite,
        ''' or
        ''' when <paramref name="circleR"/> is less than or equal to zero.
        ''' </remarks>
        Public Shared Function TryCircleLineIntersections(
            ByVal circleX As System.Double,
            ByVal circleY As System.Double, ByVal circleR As System.Double,
            ByVal line1X As System.Double, ByVal line1Y As System.Double,
            ByVal line2X As System.Double, ByVal line2Y As System.Double,
            ByRef intersect0X As System.Double,
            ByRef intersect0Y As System.Double,
            ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double) As System.Boolean

            ' Suspended to avoid exceptions:
            '       If System.Double.IsInfinity(circleX) OrElse
            '           System.Double.IsInfinity(circleY) OrElse
            '           System.Double.IsInfinity(circleR) OrElse
            '           System.Double.IsInfinity(line1X) OrElse
            '           System.Double.IsInfinity(line1Y) OrElse
            '           System.Double.IsInfinity(line2X) OrElse
            '           System.Double.IsInfinity(line2Y) Then
            '           'Dim CaughtBy As System.Reflection.MethodBase =
            '           '    System.Reflection.MethodBase.GetCurrentMethod
            '           Throw New System.ArgumentOutOfRangeException(
            '               $"Arguments to {NameOf(TryCircleLineIntersections)} {MSGCHIV}")
            '       End If
            '        If circleR <= 0.0 Then
            '            'Dim CaughtBy As System.Reflection.MethodBase =
            '            '    System.Reflection.MethodBase.GetCurrentMethod
            '            Throw New System.ArgumentOutOfRangeException(
            '                NameOf(circleR), MSGVMBGTZ)
            '        End If
            '''' ''''''''''''''''''''''''''''''''''
            ' Input checking.
            If System.Double.IsInfinity(circleX) OrElse
                System.Double.IsInfinity(circleY) OrElse
                System.Double.IsInfinity(circleR) OrElse
                System.Double.IsInfinity(line1X) OrElse
                System.Double.IsInfinity(line1Y) OrElse
                System.Double.IsInfinity(line2X) OrElse
                System.Double.IsInfinity(line2Y) OrElse
                circleR <= 0.0 Then
                Return False
            End If

            ' Check for a vertical line.
            Dim DeltaX As System.Double = line2X - line1X
            If DeltaX.Equals(0.0) Then
                ' Vertical line; X = line1X.

                ' Can there be an intersection?
                If System.Math.Abs(line1X - circleX) > circleR Then
                    ' No intersection possible.
                    intersect0X = System.Double.NaN
                    intersect0Y = System.Double.NaN
                    intersect1X = System.Double.NaN
                    intersect1Y = System.Double.NaN
                    Return False
                End If

                ' The derivation follows:
                ' Standard form of a circle.
                ' (X - h)^2 + (Y - k)^2 = r^2

                ' Substitute parameters into standard form equation. Solve for
                ' X.
                ' (X - circleX)^2 + (Y - circleY)^2 = circleR^2
                ' (Y - circleY)^2 = circleR^2 - (X - circleX)^2
                ' Y - circleY = sqrt(circleR^2 - (X - circleX)^2)
                ' Y = circleY + sqrt(circleR^2 - (X - circleX)^2)

                ' Use that at one point.
                ' Y = circleY + sqrt(circleR^2 - (line1X - circleX)^2)

                ' Get the Y values.
                ' Root = sqrt(circleR^2 - (line1X - circleX)^2)
                ' intersect0Y = circleY + Root
                ' intersect1Y = circleY - Root

                Dim Minus As System.Double = line1X - circleX
                Dim Root As System.Double =
                    System.Math.Sqrt((circleR * circleR) - (Minus * Minus))
                intersect0Y = circleY + Root
                intersect1Y = circleY - Root
                intersect0X = line1X
                intersect1X = line1X ' Yes, the same assignment.
                Return True

            End If ' Vertical line.

            ' On getting here, the line is not vertical.

            ' Get the slope of the line.
            ' M = (Y2 - Y1) / (X2 - X1); generic slope.
            Dim lineM As System.Double = (line2Y - line1Y) / DeltaX

            ' Get the equation for the line.
            ' Y = M*X + B; Standard form line.
            ' B = Y - M*X; Solve for the Y-intercept.
            Dim lineB As System.Double = line1Y - lineM * line1X

            Return TryCircleLineIntersections(circleX, circleY, circleR, lineM,
                lineB, intersect0X, intersect0Y, intersect1X, intersect1Y)

        End Function ' TryCircleLineIntersections

        ''' <summary>
        ''' Determines whether two circles intersect, given their center
        ''' coordinates and radii.
        ''' </summary>
        ''' <param name="x0">Specifies the X-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="y0">Specifies the Y-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="r0">Specifies the radius of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="x1">Specifies the X-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="y1">Specifies the Y-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="r1">Specifies the radius of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="tolerance">Specifies the maximum offset from zero which
        ''' is assumed to represent zero.</param>
        ''' <returns><c>True</c> if the circles intersect; otherwise,
        ''' <c>False</c>.</returns>
        ''' <remarks>
        ''' Any infinite value, negative radius, or a negative tolerance, will
        ''' return <c>False</c>, to avoid an exception. Tangent circles, where
        ''' any gap is deemed to be zero, will have only one intersection.
        ''' Concentric circles, where the X- and Y-coordinates are deemed equal,
        ''' will have either zero or infinite common points. The second case is
        ''' considered to not be intersecting.
        ''' <br/>The tangent and concentric states are based on the specified
        ''' <paramref name="tolerance"/>. This does comparisons based on scale,
        ''' not on an absolute numeric value. The control value is
        ''' <paramref name="tolerance"/> multiplied by the largest magnitude
        ''' among the dimensions, to determine the minimum difference that
        ''' excludes equality.
        ''' Select <paramref name="tolerance"/> such that it is a good
        ''' representation of zero relative to other known or expected
        ''' values.
        ''' </remarks>
        Public Shared Function CirclesIntersect(ByVal x0 As System.Double,
            ByVal y0 As System.Double, ByVal r0 As System.Double,
            ByVal x1 As System.Double, ByVal y1 As System.Double,
            ByVal r1 As System.Double, ByVal tolerance As System.Double) _
            As System.Boolean

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' Input checking.
            If System.Double.IsInfinity(x0) OrElse
                System.Double.IsInfinity(y0) OrElse
                System.Double.IsInfinity(r0) OrElse
                System.Double.IsInfinity(x1) OrElse
                System.Double.IsInfinity(y1) OrElse
                System.Double.IsInfinity(r1) OrElse
                System.Double.IsInfinity(tolerance) Then

                Return False ' To avoid an exception.
            End If
            If (r0 < 0.0) OrElse (r1 < 0.0) OrElse (tolerance < 0.0) Then
                Return False ' To avoid an exception.
            End If

            ' Check for solvability.
            Dim ToleranceAbs As System.Double =
                System.Math.Abs(tolerance * MaxValAbs(x0, y0, r0, x1, y1, r1))
            Dim CtrSeparation As System.Double =
                System.Double.Hypot(x1 - x0, y1 - y0)
            If CtrSeparation > (r0 + r1 + ToleranceAbs) Then
                ' Consider to be two isolated circles.
                Return False
            ElseIf CtrSeparation <
                (System.Math.Abs(r1 - r0) - ToleranceAbs) Then

                ' Consider to have one inside the other.
                Return False
            ElseIf EqualEnoughAbsolute(x1, x0, tolerance) AndAlso
                EqualEnoughAbsolute(y1, y0, tolerance) Then

                ' Consider the circles to be concentric, with either zero or
                ' infinite common points. The second case is considered to not
                ' be intersecting.
                Return False
            End If
            Return True

        End Function ' CirclesIntersect

        ''' <summary>
        ''' Determines whether two circles intersect, given their center
        ''' coordinates and radii.
        ''' </summary>
        ''' <param name="x0">Specifies the X-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="y0">Specifies the Y-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="r0">Specifies the radius of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="x1">Specifies the X-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="y1">Specifies the Y-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="r1">Specifies the radius of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <returns><c>True</c> if the circles intersect; otherwise,
        ''' <c>False</c>.</returns>
        ''' <remarks>
        ''' Tangent circles will have only one intersection. Concentric circles
        ''' will have either zero or infinite common points. The second case is
        ''' considered not to be intersecting. Any infinite value, or any
        ''' negative radius, will return <c>False</c>, to avoid an exception.
        ''' </remarks>
        Public Shared Function CirclesIntersect(ByVal x0 As System.Double,
            ByVal y0 As System.Double, ByVal r0 As System.Double,
            ByVal x1 As System.Double, ByVal y1 As System.Double,
            ByVal r1 As System.Double) As System.Boolean

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            '' Input checking.
            'If System.Double.IsInfinity(x0) OrElse
            '    System.Double.IsInfinity(y0) OrElse
            '    System.Double.IsInfinity(r0) OrElse
            '    System.Double.IsInfinity(x1) OrElse
            '    System.Double.IsInfinity(y1) OrElse
            '    System.Double.IsInfinity(r1) Then

            '    Return False ' To avoid an exception.
            'End If
            'If (r0 < 0.0) OrElse (r1 < 0.0) Then
            '    Return False ' To avoid an exception.
            'End If

            '' Check for solvability.
            'Dim CtrSeparation As System.Double =
            'System.Double.Hypot(x1 - x0, y1 - y0)
            'If CtrSeparation > (r0 + r1) Then
            '    ' Two isolated circles.
            '    Return False
            'ElseIf CtrSeparation < System.Math.Abs(r1 - r0) Then
            '    ' One inside the other.
            '    Return False
            'ElseIf x1.Equals(x0) AndAlso y1.Equals(y0) Then
            '    ' They are concentric, with either zero or infinite common
            '    ' points. The second case is considered not to be intersecting.
            '    Return False
            'End If
            'Return True

            Return CirclesIntersect(x0, y0, r0, x1, y1, r1, 0.0)

        End Function ' CirclesIntersect

        ''' <summary>
        ''' Determines whether two circles intersect.
        ''' </summary>
        ''' <param name="circle0">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle1"/>.</param>
        ''' <param name="circle1">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle0"/>.</param>
        ''' <param name="tolerance">Specifies the maximum offset from zero which
        ''' is assumed to represent zero.</param>
        ''' <returns><c>True</c> if the circles intersect; otherwise,
        ''' <c>False</c>.</returns>
        ''' <remarks>
        ''' Any infinite dimension, negative radius, or a negative tolerance, will
        ''' return <c>False</c>, to avoid an exception. Tangent circles, where
        ''' any gap is deemed to be zero, will have only one intersection.
        ''' Concentric circles, where the X- and Y-coordinates are deemed equal,
        ''' will have either zero or infinite common points. The second case is
        ''' considered to not be intersecting.
        ''' <br/>The tangent and concentric states are based on the specified
        ''' <paramref name="tolerance"/>. This does comparisons based on scale,
        ''' not on an absolute numeric value. The control value is
        ''' <paramref name="tolerance"/> multiplied by the largest magnitude
        ''' among the dimensions, to determine the minimum difference that
        ''' excludes equality.
        ''' Select <paramref name="tolerance"/> such that it is a good
        ''' representation of zero relative to other known or expected
        ''' values.
        ''' </remarks>
        Public Shared Function CirclesIntersect(ByVal circle0 As Circle2D,
                ByVal circle1 As Circle2D, ByVal tolerance As System.Double) _
                As System.Boolean

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' No input checking. circle0 and circle1 are presumed to have been
            ' checked when created.

            Return CirclesIntersect(
                circle0.CenterX, circle0.CenterY, circle0.Radius,
                circle1.CenterX, circle1.CenterY, circle1.Radius, tolerance)

        End Function ' CirclesIntersect

        ''' <summary>
        ''' Determines whether two circles intersect.
        ''' </summary>
        ''' <param name="circle0">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle1"/>.</param>
        ''' <param name="circle1">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle0"/>.</param>
        ''' <returns><c>True</c> if the circles intersect; otherwise,
        ''' <c>False</c>.</returns>
        Public Shared Function CirclesIntersect(ByVal circle0 As Circle2D,
                ByVal circle1 As Circle2D) As System.Boolean

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' No input checking. circle0 and circle1 are presumed to have been
            ' checked when created.

            Return CirclesIntersect(
                circle0.CenterX, circle0.CenterY, circle0.Radius,
                circle1.CenterX, circle1.CenterY, circle1.Radius, 0.0)

        End Function ' CirclesIntersect

        ''' <summary>
        ''' Calculates the intersection points of two circles defined by their
        ''' center coordinates and radii.
        ''' </summary>
        ''' <param name="x0">Specifies the X-coordinate of circle0.</param>
        ''' <param name="y0">Specifies the Y-coordinate of circle0.</param>
        ''' <param name="r0">Specifies the radius of circle0.</param>
        ''' <param name="x1">Specifies the X-coordinate of circle1.</param>
        ''' <param name="y1">Specifies the Y-coordinate of circle1.</param>
        ''' <param name="r1">Specifies the radius of circle0.</param>
        ''' <returns>A list of 0, 1, or 2 intersection points as
        ''' <see cref="OSNW.Math.Point2D"/> structure(s).</returns>
        ''' <exception cref="System.ArgumentOutOfRangeException"> Thrown when
        ''' any parameter is infinite.</exception>
        ''' <exception cref="ArgumentOutOfRangeException">when either radius is
        ''' negative.</exception>
        ''' <remarks>
        ''' If there are no intersection points, an empty list is returned. If
        ''' the circles are tangent to each other, a list with one intersection
        ''' point is returned. If the circles intersect at two points, a list
        ''' with both points is returned.
        ''' </remarks>
        Public Shared Function GetIntersections(ByVal x0 As System.Double,
                ByVal y0 As System.Double, ByVal r0 As System.Double,
                ByVal x1 As System.Double, ByVal y1 As System.Double,
                ByVal r1 As System.Double) _
                As System.Collections.Generic.List(Of Point2D)

            ' DEV: This is the worker for the related routines.

            ' Input checking.
            If System.Double.IsInfinity(x0) OrElse
                System.Double.IsInfinity(y0) OrElse
                System.Double.IsInfinity(r0) OrElse
                System.Double.IsInfinity(x1) OrElse
                System.Double.IsInfinity(y1) OrElse
                System.Double.IsInfinity(r1) Then

                Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                    $"Arguments to {NameOf(CaughtBy)} {MSGCHIV}")
            End If
            ' A zero radius seems useless, but may be valid in some specific
            ' case.
            If r0 < 0 OrElse r1 < 0 Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Dim ErrMsg As System.String = String.Format(
                    "{0}={1}, {2}={3}", NameOf(r0), r0, NameOf(r1), r1)
                Throw New System.ArgumentOutOfRangeException(
                    ErrMsg, OSNW.Math.MSGCHNV)
            End If

            Dim Intersections _
                As New System.Collections.Generic.List(Of Point2D)

            ' Calculate the distance between the centers of the circles.
            Dim DeltaX As System.Double = x1 - x0
            Dim DeltaY As System.Double = y1 - y0
            Dim DeltaCtr As System.Double =
                System.Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY)

            ' Concentric circles would have either zero or infinite intersecting
            ' points.
            ' Select a zero reference.
            Dim MaxAbs As System.Double =
                OSNW.Math.MaxValAbs({x0, y0, r0, x1, y1, r1})
            Dim ZeroVal As System.Double =
                OSNW.Math.DFLTGRAPHICTOLERANCE * MaxAbs

            If OSNW.Math.EqualEnoughZero(DeltaCtr, ZeroVal) Then
                Return Intersections ' Still empty.
            End If

            ' Check if the circles are too far apart or if one is contained
            ' within the other. Tangent circles do not match this test.
            If DeltaCtr > (r0 + r1) OrElse
                DeltaCtr < System.Math.Abs(r0 - r1) Then

                Return Intersections ' Still empty.
            End If

            ' On getting this far, the circles are neither isolated nor have one
            ' separately contained within the other. There should now be either
            ' one or two intersections.

            ' Check if the circles are outside-tangent to each other.
            If OSNW.Math.EqualEnough(r0 + r1, DeltaCtr,
                                     OSNW.Math.DFLTGRAPHICTOLERANCE) Then
                ' One intersection point.
                Dim C1Frac As System.Double = r0 / DeltaCtr
                Intersections.Add(New Point2D(x0 + C1Frac * DeltaX,
                                              y0 + C1Frac * DeltaY))
                Return Intersections
            End If

            ' Check if the circles are inside-tangent to each other.
            ' Two circles of the same radius cannot be inside-tangent to each
            ' other.
            If Not OSNW.Math.EqualEnough(
                r0, r1, OSNW.Math.DFLTGRAPHICTOLERANCE) Then

                If OSNW.Math.EqualEnough(System.Math.Abs(r0 - r1), DeltaCtr,
                                         OSNW.Math.DFLTGRAPHICTOLERANCE) Then
                    ' They are inside-tangent.
                    If r0 > r1 Then
                        Dim C1Frac As System.Double = r0 / DeltaCtr
                        Intersections.Add(New Point2D(
                                              x0 + (C1Frac * DeltaX),
                                              y0 + (C1Frac * DeltaY)))
                        Return Intersections
                    Else
                        Dim C2Frac As System.Double = r1 / DeltaCtr
                        Intersections.Add(New Point2D(
                                              x1 + (C2Frac * -DeltaX),
                                              y1 + (C2Frac * -DeltaY)))
                        Return Intersections
                    End If
                End If
            End If

            ' (The initial version of) the sequence below was generated by
            ' Visual Studio AI.

            ' Calculate two intersection points.
            Dim OnceA As System.Double =
                (r0 * r0 - r1 * r1 + DeltaCtr * DeltaCtr) / (2 * DeltaCtr)
            Dim OnceH As System.Double = System.Math.Sqrt(r0 * r0 - OnceA * OnceA)
            Dim ResultX0 As System.Double = x0 + OnceA * (DeltaX / DeltaCtr)
            Dim ResultY0 As System.Double = y0 + OnceA * (DeltaY / DeltaCtr)

            ' Two intersection points.
            Dim intersection1 As New Point2D(
                ResultX0 + OnceH * (DeltaY / DeltaCtr),
                ResultY0 - OnceH * (DeltaX / DeltaCtr))
            Dim intersection2 As New Point2D(
                ResultX0 - OnceH * (DeltaY / DeltaCtr),
                ResultY0 + OnceH * (DeltaX / DeltaCtr))
            Intersections.Add(intersection1)
            Intersections.Add(intersection2)
            Return Intersections

        End Function ' GetIntersections

        ''' <summary>
        ''' Calculates the intersection points between <paramref name="circle0"/>
        ''' and <paramref name="circle1"/>.
        ''' </summary>
        ''' <param name="circle0">Specifies the first circle.</param>
        ''' <param name="circle1">Specifies the second circle.</param>
        ''' <returns>A list of intersection points as
        ''' <see cref="OSNW.Math.Point2D"/> objects.</returns>
        Public Shared Function GetIntersections(
            ByVal circle0 As Circle2D, ByVal circle1 As Circle2D) _
            As System.Collections.Generic.List(Of Point2D)

            ' No input checking. circle0 and circle1 are presumed to have been
            ' checked when created.

            Return GetIntersections(
                circle0.CenterX, circle0.CenterY, circle0.Radius,
                circle1.CenterX, circle1.CenterY, circle1.Radius)

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

            ' No input checking. otherCircle is presumed to have been checked
            ' when created.

            Return GetIntersections(Me, otherCircle)
        End Function ' GetIntersections

        ''' <summary>
        ''' Attempts to determine where two circles intersect, given their
        ''' center coordinates and radii.
        ''' </summary>
        ''' <param name="x0">Specifies the X-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="y0">Specifies the Y-coordinate of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="r0">Specifies the radius of the first circle to
        ''' consider for intersection with the second circle.</param>
        ''' <param name="x1">Specifies the X-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="y1">Specifies the Y-coordinate of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="r1">Specifies the radius of the second circle to
        ''' consider for intersection with the first circle.</param>
        ''' <param name="intersect0X">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect0Y">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect1X">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <param name="intersect1Y">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <returns><c>True</c> if the intersections are found; otherwise,
        ''' <c>False</c>.
        ''' When valid, also returns the results in
        ''' <paramref name="intersect0X"/>, <paramref name="intersect0Y"/>,
        ''' <paramref name="intersect1X"/>, and
        ''' <paramref name="intersect1Y"/>.</returns>
        ''' <remarks>
        ''' Any infinite value, any negative radius, or circles that do not
        ''' intersect, will return <c>False</c>, to avoid an exception.
        ''' Concentric circles will have either zero or infinite common points;
        ''' the second case is considered to NOT be intersecting. Tangent
        ''' circles will have two identical (or nearly so) intersections.
        ''' </remarks>
        Public Shared Function TryCircleCircleIntersections(
            ByVal x0 As System.Double, ByVal y0 As System.Double,
            ByVal r0 As System.Double, ByVal x1 As System.Double,
            ByVal y1 As System.Double, ByVal r1 As System.Double,
            ByRef intersect0X As System.Double,
            ByRef intersect0Y As System.Double,
            ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double) _
            As System.Boolean

            ' DEV: This is the worker for the overload routine.

            ' Input checking.
            If System.Double.IsInfinity(x0) OrElse
                System.Double.IsInfinity(y0) OrElse
                System.Double.IsInfinity(r0) OrElse
                System.Double.IsInfinity(x1) OrElse
                System.Double.IsInfinity(y1) OrElse
                System.Double.IsInfinity(r1) Then

                intersect0X = Double.NaN
                intersect0Y = Double.NaN
                intersect1X = Double.NaN
                intersect1Y = Double.NaN
                Return False 'To avoid an exception.
            End If
            ' A zero radius seems useless, but may be valid in some specific
            ' case.
            ' Non-intersecting circles fail.
            ' Concentric circles (same center), will have either zero or
            ' infinite common points; the second case is considered to not be
            ' intersecting.
            If r0 < 0.0 OrElse r1 < 0.0 _
                OrElse Not CirclesIntersect(x0, y0, r0, x1, y1, r1) _
                OrElse x1.Equals(x0) AndAlso y1.Equals(y0) Then

                intersect0X = Double.NaN
                intersect0Y = Double.NaN
                intersect1X = Double.NaN
                intersect1Y = Double.NaN
                Return False 'To avoid an exception.
            End If

            ' Use these to substitute parameter names for clarity, squaring, and
            ' reuse.
            Dim x02 As System.Double = x0 * x0
            Dim x12 As System.Double = x1 * x1
            Dim y02 As System.Double = y0 * y0
            Dim y12 As System.Double = y1 * y1
            Dim r02 As System.Double = r0 * r0
            Dim r12 As System.Double = r1 * r1
            Dim DiffX As System.Double
            Dim DiffY As System.Double
            Dim SumRXY As System.Double
            Dim Intersect0X2 As System.Double

            If y1.Equals(y0) Then
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

                ' Since k2=k1, in this special case.
                ' [(X - h1)^2] + [(Y - k1)^2] = r1^2
                ' [(X - h2)^2] + [(Y - k1)^2] = r2^2

                ' Subtract the second equation from the first.
                ' [(X - h1)^2] + [(Y - k1)^2]
                ' - [(X - h2)^2] - [(Y - k1)^2]
                ' = r1^2 - r2^2

                ' Handle the cancellation.
                ' [(X - h1)^2] - [(X - h2)^2]
                ' = r1^2 - r2^2

                ' Expand the squares.
                ' [X^2 - (2*h1*X) + h1^2]
                ' - [X^2 - (2*h2*X) + h2^2]
                ' = r1^2 - r2^2

                ' Handle negation.
                ' X^2 - (2*h1*X) + h1^2
                ' - X^2 + (2*h2*X) - h2^2
                ' = r1^2 - r2^2

                ' Handle the cancellation.
                ' - (2*h1*X) + h1^2
                ' + (2*h2*X) - h2^2
                ' = r1^2 - r2^2

                ' Gather like terms. Extract the common 2 and X.
                ' 2*(h2 - h1)*X
                ' = r1^2 - r2^2 + h2^2 - h1^2

                ' Solve for X.
                ' *** This is part of the solution for both intersections. ***
                ' X = (r1^2 - r2^2 + h2^2 - h1^2) / (2*(h2 - h1))

                ' Solve the standard form of a circle for Y, for one circle.
                ' [(X - h)^2] + [(Y - k)^2] = r^2
                ' [(X - h1)^2] + [(Y - k1)^2] = r1^2

                ' Expand the squares.
                ' (X^2 -2*h1*X + h1^2) + (Y^2 -2*k1*Y + k1^2) = r1^2
                ' X^2 -2*h1*X + h1^2 + Y^2 -2*k1*Y + k1^2 = r1^2

                ' Arrange for the quadratic formula. X, having been solved
                ' above, may now be treated as one of the constants.
                ' Y^2
                ' - 2*k1*Y
                ' + X^2 - 2*h1*X + h1^2 + k1^2 - r1^2
                ' = 0

                ' Set up for the quadratic formula; then use that to get the two
                ' Y-coordinates, still treating X as one of the constants.
                ' a = 1
                ' b = -2*k1
                ' c = X^2 -2*h1*X + h1^2 + k1^2 - r1^2

                ' Implementation:

                intersect0X = (r02 - r12 + x12 - x02) / (2 * (x1 - x0))
                Intersect0X2 = intersect0X * intersect0X

                ' Set up and use "a*x^2 + b*x + c = 0" terms in the quadratic
                ' formula.
                Dim a As System.Double = 1
                Dim b As System.Double = -2 * y0
                Dim c As System.Double =
                    Intersect0X2 - 2 * x0 * intersect0X + x02 + y02 - r02
                If Not TryQuadratic(a, b, c, intersect0Y, intersect1Y) Then
                    intersect0X = System.Double.NaN
                    intersect0Y = System.Double.NaN
                    intersect1X = System.Double.NaN
                    intersect1Y = System.Double.NaN
                    Return False
                End If

                ' On getting here, the intersection points have been found.
                intersect1X = intersect0X
                Return True

            End If

            ' On getting here, the circles do not share the center Y-coordinate.
            ' The derivation follows:
            ' DEV: Square brackets, braces, and split lines are used below for
            ' visual clarity across the various steps. "h", "k", and squared
            ' values are carried through the derivation, in keeping with the
            ' standard form; the actual parameters, and multiplication vs.
            ' squaring, are substituted in the implementation.

            ' The basic approach is laid out here and broken out below:
            ' REF: How can I find the points at which two circles intersect?
            ' https://math.stackexchange.com/questions/256100/how-can-i-find-the-points-at-which-two-circles-intersect

            ' Standard form of a circle.
            ' [(X - h)^2] + [(Y - k)^2] = r^2

            ' Localize parameters, for a generic point (X, Y) of intersection.
            ' [(X - h1)^2] + [(Y - k1)^2] = r1^2
            ' [(X - h2)^2] + [(Y - k2)^2] = r2^2

            ' Subtract the second equation from the first.
            ' [(X - h1)^2] + [(Y - k1)^2]
            ' - {[(X - h2)^2] + [(Y - k2)^2]}
            ' = r1^2 - r2^2

            ' Handle negations.
            ' [(X - h1)^2] + [(Y - k1)^2]
            ' - [(X - h2)^2] - [(Y - k2)^2]
            ' = r1^2 - r2^2

            ' Expand the squares.
            ' [X^2 - (2*h1*X) + h1^2]
            ' + [Y^2 - (2*k1*Y) + k1^2]
            ' - [X^2 - (2*h2*X) + h2^2]
            ' - [Y^2 - (2*k2*Y) + k2^2]
            ' = r1^2 - r2^2

            ' Simplify and rearrange, in steps:

            ' Handle negations.
            ' X^2 - (2*h1*X) + h1^2
            ' + Y^2 - (2*k1*Y) + k1^2
            ' - X^2 + (2*h2*X) - h2^2
            ' - Y^2 + (2*k2*Y) - k2^2
            ' = r1^2 - r2^2

            ' Handle the cancellations.
            ' - (2*h1*X) + h1^2
            ' - (2*k1*Y) + k1^2
            ' + (2*h2*X) - h2^2
            ' + (2*k2*Y) - k2^2
            ' = r1^2 - r2^2

            ' Gather like terms.
            ' (2*h2*X) - (2*h1*X)
            ' + (2*k2*Y) - (2*k1*Y)
            ' = r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2

            ' Extract X and Y terms. Extract common 2s. Those arrange a standard
            ' "aX + bY = c" form of a linear equation.
            ' 2*(h2 - h1)*X
            ' + 2*(k2 - k1)*Y
            ' = r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2

            ' Division by 2 yields the "aX + bY = c" standard form of a linear
            ' equation.
            ' (h2 - h1)*X + (k2 - k1)*Y
            ' = (r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2)/2

            ' Arrange the "aX + bY = c" standard form of a linear equation into
            ' the "Y = mX + b" slope-intercept form of a line.
            ' Y = {[(r1^2 - r2^2 + h2^2 - h1^2 + k2^2 - k1^2)/2]
            '         - [(h2 - h1)*X]}
            '     /
            '     (k2 - k1)

            ' Substitute parameter names for clarity, squaring, and reuse.
            ' DiffH = h2 - h1
            ' DiffK = k2 - k1
            ' SumRHK = r12 - r22 + h22 - h12 + k22 - k12

            ' Y = ((SumRHK/2) - (DiffH*X)) / DiffK

            ' Implementation:

            ' Substitute parameter names for clarity, squaring, and reuse.
            DiffX = x1 - x0
            DiffY = y1 - y0
            SumRXY = r02 - r12 + x12 - x02 + y12 - y02
            Dim LineM As System.Double = -DiffX / DiffY
            Dim LineB As System.Double = SumRXY / (2 * DiffY)
            Return TryCircleLineIntersections(x0, y0, r0, LineM, LineB,
                intersect0X, intersect0Y, intersect1X, intersect1Y)

        End Function ' TryCircleCircleIntersections

        ''' <summary>
        ''' Attempts to determine where two circles intersect.
        ''' </summary>
        ''' <param name="circle0">Specifies the first circle to consider for
        ''' intersection with the second circle.</param>
        ''' <param name="circle1">Specifies the second circle to consider
        ''' for intersection with the first circle.</param>
        ''' <param name="intersect0X">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect0Y">Specifies the X-coordinate of the first
        ''' intersection.</param>
        ''' <param name="intersect1X">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <param name="intersect1Y">Specifies the X-coordinate of the second
        ''' intersection.</param>
        ''' <returns><c>True</c> if the intersections are found;
        ''' otherwise, <c>False</c>.
        ''' When valid, also returns the results in
        ''' <paramref name="intersect0X"/>, <paramref name="intersect0Y"/>,
        ''' <paramref name="intersect1X"/>, and
        ''' <paramref name="intersect1Y"/>.</returns>
        ''' <remarks>
        ''' Concentric circles will have either zero or infinite common points;
        ''' the second case is considered to NOT be intersecting.
        ''' Tangent circles will have two identical (or nearly so)
        ''' intersections.
        ''' </remarks>
        Public Shared Function TryCircleCircleIntersections(
            ByVal circle0 As OSNW.Circle2D, ByVal circle1 As OSNW.Circle2D,
            ByRef intersect0X As System.Double,
            ByRef intersect0Y As System.Double,
            ByRef intersect1X As System.Double,
            ByRef intersect1Y As System.Double) _
            As System.Boolean

            ' No input checking. circle0 and circle1 are presumed to have been
            ' checked when created.

            Return TryCircleCircleIntersections(circle0.CenterX,
                circle0.CenterY, circle0.Radius, circle1.CenterX,
                circle1.CenterY, circle1.Radius, intersect0X, intersect0Y,
                intersect1X, intersect1Y)

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
