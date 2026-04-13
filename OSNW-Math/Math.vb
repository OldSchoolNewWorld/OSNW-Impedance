Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

''' <summary>
''' A root name for a collection of types and routines for various mathematical
''' operations.
''' </summary>
''' <remarks>This primarily consists of functionality created to support the
''' needs of other OSNW implementations.</remarks>
Public Module Math

    ''' <summary>
    ''' A collection of types and methods for mathematical operations on a
    ''' 2-dimensional plane.
    ''' </summary>
    Public Structure D2
        ' This is just a place to define a name that is further defined in other
        ' documents.
    End Structure

    ''' <summary>
    ''' A collection of types and methods for mathematical operations in a
    ''' 3-dimensional space.
    ''' </summary>
    Public Structure D3
        ' This is just a place to define a name that is further defined in other
        ' documents.
    End Structure

#Region "Constants"
    ' These are constants that are used throughout the OSNW code. Ingeneral,
    ' they are not specific to any particular use case.

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
    ''' <see cref="EqualEnough(System.Double, System.Double, System.Double)"/>,
    ''' the reference value is the "refVal" parameter.
    ''' <code>
    ''' Public Shared Sub EqualityTest
    '''
    '''     Dim RefVal As System.Double = 50.0
    '''     Dim Tol As System.Double = OSNW.Math.DFLTEQUALITYTOLERANCE
    '''     Dim TestVal As System.Double = 50.000049
    '''
    '''     if OSNW.Math.EqualEnough(RefVal, Tol, TestVal) then
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
    '''     Dim Tol As System.Double = OSNW.Math.DFLTGRAPHICTOLERANCE
    '''     Dim TestVal As System.Double = 100.00005
    '''
    '''     if OSNW.Math.EqualEnough(RefVal, Tol, TestVal) then
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

    Public Const OSNWDFLTMPR As System.MidpointRounding =
        System.MidpointRounding.ToEven

    ' Just for shorthand and obvious radian/degree ties for key angles.
    Public Const PIs As System.Single = System.Single.Pi ' As a Single.
    Public Const TWOPIs As System.Single = 2.0 * System.Single.Pi ' As a Single.
    Public Const PId As System.Double = System.Double.Pi ' As a Double.
    Public Const TWOPId As System.Double = 2.0 * System.Double.Pi ' As a Double.
    Public Const RAD30s As System.Single = PIs / 6.0
    Public Const RAD45s As System.Single = PIs / 4.0
    Public Const RAD60s As System.Single = PIs / 3.0
    Public Const RAD90s As System.Single = PIs / 2.0
    Public Const RAD120s As System.Single = 2.0 * PIs / 3.0
    Public Const RAD150s As System.Single = 5.0 * PIs / 6.0
    Public Const RAD180s As System.Single = PIs
    Public Const RAD210s As System.Single = 7.0 * PIs / 6.0
    Public Const RAD240s As System.Single = 4.0 * PIs / 3.0
    Public Const RAD270s As System.Single = 1.5 * PIs
    Public Const RAD300s As System.Single = 5.0 * PIs / 3.0
    Public Const RAD330s As System.Single = 11.0 * PIs / 6.0
    Public Const RAD360s As System.Single = 2.0 * PIs
    Public Const RAD30d As System.Double = PId / 6.0
    Public Const RAD45d As System.Single = PId / 4.0
    Public Const RAD60d As System.Double = PId / 3.0
    Public Const RAD90d As System.Double = PId / 2.0
    Public Const RAD120d As System.Double = 2.0 * PId / 3.0
    Public Const RAD150d As System.Double = 5.0 * PId / 6.0
    Public Const RAD180d As System.Double = PId
    Public Const RAD210d As System.Double = 7.0 * PId / 6.0
    Public Const RAD240d As System.Double = 4.0 * PId / 3.0
    Public Const RAD270d As System.Double = 1.5 * PId
    Public Const RAD300d As System.Double = 5.0 * PId / 3.0
    Public Const RAD330d As System.Double = 11.0 * PId / 6.0
    Public Const RAD360d As System.Double = 2.0 * PId

    ' For consistency and reuse.
    Public Const MSGCHIV As System.String = "Cannot have an infinite value."
    Public Const MSGCHNV As System.String = "Cannot have a negative value."
    Public Const MSGCHZV As System.String = "Cannot have a zero value."
    Public Const MSGUEEZ As System.String = MSGCHZV & " Use EqualEnoughZero()."
    Public Const MSGVMBGTE1 As System.String =
        "Must be greater than or equal to 1."
    Public Const MSGVMBGTZ As System.String =
        "Must be a positive, non-zero value." ' Must be greater than zero.
    Public Const MSGMHUV As System.String =
        "Must have a usable value."

#End Region ' "Constants"

#Region "EqualEnough Implementations"

    ' REF: Precision and complex numbers
    ' https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers

    ' REF: Random ASCII – tech blog of Bruce Dawson
    ' https://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition/

    ''' <summary>
    ''' Check for reasonable equality when using floating point values. A
    ''' difference of less than or equal to <paramref name="tolerance"/> is
    ''' considered to establish equality.
    ''' </summary>
    ''' <param name="refVal">Specifies the reference value to which
    ''' <paramref name="otherVal"/> is compared.</param>
    ''' <param name="tolerance">Specifies the maximum difference that satisfies
    ''' equality.</param>
    ''' <param name="otherVal">Specifies the value to be compared to
    ''' <paramref name="refVal"/>.</param>
    ''' <returns><c>True</c> if the values are reasonably close in value;
    ''' otherwise, <c>False</c>.</returns>
    ''' <remarks>
    ''' This does the comparison based on an absolute numeric difference. The
    ''' control value is <paramref name="tolerance"/>. Select
    ''' <paramref name="tolerance"/> such that it is a good representation of
    ''' zero, relative to other known or expected values.</remarks>
    Public Function EqualEnoughAbsolute(ByVal refVal As System.Double,
        ByVal tolerance As System.Double, ByVal otherVal As System.Double) _
        As System.Boolean

        ' No input checking.
        Return System.Math.Abs(otherVal - refVal) <= tolerance
    End Function ' EqualEnoughAbsolute

    ''' <summary>
    ''' Check for reasonable equality to zero when using floating point values.
    ''' Any value less than or equal to <paramref name="tolerance"/> from zero
    ''' is considered to equal zero.
    ''' </summary>
    ''' <param name="value">Specifies the value to be compared to zero.</param>
    ''' <param name="tolerance">Specifies the maximum offset from zero which
    ''' is assumed to represent zero.</param>
    ''' <returns><c>True</c> if <paramref name="value"/> is reasonably close to
    ''' zero; otherwise, <c>False</c>.</returns>
    ''' <remarks>Use this when an actual zero reference would cause a failure in
    ''' <see cref="EqualEnough(System.Double, System.Double, System.Double)"/>.
    ''' Select <paramref name="tolerance"/> such that it is a good
    ''' representation of zero relative to other known or expected
    ''' values.</remarks>
    Public Function EqualEnoughZero(ByVal value As System.Double,
        ByVal tolerance As System.Double) As System.Boolean

        ' No input checking.
        Return System.Math.Abs(value) <= System.Math.Abs(tolerance)
    End Function ' EqualEnoughZero

    ''' <summary>
    ''' Check for reasonable equality, within a specified ratio, when using
    ''' floating point values.
    ''' </summary>
    ''' <param name="refVal">Specifies the reference value to which
    ''' <paramref name="otherVal"/> is compared.</param>
    ''' <param name="ratio">Specifies the maximum ratio of the values which is
    ''' assumed to represent equality.</param>
    ''' <param name="otherVal">Specifies the value to be compared to
    ''' <paramref name="refVal"/>.</param>
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
    Public Function EqualEnough(ByVal refVal As System.Double,
        ByVal ratio As System.Double, ByVal otherVal As System.Double) _
        As System.Boolean

        ' Input checking.
        If otherVal.Equals(0.0) OrElse refVal.Equals(0.0) Then
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                $"Arguments to {NameOf(EqualEnough)} {MSGCHZV} {MSGUEEZ}")
        End If

        Return System.Math.Abs(otherVal - refVal) <=
            System.Math.Abs(ratio * refVal)

    End Function ' EqualEnough

#End Region ' "EqualEnough Implementations"

#Region "Minimum/Maximum Implementations"

    ''' <summary>
    ''' Compares an array of values to compute which has the greatest magnitude.
    ''' </summary>
    ''' <param name="values">Specifies an array of values to examine.</param>
    ''' <returns>The magnitude of greatest absolute value in the
    ''' array.</returns>
    ''' <remarks>
    ''' An empty array returns <c>System.Double.NaN</c>. When the array is not
    ''' empty, this always returns a positive magnitude.
    ''' <see cref="MaxValue"/> and <c>MaxMagnitude</c> differ in their handling
    ''' of negative arguments: <c>MaxValue({4, -5})</c> returns 4;
    ''' <c>MaxMagnitude({4, -5})</c> returns 5.
    ''' <br/><example>
    ''' This example shows how to call <c>MaxMagnitude</c>.
    ''' <code>
    ''' Dim MaxAll As System.Double =
    '''     OSNW.Math.MaxMagnitude({x0, y0, r0, x1, y1, r1})
    ''' </code></example>
    ''' </remarks>
    Public Function MaxMagnitude(
        ByVal ParamArray values() As System.Double) As System.Double

        ' Input checking.
        If values.Length.Equals(0) Then
            Return System.Double.NaN
        End If

        Dim Max As System.Double ' Starts at zero.
        Dim AbsVal As System.Double
        For Each OneValue As System.Double In values
            AbsVal = System.Math.Abs(OneValue)
            If AbsVal > Max Then
                Max = AbsVal
            End If
        Next
        Return Max

    End Function ' MaxMagnitude

    ''' <summary>
    ''' Compares an array of values to compute which has the greatest value.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be
    ''' examined.</param>
    ''' <returns>The greatest value in the array.</returns>
    ''' <remarks>
    ''' An empty array returns <c>System.Double.NaN</c>.
    ''' <see cref="MaxValue"/> and <c>MaxMagnitude</c> differ in their handling
    ''' of negative arguments: <c>MaxValue({4, -5})</c> returns 4;
    ''' <c>MaxMagnitude({4, -5})</c> returns 5.
    ''' <br/><example>
    ''' This example shows how to call <c>MaxValue</c>.
    ''' <code>
    ''' Dim MaxAll As System.Double =
    '''     OSNW.Math.MaxValue({x0, y0, r0, x1, y1, r1})
    ''' <br/><br/>or, with an array variable:<br/><br/>
    ''' Dim Values As Double() = {x0, y0, r0, x1, y1, r1}
    ''' Dim MaxAll As System.Double = OSNW.Math.MaxValue({Values})
    ''' </code></example>
    ''' </remarks>
    Public Function MaxValue(
        ByVal ParamArray values() As System.Double) As System.Double

        ' Input checking.
        If values.Length.Equals(0) Then
            Return System.Double.NaN
        End If

        Dim Max As System.Double ' Starts at zero.
        For Each OneValue As System.Double In values
            If OneValue > Max Then
                Max = OneValue
            End If
        Next
        Return Max

    End Function ' MaxValue

    ''' <summary>
    ''' Compares an array of values to compute which has the smallest magnitude.
    ''' </summary>
    ''' <param name="values">Specifies an array of values to examine.</param>
    ''' <returns>The magnitude of smallest absolute value in the
    ''' array.</returns>
    ''' <remarks>
    ''' An empty array returns <c>System.Double.NaN</c>. When the array is not
    ''' empty, this always returns a positive magnitude.
    ''' <see cref="MinValue"/> and <c>MinMagnitude</c> differ in their handling
    ''' of negative arguments: <c>MinValue({4, -5})</c> returns -5;
    ''' <c>MinMagnitude({4, -5})</c> returns 4.
    ''' <br/><example>
    ''' This example shows how to call <c>MinMagnitude</c>.
    ''' <code>
    ''' Dim MinAll As System.Double =
    '''     OSNW.Math.MinMagnitude({x0, y0, r0, x1, y1, r1})
    ''' </code></example>
    ''' </remarks>
    Public Function MinMagnitude(
        ByVal ParamArray values() As System.Double) As System.Double

        ' Input checking.
        If values.Length.Equals(0) Then
            Return System.Double.NaN
        End If

        Dim Min As System.Double = values(0)
        Dim AbsVal As System.Double
        For Each OneValue As System.Double In values
            AbsVal = System.Math.Abs(OneValue)
            If AbsVal < Min Then
                Min = AbsVal
            End If
        Next
        Return Min

    End Function ' MinMagnitude

    ''' <summary>
    ''' Compares an array of values to compute which has the smallest value.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be
    ''' examined.</param>
    ''' <returns>The smallest value in the array.</returns>
    ''' <remarks>
    ''' An empty array returns <c>System.Double.NaN</c>.
    ''' <see cref="MinValue"/> and <c>MinMagnitude</c> differ in their handling
    ''' of negative arguments: <c>MinValue({4, -5})</c> returns -5;
    ''' <c>MinMagnitude({4, -5})</c> returns 4.
    ''' <br/><example>
    ''' This example shows how to call <c>MinValue</c>.
    ''' <code>
    ''' Dim MinAll As System.Double =
    '''     OSNW.Math.MinValue({x0, y0, r0, x1, y1, r1})
    ''' </code></example>
    ''' </remarks>
    Public Function MinValue(
        ByVal ParamArray values() As System.Double) As System.Double

        ' Input checking.
        If values.Length.Equals(0) Then
            Return System.Double.NaN
        End If

        Dim Min As System.Double = values(0)
        For Each OneValue As System.Double In values
            If OneValue < Min Then
                Min = OneValue
            End If
        Next
        Return Min

    End Function ' MinValue

#End Region ' "Minimum/Maximum Implementations"

    ''' <summary>
    ''' Computes the geometric mean of an array of values.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be
    ''' evaluated.</param>
    ''' <returns>The geometric mean of the values.</returns>
    ''' <remarks>
    ''' An empty array returns <c>System.Double.NaN</c>.<br/>
    ''' The geometric mean is the nth root of the product of n values. It is
    ''' only valid for positive, non-zero, values. To avoid an exception, any
    ''' negative or zero value causes the result to be
    ''' <c>System.Double.NaN</c>.<br/>
    ''' <example>
    ''' This example shows how to call <c>GeometricMean</c>.
    ''' <code>
    ''' Dim M as System.Double = OSNW.Math.GeometricMean({2.0, 3.0, 6.0})
    ''' </code></example>
    ''' </remarks>
    Public Function GeometricMean(
        ByVal ParamArray values As System.Double()) As System.Double

        ' Input checking.
        If values.Length = 0 Then
            Return System.Double.NaN
        End If

        Dim Product As System.Double = 1.0
        For Each OneValue As System.Double In values
            If OneValue <= 0.0 Then
                Return System.Double.NaN ' Early exit.
            End If
            Product *= OneValue
        Next
        ' On getting here,
        Return System.Math.Pow(Product, 1.0 / values.Length)

    End Function ' GeometricMean

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
        Optional ByVal mode As System.MidpointRounding = OSNWDFLTMPR) _
        As System.Double

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
