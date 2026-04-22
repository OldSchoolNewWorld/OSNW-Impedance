'Option Explicit On
'Option Strict On
'Option Compare Binary
'Option Infer Off

Imports System.Numerics

Partial Public Module Math

    ''' <summary>
    ''' Attempts to solve the "aX^2 + bX + c = 0" quadratic equation for real
    ''' solutions.
    ''' </summary>
    ''' <param name="a">Specifies the <paramref name="a"/> coefficient in the
    ''' standard form.</param>
    ''' <param name="b">Specifies the <paramref name="b"/> coefficient in the
    ''' standard form.</param>
    ''' <param name="c">Specifies the <paramref name="c"/> coefficient in the
    ''' standard form.</param>
    ''' <param name="x0">Returns one of the x values.</param>
    ''' <param name="x1">Returns one of the x values.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' When valid, also returns the results in <paramref name="x0"/> and
    ''' <paramref name="x1"/>.</returns>
    ''' <remarks>
    ''' 
    ''' 
    ''' 
    ''' <br/>xxx
    ''' The most familiar use of this is to find the points where the graph of
    ''' the quadratic equation crosses the x-axis.
    ''' <br/>xxx
    ''' 
    ''' VISUAL STUDIO AI GENERATED TEXT THAT INDICATES SOME OTHER USES.
    ''' This is normally used to find two places where a curve intersects a
    ''' line, or to find the points of tangency between two curves. In those
    ''' cases, the coefficients are derived from ...
    ''' xxx
    ''' 
    ''' 
    ''' 
    ''' There are combinations of a, b, and c coefficients that lead to two
    ''' duplicate results.<br/>
    ''' The formula is x = (-b +/- sqrt(b^2 - 4ac)) / 2a.<br/>
    ''' Duplicates emerge when: (-b PLUS  sqrt(b^2 - 4ac)) / 2a =
    '''                      (-b MINUS sqrt(b^2 - 4ac)) / 2a<br/>
    ''' (-b + sqrt(b^2 - 4ac)) / 2a = (-b - sqrt(b^2 - 4ac)) / 2a<br/>
    ''' Multiply both sides by 2a.<br/>
    ''' -b + sqrt(b^2 - 4ac) = -b - sqrt(b^2 - 4ac)<br/>
    ''' Add b to both sides.<br/>
    ''' sqrt(b^2 - 4ac) = - sqrt(b^2 - 4ac)<br/>
    ''' SAME = -SAME only happens when SAME is zero, so, duplicates emerge
    ''' when:<br/>
    ''' sqrt(b^2 - 4ac) = 0.<br/>
    ''' Square both sides.<br/>
    ''' b^2 - 4ac = 0<br/>
    ''' So, duplicates emerge when the discriminant (b^2 - 4ac) is zero
    ''' (when b^2 = 4ac).<br/>
    ''' In that case, the formula reduces to x = -b / 2a. <c>TryQuadratic</c>
    ''' returns both results, leaving the caller to detect the duplication and
    ''' control how to respond.<br/>
    ''' 
    ''' <br/><example>
    ''' This example shows how to use <c>TryQuadratic</c>:
    ''' <code>
    ''' Dim A As System.Double = &lt;value or calculation>
    ''' Dim B As System.Double = &lt;value or calculation>
    ''' Dim C As System.Double = &lt;value or calculation>
    ''' Dim X0 As System.Double
    ''' Dim X1 As System.Double
    ''' 
    ''' If OSNW.Math.TryQuadratic(A, B, C, X0, X1) Then
    '''  '
    '''  ' Use X0, X1, and other derived values for further processing.
    '''  '
    ''' else
    '''  '
    '''  ' Respond to the failure with a warning, exception, or default
    '''  ' return value.
    '''  '
    ''' End If
    ''' 
    '''  - or -
    ''' 
    ''' ' Check for unsolvable conditions.
    ''' If not OSNW.Math.TryQuadratic(A, B, C, X0, X1) Then
    '''  '
    '''  ' Respond to the failure with a warning, exception, or default
    '''  ' return value.
    '''  ' Early exit.
    '''  '
    ''' End If
    ''' 
    '''  '
    '''  ' Use X0, X1, and other derived values for further processing.
    '''  '
    ''' </code></example>
    ''' 
    ''' </remarks>
    Public Function TryQuadratic(ByVal a As System.Double,
        ByVal b As System.Double, ByVal c As System.Double,
        ByRef x0 As System.Double, ByRef x1 As System.Double) _
        As System.Boolean

        ' Select a factor for zero tolerance.
        Const ZEROFACTOR As System.Double = 0.001

        ' Input checking.
        Dim AC4 As System.Double = 4 * a * c
        Dim SqrB As System.Double = b * b
        If a.Equals(0.0) OrElse AC4 > SqrB Then
            ' Not a quadratic equation.
            x0 = Double.NaN
            x1 = Double.NaN
            Return False
        End If

        ' Check for tangency.
        If SqrB.Equals(AC4) Then
            ' The discriminant is zero, so the equation has one solution.
            x0 = -b / (2.0 * a)
            x1 = x0 ' Duplicate.
            Return True ' Result known.
        End If

        ' Use (scaled) proximity to the magnitude of b to determine whether the
        ' values are close enough to risk catastrophic cancellation.
        Dim UseZeroTol As System.Double = ZEROFACTOR * System.Math.Abs(b)
        Dim DiscRoot As System.Double = System.Math.Sqrt(SqrB - AC4)
        Dim Sum As System.Double = -b + DiscRoot
        Dim Diff As System.Double = -b - DiscRoot
        Dim C2 As System.Double = 2.0 * c
        Dim A2 As System.Double = 2.0 * a

        If EqualEnoughZero(UseZeroTol, Sum) Then
            ' (-b PLUS DiscRoot) near zero

            ' Consider these values to be close enough to cause catastrophic
            ' cancellation due to subtraction of nearly-equal values, which can
            ' degrade the precision of the results.
            ' REF: Numerical calculation
            ' https://en.wikipedia.org/wiki/Quadratic_formula#Numerical_calculation
            ' REF: Square root in the denominator
            ' https://en.wikipedia.org/wiki/Quadratic_formula#Square_root_in_the_denominator

            ' Use the alternate approach.
            x0 = C2 / Sum ' Avoid catastrophic cancellation.
            x1 = Sum / A2 ' Normal.

        ElseIf EqualEnoughZero(UseZeroTol, Diff) Then
            ' (-b MINUS DiscRoot) near zero.

            ' Similar to above,
            x0 = Diff / A2 ' Normal.
            x1 = C2 / Diff ' Avoid catastrophic cancellation.
        Else
            x0 = Sum / A2 ' Normal.
            x1 = Diff / A2 ' Normal.
        End If

        Return True

    End Function ' TryQuadratic

    Partial Public Structure D2

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        Public Class Parabola


            ' Places to look:

            ' Focus and Directrix of a Parabola
            ' https://www.geeksforgeeks.org/maths/focus-and-directrix-of-a-parabola/

            ' Focus and Directrix of a Parabola
            ' https://mathmonks.com/parabola/focus-and-directrix-of-a-parabola



















            ' REF: Focus and Directrix of a Parabola
            ' https://www.geeksforgeeks.org/maths/focus-and-directrix-of-a-parabola/
            ' In mathematics, a parabola is the locus of a point that moves in a plane where its distance
            ' from a fixed point known as the focus is always equal to the distance from a fixed straight
            ' line known as directrix in the same plane.
            ' In other words, a parabola is a plane curve that is almost in U shape where every point is at
            ' equidistance from a fixed point known as focus
            ' and the straight line known as directrix. Parabola has only one focus and the focus never lies
            ' on the directrix.


            ' REF: Focus and Directrix of a Parabola
            ' https://mathmonks.com/parabola/focus-and-directrix-of-a-parabola
            ' A parabola is a U-shaped curve in which all points are equidistant from a fixed point and a
            ' fixed straight line. The point is the focus of the parabola, and the line is the directrix. 
            ' The focus lies on the axis of symmetry, and the directrix is parallel to either the x-axis or
            ' the y-axis. However, the focus never lies on the directrix.



#Region "Persistent Assigned Properties"

            ' These properties are read-only and set by New(). They should not
            ' be rotated, instead using D2.Point.RotateNormalRad or
            ' D2.Point.RotateNormalDeg to obtain rotated positions of persistent
            ' points.
            ' Only StdA, StdB, StdC, and Rotation should be included in
            ' serialization, with the other persistent properties being derived
            ' from them in New() and calculated properties being generated as
            ' needed.

            ' StdA Property.
            Private m_StdA As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property StdA As System.Double
                Get
                    Return Me.m_StdA
                End Get
                Private Set
                    Me.m_StdA = Value
                End Set
            End Property

            ' StdB Property.
            Private m_StdB As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property StdB As System.Double
                Get
                    Return Me.m_StdB
                End Get
                Private Set
                    Me.m_StdB = Value
                End Set
            End Property

            ' StdC Property.
            Private m_StdC As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property StdC As System.Double
                Get
                    Return Me.m_StdC
                End Get
                Private Set
                    Me.m_StdC = Value
                End Set
            End Property

            ' Focus Property.
            Private m_Focus As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Focus As Math.D2.Point
                Get
                    Return Me.m_Focus
                End Get
                Private Set
                    Me.m_Focus = Value
                End Set
            End Property

            ' Directrix Property.
            Private m_Directrix As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' Angle in RADIANS.
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Directrix As System.Double
                Get
                    Return Me.m_Directrix
                End Get
                Private Set
                    Me.m_Directrix = Value
                End Set
            End Property

            ' WHERE WOULD ROTATION BE CENTERED? FOCUS? VERTEX? ARBITRARY POINT?
            ' Rotation Property.
            Private m_Rotation As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' Angle in RADIANS.
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Rotation As System.Double
                Get
                    Return Me.m_Rotation
                End Get
                Private Set
                    Me.m_Rotation = Value
                End Set
            End Property

#End Region ' "Persistent Assigned Properties"

#Region "Other Persistent Properties"


            ' Vertex Property.
            Private m_Vertex As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Vertex As Math.D2.Point
                Get
                    Return Me.m_Vertex
                End Get
                Private Set
                    Me.m_Vertex = Value
                End Set
            End Property

            '
            '
            '
            '
            '

#End Region ' "Other Persistent Properties"

#Region "Other Properties"

            ' XIntercept0 Property.
            Private m_XIntercept0 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property XIntercept0 As Math.D2.Point
                Get
                    Return Me.m_XIntercept0
                End Get
                Private Set
                    Me.m_XIntercept0 = Value
                End Set
            End Property

            ' XIntercept1 Property.
            Private m_XIntercept1 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property XIntercept1 As Math.D2.Point
                Get
                    Return Me.m_XIntercept1
                End Get
                Private Set
                    Me.m_XIntercept1 = Value
                End Set
            End Property
            '
            '
            '
            '
            '

#End Region ' "Other Properties"

#Region "Aliases"

            Property A As Double
                Get
                    Return Me.m_StdA
                End Get
                Set(value As Double)
                    Me.m_StdA = value
                End Set
            End Property

            Property B As Double
                Get
                    Return Me.m_StdB
                End Get
                Set(value As Double)
                    Me.m_StdB = value
                End Set
            End Property

            Property C As Double
                Get
                    Return Me.m_StdC
                End Get
                Set(value As Double)
                    Me.m_StdC = value
                End Set
            End Property

            Property F As OSNW.Math.D2.Point
                Get
                    Return Me.m_Focus
                End Get
                Set(value As OSNW.Math.D2.Point)
                    Me.m_Focus = value
                End Set
            End Property

            Property D As Double
                Get
                    Return Me.m_Directrix
                End Get
                Set(value As Double)
                    Me.m_Directrix = value
                End Set
            End Property

#End Region ' "Aliases"

#Region "AI Methods"
            ' These are reworks of methods that were generated by Visual Studio
            ' AI, based on the properties of the parabola. They are not yet
            ' fully implemented and may require further development and testing.

            ''' <summary>
            ''' Calculates the corresponding Y value for the given
            ''' <paramref name="x"/> value using the standard form of the
            ''' parabola.
            ''' </summary>
            ''' <param name="x">xxxxxxxxxx</param>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetPointAtX(ByVal x As System.Double) _
                As OSNW.Math.D2.Point

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                Dim Y As System.Double = Me.StdA * x * x + Me.StdB * x + Me.StdC
                Return New OSNW.Math.D2.Point(x, Y)
            End Function ' GetPointAtX

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="y">xxxxxxxxxx</param>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetPointAtY(ByVal y As System.Double) _
                As OSNW.Math.D2.Point()

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                ' Rearrange the standard form of the parabola to solve for X
                ' given a Y value. This will involve solving a quadratic
                ' equation of the form:
                ' A*X^2 + B*X + C = Y
                ' A*X^2 + B*X + (C-Y) = 0

                Dim A As System.Double = Me.StdA
                Dim B As System.Double = Me.StdB
                Dim C As System.Double = Me.StdC - y
                Dim X0 As System.Double
                Dim X1 As System.Double
                If TryQuadratic(A, B, C, X0, X1) Then
                    Return New OSNW.Math.D2.Point() {
                        New OSNW.Math.D2.Point(X0, y),
                        New OSNW.Math.D2.Point(X1, y)}
                Else
                    ' No real solutions, return an empty array or handle as needed.
                    Return System.Array.Empty(Of OSNW.Math.D2.Point)
                End If

            End Function ' GetPointAtY

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetCoefficients() _
                As (A As System.Double, B As System.Double, C As System.Double)

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                Return (Me.StdA, Me.StdB, Me.StdC)
            End Function ' GetCoefficients

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetVertex() As OSNW.Math.D2.Point

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx
                ' The results of two alternative algorithms were compared for
                ' sameness of results, but no actual verification of answer
                ' correctness has been done.

                ' This is the way Visual Studio AI suggested to do it:

                ' The vertex of a parabola in standard form can be calculated
                ' using the formula: Vertex = (-b/(2a), c - (b^2/(4a)))
                Dim h As Double = -Me.StdB / (2 * Me.StdA)
                Dim k As Double = Me.StdC - ((Me.StdB * Me.StdB) / (4 * Me.StdA))
                Return New OSNW.Math.D2.Point(h, k)

                ' THIS IS THE WAY OSNW DID IT: (suppressed per the analysis below).

                '' Prior to rotation, the vertex will be at the point where the
                '' slope of the parabola is zero. The slope of the parabola at
                '' any point can be calculated using the formula
                '' Slope = 2*a*X + b, which is the derivative of the parabola's
                '' equation with respect to X. The Y value can then be
                '' calculated based on the X value.
                '' 2*a*X + b = 0
                '' 2*a*X = -b
                '' X = -b / (2*a)
                '' Y = a*X^2 + b*X + c)

                'Dim Vx As System.Double = -Me.StdB / (2 * Me.StdA)
                'Dim Vy As System.Double = Me.StdA * Vx * Vx + Me.StdB * Vx + Me.StdC
                'Return New Math.D2.Point(Vx, Vy)

                ' The following derivation was done to validate the suggested
                ' formula. Maybe the differences work out ok.

                ' Formulas for the X-coordinate of the vertex match:
                ' Vx = -B / (2 * A)

                ' Start from here:
                ' Vy = A*Vx^2 + B*Vx + C

                ' Substitute calculation of Vx.
                ' Vy =
                ' A * [-B / (2 * A)]^2
                ' + <B * [-B / (2 * A)]>
                ' + C

                ' Distribute the square.
                ' Rearrange the negation and B*.
                ' Vy =
                ' A * [(-B)^2 / (2 * A)^2]
                ' + [-B*B / (2 * A)]
                ' + C
                ' Vy =
                ' A * [B^2 / (2 * A)^2]
                ' - [B^2 / (2 * A)]
                ' + C

                ' Rearrange the A*.
                ' Distribute the square.
                ' Vy =
                ' [B^2 * A / (4 * A^2)]
                ' - [B^2 / (2 * A)]
                ' + C

                ' Cancel the As in the first term.
                ' Vy =
                ' [B^2 / (4 * A)]
                ' - [B^2 / (2 * A)]
                ' + C
                ' Vy = [B^2 / (4 * A)] - [B^2 / (2 * A)] + C

                ' Extract B^2.
                ' Vy = {B^2 * <[1 / (4 * A)] - [1 / (2 * A)]>} + C

                ' Extract *A in the denominators as /A outside the <>.
                ' Vy = {B^2 * <[1 / (4)] - [1 / (2)]> / A} + C
                ' Vy = {B^2 * <[1 / 4] - [1 / 2]> / A} + C

                ' Consolidate the constants.
                ' Vy = {B^2 * <-1 / 4> / A} + C

                ' Rearrange the negation and fraction.
                ' Vy = - [B^2 / (4 * A)] + C

                ' Rearrange the terms.
                ' Vy = C - [B^2 / (4 * A)]
                ' Vy = C - [B*B / (4 * A)]

                ' The derivation matches the AI suggestion:
                ' Vy = C       - [B * B              / (4 * A)]
                ' k =  Me.StdC - (Me.StdB * Me.StdB) / (4 * Me.StdA)
                ' The AI-suggested formula is correct uses the specified
                ' coefficients directly whereas the OSNW formula relies on
                ' derived values that may already have degraded precision.
                ' So, the AI-suggested formula will be active.

                ' A spreadsheet was created to compare the derived formulas to
                ' the Y = A * Vx^2 + B * Vx + C approach. Multiple rows of
                ' simultaneous comparisons, using random coefficients in the
                ' -0.1 to 0.1, -1 to 1, -10 to 10, -100 to 100, and
                ' -1000 to 1000 ranges, were compared. Comparison of the results
                ' showed extremely small differences. Floating-point precision
                ' numeric differences were on the order of 10^-12 or less for
                ' the largest values and 10^-15 or less for the largest values.

            End Function ' GetVertex

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetAxisOfSymmetry() As System.Double

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                ' The axis of symmetry of a parabola in standard form is given
                ' by the formula: x = -b/(2a), which is the X-coordinate of the vertex.
                Return -Me.StdB / (2 * Me.StdA)
            End Function ' GetAxisOfSymmetry

            ''' <summary>
            ''' Calculates the corresponding Y value for the specified
            ''' <paramref name="x"/> value.
            ''' </summary>
            ''' <param name="x">Specifies the X value at which to calculate the
            ''' Y value.</param>
            ''' <returns>The Y value at the specified X value.</returns>
            Public Function GetYAtX(x As System.Double) As System.Double

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                ' Calculate the corresponding Y value for the given X value using the standard form of
                ' the parabola.
                Return Me.StdA * x * x + Me.StdB * x + Me.StdC
            End Function ' GetYAtX





            '==================================================================






            '==================================================================





            '         Public Function GetFocus() As OSNW.Math.D2.Point
            '             Return Me.Focus
            '         End Function

            '         Public Function GetDirectrix() As Double
            '             Return Me.Directrix
            '         End Function

            '         Public Function GetRotation() As Double
            '             Return Me.Rotation
            '         End Function

            Public Function GetXAtY(y As Double) As Double()
                ' Rearrange the standard form of the parabola to solve for x given a y value.
                ' This will involve solving a quadratic equation of the form: StdA * x^2 + StdB * x + (StdC - y) = 0
                Dim a As Double = Me.StdA
                Dim b As Double = Me.StdB
                Dim c As Double = Me.StdC - y
                Dim x0 As Double
                Dim x1 As Double
                If TryQuadratic(a, b, c, x0, x1) Then
                    Return New Double() {x0, x1}
                Else
                    ' No real solutions, return an empty array or handle as needed.
                    '                 Return New Double() {}
                    Return System.Array.Empty(Of Double)
                End If
            End Function

            Public Function GetSlopeAtX(x As Double) As Double
                ' The slope of a parabola at any point can be calculated using the formula: Slope = 2*a*x + b
                Return 2 * Me.StdA * x + Me.StdB
            End Function

            Public Function GetSlopeAtY(y As Double) As Double()
                ' To find the slope at a given y value, we first need to find the corresponding x values for that y value.
                Dim xValues As Double() = Me.GetXAtY(y)
                Dim slopes As New List(Of Double)
                For Each x As Double In xValues
                    slopes.Add(Me.GetSlopeAtX(x))
                Next
                Return slopes.ToArray()
            End Function


            Public Function GetSlopeAtPoint(point As OSNW.Math.D2.Point) As Double
                ' The slope of a parabola at any point can be calculated using the formula: Slope = 2*a*x + b
                Return 2 * Me.StdA * point.X + Me.StdB
            End Function

            Public Function GetTangentLineAtPoint(point As OSNW.Math.D2.Point) As (Slope As Double, Intercept As Double)
                ' The slope of the tangent line at a given point can be calculated using the formula: Slope = 2*a*x + b
                Dim slope As Double = Me.GetSlopeAtPoint(point)
                ' The y-intercept of the tangent line can be calculated using the point-slope form of a line: y - y1 = m(x - x1)
                Dim intercept As Double = point.Y - slope * point.X
                Return (slope, intercept)
            End Function

            Public Function GetNormalLineAtPoint(point As OSNW.Math.D2.Point) As (Slope As Double, Intercept As Double)
                ' The slope of the normal line at a given point is the negative reciprocal of the slope of the tangent line.
                Dim tangentSlope As Double = Me.GetSlopeAtPoint(point)
                Dim normalSlope As Double = -1 / tangentSlope
                ' The y-intercept of the normal line can be calculated using the point-slope form of a line: y - y1 = m(x - x1)
                Dim intercept As Double = point.Y - normalSlope * point.X
                Return (normalSlope, intercept)
            End Function

            Public Function GetFocusDirectrixDistance() As Double
                ' The distance from the focus to the directrix can be calculated using the formula: Distance = 1/(4*a)
                Return 1 / (4 * Me.StdA)
            End Function

            Public Function GetLatusRectumLength() As Double
                ' The length of the latus rectum of a parabola can be calculated using the formula: Length = 1/|a|
                Return 1 / System.Math.Abs(Me.StdA)
            End Function

            Public Function GetFocalWidth() As Double
                ' The focal width of a parabola can be calculated using the formula: Focal Width = 1/|a|
                Return 1 / System.Math.Abs(Me.StdA)
            End Function

            Public Function GetFocalDiameter() As Double
                ' The focal diameter of a parabola can be calculated using the formula: Focal Diameter = 1/|a|
                Return 1 / System.Math.Abs(Me.StdA)
            End Function

            Public Function GetFocalLength() As Double
                ' The focal length of a parabola can be calculated using the formula: Focal Length = 1/(4*a)
                Return 1 / (4 * Me.StdA)
            End Function

            Public Function GetFocalParameter() As Double
                ' The focal parameter of a parabola can be calculated using the formula: Focal Parameter = 1/(4*a)
                Return 1 / (4 * Me.StdA)
            End Function

            Public Function GetFocalDistance() As Double
                ' The focal distance of a parabola can be calculated using the formula: Focal Distance = 1/(4*a)
                Return 1 / (4 * Me.StdA)
            End Function

            Public Function GetFocalRadius() As Double
                ' The focal radius of a parabola can be calculated using the formula: Focal Radius = 1/(4*a)
                Return 1 / (4 * Me.StdA)
            End Function

            Public Function GetFocalPoint() As OSNW.Math.D2.Point
                ' The focal point of a parabola can be calculated using the formula: Focus = (h, k + 1/(4*a)), where (h, k) is the vertex of the parabola.
                Dim vertex As OSNW.Math.D2.Point = Me.GetVertex()
                Dim focusY As Double = vertex.Y + 1 / (4 * Me.StdA)
                Return New OSNW.Math.D2.Point(vertex.X, focusY)
            End Function

            Public Function GetDirectrixLine() As (Slope As Double, Intercept As Double)
                ' The directrix of a parabola can be represented as a line. If the parabola opens upwards or downwards, the directrix is horizontal and can be represented as y = k - 1/(4*a), where (h, k) is the vertex of the parabola. If the parabola opens left or right, the directrix is vertical and can be represented as x = h - 1/(4*a), where (h, k) is the vertex of the parabola.
                Dim vertex As OSNW.Math.D2.Point = Me.GetVertex()
                If Me.StdA > 0 Then
                    ' Parabola opens upwards
                    Dim intercept As Double = vertex.Y - 1 / (4 * Me.StdA)
                    Return (0, intercept) ' Slope is 0 for a horizontal line
                Else
                    ' Parabola opens downwards
                    Dim intercept As Double = vertex.X - 1 / (4 * Me.StdA)
                    Return (Double.PositiveInfinity, intercept) ' Slope is infinite for a vertical line
                End If
            End Function

            Public Function GetAxisOfSymmetryLine() As (Slope As Double, Intercept As Double)
                ' The axis of symmetry of a parabola can be represented as a line. If the parabola opens upwards or downwards, the axis of symmetry is vertical and can be represented as x = -b/(2*a). If the parabola opens left or right, the axis of symmetry is horizontal and can be represented as y = -b/(2*a).
                If Me.StdA > 0 Then
                    ' Parabola opens upwards
                    Dim intercept As Double = -Me.StdB / (2 * Me.StdA)
                    Return (Double.PositiveInfinity, intercept) ' Slope is infinite for a vertical line
                Else
                    ' Parabola opens downwards
                    Dim intercept As Double = -Me.StdB / (2 * Me.StdA)
                    Return (0, intercept) ' Slope is 0 for a horizontal line
                End If
            End Function

            Public Function GetParabolaType() As String
                ' The type of a parabola can be determined based on the coefficient a. If a > 0, the parabola opens upwards. If a < 0, the parabola opens downwards. If a = 0, the equation does not represent a parabola.
                If Me.StdA > 0 Then
                    Return "Upwards"
                ElseIf Me.StdA < 0 Then
                    Return "Downwards"
                Else
                    Return "Not a Parabola"
                End If
            End Function

            Public Function GetParabolaOrientation() As String
                ' The orientation of a parabola can be determined based on the coefficient a. If a > 0, the parabola opens upwards. If a < 0, the parabola opens downwards. If a = 0, the equation does not represent a parabola.
                If Me.StdA > 0 Then
                    Return "Upwards"
                ElseIf Me.StdA < 0 Then
                    Return "Downwards"
                Else
                    Return "Not a Parabola"
                End If
            End Function

            Public Function GetParabolaDirection() As String
                ' The direction of a parabola can be determined based on the coefficient a. If a > 0, the parabola opens upwards. If a < 0, the parabola opens downwards. If a = 0, the equation does not represent a parabola.
                If Me.StdA > 0 Then
                    Return "Upwards"
                ElseIf Me.StdA < 0 Then
                    Return "Downwards"
                Else
                    Return "Not a Parabola"
                End If
            End Function

            Public Function GetParabolaConcavity() As String
                ' The concavity of a parabola can be determined based on the coefficient a. If a > 0, the parabola is concave up. If a < 0, the parabola is concave down. If a = 0, the equation does not represent a parabola.
                If Me.StdA > 0 Then
                    Return "Concave Up"
                ElseIf Me.StdA < 0 Then
                    Return "Concave Down"
                Else
                    Return "Not a Parabola"
                End If
            End Function

            Public Function GetParabolaWidth() As Double
                ' The width of a parabola can be calculated using the formula: Width = 1/|a|
                Return 1 / System.Math.Abs(Me.StdA)
            End Function

            Public Function GetParabolaDepth() As Double
                ' The depth of a parabola can be calculated using the formula: Depth = 1/(4*a)
                Return 1 / (4 * Me.StdA)
            End Function

            Public Function GetParabolaSize() As (Width As Double, Depth As Double)
                ' The size of a parabola can be represented as a tuple containing its width and depth.
                Return (Me.GetParabolaWidth(), Me.GetParabolaDepth())
            End Function

            Public Function GetParabolaDimensions() As (Width As Double, Depth As Double)
                ' The dimensions of a parabola can be represented as a tuple containing its width and depth.
                Return (Me.GetParabolaWidth(), Me.GetParabolaDepth())
            End Function

            'Public Function GetParabolaProperties() As (Focus As OSNW.Math.D2.Point, Directrix As Double, Rotation As Double, Coefficients As (A As Double, B As Double, C As Double), Vertex As OSNW.Math.D2.Point, AxisOfSymmetry As Double)
            ' ' The properties of a parabola can be represented as a tuple containing its focus, directrix, rotation, coefficients, vertex, and axis of symmetry.
            ' Return (Me.GetFocus(), Me.GetDirectrix(), Me.GetRotation(), Me.GetCoefficients(), Me.GetVertex(), Me.GetAxisOfSymmetry())
            'End Function
            Public Function GetParabolaProperties() As (Focus As OSNW.Math.D2.Point, Directrix As Double, Rotation As Double, Coefficients As (A As Double, B As Double, C As Double), Vertex As OSNW.Math.D2.Point, AxisOfSymmetry As Double)
                ' The properties of a parabola can be represented as a tuple containing its focus, directrix, rotation, coefficients, vertex, and axis of symmetry.
                Return (Me.Focus(), Me.Directrix(), Me.Rotation(), Me.GetCoefficients(), Me.GetVertex(), Me.GetAxisOfSymmetry())
            End Function

            Public Function GetParabolaCharacteristics() As (Type As String, Orientation As String, Direction As String, Concavity As String)
                ' The characteristics of a parabola can be represented as a tuple containing its type, orientation, direction, and concavity.
                Return (Me.GetParabolaType(), Me.GetParabolaOrientation(), Me.GetParabolaDirection(), Me.GetParabolaConcavity())
            End Function

            'Public Function GetParabolaInfo() As (Focus As OSNW.Math.D2.Point, Directrix As Double, Rotation As Double, Coefficients As (A As Double, B As Double, C As Double), Vertex As OSNW.Math.D2.Point, AxisOfSymmetry As Double, Type As String, Orientation As String, Direction As String, Concavity As String)
            ' ' The info of a parabola can be represented as a tuple containing its focus, directrix, rotation, coefficients, vertex, axis of symmetry, type, orientation, direction, and concavity.
            ' Return (Me.GetFocus(), Me.GetDirectrix(), Me.GetRotation(), Me.GetCoefficients(), Me.GetVertex(), Me.GetAxisOfSymmetry(), Me.GetParabolaType(), Me.GetParabolaOrientation(), Me.GetParabolaDirection(), Me.GetParabolaConcavity())
            'End Function
            Public Function GetParabolaInfo() As (Focus As OSNW.Math.D2.Point, Directrix As Double, Rotation As Double, Coefficients As (A As Double, B As Double, C As Double), Vertex As OSNW.Math.D2.Point, AxisOfSymmetry As Double, Type As String, Orientation As String, Direction As String, Concavity As String)
                ' The info of a parabola can be represented as a tuple containing its focus, directrix, rotation, coefficients, vertex, axis of symmetry, type, orientation, direction, and concavity.
                Return (Me.Focus(), Me.Directrix(), Me.Rotation(), Me.GetCoefficients(), Me.GetVertex(), Me.GetAxisOfSymmetry(), Me.GetParabolaType(), Me.GetParabolaOrientation(), Me.GetParabolaDirection(), Me.GetParabolaConcavity())
            End Function

            Public Function GetParabolaSummary() As String
                ' The summary of a parabola can be represented as a string containing its properties and characteristics.
                Dim info = Me.GetParabolaInfo()
                Return $"Focus: {info.Focus}, Directrix: {info.Directrix}, Rotation: {info.Rotation}, Coefficients: (A: {info.Coefficients.A}, B: {info.Coefficients.B}, C: {info.Coefficients.C}), Vertex: {info.Vertex}, Axis of Symmetry: {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}"
            End Function

            Public Function GetParabolaDescription() As String
                ' The description of a parabola can be represented as a string containing its properties and characteristics in a more human-readable format.
                Dim info = Me.GetParabolaInfo()
                Return $"This parabola has a focus at {info.Focus}, a directrix at y = {info.Directrix}, and a rotation of {info.Rotation} radians. Its coefficients are A: {info.Coefficients.A}, B: {info.Coefficients.B}, C: {info.Coefficients.C}. The vertex is located at {info.Vertex}, and the axis of symmetry is x = {info.AxisOfSymmetry}. This parabola is classified as {info.Type}, with an orientation of {info.Orientation}, a direction of {info.Direction}, and it is {info.Concavity}."
            End Function

            Public Function GetParabolaDetails() As String
                ' The details of a parabola can be represented as a string containing its properties and characteristics in an even more detailed format.
                Dim info = Me.GetParabolaInfo()
                Return $"The parabola defined by the equation y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C} has a focus located at the point {info.Focus} and a directrix represented by the line y = {info.Directrix}. The parabola is rotated by {info.Rotation} radians. The vertex of the parabola is at the point {info.Vertex}, and the axis of symmetry is given by the line x = {info.AxisOfSymmetry}. Based on its coefficients, this parabola opens {info.Type}, with an orientation of {info.Orientation}, a direction of {info.Direction}, and it is characterized as being {info.Concavity}."
            End Function

            Public Function GetParabolaFullDescription() As String
                ' The full description of a parabola can be represented as a string containing its properties and characteristics in the most detailed format possible.
                Dim info = Me.GetParabolaInfo()
                Return $"The parabola defined by the equation y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C} has a focus located at the point {info.Focus} and a directrix represented by the line y = {info.Directrix}. The parabola is rotated by {info.Rotation} radians. The vertex of the parabola is at the point {info.Vertex}, and the axis of symmetry is given by the line x = {info.AxisOfSymmetry}. Based on its coefficients, this parabola opens {info.Type}, with an orientation of {info.Orientation}, a direction of {info.Direction}, and it is characterized as being {info.Concavity}. This parabola has a width of {Me.GetParabolaWidth()} units and a depth of {Me.GetParabolaDepth()} units."
            End Function

            Public Function GetParabolaCompleteDescription() As String
                ' The complete description of a parabola can be represented as a string containing all of its properties and characteristics in the most comprehensive format possible.
                Dim info = Me.GetParabolaInfo()
                Return $"The parabola defined by the equation y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C} has a focus located at the point {info.Focus} and a directrix represented by the line y = {info.Directrix}. The parabola is rotated by {info.Rotation} radians. The vertex of the parabola is at the point {info.Vertex}, and the axis of symmetry is given by the line x = {info.AxisOfSymmetry}. Based on its coefficients, this parabola opens {info.Type}, with an orientation of {info.Orientation}, a direction of {info.Direction}, and it is characterized as being {info.Concavity}. This parabola has a width of {Me.GetParabolaWidth()} units and a depth of {Me.GetParabolaDepth()} units. The focal length is {Me.GetFocalLength()} units, and the focal width is {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteInfo() As String
                ' The complete info of a parabola can be represented as a string containing all of its properties and characteristics in the most comprehensive format possible.
                Dim info = Me.GetParabolaInfo()
                Return $"The parabola defined by the equation y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C} has a focus located at the point {info.Focus} and a directrix represented by the line y = {info.Directrix}. The parabola is rotated by {info.Rotation} radians. The vertex of the parabola is at the point {info.Vertex}, and the axis of symmetry is given by the line x = {info.AxisOfSymmetry}. Based on its coefficients, this parabola opens {info.Type}, with an orientation of {info.Orientation}, a direction of {info.Direction}, and it is characterized as being {info.Concavity}. This parabola has a width of {Me.GetParabolaWidth()} units and a depth of {Me.GetParabolaDepth()} units. The focal length is {Me.GetFocalLength()} units, and the focal width is {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummary() As String
                ' The complete summary of a parabola can be represented as a string containing all of its properties and characteristics in a concise format.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLine() As String
                ' The complete summary line of a parabola can be represented as a single line string containing all of its properties and characteristics in a concise format.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingle() As String
                ' The complete summary line single of a parabola can be represented as a single line string containing all of its properties and characteristics in a concise format.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConcise() As String
                ' The complete summary line single concise of a parabola can be represented as a single line string containing all of its properties and characteristics in a very concise format.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShort() As String
                ' The complete summary line single concise short of a parabola can be represented as a single line string containing all of its properties and characteristics in an extremely concise format.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortest() As String
                ' The complete summary line single concise shortest of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremely() As String
                ' The complete summary line single concise shortest extremely of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailed() As String
                ' The complete summary line single concise shortest extremely detailed of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensive() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed and comprehensive.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFull() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, and full.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremely() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, and extremely so.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremelyConcise() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely concise of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, extremely so, and concise.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremelyConciseShort() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely concise short of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, extremely so, concise, and short.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremelyConciseShortVery() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely concise short very of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, extremely so, concise, short, and very.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremelyConciseShortVeryExtremely() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely concise short very extremely of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, extremely so, concise, short, very, and extremely so.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremelyConciseShortVeryExtremelyShort() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely concise short very extremely short of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, extremely so, concise, short, very, extremely so, and short.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremelyConciseShortVeryExtremelyShortConcise() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely concise short very extremely short concise of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, extremely so, concise, short, very, extremely so, short, and concise.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

            Public Function GetParabolaCompleteSummaryLineSingleConciseShortestExtremelyDetailedComprehensiveFullExtremelyConciseShortVeryExtremelyShortConciseShort() As String
                ' The complete summary line single concise shortest extremely detailed comprehensive full extremely concise short very extremely short concise short of a parabola can be represented as a single line string containing all of its properties and characteristics in the most concise format possible while still being extremely detailed, comprehensive, full, extremely so, concise, short, very, extremely so, short, concise, and short.
                Dim info = Me.GetParabolaInfo()
                Return $"Parabola: y = {info.Coefficients.A}x^2 + {info.Coefficients.B}x + {info.Coefficients.C}, Focus: {info.Focus}, Directrix: y = {info.Directrix}, Rotation: {info.Rotation} radians, Vertex: {info.Vertex}, Axis of Symmetry: x = {info.AxisOfSymmetry}, Type: {info.Type}, Orientation: {info.Orientation}, Direction: {info.Direction}, Concavity: {info.Concavity}, Width: {Me.GetParabolaWidth()} units, Depth: {Me.GetParabolaDepth()} units, Focal Length: {Me.GetFocalLength()} units, Focal Width: {Me.GetFocalWidth()} units."
            End Function

#End Region ' "AI Methods"

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetDirectrix() As System.Double

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                ' The directrix of a parabola can be calculated using the formula:
                ' directrix = vertex_y - (1 / (4 * A)), where vertex_y is the y-coordinate of the vertex
                ' and A is the coefficient of x^2 in the standard form of the parabola.
                '             Dim vertex As D2.Point = Me.GetVertex()
                '             Return vertex.Y - (1 / (4 * Me.StdA))
                Return Me.Vertex.Y - (1 / (4 * Me.StdA))
            End Function

#Region "OSNW Methods"

#End Region ' "OSNW Methods"

#Region "Constructors"

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            Private Sub PushFromNew()

                ' REF: Parabola
                ' https://en.wikipedia.org/wiki/Parabola

                With Me

                    ' Assign the X-intercepts.
                    Dim X0 As System.Double
                    Dim X1 As System.Double
                    If Not TryQuadratic(Me.StdA, Me.StdB, Me.StdC, X0, X1) Then
                        ' Unsolvable conditions, leave the derived values as NaN
                        ' and exit.
                        Return
                    End If
                    Me.m_XIntercept0 = New Math.D2.Point(X0, 0.0)
                    Me.m_XIntercept1 = New Math.D2.Point(X1, 0.0)

                    ' ACCOUNT FOR ROTATION TO SET DERIVED POINT PROPERTIES.
                    '.Sx = ????
                    '.Sy = ????

                End With

            End Sub ' PushFromNew

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            Private Sub PushFromABC()

                ' By definition, all points on a parabola are equidistant from
                ' the focus and the directrix.

                ' Find the vertex and directrix.
                Me.m_Vertex = Me.GetVertex()
                Me.m_Directrix = Me.GetDirectrix()

                ' The distance from the vertex to the directrix is:
                Dim OffsetD As System.Double = Me.Vertex.Y - Me.Directrix

                ' By definition, the vertex is equidistant from the focus and
                ' the directrix. Also by definition, the focus never lies on the
                ' directrix. Therefore, the offset from the vertex to the focus
                ' is the same as the offset from the vertex to the directrix,
                ' but in the opposite direction.
                Dim YOffsetF As System.Double = -OffsetD
                Dim Fy As System.Double = Me.Vertex.Y + YOffsetF

                ' The axis of symmetry is vertical, so the focus has the same
                ' x-coordinate as the vertex.
                Dim Fx As System.Double = Me.Vertex.X

                'Now, assign the focus.
                Me.m_Focus = New D2.Point(Fx, Fy)

                Me.PushFromNew()

            End Sub ' PushFromABC

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            Private Sub PushFromFD()

                '===============================================================

                ' y = Ax^2 + Bx + C WILL ALWAYS BE CENTERED ON x = 0. HOW SHOULD
                ' THIS HANDLE A FOCUS THAT DOES NOT FALL ON THE x=0 SYMMETRY
                ' LINE? MAYBE SHIFT IT OVER TO X=0, THEN SHIFT BACK AFTER THE
                ' CALCULATIONS HAVE BEEN DONE?

                ' Focus (Fx, Fy) and Directrix (Dy) are known from the arguments
                ' to New(focus, directrix, rotation). StdA, StdB, and StdC are
                ' the coefficients to be found.

                ' From GetVertex():
                ' Vx = -StdB / (2 * StdA)
                ' Vy = StdC - (StdB^2 / (4 * StdA))

                ' Because the vertex fits the standard form:
                ' Vy = (StdA * Vx^2) + (StdB * Vx) + StdC

                ' The vertex is always offset from Y=0 by C in the standard
                ' form equation.
                ' StdC = Vy

                ' By definition,
                ' - Any point on the curve is equidistant from the focus and the
                ' directrix.
                ' - The directrix is horizontal.
                ' - The line of symmetry is vertical and passes through both the
                ' focus and the vertex.
                ' For any point,
                '  DistPF = sqrt((Px - Fx)^2 + (Py - Fy)^2)
                '  DistPD = Py - Dy
                '  DistPF = DistPD
                ' Therefore, the vertex is equidistant from the focus
                ' and the directrix.
                '  DistVF = sqrt((Vx - Fx)^2 + (Vy - Fy)^2)
                '  DistVD = Vy - Dy
                '  DistVF = DistVD
                '  sqrt((Vx - Fx)^2 + (Vy - Fy)^2) = Vy - Dy
                ' - Any point on the curve is equidistant from the focus and the
                ' directrix.
                ' - The focus shares the line of symmetry with the vertex.
                ' - The focus never lies on the directrix.
                ' Therefore, the offset from the vertex to the focus is the same
                ' as the offset from the vertex to the directrix, but in the
                ' opposite direction.
                ' Vx = Fx
                ' Vy = (Fy + Dy) / 2

                ' Vx = -StdB / (2 * StdA)
                ' Vy = StdC - (StdB^2 / (4 * StdA))
                ' Vy = (StdA * Vx^2) + (StdB * Vx) + StdC

                '             Me.CoeffA = ????
                '             Me.CoeffB = ????
                '             Me.CoeffC = ????

                '===============================================================





                '===============================================================










                '===============================================================

                Me.PushFromNew()

            End Sub ' PushFromFD

            ''' <summary>
            ''' Default contructor.
            ''' Initializes a new instance of the <c>Parabola</c> class with all
            ''' properties initially set to <see cref="System.Double.NaN"/> or
            ''' default values, as appropriate.
            ''' </summary>
            Public Sub New()
                With Me

                    ' Set all properties to NaN or default values, as
                    ' appropriate.

                    ' Persistent assigned properties.
                    ' From New(Double, Double, Double, Double)
                    .m_StdA = System.Double.NaN
                    .m_StdB = System.Double.NaN
                    .m_StdC = System.Double.NaN
                    ' From New(OSNW.Math.D2.Point, Double, Double)
                    .m_Focus = New Math.D2.Point(
                        System.Double.NaN, System.Double.NaN)
                    .m_Directrix = System.Double.NaN
                    ' From both, whichever was used.
                    .m_Rotation = System.Double.NaN
                    '
                    '
                    '

                    ' Other properties.
                    '                 .Sx = System.Double.NaN ' Symmetry.
                    '                 .Sy = System.Double.NaN ' Symmetry.
                    '                 .X0 = System.Double.NaN ' X-intercept.
                    '                 .X1 = System.Double.NaN ' X-intercept.
                    '                 .Vertex = New Math.D2.Point(System.Double.NaN,
                    '                                             System.Double.NaN)
                    '
                    '
                    '

                End With
            End Sub ' New

            ''' <summary>
            ''' Initializes a new instance of the <c>Parabola</c> class with the
            ''' specified coefficients <paramref name="a"/>,
            ''' <paramref name="b"/>, and <paramref name="c"/>, along with the
            ''' specified angle of <paramref name="rotation"/>.
            ''' </summary>
            ''' <param name="a">xxxxxxxxxx</param>
            ''' <param name="b">xxxxxxxxxx</param>
            ''' <param name="c">xxxxxxxxxx</param>
            ''' <param name="rotation">
            ''' xxxxxxxxxx
            ''' RADIANS, where 0 radians corresponds to a parabola that opens upwards, and positive values correspond to counterclockwise rotation.
            ''' xxxxxxxxxx
            ''' </param>
            ''' <remarks>
            ''' No exceptions are thrown, but properties will be left at their
            ''' default value when any argument is infinite or any argument is
            ''' <c>System.Double.NaN</c>.
            ''' </remarks>
            Public Sub New(ByVal a As System.Double, ByVal b As System.Double,
                ByVal c As System.Double, ByVal rotation As System.Double)

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                Me.New()

                ' Input checking.
                If System.Double.IsInfinity(a) OrElse
                    System.Double.IsInfinity(b) OrElse
                    System.Double.IsInfinity(c) OrElse
                    System.Double.IsInfinity(rotation) OrElse
                    System.Double.IsNaN(a) OrElse
                    System.Double.IsNaN(b) OrElse
                    System.Double.IsNaN(c) OrElse
                    System.Double.IsNaN(rotation) Then

                    ' Leave default values in place.
                    Exit Sub
                End If

                With Me

                    ' Take the provided values.
                    .m_StdA = a
                    .m_StdB = b
                    .m_StdC = c
                    .m_Rotation = rotation

                    .PushFromABC()

                End With

            End Sub ' New

            ''' <summary>
            ''' Initializes a new instance of the <c>Parabola</c> class with the
            ''' specified <paramref name="focus"/>,
            ''' <paramref name="directrix"/>, and <paramref name="rotation"/>.
            ''' </summary>
            ''' <param name="focus">xxxxxxxxxx</param>
            ''' <param name="directrix">xxxxxxxxxx</param>
            ''' <param name="rotation">xxxxxxxxxx</param>
            ''' <remarks>
            ''' No exceptions are thrown, but properties will be left at their
            ''' default value when any argument is infinite, any argument is
            ''' <c>System.Double.NaN</c>, or <paramref name="focus"/> is located
            ''' on <paramref name="directrix"/>.
            ''' </remarks>
            Public Sub New(ByVal focus As OSNW.Math.D2.Point,
                           ByVal directrix As System.Double,
                           ByVal rotation As System.Double)

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                Me.New()

                ' Input checking.
                If System.Double.IsInfinity(focus.X) OrElse
                    System.Double.IsInfinity(focus.Y) OrElse
                    System.Double.IsInfinity(directrix) OrElse
                    System.Double.IsInfinity(rotation) OrElse
                    System.Double.IsNaN(focus.X) OrElse
                    System.Double.IsNaN(focus.Y) OrElse
                    System.Double.IsNaN(directrix) OrElse
                    System.Double.IsNaN(rotation) OrElse
                    focus.Y.Equals(directrix) Then

                    ' Leave default values in place.
                    Exit Sub
                End If

                With Me

                    ' Take the provided values.
                    .m_Focus = New Math.D2.Point(System.Double.NaN, System.Double.NaN)
                    .m_Directrix = System.Double.NaN
                    .m_Rotation = System.Double.NaN

                    .PushFromFD()

                End With

            End Sub ' New

#End Region ' "Constructors"

        End Class ' Parabola

        '     ''' <summary>
        '     ''' xxxxxxxxxx
        '     ''' </summary>
        '     Public Class Parabola

        '         ' In standard form, the equation of a parabola is Y = aX^2 + bX + c,
        '         ' where a, b, and c are real numbers and "a" is non-zero.

        '         ' A parabola is a conic section that is generated by the
        '         ' intersection of the surface of a plane with a cone. A parabola is
        '         ' a plane curve that is formed when a point moves such that the
        '         ' distance between it and a fixed point equals the distance between
        '         ' it and a fixed line in the Cartesian plane. The fixed point is the
        '         ' focus of the parabola, and the fixed line is the directrix of the
        '         ' parabola.

        '#Region "Persistent Properties"

        '         ' These are properties whose value does not change with rotation.

        '#Region "Persistent Assigned Properties"

        '         ' These properties are read-only and set by New(). Only these
        '         ' properties should be included in serialization, with the other
        '         ' properties being derived from them in New().

        '         ' StdA Property.
        '         Private m_StdA As System.Double
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Public Property StdA As System.Double
        '             Get
        '                 Return Me.m_StdA
        '             End Get
        '             Private Set
        '                 Me.m_StdA = Value
        '             End Set
        '         End Property

        '         ' StdB Property.
        '         Private m_StdB As System.Double
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Public Property StdB As System.Double
        '             Get
        '                 Return Me.m_StdB
        '             End Get
        '             Private Set
        '                 Me.m_StdB = Value
        '             End Set
        '         End Property

        '         ' StdC Property.
        '         Private m_StdC As System.Double
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Public Property StdC As System.Double
        '             Get
        '                 Return Me.m_StdC
        '             End Get
        '             Private Set
        '                 Me.m_StdC = Value
        '             End Set
        '         End Property

        '         ' Focus Property.
        '         Private m_Focus As Math.D2.Point
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Public Property Focus As Math.D2.Point
        '             Get
        '                 Return Me.m_Focus
        '             End Get
        '             Private Set
        '                 Me.m_Focus = Value
        '             End Set
        '         End Property

        '         ' D Property.
        '         ' RENAME LATER: D IS NOT A DESCRIPTIVE NAME. CONSIDER "DIRECTRIXY"
        '         ' OR "DIRECTRIXDISTANCE" OR SOMETHING MORE DESCRIPTIVE.
        '         Private m_D As System.Double
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' Angle in RADIANS.
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Public Property D As System.Double
        '             Get
        '                 Return Me.m_D
        '             End Get
        '             Private Set
        '                 Me.m_D = Value
        '             End Set
        '         End Property

        '         ' Rotation Property.
        '         Private m_Rotation As System.Double
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' Angle in RADIANS.
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Public Property Rotation As System.Double
        '             Get
        '                 Return Me.m_Rotation
        '             End Get
        '             Private Set
        '                 Me.m_Rotation = Value
        '             End Set
        '         End Property

        '#End Region ' "Persistent Assigned Properties"

        '#Region "Persistent Derived Properties"

        '         ' These properties should be excluded from serialization, with their
        '         ' values being derived in New().

        '         '' RENAME LATER.
        '         '''' <summary>
        '         '''' xxxxxxxxxx
        '         '''' </summary>
        '         '''' <returns>xxxxxxxxxx</returns>
        '         'Property X0 As System.Double

        '         '' RENAME LATER.
        '         '''' <summary>
        '         '''' xxxxxxxxxx
        '         '''' </summary>
        '         '''' <returns>xxxxxxxxxx</returns>
        '         'Property X1 As System.Double

        '         '''' <summary>
        '         '''' xxxxxxxxxx
        '         '''' </summary>
        '         '''' <returns>xxxxxxxxxx</returns>
        '         'Property Vertex As Math.D2.Point

        '         '' RENAME LATER.
        '         '''' <summary>
        '         '''' Returns the x-value of the axis of symmetry.
        '         '''' </summary>
        '         '''' <returns>The x-value of the axis of symmetry.</returns>
        '         'Property Sx As System.Double

        '         '' RENAME LATER.
        '         '''' <summary>
        '         '''' Returns the y-value where X=Sx
        '         '''' </summary>
        '         '''' <returns>The y-value where X=Sx</returns>
        '         'Property Sy As System.Double

        '#End Region ' "Persistent Derived Properties"

        '#End Region ' "Persistent Properties"

        '#Region "Rotatable Properties"

        '         ' These properties should be excluded from serialization, with their
        '         ' values being derived in New().

        '         ' RENAME LATER.
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Property X0 As System.Double

        '         ' RENAME LATER.
        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Property X1 As System.Double

        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <returns>xxxxxxxxxx</returns>
        '         Property Vertex As Math.D2.Point

        '         ' RENAME LATER.
        '         ''' <summary>
        '         ''' Returns the x-value of the axis of symmetry.
        '         ''' </summary>
        '         ''' <returns>The x-value of the axis of symmetry.</returns>
        '         Property Sx As System.Double

        '         ' RENAME LATER.
        '         ''' <summary>
        '         ''' Returns the y-value where X=Sx
        '         ''' </summary>
        '         ''' <returns>The y-value where X=Sx</returns>
        '         Property Sy As System.Double

        '         '
        '         '
        '         '
        '         '
        '         '

        '#End Region ' "Rotatable Properties"

        '#Region "Constructors"

        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         Private Sub PushFromNew()

        '             ' REF: Parabola
        '             ' https://en.wikipedia.org/wiki/Parabola

        '             With Me


        '                 ' ACCOUNT FOR ROTATION TO SET DERIVED POINT PROPERTIES.
        '                 '.Sx = ????
        '                 '.Sy = ????

        '             End With

        '         End Sub ' PushFromNew

        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         Private Sub PushFromABC()

        '             ' Assign the X-intercepts.
        '             If Not TryQuadratic(Me.StdA, Me.StdB, Me.StdC, Me.X0, Me.X1) Then
        '                 ' Unsolvable conditions, leave the derived values as NaN
        '                 ' and exit.
        '                 Return
        '             End If

        '             ' Prior to rotation, the vertex will be at the point where the slope of the parabola is zero.
        '             ' The slope of the parabola at any point can be calculated using the formula Slope = 2*a*X + b,
        '             ' which is the derivative of the parabola's equation with respect to X.
        '             ' The Y value can then be calculated based on the X value.
        '             ' 2*a*X + b = 0
        '             ' 2*a*X = -b
        '             ' X = -b / (2*a)
        '             ' Y = a*X^2 + b*X + c)

        '             Dim Vx As System.Double = -Me.StdB / (2 * Me.StdA)
        '             Dim Vy As System.Double = Me.StdA * Vx * Vx + Me.StdB * Vx + Me.StdC
        '             Me.Vertex = New Math.D2.Point(Vx, Vy)



        '             ' THERE ARE NOW THREE KNOWN POINTS ON THE PARABOLA: THE VERTEX AND THE TWO X-INTERCEPTS.
        '             ' EACH OF THOSE POINTS MUST SATISFY THE STANDARD EQUATION OF THE PARABOLA AND THE DISTANCES
        '             ' TO THE FOCUS AND TO THE TO THE DIRECTRIX.

        '             ' The distance from the focus to the vertex must equal the distance from the directrix to the
        '             ' vertex.
        '             ' Df = sqrt((Fx - Vx)^2 + (Fy - Vy)^2)
        '             ' Dd = abs(Me.D - Vy)

        '             ' Df = Dd
        '             ' sqrt((Fx - Vx)^2 + (Fy - Vy)^2) = abs(D - Vy)

        '             ' Square both sides.
        '             ' (Fx - Vx)^2 + (Fy - Vy)^2 = abs(D - Vy)^2

        '             ' Fx will match Vx, so the equation can be simplified to:
        '             ' (Vx - Vx)^2 + (Fy - Vy)^2 = abs(D - Vy)^2
        '             ' 0^2 + (Fy - Vy)^2 = abs(D - Vy)^2
        '             ' (Fy - Vy)^2 = abs(D - Vy)^2

        '             ' Take the square root of both sides.
        '             ' Fy - Vy = abs(D - Vy)
        '             ' Fy = abs(D - Vy) + Vy





















        '             Dim DistToFocus As System.Double =
        '                 System.Math.Sqrt((Vx - Me.Focus.X) ^ 2 + (Vy - Me.Focus.Y) ^ 2)
        '             Dim DistToD As System.Double = System.Math.Abs(Vy - Me.D)












        '             'xxxx

        '             '
        '             '
        '             '
        '             '
        '             '

        '             Me.PushFromNew()

        '         End Sub ' PushFromABC

        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         Private Sub PushFromFD()

        '             '             Me.CoeffA = ????
        '             '             Me.CoeffB = ????
        '             '             Me.CoeffC = ????



        '             ' Text generated by Visual Studio AI:
        '             ' A paraola can be defined by its focus and D, but the coefficients of the standard
        '             ' form of the parabola are not directly derived from the focus and D. Instead, they
        '             ' are derived from the vertex form of the parabola, which is given by the equation:
        '             ' y = a(x - h)^2 + k, where (h, k) is the vertex of the parabola and a is a coefficient that
        '             ' determines the width and direction of the parabola. To derive the coefficients a, b, and c
        '             ' from the focus and D, you can follow these steps:
        '             ' ??????????????????????????????



        '             ' Also generated by Visual Studio AI:
        '             ' The vertex of the parabola is the midpoint between the focus and the D. 
        '             ' The distance from the vertex to the focus is equal to the distance from the vertex to the
        '             ' D. 
        '             ' The coefficient a can be calculated using the distance from the vertex to the focus
        '             ' (or D) and the formula a = 1/(4*p), where p is the distance from the vertex to the
        '             ' focus (or D). 



        '             Me.PushFromNew()

        '         End Sub ' PushFromFD

        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' Default contructor.
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         Public Sub New()
        '             With Me

        '                 .m_StdC = System.Double.NaN
        '                 .m_StdC = System.Double.NaN
        '                 .m_StdC = System.Double.NaN
        '                 .m_D = System.Double.NaN
        '                 .m_Focus = New Math.D2.Point(
        '                     System.Double.NaN, System.Double.NaN)
        '                 .m_Rotation = System.Double.NaN
        '                 '
        '                 '
        '                 '

        '                 .Sx = System.Double.NaN
        '                 .Sy = System.Double.NaN
        '                 .X0 = System.Double.NaN
        '                 .X1 = System.Double.NaN
        '                 .Vertex = New Math.D2.Point(System.Double.NaN,
        '                                             System.Double.NaN)
        '                 '
        '                 '
        '                 '

        '             End With
        '         End Sub ' New

        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <param name="a">xxxxxxxxxx</param>
        '         ''' <param name="b">xxxxxxxxxx</param>
        '         ''' <param name="c">xxxxxxxxxx</param>
        '         ''' <param name="rotation">xxxxxxxxxx</param>
        '         Public Sub New(ByVal a As System.Double, ByVal b As System.Double,
        '             ByVal c As System.Double, ByVal rotation As System.Double)

        '             Me.New()

        '             With Me

        '                 ' Take the provided values.
        '                 .m_StdA = a
        '                 .m_StdB = b
        '                 .m_StdC = c
        '                 .m_Rotation = rotation

        '                 .PushFromABC()

        '             End With

        '         End Sub ' New

        '         ''' <summary>
        '         ''' xxxxxxxxxx
        '         ''' </summary>
        '         ''' <param name="focus">xxxxxxxxxx</param>
        '         ''' <param name="D">xxxxxxxxxx</param>
        '         ''' <param name="rotation">xxxxxxxxxx</param>
        '         Public Sub New(ByVal focus As Math.D2.Point,
        '                        ByVal D As System.Double,
        '                        ByVal rotation As System.Double)

        '             Me.New()

        '             '' Input checking.
        '             'If width <= 0 Then
        '             ' 'Dim CaughtBy As System.Reflection.MethodBase =
        '             ' ' System.Reflection.MethodBase.GetCurrentMethod
        '             ' Throw New System.ArgumentOutOfRangeException(NameOf(width), MSGVMBGTZ)
        '             'End If
        '             'If height <= 0 Then
        '             ' 'Dim CaughtBy As System.Reflection.MethodBase =
        '             ' ' System.Reflection.MethodBase.GetCurrentMethod
        '             ' Throw New System.ArgumentOutOfRangeException(NameOf(height), MSGVMBGTZ)
        '             'End If

        '             With Me

        '                 ' Take the provided values.
        '                 .m_Focus = New Math.D2.Point(System.Double.NaN, System.Double.NaN)
        '                 .m_D = System.Double.NaN
        '                 .m_Rotation = System.Double.NaN

        '                 .PushFromFD()

        '             End With

        '         End Sub ' New

        '#End Region ' "Constructors"

        '     End Class ' Parabola


    End Structure ' Math2D

End Module ' Math
