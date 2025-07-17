Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Diagnostics.CodeAnalysis

' The generic tuning process is intended to be able to select a method to obtain
' a conjugate match for a load impedance to a source characteristic impedance.
' It is not intended to select specific capacitance or inductance values. The
' goal is to be able to lay out a L-section and select a reactance value for
' each component. Those reactance values could then be used to select
' appropriate component values based on frequency.

' The comments here relate to solving conjugate matches on a Smith chart that
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

'              Component impact
' A series inductor moves CW on a R circle.
' A shunt inductor moves CCW on a G circle.
' A series capacitor moves CCW on a R circle.
' A shunt capacitor moves CW on a G circle.

'             Component selection
' Move | On       | Using
' CW   | R circle | series inductor
' CW   | G circle | shunt capacitor
' CCW  | R circle | series capacitor
' CCW  | G circle | shunt inductor

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
    '   PI, T, M, band-pass, and notch filters.
    '   Shunt or series, parallel tank or series-resonant, sections to construct
    '     band-pass or notch filters.
    '   Feedline segments to cause impedance rotation or quarter-wave impedance
    '     transformers, perhaps to allow the use of 75-ohm hard line in a 50-ohm
    '     installation.
    '   Open or closed coax stubs the create band-pass or notch filters.
    '
    '

End Enum ' TransformationStyles

Partial Public Structure Impedance

    ''' <summary>
    ''' xxxxxxxxxxxxxxx
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance to which the
    ''' current instance should be matched.</param>
    ''' <returns>
    ''' xxxxxxx THE RETURN AS DOUBLE IS JUST A PLACEHOLDER xxxxxxxx
    ''' xxxxxxx MAYBE THIS SHOULD BE A SUB AND/OR HAVE BYREF PARAMETERS TO RETURN RESULTS xxxxxxxx
    ''' </returns>
    Public Function SelectLayout(z0 As System.Double) _
        As System.Boolean

        ' The terminology here relates to solving conjugate matches on a Smith
        ' chart.

        Dim NormR As System.Double = Me.Resistance / z0
        Dim NormX As System.Double = Me.Reactance / z0
        Dim NormG As System.Double = Me.ToAdmittance.Conductance / z0
        'Dim NormB As System.Double = Me.ToAdmittance.Susceptance / z0

        ' Check for an open- or short-circuit.
        'If NormX.Equals(0.0) Then
        '    ' Z is on the resonance line.
        '    If NormR.Equals(0.0) OrElse System.Double.IsInfinity(NormR) Then

        '        '
        '        '
        '        ' XXXXX WHAT NEXT? XXXXX
        '        '
        '        '

        '    End If
        'End If

        '' Check for an open- or short-circuit.
        'If NormX.Equals(0.0) AndAlso
        '    (NormR.Equals(0.0) OrElse System.Double.IsInfinity(NormR)) Then
        '    ' Z is on the resonance line, at one end.

        '    '
        '    '
        '    ' XXXXX WHAT NEXT? XXXXX
        '    '
        '    '

        'End If

        ' Check for an open- or short-circuit.
        If NormR.Equals(0.0) OrElse System.Double.IsInfinity(NormR) Then
            ' xxxxxxxxxxxxxxxxxxxxxxxxx

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            '
            '

            Return False ' DEFAULT UNTIL IMPLEMENTED.
        End If

        If NormR.Equals(1.0) Then
            ' Z is on perimeter of the right (R=Z0) circle.
            If NormX.Equals(0.0) Then
                ' Z is already at the origin, where Z=1+j0, and the conjugate
                ' match is good.

                '
                '
                ' XXXXX WHAT NEXT? XXXXX
                '
                '

                Return False ' DEFAULT UNTIL IMPLEMENTED.
            Else
                ' Z is on perimeter of the right (R=Z0) circle and only needs a
                ' reactance.

                '
                '
                ' Move CW or CCW on the R circle to reach the origin.
                ' XXXXX WHAT NEXT? XXXXX
                ' Would there ever be a case for taking the long way around?
                ' Maybe to favor high- or low-pass?
                '
                '

                Return False ' DEFAULT UNTIL IMPLEMENTED.
            End If
        ElseIf NormG.Equals(1.0) Then
            ' Z is on perimeter of the left (G=Y0) circle.

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            ' Move CW or CCW on the G circle to reach the origin.
            ' Would there ever be a case for taking the long way around?
            ' Maybe to favor high- or low-pass?
            '
            '

            Return False ' DEFAULT UNTIL IMPLEMENTED.
        ElseIf NormR < 1.0 Then
            ' Z is INSIDE the right (R=Z0) circle.

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            ' Move CW or CCW on the G circle to reach the R=Z0 circle.
            ' Would there ever be a case for taking the long way around?
            ' Maybe to favor high- or low-pass?
            '
            '

            Return False ' DEFAULT UNTIL IMPLEMENTED.
        ElseIf NormG < 1.0 Then
            ' Z is INSIDE the left (G=Y0) circle.

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            ' Would there ever be a case for taking the long way around?
            ' Maybe to favor high- or low-pass?
            '
            '

            ' On getting this far, the impedance will, usually, fall into either
            ' the top or bottom center section.
            If NormX.Equals(0.0) Then
                ' Z is ON the resonance line.

                '
                '
                ' XXXXX WHAT NEXT? XXXXX
                ' Would this case have been caught above? Yes, it would be in or
                ' on the R or G circle or at the origin.
                '
                '

                ' DELETE THIS AFTER TESTING CONFIRMS THAT IT IS NOT HIT BY ANY TEST CASES.
                Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
                Throw New ApplicationException(
                    """NormX.Equals(0.0)"" should never be matched in " &
                    NameOf(SelectLayout))


                Return False ' DEFAULT UNTIL IMPLEMENTED.
            ElseIf NormX > 0.0 Then
                ' Z is ABOVE the resonance line, between the left (G=Y0) and
                ' right (R=Z0) circles.

                '
                '
                ' XXXXX WHAT NEXT? XXXXX
                ' Move CW on the G circle to reach the R=Z0 circle. Use a shunt
                ' capacitor.
                ' Would there ever be a case to prefer the first or second
                ' intersection? Maybe to favor high- or low-pass?
                '          or
                ' Move CCW on the R circle to reach the G=Y0 circle. Use a
                ' series capacitor.
                ' Would there ever be a case to prefer the first or second
                ' intersection? Maybe to favor high- or low-pass?
                '
                '

                Return False ' DEFAULT UNTIL IMPLEMENTED.
            Else
                ' Z is BELOW the resonance line, between the left (G=Y0) and
                ' right (R=Z0) circles.

                '
                '
                ' XXXXX WHAT NEXT? XXXXX
                ' Move CCW on the G circle to reach the R=Z0 circle. Use a shunt
                ' inductor.
                ' Would there ever be a case to prefer the first or second
                ' intersection? Maybe to favor high- or low-pass?
                '          or
                ' Move CW on the R circle to reach the G=Y0 circle. Use a
                ' series inductor.
                ' Would there ever be a case to prefer the first or second
                ' intersection? Maybe to favor high- or low-pass?
                '
                '

                Return False ' DEFAULT UNTIL IMPLEMENTED.
            End If

        Else
            Return False ' DEFAULT UNTIL IMPLEMENTED.
        End If

    End Function ' SelectLayout

End Structure ' Impedance
