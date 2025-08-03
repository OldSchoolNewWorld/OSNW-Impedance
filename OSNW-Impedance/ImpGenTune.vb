Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

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

' Smith Chart Table of Contents
' http://www.antenna-theory.com/tutorial/smith/chart.php

'              Component impact
' A series inductor moves CW on a R circle.
' A shunt inductor moves CCW on a G circle.
' A series capacitor moves CCW on a R circle.
' A shunt capacitor moves CW on a G circle.

'             Component selection
' To go | On a     | Use a
' CW    | R circle | series inductor
' CW    | G circle | shunt capacitor
' CCW   | R circle | series capacitor
' CCW   | G circle | shunt inductor

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

    '''' <summary>
    '''' Z is on perimeter of the R=Z0 circle.
    '''' </summary>
    '''' <param name="z0">xxxxxxxxxx</param>
    '''' <param name="transformations">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    Private Function NormREqualsZ0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        'Dim NormR As System.Double = Me.Resistance / z0
        Dim NormX As System.Double = Me.Reactance / z0
        'Dim Y0 As System.Double = 1.0 / z0
        'Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0

        If NormX.Equals(0.0) Then
            ' This happens at two places. One would have been handled as
            ' position C. The other is at the center of the chart.

            ' D: At the center.
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
                ' E1: Above the resonance line. Only needs reactance.
                ' CCW on a R circle needs a series capacitor.
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.SeriesCap,
                    .Value1 = -NormX}
                }
                Return True

                ' Consider alternative approaches.
                ' CW on a R circle would need a series inductor, increasing
                ' the inductance of an already inductive load. NO.
                ' What about tuning the equivalent admittance?
                ' CCW on a G circle would need a shunt inductor, reducing
                ' but not canceling the reactance. NO.
                ' CW on a G circle would need a shunt capacitor. For Z=1+j3,
                ' Y=0.1-j0.3. Adding a shunt capacitor 0+j0.3 results in a
                ' total admittance Y=0.1+j0. For Y=0.1+j0, Z=10+j0. NO.

            Else
                ' E2: Below the resonance line. Only needs reactance.
                ' CW on a R circle needs a series inductor.
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.SeriesInd,
                    .Value1 = -NormX}
                }
                Return True
            End If
        End If
    End Function ' NormREqualsZ0

    '''' <summary>
    '''' Z is on the perimeter of the G=Y0 circle.
    '''' </summary>
    '''' <param name="z0">xxxxxxxxxx</param>
    '''' <param name="transformations">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    Private Function NormGEquals1(ByVal z0 As System.Double,
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
                ' CW on a G circle needs a shunt capacitor.
                Dim V1 As System.Double = -NormB
                Dim EffectiveY As New Admittance(0, V1)
                Dim EffectiveZ As Impedance = EffectiveY.ToImpedance
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.ShuntCap,
                    .Value1 = EffectiveZ.Reactance}
                }
                Return True
            Else
                ' CCW on a G circle needs a shunt inductor.
                Dim V1 As System.Double = -NormB
                Dim EffectiveY As New Admittance(0, V1)
                Dim EffectiveZ As Impedance = EffectiveY.ToImpedance
                transformations = {
                    New Transformation With {
                    .Style = TransformationStyles.ShuntInd,
                    .Value1 = EffectiveZ.Reactance}
                }
                Return True
            End If
        End If

    End Function ' NormGEquals1

    '''' <summary>
    '''' Z is INSIDE the R=Z0 circle.
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
        'Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0



        '
        '
        ' XXXXX WHAT NEXT? XXXXX
        ' Move CW or CCW on the G circle to reach the R=Z0 circle.
        ' Would there ever be a case for taking the long path?
        ' Maybe to favor high- or low-pass?
        '
        '

        Return False ' DEFAULT UNTIL IMPLEMENTED.
        'xxxx


    End Function ' InsideREqualsZ0
    'xxxx

    '''' <summary>
    '''' Z is INSIDE the R=Z0 circle.
    '''' </summary>
    '''' <param name="z0">xxxxxxxxxx</param>
    '''' <param name="transformations">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    Private Function InsideGEqualsY0(ByVal z0 As System.Double,
        ByRef transformations As Transformation()) _
        As System.Boolean

        'Dim NormR As System.Double = Me.Resistance / z0
        'Dim NormX As System.Double = Me.Reactance / z0
        'Dim Y0 As System.Double = 1.0 / z0
        'Dim Y As Admittance = Me.ToAdmittance()
        'Dim NormG As System.Double = Y.Conductance / Y0
        'Dim NormB As System.Double = Y.Susceptance / Y0

        '
        '
        ' XXXXX WHAT NEXT? XXXXX
        ' Move CW or CCW on the R circle to reach the G=Y0 circle.
        ' Would there ever be a case for taking the long path?
        ' Maybe to favor high- or low-pass?
        '
        '

        Return False ' DEFAULT UNTIL IMPLEMENTED.
        'xxxx




    End Function ' InsideGEqualsY0
    'xxxx




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
        ' chart.

        ' Chart location cases:
        ' A: At the short circuit point on the left. Omit; Covered by B.
        ' B: Anywhere else on the outer circle. R=0.0.
        ' C: At the open circuit point on the right.
        ' D: At the center.
        ' E: On the R=Z0 circle.
        '     Omit: On the resonance line. Already covered by C or D.
        '     E1: Above the resonance line. Only needs reactance.
        '     E2: Below the resonance line. Only needs reactance.
        ' F: Inside the R=Z0 circle. Two choices: CW or CCW on the G circle.
        ' G: On the G=Y0 circle.
        '     Omit: On the resonance line. Already either B or D.
        '     G1: Above the resonance line. Only needs reactance.
        '     G2: Below the resonance line. Only needs reactance.
        ' H: Inside the G=Y0 circle. Two choices: CW or CCW on the R circle.
        ' I: In the top remainder.
        ' J: In the bottom remainder.

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
            ' A: At the short circuit point on the left. Omit; Covered by B.
            ' B: Anywhere else on the outer circle. R=0.0
            ' C: At the open circuit point on the right.
            transformations = Nothing
            Return False
        End If

        If NormR.Equals(z0) Then
            ' Z is on perimeter of the R=Z0 circle.
            Return NormREqualsZ0(z0, transformations)
        ElseIf NormG.Equals(1.0) Then
            ' Z is on the perimeter of the G=Y0 circle.
            Return NormGEquals1(z0, transformations)
        ElseIf NormR > z0 Then
            ' Z is INSIDE the R=Z0 circle.

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            ' Move CW or CCW on the G circle to reach the R=Z0 circle.
            ' Would there ever be a case for taking the long path?
            ' Maybe to favor high- or low-pass?
            '
            '

            Return False ' DEFAULT UNTIL IMPLEMENTED.
            'xxxx
        ElseIf NormG > Y0 Then
            ' Z is INSIDE the G=Y0 circle.

            '
            '
            ' XXXXX WHAT NEXT? XXXXX
            ' Move CW or CCW on the R circle to reach the G=Y0 circle.
            ' Would there ever be a case for taking the long path?
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
                ' on the R or G circle or at the center.
                '
                '

                ' DELETE THIS AFTER TESTING CONFIRMS THAT IT IS NOT HIT BY ANY TEST CASES.
                Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
                Throw New ApplicationException(
                    """NormX.Equals(0.0)"" should never be matched in " &
                    NameOf(TrySelectTuningLayout))


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

    End Function ' TrySelectTuningLayout

    '' USE THIS TO TRY TO SET UP MATCHING A LOAD TO AN ARBITRARY SOURCE
    '' IMPEDANCE. IF IT WORKS OUT, MATCHING TO A CHARACTERISTIC IMPEDANCE COULD
    '' JUST BE A SPECIAL CASE.
    '''' <summary>
    '''' Attempts to obtain a conjugate match from the current load instance to
    '''' the source impedance specified by <paramref name="sourceR"/> and
    '''' <paramref name="sourceX"/>.
    '''' </summary>
    '''' <param name="z0">Specifies the characteristic impedance to which the
    '''' calculations should be referenced.</param>
    '''' <param name="sourceR">Specifies the resistance component of the source
    '''' impedance to which the current load instance should be matched.</param>
    '''' <param name="sourceX">Specifies the reactance component of the source
    '''' impedance to which the current load instance should be matched.</param>
    '''' <param name="transformations"></param>
    '''' <returns><c>True</c> if the xxxxxxxxxxxxxxx succeeds; otherwise,
    '''' <c>False</c> and also returns xxxxxxxxxxxxxxx xxxxxxxxxxxxxxx.</returns>
    'Public Function TrySelectTuningLayout(ByVal z0 As System.Double,
    '    ByVal sourceR As System.Double, ByVal sourceX As System.Double,
    '    ByRef transformations As Transformation()) _
    '    As System.Boolean

    '    ' The terminology here relates to solving conjugate matches on a Smith
    '    ' chart.

    '    ' Chart location cases:
    '    ' A: At the short circuit point on the left. Omit; Covered by B.
    '    ' B: Anywhere else on the outer circle. R=0.0.
    '    ' C: At the open circuit point on the right.
    '    ' D: At the center.
    '    ' E: On the R=Z0 circle.
    '    '     Omit: On the resonance line. Already covered by C or D.
    '    '     E1: Above the resonance line. Only needs reactance.
    '    '     E2: Below the resonance line. Only needs reactance.
    '    ' F: Inside the R=Z0 circle. Two choices: CW or CCW on the G circle.
    '    ' G: On the G=Y0 circle.
    '    '     Omit: On the resonance line. Already either B or D.
    '    '     G1: Above the resonance line. Only needs reactance.
    '    '     G2: Below the resonance line. Only needs reactance.
    '    ' H: Inside the G=Y0 circle. Two choices: CW or CCW on the R circle.
    '    ' I: In the top remainder.
    '    ' J: In the bottom remainder.

    '    ' A series inductor moves CW on a R circle.
    '    ' A series capacitor moves CCW on a R circle.
    '    ' A shunt inductor moves CCW on a G circle.
    '    ' A shunt capacitor moves CW on a G circle.
    '    ' A series resistor moves an impedance along the R circles. 
    '    ' A shunt resistor moves an impedance along the constant G circles.

    '    Dim TargetNormR As System.Double = sourceR / z0
    '    Dim TargetNormX As System.Double = sourceX / z0
    '    Dim TargetNormZ As New Impedance(TargetNormR, TargetNormX)
    '    Dim TargetNormY As Admittance = TargetNormZ.ToAdmittance()
    '    Dim TargetNormG As System.Double = TargetNormY.Conductance
    '    Dim TargetNormB As System.Double = TargetNormY.Susceptance

    '    Dim OwnNormR As System.Double = Me.Resistance / z0
    '    Dim OwnNormX As System.Double = Me.Reactance / z0




    '    Return False ' DEFAULT UNTIL IMPLEMENTED.
    'End Function ' TrySelectTuningLayout

    '' USE THIS TO TRY TO SET UP MATCHING TO AN ARBITRARY SOURCE IMPEDANCE.
    '''' <summary>
    '''' Attempts to obtain a conjugate match from the current load instance to
    '''' the source impedance specified by <paramref name="sourceZ"/>.
    '''' </summary>
    '''' <param name="z0">Specifies the characteristic impedance to which the
    '''' calculations should be referenced.</param>
    '''' <param name="sourceZ">Specifies the source impedance to which the
    '''' current load instance should be matched.</param>
    '''' <returns><c>True</c> if the xxxxxxxxxxxxxxx succeeds; otherwise,
    '''' <c>False</c> and also returns xxxxxxxxxxxxxxx xxxxxxxxxxxxxxx.</returns>
    'Public Function TrySelectTuningLayout(ByVal z0 As System.Double,
    '    ByVal sourceZ As Impedance, ByRef transformations As Transformation()) _
    '    As System.Boolean

    '    Return TrySelectTuningLayout(z0, sourceZ.Resistance, sourceZ.Reactance, transformations)
    'End Function ' TrySelectTuningLayout

End Structure ' Impedance
