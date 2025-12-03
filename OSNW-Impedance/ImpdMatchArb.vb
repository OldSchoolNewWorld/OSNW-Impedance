Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

' This document contains items related to matching a load impedance to an
' arbitrary source impedance.

Partial Public Structure Impedance

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    ''' the specified <c>SmithMainCircle</c>.
    ''' This method attempts to find a match by first moving, on a G-circle,
    ''' from the load impedance to an image impedance at a specified Cartesian
    ''' coordinate, then moving, on an R-circle, from the image impedance to the
    ''' source impedance.
    ''' </summary>
    ''' <param name="mainCirc">Specifies a <c>SmithMainCircle</c> in which the
    ''' match is to be made.</param>
    ''' <param name="oneIntersection">Specifies the Cartesian coordinate of the
    ''' image impedance.</param>
    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    ''' <paramref name="loadZ"/> should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load
    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    Public Shared Function MatchArbFirstOnG(ByVal mainCirc As SmithMainCircle,
        ByVal oneIntersection As OSNW.Numerics.PointD,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' Find out about the intersection/image impedance.
        Dim ImagePD As PlotDetails =
            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)

        Dim CurrTransCount As System.Int32 = transformations.Length
        Dim Trans As New Transformation

        ' The intended process is to create an L-section. The first move is on
        ' the LoadG-circle, from the load impedance to the image impedance
        ' and the second move is on the SourceR-circle, from the image
        ' impedance to the source impedance.
        ' There are two special cases to consider: 1) If the load impedance is
        ' already on the SourceR circle, only the SourceR-circle move is needed,
        ' and 2) If the load impedance is already on the SourceG-circle, only
        ' the SourceG-circle move is needed.

        ' If the load is already on the SourceR-circle,xxxxxxxxxxxx no transformation is
        ' needed to get there. That happens when xxxxxxxxxLoadR is already at SourceR.

        Dim DeltaX As System.Double
        If Impedance.EqualEnough(mainCirc.Z0, loadZ, ImagePD.Impedance) Then

            ' Move only on the SourceR-circle.
            DeltaX = sourceZ.Reactance - loadZ.Reactance
            With Trans
                If DeltaX < 0.0 Then
                    ' CCW on an R-circle needs a series capacitor.
                    .Style = TransformationStyles.SeriesCap
                Else
                    ' CW on an R-circle needs a series inductor.
                    .Style = TransformationStyles.SeriesInd
                End If
                .Value1 = DeltaX
            End With

            ' Add to the array of transformations.
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans
            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}, Value1={Trans.Value1}, Value2={Trans.Value2}")
            'xxxxxxxxxxxxxx
            Return True

        End If

        '' If the load impedance is already on the SourceG-circle,xxxxxxxxxxx only the
        '' SourceG-circle move is needed. That happens when xxxxxxxxxxxxxxxLoadG is already at
        '' SourceG.
        'Dim LoadG As System.Double = loadZ.ToAdmittance.Conductance
        'Dim SourceG As System.Double = sourceZ.ToAdmittance.Conductance
        'Dim DeltaB As System.Double
        'If LoadG = SourceG Then

        '    ' No movement on R-circle needed. Move only on the SourceG-circle.
        '    DeltaB = ImagePD.Admittance.Susceptance -
        '        loadZ.ToAdmittance().Susceptance
        '    With Trans
        '        If DeltaB < 0.0 Then
        '            ' CCW on a G-circle needs a shunt inductor.
        '            .Style = TransformationStyles.ShuntInd
        '        Else ' Deltab > 0.0
        '            ' CW on a G-circle needs a shunt capacitor.
        '            .Style = TransformationStyles.ShuntCap
        '        End If
        '        Dim DeltaY As New Admittance(0, DeltaB)
        '        .Value1 = DeltaY.ToImpedance.Reactance
        '    End With

        '    ' Add to the array of transformations.
        '    ReDim Preserve transformations(CurrTransCount)
        '    transformations(CurrTransCount) = Trans
        '    'xxxxxxxxxxxxxx
        '    System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}, Value1={Trans.Value1}, Value2={Trans.Value2}")
        '    'xxxxxxxxxxxxxx
        '    Return True

        'End If

        ' On getting this far,
        ' Move on the LoadG-circle first, to the image point, then on the
        ' SourceR-circle to the source.
        Dim DeltaB As System.Double
        With Trans
            DeltaB = ImagePD.Admittance.Susceptance -
                loadZ.ToAdmittance().Susceptance
            DeltaX = sourceZ.Reactance - ImagePD.Impedance.Reactance
            If DeltaB < 0.0 Then
                ' CCW on a G-circle needs a shunt inductor.
                If DeltaX < 0.0 Then
                    ' CCW on a R-circle needs a series capacitor.
                    .Style = TransformationStyles.ShuntIndSeriesCap
                Else ' DeltaX > 0.0
                    ' CW on a R-circle needs a series inductor.
                    .Style = TransformationStyles.ShuntIndSeriesInd
                End If
            Else ' Deltab > 0.0
                ' CW on a G-circle needs a shunt capacitor.
                If DeltaX < 0.0 Then
                    ' CCW on a R-circle needs a series capacitor.
                    .Style = TransformationStyles.ShuntCapSeriesCap
                Else ' DeltaX > 0.0
                    ' CW on a R-circle needs a series inductor.
                    .Style = TransformationStyles.ShuntCapSeriesInd
                End If
            End If
            Dim DeltaY As New Admittance(0, DeltaB)
            .Value1 = DeltaY.ToImpedance.Reactance
            .Value2 = DeltaX
        End With

        ' On getting this far,
        ' Add to the array of transformations.
        ReDim Preserve transformations(CurrTransCount)
        transformations(CurrTransCount) = Trans
        'xxxxxxxxxxxxxx
        System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}, Value1={Trans.Value1}, Value2={Trans.Value2}")
        'xxxxxxxxxxxxxx
        Return True

    End Function ' MatchArbFirstOnG

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    ''' the specified <c>SmithMainCircle</c>.
    ''' This method attempts to find a match by first moving, on an R-circle,
    ''' from the load impedance to an image impedance at a specified Cartesian
    ''' coordinate, then moving, on a G-circle, from the image impedance to the
    ''' source impedance.
    ''' </summary>
    ''' <param name="mainCirc">Specifies a <c>SmithMainCircle</c> in which the
    ''' match is to be made.</param>
    ''' <param name="oneIntersection">Specifies the Cartesian coordinate of the
    ''' image impedance.</param>
    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    ''' <paramref name="loadZ"/> should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load
    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    Public Shared Function MatchArbFirstOnR(ByVal mainCirc As SmithMainCircle,
        ByVal oneIntersection As OSNW.Numerics.PointD,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' Find out about the intersection/image impedance.
        Dim ImagePD As PlotDetails =
            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)

        Dim CurrTransCount As System.Int32 = transformations.Length
        Dim Trans As New Transformation

        ' The intended process is to create an L-section. The first move is on
        ' the LoadR-circle, from the load impedance to the image impedance and
        ' the second move is on the SourceG-circle, from the image impedance to
        ' the source impedance.
        ' There are two special cases to consider: 1) If the load impedance is
        ' already on the SourceG-circle, only the SourceG-circle move is needed,
        ' and 2) If the load impedance is already on the SourceR-circle, only
        ' the SourceR-circle move is needed.

        ' If the load is already on the SourceG-circle, no transformation is
        ' needed to get there. That happens when LoadG is already at SourceG.
        Dim LoadG As System.Double = loadZ.ToAdmittance.Conductance
        Dim SourceG As System.Double = sourceZ.ToAdmittance.Conductance
        Dim DeltaB As System.Double
        Dim DeltaX As System.Double
        If Impedance.EqualEnough(LoadG, SourceG, IMPDTOLERANCE) Then

            ' Move only on the SourceG-circle.
            DeltaB = sourceZ.ToAdmittance.Susceptance -
                    loadZ.ToAdmittance.Susceptance
            With Trans
                If DeltaB < 0.0 Then
                    ' CW on a G-circle needs a shunt capacitor.
                    .Style = TransformationStyles.ShuntCap
                Else
                    ' CCW on a G-circle needs a shunt inductor.
                    .Style = TransformationStyles.ShuntInd
                End If
                Dim DeltaY As New Admittance(0.0, DeltaB)
                DeltaX = DeltaY.ToImpedance.Reactance
                .Value1 = DeltaX
            End With

            ' Add to the array of transformations.
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans
            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}, Value1={Trans.Value1}, Value2={Trans.Value2}")
            'xxxxxxxxxxxxxx
            Return True

        End If

        ' If the load impedance is already on the SourceR-circle, only the
        ' SourceR-circle move is needed. That happens when LoadR is already at
        ' SourceR.
        Dim LoadR As System.Double = loadZ.Resistance
        Dim SourceR As System.Double = sourceZ.Resistance
        If LoadR = SourceR Then

            ' No movement on G-circle needed.
            Dim LoadX As System.Double = loadZ.Reactance
            Dim SourceX As System.Double = sourceZ.Reactance
            DeltaX = SourceX - LoadX
            With Trans
                If DeltaX < 0.0 Then
                    ' CCW on a R-circle needs a series capacitor.
                    .Style = TransformationStyles.SeriesCap
                Else ' DeltaX > 0.0
                    ' CW on an R-circle needs a series inductor.
                    .Style = TransformationStyles.SeriesInd
                End If
                .Value1 = DeltaX
            End With

            ' Add to the array of transformations.
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans
            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}, Value1={Trans.Value1}, Value2={Trans.Value2}")
            'xxxxxxxxxxxxxx
            Return True

        End If

        ' On getting this far,
        ' Move on the LoadR-circle first, to the image point, then on the
        ' SourceG-circle to the source.
        DeltaX = ImagePD.Impedance.Reactance - loadZ.Reactance
        DeltaB = sourceZ.ToAdmittance().Susceptance -
            ImagePD.Admittance.Susceptance
        With Trans
            If DeltaX < 0.0 Then
                ' CCW on a R-circle needs a series capacitor.
                If DeltaB < 0.0 Then
                    ' CCW on a G-circle needs a shunt inductor.
                    .Style = TransformationStyles.SeriesCapShuntInd
                Else ' DeltaB > 0.0
                    ' CW on a G-circle needs a shunt capacitor.
                    .Style = TransformationStyles.SeriesCapShuntCap
                End If
            Else ' DeltaX > 0.0
                ' CW on a R-circle needs a series inductor.
                If DeltaB < 0.0 Then
                    ' CCW on a G-circle needs a shunt inductor.
                    .Style = TransformationStyles.SeriesIndShuntInd
                Else ' DeltaB > 0.0
                    ' CW on a G-circle needs a shunt capacitor.
                    .Style = TransformationStyles.SeriesIndShuntCap
                End If
            End If
            .Value1 = DeltaX
            .Value2 = New Admittance(0, DeltaB).ToImpedance().Reactance
        End With

        ' Add to the array of transformations.
        ReDim Preserve transformations(CurrTransCount)
        transformations(CurrTransCount) = Trans
        'xxxxxxxxxxxxxx
        System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}, Value1={Trans.Value1}, Value2={Trans.Value2}")
        'xxxxxxxxxxxxxx
        Return True

    End Function ' MatchArbFirstOnR



    ' THIS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.

    '<InlineData(  Z0,        R,         X)> ' Model

    '<InlineData(1, 1.0, 1.0, 0.5, 0.2)> ' 
    '<InlineData(1, 0.5, 0.2, 1.0, 1.0)> ' 
    '<InlineData(100.0, 100.0, 100.0, 50.0, 20.0)> ' 
    '<InlineData(100.0, 50.0, 20.0, 100.0, 100.0)> ' 
    '<InlineData(1, 1.0, 1.0, 2.0, -2.0)> ' 
    '<InlineData(1, 2.0, -2.0, 1.0, 1.0)> ' 
    '<InlineData(75, 50.0, 50.0, 100.0, -100.0)> ' 
    '<InlineData(75, 100.0, -100.0, 50.0, 50.0)> ' 
    '<InlineData(1, 1 / 3.0, 1 / 3.0, 1 / 3.0, 0.0000)> ' 
    '<InlineData(1, 1 / 3.0, 0.0000, 1 / 3.0, 1 / 3.0)> ' 
    '<InlineData(75, 25.0, 25.0, 25.0, 0.0000)> ' 
    '<InlineData(75, 25.0, 0.0000, 25.0, 25.0)> ' 
    '<InlineData(1.0, 0.2, 1.4, 0.4, -0.8)> ' 
    '<InlineData(1.0, 0.4, -0.8, 0.2, 1.4)> ' 
    '<InlineData(50.0, 10.0, 70.0, 20.0, -40.0)> ' 
    '<InlineData(50.0, 20.0, -40.0, 10.0, 70.0)> ' 
    '<InlineData(1.0, 1.0, 1.0, 1 / 2.0, 1 / 2.0)> ' 
    '<InlineData(1.0, 1 / 2.0, 1 / 2.0, 1.0, 1.0)> ' 
    '<InlineData(50.0, 50.0, 50.0, 25.0, 25.0)> ' 
    '<InlineData(50.0, 25.0, 25.0, 50.0, 50.0)> ' 

    '<InlineData( 1.0,   0.0000,    0.0000)> ' A: At the short circuit point. Omit - covered by B.
    '<InlineData( 1.0,   0.0000,     1/2.0)> ' B: Anywhere else on the perimeter. R=0.0.
    '<InlineData( 1.0,      INF,    0.0000)> ' C: At the open circuit point on the right.
    '<InlineData( 1.0,   1.0000,    0.0000)> ' D1: At the center.
    '<InlineData(75.0,  75.0000,    0.0000)> ' D75: At the center. Z0=75.
    '<InlineData( 1.0,   1.0000,    1.0000)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
    '<InlineData(50.0,  50.0000,   50.0000)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
    '<InlineData( 1.0,   1.0000,   -2.0000)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
    '<InlineData(50.0,  50.0000, -100.0000)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
    '<InlineData( 1.0,   2.0000,     1/2.0)> ' G1: Inside R=Z0 circle, above resonance line.
    '<InlineData(50.0, 100.0000,   25.0000)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
    '<InlineData( 1.0,   3.0000,    0.0000)> ' H1: Inside R=Z0 circle, on line.
    '<InlineData(50.0, 150.0000,    0.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
    '<InlineData( 1.0,   2.0000,   -2.0000)> ' I1: Inside R=Z0 circle, below resonance line.
    '<InlineData(50.0, 100.0000, -100.0000)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
    '<InlineData( 1.0,    1/2.0,     1/2.0)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
    '<InlineData(50.0,  25.0000,   25.0000)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
    '<InlineData( 1.0,    1/2.0,    -1/2.0)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
    '<InlineData(50.0,  25.0000,  -25.0000)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
    '<InlineData( 1.0,    1/3.0,     1/3.0)> ' L1: Inside G=Y0 circle, above resonance line.
    '<InlineData(75.0,  25.0000,   25.0000)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
    '<InlineData( 1.0,    1/3.0,    0.0000)> ' M1: Inside G=Y0 circle, on line.
    '<InlineData(75.0,  25.0000,    0.0000)> ' M75: Inside G=Y0 circle, on line. Z0=75.
    '<InlineData( 1.0,    1/2.0,    -1/3.0)> ' N1: Inside G=Y0 circle, below line.
    '<InlineData(75.0,  37.5000,  -25.0000)> ' N75: Inside G=Y0 circle, below line. Z0=75.
    '<InlineData( 1.0,   0.2000,    1.4000)> ' O1: In the top center.
    '<InlineData(50.0,  10.0000,   70.0000)> ' O50: In the top center. Z0=50.
    '<InlineData( 1.0,   0.4000,   -0.8000)> ' P1: In the bottom center.
    '<InlineData(50.0,  20.0000,  -40.0000)> ' P50: In the bottom center. Z0=50.
    '<InlineData( 1.0,  -0.0345,    0.4138)> ' Q: Outside of main circle. Invalid.
    '<InlineData( 1.0,  -2.0000,       999)> ' R: NormR<=0. Invalid.

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    ''' the specified <c>SmithMainCircle</c>.
    ''' </summary>
    ''' <param name="mainCirc">Specifies a <c>SmithMainCircle</c> in which the
    ''' match is to be made.</param>
    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    ''' <paramref name="loadZ"/> should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load
    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' A succcessful process might result in no transformation being done.
    ''' </remarks>
    Public Shared Function MatchArbitrary(
        ByVal mainCirc As SmithMainCircle,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' REF: Smith Chart Full Presentation, page 26 shows a geometric
        ' approach to finding a match.
        ' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

        'xxxxxxxxxxxxxx
        System.Diagnostics.Debug.WriteLine($"Z0:{mainCirc.Z0}; {loadZ} to {sourceZ}:")
        'xxxxxxxxxxxxxx

        ' Input checking.
        ' Leave this here, at least for now. Bad values should have been
        ' rejected in New(Double, Double), and this should not be needed unless
        ' some reason is discovered that requires extremes to be allowed. Maybe
        ' that could happen in a situation involving active components that can
        ' effectively have a negative resitance value.
        ' Check for a short- or open-circuit or for invalid resistances.
        Dim LoadR As System.Double = loadZ.Resistance
        Dim SourceR As System.Double = sourceZ.Resistance
        If LoadR <= 0.0 OrElse System.Double.IsInfinity(LoadR) OrElse
            SourceR <= 0.0 OrElse System.Double.IsInfinity(SourceR) Then

            ' Leave transformations as is.
            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"  Invalid input.")
            'xxxxxxxxxxxxxx
            Return False
        End If

        ' Check whether a match is needed.
        If Impedance.EqualEnough(mainCirc.Z0, loadZ, sourceZ) Then
            ' Not needed. Add the inaction to the array of transformations.
            Dim Trans As New Transformation With
                {.Style = TransformationStyles.None}
            Dim CurrTransCount As System.Int32 = transformations.Length
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans
            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}, Value1={Trans.Value1}, Value2={Trans.Value2}")
            'xxxxxxxxxxxxxx
            Return True
        End If

        ' Only if the relevant circles intersect, try each geometric approach to
        ' finding a match.
        Dim Intersections _
            As New System.Collections.Generic.List(Of OSNW.Numerics.PointD)

        ' Try first on a G-circle, then on an R-circle.
        Dim LoadCircG As New GCircle(mainCirc, loadZ.ToAdmittance().Conductance)
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        If GenericCircle.CirclesIntersect(
            LoadCircG, SourceCircR, Intersections) Then

            ' The circles intersect. That is not useful at the perimeter.
            If Intersections.Count.Equals(1) AndAlso
             Impedance.EqualEnough(Intersections(0).X,
                                   mainCirc.GridLeftEdgeX, IMPDTOLERANCE) Then

                ' They intersect at the perimeter. No update to transformations.
                Return True
            End If

            ' There are now either one or two intersection points. With one, the
            ' circles are tangent at a point on the resonance line. With two,
            ' there is one above, and one below, the resonance line; the X
            ' values match; the Y values are the same distance above and below
            ' the resonance line.

            For Each OneIntersection As OSNW.Numerics.PointD In Intersections
                If Not MatchArbFirstOnG(mainCirc, OneIntersection,
                                        loadZ, sourceZ, transformations) Then

                    Return False
                End If
            Next

        End If

        ' Try first on an R-circle, then on a G-circle.
        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim SourceCircG As New GCircle(mainCirc,
                                       sourceZ.ToAdmittance().Conductance)
        If GenericCircle.CirclesIntersect(
            LoadCircR, SourceCircG, Intersections) Then

            ' The circles intersect. That is not useful at the perimeter.
            If Intersections.Count.Equals(1) AndAlso
                Impedance.EqualEnough(Intersections(0).X,
                    mainCirc.GridRightEdgeX, IMPDTOLERANCE) Then

                ' They intersect at the perimeter. No update to transformations.
                Return True
            End If

            ' There are now either one or two intersection points. With one, the
            ' circles are tangent at a point on the resonance line. With two,
            ' there is one above, and one below, the resonance line; the X
            ' values match; the Y values are the same distance above and below
            ' the resonance line.

            For Each OneIntersection As OSNW.Numerics.PointD In Intersections

                If Not MatchArbFirstOnR(mainCirc, OneIntersection,
                                        loadZ, sourceZ, transformations) Then

                    Return False
                End If

                ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
                For Each OneTrans As Transformation In transformations
                    If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
                        Return False
                    End If
                Next

            Next

        End If

        ' On getting this far,
        ' THIS CALL CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
        Return True

    End Function ' MatchArbitrary

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c> in a
    ''' system having the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance of the
    ''' system.</param>
    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    ''' <paramref name="loadZ"/> should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load
    ''' <c>Impedance</c> to a source <c>Impedance</c>.</param>
    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="transformations"/>, the components to construct the
    ''' match.</returns>
    ''' <remarks>
    ''' <para> An assumption is made that the calling code has determined that
    ''' the <c>Impedance</c>s lie in valid positions. Failure to meet that
    ''' assumption could result in invalid, or incomplete, results.</para>
    ''' <paramref name="z0"/> is the characteristic impedance for the system in
    ''' which the <c>Impedance</c>s should be matched. It should have a
    ''' practical value with regard to the impedance values involved. A
    ''' succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Public Shared Function MatchArbitrary(z0 As System.Double,
        loadZ As Impedance, sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        'xxxxxxxxxxxxxx
        System.Diagnostics.Debug.WriteLine(String.Empty)
        'xxxxxxxxxxxxxx

        ' Input checking.
        If z0 <= 0.0 Then
            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"Z0:{z0}; {loadZ} to {sourceZ}:")
            System.Diagnostics.Debug.WriteLine("Z0 is negative")
            'xxxxxxxxxxxxxx
            Return False
        End If

        ' Create a SmithMainCircle for the specified Z0 and pass it to the
        ' geometric worker.
        ' Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        Return MatchArbitrary(MainCirc, loadZ, sourceZ, transformations)

    End Function ' MatchArbitrary

End Structure ' Impedance
