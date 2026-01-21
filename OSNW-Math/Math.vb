Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OsnwPoint2D = OSNW.Math.Point2D

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
    ''' Represents an ordered pair of X and Y double precision coordinates that
    ''' define a point in a two-dimensional plane.
    ''' </summary>
    Public Structure Point2D

        ''' <summary>
        ''' Represents the X-coordinate of this <see cref='Point2D'/>.
        ''' </summary>
        Public X As System.Double

        ''' <summary>
        ''' Represents the Y-coordinate of this <see cref='Point2D'/>.
        ''' </summary>
        Public Y As System.Double

        ''' <summary>
        ''' Initializes a New instance of the <see cref="Point2D"/>
        ''' class with the specified coordinates.
        ''' </summary>
        Public Sub New(ByVal x As System.Double, ByVal y As System.Double)
            ' No input checking.
            Me.X = x
            Me.Y = y
        End Sub ' New

        Public Overrides Function ToString() As System.String
            Return String.Format("<{0}, {1}>", Me.X, Me.Y)
        End Function ' ToString

    End Structure ' Point2D

    ''' <summary>
    ''' Represents an ordered pair of X and Y double precision coordinates that
    ''' define a point in a three-dimensional space.
    ''' </summary>
    Public Structure Point3D

        ''' <summary>
        ''' Represents the X-coordinate of this <see cref='Point2D'/>.
        ''' </summary>
        Public X As System.Double

        ''' <summary>
        ''' Represents the Y-coordinate of this <see cref='Point2D'/>.
        ''' </summary>
        Public Y As System.Double

        ''' <summary>
        ''' Represents the Z-coordinate of this <see cref='Point2D'/>.
        ''' </summary>
        Public Z As System.Double

        ''' <summary>
        ''' Initializes a New instance of the <see cref="Point2D"/>
        ''' class with the specified coordinates.
        ''' </summary>
        Public Sub New(ByVal x As System.Double, ByVal y As System.Double, ByVal z As System.Double)
            ' No input checking.
            Me.X = x
            Me.Y = y
            Me.Z = z
        End Sub ' New

        Public Overrides Function ToString() As System.String
            Return System.String.Format("<{0}, {1}, {2}>", Me.X, Me.Y, Me.Z)
        End Function ' ToString

    End Structure ' Point3D

    ''' <summary>
    ''' Computes the distance between two points in a 3D space.
    ''' </summary>
    ''' <param name="p1">Specifies one point.</param>
    ''' <param name="p2">Specifies the other point.</param>
    ''' <returns>The distance between the two points.</returns>
    Public Function Distance3D(ByVal p1 As Point3D, ByVal p2 As Point3D) _
        As System.Double

        ' Based on the Pythagorean theorem.
        Dim DeltaX As System.Double = p2.X - p1.X
        Dim DeltaY As System.Double = p2.Y - p1.Y
        Dim DeltaZ As System.Double = p2.Z - p1.Z
        Return System.Math.Sqrt(
            (DeltaX * DeltaX) + (DeltaY * DeltaY) + (DeltaZ * DeltaZ))
    End Function ' Distance3D

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
            ' DEV: Being functionally redundant, this may need to be excluded from
            ' any serialization process.
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

        ''' <summary>
        ''' Calculates the intersection points between two circles defined by their
        ''' center coordinates and radii.
        ''' </summary>
        ''' <param name="c1X">Specifies the X-coordinate of circle 1.</param>
        ''' <param name="c1Y">Specifies the Y-coordinate of circle 1.</param>
        ''' <param name="c1R">Specifies the radius of circle 1.</param>
        ''' <param name="c2X">Specifies the X-coordinate of circle 2.</param>
        ''' <param name="c2Y">Specifies the Y-coordinate of circle 2.</param>
        ''' <param name="c2R">Specifies the radius of circle 1.</param>
        ''' <returns>A list of 0, 1, or 2 intersection points as
        ''' <see cref="OSNW.Math.Point2D"/> structure(s).</returns>
        ''' <exception cref="ArgumentOutOfRangeException">when either radius is less
        ''' than or equal to zero.</exception>
        ''' <remarks>
        ''' If there are no intersection points, an empty list is returned. If the
        ''' circles are tangent to each other, a list with one intersection point is
        ''' returned. If the circles intersect at two points, a list with both
        ''' points is returned.
        ''' </remarks>
        Public Shared Function GetIntersections(ByVal c1X As System.Double,
                ByVal c1Y As System.Double, ByVal c1R As System.Double,
                ByVal c2X As System.Double, ByVal c2Y As System.Double,
                ByVal c2R As System.Double) _
                As System.Collections.Generic.List(Of OsnwPoint2D)

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
                As New System.Collections.Generic.List(Of OsnwPoint2D)

            ' Concentric circles would have zero or infinite intersection points.
            If OSNW.Math.EqualEnough(c1X, c2X, OSNW.Math.GRAPHICTOLERANCE) AndAlso
                OSNW.Math.EqualEnough(c1Y, c2Y, OSNW.Math.GRAPHICTOLERANCE) Then

                Return Intersections ' Still empty.
            End If

            ' Calculate the distance between the centers of the circles.
            Dim DeltaX As System.Double = c2X - c1X
            Dim DeltaY As System.Double = c2Y - c1Y
            Dim DeltaCtr As System.Double =
                System.Math.Sqrt(DeltaX * DeltaX + DeltaY * DeltaY)

            ' Check if circles are too far apart or if one is contained within, but
            ' not tangent to, the other.
            If DeltaCtr > (c1R + c2R) OrElse
                DeltaCtr < System.Math.Abs(c1R - c2R) Then

                Return Intersections ' Still empty.
            End If

            ' On getting this far, the circles are neither isolated nor have one
            ' separately contained within the other. There should now be either one
            ' or two intersections.

            ' Check if the circles are outside-tangent to each other.
            If OSNW.Math.EqualEnough(c1R + c2R, DeltaCtr,
                                     OSNW.Math.GRAPHICTOLERANCE) Then
                ' One intersection point.
                Dim C1Frac As System.Double = c1R / DeltaCtr
                Intersections.Add(New OsnwPoint2D(
                                  c1X + C1Frac * DeltaX, c1Y + C1Frac * DeltaY))
                Return Intersections
            End If

            ' Check if the circles are inside-tangent to each other.
            ' Two circles of the same radius cannot be inside-tangent to each other.
            If Not OSNW.Math.EqualEnough(c1R, c2R, OSNW.Math.GRAPHICTOLERANCE) Then
                If OSNW.Math.EqualEnough(System.Math.Abs(c1R - c2R), DeltaCtr,
                                         OSNW.Math.GRAPHICTOLERANCE) Then
                    ' They are inside-tangent.
                    If c1R > c2R Then
                        Dim C1Frac As System.Double = c1R / DeltaCtr
                        Intersections.Add(New OsnwPoint2D(
                                              c1X + (C1Frac * DeltaX),
                                              c1Y + (C1Frac * DeltaY)))
                        Return Intersections
                    Else
                        Dim C2Frac As System.Double = c2R / DeltaCtr
                        Intersections.Add(New OsnwPoint2D(
                                              c2X + (C2Frac * -DeltaX),
                                              c2Y + (C2Frac * -DeltaY)))
                        Return Intersections
                    End If
                End If
            End If

            ' (The initial version of) the sequence below was generated by Visual
            ' Studio AI.

            ' Calculate two intersection points.
            Dim OnceA As System.Double =
                (c1R * c1R - c2R * c2R + DeltaCtr * DeltaCtr) / (2 * DeltaCtr)
            Dim OnceH As System.Double = System.Math.Sqrt(c1R * c1R - OnceA * OnceA)
            Dim X0 As System.Double = c1X + OnceA * (DeltaX / DeltaCtr)
            Dim Y0 As System.Double = c1Y + OnceA * (DeltaY / DeltaCtr)

            ' Two intersection points.
            Dim intersection1 As New OsnwPoint2D(
                X0 + OnceH * (DeltaY / DeltaCtr),
                Y0 - OnceH * (DeltaX / DeltaCtr))
            Dim intersection2 As New OsnwPoint2D(
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
            As System.Collections.Generic.List(Of OsnwPoint2D)

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
            As System.Collections.Generic.List(Of OsnwPoint2D)

            Return GetIntersections(Me, otherCircle)
        End Function ' GetIntersections

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        ''' <param name="circle1">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle2"/>.</param>
        ''' <param name="circle2">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle1"/>.</param>
        ''' <returns>xxxxxxxxxx</returns>
        Public Shared Function CirclesIntersect(ByVal circle1 As Circle2D,
                ByVal circle2 As Circle2D) As System.Boolean

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx
            Return circle1.GetIntersections(circle2).Count > 0
        End Function ' CirclesIntersect

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        ''' <param name="circle1">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle2"/>.</param>
        ''' <param name="circle2">Specifies the <c>Circle2D</c> to consider for
        ''' intersection with <paramref name="circle1"/>.</param>
        ''' <param name="intersections">xxxxxxxxxx</param>
        ''' <returns>xxxxxxxxxx</returns>
        Public Shared Function CirclesIntersect(
            ByVal circle1 As Circle2D, ByVal circle2 As Circle2D,
            ByRef intersections As System.Collections.Generic.List(
                Of OSNW.Math.Point2D)) As System.Boolean

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx
            intersections = circle1.GetIntersections(circle2)
            Return intersections.Count > 0
        End Function ' CirclesIntersect

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
    ''' <param name="x2">xxxxxxxxxx</param>
    ''' <param name="y2">xxxxxxxxxx</param>
    ''' <param name="r2">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' Tangent circles will have only one intersection. When both circles
    ''' specify the same circle, they are considered not to intersect.
    ''' </remarks>
    Public Function CirclesIntersect(ByVal x1 As System.Double,
        ByVal y1 As System.Double, ByVal r1 As System.Double,
        ByVal x2 As System.Double, ByVal y2 As System.Double,
        ByVal r2 As System.Double) As System.Boolean

        ' Input checking.
        If (r1 < 0.0) OrElse (r2 < 0.0) Then
            Return False
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
    Public Function TryCircleIntersection(
        ByVal x1 As System.Double, ByVal y1 As System.Double, ByVal r1 As System.Double,
        ByVal x2 As System.Double, ByVal y2 As System.Double, ByVal r2 As System.Double,
        ByRef i1x As System.Double, ByRef i1y As System.Double,
        ByRef i2x As System.Double, ByRef i2y As System.Double) As System.Boolean

        If Not CirclesIntersect(x1, y1, r1, x2, y2, r2) Then
            i1x = Double.NaN
            i1y = Double.NaN
            i2x = Double.NaN
            i2y = Double.NaN
            Return False
        End If

        i1x = 999.99
        i1y = 999.99
        i2x = 999.99
        i2y = 999.99



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
