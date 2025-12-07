Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off
Imports System.Net.Mime.MediaTypeNames



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

        ' If the load susceptance already matches the image susceptance, no
        ' transformation is needed to get to the image impedance.
        Dim ImageB As System.Double = ImagePD.Susceptance
        Dim LoadB As System.Double = loadZ.ToAdmittance.Susceptance
        Dim DeltaX As System.Double
        If EqualEnoughZero(ImageB - LoadB, IMPDTOLERANCE0 * mainCirc.Z0) Then

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
            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
                $" Value1={Trans.Value1}, Value2={Trans.Value2}")
            'xxxxxxxxxxxxxx
            Return True

        End If

        ' On getting this far,
        ' Move on the LoadG-circle first, to the image point, then on the
        ' SourceR-circle to the source.
        Dim DeltaB As System.Double = ImagePD.Susceptance -
            loadZ.ToAdmittance().Susceptance
        DeltaX = sourceZ.Reactance - ImagePD.Reactance
        With Trans
            If DeltaB < 0.0 Then
                ' CCW on a G-circle needs a shunt inductor.
                If DeltaX < 0.0 Then
                    ' CCW on a R-circle needs a series capacitor.
                    .Style = TransformationStyles.ShuntIndSeriesCap
                ElseIf DeltaX > 0.0 Then
                    ' CW on a R-circle needs a series inductor.
                    .Style = TransformationStyles.ShuntIndSeriesInd
                Else ' DeltaX = 0.0
                    .Style = TransformationStyles.ShuntInd
                End If
            Else ' DeltaB > 0.0
                ' CW on a G-circle needs a shunt capacitor.
                If DeltaX < 0.0 Then
                    ' CCW on a R-circle needs a series capacitor.
                    .Style = TransformationStyles.ShuntCapSeriesCap
                ElseIf DeltaX > 0.0 Then
                    ' CW on a R-circle needs a series inductor.
                    .Style = TransformationStyles.ShuntCapSeriesInd
                Else ' DeltaX = 0.0
                    .Style = TransformationStyles.ShuntCap
                End If
            End If
            .Value1 = New Admittance(0, DeltaB).ToImpedance.Reactance
            .Value2 = DeltaX
        End With

        ' Add to the array of transformations.
        ReDim Preserve transformations(CurrTransCount)
        transformations(CurrTransCount) = Trans
        'xxxxxxxxxxxxxx
        System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
            $" Value1={Trans.Value1}, Value2={Trans.Value2}")
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

        ' If the load reactance already matches the image reactance, no
        ' transformation is needed to get to the image impedance.
        Dim ImageX As System.Double = ImagePD.Reactance
        Dim LoadX As System.Double = loadZ.Reactance
        Dim DeltaB As System.Double
        Dim DeltaX As System.Double
        If EqualEnoughZero(ImageX - LoadX, IMPDTOLERANCE0 * mainCirc.Z0) Then

            ' Move only on the SourceG-circle.
            DeltaB = sourceZ.ToAdmittance.Susceptance -
                loadZ.ToAdmittance.Susceptance
            With Trans
                If DeltaB < 0.0 Then
                    ' CCW on a G-circle needs a shunt inductor.
                    .Style = TransformationStyles.ShuntInd
                Else
                    ' CW on a G-circle needs a shunt capacitor.
                    .Style = TransformationStyles.ShuntCap
                End If
                DeltaX = New Admittance(0.0, DeltaB).ToImpedance.Reactance
                .Value1 = DeltaX
            End With

            ' Add to the array of transformations.
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans
            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
                    $" Value1={Trans.Value1}, Value2={Trans.Value2}")
            'xxxxxxxxxxxxxx
            Return True

        End If

        ' On getting this far,
        ' Move on the LoadR-circle first, to the image point, then on the
        ' SourceG-circle to the source.
        DeltaX = ImagePD.Reactance - loadZ.Reactance
        DeltaB = sourceZ.ToAdmittance().Susceptance - ImagePD.Susceptance
        With Trans
            If DeltaX < 0.0 Then
                ' CCW on a R-circle needs a series capacitor.
                If DeltaB < 0.0 Then
                    ' CCW on a G-circle needs a shunt inductor.
                    .Style = TransformationStyles.SeriesCapShuntInd
                ElseIf DeltaB > 0.0 Then
                    ' CW on a G-circle needs a shunt capacitor.
                    .Style = TransformationStyles.SeriesCapShuntCap
                Else ' DeltaB = 0.0
                    .Style = TransformationStyles.SeriesCap
                End If
            Else ' DeltaX > 0.0
                ' CW on a R-circle needs a series inductor.
                If DeltaB < 0.0 Then
                    ' CCW on a G-circle needs a shunt inductor.
                    .Style = TransformationStyles.SeriesIndShuntInd
                ElseIf DeltaB > 0.0 Then
                    ' CW on a G-circle needs a shunt capacitor.
                    .Style = TransformationStyles.SeriesIndShuntCap
                Else ' DeltaB = 0.0
                    .Style = TransformationStyles.SeriesInd
                End If
            End If
            .Value1 = DeltaX
            .Value2 = New Admittance(0, DeltaB).ToImpedance().Reactance
        End With

        ' Add to the array of transformations.
        ReDim Preserve transformations(CurrTransCount)
        transformations(CurrTransCount) = Trans
        'xxxxxxxxxxxxxx
        System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
            $" Value1={Trans.Value1}, Value2={Trans.Value2}")
        'xxxxxxxxxxxxxx
        Return True

    End Function ' MatchArbFirstOnR

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
            System.Diagnostics.Debug.WriteLine($"  Style={Trans.Style}," &
                $" Value1={Trans.Value1}, Value2={Trans.Value2}")
            'xxxxxxxxxxxxxx
            Return True
        End If

        ' Try each geometric approach to finding a match.
        '        Dim Intersections _
        '            As New System.Collections.Generic.List(Of OSNW.Numerics.PointD)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD)

        ' Try first on a G-circle, then on an R-circle.
        Dim LoadCircG As New GCircle(mainCirc, loadZ.ToAdmittance().Conductance)
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        Intersections = LoadCircG.GetIntersections(SourceCircR)

        ' There are now either one or two intersection points. With one, the
        ' circles are tangent at a point on the resonance line. With two, there
        ' is one above, and one below, the resonance line; the X values match;
        ' the Y values are the same distance above and below the resonance line.

        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

            If Not MatchArbFirstOnG(mainCirc, OneIntersection, loadZ, sourceZ,
                                    transformations) Then

                Return False
            End If

            ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            For Each OneTrans As Transformation In transformations
                If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
                    Return False
                End If
            Next

        Next

        ' Try first on an R-circle, then on a G-circle.
        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim SourceCircG As New GCircle(
            mainCirc, sourceZ.ToAdmittance().Conductance)
        Intersections = LoadCircR.GetIntersections(SourceCircG)

        ' There are now either one or two intersection points. With one, the
        ' circles are tangent at a point on the resonance line. With two, there
        ' is one above, and one below, the resonance line; the X values match;
        ' the Y values are the same distance above and below the resonance line.

        For Each OneIntersection As OSNW.Numerics.PointD In Intersections

            If Not MatchArbFirstOnR(mainCirc, OneIntersection, loadZ, sourceZ,
                                    transformations) Then

                Return False
            End If

            ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
            For Each OneTrans As Transformation In transformations
                If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
                    Return False
                End If
            Next

        Next

        ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
        For Each OneTrans As Transformation In transformations
            If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
                Return False
            End If
        Next

        ' On getting this far,
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
