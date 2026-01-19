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

'             Component selection
' To go | On a(n)  | Use a
' CW    | R-circle | series inductor, increasing X.
' CW    | G-circle | shunt capacitor, increasing B.
' CCW   | R-circle | series capacitor, decreasing X.
' CCW   | G-circle | shunt inductor, decreasing B.

'              Component impact
' A series inductor moves CW on an R-circle, increasing X.
' A shunt inductor moves CCW on a G-circle, increasing B.
' A series capacitor moves CCW on an R-circle, decreasing X.
' A shunt capacitor moves CW on a G-circle, decreasing B.

''' <summary>
''' Represents the circuit layout to be used to match a load impedance to a
''' source characteristic impedance or to otherwise modify the impedance.
''' </summary>
''' <remarks>
''' Member names begin with the first component encountered by the load,
''' proceeding toward the source.
''' The default is <c>MatchingLayouts.None</c>.
''' </remarks>>
Public Enum TransformationStyles

    ''' <summary>
    ''' The default value. No transformation takes place.
    ''' </summary>
    None

    ' Define the four single-element possibilities.

    ''' <summary>
    ''' Source &lt;-- ShuntCap &lt;-- Load<br/>
    ''' A shunt capacitor moves CW on a G-circle, decreasing B.
    ''' </summary>
    ShuntCap

    ''' <summary>
    ''' Source &lt;-- ShuntInd &lt;-- Load<br/>
    ''' A shunt inductor moves CCW on a G-circle, increasing B.
    ''' </summary>
    ShuntInd

    ''' <summary>
    ''' Source &lt;-- SeriesCap &lt;-- Load<br/>
    ''' A series capacitor moves CCW on an R-circle, decreasing X.
    ''' </summary>
    SeriesCap

    ''' <summary>
    ''' Source &lt;-- SeriesInd &lt;-- Load<br/>
    ''' A series inductor moves CW on an R-circle, increasing X.
    ''' </summary>
    SeriesInd

    ' The eight L-sections below follow the sequence shown on page 21 of the link
    ' below, from left-to-right per row.
    ' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

    ''' <summary>
    ''' Source &lt;-- SeriesInd &lt;-- ShuntCap &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
    ''' </summary>
    ShuntCapSeriesInd

    ''' <summary>
    ''' Source &lt;-- SeriesCap &lt;-- ShuntCap &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
    ''' </summary>
    ShuntCapSeriesCap

    ''' <summary>
    ''' Source &lt;-- SeriesCap &lt;-- ShuntInd &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
    ''' </summary>
    ShuntIndSeriesCap

    ''' <summary>
    ''' Source &lt;-- SeriesInd &lt;-- ShuntInd &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
    ''' </summary>
    ShuntIndSeriesInd

    ''' <summary>
    ''' Source &lt;-- ShuntInd &lt;-- SeriesCap &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
    ''' </summary>
    SeriesCapShuntInd

    ''' <summary>
    ''' Source &lt;-- ShuntCap &lt;-- SeriesCap &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
    ''' </summary>
    SeriesCapShuntCap

    ''' <summary>
    ''' Source &lt;-- ShuntCap &lt;-- SeriesInd &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
    ''' </summary>
    SeriesIndShuntCap

    ''' <summary>
    ''' Source &lt;-- ShuntInd &lt;-- SeriesInd &lt;-- Load<br/>
    ''' Member names begin with the first component encountered by the load,
    ''' proceeding toward the source.
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
    ''' Confirms that the specified <see cref="Transformation"/> produces the
    ''' expected result. This is a worker for <seealso cref="MatchArbitrary"/>.
    ''' </summary>
    ''' <param name="mainCirc">Specifies the <see cref="SmithMainCircle"/> with
    ''' which the current instance is associated.</param>
    ''' <param name="sourceZ">Specifies the source <c>Impedance</c> to which the
    ''' current instance should be matched.</param>
    ''' <param name="aTransformation">Specifies the <see cref="Transformation"/>
    ''' to be used to perform the matching.</param>
    ''' <returns><c>True</c> if the proposed <see cref="Transformation"/>
    ''' results in a conjugate match for the current instance to
    ''' <paramref name="sourceZ"/>; otherwise, <c>False</c>.</returns>
    Public Function ValidateTransformation(ByVal mainCirc As SmithMainCircle,
        ByVal sourceZ As Impedance, ByVal aTransformation As Transformation) _
        As System.Boolean

        Dim DeltaZ1 As New Impedance(0.0, aTransformation.Value1)
        Dim DeltaZ2 As New Impedance(0.0, aTransformation.Value2)
        Dim WorkZ As Impedance

        If aTransformation.Style.Equals(TransformationStyles.ShuntCap) OrElse
            aTransformation.Style.Equals(TransformationStyles.ShuntInd) Then

            ' A shunt capacitor moves CW on a G-circle, decreasing B.
            ' A shunt inductor moves CCW on a G-circle, increasing B.
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ1)

        ElseIf aTransformation.Style.Equals(
            TransformationStyles.SeriesInd) OrElse
            aTransformation.Style.Equals(TransformationStyles.SeriesCap) Then

            ' A series inductor moves CW on an R-circle, increasing X.
            ' A series capacitor moves CCW on an R-circle, decreasing X.
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ1)

        ElseIf aTransformation.Style.Equals(
                TransformationStyles.ShuntCapSeriesInd) OrElse
            aTransformation.Style.Equals(
                TransformationStyles.ShuntCapSeriesCap) OrElse
            aTransformation.Style.Equals(
                TransformationStyles.ShuntIndSeriesCap) OrElse
            aTransformation.Style.Equals(
                TransformationStyles.ShuntIndSeriesInd) Then

            ' The first change is a shunt component.
            ' A shunt inductor moves CCW on a G-circle, increasing B.
            ' A shunt capacitor moves CW on a G-circle, decreasing B.
            WorkZ = Impedance.AddShuntImpedance(Me, DeltaZ1)

            ' The second change is a series component.
            ' A series inductor moves CW on an R-circle, increasing X.
            ' A series capacitor moves CCW on an R-circle, decreasing X.
            WorkZ = Impedance.AddSeriesImpedance(WorkZ, DeltaZ2)

        ElseIf aTransformation.Style.Equals(
                TransformationStyles.SeriesCapShuntInd) OrElse
            aTransformation.Style.Equals(
               TransformationStyles.SeriesCapShuntCap) OrElse
            aTransformation.Style.Equals(
                TransformationStyles.SeriesIndShuntCap) OrElse
            aTransformation.Style.Equals(
                TransformationStyles.SeriesIndShuntInd) Then

            ' The first change is a series component.
            ' A series inductor moves CW on an R-circle, increasing X.
            ' A series capacitor moves CCW on an R-circle, decreasing X.
            WorkZ = Impedance.AddSeriesImpedance(Me, DeltaZ1)

            ' The second change is a shunt component.
            ' A shunt inductor moves CCW on a G-circle, increasing B.
            ' A shunt capacitor moves CW on a G-circle, decreasing B.
            WorkZ = Impedance.AddShuntImpedance(WorkZ, DeltaZ2)

        Else
            ' Invalid transformation style.
            Return False
        End If

        ' xxxxxxxxxxxx TESTS CAN BE REMOVED WHEN ALL IS OK.
        Dim Z0 As System.Double = mainCirc.Z0
        Dim TestPassed As System.Boolean = True ' For now.
        If Not OSNW.Math.EqualEnough(WorkZ.Resistance, sourceZ.Resistance,
                                     DFLTIMPDTOLERANCE) Then
            TestPassed = False
        End If
        Dim NearlyZero As System.Double = Z0 * DFLTIMPDTOLERANCE0
        If OSNW.Math.EqualEnoughZero(sourceZ.Reactance, NearlyZero) Then
            ' This wants a Z0 match.
            If Not OSNW.Math.EqualEnoughZero(WorkZ.Reactance, NearlyZero) Then
                TestPassed = False
            End If
        Else
            ' This wants a match to an arbitrary load.
            If Not OSNW.Math.EqualEnough(WorkZ.Reactance, sourceZ.Reactance,
                                         DFLTIMPDTOLERANCE) Then

                TestPassed = False
            End If
        End If
        If Not TestPassed Then
            Return False
        End If

        ' On getting this far,
        Return True

    End Function ' ValidateTransformation

End Structure ' Impedance
