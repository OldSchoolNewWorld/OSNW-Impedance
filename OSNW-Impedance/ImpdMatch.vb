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
' A series inductor moves CW on an R-circle, increasing X.
' A shunt inductor moves CCW on a G-circle, increasing B.
' A series capacitor moves CCW on an R-circle, decreasing X.
' A shunt capacitor moves CW on a G-circle, decreasing B.

'             Component selection
' To go | On a(n)  | Use a
' CW    | R-circle | series inductor, increasing X.
' CW    | G-circle | shunt capacitor, increasing B.
' CCW   | R-circle | series capacitor, decreasing X.
' CCW   | G-circle | shunt inductor, decreasing B.

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
        Dim DeltaZ As Impedance
        Dim WorkZ As Impedance
        Dim TestPassed As System.Boolean = True ' For now.

        If aTransformation.Style.Equals(TransformationStyles.ShuntCap) Then

            ' A shunt capacitor moves CW on a G-circle.

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ)

            ' xxxxxxxxxxxx TESTS CAN BE REMOVED WHEN ALL IS OK.
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
                                             z0 * IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                        "ShuntCap" & MSGTDNRT)
            End If

            ' On getting this far,
            Return True

        ElseIf aTransformation.Style.Equals(TransformationStyles.SeriesInd) Then

            ' A series inductor moves CW on an R-circle.

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ)

            ' xxxxxxxxxxxx TESTS CAN BE REMOVED WHEN ALL IS OK.
            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                        "SeriesInd" & MSGTDNRT)
            End If

            ' On getting this far,
            Return True

        ElseIf aTransformation.Style.Equals(TransformationStyles.ShuntInd) Then

            ' A shunt inductor moves CCW on a G-circle.

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ)

            ' xxxxxxxxxxxx TESTS CAN BE REMOVED WHEN ALL IS OK.
            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                        "ShuntInd" & MSGTDNRT)
            End If

            ' On getting this far,
            Return True

        ElseIf aTransformation.Style.Equals(TransformationStyles.SeriesCap) Then

            ' A series capacitor moves CCW on an R-circle.

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ)

            ' xxxxxxxxxxxx TESTS CAN BE REMOVED WHEN ALL IS OK.
            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
                    TestPassed = False
                End If
            End If
            If Not TestPassed Then
                Throw New System.ApplicationException(
                        "SeriesCap" & MSGTDNRT)
            End If

            ' On getting this far,
            Return True

        ElseIf aTransformation.Style.Equals(
             TransformationStyles.ShuntCapSeriesInd) Then

            ' To go | On a     | Use a
            ' CW    | G-circle | shunt capacitor
            ' CW    | R-circle | series inductor

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

            DeltaZ = New Impedance(0.0, aTransformation.Value1)
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ)
            DeltaZ = New Impedance(0.0, aTransformation.Value2)
            WorkZ = Impedance.AddShuntImpedance(WorkZ, DeltaZ)

            If Not Impedance.EqualEnough(WorkZ.Resistance, ExpectZ.Resistance,
                                         z0 * IMPDTOLERANCE) Then
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
                                             z0 * IMPDTOLERANCE) Then
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

End Structure ' Impedance
