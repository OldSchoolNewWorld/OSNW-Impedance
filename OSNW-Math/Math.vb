Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports Point2D = OSNW.Math2D.Point
Imports Point3D = OSNW.Math3D.Point

Public Module Math

#Region "Constants"

    ''' <summary>
    ''' This sets a practical limit on the precision of equality detection in
    ''' mathematical operations. It is intended to prevent issues arising from
    ''' minor inequalities due to floating point precision limitations. A
    ''' smaller value DEcreases the liklihood of detecting equality; a larger
    ''' value INcreases the liklihood of detecting equality.
    ''' </summary>
    ''' <remarks>
    ''' This is intended to be used as a factor to be multiplied by some
    ''' practical reference value.<br/>
    ''' <example>
    ''' This example uses the <c>DFLTEQUALITYTOLERANCE</c> value to determine if
    ''' two values are close enough to be treated as being equal. In the case of
    ''' <see cref="OSNW.Math.EqualEnough(Double, Double, Double)"/>,
    ''' the reference value is the "refVal" parameter.
    ''' <code>
    ''' Public Shared Sub EqualityTest
    '''
    '''     Dim Tol As System.Double = OSNW.Math.DFLTEQUALITYTOLERANCE
    '''     Dim RefVal As System.Double = 50.0
    '''     Dim TestVal As System.Double = 50.000049
    '''
    '''     if OSNW.Math.EqualEnough(TestVal, RefVal, Tol) then
    '''         '
    '''         ' Code for a match.
    '''         '
    '''     else
    '''         '
    '''         ' Code for a mismatch.
    '''         '
    '''     end if
    '''
    ''' End Sub
    ''' </code></example>
    ''' </remarks>
    Public Const DFLTEQUALITYTOLERANCE As System.Double = 0.000_001

    ''' <summary>
    ''' This sets a practical limit on the precision of equality detection in
    ''' graphics operations. It is intended to prevent issues arising from
    ''' floating point precision limitations. A smaller value DEcreases the
    ''' liklihood of detecting equality; a larger value INcreases the liklihood
    ''' of detecting equality.
    ''' </summary>
    ''' <remarks>
    ''' The default value will generally accommodate indistinguishable,
    ''' sub-pixel, differences on any current monitor or printer.<br/>
    ''' <example>
    ''' This example uses the <c>DFLTGRAPHICTOLERANCE</c> value to determine if
    ''' two values are close enough to be treated as being equal. In the case of
    ''' <see cref="OSNW.Math.EqualEnough(Double, Double, Double)"/>,
    ''' the reference value is the "refVal" parameter.
    ''' <code>
    ''' Public Shared Sub GraphicEqualityTest
    '''
    '''     Dim RefVal As System.Double = 100.0
    '''     Dim TestVal As System.Double = 100.00005
    '''     Dim Tol As System.Double = OSNW.Math.DFLTGRAPHICTOLERANCE
    '''
    '''     if OSNW.Math.EqualEnough(TestVal, RefVal, Tol) then
    '''         '
    '''         ' Code for a match.
    '''         '
    '''     else
    '''         '
    '''         ' Code for a mismatch.
    '''         '
    '''     end if
    '''
    ''' End Sub
    ''' </code></example>
    ''' </remarks>
    Public Const DFLTGRAPHICTOLERANCE As System.Double = 0.0001

    ' Just for shorthand.
    Public Const PIs As Single = System.Single.Pi
    Public Const HALFPIs As System.Single = PIs / 2.0
    Public Const PId As System.Double = System.Double.Pi
    Public Const HALFPId As System.Double = PId / 2.0

    Public Const MSGCHIV As System.String = "Cannot have an infinite value."
    Public Const MSGCHNV As System.String = "Cannot have a negative value."
    Public Const MSGCHZV As System.String = "Cannot have a zero value."
    Public Const MSGUEEZ As System.String = MSGCHZV & " Use EqualEnoughZero()."
    Public Const MSGVMBGTE1 As System.String =
        "Must be greater than or equal to 1."
    Public Const MSGVMBGTZ As System.String =
        "Must be a positive, non-zero value." ' Must be greater than zero.

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

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

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

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

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

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

        ' Input checking.
        If otherVal.Equals(0.0) OrElse refVal.Equals(0.0) Then
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                $"Arguments to {NameOf(CaughtBy)} {MSGCHZV} {MSGUEEZ}")
        End If

        Return System.Math.Abs(otherVal - refVal) <=
            System.Math.Abs(ratio * refVal)

    End Function ' EqualEnough

#End Region ' "EqualEnough Implementations"

    ''' <summary>
    ''' Computes the distance between two points in a 2D plane.
    ''' </summary>
    ''' <param name="p0">Specifies one point.</param>
    ''' <param name="p1">Specifies the other point.</param>
    ''' <returns>The distance between the two points.</returns>
    Public Function Distance(ByVal p0 As Point2D, ByVal p1 As Point2D) _
        As System.Double

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

        ' Based on the Pythagorean theorem.
        Dim DeltaX As System.Double = p1.X - p0.X
        Dim DeltaY As System.Double = p1.Y - p0.Y
        Return System.Math.Sqrt((DeltaX * DeltaX) + (DeltaY * DeltaY))
    End Function ' Distance

    ''' <summary>
    ''' Computes the distance between two points in a 3D space.
    ''' </summary>
    ''' <param name="p0">Specifies one point.</param>
    ''' <param name="p1">Specifies the other point.</param>
    ''' <returns>The distance between the two points.</returns>
    Public Function Distance(ByVal p0 As Point3D, ByVal p1 As Point3D) _
        As System.Double

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

        ' Based on the Pythagorean theorem.
        Dim DeltaX As System.Double = p1.X - p0.X
        Dim DeltaY As System.Double = p1.Y - p0.Y
        Dim DeltaZ As System.Double = p1.Z - p0.Z
        Return System.Math.Sqrt(
            (DeltaX * DeltaX) + (DeltaY * DeltaY) + (DeltaZ * DeltaZ))
    End Function ' Distance

    ''' <summary>
    ''' Attempts to solve the "aX^2 + bX + c = 0" quadratic equation for real
    ''' solutions.
    ''' </summary>
    ''' <param name="a">Specifies the <paramref name="a"/> in the well-known
    ''' formula.</param>
    ''' <param name="b">Specifies the <paramref name="b"/> in the well-known
    ''' formula.</param>
    ''' <param name="c">Specifies the <paramref name="c"/> in the well-known
    ''' formula.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' When valid, also returns the results in <paramref name="x0"/> and
    ''' <paramref name="x1"/>.</returns>
    ''' <remarks>
    ''' <br/><example>
    ''' This example shows how to use <c>TryQuadratic</c>.
    ''' <code>
    ''' Dim A As System.Double = something
    ''' Dim B As System.Double = something
    ''' Dim C As System.Double = something
    ''' Dim x0 As System.Double
    ''' Dim x1 As System.Double
    ''' 
    ''' If OSNW.Math.TryQuadratic(A, B, C, x0, x1) Then
    '''     '
    '''     Use x0 and x1 for further processing.
    '''     '
    ''' else
    '''     '
    '''     ' Respond to the failure with a warning, exception, or default
    '''     ' value.
    '''     '
    ''' End If
    ''' 
    '''     - or -
    ''' 
    ''' If not OSNW.Math.TryQuadratic(A, B, C, x0, x1) Then
    '''     '
    '''     ' Respond to the failure with a warning, default value,
    '''     ' or exception.
    '''     ' Early exit.
    '''     '
    ''' End If
    ''' '
    ''' Use x0 and x1 for further processing.
    ''' '
    ''' </code></example>
    ''' </remarks>
    Public Function TryQuadratic(ByVal a As System.Double,
        ByVal b As System.Double, ByVal c As System.Double,
        ByRef x0 As System.Double, ByRef x1 As System.Double) As System.Boolean

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

        ' Input checking.
        Dim Discriminant As System.Double = b * b - 4.0 * a * c
        If a.Equals(0.0) OrElse Discriminant < 0.0 Then
            ' Not a quadratic equation.
            x0 = Double.NaN
            x1 = Double.NaN
            Return False
        End If

        Dim DiscRoot As System.Double = System.Math.Sqrt(Discriminant)
        Dim A2 As System.Double = 2.0 * a
        x0 = (-b + DiscRoot) / A2
        x1 = (-b - DiscRoot) / A2
        Return True

    End Function ' TryQuadratic

    ''' <summary>
    ''' Compares an array of values to compute which is greater.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be
    ''' examined.</param>
    ''' <returns>The greatest value in the array.</returns>
    ''' <remarks>
    ''' An empty array returns <c>System.Double.NaN</c>.
    ''' <br/><example>
    ''' This example shows how to call <c>MaxVal</c>.
    ''' <code>
    ''' Dim MaxAll As System.Double =
    '''     OSNW.Math.MaxVal({x0, y0, r0, x1, y1, r1})
    ''' </code></example>
    ''' </remarks>
    Public Function MaxVal(
        ByVal ParamArray values() As System.Double) As System.Double

        ' Input checking.
        If values.Length.Equals(0) Then
            Return System.Double.NaN
        End If

        Dim Max As System.Double
        For Each OneValue As System.Double In values
            If OneValue > Max Then
                Max = OneValue
            End If
        Next
        Return Max

    End Function ' MaxVal

    ''' <summary>
    ''' Compares an array of values to compute which has the greatest magnitude.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be
    ''' examined.</param>
    ''' <returns>The magnitude greatest absolute value in the array.</returns>
    ''' <remarks>
    ''' An empty array returns <c>System.Double.NaN</c>. When the array is not
    ''' empty, this always returns a positive magnitude.
    ''' <br/><example>
    ''' This example shows how to call <c>MaxValAbs</c>.
    ''' <code>
    ''' Dim MaxAll As System.Double =
    '''     OSNW.Math.MaxValAbs({x0, y0, r0, x1, y1, r1})
    ''' </code></example>
    ''' </remarks>
    Public Function MaxValAbs(
        ByVal ParamArray values() As System.Double) As System.Double

        ' Input checking.
        If values.Length.Equals(0) Then
            Return System.Double.NaN
        End If

        Dim Max As System.Double
        Dim AbsVal As System.Double
        For Each OneValue As System.Double In values
            AbsVal = System.Math.Abs(OneValue)
            If AbsVal > Max Then
                Max = AbsVal
            End If
        Next
        Return Max

    End Function ' MaxSetAbs

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="values">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' xxxxxxxxxx
    ''' ADD AN EXAMPLE
    ''' xxxxxxxxxx
    ''' </remarks>
    Public Function GeometricMean(
        ByVal ParamArray values() As System.Double) As System.Double

        ' Input checking.
        If values.Length = 0 Then
            Return System.Double.NaN
        End If

        Dim Product As System.Double = 1.0
        For Each OneValue As System.Double In values
            Product *= OneValue
        Next
        Return System.Math.Pow(Product, 1.0 / values.Length)

    End Function ' GeometricMean

    ''' <summary>
    ''' Calculates the length of a hypotenuse using the Pythagorean theorem.
    ''' </summary>
    ''' <param name="values">An array of values: Hypotenuse(V1, V2, V3,...Vn)</param>
    ''' <returns>Hypotenuse As System.Double</returns>
    ''' <remarks>
    ''' This works in more than just 2 dimensions;
    ''' 2D: Hypotenuse(X, Y);
    ''' 3D: Hypotenuse(X, Y, Z);
    ''' nD: Hypotenuse(V1, V2, V3,...Vn)
    ''' <br/>An empty array returns <c>System.Double.NaN</c>.
    ''' </remarks>
    Public Function Hypotenuse(ByVal ParamArray values() As System.Double
                               ) As System.Double

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

        ' This may need to do input checking because, for instance, a triangle could not
        ' have a side with zero or negative length in practical cases. Then again, some physics 
        ' problems deal with values not normally seen. Also, impedances can have a negative
        ' component.
        ' Is there any harm in allowing other values?  A Cartesian coordinate calculation could
        ' have negative lengths unless something is done to make them result in positive values.
        ' At least for now, negatives are allowed. The squaring prevents any impact from negative
        ' values. Similarly, zeroes are allowed because of the lack of significant consequence. A
        ' triangle with a zero side would be a line.
        '
        ' 2D: Hypotenuse(X, Y)
        ' 3D: Hypotenuse(X, Y, Z)
        ' Assumed to work out in 4, 5, ... dimensions.
        ' http://en.wikipedia.org/wiki/Pythagorean_theorem has remarks regarding
        ' an "n-dimensional Pythagorean theorem".

        ' Input checking.
        If values.Length.Equals(0) Then
            Return System.Double.NaN
        End If

        Dim Sum As System.Double = 0.0
        For Each OneValue As System.Double In values
            Sum += OneValue * OneValue
        Next
        Return System.Math.Sqrt(Sum)

    End Function ' Hypotenuse

    ''' <summary>
    ''' Rounds a double-precision floating-point value to the nearest multiple
    ''' of the specified value. A parameter specifies how to round the interim
    ''' value if it is midway between two other numbers.
    ''' </summary>
    ''' <param name="nearest">Specification for the value to which
    ''' <paramref name="value"/> should be rounded.</param>
    ''' <param name="value">A double-precision, floating-point, number to be
    ''' rounded.</param>
    ''' <param name="mode">Specification for how to round
    ''' <paramref name="value"/> if it is midway between two potential results.
    ''' This is optional. If not specified, System.MidpointRounding.ToEven is
    ''' assumed.</param>
    ''' <returns>
    ''' The nearest multiple of <paramref name="value"/>. If the interim
    ''' rounding point value is halfway between two digits, one of which is even
    ''' and the other odd, then <paramref name="mode"/> determines which of the
    ''' two is used.
    ''' </returns>
    ''' <exception cref="System.ArgumentException"> Thrown when
    ''' <paramref name="nearest"/> is negative or zero.</exception>
    Public Function RoundTo(ByVal nearest As System.Double,
        ByVal value As System.Double,
        Optional ByVal mode As System.MidpointRounding =
            System.MidpointRounding.ToEven) _
        As System.Double

        ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

        ' Input checking.
        If nearest <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(nearest), MSGVMBGTZ)
        End If

        Dim Interim As System.Double = System.Math.Round(value / nearest, mode)
        Return Interim * nearest

    End Function ' RoundTo

End Module ' Math
