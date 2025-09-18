Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

' The generic tuning process is intended to be able to select a method to obtain
' a conjugate match for a load impedance to a source characteristic impedance.
' It is not intended to select specific capacitance or inductance values. The
' goal is to be able to lay out an L-section and select a reactance value for
' each component. Those reactance values could then be used to select
' appropriate component values based on frequency.

' The comments here relate to solving conjugate matches on a Smith Chart that
' has a horizontal resonance line, with R=0 on the left.

' Smith-Chart - University of Utah
' https://my.ece.utah.edu/~ece5321/ZY_chart.pdf
'
' NORMALIZED IMPEDANCE AND ADMITTANCE COORDINATES
' https://mtt.org/app/uploads/2023/08/ZY_color_smith_chart.pdf

' Chapter 2-2 The Smith Chart - University of Florida
' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

' Impedance Matching and Smith Chart Impedance
' https://www.analog.com/en/resources/technical-articles/impedance-matching-and-smith-chart-impedance-maxim-integrated.html?gated=1751854195363

' Microsoft Word - The Smith Chart.doc
' https://ittc.ku.edu/~jstiles/723/handouts/The%20Smith%20Chart.pdf

' Smith Chart Table of Contents
' http://www.antenna-theory.com/tutorial/smith/chart.php

'              Component impact
' A series inductor moves CW on an R-circle.
' A shunt inductor moves CCW on a G-circle.
' A series capacitor moves CCW on an R-circle.
' A shunt capacitor moves CW on a G-circle.

'             Component selection
' To go | On a     | Use a
' CW    | R-circle | series inductor
' CW    | G-circle | shunt capacitor
' CCW   | R-circle | series capacitor
' CCW   | G-circle | shunt inductor

''' <summary>
''' The circuit layout to be used to match a load impedance to a source
''' characteristic impedance or to otherwise modify the impedance.
''' </summary>
''' <remarks>
''' Member names begin with the first component encountered by the load,
''' proceeding toward the source.
''' The default is <c>MatchingLayouts.None.</c>.
''' </remarks>>
Public Enum TransformationStyles

    ''' <summary>
    ''' The default value.
    ''' </summary>
    None

    ' Define single-element arrangements.

    ''' <summary>
    ''' Source &lt;-- ShuntCap &lt;-- Load
    ''' </summary>
    ShuntCap

    ''' <summary>
    ''' Source &lt;-- ShuntInd &lt;-- Load
    ''' </summary>
    ShuntInd

    ''' <summary>
    ''' Source &lt;-- SeriesCap &lt;-- Load
    ''' </summary>
    SeriesCap

    ''' <summary>
    ''' Source &lt;-- SeriesInd &lt;-- Load
    ''' </summary>
    SeriesInd

    ' The L sections below follow the sequence shown on page 21 of the link
    ' below, from left-to-right per row.
    ' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

    ''' <summary>
    ''' Source &lt;-- SeriesInd &lt;-- ShuntCap &lt;-- Load
    ''' </summary>
    ShuntCapSeriesInd

    ''' <summary>
    ''' Source &lt;-- SeriesCap &lt;-- ShuntCap &lt;-- Load
    ''' </summary>
    ShuntCapSeriesCap

    ''' <summary>
    ''' Source &lt;-- SeriesCap &lt;-- ShuntInd &lt;-- Load
    ''' </summary>
    ShuntIndSeriesCap

    ''' <summary>
    ''' Source &lt;-- SeriesInd &lt;-- ShuntInd &lt;-- Load
    ''' </summary>
    ShuntIndSeriesInd

    ''' <summary>
    ''' Source &lt;-- ShuntInd &lt;-- SeriesCap &lt;-- Load
    ''' </summary>
    SeriesCapShuntInd

    ''' <summary>
    ''' Source &lt;-- ShuntCap &lt;-- SeriesCap &lt;-- Load
    ''' </summary>
    SeriesCapShuntCap

    ''' <summary>
    ''' Source &lt;-- ShuntCap &lt;-- SeriesInd &lt;-- Load
    ''' </summary>
    SeriesIndShuntCap

    ''' <summary>
    ''' Source &lt;-- ShuntInd &lt;-- SeriesInd &lt;-- Load
    ''' </summary>
    SeriesIndShuntInd

    '
    '
    ' Futures could include the addition of the ability to insert:
    '   PI, T, M, band-pass, and notch filter sections.
    '   Shunt or series, parallel tank or series-resonant, sections to construct
    '     band-pass or notch filters.
    '   Feedline segments to cause impedance rotation or quarter-wave impedance
    '     transformers, perhaps to allow the use of 75-ohm hard line in a 50-ohm
    '     installation.
    '   Open or closed coax stubs the create band-pass or notch filters.
    '
    '

End Enum ' TransformationStyles

''' <summary>
''' Describes a single element that contributes to transformation of an Impedance.
''' </summary>
Public Structure Transformation

    ''' <summary>
    ''' 
    ''' </summary>
    Public Style As TransformationStyles

    ''' <summary>
    ''' The first component, next to the load.
    ''' </summary>
    Public Value1 As System.Double

    ''' <summary>
    ''' The next component toward the source, after Value1.
    ''' </summary>
    Public Value2 As System.Double

    ''' <summary>
    ''' The next component toward the source, after Value2.
    ''' </summary>
    Public Value3 As System.Double

End Structure ' Transformation

Partial Public Structure Impedance

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance specified by
    ''' <paramref name="z0"/>, when the current instance appears directly on the
    ''' R=Z0 circle.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <param name="transformations">xxxxxxxxxx</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <paramref name="z0"/> is the characteristic impedance to which the
    ''' current instance should be matched. It should have a practical value
    ''' with regard to the impedance values involved.
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function OnREqualsZ0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' Test data E: On R=Z0 circle, above resonance line. Only needs reactance.
        ' Test data F: On R=Z0 circle, below resonance line. Only needs reactance.

        'Dim NormR As System.Double = Me.Resistance / z0
        Dim NormX As System.Double = Me.Reactance / z0
        'Dim Y0 As System.Double = 1.0 / z0
        'Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0

        If NormX.Equals(0.0) Then
            ' This happens at two places. One would have been handled as
            ' position C. The other is at the center of the chart.

            ' Test data D: At the center.
            ' Z is at the center point and already has a conjugate match.
            transformations = {
                New Transformation With {
                .Style = TransformationStyles.None}
            }
            Return True
        Else
            ' Z is on the perimeter of the R=Z0 circle and only needs a
            ' reactance.

            If NormX > 0.0 Then
                ' Test data E: On R=Z0 circle, above resonance line. Only needs
                ' reactance.
                ' CCW on an R-circle needs a series capacitor.
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.SeriesCap,
                    .Value1 = -NormX}
                }
                Return True

                ' Consider alternative approaches.
                ' CW on an R-circle would need a series inductor, increasing
                ' the inductance of an already inductive load. NO.
                ' What about tuning the equivalent admittance?
                ' CCW on a G-circle would need a shunt inductor, reducing
                ' but not canceling the reactance. NO.
                ' CW on a G-circle would need a shunt capacitor. For Z=1+j3,
                ' Y=0.1-j0.3. Adding a shunt capacitor 0+j0.3 results in a
                ' total admittance Y=0.1+j0. For Y=0.1+j0, Z=10+j0. NO.

            Else
                ' Test data F: On R=Z0 circle, below resonance line. Only needs
                ' reactance.
                ' CW on an R-circle needs a series inductor.
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.SeriesInd,
                    .Value1 = -NormX}
                }
                Return True
            End If
        End If
    End Function ' OnREqualsZ0

    '''' <summary>
    '''' G: On the G=Y0 circle.
    '''' </summary>
    '''' <param name="z0">xxxxxxxxxx</param>
    '''' <param name="transformations">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    Private Function OnGEqualsY0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        'Dim NormR As System.Double = Me.Resistance / z0
        'Dim NormX As System.Double = Me.Reactance / z0
        Dim Y0 As System.Double = 1.0 / z0
        Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        Dim NormB As System.Double = Y.Susceptance / Y0

        If NormB.Equals(0.0) Then
            ' Z is already at the center, where Z=1+j0, and already has a
            ' conjugate match.
            ' THAT SHOULD HAVE ALREADY BEEN CAUGHT???
            transformations = {
                New Transformation With {
                .Style = TransformationStyles.None}
            }
            Return True
        Else
            ' Z is on the perimeter of the G=Y0 circle and only needs a
            ' reactance.

            If NormB > 0.0 Then
                ' K: On G=Y0 circle, below resonance line. Only needs reactance.
                ' CCW on a G-circle needs a shunt inductor.
                Dim V1 As System.Double = -NormB
                Dim EffectiveY As New Admittance(0, V1)
                Dim EffectiveZ As Impedance = EffectiveY.ToImpedance
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.ShuntInd,
                    .Value1 = EffectiveZ.Reactance}
                }
                Return True
            Else
                ' J: On G=Y0 circle, above resonance line. Only needs reactance.
                ' CW on a G-circle needs a shunt capacitor.
                Dim V1 As System.Double = -NormB
                Dim EffectiveY As New Admittance(0, V1)
                Dim EffectiveZ As Impedance = EffectiveY.ToImpedance
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.ShuntCap,
                    .Value1 = EffectiveZ.Reactance}
                }
                Return True
            End If
        End If

    End Function ' OnGEqualsY0

    ''' <summary>
    ''' xxxxxxxxxxxxxxxxxx
    ''' Worker for routines below.
    ''' </summary>
    ''' <param name="z0">xxxxxxxxxxxxxxxxxx</param>
    ''' <param name="aTransformation">xxxxxxxxxxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxxxxxxxxxx</returns>
    Private Function ValidateTransformation(ByVal z0 As System.Double,
        ByVal aTransformation As Transformation) As System.Boolean

        Dim TargetZ As New Impedance(z0, 0.0)
        Dim WorkZ As Impedance
        Dim FixupY As Admittance
        Dim FixupZ As Impedance

        If aTransformation.Style = TransformationStyles.ShuntCapSeriesInd Then

            FixupY = New Admittance(0.0, aTransformation.Value1)
            FixupZ = FixupY.ToImpedance
            WorkZ = Impedance.AddShuntImpedance(Me, FixupZ)

            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, FixupZ)

            Dim NearlyZero As System.Double = z0 * 0.000001
            If Not Impedance.EqualEnough(WorkZ.Resistance, z0) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException(
                    "Resistance did not reach target.")
            End If
            If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException(
                    "Reactance did not reach target.")
            End If

        ElseIf aTransformation.Style = TransformationStyles.ShuntIndSeriesCap Then

            FixupY = New Admittance(0.0, aTransformation.Value1)
            FixupZ = FixupY.ToImpedance
            WorkZ = Impedance.AddShuntImpedance(Me, FixupZ)

            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, FixupZ)

            Dim NearlyZero As System.Double = z0 * 0.000001
            If Not Impedance.EqualEnough(WorkZ.Resistance, z0) OrElse
                Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException("Transformation did not reach target.")
            End If

        ElseIf aTransformation.Style = TransformationStyles.SeriesIndShuntCap Then

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, FixupZ)

            FixupY = New Admittance(0.0, aTransformation.Value2)
            FixupZ = FixupY.ToImpedance
            WorkZ = Impedance.AddShuntImpedance(WorkZ, FixupZ)

            Dim NearlyZero As System.Double = z0 * 0.000001
            If Not Impedance.EqualEnough(WorkZ.Resistance, z0) OrElse
                Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException("Transformation did not reach target.")
            End If

        ElseIf aTransformation.Style = TransformationStyles.SeriesCapShuntInd Then

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, FixupZ)

            FixupY = New Admittance(0.0, aTransformation.Value2)
            FixupZ = FixupY.ToImpedance
            WorkZ = Impedance.AddShuntImpedance(WorkZ, FixupZ)

            Dim NearlyZero As System.Double = z0 * 0.000001
            If Not Impedance.EqualEnough(WorkZ.Resistance, z0) OrElse
                Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException("Transformation did not reach target.")
            End If

        Else
            ' Invalid transformation style.
            Return False
        End If

        ' On getting this far,
        Return True

    End Function ' ValidateTransformation

    ''' <summary>
    '''  Processes one intersection found in
    '''  <see cref="M:InsideREqualsZ0(z0, transformations)"/>".>
    ''' </summary>
    ''' <param name="mainCirc">Specifies an arbitrary
    ''' <see cref="SmithMainCircle"/> reference for calculations.</param>
    ''' <param name="intersection">Specifies the Cartesian coordinates of one
    ''' intersection of R- and G-circles.</param>
    ''' <param name="transformation">Specifies the proposed
    ''' <see cref="Transformation"/> to be checked.</param>
    ''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    ''' results in a conjugate match for the current instance; otherwise,
    ''' <c>False</c>.</returns>
    Private Function InsideREqualsZ0(ByVal mainCirc As SmithMainCircle,
        ByVal intersection As System.Drawing.PointF,
        ByRef transformation As Transformation) As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance/admittance
        ' with actual R/X values.

        'Dim NormR As System.Double = Me.Resistance / z0
        'Dim NormX As System.Double = Me.Reactance / z0
        'Dim Y0 As System.Double = 1.0 / z0
        Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0

        Try

            ' First move, to the image impedance.
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(intersection.X, intersection.Y)
            Dim DiffImageB As System.Double =
                ImageY.Susceptance - Y.Susceptance

            ' Second move.
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(intersection.X, intersection.Y)
            Dim DiffFinalX As System.Double = -ImageZ.Reactance

            ' Select the transformations, based on the location of the
            ' intersection relative to the resonance line.
            If intersection.Y > mainCirc.GridCenterY Then
                ' Intersection above the resonance line.

                ' Use a shunt inductor to move CCW on the G-circle to the R=Z0
                ' circle, then use a series capacitor to move CCW on the R=Z0
                ' circle to the center.
                transformation = New Transformation With {
                    .Style = TransformationStyles.ShuntIndSeriesCap,
                    .Value1 = DiffImageB,
                    .Value2 = DiffFinalX
                }
            Else
                ' Intersection below the resonance line.

                '  Use a shunt capacitor to move CW on the G-circle to the R=Z0
                '  circle, then use a series inductor to move CW on the R=Z0
                '  circle to the center.
                transformation = New Transformation With {
                    .Style = TransformationStyles.ShuntCapSeriesInd,
                    .Value1 = DiffImageB,
                    .Value2 = DiffFinalX
                }
            End If

        Catch CaughtEx As Exception
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            'Throw New System.InvalidOperationException(
            '    $"Failed to process {CaughtBy}.")
            Return False
        End Try

        ' On getting this far,
        Return True

    End Function ' InsideREqualsZ0

    '''' <summary>
    '''' GHI: Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
    '''' </summary>
    '''' <param name="z0">xxxxxxxxxx</param>
    '''' <param name="transformations">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    Private Function InsideREqualsZ0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        'Dim NormR As System.Double = Me.Resistance / z0
        'Dim NormX As System.Double = Me.Reactance / z0
        'Dim Y0 As System.Double = 1.0 / z0
        Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0

        ' The first move will be to the intersection of the R=Z0 circle and the
        ' G-circle that contains the load impedance. From inside the R=Z0
        ' circle, there are two ways to proceed:
        '  - Use a shunt capacitor to move CW on the G-circle to the R=Z0
        '  circle, then use a series inductor to move CW on the R=Z0 circle to
        '  the center.
        '  - Use a shunt inductor to move CCW on the G-circle to the R=Z0
        '  circle, then use a series capacitor to move CCW on the R=Z0 circle to
        '  the center.
        ' Would there ever be a reason to prefer one approach over the other?
        '  - To favor high- or low-pass?
        '  - To favor the shortest first path?

        ' Determine the circles and their intersections.
        'Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
        Dim CircR As New RCircle(MainCirc, z0)
        Dim CircG As New GCircle(MainCirc, Y.Conductance)
        Dim Intersections _
            As System.Collections.Generic.List(Of System.Drawing.PointF) =
                GenericCircle.GetIntersections(CircR, CircG)

        '' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
        '' RESULTS ARE KNOWN TO BE CORRECT.
        '' There should now be two intersection points, with one above, and one
        '' below, the resonance line.
        'If Intersections.Count <> 2 Then
        '    'Dim CaughtBy As System.Reflection.MethodBase =
        '    '    System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ApplicationException(Impedance.MSGIIC)
        'End If
        '' The X values should match. Check for reasonable equality when using
        '' floating point values.
        'If Not EqualEnough(Intersections(0).X, Intersections(0).X) Then
        '    'Dim CaughtBy As System.Reflection.MethodBase =
        '    '    System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ApplicationException("X values do not match.")
        'End If
        '' The Y values should be the same distance above and below the
        '' resonance line. Check for reasonable equality when using floating
        '' point values.
        'Dim Offset0 As System.Double =
        '    System.Math.Abs(Intersections(0).Y - MainCirc.GridCenterY)
        'Dim Offset1 As System.Double =
        '    System.Math.Abs(Intersections(1).Y - MainCirc.GridCenterY)
        'If Not EqualEnough(Offset1, Offset0) Then
        '    'Dim CaughtBy As System.Reflection.MethodBase =
        '    '    System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ApplicationException("Y offsets do not match.")
        'End If

        ' There are now two intersection points, with one above and one below
        ' the resonance line. The X values match. The Y values are the same
        ' distance above and below the resonance line.

        ' Expect two valid solutions, one to each intersection.
        Dim Transformation0 As Transformation
        If Not Me.InsideREqualsZ0(
            MainCirc, Intersections(0), Transformation0) Then

            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("Transformation 0 failed.")
        End If
        Dim Transformation1 As Transformation
        If Not Me.InsideREqualsZ0(
            MainCirc, Intersections(1), Transformation1) Then

            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("Transformation 1 failed.")
        End If

        '' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
        '' RESULTS ARE KNOWN TO BE CORRECT.
        '' There should now be two valid solutions the tune to Z=Z0+j0.0.
        '' Check first solution.
        'If Not ValidateTransformation(z0, Transformation0) Then
        '    Return False
        'End If
        '' Check second solution.
        'If Not ValidateTransformation(z0, Transformation1) Then
        '    Return False
        'End If

        ' On getting this far,
        Return True

    End Function ' InsideREqualsZ0

    '''' <summary>
    ''''  Processes one intersection found in
    ''''  <see cref="M:InsideGEqualsY0(z0, transformations)"/>".>
    '''' </summary>
    '''' <param name="mainCirc">Specifies an arbitrary
    '''' <see cref="SmithMainCircle"/> reference for calculations.</param>
    '''' <param name="intersection">Specifies the Cartesian coordinates of one
    '''' intersection of R- and G-circles.</param>
    '''' <param name="transformation">Specifies the proposed
    '''' <see cref="Transformation"/> to be checked.</param>
    '''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    '''' results in a conjugate match for the current instance; otherwise,
    '''' <c>False</c>.</returns>
    Private Function InsideGEqualsY0(ByVal mainCirc As SmithMainCircle,
        ByVal intersection As System.Drawing.PointF,
        ByRef transformation As Transformation) As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance/admittance
        ' with actual R/X values.

        'Dim NormR As System.Double = Me.Resistance / z0
        'Dim NormX As System.Double = Me.Reactance / z0
        'Dim Y0 As System.Double = 1.0 / z0
        Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0

        Try

            ' First move, to the image impedance.
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(intersection.X, intersection.Y)
            Dim DiffImageX As System.Double =
                ImageZ.Reactance - Me.Reactance

            ' Second move.
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(intersection.X, intersection.Y)
            Dim DiffFinalY As System.Double = -ImageY.Susceptance

            ' Select the transformations, based on the location of the
            ' intersection relative to the resonance line.
            If intersection.Y > mainCirc.GridCenterY Then
                ' Intersection above the resonance line.

                ' Use a series inductor to move CW on the R-circle to the G=Y0
                ' circle, then use a shunt capacitor to move CW on the G=Y0
                ' circle to the center.
                transformation = New Transformation With {
                    .Style = TransformationStyles.SeriesIndShuntCap,
                    .Value1 = DiffImageX,
                    .Value2 = DiffFinalY
                }
            Else
                ' Intersection below the resonance line.

                '  Use a series capacitor to move CCW on the R-circle to the G=Y0
                '  circle, then use a shunt inductor to move CCW on the G=Y0
                '  circle to the center.
                transformation = New Transformation With {
                    .Style = TransformationStyles.SeriesCapShuntInd,
                    .Value1 = DiffImageX,
                    .Value2 = DiffFinalY
                }
            End If

        Catch CaughtEx As Exception
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            'Throw New System.InvalidOperationException(
            '    $"Failed to process {CaughtBy}.")
            Return False
        End Try

        ' On getting this far,
        Return True

    End Function ' InsideGEqualsY0

    '''' <summary>
    '''' LMN: Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
    '''' </summary>
    '''' <param name="z0">xxxxxxxxxx</param>
    '''' <param name="transformations">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    Private Function InsideGEqualsY0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        'Dim NormR As System.Double = Me.Resistance / z0
        'Dim NormX As System.Double = Me.Reactance / z0
        Dim Y0 As System.Double = 1.0 / z0
        'Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0

        ' The first move will be to the intersection of the G=Y0 circle and the
        ' R-circle that contains the load impedance. From inside the G=Y0
        ' circle, there are two ways to proceed:
        '  - Use a series inductor to move CW on the R-circle to the G=Y0
        '  circle, then use a shunt capacitor to move CW on the G=Y0 circle to
        '  the center.
        '  - Use a series capacitor to move CCW on the R-circle to the G=Y0
        '  circle, then use a shunt inductor to move CCW on the G=Y0 circle to
        '  the center.
        ' Would there ever be a reason to prefer one approach over the other?
        '  - To favor high- or low-pass?
        '  - To favor the shortest first path?

        ' Determine the circles and their intersections.
        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
        Dim CircG As New GCircle(MainCirc, Y0)
        Dim CircR As New RCircle(MainCirc, Me.Resistance)
        Dim Intersections _
            As System.Collections.Generic.List(Of System.Drawing.PointF) =
                GenericCircle.GetIntersections(CircR, CircG)

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
        ' RESULTS ARE KNOWN TO BE CORRECT.
        ' There should now be two intersection points, with one above, and one
        ' below, the resonance line.
        If Intersections.Count <> 2 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException(Impedance.MSGIIC)
        End If
        ' The X values should match. Check for reasonable equality when using
        ' floating point values.
        If Not EqualEnough(Intersections(0).X, Intersections(0).X) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("X values do not match.")
        End If
        ' The Y values should be the same distance above and below the
        ' resonance line. Check for reasonable equality when using floating
        ' point values.
        Dim Offset0 As System.Double =
            System.Math.Abs(Intersections(0).Y - MainCirc.GridCenterY)
        Dim Offset1 As System.Double =
            System.Math.Abs(Intersections(1).Y - MainCirc.GridCenterY)
        If Not EqualEnough(Offset1, Offset0) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("Y offsets do not match.")
        End If

        ' There are now two intersection points, with one above and one below
        ' the resonance line. The X values match. The Y values are the same
        ' distance above and below the resonance line.

        ' Expect two valid solutions, one to each intersection.
        Dim Transformation0 As Transformation
        If Not Me.InsideGEqualsY0(
            MainCirc, Intersections(0), Transformation0) Then

            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("Transformation 0 failed.")
        End If
        Dim Transformation1 As Transformation
        If Not Me.InsideGEqualsY0(
            MainCirc, Intersections(1), Transformation1) Then

            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("Transformation 1 failed.")
        End If

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
        ' RESULTS ARE KNOWN TO BE CORRECT.
        ' There should now be two valid solutions the tune to Z=Z0+j0.0.
        ' Check first solution.
        If Not ValidateTransformation(z0, Transformation0) Then
            Return False
        End If
        ' Check second solution.
        If Not ValidateTransformation(z0, Transformation1) Then
            Return False
        End If

        ' On getting this far,
        Return True

    End Function ' InsideGEqualsY0

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current load instance to
    ''' the source characteristic impedance specified by <paramref name="z0"/>.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <param name="transformations"></param>
    ''' <returns><c>True</c> if a conjugate match solution is found and also
    ''' returns the components to construct the match; otherwise, <c>False</c>.
    ''' </returns>
    Public Function TrySelectTuningLayout(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' The terminology here relates to solving conjugate matches on a Smith
        ' Chart.

        ' Chart location cases:
        ' A: At the short circuit point. Omit; Covered by B.
        ' B: Anywhere else on the perimeter. R=0.0.
        ' C: At the open circuit point on the right.
        ' D: At the center.
        ' On the R=Z0 circle.
        '     Omit: On the resonance line. Already covered by C or D.
        '     E: On R=Z0 circle, above resonance line. Only needs reactance.
        '     F: On R=Z0 circle, below resonance line. Only needs reactance.
        ' Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
        '     G1: Inside R=Z0 circle, above resonance line.
        '     G2: Inside R=Z0 circle, above resonance line, Z0=50
        '     H: Inside R=Z0 circle, on line
        '     I: Inside R=Z0 circle, below resonance line.
        ' On the G=Y0 circle.
        '     Omit: On the resonance line. Already either A or D.
        '     J: On G=Y0 circle, above resonance line. Only needs reactance.
        '     K: On G=Y0 circle, below resonance line. Only needs reactance.
        ' LMN: Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
        ' O: In the top remainder.
        ' P: In the bottom remainder.

        Dim NormR As System.Double = Me.Resistance / z0
        Dim NormX As System.Double = Me.Reactance / z0
        Dim Y0 As System.Double = 1.0 / z0
        Dim Y As Admittance = Me.ToAdmittance()
        Dim NormG As System.Double = Y.Conductance / Y0
        Dim NormB As System.Double = Y.Susceptance / Y0

        ' LEAVE THIS HERE FOR NOW.
        ' OPEN OR SHORT SHOULD HAVE BEEN REJECTED IN NEW() AND THIS SHOULD NOT
        ' BE NEEDED UNLESS SOME REASON IS DISCOVERED THAT REQUIRES EXTREMES TO
        ' BE ALLOWED. MAYBE THAT WILL HAVE TO BE ALLOWED. THAT MIGHT HAPPEN IF
        ' AN IMAGE IMPEDANCE HAS EXTREME VALUES THAT CANCEL OR FOR SOME OTHER
        ' INTERIM STATE. MAYBE IF A MATCH IS BEING MADE TO AN IMAGE IMPEDANCE OR
        ' A SITUATION INVOLVING ACTIVE COMPONENTS THAT CAN HAVE A NEGATIVE
        ' RESITANCE.    
        ' Check for a short- or open-circuit.
        If NormR.Equals(0.0) OrElse System.Double.IsInfinity(NormR) Then
            ' A: At the short circuit point. Omit; Covered by B.
            ' B: Anywhere else on the outer circle. R=0.0
            ' C: At the open circuit point on the right.
            transformations = Nothing
            Return False
        End If

        If NormR >= 1.0 Then
            Return If(NormR.Equals(z0),
                Me.OnREqualsZ0(z0, transformations),
                Me.InsideREqualsZ0(z0, transformations))
        ElseIf NormG >= 1.0 Then
            Return If(NormG.Equals(Y0),
                Me.OnGEqualsY0(z0, transformations),
                Me.InsideGEqualsY0(z0, transformations))
        End If

        ' DELETE THIS AFTER TESTING CONFIRMS THAT IT IS NOT HIT BY ANY TEST CASES.
        ' On getting this far, the impedance will, usually, fall into either
        ' the top or bottom center section.
        If NormX.Equals(0.0) Then
            ' Z is ON the resonance line.

            ' Would this case have been caught above? Yes, it would be in or
            ' on the R or G-circle or at the center.
            Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
            Throw New ApplicationException(
                    """NormX.Equals(0.0)"" should never be matched in " &
                    NameOf(TrySelectTuningLayout))
        ElseIf NormX > 0.0 Then
            ' Z is ABOVE the resonance line, between the G=Y0 and R=Z0 circles.

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            ' Move CW on the G-circle to reach the R=Z0 circle. Use a shunt
            ' capacitor.
            ' Would there ever be a case to prefer the first or second
            ' intersection? Maybe to favor high- or low-pass?
            '          or
            ' Move CCW on the R-circle to reach the G=Y0 circle. Use a
            ' series capacitor.
            ' Would there ever be a case to prefer the first or second
            ' intersection? Maybe to favor high- or low-pass?
            '
            '

            Return False ' DEFAULT UNTIL IMPLEMENTED.
        ElseIf NormX < 0.0 Then
            ' Z is BELOW the resonance line, between the G=Y0 and R=Z0 circles.

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            ' Move CCW on the G-circle to reach the R=Z0 circle. Use a shunt
            ' inductor.
            ' Would there ever be a case to prefer the first or second
            ' intersection? Maybe to favor high- or low-pass?
            '          or
            ' Move CW on the R-circle to reach the G=Y0 circle. Use a
            ' series inductor.
            ' Would there ever be a case to prefer the first or second
            ' intersection? Maybe to favor high- or low-pass?
            '
            '

            Return False ' DEFAULT UNTIL IMPLEMENTED.

        Else
            ' GETTING HERE MEANS THAT NO CASES MATCHED.
            Return False ' DEFAULT UNTIL IMPLEMENTED.
        End If

    End Function ' TrySelectTuningLayout

End Structure ' Impedance
