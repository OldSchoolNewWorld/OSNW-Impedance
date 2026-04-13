Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Partial Public Module Math

    ' REF: Standard Form of a Line
    ' https://www.geeksforgeeks.org/maths/standard-form-of-a-straight-line/

    ' REF: Standard Form of a Line
    ' https://www.geeksforgeeks.org/maths/standard-form-of-a-straight-line/

    ' The standard form of a linear equation is aX + bY + c = 0.
    ' where:
    '   A, B, and C are integers
    '   xxxxxxxxxx "MANDATORY" OR "SHOULD BE" INTEGERS? xxxxxxxxxx
    '   A and B cannot be zero simultaneously
    '
    ' Coefficients: A, B, and C should be integers.
    ' Non-negativity: Typically, A is non-negative. If A is negative, the entire equation can be multiplied by -1 to make it positive.
    ' Graphing: It is useful for finding x and y intercepts easily.
    ' Conversion: Can be converted to slope-intercept form y=mx+b for further analysis.

    ' When A=0 and B=0, aX + bY + c = 0 becomes c = 0, which is invalid (except
    '   when c=0).
    ' Otherwise,
    '   When A=0, aX + bY + c = 0 becomes Y = -c/b, which is a horizontal line.
    '   When B=0, aX + bY + c = 0 becomes X = -c/a, which is a vertical line.

    ' When X=0 (or a=0):
    ' Y = -c / b ' Horizontal.
    ' Y-intercept: (0, -c/b)

    ' When Y=0 (or b=0):
    ' X = -c / a ' Vertical.
    ' X-intercept (-c/a, 0)

    ' Reworks of the standard form:
    ' Y = -(aX + c) / b
    ' Y = -(a/b)X - (c/b)

    Partial Public Structure D2

        ''' <summary>
        ''' A base class that represents the geometry of a generic line, for use
        ''' on a Cartesian plane. Dimensions are in generic "units".
        ''' </summary>
        Public Class Line

#Region "Fields and Properties"

            ' These properties are read-only and set by New(). They should not
            ' be rotated, instead using D2.Point.RotateNormalRad or
            ' D2.Point.RotateNormalDeg to obtain rotated positions of persistent
            ' points.
            ' Only StdA, StdB, StdC, and Rotation should be included in
            ' serialization, with the other persistent properties being derived
            ' from them in New() and calculated properties being generated as
            ' needed.

            ' DOES THIS ACTUALLY ***HAVE TO*** BE AN INTEGER?
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

                    ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                    ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                    ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                    Me.m_StdA = Value
                End Set
            End Property

            ' DOES THIS ACTUALLY ***HAVE TO*** BE AN INTEGER?
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

                    ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                    ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                    ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                    Me.m_StdB = Value
                End Set
            End Property

            ' DOES THIS ACTUALLY ***HAVE TO*** BE AN INTEGER?
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

                    ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                    ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                    ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                    Me.m_StdC = Value
                End Set
            End Property

#End Region ' "Fields and Properties"

#Region "Methods"

#Region "Static/Shared Methods"

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="a">xxxxxxxxxx</param>
            ''' <param name="b">xxxxxxxxxx</param>
            ''' <param name="c">xxxxxxxxxx</param>
            ''' <param name="slope">xxxxxxxxxx</param>
            ''' <param name="yInt">xxxxxxxxxx</param>
            ''' <remarks>
            ''' When <paramref name="a"/>=0, aX + bY + c = 0 becomes Y = -c/b,
            ''' which is a horizontal line having both a zero slope and no
            ''' X-intercept.
            ''' <br/>
            ''' When <paramref name="b"/>=0, aX + bY + c = 0 becomes X = -c/a,
            ''' which is a vertical line having both an infinite slope and no
            ''' Y-intercept. When <paramref name="b"/>=0, both
            ''' <paramref name="slope"/> and <paramref name="yInt"/> will return
            ''' <see cref="System.Double.NaN"/> 
            ''' </remarks>
            Public Shared Sub GetSlopeIntFromABC(ByVal a As System.Double,
                ByVal b As System.Double, ByVal c As System.Double,
                ByRef slope As System.Double, ByRef yInt As System.Double)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' Solve the standard form of a linear equation for Y:
                ' aX + bY + c = 0
                ' b*Y = -(a*X) - c
                ' Y = (-(a*X) - c) / b
                ' Y = (-(a*X)/b) - (c/b)
                ' Y = (-(a/b)*X) - (c/b)

                ' Map the values into the Y = mX + b slope-intercept equation
                ' for a line.
                ' NOTE: Unfortunately, "b" as a coefficient of the standard form
                ' and "b" as the Y-intercept in the slope-intercept form are two
                ' different uses of "b".
                slope = -a / b
                yInt = -c / b

            End Sub ' GetSlopeIntFromABC

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="x0">xxxxxxxxxx</param>
            ''' <param name="y0">xxxxxxxxxx</param>
            ''' <param name="x1">xxxxxxxxxx</param>
            ''' <param name="y1">xxxxxxxxxx</param>
            ''' <param name="m">xxxxxxxxxx</param>
            ''' <param name="b">xxxxxxxxxx</param>
            Public Shared Sub GetSlopeIntFromTwoPoints(ByVal x0 As System.Double,
            ByVal y0 As System.Double, ByVal x1 As System.Double,
            ByVal y1 As System.Double, ByRef m As System.Double,
            ByRef b As System.Double)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' No input checking.

                ' Get the equation for the line.
                ' Y = M*X + B; Standard form line.
                ' B = Y - M*X; Solve for the Y-intercept.
                m = D2.Line.GetSlopeFromTwoPoints(x0, x1, y0, y1)
                b = y0 - m * x0
            End Sub ' GetSlopeIntFromTwoPoints

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="a">xxxxxxxxxx</param>
            ''' <param name="b">xxxxxxxxxx</param>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <remarks>
            ''' When <paramref name="a"/>=0, aX + bY + c = 0 becomes Y = -c/b,
            ''' which is a horizontal line having both a zero slope and no
            ''' X-intercept.
            ''' <br/>
            ''' When <paramref name="b"/>=0, aX + bY + c = 0 becomes X = -c/a,
            ''' which is a vertical line having both an infinite slope and no
            ''' Y-intercept. When <paramref name="b"/>=0, <c>GetSlopeFromABC</c>
            ''' will return <see cref="System.Double.NaN"/> 
            ''' <br/>
            ''' The "c" value in the standard form of a linear equation is not
            ''' needed here.
            ''' </remarks>
            Public Shared Function GetSlopeFromABC(ByVal a As System.Double,
                ByVal b As System.Double) As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' From GetSlopeIntFromABC():
                Return -a / b
            End Function ' GetSlopeFromABC

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="b">xxxxxxxxxx</param>
            ''' <param name="c">xxxxxxxxxx</param>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <remarks>
            ''' <br/>
            ''' When <paramref name="b"/>=0, aX + bY + c = 0 becomes X = -c/a,
            ''' which is a vertical line having both an infinite slope and no
            ''' Y-intercept. When <paramref name="b"/>=0, <c>GetYIntFromABC</c>
            ''' will return <see cref="System.Double.NaN"/> 
            ''' <br/>
            ''' The "a" value in the standard form of a linear equation is not
            ''' needed here.
            ''' </remarks>
            Public Shared Function GetYIntFromABC(ByVal b As System.Double,
                ByVal c As System.Double) As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' From GetSlopeIntFromABC():
                Return -c / b
            End Function ' GetYIntFromABC

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="a">xxxxxxxxxx</param>
            ''' <param name="c">xxxxxxxxxx</param>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <remarks>
            ''' When <paramref name="a"/>=0, aX + bY + c = 0 becomes Y = -c/b,
            ''' which is a horizontal line having both a zero slope and no
            ''' X-intercept. When <paramref name="a"/>=0, <c>GetXIntFromABC</c>
            ''' will return <see cref="System.Double.NaN"/> 
            ''' <br/>
            ''' The "b" value in the standard form of a linear equation is not
            ''' needed here.
            ''' </remarks>
            Public Shared Function GetXIntFromABC(ByVal a As System.Double,
                ByVal c As System.Double) As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' The standard form of a linear equation is aX + bY + c = 0.
                ' Solve for X when Y=0.
                ' aX + c = 0
                ' aX = -c/a

                Return -c / a

            End Function ' GetXIntFromABC

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="slope">xxxxxxxxxx</param>
            ''' <param name="yInt">xxxxxxxxxx</param>
            ''' <param name="a">xxxxxxxxxx</param>
            ''' <param name="b">xxxxxxxxxx</param>
            ''' <param name="c">xxxxxxxxxx</param>
            Public Shared Sub GetABCFromSlopeInt(ByVal slope As System.Double,
                ByVal yInt As System.Double, ByRef a As System.Double,
                ByRef b As System.Double, ByRef c As System.Double)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' The slope-intercept equation for a line is Y = mX + b.
                ' Rearrange.
                ' -mX + Y - b = 0
                ' mX - Y + b = 0

                ' The standard form of a linear equation is aX + bY + c = 0.
                ' Map the values.
                a = slope
                b = -1.0
                c = yInt

            End Sub ' GetABCFromSlopeInt

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="point">xxxxxxxxxx</param>
            ''' <param name="slope">xxxxxxxxxx</param>
            ''' <param name="a">xxxxxxxxxx</param>
            ''' <param name="b">xxxxxxxxxx</param>
            ''' <param name="c">xxxxxxxxxx</param>
            Public Shared Sub GetABCFromPointSlope(ByVal point As D2.Point,
                ByVal slope As System.Double, ByRef a As System.Double,
                ByRef b As System.Double, ByRef c As System.Double)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' The slope-intercept equation for a line is Y = mX + b.
                ' point.Y = (slope * point.X) + YInt
                ' point.Y - (slope * point.X) = YInt
                Dim YInt As System.Double = point.Y - (slope * point.X)

                Line.GetABCFromSlopeInt(slope, YInt, a, b, c)

            End Sub ' GetABCFromPointSlope

            ''' <summary>
            ''' xxxxxxx
            ''' </summary>
            ''' <param name="point0">xxxxxxx</param>
            ''' <param name="point1">xxxxxxx</param>
            ''' <param name="a">xxxxxxx</param>
            ''' <param name="b">xxxxxxx</param>
            ''' <param name="c">xxxxxxx</param>
            Public Shared Sub GetABCFromTwoPoints(ByVal point0 As D2.Point,
                   ByVal point1 As D2.Point, ByRef a As System.Double,
                   ByRef b As System.Double, ByRef c As System.Double)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                Dim DeltaY As System.Double = point1.Y - point0.Y
                Dim DeltaX As System.Double = point1.X - point0.X
                Dim Slope As System.Double = DeltaY / DeltaX
                Line.GetABCFromPointSlope(point0, Slope, a, b, c)
            End Sub ' GetABCFromTwoPoints

#End Region ' "Static/Shared Methods"

#Region "Instance Methods"

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetSlope() As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                Return Line.GetSlopeFromABC(Me.StdA, Me.StdB)

            End Function ' GetSlope

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function GetYIntercept() As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                Return Line.GetYIntFromABC(Me.StdB, Me.StdC)

            End Function ' GetYIntercept

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="x">xxxxxxxxxx</param>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function YatX(ByVal x As System.Double) As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' The standard form of a linear equation is aX + bY + c = 0.
                ' bY = -aX - c
                ' Y = (-aX - c) / b
                Return (-Me.StdA * x) / Me.StdB
            End Function ' YatX

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="y">xxxxxxxxxx</param>
            ''' <returns>xxxxxxxxxx</returns>
            Public Function XatY(ByVal y As System.Double) As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' The standard form of a linear equation is aX + bY + c = 0.
                ' aX = - bY - c
                ' X = (- bY - c) / a
                ' X = -(bY + c) / a
                Return -(Me.StdB * y + Me.StdC) / Me.StdA
            End Function ' XatY

#End Region ' "Instance Methods"

#End Region ' "Methods"

#Region "Constructors"

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            Public Sub New()

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                Me.m_StdA = Double.NaN
                Me.m_StdB = Double.NaN
                Me.m_StdC = Double.NaN
            End Sub ' New

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="StdA">xxxxxxxxxx</param>
            ''' <param name="StdB">xxxxxxxxxx</param>
            ''' <param name="StdC">xxxxxxxxxx</param>
            Public Sub New(ByVal stdA As System.Double,
                ByVal stdB As System.Double, ByVal stdC As System.Double)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                Me.New()

                ' Input checking.
                If System.Double.IsNaN(stdA) OrElse
                    System.Double.IsNaN(stdB) OrElse
                    System.Double.IsNaN(stdC) Then

                    ' Leave default values in place.
                    Exit Sub
                End If
                If stdA.Equals(0.0) AndAlso stdB.Equals(0.0) Then
                    ' When A=0 and B=0, aX + bY + c = 0 becomes c = 0
                    ' That is invalid (except when c=0).
                    ' Leave default values in place.
                    Exit Sub
                End If

                ' On getting here,
                If stdA < 0.0 Then
                    With Me
                        ' Normalize the appearance; negate both sides of the
                        ' standard form, which negates the provided values.
                        Me.m_StdA = -stdA
                        Me.m_StdB = -stdB
                        Me.m_StdC = -stdC
                    End With
                Else
                    With Me
                        ' Take the provided values.
                        Me.m_StdA = stdA
                        Me.m_StdB = stdB
                        Me.m_StdC = stdC
                    End With
                End If

            End Sub ' New            

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="slope">xxxxxxxxxx</param>
            ''' <param name="yIntercept">xxxxxxxxxx</param>
            Public Sub New(ByVal slope As System.Double,
                           ByVal yIntercept As System.Double)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                Me.New()

                ' Input checking.
                If System.Double.IsNaN(slope) OrElse
                    System.Double.IsNaN(yIntercept) Then

                    ' Leave default values in place.
                    Exit Sub
                End If

                ' On getting here,
                With Me
                    ' Use the provided values.
                    Line.GetABCFromSlopeInt(
                        slope, yIntercept, Me.m_StdA, Me.m_StdB, Me.m_StdC)
                End With

            End Sub ' New

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="point">xxxxxxxxxx</param>
            ''' <param name="slope">xxxxxxxxxx</param>
            Public Sub New(ByVal point As D2.Point,
                           ByVal slope As System.Double)


                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx
                ' xxxxxxxxxx TAKE ANOTHER LOOK AT ZEROES AND INFINITIES. xxxxxxxxxx

                Me.New()

                ' Input checking.
                If System.Double.IsNaN(point.X) OrElse
                    System.Double.IsNaN(point.Y) OrElse
                    System.Double.IsNaN(slope) Then

                    ' Leave default values in place.
                    Exit Sub
                End If

                With Me
                    Line.GetABCFromPointSlope(
                        point, slope, Me.m_StdA, Me.m_StdC, Me.m_StdC)
                End With

            End Sub ' New

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="point0">xxxxxxxxxx</param>
            ''' <param name="point1">xxxxxxxxxx</param>
            Public Sub New(ByVal point0 As D2.Point, ByVal point1 As D2.Point)


                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' xxxxxxxxxx TAKE ANOTHER LOOK AT ZEROES AND INFINITIES. xxxxxxxxxx

                Me.New()

                ' Input checking.
                If System.Double.IsInfinity(point0.X) OrElse
                    System.Double.IsInfinity(point0.Y) OrElse
                    System.Double.IsInfinity(point1.X) OrElse
                    System.Double.IsInfinity(point1.Y) OrElse
                    System.Double.IsNaN(point0.X) OrElse
                    System.Double.IsNaN(point0.Y) OrElse
                    System.Double.IsNaN(point1.X) OrElse
                    System.Double.IsNaN(point1.Y) Then

                    ' Leave default values in place.
                    Exit Sub
                End If

                With Me
                    ' Use the provided values.
                    Line.GetABCFromTwoPoints(point0, point1,
                                             Me.m_StdA, Me.m_StdB, Me.m_StdC)
                End With

            End Sub ' New

#End Region ' "Constructors"

            ''' <summary>
            ''' Returns the slope of a line passing through two specified points.
            ''' </summary>
            ''' <param name="x0">Specifies the X-coordinate of one point.</param>
            ''' <param name="y0">Specifies the Y-coordinate of one point.</param>
            ''' <param name="x1">Specifies the X-coordinate of the other
            ''' point.</param>
            ''' <param name="y1">Specifies the Y-coordinate of the other
            ''' point.</param>
            ''' <returns>The slope of a line passing through the specified
            ''' points.</returns>
            Public Shared Function GetSlopeFromTwoPoints(
                ByVal x0 As System.Double, ByVal y0 As System.Double,
                ByVal x1 As System.Double, ByVal y1 As System.Double) _
                As System.Double

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                ' No input checking.
                Return (y1 - y0) / (x1 - x0)
            End Function ' GetSlopeFromTwoPoints

        End Class ' Line

    End Structure ' D2

End Module ' Math
