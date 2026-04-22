Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Partial Public Module Math

    Partial Public Structure D2

        ''' <summary>
        ''' Represents an ordered pair of double precision X- and Y-coordinates
        ''' that define a point in a two-dimensional plane. Dimensions are in
        ''' generic "units".
        ''' </summary>
        Public Class Point

            Public Const DFLTX As System.Double = 0.0
            Public Const DFLTY As System.Double = 0.0

#Region "Fields and Properties"

            Private m_X As System.Double
            ''' <summary>
            ''' Represents the X-coordinate of this <c>D2.Point</c>.
            ''' </summary>
            ''' <remarks>
            ''' Assignment of <see cref="System.Double.NaN"/> or an infinite
            ''' value is allowed, but may cause unexpected results. Calling
            ''' routines might need special handling where those values are
            ''' valid. For example, the Y=mX+b formula would result in an
            ''' infinite Y when a vertical line has an infinite slope.
            ''' </remarks>
            Public Property X As System.Double
                Get
                    Return Me.m_X
                End Get
                Private Set

                    '' Suspend to avoid exceptions:
                    '' Input checking.
                    'If System.Double.IsNaN(Value) OrElse
                    '    System.Double.IsInfinity(Value) Then
                    '    Dim CaughtBy As System.Reflection.MethodBase =
                    '        System.Reflection.MethodBase.GetCurrentMethod
                    '    Throw New System.ArgumentOutOfRangeException(
                    '        $"Arguments to {CaughtBy} {MSGMHUV}")
                    'End If

                    Me.m_X = Value

                End Set
            End Property

            Private m_Y As System.Double
            ''' <summary>
            ''' Represents the Y-coordinate of this <c>D2.Point</c>.
            ''' </summary>
            ''' <remarks> See <see cref="X"/> regarding
            ''' <see cref="System.Double.NaN"/> or infinite values.</remarks>
            Public Property Y As System.Double
                Get
                    Return Me.m_Y
                End Get
                Private Set

                    '' Suspend to avoid exceptions:
                    '' Input checking.
                    'If System.Double.IsNaN(Value) OrElse
                    '    System.Double.IsInfinity(Value) Then
                    '    Dim CaughtBy As System.Reflection.MethodBase =
                    '        System.Reflection.MethodBase.GetCurrentMethod
                    '    Throw New System.ArgumentOutOfRangeException(
                    '        $"Arguments to {CaughtBy} {MSGMHUV}")
                    'End If

                    Me.m_Y = Value

                End Set
            End Property

#End Region ' "Fields and Properties"

#Region "Methods"

#Region "Distance"

            ''' <summary>
            ''' Returns the distance between two points, specified by their X-
            ''' and Y-coordinates, in a 2D plane.
            ''' </summary>
            ''' <param name="x0">Specifies the X-coordinate of one
            ''' point.</param>
            ''' <param name="y0">Specifies the Y-coordinate of one
            ''' point.</param>
            ''' <param name="x1">Specifies the X-coordinate of the other
            ''' point.</param>
            ''' <param name="y1">Specifies the Y-coordinate of the other
            ''' point.</param>
            ''' <returns>The distance between the two points.</returns>
            ''' <remarks>
            ''' See <see cref="X"/> regarding <see cref="System.Double.NaN"/> or
            ''' infinite values.
            ''' <br/>
            ''' <see cref="D2.point.Distance(System.Double, System.Double,
            ''' System.Double, System.Double)"/>,
            ''' <see cref="D2.point.Distance(D2.Point, D2.Point)"/>, and
            ''' <see cref="D2.Point.Distance(D2.Point)"/> are effectively the
            ''' same thing. Use whichever version best suits the variables at
            ''' hand.
            ''' </remarks>
            Public Shared Function Distance(ByVal x0 As System.Double,
                ByVal y0 As System.Double, ByVal x1 As System.Double,
                ByVal y1 As System.Double) As System.Double

                ' No input checking.

                ' This is just the Pythagorean theorem, using multiplication vs.
                ' squares.
                Dim DeltaX As System.Double = x1 - x0
                Dim DeltaY As System.Double = y1 - y0
                Return System.Math.Sqrt((DeltaX * DeltaX) + (DeltaY * DeltaY))

            End Function ' Distance

            ''' <summary>
            ''' Returns the distance between two <c>D2.Point</c>s in a 2D plane.
            ''' </summary>
            ''' <param name="point0">Specifies one point.</param>
            ''' <param name="point1">Specifies the other point.</param>
            ''' <returns>The distance between the two points.</returns>
            ''' <remarks>
            ''' See <see cref="X"/> regarding <see cref="System.Double.NaN"/> or
            ''' infinite values.
            ''' <br/>
            ''' <see cref="D2.point.Distance(System.Double, System.Double,
            ''' System.Double, System.Double)"/>,
            ''' <see cref="D2.point.Distance(D2.Point, D2.Point)"/>, and
            ''' <see cref="D2.Point.Distance(D2.Point)"/> are effectively the
            ''' same thing. Use whichever version best suits the variables at
            ''' hand.
            ''' </remarks>
            Public Shared Function Distance(ByVal point0 As D2.Point,
                ByVal point1 As D2.Point) As System.Double

                ' No input checking.

                ' This is just the Pythagorean theorem, using multiplication vs.
                ' squares.
                Dim DeltaX As System.Double = point1.X - point0.X
                Dim DeltaY As System.Double = point1.Y - point0.Y
                Return System.Math.Sqrt((DeltaX * DeltaX) + (DeltaY * DeltaY))

            End Function ' Distance

            ''' <summary>
            ''' Returns the distance between the current instance and another
            ''' <c>D2.Point</c> in the same 2D plane.
            ''' </summary>
            ''' <param name="other">Specifies a distant <c>D2.Point</c>.</param>
            ''' <returns>The distance between the two
            ''' <c>D2.Point</c>s.</returns>
            ''' <remarks>
            ''' See <see cref="X"/> regarding <see cref="System.Double.NaN"/> or
            ''' infinite values.
            ''' <br/>
            ''' <see cref="D2.point.Distance(System.Double, System.Double,
            ''' System.Double, System.Double)"/>,
            ''' <see cref="D2.point.Distance(D2.Point, D2.Point)"/>, and
            ''' <see cref="D2.Point.Distance(D2.Point)"/> are effectively the
            ''' same thing. Use whichever version best suits the variables at
            ''' hand.
            ''' </remarks>
            Public Function Distance(ByVal other As D2.Point) As System.Double

                ' No input checking.

                ' This is just the Pythagorean theorem, using multiplication vs.
                ' squares.
                Dim DeltaX As System.Double = other.X - Me.X
                Dim DeltaY As System.Double = other.Y - Me.Y
                Return System.Math.Sqrt((DeltaX * DeltaX) + (DeltaY * DeltaY))

            End Function ' Distance

#End Region ' "Distance"

#Region "Movement"

            ''' <summary>
            ''' Returns a <see cref="D2.Point"/> that is the result of rotating
            ''' the current instance, by the specified angle in radians,
            ''' relative to the specified center of rotation.
            ''' </summary>
            ''' <param name="angle">Specifies the angle in radians (positive for
            ''' CCW; negative for CW) by which to rotate. </param>
            ''' <param name="centerX">Specifies the X-coordinate of the center
            ''' of rotation.</param>
            ''' <param name="centerY">Specifies the Y-coordinate of the center
            ''' of rotation.</param>
            ''' <returns>The result of rotating the current instance around the
            ''' specified center of rotation.</returns>
            ''' <remarks>
            ''' See <see cref="X"/> regarding <c>NaN</c> or infinite values.
            ''' <br/>
            ''' <c>D2.Point.RotatedAround(System.Double, System.Double,
            ''' System.Double)</c> and <see cref="D2.Point.RotatedAround(
            ''' System.Double, D2.Point)"/> are effectively the same thing. Use
            ''' whichever version best suits the variables at hand.
            ''' </remarks>
            Public Function RotatedAround(
                ByVal angle As System.Double, ByVal centerX As System.Double,
                ByVal centerY As System.Double) As D2.Point

                ' DEV: This is the worker for the related routine(s).

                '' Suspend to avoid exceptions:
                '' Input checking.
                'If System.Double.IsNaN(angle) OrElse
                '    System.Double.IsNaN(centerX) OrElse
                '    System.Double.IsNaN(centerY) OrElse
                '    System.Double.IsInfinity(angle) OrElse
                '    System.Double.IsInfinity(centerX) OrElse
                '    System.Double.IsInfinity(centerY) Then

                '    Dim CaughtBy As System.Reflection.MethodBase =
                '        System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Arguments to {NameOf(Rotated)} {MSGMHUV}")
                'End If

                ' Determine the current angle and radial length.
                Dim DeltaX As System.Double = Me.X - centerX
                Dim DeltaY As System.Double = Me.Y - centerY
                Dim CurrentAngle As System.Double
                Dim RadLen As System.Double
                If DeltaX.Equals(0.0) Then
                    ' Same X. Vertical line; special handling.
                    If DeltaY > 0.0 Then
                        ' Above the reference.
                        CurrentAngle = OSNW.Math.RAD090d
                    ElseIf DeltaY < 0.0 Then
                        ' Below the reference.
                        CurrentAngle = -OSNW.Math.RAD090d
                    Else
                        ' Same point.
                        Return New D2.Point(Me.X, Me.Y) ' Early exit.
                    End If
                    RadLen = System.Math.Abs(DeltaY)
                Else
                    ' Different X.

                    ' CurrentTan will have a value in the
                    ' [-infinity, +infinity] range and does not distinguish
                    ' whether Me.X < centerX or Me.X > centerX.
                    Dim CurrentTan As System.Double = DeltaY / DeltaX

                    ' Atan() computes the arctangent of CurrentTan, in radians,
                    ' in the [-PI/2, +PI/2] range.
                    ' https://learn.microsoft.com/en-us/dotnet/api/system.double.atan?view=net-10.0&f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(System.Double.Atan)%3Bk(DevLang-VB)%26rd%3Dtrue
                    CurrentAngle = System.Double.Atan(CurrentTan)

                    ' Ensure that the result is in the [0, 2*PI] range.
                    If DeltaX < 0.0 Then
                        CurrentAngle += OSNW.Math.RAD090d
                    End If

                    RadLen = System.Math.Sqrt(
                        (DeltaX * DeltaX) + (DeltaY * DeltaY))

                End If

                ' On getting here, the angle and distamce to the current
                ' location are known. Calculate and normalize the new [0, 2*PI]
                ' range angle.
                ' Truncate rounds NewAngle to the nearest integer toward zero,
                ' keeping the sign intact.
                Dim NewAngle As System.Double = CurrentAngle + angle
                Dim Rotations As System.Double = NewAngle / OSNW.Math.RAD360d
                Dim IntegerPart As System.Double = System.Math.Truncate(Rotations)
                Dim FractionalPart As System.Double = Rotations - IntegerPart
                NewAngle = FractionalPart * OSNW.Math.RAD360d

                ' The new [0, 2*PI] range angle and the radial length are now
                ' known. Calculate and apply the new offsets from the center of
                ' rotation.

                ' cos(theta) = adjacent / hypotenuse
                ' cos(theta) = OffsetX / RadLen.
                ' cos(theta) * RadLen = OffsetX
                Dim OffsetX As System.Double =
                    System.Double.Cos(NewAngle) * RadLen
                Dim NewX As System.Double = centerX + OffsetX

                ' sin(theta) = opposite / hypotenuse
                ' sin(theta) = OffsetY / RadLen.
                ' sin(theta) * RadLen = OffsetY
                Dim OffsetY As System.Double =
                    System.Double.Sin(NewAngle) * RadLen
                Dim NewY As System.Double = centerY + OffsetY

                Return New D2.Point(NewX, NewY)

            End Function ' RotatedAround

            ''' <summary>
            ''' Returns a <see cref="D2.Point"/> that is the result of rotating
            ''' the current instance, by the specified <paramref name="angle"/>
            ''' in radians, relative to the specified <c>center</c> of rotation.
            ''' </summary>
            ''' <param name="angle">Specifies the angle in radians (positive for
            ''' CCW; negative for CW) by which to rotate. </param>
            ''' <param name="center">Specifies the center of rotation.</param>
            ''' <returns>The result of rotating the current instance around the
            ''' specified center of rotation.</returns>
            ''' <remarks>
            ''' See <see cref="X"/> regarding <c>NaN</c> or infinite values.
            ''' <br/>
            ''' <see cref="D2.point.RotatedAround(System.Double, System.Double,
            ''' System.Double)"/> and <c>D2.Point.RotatedAround(System.Double,
            ''' D2.Point)</c> are effectively the same thing. Use whichever
            ''' version best suits the variables at hand.
            ''' </remarks>
            Public Function RotatedAround(ByVal angle As System.Double,
                ByVal center As D2.Point) As D2.Point

                Return Me.RotatedAround(angle, center.X, center.Y)
            End Function ' RotatedAround

            ''' <summary>
            ''' Returns the result of shifting the current instance by the
            ''' specified horizontal (<paramref name="shiftX"/>) and vertical
            ''' (<paramref name="shiftY"/>) amounts.
            ''' </summary>
            ''' <param name="shiftX">Specifies the amount of the horizontal
            ''' shift; a positive value shifts right and a negative value shifts
            ''' left.</param>
            ''' <param name="shiftY">Specifies the amount of the horizontal
            ''' shift; a positive value shifts up and a negative value shifts
            ''' down.</param>
            ''' <returns>The shifted equivalent of the current
            ''' instance.</returns>
            ''' <remarks> See <see cref="X"/> regarding <c>NaN</c> or infinite
            ''' values.</remarks>
            Public Function Shifted(ByVal shiftX As System.Double,
                ByVal shiftY As System.Double) As D2.Point

                '' Suspend to avoid exceptions:
                '' Input checking.
                'If System.Double.IsNaN(shiftX) OrElse
                '    System.Double.IsNaN(shiftY) OrElse
                '    System.Double.IsInfinity(shiftX) OrElse
                '    System.Double.IsInfinity(shiftY) Then

                Return New D2.Point(Me.X + shiftX, Me.Y + shiftY)
            End Function ' Shifted

#End Region ' "Movement"

            ''' <summary>
            ''' Converts the numeric value of this instance to its equivalent
            ''' string representation.
            ''' </summary>
            ''' <returns>
            ''' The string representation of the value of this instance.
            ''' </returns>
            Public Overrides Function ToString() As System.String

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                Return String.Format("<{0}, {1}>", Me.X, Me.Y)
            End Function ' ToString

#End Region ' "Methods"

#Region "Constructors"

            ''' <summary>
            ''' A default constructor that initializes a new instance of the
            ''' <c>D2.Point</c> class with default X- and Y-coordinates. The
            ''' default is a point at the origin.
            ''' </summary>
            Public Sub New()
                ' A default constructor is required to allow inheritance.
                With Me
                    Me.m_X = DFLTX
                    Me.m_Y = DFLTY
                End With
            End Sub ' New

            ''' <summary>
            ''' Initializes a new instance of the <c>D2.Point</c> class with the
            ''' specified X- and Y-coordinates.
            ''' </summary>
            ''' <remarks> See <see cref="X"/> regarding <c>NaN</c> or infinite
            ''' values.</remarks>
            Public Sub New(ByVal x As System.Double, ByVal y As System.Double)

                '' Suspend to avoid exceptions:
                '' Input checking.
                'If System.Double.IsNaN(x) OrElse
                '    System.Double.IsNaN(y) OrElse
                '    System.Double.IsInfinity(x) OrElse
                '    System.Double.IsInfinity(y) Then

                Me.m_X = x
                Me.m_Y = y

            End Sub ' New

#End Region ' "Constructors"

        End Class ' Point

    End Structure ' D2

    'Partial Public Structure D3

    '    ''' <summary>
    '    ''' Computes the distance between two points in a 3D space.
    '    ''' </summary>
    '    ''' <param name="x0">Specifies the X-coordinate of one point.</param>
    '    ''' <param name="y0">Specifies the Y-coordinate of one point.</param>
    '    ''' <param name="z0">Specifies the Z-coordinate of one point.</param>
    '    ''' <param name="x1">Specifies the X-coordinate of the other
    '    ''' point.</param>
    '    ''' <param name="y1">Specifies the Y-coordinate of the other
    '    ''' point.</param>
    '    ''' <param name="z1">Specifies the Z-coordinate of the other
    '    ''' point.</param>
    '    ''' <returns>The distance between the two points.</returns>
    '    ''' <remarks>
    '    ''' <see cref="D3.Distance(Double, Double, Double, Double, Double,
    '    ''' Double)"/>,
    '    ''' <see cref="D3.Distance(D3.Point, D3.Point)"/>,
    '    ''' and <see cref="OSNW.D3.Point.Distance(OSNW.D3.Point)"/>
    '    ''' are effectively the same thing. Use whichever best suits the
    '    ''' variables at hand.
    '    ''' </remarks>
    '    Public Shared Function Distance(ByVal x0 As System.Double,
    '        ByVal y0 As System.Double, ByVal z0 As System.Double,
    '        ByVal x1 As System.Double, ByVal y1 As System.Double,
    '        ByVal z1 As System.Double) As System.Double

    '        ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
    '        ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
    '        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

    '        ' Based on the Pythagorean theorem.
    '        Dim DeltaX As System.Double = x1 - x0
    '        Dim DeltaY As System.Double = y1 - y0
    '        Dim DeltaZ As System.Double = z1 - z0
    '        Return System.Math.Sqrt(
    '            (DeltaX * DeltaX) + (DeltaY * DeltaY) + (DeltaZ * DeltaZ))
    '    End Function ' Distance

    '    ''' <summary>
    '    ''' Represents an ordered triplet of X, Y and Z double precision coordinates
    '    ''' that define a point in a three-dimensional space.
    '    ''' </summary>
    '    Public Class Point

    '        ''' <summary>
    '        ''' Represents the X-coordinate of the current instance.
    '        ''' </summary>
    '        Public X As System.Double

    '        ''' <summary>
    '        ''' Represents the Y-coordinate of the current instance.
    '        ''' </summary>
    '        Public Y As System.Double

    '        ''' <summary>
    '        ''' Represents the Z-coordinate of the current instance.
    '        ''' </summary>
    '        Public Z As System.Double

    '        ' THIS WOULD ALSO NEED A DEFAULT CONSTRUCTOR TO ALLOW INHERITANCE
    '        ''' <summary>
    '        ''' Initializes a new instance of the <c>Point3D</c> class with the
    '        ''' specified coordinates.
    '        ''' </summary>
    '        Public Sub New(ByVal x As System.Double, ByVal y As System.Double,
    '                       ByVal z As System.Double)

    '            ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
    '            ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
    '            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

    '            ' No input checking.
    '            Me.X = x
    '            Me.Y = y
    '            Me.Z = z
    '        End Sub ' New

    '        ''' <summary>
    '        ''' Computes the distance between the current instance and another
    '        ''' <c>Point3D</c> in a 3D space.
    '        ''' </summary>
    '        ''' <param name="other">Specifies a distant <c>Point3D</c>.</param>
    '        ''' <returns>The distance between the two points.</returns>
    '        ''' <remarks>
    '        ''' <see cref="Math.D3.Distance(Double, Double, Double, Double,
    '        ''' Double, Double)"/>,
    '        ''' <see cref="Math.D3.Distance(Math.D3.Point, Math.D3.Point)"/>, and
    '        ''' <see cref="OSNW.Math.D3.Point.Distance(OSNW.Math.D3.Point)"/>
    '        ''' are effectively the same thing. Use whichever best suits the
    '        ''' variables at hand.
    '        ''' </remarks>
    '        Public Function Distance(ByVal other As Point) As System.Double

    '            ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
    '            ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
    '            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

    '            Return Math.D3.Distance(Me.X, Me.Y, Me.Z,
    '                                    other.X, other.Y, other.Z)
    '        End Function ' Distance

    '        ''' <summary>
    '        ''' Converts the numeric value of this instance to its equivalent string
    '        ''' representation.
    '        ''' </summary>
    '        ''' <returns>
    '        ''' The string representation of the value of this instance.
    '        ''' </returns>
    '        Public Overrides Function ToString() As System.String

    '            ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
    '            ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
    '            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

    '            Return System.String.Format("<{0}, {1}, {2}>", Me.X, Me.Y, Me.Z)
    '        End Function ' ToString

    '    End Class ' Point

    '    ''' <summary>
    '    ''' Computes the distance between two points in a 3D space.
    '    ''' </summary>
    '    ''' <param name="p0">Specifies one point.</param>
    '    ''' <param name="p1">Specifies the other point.</param>
    '    ''' <returns>The distance between the two points.</returns>
    '    ''' <remarks>
    '    ''' <see cref="Math.D3.Distance(Double, Double, Double, Double, Double,
    '    ''' Double)"/>,
    '    ''' <see cref="Math.D3.Distance(Math.D3.Point, Math.D3.Point)"/>,
    '    ''' and <see cref="OSNW.Math.D3.Point.Distance(OSNW.Math.D3.Point)"/>
    '    ''' are effectively the same thing. Use whichever best suits the
    '    ''' variables at hand.
    '    ''' </remarks>
    '    Public Shared Function Distance(ByVal p0 As Math.D3.Point,
    '        ByVal p1 As Math.D3.Point) As System.Double

    '        ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
    '        ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
    '        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

    '        Return Math.D3.Distance(p0.X, p0.Y, p0.Z, p1.X, p1.Y, p1.Z)
    '    End Function ' Distance

    'End Structure ' D3

End Module ' Math
