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
    Public Const DFLTGRAPHICTOLERANCE As System.Double = 0.0001

    ' Just for shorthand.
    Public Const PIs As Single = System.Single.Pi
    Public Const HALFPIs As System.Single = System.Single.Pi / 2.0
    Public Const PId As System.Double = System.Double.Pi
    Public Const HALFPId As System.Double = System.Double.Pi / 2.0

    Public Const MSGCHIV As System.String = "Cannot have an infinite value."
    Public Const MSGCHNV As System.String = "Cannot have a negative value."
    Public Const MSGCHZV As System.String = "Cannot have a zero value."
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

End Module ' Math
