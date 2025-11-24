Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

' This document contains items related to matching a load impedance to a source
' impedance.

' The simple matching process is intended to be able to select a method to
' obtain a conjugate match for a load impedance to a source characteristic
' impedance. It is not intended to select specific capacitance or inductance
' values. The goal is to be able to lay out a single-element, or an L-section,
' and select a reactance value for each component. Those reactance values could
' then be used to select appropriate component values, based on frequency.

' The comments here relate to solving conjugate matches on a Smith Chart that
' has a horizontal resonance line, with R=0 at the left.

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
''' Represents the circuit layout to be used to match a load impedance to a
''' source characteristic impedance or to otherwise modify the impedance.
''' </summary>
''' <remarks>
''' Member names begin with the first component encountered by the load,
''' proceeding toward the source.
''' The default is <c>MatchingLayouts.None.</c>.
''' </remarks>>
Public Enum TransformationStyles

    ''' <summary>
    ''' The default value. No transformation takes place.
    ''' </summary>
    None

    ' Define the four single-element arrangements.

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
    '   PI, T, LC series/tank band pass, LC notch, and M filter sections.
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
    ''' Specifies the transformation style to be used.
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
    ''' Confirms that the specified transformation produces the expected result.
    ''' This is a worker for routines below.
    ''' </summary>
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="aTransformation">Specifies the <see cref="Transformation"/>
    ''' to be used to perform the matching.</param>
    ''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    ''' results in a conjugate match for the current instance; otherwise,
    ''' <c>False</c>.</returns>
    ''' <remarks>
    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    ''' which the current instance should be matched. It should have a practical
    ''' value with regard to the impedance values involved.
    ''' </remarks>
    Public Function ValidateTransformation(ByVal mainCirc As SmithMainCircle,
        ByVal ExpectZ As Impedance, ByVal aTransformation As Transformation) _
        As System.Boolean

        Dim z0 As System.Double = mainCirc.Z0
        Dim NearlyZero As System.Double = z0 * 0.000001
        Dim FixupZ As Impedance
        Dim WorkZ As Impedance
        Dim TestPassed As System.Boolean = True ' For now.

        If aTransformation.Style.Equals(
            TransformationStyles.ShuntCapSeriesInd) Then

            ' To go | On a     | Use a
            ' CW    | G-circle | shunt capacitor
            ' CW    | R-circle | series inductor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance,
                                                 NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "ShuntCapSeriesInd" & MSGTDNRT)
            End If

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.ShuntCapSeriesCap) Then

            ' To go | On a     | Use a
            ' CW    | G-circle | shunt capacitor
            ' CCW   | R-circle | series capacitor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "ShuntCapSeriesCap" & MSGTDNRT)
            End If

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.ShuntIndSeriesCap) Then

            ' To go | On a     | Use a
            ' CCW   | G-circle | shunt inductor
            ' CCW   | R-circle | series capacitor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "ShuntCapSeriesInd" & MSGTDNRT)
            End If

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.ShuntIndSeriesInd) Then

            ' To go | On a     | Use a
            ' CCW   | G-circle | shunt inductor
            ' CW    | R-circle | series inductor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "ShuntIndSeriesInd" & MSGTDNRT)
            End If

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.SeriesCapShuntInd) Then

            ' To go | On a     | Use a
            ' CCW   | R-circle | series capacitor
            ' CCW   | G-circle | shunt inductor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "SeriesCapShuntInd" & MSGTDNRT)
            End If

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.SeriesCapShuntCap) Then

            ' To go | On a     | Use a
            ' CCW   | R-circle | series capacitor
            ' CW    | G-circle | shunt capacitor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "SeriesCapShuntCap" & MSGTDNRT)
            End If

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.SeriesIndShuntCap) Then

            ' To go | On a     | Use a
            ' CW    | R-circle | series inductor
            ' CW    | G-circle | shunt capacitor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "SeriesIndShuntCap" & MSGTDNRT)
            End If

            ' On getting this far,
            Return True

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.SeriesIndShuntInd) Then

            ' To go | On a     | Use a
            ' CW    | R-circle | series inductor
            ' CCW   | G-circle | shunt inductor

            FixupZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, FixupZ)
            FixupZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, FixupZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         IMPDTOLERANCE) Then
                TestPassed = False
            End If
            If Impedance.EqualEnoughZero(ExpectZ.Reactance, NearlyZero) Then
                ' This wants a Z0 match.
                If Not Impedance.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                    TestPassed = False
                End If
            Else
                ' This wants a match to an arbitrary load.
                If Not Impedance.EqualEnough(WorkZ.Reactance, ExpectZ.Reactance,
                                             IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                    "SeriesIndShuntInd" & MSGTDNRT)
            End If

            ' On getting this far,
            Return True

        Else
            ' Invalid transformation style.
            Return False
        End If

        ' On getting this far,
        Return True

    End Function ' ValidateTransformation

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance, when the current
    ''' instance appears directly
    ''' on the R=Z0 circle.
    ''' </summary>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function OnREqualsZ0(ByRef transformations As Transformation()) _
        As System.Boolean

        ' Test data C: At the open circuit point on the right.
        ' Test data D: At the center.
        ' Test data E: On R=Z0 circle, above resonance line. Only needs reactance.
        ' Test data F: On R=Z0 circle, below resonance line. Only needs reactance.

        Dim CurrentX As System.Double = Me.Reactance
        If Impedance.EqualEnoughZero(CurrentX, IMPDTOLERANCE0) Then
            ' This happens at two places.

            If System.Double.IsInfinity(Me.Resistance) Then
                ' Test data C: At the open circuit point on the right.
                Return False
            End If

            ' Test data D1: At the center.
            ' Z is at the center point and already has a conjugate match.
            transformations = {
                New Transformation With {
                    .Style = TransformationStyles.None}
            }
            Return True

        Else
            ' Z is elsewhere on the perimeter of the R=Z0 circle and only needs
            ' a reactance.
            Dim Style As TransformationStyles
            If CurrentX > 0.0 Then
                ' Test data E: On R=Z0 circle, above resonance line.
                ' CCW on an R-circle needs a series capacitor.
                Style = TransformationStyles.SeriesCap
            Else
                ' Test data F: On R=Z0 circle, below resonance line.
                ' CW on an R-circle needs a series inductor.
                Style = TransformationStyles.SeriesInd
            End If
            transformations = {
                New Transformation With {
                    .Style = Style,
                    .Value1 = -CurrentX}
                }
            Return True
        End If
    End Function ' OnREqualsZ0

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance specified by
    ''' <paramref name="z0"/>, when the current instance appears directly on the
    ''' G=Y0 circle.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' <paramref name="z0"/> is the characteristic impedance to which the
    ''' current instance should be matched. It should have a practical value
    ''' with regard to the impedance values involved.
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function OnGEqualsY0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' A: At the short circuit point.
        ' D: At the center.
        ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        ' K: On G=Y0 circle, below resonance line. Only needs reactance.

        Dim CurrentB As System.Double = Me.ToAdmittance().Susceptance
        If Impedance.EqualEnoughZero(CurrentB, Impedance.IMPDTOLERANCE0) Then
            ' This happens at two places.

            If Impedance.EqualEnoughZero(Me.Resistance,
                                         Impedance.IMPDTOLERANCE0) Then
                ' Test data A: At the short circuit point.
                Return False
            End If

            ' Test data D: At the center.
            ' Z is at the center point and already has a conjugate match.
            transformations = {
                New Transformation With {
                    .Style = TransformationStyles.None}
            }
            Return True

        Else
            ' Z is elsewhere on the perimeter of the Y=G0 circle and only needs
            ' a reactance.
            Dim Style As TransformationStyles
            Dim DeltaZ As Impedance = New Admittance(0.0, -CurrentB).ToImpedance
            If CurrentB > 0.0 Then
                ' Test data K: On G=Y0 circle, below resonance line.
                ' CCW on a G-circle needs a shunt inductor.
                Style = TransformationStyles.ShuntInd
            Else
                ' Test data J: On G=Y0 circle, above resonance line.
                ' CW on a G-circle needs a shunt capacitor.
                Style = TransformationStyles.ShuntCap
            End If
            transformations = {
                New Transformation With {
                    .Style = Style,
                    .Value1 = DeltaZ.Reactance}
                }
            Return True
        End If

    End Function ' OnGEqualsY0

    '''' <summary>
    ''''  Processes one intersection found in
    ''''  <see cref="M:InsideREqualsZ0(z0, transformations)"/>".>
    '''' </summary>
    '''' <param name="mainCirc">Specifies an arbitrary
    '''' <see cref="SmithMainCircle"/> reference for calculations.</param>
    '''' <param name="intersection">Specifies the Cartesian coordinates of one
    '''' intersection of R- and G-circles.</param>
    '''' <param name="transformation"> Returns a <see cref="Transformation"/>
    '''' that can be used to match a load impedance, located at the specified
    '''' <paramref name="intersection"/>, to match a source impedance.</param>
    '''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    '''' results in a conjugate match for the current instance; otherwise,
    '''' <c>False</c>.</returns>
    '''' <remarks>
    '''' <para> An assumption is made that the calling code has determined that
    '''' the current instance lies in the expected position. Failure to meet that
    '''' assumption could result in an invalid or incomplete result. </para>
    '''' </remarks>
    Private Function InsideREqualsZ0(ByVal mainCirc As SmithMainCircle,
        ByVal intersection As OSNW.Numerics.PointD,
        ByRef transformation As Transformation) As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        Try

            ' First move, to the image impedance.
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(intersection.X, intersection.Y)
            Dim DiffImageB As System.Double =
                ImageY.Susceptance - Me.ToAdmittance().Susceptance

            ' Second move, to the center.
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(intersection.X, intersection.Y)
            Dim DiffFinalX As System.Double = -ImageZ.Reactance

            ' Select the transformations, based on the location of the
            ' intersection relative to the resonance line.
            If intersection.Y > mainCirc.GridCenterY Then
                ' Intersection above the resonance line.

                ' Use a shunt inductor to move CCW the G-circle to the R=Z0
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

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance specified by
    ''' <paramref name="z0"/>, when the current instance appears inside the
    ''' R=Z0 circle.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' <paramref name="z0"/> is the characteristic impedance to which the
    ''' current instance should be matched. It should have a practical value
    ''' with regard to the impedance values involved.
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InsideREqualsZ0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' The first move will be to the intersection of the R=Z0 circle and the
        ' G-circle that includes the load impedance. From inside the R=Z0
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
        '  - To favor availability of suitable components for the frequency of
        '      interest?

        ' Determine the circles and their intersections.
        'Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
        Dim CircR As New RCircle(MainCirc, z0)
        Dim CircG As New GCircle(MainCirc, Me.ToAdmittance().Conductance)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
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
        '' There should now be two valid solutions that match to Z=Z0+j0.0.
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

    ''' <summary>
    '''  Processes one intersection found in
    '''  <see cref="M:InsideGEqualsY0(z0, transformations)"/>".>
    ''' </summary>
    ''' <param name="mainCirc">Specifies an arbitrary
    ''' <see cref="SmithMainCircle"/> reference for calculations.</param>
    ''' <param name="intersection">Specifies the Cartesian coordinates of one
    ''' intersection of R- and G-circles.</param>
    ''' <param name="transformation"> Returns a <see cref="Transformation"/>
    ''' that can be used to match a load impedance, located at the specified
    ''' <paramref name="intersection"/>, to match a source impedance.</param>
    ''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    ''' results in a conjugate match for the current instance; otherwise,
    ''' <c>False</c>.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' </remarks>
    Private Function InsideGEqualsY0(ByVal mainCirc As SmithMainCircle,
        ByVal intersection As OSNW.Numerics.PointD,
        ByRef transformation As Transformation) As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        Try

            ' First move, to the image impedance.
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(intersection.X, intersection.Y)
            Dim DiffImageX As System.Double =
                ImageZ.Reactance - Me.Reactance

            ' Second move, to the center.
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(intersection.X, intersection.Y)
            Dim DiffFinalG As System.Double = -ImageY.Susceptance
            Dim FinalY As New Admittance(0.0, DiffFinalG)
            Dim FinalX As Impedance = FinalY.ToImpedance

            ' Select the transformations, based on the location of the
            ' intersection relative to the resonance line.
            If intersection.Y > mainCirc.GridCenterY Then
                ' Intersection above the resonance line.

                ' Use a series inductor to move CW on the R-circle to the G=Y0
                ' circle, then use a shunt capacitor to move CW on the G=Y0
                ' circle to the center.
                '               transformation = New Transformation With {
                '                   .Style = TransformationStyles.SeriesIndShuntCap,
                '                   .Value1 = DiffImageX,
                '                   .Value2 = DiffFinalY
                '               }
                transformation = New Transformation With {
                    .Style = TransformationStyles.SeriesIndShuntCap,
                    .Value1 = DiffImageX,
                    .Value2 = FinalX.Reactance
                }
            Else
                ' Intersection below the resonance line.

                '  Use a series capacitor to move CCW on the R-circle to the G=Y0
                '  circle, then use a shunt inductor to move CCW on the G=Y0
                '  circle to the center.
                transformation = New Transformation With {
                    .Style = TransformationStyles.SeriesCapShuntInd,
                    .Value1 = DiffImageX,
                    .Value2 = FinalX.Reactance
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

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance of
    ''' <paramref name="mainCirc"/>, when the current instance appears inside the
    ''' G=Y0 circle.
    ''' </summary>
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    ''' which the current instance should be matched. It should have a practical
    ''' value with regard to the impedance values involved.
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InsideGEqualsY0(ByVal mainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

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
        Dim CircG As New GCircle(mainCirc, 1.0 / mainCirc.Z0)
        Dim CircR As New RCircle(mainCirc, Me.Resistance)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
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
        If Not EqualEnough(Intersections(0).X, Intersections(0).X,
                           GRAPHICTOLERANCE) Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("X values do not match.")
        End If
        ' The Y values should be the same distance above and below the
        ' resonance line. Check for reasonable equality when using floating
        ' point values.
        Dim Offset0 As System.Double =
            System.Math.Abs(Intersections(0).Y - mainCirc.GridCenterY)
        Dim Offset1 As System.Double =
            System.Math.Abs(Intersections(1).Y - mainCirc.GridCenterY)
        If Not EqualEnough(Offset1, Offset0, GRAPHICTOLERANCE) Then
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
            mainCirc, Intersections(0), Transformation0) Then

            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("Transformation 0 failed.")
        End If
        Dim Transformation1 As Transformation
        If Not Me.InsideGEqualsY0(
            mainCirc, Intersections(1), Transformation1) Then

            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException("Transformation 1 failed.")
        End If

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
        ' RESULTS ARE KNOWN TO BE CORRECT.
        ' There should now be two valid solutions that match to Z=Z0+j0.0.
        Dim ExpectZ As New Impedance(mainCirc.Z0, 0)
        ' Check first solution.
        If Not ValidateTransformation(mainCirc, ExpectZ, Transformation0) Then
            Return False
        End If
        ' Check second solution.
        If Not ValidateTransformation(mainCirc, ExpectZ, Transformation1) Then
            Return False
        End If

        ' On getting this far,
        Return True

    End Function ' InsideGEqualsY0

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance of
    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    ''' top central area. This is to have the first move go CW.
    ''' </summary>
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="transformations">Accumulates an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to a source impedance.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' Also returns, by reference in <paramref name="transformations"/>, the
    ''' components to construct the match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    ''' which the current instance should be matched. It should have a practical
    ''' value with regard to the impedance values involved. A succcessful
    ''' process might result in an empty <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InTopCenterCW(ByVal mainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        Dim Y As Admittance = Me.ToAdmittance()

        ' Move CW on the G-circle to reach the R=Z0 circle. Use a shunt
        ' capacitor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?

        ' Determine the circle intersections.
        Dim CircG As New GCircle(mainCirc, Y.Conductance)
        Dim CircR As New RCircle(mainCirc, mainCirc.Z0)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                GenericCircle.GetIntersections(CircR, CircG)

        ' Process each intersection.
        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

            ' Determine the changes to take place.
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaB As System.Double =
                ImageY.Susceptance - Y.Susceptance
            Dim DeltaY As New Admittance(0, DeltaB)
            Dim DeltaZ As Impedance = DeltaY.ToImpedance
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaX As System.Double = -ImageZ.Reactance

            ' Set up the transformation.
            Dim Trans As New Transformation
            With Trans
                If OneIntersection.Y > mainCirc.GridCenterY Then
                    ' The short first move. Now CCW on R-Circle.
                    .Style = TransformationStyles.ShuntCapSeriesCap
                Else
                    ' The long first move. Now CW on R-Circle.
                    .Style = TransformationStyles.ShuntCapSeriesInd
                End If
                .Value1 = DeltaZ.Reactance
                .Value2 = DeltaX
            End With

            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
            ' RESULTS ARE KNOWN TO BE CORRECT.
            ' There should now be a valid solution that matches to Z=Z0+j0.0.
            If Not ValidateTransformation(
                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
                Return False
            End If

            Dim CurrTransCount As System.Int32 = transformations.Length
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans

        Next

        ' On getting this far,
        Return True

    End Function ' InTopCenterCW

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance of
    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    ''' top central area. This is to have the first move go CCW.
    ''' </summary>
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="transformations">Accumulates an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to a source impedance.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' Also returns, by reference in <paramref name="transformations"/>, the
    ''' components to construct the match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    ''' which the current instance should be matched. It should have a practical
    ''' value with regard to the impedance values involved. A succcessful
    ''' process might result in an empty <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InTopCenterCCW(ByVal mainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' Move CCW on the R-circle to reach the G=Y0 circle. Use a
        ' series capacitor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?

        ' Determine the circle intersections.
        Dim CircG As New GCircle(mainCirc, mainCirc.Y0)
        Dim CircR As New RCircle(mainCirc, Me.Resistance)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                GenericCircle.GetIntersections(CircR, CircG)

        ' Process each intersection.
        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

            ' Determine the changes to take place.
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaX As System.Double =
                ImageZ.Reactance - Me.Reactance
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaB As System.Double = -ImageY.Susceptance
            Dim FinalY As New Admittance(0.0, DeltaB)
            Dim FinalZ As Impedance = FinalY.ToImpedance

            ' Set up the transformation.
            Dim Trans As New Transformation
            If OneIntersection.Y > mainCirc.GridCenterY Then
                ' The short first move. Now CCW on R-Circle.
                Trans.Style = TransformationStyles.SeriesCapShuntCap
            Else
                ' The long first move. Now CW on R-Circle.
                Trans.Style = TransformationStyles.SeriesCapShuntInd
            End If
            With Trans
                .Value1 = DeltaX
                .Value2 = FinalZ.Reactance
            End With

            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
            ' RESULTS ARE KNOWN TO BE CORRECT.
            ' There should now be a valid solution that matches to Z=Z0+j0.0.
            If Not ValidateTransformation(
                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
                Return False
            End If

            Dim CurrTransCount As System.Int32 = transformations.Length
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans

        Next

        ' On getting this far,
        Return True

    End Function ' InTopCenterCCW

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance specified by
    ''' <paramref name="z0"/>, when the current instance appears in the top
    ''' central area.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' <paramref name="z0"/> is the characteristic impedance to which the
    ''' current instance should be matched. It should have a practical value
    ''' with regard to the impedance values involved.
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InTopCenter(ByVal z0 As System.Double,
        ByRef MainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' Move CW on the G-circle to reach the R=Z0 circle. Use a shunt
        ' capacitor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?

        If Not Me.InTopCenterCW(
            MainCirc, transformations) Then

            Return False
        End If

        ' Move CCW on the R-circle to reach the G=Y0 circle. Use a
        ' series capacitor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?
        If Not Me.InTopCenterCCW(
            MainCirc, transformations) Then

            Return False
        End If

        ' On getting this far,
        Return True

    End Function ' InTopCenter

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance of
    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    ''' bottom central area. This is to have the first move go CW.
    ''' </summary>
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="transformations">Accumulates an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to a source impedance.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' Also returns, by reference in <paramref name="transformations"/>, the
    ''' components to construct the match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    ''' which the current instance should be matched. It should have a practical
    ''' value with regard to the impedance values involved. A succcessful
    ''' process might result in an empty <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InBottomCenterCW(ByVal mainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' Move CW on the R-circle to reach the G=Y0 circle. Use a series
        ' inductor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?

        ' Determine the circle intersections.
        Dim CircG As New GCircle(mainCirc, mainCirc.Y0)
        Dim CircR As New RCircle(mainCirc, Me.Resistance)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                GenericCircle.GetIntersections(CircR, CircG)

        ' Process each intersection.
        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

            ' Determine the changes to take place.
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaX As System.Double =
                ImageZ.Reactance - Me.Reactance
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaB As System.Double = -ImageY.Susceptance
            Dim FinalY As New Admittance(0.0, DeltaB)
            Dim FinalZ As Impedance = FinalY.ToImpedance

            ' Set up the transformation.
            Dim Trans As New Transformation
            If OneIntersection.Y > mainCirc.GridCenterY Then
                ' The long first move. Now CW on G-Circle.
                Trans.Style = TransformationStyles.SeriesIndShuntCap
            Else
                ' The short first move. Now CCW on G-Circle.
                Trans.Style = TransformationStyles.SeriesIndShuntInd
            End If
            With Trans
                .Value1 = DeltaX
                .Value2 = FinalZ.Reactance
            End With

            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
            ' RESULTS ARE KNOWN TO BE CORRECT.
            ' There should now be a valid solution that matches to Z=Z0+j0.0.
            If Not ValidateTransformation(
                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
                Return False
            End If

            Dim CurrTransCount As System.Int32 = transformations.Length
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans

        Next

        ' On getting this far,
        Return True

    End Function ' InBottomCenterCW

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance of
    ''' <paramref name="mainCirc"/>, when the current instance appears in the
    ''' bottom central area. This is to have the first move go CCW.
    ''' </summary>
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="transformations">Accumulates an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to a source impedance.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' Also returns, by reference in <paramref name="transformations"/>, the
    ''' components to construct the match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' Z0 in <paramref name="mainCirc"/> is the characteristic impedance to
    ''' which the current instance should be matched. It should have a practical
    ''' value with regard to the impedance values involved. A succcessful
    ''' process might result in an empty <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InBottomCenterCCW(ByVal mainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        Dim Y As Admittance = Me.ToAdmittance()

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' Move CCW on the G-circle to reach the R=Z0 circle. Use a
        ' shunt inductor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?

        ' Determine the circle intersections.
        Dim CircG As New GCircle(mainCirc, Y.Conductance)
        Dim CircR As New RCircle(mainCirc, mainCirc.Z0)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                GenericCircle.GetIntersections(CircR, CircG)

        ' Process each intersection.
        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

            ' Determine the changes to take place.
            Dim ImageY As Admittance =
                mainCirc.GetYFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaB As System.Double =
                ImageY.Susceptance - Y.Susceptance
            Dim FixupY As New Admittance(0.0, DeltaB)
            Dim FixupZ As Impedance = FixupY.ToImpedance
            Dim ImageZ As Impedance =
                mainCirc.GetZFromPlot(OneIntersection.X, OneIntersection.Y)
            Dim DeltaX As System.Double = -ImageZ.Reactance

            ' Set up the transformation.
            Dim Trans As New Transformation
            If OneIntersection.Y > mainCirc.GridCenterY Then
                ' The short first move. Now CCW on R-Circle.
                Trans.Style = TransformationStyles.ShuntIndSeriesCap
            Else
                ' The long first move. Now CW on R-Circle.
                Trans.Style = TransformationStyles.ShuntIndSeriesInd
            End If
            With Trans
                .Value1 = FixupZ.Reactance
                .Value2 = DeltaX
            End With

            ' THIS CHECK CAN BE DELETED/COMMENTED AFTER THE Transformation
            ' RESULTS ARE KNOWN TO BE CORRECT.
            ' There should now be a valid solution that matches to Z=Z0+j0.0.
            If Not ValidateTransformation(
                mainCirc, New Impedance(mainCirc.Z0, 0), Trans) Then
                Return False
            End If

            Dim CurrTransCount As System.Int32 = transformations.Length
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans

        Next

        ' On getting this far,
        Return True

    End Function ' InBottomCenterCCW

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance specified by
    ''' <paramref name="z0"/>, when the current instance appears in the Bottom
    ''' central area.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' <paramref name="z0"/> is the characteristic impedance to which the
    ''' current instance should be matched. It should have a practical value
    ''' with regard to the impedance values involved.
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InBottomCenter(ByVal z0 As System.Double,
        ByRef MainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' Move CW on the G-circle to reach the R=Z0 circle. Use a shunt
        ' capacitor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?

        If Not Me.InBottomCenterCW(
            MainCirc, transformations) Then

            Return False
        End If

        ' Move CCW on the R-circle to reach the G=Y0 circle. Use a
        ' series capacitor. Two choices where to end.
        ' Would there ever be a case to prefer the first or second
        ' intersection? Maybe to favor high- or low-pass?
        If Not Me.InBottomCenterCCW(
            MainCirc, transformations) Then

            Return False
        End If

        ' On getting this far,
        Return True

    End Function ' InBottomCenter

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance specified by
    ''' <paramref name="z0"/>, when the current instance appears in the top or
    ''' bottom central area.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns>
    ''' Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the current instance lies in the expected position. Failure to meet that
    ''' assumption could result in an invalid or incomplete result. </para>
    ''' <paramref name="z0"/> is the characteristic impedance to which the
    ''' current instance should be matched. It should have a practical value
    ''' with regard to the impedance values involved.
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Private Function InRemainder(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' Assign the outer circle.
        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.

        ' Try to solve in the appropriate space.
        If Me.Reactance > 0.0 Then
            ' Z is ABOVE the resonance line, between the G=Y0 and R=Z0 circles.
            Return Me.InTopCenter(z0, MainCirc, transformations) ' O.
        ElseIf Me.Reactance < 0.0 Then
            ' Z is BELOW the resonance line, between the G=Y0 and R=Z0 circles.
            Return Me.InBottomCenter(z0, MainCirc, transformations) ' P.
        End If

        Return False ' DEFAULT UNTIL IMPLEMENTED.

    End Function ' InRemainder

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the current instance (load
    ''' impedance) to the source characteristic impedance of
    ''' <paramref name="mainCirc"/>.
    ''' </summary> 
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load impedance
    ''' to match a source impedance.</param>
    ''' <returns><c>True</c> if a conjugate match solution is found and also
    ''' returns the components to construct the match; otherwise, <c>False</c>.
    ''' </returns>
    ''' <remarks>
    ''' An already-matched impedance returns <c>True</c>, with
    ''' <c>Nothing</c>/<c>Null</c> for <paramref name="transformations"/>.
    ''' </remarks>
    Public Function TrySelectMatchLayout(ByVal mainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' DEV: This development implementation is based on selection of pure
        ' impedances. A future derivation might need to select the nearest
        ' commonly available component values, as a practical consideration. In
        ' that case, the math should be changed to add an impedance with actual
        ' R/X values.

        ' The terminology here relates to solving conjugate matches on a Smith
        ' Chart.

        ' Chart location cases:
        ' A: At the short circuit point. Omit - covered by B.
        ' B: Anywhere else on the perimeter. R=0.0.
        ' C: At the open circuit point on the right.
        ' D1: At the center.
        ' On the R=Z0 circle.
        '     Omit: On the resonance line. Already covered by C or D.
        '     E: On R=Z0 circle, above resonance line. Only needs reactance.
        '     F: On R=Z0 circle, below resonance line. Only needs reactance.
        ' Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
        '     G1: Inside R=Z0 circle, above resonance line.
        '     G50: Inside R=Z0 circle, above resonance line. Z0=50.
        '     H1: Inside R=Z0 circle, on line.
        '     I1: Inside R=Z0 circle, below resonance line.
        ' On the G=Y0 circle.
        '     Omit: On the resonance line. Already either A or D.
        '     J: On G=Y0 circle, above resonance line. Only needs reactance.
        '     K: On G=Y0 circle, below resonance line. Only needs reactance.
        ' Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
        '     L1: Inside G=Y0 circle, above resonance line.
        '     L75: Inside G=Y0 circle, above resonance line. Z0=75.
        '     M1: Inside G=Y0 circle, on line.
        '     N1: Inside G=Y0 circle, below line.
        ' O: In the top center.
        '     O1: In the top center.
        '     O50: In the top center. Z0=50.
        ' P: In the bottom center.
        '     P1: In the bottom center.
        '     P50: In the bottom center. Z0=50.
        ' Q: Outside of main circle. Invalid.
        ' R: NormR<=0. Invalid.

        Dim Z0 As System.Double = mainCirc.Z0
        Dim CurrentR As System.Double = Me.Resistance
        Dim Y0 As System.Double = 1.0 / Z0
        Dim CurrentG As System.Double = Me.ToAdmittance().Conductance * Z0

        ' LEAVE THIS HERE FOR NOW.
        ' OPEN OR SHORT SHOULD HAVE BEEN REJECTED IN NEW() AND THIS SHOULD NOT
        ' BE NEEDED UNLESS SOME REASON IS DISCOVERED THAT REQUIRES EXTREMES TO
        ' BE ALLOWED. THAT MIGHT HAPPEN IF AN IMAGE IMPEDANCE HAS EXTREME VALUES
        ' THAT CANCEL OR FOR SOME OTHER INTERIM STATE. MAYBE IF A MATCH IS BEING
        ' MADE TO AN IMAGE IMPEDANCE OR A SITUATION INVOLVING ACTIVE COMPONENTS
        ' THAT CAN EFFECTIVELY HAVE A NEGATIVE RESITANCE VALUE.
        ' Check for a short- or open-circuit.
        If Impedance.EqualEnoughZero(CurrentR, IMPDTOLERANCE) OrElse
            System.Double.IsInfinity(CurrentR) Then
            ' A: At the short circuit point. Omit - covered by B.
            ' B: Anywhere else on the perimeter. R=0.0.
            ' C: At the open circuit point on the right.

            transformations = Nothing
            Return False
        End If

        If Impedance.EqualEnough(CurrentR, Z0, IMPDTOLERANCE) AndAlso
            Impedance.EqualEnoughZero(Me.Reactance, IMPDTOLERANCE0) Then
            ' D: At the center.
            ' Leave transformations as the incoming empty array.

            Return True
        End If

        If CurrentR >= Z0 Then
            ' On the R=Z0 circle.
            '     Omit: On the resonance line. Already covered by C or D.
            '     E: On R=Z0 circle, above resonance line. Only needs reactance.
            '     F: On R=Z0 circle, below resonance line. Only needs reactance.
            ' Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
            '     G1: Inside R=Z0 circle, above resonance line.
            '     G50: Inside R=Z0 circle, above resonance line. Z0=50.
            '     H1: Inside R=Z0 circle, on line.
            '     I1: Inside R=Z0 circle, below resonance line.
            If Impedance.EqualEnough(CurrentR, Z0, IMPDTOLERANCE) Then
                Return Me.OnREqualsZ0(transformations) ' E, F.
            Else
                Return Me.InsideREqualsZ0(Z0, transformations) 'G, H, I.
            End If
        ElseIf CurrentG >= 1.0 Then
            ' On the G=Y0 circle.
            '     Omit: On the resonance line. Already covered by A or D.
            '     J: On G=Y0 circle, above resonance line. Only needs reactance.
            '     K: On G=Y0 circle, below resonance line. Only needs reactance.
            ' Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
            '     L1: Inside G=Y0 circle, above resonance line.
            '     L75: Inside G=Y0 circle, above resonance line. Z0=75.
            '     M1: Inside G=Y0 circle, on line.
            '     N1: Inside G=Y0 circle, below line.
            If Impedance.EqualEnough(Me.ToAdmittance().Conductance, Y0,
                                     IMPDTOLERANCE) Then
                Return Me.OnGEqualsY0(Z0, transformations) ' J, K.
            Else
                Return Me.InsideGEqualsY0(mainCirc, transformations) ' L, M, N.
            End If
        End If

        ' DELETE THIS AFTER TESTING CONFIRMS THAT IT IS NEVER HIT BY ANY TEST CASES.
        ' On getting this far, the impedance will, usually, fall into either
        ' the top or bottom center section.
        Dim NormX As System.Double = Me.Reactance / Z0
        If Impedance.EqualEnoughZero(NormX, IMPDTOLERANCE) Then
            ' Z is ON the resonance line.

            ' Should this case have been caught above? Yes, it would be in or
            ' on the R- or G-circle, or at the center.
            Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
            Throw New ApplicationException(
                    """EqualEnoughZero(NormX, TOLERANCE)"" should never be" &
                    " matched in " & NameOf(TrySelectMatchLayout))
        End If

        Return Me.InRemainder(Z0, transformations)

        ' GETTING HERE MEANS THAT NO CASES MATCHED.
        Return False ' DEFAULT UNTIL IMPLEMENTED.

    End Function ' TrySelectMatchLayout

End Structure ' Impedance
