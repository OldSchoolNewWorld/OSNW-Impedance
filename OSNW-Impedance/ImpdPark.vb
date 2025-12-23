Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

' This document contains items no longer in current use which may be useful later.

Partial Public Structure Impedance

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance, when the current
    '    ''' instance appears directly
    '    ''' on the R=Z0 circle.
    '    ''' </summary>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns>
    '    ''' Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' A succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function OnREqualsZ0(
    '        ByVal z0 As System.Double, ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' Test data C: At the open circuit point on the right.
    '        ' Test data D: At the center.
    '        ' Test data E: On R=Z0 circle, above resonance line.
    '        ' Test data F: On R=Z0 circle, below resonance line.

    '        Dim CurrentX As System.Double = Me.Reactance
    '        If Impedance.EqualEnoughZero(CurrentX, Impedance.IMPDTOLERANCE0 * z0) Then
    '            ' This happens at two places.

    '            If System.Double.IsInfinity(Me.Resistance) Then
    '                ' Test data C: At the open circuit point on the right.
    '                Return False
    '            End If

    '            ' Test data D1: At the center.
    '            ' Z is at the center point and already has a conjugate match.
    '            transformations = {
    '                New Transformation With {
    '                    .Style = TransformationStyles.None}
    '            }
    '            Return True

    '        Else
    '            ' Z is elsewhere on the perimeter of the R=Z0 circle and only needs
    '            ' a reactance.
    '            Dim Style As TransformationStyles
    '            If CurrentX > 0.0 Then
    '                ' Test data E: On R=Z0 circle, above resonance line.
    '                ' CCW on an R-circle needs a series capacitor.
    '                Style = TransformationStyles.SeriesCap
    '            Else
    '                ' Test data F: On R=Z0 circle, below resonance line.
    '                ' CW on an R-circle needs a series inductor.
    '                Style = TransformationStyles.SeriesInd
    '            End If
    '            transformations = {
    '                New Transformation With {
    '                    .Style = Style,
    '                    .Value1 = -CurrentX}
    '                }
    '            Return True
    '        End If
    '    End Function ' OnREqualsZ0

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance specified by
    '    ''' <paramref name="z0"/>, when the current instance appears directly on the
    '    ''' G=Y0 circle.
    '    ''' </summary>
    '    ''' <param name="z0">Specifies the characteristic impedance to which the
    '    ''' current instance should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns>
    '    ''' Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' <paramref name="z0"/> is the characteristic impedance to which the
    '    ''' current instance should be matched. It should have a practical value
    '    ''' with regard to the impedance values involved.
    '    ''' A succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function OnGEqualsY0(ByVal z0 As System.Double,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' A: At the short circuit point.
    '        ' D: At the center.
    '        ' J: On G=Y0 circle, above resonance line.
    '        ' K: On G=Y0 circle, below resonance line.

    '        Dim CurrentB As System.Double = Me.ToAdmittance().Susceptance
    '        If Impedance.EqualEnoughZero(
    '            CurrentB, Impedance.IMPDTOLERANCE0 * z0) Then
    '            ' This happens at two places.

    '            If Impedance.EqualEnoughZero(
    '                Me.Resistance, Impedance.IMPDTOLERANCE0 * z0) Then
    '                ' Test data A: At the short circuit point.
    '                Return False
    '            End If

    '            ' Test data D: At the center.
    '            ' Z is at the center point and already has a conjugate match.
    '            transformations = {
    '                New Transformation With {
    '                    .Style = TransformationStyles.None}
    '            }
    '            Return True

    '        Else
    '            ' Z is elsewhere on the perimeter of the Y=G0 circle and only needs
    '            ' a reactance.
    '            Dim Style As TransformationStyles
    '            Dim DeltaZ As Impedance = New Admittance(0.0, -CurrentB).ToImpedance
    '            If CurrentB > 0.0 Then
    '                ' Test data K: On G=Y0 circle, below resonance line.
    '                ' CCW on a G-circle needs a shunt inductor.
    '                Style = TransformationStyles.ShuntInd
    '            Else
    '                ' Test data J: On G=Y0 circle, above resonance line.
    '                ' CW on a G-circle needs a shunt capacitor.
    '                Style = TransformationStyles.ShuntCap
    '            End If
    '            transformations = {
    '                New Transformation With {
    '                    .Style = Style,
    '                    .Value1 = DeltaZ.Reactance}
    '                }
    '            Return True
    '        End If

    '    End Function ' OnGEqualsY0

    '    '''' <summary>
    '    ''''  Processes one intersection found in
    '    ''''  <see cref="M:InsideREqualsZ0(z0, transformations)"/>".>
    '    '''' </summary>
    '    '''' <param name="mainCirc">Specifies an arbitrary
    '    '''' <see cref="SmithMainCircle"/> reference for calculations.</param>
    '    '''' <param name="intersection">Specifies the Cartesian coordinates of one
    '    '''' intersection of R- and G-circles.</param>
    '    '''' <param name="transformation"> Returns a <see cref="Transformation"/>
    '    '''' that can be used to match a load impedance, located at the specified
    '    '''' <paramref name="intersection"/>, to match a source impedance.</param>
    '    '''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    '    '''' results in a conjugate match for the current instance; otherwise,
    '    '''' <c>False</c>.</returns>
    '    '''' <remarks>
    '    '''' <para> An assumption is made that the calling code has determined that
    '    '''' the current instance lies in the expected position. Failure to meet that
    '    '''' assumption could result in an invalid or incomplete result. </para>
    '    '''' </remarks>
    '    Private Function InsideREqualsZ0(ByVal mainCirc As SmithMainCircle,
    '        ByVal intersection As OSNW.Numerics.PointD,
    '        ByRef transformation As Transformation) As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        Try

    '            ' First move, to the image impedance.
    '            Dim ImageY As Admittance =
    '                mainCirc.GetYFromPlot(intersection.X, intersection.Y)
    '            Dim DiffImageB As System.Double =
    '                ImageY.Susceptance - Me.ToAdmittance().Susceptance

    '            ' Second move, to the center.
    '            Dim ImageZ As Impedance =
    '                mainCirc.GetZFromPlot(intersection.X, intersection.Y)
    '            Dim DiffFinalX As System.Double = -ImageZ.Reactance

    '            ' Select the transformations, based on the location of the
    '            ' intersection relative to the resonance line.
    '            If intersection.Y > mainCirc.GridCenterY Then
    '                ' Intersection above the resonance line.

    '                ' Use a shunt inductor to move CCW the G-circle to the R=Z0
    '                ' circle, then use a series capacitor to move CCW on the R=Z0
    '                ' circle to the center.
    '                transformation = New Transformation With {
    '                    .Style = TransformationStyles.ShuntIndSeriesCap,
    '                    .Value1 = DiffImageB,
    '                    .Value2 = DiffFinalX
    '                }
    '            Else
    '                ' Intersection below the resonance line.

    '                '  Use a shunt capacitor to move CW on the G-circle to the R=Z0
    '                '  circle, then use a series inductor to move CW on the R=Z0
    '                '  circle to the center.
    '                transformation = New Transformation With {
    '                    .Style = TransformationStyles.ShuntCapSeriesInd,
    '                    .Value1 = DiffImageB,
    '                    .Value2 = DiffFinalX
    '                }
    '            End If

    '        Catch CaughtEx As Exception
    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            'Throw New System.InvalidOperationException(
    '            '    $"Failed to process {CaughtBy}.")
    '            Return False
    '        End Try

    '        ' On getting this far,
    '        Return True

    '    End Function ' InsideREqualsZ0

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance specified by
    '    ''' <paramref name="z0"/>, when the current instance appears inside the
    '    ''' R=Z0 circle.
    '    ''' </summary>
    '    ''' <param name="z0">Specifies the characteristic impedance to which the
    '    ''' current instance should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns>
    '    ''' Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' <paramref name="z0"/> is the characteristic impedance to which the
    '    ''' current instance should be matched. It should have a practical value
    '    ''' with regard to the impedance values involved.
    '    ''' A succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InsideREqualsZ0(ByVal z0 As System.Double,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' The first move will be to the intersection of the R=Z0 circle and the
    '        ' G-circle that includes the load impedance. From inside the R=Z0
    '        ' circle, there are two ways to proceed:
    '        '  - Use a shunt capacitor to move CW on the G-circle to the R=Z0
    '        '  circle, then use a series inductor to move CW on the R=Z0 circle to
    '        '  the center.
    '        '  - Use a shunt inductor to move CCW on the G-circle to the R=Z0
    '        '  circle, then use a series capacitor to move CCW on the R=Z0 circle to
    '        '  the center.
    '        ' Would there ever be a reason to prefer one approach over the other?
    '        '  - To favor high- or low-pass?
    '        '  - To favor the shortest first path?
    '        '  - To favor availability of suitable components for the frequency of
    '        '      interest?

    '        ' Determine the circles and their intersections.
    '        'Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
    '        Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
    '        Dim CircR As New RCircle(MainCirc, z0)
    '        Dim CircG As New GCircle(MainCirc, Me.ToAdmittance().Conductance)
    '        Dim Intersections _
    '            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
    '                GenericCircle.GetIntersections(CircR, CircG)

    '        '' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
    '        '' RESULTS ARE KNOWN TO BE CORRECT.
    '        '' There should now be two intersection points, with one above, and one
    '        '' below, the resonance line.
    '        'If Intersections.Count <> 2 Then
    '        '    'Dim CaughtBy As System.Reflection.MethodBase =
    '        '    '    System.Reflection.MethodBase.GetCurrentMethod
    '        '    Throw New System.ApplicationException(Impedance.MSGIIC)
    '        'End If
    '        '' The X values should match. Check for reasonable equality when using
    '        '' floating point values.
    '        'If Not EqualEnough(Intersections(0).X, Intersections(0).X) Then
    '        '    'Dim CaughtBy As System.Reflection.MethodBase =
    '        '    '    System.Reflection.MethodBase.GetCurrentMethod
    '        '    Throw New System.ApplicationException("X values do not match.")
    '        'End If
    '        '' The Y values should be the same distance above and below the
    '        '' resonance line. Check for reasonable equality when using floating
    '        '' point values.
    '        'Dim Offset0 As System.Double =
    '        '    System.Math.Abs(Intersections(0).Y - MainCirc.GridCenterY)
    '        'Dim Offset1 As System.Double =
    '        '    System.Math.Abs(Intersections(1).Y - MainCirc.GridCenterY)
    '        'If Not EqualEnough(Offset1, Offset0) Then
    '        '    'Dim CaughtBy As System.Reflection.MethodBase =
    '        '    '    System.Reflection.MethodBase.GetCurrentMethod
    '        '    Throw New System.ApplicationException("Y offsets do not match.")
    '        'End If

    '        ' There are now two intersection points, with one above and one below
    '        ' the resonance line. The X values match. The Y values are the same
    '        ' distance above and below the resonance line.

    '        ' Expect two valid solutions, one to each intersection.
    '        Dim Transformation0 As Transformation
    '        If Not Me.InsideREqualsZ0(
    '            MainCirc, Intersections(0), Transformation0) Then

    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New System.ApplicationException("Transformation 0 failed.")
    '        End If
    '        Dim Transformation1 As Transformation
    '        If Not Me.InsideREqualsZ0(
    '            MainCirc, Intersections(1), Transformation1) Then

    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New System.ApplicationException("Transformation 1 failed.")
    '        End If

    '        '' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
    '        '' RESULTS ARE KNOWN TO BE CORRECT.
    '        '' There should now be two valid solutions that match to Z=Z0+j0.0.
    '        '' Check first solution.
    '        'If Not ValidateTransformation(z0, Transformation0) Then
    '        '    Return False
    '        'End If
    '        '' Check second solution.
    '        'If Not ValidateTransformation(z0, Transformation1) Then
    '        '    Return False
    '        'End If

    '        ' On getting this far,
    '        Return True

    '    End Function ' InsideREqualsZ0

    '    ''' <summary>
    '    '''  Processes one intersection found in
    '    '''  <see cref="M:InsideGEqualsY0(z0, transformations)"/>".>
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies an arbitrary
    '    ''' <see cref="SmithMainCircle"/> reference for calculations.</param>
    '    ''' <param name="intersection">Specifies the Cartesian coordinates of one
    '    ''' intersection of R- and G-circles.</param>
    '    ''' <param name="transformation"> Returns a <see cref="Transformation"/>
    '    ''' that can be used to match a load impedance, located at the specified
    '    ''' <paramref name="intersection"/>, to match a source impedance.</param>
    '    ''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    '    ''' results in a conjugate match for the current instance; otherwise,
    '    ''' <c>False</c>.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' </remarks>
    '    Private Function InsideGEqualsY0(ByVal mainCirc As SmithMainCircle,
    '        ByVal intersection As OSNW.Numerics.PointD,
    '        ByRef transformation As Transformation) As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        Try

    '            ' First move, to the image impedance.
    '            Dim ImageZ As Impedance =
    '                mainCirc.GetZFromPlot(intersection.X, intersection.Y)
    '            Dim DiffImageX As System.Double =
    '                ImageZ.Reactance - Me.Reactance

    '            ' Second move, to the center.
    '            Dim ImageY As Admittance =
    '                mainCirc.GetYFromPlot(intersection.X, intersection.Y)
    '            Dim DiffFinalG As System.Double = -ImageY.Susceptance
    '            Dim FinalY As New Admittance(0.0, DiffFinalG)
    '            Dim FinalX As Impedance = FinalY.ToImpedance

    '            ' Select the transformations, based on the location of the
    '            ' intersection relative to the resonance line.
    '            If intersection.Y > mainCirc.GridCenterY Then
    '                ' Intersection above the resonance line.

    '                ' Use a series inductor to move CW on the R-circle to the G=Y0
    '                ' circle, then use a shunt capacitor to move CW on the G=Y0
    '                ' circle to the center.
    '                '               transformation = New Transformation With {
    '                '                   .Style = TransformationStyles.SeriesIndShuntCap,
    '                '                   .Value1 = DiffImageX,
    '                '                   .Value2 = DiffFinalY
    '                '               }
    '                transformation = New Transformation With {
    '                    .Style = TransformationStyles.SeriesIndShuntCap,
    '                    .Value1 = DiffImageX,
    '                    .Value2 = FinalX.Reactance
    '                }
    '            Else
    '                ' Intersection below the resonance line.

    '                '  Use a series capacitor to move CCW on the R-circle to the G=Y0
    '                '  circle, then use a shunt inductor to move CCW on the G=Y0
    '                '  circle to the center.
    '                transformation = New Transformation With {
    '                    .Style = TransformationStyles.SeriesCapShuntInd,
    '                    .Value1 = DiffImageX,
    '                    .Value2 = FinalX.Reactance
    '                }
    '            End If

    '        Catch CaughtEx As Exception
    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            'Throw New System.InvalidOperationException(
    '            '    $"Failed to process {CaughtBy}.")
    '            Return False
    '        End Try

    '        ' On getting this far,
    '        Return True

    '    End Function ' InsideGEqualsY0

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance of
    '    ''' <paramref name="mainCirc"/>, when the current instance appears inside the
    '    ''' G=Y0 circle.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    '    ''' which the current instance is associated.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns>
    '    ''' Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    '    ''' which the current instance should be matched. It should have a practical
    '    ''' value with regard to the impedance values involved.
    '    ''' A succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InsideGEqualsY0(ByVal mainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' The first move will be to the intersection of the G=Y0 circle and the
    '        ' R-circle that contains the load impedance. From inside the G=Y0
    '        ' circle, there are two ways to proceed:
    '        '  - Use a series inductor to move CW on the R-circle to the G=Y0
    '        '  circle, then use a shunt capacitor to move CW on the G=Y0 circle to
    '        '  the center.
    '        '  - Use a series capacitor to move CCW on the R-circle to the G=Y0
    '        '  circle, then use a shunt inductor to move CCW on the G=Y0 circle to
    '        '  the center.
    '        ' Would there ever be a reason to prefer one approach over the other?
    '        '  - To favor high- or low-pass?
    '        '  - To favor the shortest first path?

    '        ' Determine the circles and their intersections.
    '        Dim CircG As New GCircle(mainCirc, 1.0 / mainCirc.Z0)
    '        Dim CircR As New RCircle(mainCirc, Me.Resistance)
    '        Dim Intersections _
    '            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
    '                GenericCircle.GetIntersections(CircR, CircG)

    '        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
    '        ' RESULTS ARE KNOWN TO BE CORRECT.
    '        ' There should now be two intersection points, with one above, and one
    '        ' below, the resonance line.
    '        If Intersections.Count <> 2 Then
    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New System.ApplicationException(Impedance.MSGIIC)
    '        End If
    '        ' The X values should match. Check for reasonable equality when using
    '        ' floating point values.
    '        If Not EqualEnough(Intersections(0).X, Intersections(0).X,
    '                           GRAPHICTOLERANCE) Then
    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New System.ApplicationException("X values do not match.")
    '        End If
    '        ' The Y values should be the same distance above and below the
    '        ' resonance line. Check for reasonable equality when using floating
    '        ' point values.
    '        Dim Offset0 As System.Double =
    '            System.Math.Abs(Intersections(0).Y - mainCirc.GridCenterY)
    '        Dim Offset1 As System.Double =
    '            System.Math.Abs(Intersections(1).Y - mainCirc.GridCenterY)
    '        If Not EqualEnough(Offset1, Offset0, GRAPHICTOLERANCE) Then
    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New System.ApplicationException("Y offsets do not match.")
    '        End If

    '        ' There are now two intersection points, with one above and one below
    '        ' the resonance line. The X values match. The Y values are the same
    '        ' distance above and below the resonance line.

    '        ' Expect two valid solutions, one to each intersection.
    '        Dim Transformation0 As Transformation
    '        If Not Me.InsideGEqualsY0(
    '            mainCirc, Intersections(0), Transformation0) Then

    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New System.ApplicationException("Transformation 0 failed.")
    '        End If
    '        Dim Transformation1 As Transformation
    '        If Not Me.InsideGEqualsY0(
    '            mainCirc, Intersections(1), Transformation1) Then

    '            'Dim CaughtBy As System.Reflection.MethodBase =
    '            '    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New System.ApplicationException("Transformation 1 failed.")
    '        End If

    '        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
    '        ' RESULTS ARE KNOWN TO BE CORRECT.
    '        ' There should now be two valid solutions that match to Z=Z0+j0.0.
    '        Dim ExpectZ As New Impedance(mainCirc.Z0, 0)
    '        ' Check first solution.
    '        If Not ValidateTransformation(mainCirc, ExpectZ, Transformation0) Then
    '            Return False
    '        End If
    '        ' Check second solution.
    '        If Not ValidateTransformation(mainCirc, ExpectZ, Transformation1) Then
    '            Return False
    '        End If

    '        ' On getting this far,
    '        Return True

    '    End Function ' InsideGEqualsY0

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance of
    '    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    '    ''' top central area. This is to have the first move go CW.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    '    ''' which the current instance is associated.</param>
    '    ''' <param name="transformations">Accumulates an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to a source impedance.</param>
    '    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    '    ''' Also returns, by reference in <paramref name="transformations"/>, the
    '    ''' components to construct the match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    '    ''' which the current instance should be matched. It should have a practical
    '    ''' value with regard to the impedance values involved. A succcessful
    '    ''' process might result in an empty <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InTopCenterCW(ByVal mainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        Dim Y As Admittance = Me.ToAdmittance()

    '        ' Move CW on the G-circle to reach the R=Z0 circle. Use a shunt
    '        ' capacitor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?

    '        ' Determine the circle intersections.
    '        Dim CircG As New GCircle(mainCirc, Y.Conductance)
    '        Dim CircR As New RCircle(mainCirc, mainCirc.Z0)
    '        Dim Intersections _
    '            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
    '                GenericCircle.GetIntersections(CircR, CircG)

    '        ' Process each intersection.
    '        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

    '            ' Determine the changes to take place.
    '            Dim ImageY As Admittance =
    '                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaB As System.Double =
    '                ImageY.Susceptance - Y.Susceptance
    '            Dim DeltaY As New Admittance(0, DeltaB)
    '            Dim DeltaZ As Impedance = DeltaY.ToImpedance
    '            Dim ImageZ As Impedance =
    '                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaX As System.Double = -ImageZ.Reactance

    '            ' Set up the transformation.
    '            Dim Trans As New Transformation
    '            With Trans
    '                If OneIntersection.Y > mainCirc.GridCenterY Then
    '                    ' The short first move. Now CCW on R-Circle.
    '                    .Style = TransformationStyles.ShuntCapSeriesCap
    '                Else
    '                    ' The long first move. Now CW on R-Circle.
    '                    .Style = TransformationStyles.ShuntCapSeriesInd
    '                End If
    '                .Value1 = DeltaZ.Reactance
    '                .Value2 = DeltaX
    '            End With

    '            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
    '            ' RESULTS ARE KNOWN TO BE CORRECT.
    '            ' There should now be a valid solution that matches to Z=Z0+j0.0.
    '            If Not ValidateTransformation(
    '                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
    '                Return False
    '            End If

    '            Dim CurrTransCount As System.Int32 = transformations.Length
    '            ReDim Preserve transformations(CurrTransCount)
    '            transformations(CurrTransCount) = Trans

    '        Next

    '        ' On getting this far,
    '        Return True

    '    End Function ' InTopCenterCW

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance of
    '    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    '    ''' top central area. This is to have the first move go CCW.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    '    ''' which the current instance is associated.</param>
    '    ''' <param name="transformations">Accumulates an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to a source impedance.</param>
    '    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    '    ''' Also returns, by reference in <paramref name="transformations"/>, the
    '    ''' components to construct the match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    '    ''' which the current instance should be matched. It should have a practical
    '    ''' value with regard to the impedance values involved. A succcessful
    '    ''' process might result in an empty <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InTopCenterCCW(ByVal mainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' Move CCW on the R-circle to reach the G=Y0 circle. Use a
    '        ' series capacitor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?

    '        ' Determine the circle intersections.
    '        Dim CircG As New GCircle(mainCirc, mainCirc.Y0)
    '        Dim CircR As New RCircle(mainCirc, Me.Resistance)
    '        Dim Intersections _
    '            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
    '                GenericCircle.GetIntersections(CircR, CircG)

    '        ' Process each intersection.
    '        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

    '            ' Determine the changes to take place.
    '            Dim ImageZ As Impedance =
    '                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaX As System.Double =
    '                ImageZ.Reactance - Me.Reactance
    '            Dim ImageY As Admittance =
    '                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaB As System.Double = -ImageY.Susceptance
    '            Dim FinalY As New Admittance(0.0, DeltaB)
    '            Dim FinalZ As Impedance = FinalY.ToImpedance

    '            ' Set up the transformation.
    '            Dim Trans As New Transformation
    '            If OneIntersection.Y > mainCirc.GridCenterY Then
    '                ' The short first move. Now CCW on R-Circle.
    '                Trans.Style = TransformationStyles.SeriesCapShuntCap
    '            Else
    '                ' The long first move. Now CW on R-Circle.
    '                Trans.Style = TransformationStyles.SeriesCapShuntInd
    '            End If
    '            With Trans
    '                .Value1 = DeltaX
    '                .Value2 = FinalZ.Reactance
    '            End With

    '            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
    '            ' RESULTS ARE KNOWN TO BE CORRECT.
    '            ' There should now be a valid solution that matches to Z=Z0+j0.0.
    '            If Not ValidateTransformation(
    '                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
    '                Return False
    '            End If

    '            Dim CurrTransCount As System.Int32 = transformations.Length
    '            ReDim Preserve transformations(CurrTransCount)
    '            transformations(CurrTransCount) = Trans

    '        Next

    '        ' On getting this far,
    '        Return True

    '    End Function ' InTopCenterCCW

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance specified by
    '    ''' <paramref name="z0"/>, when the current instance appears in the top
    '    ''' central area.
    '    ''' </summary>
    '    ''' <param name="z0">Specifies the characteristic impedance to which the
    '    ''' current instance should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns>
    '    ''' Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' <paramref name="z0"/> is the characteristic impedance to which the
    '    ''' current instance should be matched. It should have a practical value
    '    ''' with regard to the impedance values involved.
    '    ''' A succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InTopCenter(ByVal z0 As System.Double,
    '        ByRef MainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' Move CW on the G-circle to reach the R=Z0 circle. Use a shunt
    '        ' capacitor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?

    '        If Not Me.InTopCenterCW(
    '            MainCirc, transformations) Then

    '            Return False
    '        End If

    '        ' Move CCW on the R-circle to reach the G=Y0 circle. Use a
    '        ' series capacitor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?
    '        If Not Me.InTopCenterCCW(
    '            MainCirc, transformations) Then

    '            Return False
    '        End If

    '        ' On getting this far,
    '        Return True

    '    End Function ' InTopCenter

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance of
    '    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    '    ''' bottom central area. This is to have the first move go CW.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    '    ''' which the current instance is associated.</param>
    '    ''' <param name="transformations">Accumulates an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to a source impedance.</param>
    '    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    '    ''' Also returns, by reference in <paramref name="transformations"/>, the
    '    ''' components to construct the match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    '    ''' which the current instance should be matched. It should have a practical
    '    ''' value with regard to the impedance values involved. A succcessful
    '    ''' process might result in an empty <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InBottomCenterCW(ByVal mainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' Move CW on the R-circle to reach the G=Y0 circle. Use a series
    '        ' inductor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?

    '        ' Determine the circle intersections.
    '        Dim CircG As New GCircle(mainCirc, mainCirc.Y0)
    '        Dim CircR As New RCircle(mainCirc, Me.Resistance)
    '        Dim Intersections _
    '            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
    '                GenericCircle.GetIntersections(CircR, CircG)

    '        ' Process each intersection.
    '        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

    '            ' Determine the changes to take place.
    '            Dim ImageZ As Impedance =
    '                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaX As System.Double =
    '                ImageZ.Reactance - Me.Reactance
    '            Dim ImageY As Admittance =
    '                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaB As System.Double = -ImageY.Susceptance
    '            Dim FinalY As New Admittance(0.0, DeltaB)
    '            Dim FinalZ As Impedance = FinalY.ToImpedance

    '            ' Set up the transformation.
    '            Dim Trans As New Transformation
    '            If OneIntersection.Y > mainCirc.GridCenterY Then
    '                ' The long first move. Now CW on G-Circle.
    '                Trans.Style = TransformationStyles.SeriesIndShuntCap
    '            Else
    '                ' The short first move. Now CCW on G-Circle.
    '                Trans.Style = TransformationStyles.SeriesIndShuntInd
    '            End If
    '            With Trans
    '                .Value1 = DeltaX
    '                .Value2 = FinalZ.Reactance
    '            End With

    '            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
    '            ' RESULTS ARE KNOWN TO BE CORRECT.
    '            ' There should now be a valid solution that matches to Z=Z0+j0.0.
    '            If Not ValidateTransformation(
    '                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
    '                Return False
    '            End If

    '            Dim CurrTransCount As System.Int32 = transformations.Length
    '            ReDim Preserve transformations(CurrTransCount)
    '            transformations(CurrTransCount) = Trans

    '        Next

    '        ' On getting this far,
    '        Return True

    '    End Function ' InBottomCenterCW

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance of
    '    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    '    ''' bottom central area. This is to have the first move go CCW.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    '    ''' which the current instance is associated.</param>
    '    ''' <param name="transformations">Accumulates an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to a source impedance.</param>
    '    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    '    ''' Also returns, by reference in <paramref name="transformations"/>, the
    '    ''' components to construct the match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    '    ''' which the current instance should be matched. It should have a practical
    '    ''' value with regard to the impedance values involved. A succcessful
    '    ''' process might result in an empty <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InBottomCenterCCW(ByVal mainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        Dim Y As Admittance = Me.ToAdmittance()

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' Move CCW on the G-circle to reach the R=Z0 circle. Use a
    '        ' shunt inductor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?

    '        ' Determine the circle intersections.
    '        Dim CircG As New GCircle(mainCirc, Y.Conductance)
    '        Dim CircR As New RCircle(mainCirc, mainCirc.Z0)
    '        Dim Intersections _
    '            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
    '                GenericCircle.GetIntersections(CircR, CircG)

    '        ' Process each intersection.
    '        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

    '            ' Determine the changes to take place.
    '            Dim ImageY As Admittance =
    '                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaB As System.Double =
    '                ImageY.Susceptance - Y.Susceptance
    '            Dim FixupY As New Admittance(0.0, DeltaB)
    '            Dim FixupZ As Impedance = FixupY.ToImpedance
    '            Dim ImageZ As Impedance =
    '                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
    '            Dim DeltaX As System.Double = -ImageZ.Reactance

    '            ' Set up the transformation.
    '            Dim Trans As New Transformation
    '            If OneIntersection.Y > mainCirc.GridCenterY Then
    '                ' The short first move. Now CCW on R-Circle.
    '                Trans.Style = TransformationStyles.ShuntIndSeriesCap
    '            Else
    '                ' The long first move. Now CW on R-Circle.
    '                Trans.Style = TransformationStyles.ShuntIndSeriesInd
    '            End If
    '            With Trans
    '                .Value1 = FixupZ.Reactance
    '                .Value2 = DeltaX
    '            End With

    '            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
    '            ' RESULTS ARE KNOWN TO BE CORRECT.
    '            ' There should now be a valid solution that matches to Z=Z0+j0.0.
    '            If Not ValidateTransformation(
    '                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
    '                Return False
    '            End If

    '            Dim CurrTransCount As System.Int32 = transformations.Length
    '            ReDim Preserve transformations(CurrTransCount)
    '            transformations(CurrTransCount) = Trans

    '        Next

    '        ' On getting this far,
    '        Return True

    '    End Function ' InBottomCenterCCW

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance specified by
    '    ''' <paramref name="z0"/>, when the current instance appears in the Bottom
    '    ''' central area.
    '    ''' </summary>
    '    ''' <param name="z0">Specifies the characteristic impedance to which the
    '    ''' current instance should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns>
    '    ''' Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' <paramref name="z0"/> is the characteristic impedance to which the
    '    ''' current instance should be matched. It should have a practical value
    '    ''' with regard to the impedance values involved.
    '    ''' A succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InBottomCenter(ByVal z0 As System.Double,
    '        ByRef MainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' Move CW on the G-circle to reach the R=Z0 circle. Use a shunt
    '        ' capacitor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?

    '        If Not Me.InBottomCenterCW(
    '            MainCirc, transformations) Then

    '            Return False
    '        End If

    '        ' Move CCW on the R-circle to reach the G=Y0 circle. Use a
    '        ' series capacitor. Two choices where to end.
    '        ' Would there ever be a case to prefer the first or second
    '        ' intersection? Maybe to favor high- or low-pass?
    '        If Not Me.InBottomCenterCCW(
    '            MainCirc, transformations) Then

    '            Return False
    '        End If

    '        ' On getting this far,
    '        Return True

    '    End Function ' InBottomCenter

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance specified by
    '    ''' <paramref name="z0"/>, when the current instance appears in the top or
    '    ''' bottom central area.
    '    ''' </summary>
    '    ''' <param name="z0">Specifies the characteristic impedance to which the
    '    ''' current instance should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns>
    '    ''' Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the current instance lies in the expected position. Failure to meet that
    '    ''' assumption could result in an invalid or incomplete result. </para>
    '    ''' <paramref name="z0"/> is the characteristic impedance to which the
    '    ''' current instance should be matched. It should have a practical value
    '    ''' with regard to the impedance values involved.
    '    ''' A succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Private Function InRemainder(ByVal z0 As System.Double,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' Assign the outer circle.
    '        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
    '        'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.

    '        ' Try to solve in the appropriate space.
    '        If Me.Reactance > 0.0 Then
    '            ' Z is ABOVE the resonance line, between the G=Y0 and R=Z0 circles.
    '            Return Me.InTopCenter(z0, MainCirc, transformations) ' O.
    '        ElseIf Me.Reactance < 0.0 Then
    '            ' Z is BELOW the resonance line, between the G=Y0 and R=Z0 circles.
    '            Return Me.InBottomCenter(z0, MainCirc, transformations) ' P.
    '        End If

    '        Return False ' DEFAULT UNTIL IMPLEMENTED.

    '    End Function ' InRemainder

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the current instance (load
    '    ''' impedance) to the source characteristic impedance of
    '    ''' <paramref name="mainCirc"/>.
    '    ''' </summary> 
    '    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    '    ''' which the current instance is associated.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    '    ''' to match a source impedance.</param>
    '    ''' <returns><c>True</c> if a conjugate match solution is found and also
    '    ''' returns the components to construct the match; otherwise, <c>False</c>.
    '    ''' </returns>
    '    ''' <remarks>
    '    ''' An already-matched impedance returns <c>True</c>, with
    '    ''' <c>Nothing</c>/<c>Null</c> for <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Public Function TrySelectMatchLayout(ByVal mainCirc As SmithMainCircle,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' DEV: This development implementation is based on selection of pure
    '        ' impedances. A future derivation might need to select the nearest
    '        ' commonly available component values, as a practical consideration. In
    '        ' that case, the math should be changed to add an impedance with actual
    '        ' R/X values.

    '        ' The terminology here relates to solving conjugate matches on a Smith
    '        ' Chart.

    '        ' Chart location cases:
    '        ' A: At the short circuit point.
    '        ' B: Anywhere else on the perimeter. R=0.0.
    '        ' C: At the open circuit point on the right.
    '        ' D1: At the center.
    '        ' On the R=Z0 circle.
    '        '     Omit: On the resonance line. Already covered by C or D.
    '        '     E: On R=Z0 circle, above resonance line.
    '        '     F: On R=Z0 circle, below resonance line.
    '        ' Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
    '        '     G1: Inside R=Z0 circle, above resonance line.
    '        '     G50: Inside R=Z0 circle, above resonance line. Z0=50.
    '        '     H1: Inside R=Z0 circle, on line.
    '        '     I1: Inside R=Z0 circle, below resonance line.
    '        ' On the G=Y0 circle.
    '        '     Omit: On the resonance line. Already either A or D.
    '        '     J: On G=Y0 circle, above resonance line.
    '        '     K: On G=Y0 circle, below resonance line.
    '        ' Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
    '        '     L1: Inside G=Y0 circle, above resonance line.
    '        '     L75: Inside G=Y0 circle, above resonance line. Z0=75.
    '        '     M1: Inside G=Y0 circle, on line.
    '        '     N1: Inside G=Y0 circle, below line.
    '        ' O: In the top center.
    '        '     O1: In the top center.
    '        '     O50: In the top center. Z0=50.
    '        ' P: In the bottom center.
    '        '     P1: In the bottom center.
    '        '     P50: In the bottom center. Z0=50.
    '        ' Q: Outside of main circle. Invalid.
    '        ' R: NormR<=0. Invalid.

    '        Dim Z0 As System.Double = mainCirc.Z0
    '        Dim CurrentR As System.Double = Me.Resistance
    '        Dim Y0 As System.Double = 1.0 / Z0
    '        Dim CurrentG As System.Double = Me.ToAdmittance().Conductance * Z0

    '        ' LEAVE THIS HERE FOR NOW.
    '        ' OPEN OR SHORT SHOULD HAVE BEEN REJECTED IN NEW() AND THIS SHOULD NOT
    '        ' BE NEEDED UNLESS SOME REASON IS DISCOVERED THAT REQUIRES EXTREMES TO
    '        ' BE ALLOWED. THAT MIGHT HAPPEN IF AN IMAGE IMPEDANCE HAS EXTREME VALUES
    '        ' THAT CANCEL OR FOR SOME OTHER INTERIM STATE. MAYBE IF A MATCH IS BEING
    '        ' MADE TO AN IMAGE IMPEDANCE OR A SITUATION INVOLVING ACTIVE COMPONENTS
    '        ' THAT CAN EFFECTIVELY HAVE A NEGATIVE RESITANCE VALUE.
    '        ' Check for a short- or open-circuit.
    '        If Impedance.EqualEnoughZero(CurrentR, Z0 * IMPDTOLERANCE0) OrElse
    '            System.Double.IsInfinity(CurrentR) Then
    '            ' A: At the short circuit point.
    '            ' B: Anywhere else on the perimeter. R=0.0.
    '            ' C: At the open circuit point on the right.

    '            transformations = Nothing
    '            Return False
    '        End If

    '        If Impedance.EqualEnough(CurrentR, Z0, Z0 * IMPDTOLERANCE) AndAlso
    '            Impedance.EqualEnoughZero(Me.Reactance, Z0 * IMPDTOLERANCE0) Then
    '            ' D: At the center.
    '            ' Leave transformations as the incoming empty array.

    '            Return True
    '        End If

    '        If CurrentR >= Z0 Then
    '            ' On the R=Z0 circle.
    '            '     Omit: On the resonance line. Already covered by C or D.
    '            '     E: On R=Z0 circle, above resonance line.
    '            '     F: On R=Z0 circle, below resonance line.
    '            ' Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
    '            '     G1: Inside R=Z0 circle, above resonance line.
    '            '     G50: Inside R=Z0 circle, above resonance line. Z0=50.
    '            '     H1: Inside R=Z0 circle, on line.
    '            '     I1: Inside R=Z0 circle, below resonance line.
    '            If Impedance.EqualEnough(CurrentR, Z0, Z0 * IMPDTOLERANCE) Then
    '                Return Me.OnREqualsZ0(Z0, transformations) ' E, F.
    '            Else
    '                Return Me.InsideREqualsZ0(Z0, transformations) 'G, H, I.
    '            End If
    '        ElseIf CurrentG >= 1.0 Then
    '            ' On the G=Y0 circle.
    '            '     Omit: On the resonance line. Already covered by A or D.
    '            '     J: On G=Y0 circle, above resonance line.
    '            '     K: On G=Y0 circle, below resonance line.
    '            ' Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
    '            '     L1: Inside G=Y0 circle, above resonance line.
    '            '     L75: Inside G=Y0 circle, above resonance line. Z0=75.
    '            '     M1: Inside G=Y0 circle, on line.
    '            '     N1: Inside G=Y0 circle, below line.
    '            If Impedance.EqualEnough(Me.ToAdmittance().Conductance, Y0,
    '                                     Y0 * IMPDTOLERANCE) Then
    '                Return Me.OnGEqualsY0(Z0, transformations) ' J, K.
    '            Else
    '                Return Me.InsideGEqualsY0(mainCirc, transformations) ' L, M, N.
    '            End If
    '        End If

    '        ' DELETE THIS AFTER TESTING CONFIRMS THAT IT IS NEVER HIT BY ANY TEST CASES.
    '        ' On getting this far, the impedance will, usually, fall into either
    '        ' the top or bottom center section.
    '        Dim NormX As System.Double = Me.Reactance / Z0
    '        If Impedance.EqualEnoughZero(NormX, Z0 * IMPDTOLERANCE0) Then
    '            ' Z is ON the resonance line.

    '            ' Should this case have been caught above? Yes, it would be in or
    '            ' on the R- or G-circle, or at the center.
    '            Dim CaughtBy As System.Reflection.MethodBase =
    '                    System.Reflection.MethodBase.GetCurrentMethod
    '            Throw New ApplicationException(
    '                    """EqualEnoughZero(NormX, TOLERANCE)"" should never be" &
    '                    " matched in " & NameOf(TrySelectMatchLayout))
    '        End If

    '        Return Me.InRemainder(Z0, transformations)

    '        ' GETTING HERE MEANS THAT NO CASES MATCHED.
    '        Return False ' DEFAULT UNTIL IMPLEMENTED.

    '    End Function ' TrySelectMatchLayout

    '#Region "MatchArbitrary1"

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the specified load
    '    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    '    ''' the specified <c>SmithMainCircle</c>.
    '    ''' This method attempts to find a match by first moving, on a G-circle,
    '    ''' from the load impedance to an image impedance at a specified Cartesian
    '    ''' coordinate, then moving, on an R-circle, from the image impedance to the
    '    ''' source impedance.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies a <c>SmithMainCircle</c> in which the
    '    ''' match is to be made.</param>
    '    ''' <param name="oneIntersection">Specifies the Cartesian coordinate of the
    '    ''' image impedance.</param>
    '    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    '    ''' <paramref name="sourceZ"/>.</param>
    '    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    '    ''' <paramref name="loadZ"/> should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load
    '    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    '    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    Public Shared Function MatchArbFirstOnG1(ByVal mainCirc As SmithMainCircle,
    '        ByVal oneIntersection As OSNW.Numerics.PointD,
    '        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' Find out about the intersection/image impedance.
    '        Dim ImagePD As PlotDetails =
    '            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)

    '        Dim CurrTransCount As System.Int32 = transformations.Length
    '        Dim Trans As New Transformation

    '        ' The intended process is to create an L-section. The first move is on
    '        ' the LoadG-circle, from the load impedance to the image impedance
    '        ' and the second move is on the SourceR-circle, from the image
    '        ' impedance to the source impedance.

    '        ' If the load susceptance already matches the image susceptance, no
    '        ' transformation is needed to get to the image impedance.
    '        Dim ImageB As System.Double = ImagePD.Susceptance
    '        Dim LoadB As System.Double = loadZ.ToAdmittance.Susceptance
    '        Dim DeltaX As System.Double
    '        If EqualEnoughZero(ImageB - LoadB, IMPDTOLERANCE0 * mainCirc.Z0) Then

    '            ' Move only on the SourceR-circle.
    '            DeltaX = sourceZ.Reactance - loadZ.Reactance
    '            With Trans
    '                If DeltaX < 0.0 Then
    '                    ' CCW on an R-circle needs a series capacitor.
    '                    .Style = TransformationStyles.SeriesCap
    '                Else
    '                    ' CW on an R-circle needs a series inductor.
    '                    .Style = TransformationStyles.SeriesInd
    '                End If
    '                .Value1 = DeltaX
    '            End With

    '            ' Add to the array of transformations.
    '            ReDim Preserve transformations(CurrTransCount)
    '            transformations(CurrTransCount) = Trans
    '            'xxxxxxxxxxxxxx
    '            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
    '                $" Value1={Trans.Value1}, Value2={Trans.Value2}")
    '            'xxxxxxxxxxxxxx
    '            Return True

    '        End If

    '        ' On getting this far,
    '        ' Move on the LoadG-circle first, to the image point, then on the
    '        ' SourceR-circle to the source.
    '        Dim DeltaB As System.Double = ImagePD.Susceptance -
    '            loadZ.ToAdmittance().Susceptance
    '        DeltaX = sourceZ.Reactance - ImagePD.Reactance
    '        With Trans
    '            If DeltaB < 0.0 Then
    '                ' CCW on a G-circle needs a shunt inductor.
    '                If DeltaX < 0.0 Then
    '                    ' CCW on a R-circle needs a series capacitor.
    '                    .Style = TransformationStyles.ShuntIndSeriesCap
    '                ElseIf DeltaX > 0.0 Then
    '                    ' CW on a R-circle needs a series inductor.
    '                    .Style = TransformationStyles.ShuntIndSeriesInd
    '                Else ' DeltaX = 0.0
    '                    .Style = TransformationStyles.ShuntInd
    '                End If
    '            Else ' DeltaB > 0.0
    '                ' CW on a G-circle needs a shunt capacitor.
    '                If DeltaX < 0.0 Then
    '                    ' CCW on a R-circle needs a series capacitor.
    '                    .Style = TransformationStyles.ShuntCapSeriesCap
    '                ElseIf DeltaX > 0.0 Then
    '                    ' CW on a R-circle needs a series inductor.
    '                    .Style = TransformationStyles.ShuntCapSeriesInd
    '                Else ' DeltaX = 0.0
    '                    .Style = TransformationStyles.ShuntCap
    '                End If
    '            End If
    '            .Value1 = New Admittance(0, DeltaB).ToImpedance.Reactance
    '            .Value2 = DeltaX
    '        End With

    '        ' Add to the array of transformations.
    '        ReDim Preserve transformations(CurrTransCount)
    '        transformations(CurrTransCount) = Trans
    '        'xxxxxxxxxxxxxx
    '        System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
    '            $" Value1={Trans.Value1}, Value2={Trans.Value2}")
    '        'xxxxxxxxxxxxxx
    '        Return True

    '    End Function ' MatchArbFirstOnG1

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the specified load
    '    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    '    ''' the specified <c>SmithMainCircle</c>.
    '    ''' This method attempts to find a match by first moving, on an R-circle,
    '    ''' from the load impedance to an image impedance at a specified Cartesian
    '    ''' coordinate, then moving, on a G-circle, from the image impedance to the
    '    ''' source impedance.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies a <c>SmithMainCircle</c> in which the
    '    ''' match is to be made.</param>
    '    ''' <param name="oneIntersection">Specifies the Cartesian coordinate of the
    '    ''' image impedance.</param>
    '    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    '    ''' <paramref name="sourceZ"/>.</param>
    '    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    '    ''' <paramref name="loadZ"/> should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load
    '    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    '    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    Public Shared Function MatchArbFirstOnR1(ByVal mainCirc As SmithMainCircle,
    '        ByVal oneIntersection As OSNW.Numerics.PointD,
    '        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' Find out about the intersection/image impedance.
    '        Dim ImagePD As PlotDetails =
    '            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)

    '        Dim CurrTransCount As System.Int32 = transformations.Length
    '        Dim Trans As New Transformation

    '        ' The intended process is to create an L-section. The first move is on
    '        ' the LoadR-circle, from the load impedance to the image impedance and
    '        ' the second move is on the SourceG-circle, from the image impedance to
    '        ' the source impedance.

    '        ' If the load reactance already matches the image reactance, no
    '        ' transformation is needed to get to the image impedance.
    '        Dim ImageX As System.Double = ImagePD.Reactance
    '        Dim LoadX As System.Double = loadZ.Reactance
    '        Dim DeltaB As System.Double
    '        Dim DeltaX As System.Double
    '        If EqualEnoughZero(ImageX - LoadX, IMPDTOLERANCE0 * mainCirc.Z0) Then

    '            ' Move only on the SourceG-circle.
    '            DeltaB = sourceZ.ToAdmittance.Susceptance -
    '                loadZ.ToAdmittance.Susceptance
    '            With Trans
    '                If DeltaB < 0.0 Then
    '                    ' CCW on a G-circle needs a shunt inductor.
    '                    .Style = TransformationStyles.ShuntInd
    '                Else
    '                    ' CW on a G-circle needs a shunt capacitor.
    '                    .Style = TransformationStyles.ShuntCap
    '                End If
    '                DeltaX = New Admittance(0.0, DeltaB).ToImpedance.Reactance
    '                .Value1 = DeltaX
    '            End With

    '            ' Add to the array of transformations.
    '            ReDim Preserve transformations(CurrTransCount)
    '            transformations(CurrTransCount) = Trans
    '            'xxxxxxxxxxxxxx
    '            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
    '                    $" Value1={Trans.Value1}, Value2={Trans.Value2}")
    '            'xxxxxxxxxxxxxx
    '            Return True

    '        End If

    '        ' On getting this far,
    '        ' Move on the LoadR-circle first, to the image point, then on the
    '        ' SourceG-circle to the source.
    '        DeltaX = ImagePD.Reactance - loadZ.Reactance
    '        DeltaB = sourceZ.ToAdmittance().Susceptance - ImagePD.Susceptance
    '        With Trans
    '            If DeltaX < 0.0 Then
    '                ' CCW on a R-circle needs a series capacitor.
    '                If DeltaB < 0.0 Then
    '                    ' CCW on a G-circle needs a shunt inductor.
    '                    .Style = TransformationStyles.SeriesCapShuntInd
    '                ElseIf DeltaB > 0.0 Then
    '                    ' CW on a G-circle needs a shunt capacitor.
    '                    .Style = TransformationStyles.SeriesCapShuntCap
    '                Else ' DeltaB = 0.0
    '                    .Style = TransformationStyles.SeriesCap
    '                End If
    '            Else ' DeltaX > 0.0
    '                ' CW on a R-circle needs a series inductor.
    '                If DeltaB < 0.0 Then
    '                    ' CCW on a G-circle needs a shunt inductor.
    '                    .Style = TransformationStyles.SeriesIndShuntInd
    '                ElseIf DeltaB > 0.0 Then
    '                    ' CW on a G-circle needs a shunt capacitor.
    '                    .Style = TransformationStyles.SeriesIndShuntCap
    '                Else ' DeltaB = 0.0
    '                    .Style = TransformationStyles.SeriesInd
    '                End If
    '            End If
    '            .Value1 = DeltaX
    '            .Value2 = New Admittance(0, DeltaB).ToImpedance().Reactance
    '        End With

    '        ' Add to the array of transformations.
    '        ReDim Preserve transformations(CurrTransCount)
    '        transformations(CurrTransCount) = Trans
    '        'xxxxxxxxxxxxxx
    '        System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
    '            $" Value1={Trans.Value1}, Value2={Trans.Value2}")
    '        'xxxxxxxxxxxxxx
    '        Return True

    '    End Function ' MatchArbFirstOnR1

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the specified load
    '    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    '    ''' the specified <c>SmithMainCircle</c>.
    '    ''' </summary>
    '    ''' <param name="mainCirc">Specifies a <c>SmithMainCircle</c> in which the
    '    ''' match is to be made.</param>
    '    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    '    ''' <paramref name="sourceZ"/>.</param>
    '    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    '    ''' <paramref name="loadZ"/> should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load
    '    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    '    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' A succcessful process might result in no transformation being done.
    '    ''' </remarks>
    '    Public Shared Function MatchArbitrary1(
    '        ByVal mainCirc As SmithMainCircle,
    '        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        ' REF: Smith Chart Full Presentation, page 26 shows a geometric
    '        ' approach to finding a match.
    '        ' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

    '        'xxxxxxxxxxxxxx
    '        System.Diagnostics.Debug.WriteLine($"Z0:{mainCirc.Z0}; {loadZ} to {sourceZ}:")
    '        'xxxxxxxxxxxxxx

    '        ' Input checking.
    '        ' Leave this here, at least for now. Bad values should have been
    '        ' rejected in New(Double, Double), and this should not be needed unless
    '        ' some reason is discovered that requires extremes to be allowed. Maybe
    '        ' that could happen in a situation involving active components that can
    '        ' effectively have a negative resitance value.
    '        ' Check for a short- or open-circuit or for invalid resistances.
    '        Dim LoadR As System.Double = loadZ.Resistance
    '        Dim SourceR As System.Double = sourceZ.Resistance
    '        If LoadR <= 0.0 OrElse System.Double.IsInfinity(LoadR) OrElse
    '            SourceR <= 0.0 OrElse System.Double.IsInfinity(SourceR) Then

    '            ' Leave transformations as is.
    '            'xxxxxxxxxxxxxx
    '            System.Diagnostics.Debug.WriteLine($"  Invalid input.")
    '            'xxxxxxxxxxxxxx
    '            Return False
    '        End If

    '        ' Check whether a match is needed.
    '        If Impedance.EqualEnough(mainCirc.Z0, loadZ, sourceZ) Then
    '            ' Not needed. Add the inaction to the array of transformations.
    '            Dim Trans As New Transformation With
    '                {.Style = TransformationStyles.None}
    '            Dim CurrTransCount As System.Int32 = transformations.Length
    '            ReDim Preserve transformations(CurrTransCount)
    '            transformations(CurrTransCount) = Trans
    '            'xxxxxxxxxxxxxx
    '            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
    '                $" Value1={Trans.Value1}, Value2={Trans.Value2}")
    '            'xxxxxxxxxxxxxx
    '            Return True
    '        End If

    '        ' Try each geometric approach to finding a match.
    '        '        Dim Intersections _
    '        '            As New System.Collections.Generic.List(Of OSNW.Numerics.PointD)
    '        Dim Intersections _
    '            As System.Collections.Generic.List(Of OSNW.Numerics.PointD)

    '        ' Try first on a G-circle, then on an R-circle.
    '        Dim LoadCircG As New GCircle(mainCirc, loadZ.ToAdmittance().Conductance)
    '        Dim SourceCircR As New RCircle(mainCirc, SourceR)
    '        Intersections = LoadCircG.GetIntersections(SourceCircR)

    '        ' There are now either one or two intersection points. With one, the
    '        ' circles are tangent at a point on the resonance line. With two, there
    '        ' is one above, and one below, the resonance line; the X values match;
    '        ' the Y values are the same distance above and below the resonance line.

    '        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

    '            If Not MatchArbFirstOnG1(mainCirc, OneIntersection, loadZ, sourceZ,
    '                                     transformations) Then

    '                Return False
    '            End If

    '            ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
    '            For Each OneTrans As Transformation In transformations
    '                If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
    '                    Return False
    '                End If
    '            Next

    '        Next

    '        ' Try first on an R-circle, then on a G-circle.
    '        Dim LoadCircR As New RCircle(mainCirc, LoadR)
    '        Dim SourceCircG As New GCircle(
    '            mainCirc, sourceZ.ToAdmittance().Conductance)
    '        Intersections = LoadCircR.GetIntersections(SourceCircG)

    '        ' There are now either one or two intersection points. With one, the
    '        ' circles are tangent at a point on the resonance line. With two, there
    '        ' is one above, and one below, the resonance line; the X values match;
    '        ' the Y values are the same distance above and below the resonance line.

    '        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

    '            If Not MatchArbFirstOnR1(mainCirc, OneIntersection, loadZ, sourceZ,
    '                                     transformations) Then

    '                Return False
    '            End If

    '            ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
    '            For Each OneTrans As Transformation In transformations
    '                If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
    '                    Return False
    '                End If
    '            Next

    '        Next

    '        ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
    '        For Each OneTrans As Transformation In transformations
    '            If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
    '                Return False
    '            End If
    '        Next

    '        ' On getting this far,
    '        Return True

    '    End Function ' MatchArbitrary1

    '    ''' <summary>
    '    ''' Attempts to obtain a conjugate match from the specified load
    '    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c> in a
    '    ''' system having the specified characteristic impedance.
    '    ''' </summary>
    '    ''' <param name="z0">Specifies the characteristic impedance of the
    '    ''' system.</param>
    '    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    '    ''' <paramref name="sourceZ"/>.</param>
    '    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    '    ''' <paramref name="loadZ"/> should be matched.</param>
    '    ''' <param name="transformations">Specifies an array of
    '    ''' <see cref="Transformation"/>s that can be used to match a load
    '    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    '    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    '    ''' <c>False</c>. Also returns, by reference in
    '    ''' <paramref name="transformations"/>, the components to construct the
    '    ''' match.</returns>
    '    ''' <remarks>
    '    ''' <para> An assumption is made that the calling code has determined that
    '    ''' the <c>Impedance</c>s lie in valid positions. Failure to meet that
    '    ''' assumption could result in invalid, or incomplete, results.</para>
    '    ''' <paramref name="z0"/> is the characteristic impedance for the system in
    '    ''' which the <c>Impedance</c>s should be matched. It should have a
    '    ''' practical value with regard to the impedance values involved. A
    '    ''' succcessful process might result in an empty
    '    ''' <paramref name="transformations"/>.
    '    ''' </remarks>
    '    Public Shared Function MatchArbitrary1(z0 As System.Double,
    '        loadZ As Impedance, sourceZ As Impedance,
    '        ByRef transformations As Transformation()) _
    '        As System.Boolean

    '        'xxxxxxxxxxxxxx
    '        System.Diagnostics.Debug.WriteLine(String.Empty)
    '        'xxxxxxxxxxxxxx

    '        ' Input checking.
    '        If z0 <= 0.0 Then
    '            'xxxxxxxxxxxxxx
    '            System.Diagnostics.Debug.WriteLine($"Z0:{z0}; {loadZ} to {sourceZ}:")
    '            System.Diagnostics.Debug.WriteLine("Z0 is negative")
    '            'xxxxxxxxxxxxxx
    '            Return False
    '        End If

    '        ' Create a SmithMainCircle for the specified Z0 and pass it to the
    '        ' geometric worker.
    '        ' Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
    '        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
    '        Return MatchArbitrary1(MainCirc, loadZ, sourceZ, transformations)

    '    End Function ' MatchArbitrary1

    '#End Region '  "MatchArbitrary1"

End Structure ' Impedance

'Namespace TrySelectMatchLayoutTests

'    Public Class TestTrySelectMatchLayoutB
'        ' A: At the short circuit point. Omit; Covered by B.
'        ' B: Anywhere else on the perimeter. R=0.0.

'        '<InlineData(    Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 0.0000, 0.0000)> ' A: At the short circuit point.
'        <InlineData(1.0, 0.0000, 1 / 2.0)> ' B: Anywhere else on the perimeter. R=0.0.
'        Public Sub TrySelectMatch_PositionBZeroR_Fails(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim Z As New OSNW.Numerics.Impedance(r, x)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)

'            ' This version, with R=0, does not throw an exception when R=0 is
'            ' allowed by Impedance.New(), but it does fail to match.
'            Assert.False(Z.TrySelectMatchLayout(MainCirc, transformations))
'            Assert.True(transformations Is Nothing)

'        End Sub

'    End Class ' TestTrySelectMatchLayoutB

'    Public Class TestTrySelectMatchLayoutC
'        ' C: At the open circuit point on the right.

'        Const INF As Double = Double.PositiveInfinity

'        '<InlineData(    Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, INF, 0.0000)> ' C: At the open circuit point on the right.
'        Public Sub TrySelectMatch_PositionC_Fails(z0 As Double, r As Double, x As Double)
'            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
'                Sub()
'                    ' Code that throws the exception.
'                    Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'                    'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'                    Dim Z As New OSNW.Numerics.Impedance(r, x)
'                    Dim transformations As Transformation() = Array.Empty(Of Transformation)
'                    Assert.False(Z.TrySelectMatchLayout(MainCirc, transformations))
'                End Sub)
'        End Sub

'    End Class ' TestTrySelectMatchLayoutC

'    Public Class TestTrySelectMatchLayoutD
'        ' D: At the center.

'        '<InlineData(    Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 1.0, 0.0000)> ' D1: At the center.
'        <InlineData(75.0, 75.0, 0.0000)> ' D75: At the center.
'        Public Sub TestTrySelectMatchLayoutD(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim Z As New OSNW.Numerics.Impedance(r, x)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)

'            Assert.True(Z.TrySelectMatchLayout(MainCirc, transformations), Messages.TF)
'            Assert.True(transformations.Length = 0, Messages.ITC)

'        End Sub

'    End Class ' TestTrySelectMatchLayoutD

'    Public Class TestTrySelectMatchLayoutEF
'        ' On the R=Z0 circle.
'        ' On R=Z0 circle, on the resonance line. Already covered by C or D.
'        ' E: On R=Z0 circle, above resonance line.
'        ' F: On R=Z0 circle, below resonance line.

'        '<InlineData(    Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 1.0, 1.0)> ' E1: On R=Z0 circle, above resonance line.
'        <InlineData(50.0, 50.0, 50.0)> ' E50: On R=Z0 circle, above resonance line.
'        Public Sub TrySelectMatch_PositionE_Succeeds(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

'            Dim TargetZ As New Impedance(z0, 0.0)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
'            Dim CombinedZ As Impedance = Impedance.AddSeriesImpedance(TestZ, AddZ)

'            Assert.True(transformations.Length = 1, Messages.ITC)
'            Assert.True(transformations(0).Style.Equals(TransformationStyles.SeriesCap), Messages.ITS)
'            Assert.Equal(-x, transformations(0).Value1)
'            Assert.Equal(TargetZ, CombinedZ)

'        End Sub

'        '<InlineData(    Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 1.0, -2.0)> ' F1: On R=Z0 circle, below resonance line.
'        <InlineData(50.0, 50.0, -100.0)> ' F50: On R=Z0 circle, below resonance line.
'        Public Sub TrySelectMatch_PositionF_Succeeds(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

'            Dim TargetZ As New Impedance(z0, 0.0)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
'            Dim CombinedZ As Impedance = Impedance.AddSeriesImpedance(TestZ, AddZ)

'            Assert.True(transformations.Length = 1, Messages.ITC)
'            Assert.True(transformations(0).Style.Equals(TransformationStyles.SeriesInd), Messages.ITS)
'            Assert.Equal(-x, transformations(0).Value1)
'            Assert.Equal(TargetZ, CombinedZ)

'        End Sub

'    End Class ' TestTrySelectMatchLayoutEF

'    Public Class TestTrySelectMatchLayoutJK
'        ' On the G=Y0 circle.
'        ' On G=Y0 circle, on resonance line. Omit - already either A or D.
'        ' J1: On G=Y0 circle, above resonance line.
'        ' K1: On G=Y0 circle, below resonance line.

'        '<InlineData(    Z0,      G,       B)> ' Model
'        <Theory>
'        <InlineData(1.0, 1.0, -1.0)> ' J1: On G=Y0 circle, above resonance line.
'        <InlineData(50.0, 0.02, -0.02)> ' J50: On G=Y0 circle, above resonance line.
'        Public Sub TrySelectMatch_PositionJ_Succeeds(z0 As Double, g As Double, b As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As OSNW.Numerics.Impedance =
'                New OSNW.Numerics.Admittance(g, b).ToImpedance
'            Dim TargetZ As New Impedance(z0, 0.0)

'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
'            Dim CombinedZ As Impedance = Impedance.AddShuntImpedance(TestZ, AddZ)

'            Assert.True(transformations.Length = 1, Messages.ITC)
'            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntCap), Messages.ITS)
'            Assert.Equal(-z0, transformations(0).Value1)
'            Assert.Equal(TargetZ, CombinedZ)

'        End Sub

'        '<InlineData(    Z0,      G,       B)> ' Model
'        <Theory>
'        <InlineData(1.0, 1.0, 1.0)> ' K1: On G=Y0 circle, below resonance line.
'        <InlineData(50.0, 0.02, 0.02)> ' K50: On G=Y0 circle, below resonance line.
'        Public Sub TrySelectMatch_PositionK_Succeeds(z0 As Double, g As Double, b As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As OSNW.Numerics.Impedance =
'                New OSNW.Numerics.Admittance(g, b).ToImpedance
'            Dim TargetZ As New Impedance(z0, 0.0)

'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
'            Dim CombinedZ As Impedance = Impedance.AddShuntImpedance(TestZ, AddZ)

'            Assert.True(transformations.Length = 1, Messages.ITC)
'            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntInd), Messages.ITS)
'            Assert.Equal(z0, transformations(0).Value1)
'            Assert.Equal(TargetZ, CombinedZ)

'        End Sub

'    End Class ' TestTrySelectMatchLayoutJK

'    Public Class TestTrySelectMatchLayoutGHI
'        ' GHI: Inside the R=Z0 circle.

'        '<InlineData(     Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 2.0, 1 / 2.0)> ' G1: Inside R=Z0 circle, above resonance line.
'        <InlineData(50.0, 100.0, 25.0)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
'        <InlineData(1.0, 3.0, 0.0000)> ' H1: Inside R=Z0 circle, on line.
'        <InlineData(50.0, 150.0, 0.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
'        <InlineData(1.0, 2.0, -2.0)> ' I1: Inside R=Z0 circle, below resonance line.
'        <InlineData(50.0, 100.0, -100.0)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
'        Public Sub TrySelectMatch_PositionGHI_Succeeds(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

'            Dim TargetZ As New Impedance(z0, 0.0)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Assert.True(True)

'        End Sub

'        '<Theory>
'        'Public Sub TrySelectMatch_PositionGHI_Fails()
'        '    '
'        '    '
'        '    '
'        '    '
'        '    '
'        'End Sub

'    End Class ' TestTrySelectMatchLayoutGHI

'    Public Class TestTrySelectMatchLayoutLMN
'        ' LMN: Inside the G=Y0 circle.

'        '<InlineData(     Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 1 / 3.0, 1 / 3.0)> ' L1: Inside G=Y0 circle, above resonance line.
'        <InlineData(75.0, 25.0, 25.0)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
'        <InlineData(1.0, 1 / 3.0, 0.0000)> ' M1: Inside G=Y0 circle, on line.
'        <InlineData(75.0, 25.0, 0.0000)> ' M75: Inside G=Y0 circle, on line.
'        <InlineData(1.0, 1 / 2.0, -1 / 3.0)> ' N1: Inside G=Y0 circle, below line.
'        <InlineData(75.0, 37.5, -25.0)> ' N75: Inside G=Y0 circle, below line.
'        Public Sub TrySelectMatch_PositionLMN_Succeeds(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

'            Dim TargetZ As New Impedance(z0, 0.0)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Assert.True(True)

'        End Sub

'        '<Theory>
'        'Public Sub TrySelectMatch_PositionLMN_Fails()
'        '    '
'        '    '
'        '    '
'        '    '
'        '    '
'        'End Sub

'    End Class ' TestTrySelectMatchLayoutLMN

'    Public Class TestTrySelectMatchO
'        ' O: In the top center.

'        '<InlineData(     Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 0.2, 1.4)> ' O1: In the top center.
'        <InlineData(50.0, 10.0, 70.0)> ' O50: In the top center. Z0=50.
'        Public Sub TestTrySelectMatchLayoutO(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

'            Dim TargetZ As New Impedance(z0, 0.0)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Assert.True(True)

'        End Sub

'    End Class ' TestTrySelectMatchO

'    Public Class TestTrySelectMatchP
'        ' P: In the bottom center.

'        '<InlineData(     Z0,        R,       X)> ' Model
'        <Theory>
'        <InlineData(1.0, 0.4, -0.8)> ' P: In the bottom center.
'        <InlineData(50.0, 20.0, -40.0)> ' P: In the bottom center.
'        Public Sub TestTrySelectMatchLayoutP(z0 As Double, r As Double, x As Double)

'            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
'            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
'            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

'            Dim TargetZ As New Impedance(z0, 0.0)
'            Dim transformations As Transformation() = Array.Empty(Of Transformation)
'            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
'                Assert.True(False, Messages.TF)
'            End If
'            Assert.True(True)

'        End Sub

'    End Class ' TestTrySelectMatchP

'End Namespace ' TrySelectMatchLayoutTests

'Namespace MatchArbitrary1Tests

'    Public Class TestMatchArbitrary1_UnlikelyZ0

'        Const INF As Double = Double.PositiveInfinity

'        '<InlineData(  Z0,        R,         X)> ' Model
'        <Theory>
'        <InlineData(-1.0, 1.0, 0.0000)> ' Negative Z0. Invalid.
'        <InlineData(1.0, INF, 0.0000)> ' C: At the open circuit point on the right.
'        <InlineData(1.0, -0.0345, 0.4138)> ' Q: Outside of main circle. Invalid.
'        <InlineData(1.0, -2.0, 999)> ' R: NormR<=0. Invalid.
'        Public Sub MatchArbitrary1Z0_BadInput_ThrowsException(z0 As System.Double, loadR As Double, loadX As Double)
'            Try
'                ' Code that throws the exception.
'                Dim LoadZ As New Impedance(loadR, loadX)
'                Dim SourceZ As New Impedance(z0, 0.0)
'                Dim Transformations As Transformation() = Array.Empty(Of Transformation)
'            Catch ex As Exception
'                Assert.True(True)
'                Exit Sub
'            End Try
'            Assert.True(False, "Did not fail.")
'        End Sub

'        '<InlineData(  Z0,        R,         X)> ' Model
'        <Theory>
'        <InlineData(1.0, 0.0000, 0.0000)> ' A: At the short circuit point.
'        <InlineData(1.0, 0.0000, 1 / 2.0)> ' B: Anywhere else on the perimeter. R=0.0.
'        Public Sub MatchArbitrary1Z0_BadInput_Fails(z0 As System.Double, loadR As Double, loadX As Double)

'            Dim LoadZ As New Impedance(loadR, loadX)
'            Dim SourceZ As New Impedance(z0, 0.0)
'            Dim Transformations As Transformation() = Array.Empty(Of Transformation)

'            Assert.False(Impedance.MatchArbitrary1(z0, LoadZ, SourceZ, Transformations))

'        End Sub

'    End Class ' TestMatchArbitrary1_UnlikelyZ0

'    Public Class TestMatchArbitrary1Z0

'        '<InlineData(  Z0,        R,         X)> ' Model
'        <Theory>
'        <InlineData(1.0, 1.0, 0.0000)> ' D1: At the center.
'        <InlineData(75.0, 75.0, 0.0000)> ' D75: At the center. Z0=75.
'        <InlineData(1.0, 1.0, 1.0)> ' E1: On R=Z0 circle, above resonance line.
'        <InlineData(50.0, 50.0, 50.0)> ' E50: On R=Z0 circle, above resonance line. Z0=50.
'        <InlineData(1.0, 1.0, -2.0)> ' F1: On R=Z0 circle, below resonance line.
'        <InlineData(50.0, 50.0, -100.0)> ' F50: On R=Z0 circle, below resonance line. Z0=50.
'        <InlineData(1.0, 2.0, 1 / 2.0)> ' G1: Inside R=Z0 circle, above resonance line.
'        <InlineData(50.0, 100.0, 25.0)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
'        <InlineData(1.0, 3.0, 0.0000)> ' H1: Inside R=Z0 circle, on line.
'        <InlineData(50.0, 150.0, 0.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
'        <InlineData(1.0, 2.0, -2.0)> ' I1: Inside R=Z0 circle, below resonance line.
'        <InlineData(50.0, 100.0, -100.0)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
'        <InlineData(1.0, 1 / 2.0, 1 / 2.0)> ' J1: On G=Y0 circle, above resonance line.
'        <InlineData(50.0, 25.0, 25.0)> ' J50: On G=Y0 circle, above resonance line. Z0=50.
'        <InlineData(1.0, 1 / 2.0, -1 / 2.0)> ' K1: On G=Y0 circle, below resonance line.
'        <InlineData(50.0, 25.0, -25.0)> ' K50: On G=Y0 circle, below resonance line. Z0=50.
'        <InlineData(1.0, 1 / 3.0, 1 / 3.0)> ' L1: Inside G=Y0 circle, above resonance line.
'        <InlineData(75.0, 25.0, 25.0)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
'        <InlineData(1.0, 1 / 3.0, 0.0000)> ' M1: Inside G=Y0 circle, on line.
'        <InlineData(75.0, 25.0, 0.0000)> ' M75: Inside G=Y0 circle, on line. Z0=75.
'        <InlineData(1.0, 1 / 2.0, -1 / 3.0)> ' N1: Inside G=Y0 circle, below line.
'        <InlineData(75.0, 37.5, -25.0)> ' N75: Inside G=Y0 circle, below line. Z0=75.
'        <InlineData(1.0, 0.2, 1.4)> ' O1: In the top center.
'        <InlineData(50.0, 10.0, 70.0)> ' O50: In the top center. Z0=50.
'        <InlineData(1.0, 0.4, -0.8)> ' P1: In the bottom center.
'        <InlineData(50.0, 20.0, -40.0)> ' P50: In the bottom center. Z0=50.
'        Public Sub MatchArbitrary1Z0_GoodInput_Succeeds(z0 As System.Double, loadR As Double, loadX As Double)

'            Dim LoadZ As New Impedance(loadR, loadX)
'            Dim SourceZ As New Impedance(z0, 0.0)
'            Dim Transformations As Transformation() = Array.Empty(Of Transformation)

'            Assert.True(Impedance.MatchArbitrary1(z0, LoadZ, SourceZ, Transformations))

'        End Sub

'    End Class ' TestMatchArbitrary1Z0

'    Public Class TestMatchArbitrary1Any

'        <Theory>
'        <InlineData(1, 1.0, 1.0, 0.5, 0.2)> ' AMRIS1.
'        <InlineData(1, 0.5, 0.2, 1.0, 1.0)> ' AMRIS1 reversed.
'        <InlineData(100.0, 100.0, 100.0, 50.0, 20.0)> ' AMRIS100.
'        <InlineData(100.0, 50.0, 20.0, 100.0, 100.0)> ' AMRIS100 reversed.
'        <InlineData(1, 1.0, 1.0, 2.0, -2.0)> ' E1 to I1.
'        <InlineData(1, 2.0, -2.0, 1.0, 1.0)> ' I1 to E1.
'        <InlineData(75, 50.0, 50.0, 100.0, -100.0)> ' E50 to I50 (75).
'        <InlineData(75, 100.0, -100.0, 50.0, 50.0)> ' I50 to E50 (75).
'        <InlineData(1, 1 / 3.0, 1 / 3.0, 1 / 3.0, 0.0000)> ' L1 to M1.
'        <InlineData(1, 1 / 3.0, 0.0000, 1 / 3.0, 1 / 3.0)> ' M1 to L1.
'        <InlineData(75, 25.0, 25.0, 25.0, 0.0000)> ' L75 to M75.
'        <InlineData(75, 25.0, 0.0000, 25.0, 25.0)> ' M75 to L75.
'        <InlineData(1.0, 0.2, 1.4, 0.4, -0.8)> ' O1 to P1.
'        <InlineData(1.0, 0.4, -0.8, 0.2, 1.4)> ' P1 to O1.
'        <InlineData(50.0, 10.0, 70.0, 20.0, -40.0)> ' O50 to P50.
'        <InlineData(50.0, 20.0, -40.0, 10.0, 70.0)> ' P50 to O50.
'        <InlineData(1.0, 1.0, 1.0, 1 / 2.0, 1 / 2.0)> ' E1 to J1.
'        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 1.0, 1.0)> ' J1 to E1.
'        <InlineData(50.0, 50.0, 50.0, 25.0, 25.0)> ' E50 to J50.
'        <InlineData(50.0, 25.0, 25.0, 50.0, 50.0)> ' J50 to E50.
'        Public Sub MatchArbitrary1_GoodInput_Succeeds(z0 As Double, loadR As Double, loadX As Double,
'                                                     sourceR As Double, sourceX As Double)

'            Dim LoadZ As New Impedance(loadR, loadX)
'            Dim SourceZ As New Impedance(sourceR, sourceX)
'            Dim Transformations As Transformation() = Array.Empty(Of Transformation)

'            Assert.True(Impedance.MatchArbitrary1(z0, LoadZ, SourceZ, Transformations))

'        End Sub

'    End Class ' TestMatchArbitrary1Any

'End Namespace ' MatchArbitrary1Tests
