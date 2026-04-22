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
    Public Const RAD030s As System.Single = PIs / 6.0
    Public Const RAD045s As System.Single = PIs / 4.0
    Public Const RAD060s As System.Single = PIs / 3.0
    Public Const RAD090s As System.Single = PIs / 2.0
    Public Const RAD120s As System.Single = 2.0 * PIs / 3.0
    Public Const RAD135s As System.Single = 3.0 * PIs / 4.0
    Public Const RAD150s As System.Single = 5.0 * PIs / 6.0
    Public Const RAD180s As System.Single = PIs
    Public Const RAD210s As System.Single = 7.0 * PIs / 6.0
    Public Const RAD225s As System.Single = 5.0 * PIs / 4.0
    Public Const RAD240s As System.Single = 4.0 * PIs / 3.0
    Public Const RAD270s As System.Single = 1.5 * PIs
    Public Const RAD300s As System.Single = 5.0 * PIs / 3.0
    Public Const RAD315s As System.Single = 7.0 * PIs / 4.0
    Public Const RAD330s As System.Single = 11.0 * PIs / 6.0
    Public Const RAD360s As System.Single = 2.0 * PIs
    Public Const RAD030d As System.Double = PId / 6.0
    Public Const RAD045d As System.Double = PId / 4.0
    Public Const RAD060d As System.Double = PId / 3.0
    Public Const RAD090d As System.Double = PId / 2.0
    Public Const RAD120d As System.Double = 2.0 * PId / 3.0
    Public Const RAD135d As System.Double = 3.0 * PId / 4.0
    Public Const RAD150d As System.Double = 5.0 * PId / 6.0
    Public Const RAD180d As System.Double = PId
    Public Const RAD210d As System.Double = 7.0 * PId / 6.0
    Public Const RAD225d As System.Double = 5.0 * PId / 4.0
    Public Const RAD240d As System.Double = 4.0 * PId / 3.0
    Public Const RAD270d As System.Double = 1.5 * PId
    Public Const RAD300d As System.Double = 5.0 * PId / 3.0
    Public Const RAD315d As System.Double = 7.0 * PId / 4.0
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
    ''' Checks for reasonable equality when using floating point values. A
    ''' difference of less than or equal to <c>tolerance</c> is considered to
    ''' establish equality.
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
    ''' This does the comparison based on an absolute numeric difference. Select
    ''' <paramref name="tolerance"/> such that it is a good representation of
    ''' zero, relative to other known or expected values.</remarks>
    Public Function EqualEnoughAbsolute(ByVal refVal As System.Double,
        ByVal tolerance As System.Double, ByVal otherVal As System.Double) _
        As System.Boolean

        ' No input checking.
        Return System.Double.Abs(otherVal - refVal) <= tolerance
    End Function ' EqualEnoughAbsolute

    ''' <summary>
    ''' Checks for reasonable equality to zero when using floating point values.
    ''' Any value less than or equal to <c>tolerance</c> from zero is considered
    ''' to equal zero.
    ''' </summary>
    ''' <param name="tolerance">Specifies the maximum offset from zero which
    ''' is assumed to represent zero.</param>
    ''' <param name="value">Specifies the value to be compared to zero.</param>
    ''' <returns><c>True</c> if <paramref name="value"/> is reasonably close to
    ''' zero; otherwise, <c>False</c>.</returns>
    ''' <remarks>Use this when an actual zero reference would cause a failure in
    ''' <see cref="EqualEnough(System.Double, System.Double, System.Double)"/>.
    ''' Select <paramref name="tolerance"/> such that it is a good
    ''' representation of zero relative to other known or expected
    ''' values.</remarks>
    Public Function EqualEnoughZero(ByVal tolerance As System.Double,
                                    ByVal value As System.Double) As System.Boolean

        ' No input checking.
        Return System.Double.Abs(value) <= System.Double.Abs(tolerance)
    End Function ' EqualEnoughZero

    ''' <summary>
    ''' Checks for reasonable equality, within a specified ratio, when using
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
    ''' by <paramref name="refVal"/>, to determine the maximum difference that
    ''' satisfies equality. Infinities are only equal to other infinities with
    ''' the same sign.
    ''' <br/>
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

        If System.Double.IsInfinity(refVal) Then
            Return System.Double.IsInfinity(otherVal) AndAlso
                System.Double.Sign(refVal).Equals(System.Double.Sign(otherVal))
            ' Early exit.
        End If

        Return System.Double.Abs(otherVal - refVal) <=
            System.Double.Abs(ratio * refVal)

    End Function ' EqualEnough

#End Region ' "EqualEnough Implementations"

#Region "Minimum/Maximum Implementations"

    ''' <summary>
    ''' Compares an array of values to compute which has the smallest value.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be examined.
    ''' Infinite values are allowed; <c>NaN</c> values are (effectively)
    ''' ignored.</param>
    ''' <returns>The smallest value in the array.</returns>
    ''' <remarks>
    ''' An empty array returns <see cref="System.Double.NaN"/>.
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
            Return System.Double.NaN ' Early exit.
        End If

        Dim Min As System.Double = values(0)
        For Each OneValue As System.Double In values
            If OneValue < Min Then
                Min = OneValue
            End If
        Next
        Return Min

    End Function ' MinValue

    ''' <summary>
    ''' Compares an array of values to compute which has the smallest magnitude.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be examined.
    ''' Infinite values are allowed; <c>NaN</c> values are (effectively)
    ''' ignored.</param>
    ''' <returns>The magnitude of smallest absolute value in the
    ''' array.</returns>
    ''' <remarks>
    ''' An empty array returns <see cref="System.Double.NaN"/>. When the array
    ''' is not empty, this always returns a positive magnitude.
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
            Return System.Double.NaN ' Early exit.
        End If

        Dim Min As System.Double = values(0)
        Dim AbsVal As System.Double
        For Each OneValue As System.Double In values
            AbsVal = System.Double.Abs(OneValue)
            If AbsVal < Min Then
                Min = AbsVal
            End If
        Next
        Return Min

    End Function ' MinMagnitude

    ''' <summary>
    ''' Compares an array of values to compute which has the greatest value.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be examined.
    ''' Infinite values are allowed; <c>NaN</c> values are (effectively)
    ''' ignored.</param>
    ''' <returns>The greatest value in the array.</returns>
    ''' <remarks>
    ''' An empty array returns <see cref="System.Double.NaN"/>. <c>MaxValue</c>
    ''' and <see cref="MaxMagnitude"/> differ in their handling of negative
    ''' arguments: <c>MaxValue({4, -5})</c> returns 4;
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
            Return System.Double.NaN ' Early exit.
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
    ''' Compares an array of values to compute which has the greatest magnitude.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be examined.
    ''' Infinite values are allowed; <c>NaN</c> values are (effectively)
    ''' ignored.</param>
    ''' <returns>The magnitude of greatest absolute value in the
    ''' array.</returns>
    ''' <remarks>
    ''' An empty array returns <see cref="System.Double.NaN"/>. When the array
    ''' is not empty, this always returns a positive magnitude.
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
            Return System.Double.NaN ' Early exit.
        End If

        Dim Max As System.Double ' Starts at zero.
        Dim AbsVal As System.Double
        For Each OneValue As System.Double In values
            AbsVal = System.Double.Abs(OneValue)
            If AbsVal > Max Then
                Max = AbsVal
            End If
        Next
        Return Max

    End Function ' MaxMagnitude

#End Region ' "Minimum/Maximum Implementations"

    ''' <summary>
    ''' Computes the geometric mean of an array of values.
    ''' </summary>
    ''' <param name="values">Specifies the array of values to be evaluated.
    ''' </param>
    ''' <returns>The geometric mean of the values.</returns>
    ''' <remarks>
    ''' The geometric mean is the nth root of the product of n values. It is
    ''' only valid for positive, non-zero, values. To avoid an exception, any
    ''' negative, zero, or <see cref="System.Double.NaN"/> value causes the
    ''' result to be <see cref="System.Double.NaN"/>.
    ''' <see cref="System.Double.PositiveInfinity"/> is allowed, but the
    ''' result will always be <see cref="System.Double.PositiveInfinity"/>. An
    ''' empty array returns <see cref="System.Double.NaN"/>.
    ''' <br/><example>
    ''' This example shows how to call <c>GeometricMean</c>.
    ''' <code>
    ''' Dim M as System.Double = OSNW.Math.GeometricMean({2.0, 3.0, 6.0})
    ''' </code></example>
    ''' </remarks>
    Public Function GeometricMean(
        ByVal ParamArray values As System.Double()) As System.Double

        ' Input checking.
        If values.Length = 0 Then
            Return System.Double.NaN ' Early exit.
        End If

        Dim Product As System.Double = 1.0
        For Each OneValue As System.Double In values
            If OneValue <= 0.0 Then
                Return System.Double.NaN ' Early exit.
            End If
            Product *= OneValue
        Next
        ' On getting here,
        Return System.Double.Pow(Product, 1.0 / values.Length)

    End Function ' GeometricMean

    ''' <summary>
    ''' Rounds a value to the nearest multiple of the specified
    ''' <c>multipleOf</c>, using the specified <c>MidpointRounding</c> mode.
    ''' </summary>
    ''' <param name="multipleOf">Specifies the value to which
    ''' <paramref name="value"/> should be rounded.</param>
    ''' <param name="value">Specifies the  number to be rounded.</param>
    ''' <param name="mode">Optional. Specifies how to round
    ''' <paramref name="value"/> if it is midway between two potential results.
    ''' If not specified, <see cref="System.MidpointRounding.ToEven"/> is
    ''' assumed.</param>
    ''' <returns>The nearest multiple of the specified
    ''' <paramref name="multipleOf"/>, using the specified
    ''' <see cref="System.MidpointRounding"/> mode. If the rounding point value
    ''' is halfway between two digits, one of which is even and the other odd,
    ''' then <paramref name="mode"/> determines which of the two is used. Also
    ''' returns <see cref="System.Double.NaN"/> when
    ''' <paramref name="multipleOf"/> is negative, zero, infinite or
    ''' <see cref="System.Double.NaN"/>. Also returns
    ''' <see cref="System.Double.NaN"/> when <paramref name="value"/> is
    ''' infinite or <see cref="System.Double.NaN"/>.</returns>
    Public Function RoundTo(ByVal multipleOf As System.Double,
        ByVal value As System.Double,
        Optional ByVal mode As System.MidpointRounding = OSNWDFLTMPR) _
        As System.Double

        ' Input checking.
        If multipleOf <= 0.0 OrElse
            System.Double.IsInfinity(multipleOf) OrElse
            System.Double.IsNaN(multipleOf) Then

            Return Double.NaN ' Early exit.
        End If
        If System.Double.IsInfinity(value) OrElse
            System.Double.IsNaN(value) Then

            Return Double.NaN ' Early exit.
        End If

        'Dim Multiples As System.Double = value / multipleOf
        'Dim RoundMults As System.Double = System.Double.Round(Multiples, mode)
        'Dim Result As System.Double = RoundMults * multipleOf
        'Return Result

        Return System.Double.Round(value / multipleOf, mode) * multipleOf

    End Function ' RoundTo

End Module ' Math
