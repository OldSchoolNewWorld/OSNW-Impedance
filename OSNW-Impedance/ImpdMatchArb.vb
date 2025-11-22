Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

' This document contains items related to matching a load impedance to an
' arbitrary source impedance.

Partial Public Structure Impedance

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCirc">xxxxxxxxxx</param>
    ''' <param name="oneIntersection">xxxxxxxxxx</param>
    ''' <param name="loadZ">xxxxxxxxxx</param>
    ''' <param name="sourceZ">xxxxxxxxxx</param>
    ''' <param name="transformations">xxxxxxxxxx</param>
    ''' <returns></returns>
    Public Shared Function MatchArbFirstOnG(ByVal mainCirc As SmithMainCircle,
        ByVal oneIntersection As OSNW.Numerics.PointD,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        Dim PD As PlotDetails =
            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)

        ' Set up the transformation.
        Dim Trans As New Transformation
        With Trans
            Dim ImageB As System.Double = PD.Admittance.Susceptance
            Dim LoadB As System.Double = loadZ.ToAdmittance().Susceptance
            Dim DeltaB As System.Double = ImageB - LoadB
            Dim ImageX As System.Double = PD.Impedance.Reactance
            Dim DeltaX As System.Double = sourceZ.Reactance - ImageX
            If DeltaB < 0.0 Then
                ' CCW on a G-circle needs a shunt inductor.
                If DeltaX < 0.0 Then
                    ' CCW on a R-circle needs a series capacitor.
                    .Style = TransformationStyles.ShuntIndSeriesCap
                Else
                    ' CW on a R-circle needs a series inductor.
                    .Style = TransformationStyles.ShuntIndSeriesInd
                End If
            Else
                ' CW on a G-circle needs a shunt capacitor.
                If DeltaX < 0.0 Then
                    ' CCW on a R-circle needs a series capacitor.
                    .Style = TransformationStyles.ShuntCapSeriesCap
                Else
                    ' CW on a R-circle needs a series inductor.
                    .Style = TransformationStyles.ShuntCapSeriesInd
                End If
            End If
            Dim DeltaY As New Admittance(0, DeltaB)
            .Value1 = DeltaY.ToImpedance.Reactance
            .Value2 = DeltaX
        End With

        ' Add to the array of transformations.
        Dim CurrTransCount As System.Int32 = transformations.Length
        ReDim Preserve transformations(CurrTransCount)
        transformations(CurrTransCount) = Trans

        ' On getting this far,
        Return True

    End Function ' MatchArbFirstOnG

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCirc">xxxxxxxxxx</param>
    ''' <param name="loadZ">xxxxxxxxxx</param>
    ''' <param name="sourceZ">xxxxxxxxxx</param>
    ''' <param name="transformations">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function MatchArbFirstOnR(ByVal mainCirc As SmithMainCircle,
        ByVal oneIntersection As OSNW.Numerics.PointD,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        Dim PD As PlotDetails =
            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)

        ' Set up the transformation.
        Dim Trans As New Transformation
        With Trans
            Dim ImageX As System.Double = PD.Impedance.Reactance
            Dim LoadX As System.Double = loadZ.Reactance
            Dim DeltaX As System.Double = ImageX - LoadX
            Dim SourceB As System.Double = sourceZ.ToAdmittance().Susceptance
            Dim ImageB As System.Double = PD.Admittance.Susceptance
            Dim DeltaB As System.Double = SourceB - ImageB
            If DeltaX < 0.0 Then
                ' CCW on a R-circle needs a series capacitor
                If DeltaB < 0.0 Then
                    ' CCW on a G-circle needs a shunt inductor
                    .Style = TransformationStyles.SeriesCapShuntInd
                Else
                    ' CW on a G-circle needs a shunt capacitor
                    .Style = TransformationStyles.SeriesCapShuntCap
                End If
            Else
                ' CW on a R-circle needs a series inductor
                If DeltaB < 0.0 Then
                    ' CCW on a G-circle needs a shunt inductor
                    .Style = TransformationStyles.SeriesIndShuntInd
                Else
                    ' CW on a G-circle needs a shunt capacitor
                    .Style = TransformationStyles.SeriesIndShuntCap
                End If
            End If
            .Value1 = DeltaX
            Dim DeltaY As New Admittance(0, DeltaB)
            Dim DeltaZ As Impedance = DeltaY.ToImpedance
            .Value2 = DeltaZ.Reactance
        End With

        ' Add to the array of transformations.
        Dim CurrTransCount As System.Int32 = transformations.Length
        ReDim Preserve transformations(CurrTransCount)
        transformations(CurrTransCount) = Trans

        ' On getting this far,
        Return True

    End Function ' MatchArbFirstOnR

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCirc">xxxxxxxxxx</param>
    ''' <param name="loadZ">xxxxxxxxxx</param>
    ''' <param name="sourceZ">xxxxxxxxxx</param>
    ''' <param name="transformations">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function MatchArbFirstOnG(ByVal mainCirc As SmithMainCircle,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' The circles are assumed intersect (from MatchArbitrary(
        ' SmithMainCircle, Impedance, Impedance, Transformation())). That is not
        ' useful at the perimeter.

        ' Determine the circle intersection(s).
        Dim LoadCircG As New GCircle(mainCirc, loadZ.ToAdmittance().Conductance)
        Dim SourceCircR As New RCircle(mainCirc, sourceZ.Resistance)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                   GenericCircle.GetIntersections(LoadCircG, SourceCircR)

        If Intersections.Count.Equals(1) AndAlso
             Impedance.EqualEnough(Intersections(0).X,
                                   mainCirc.GridLeftEdgeX) Then

            ' They intersect at the perimeter.
            ' No update to transformations.
            Return True
        End If

        ' There are now either one or two intersection points. With one, the two
        ' circles are tangent at a point on the resonance line. With two: there
        ' is one above, and one below, the resonance line; the X values match;
        ' the Y values are the same distance above and below the resonance line.

        For Each OneIntersection As OSNW.Numerics.PointD In Intersections
            If Not MatchArbFirstOnG(mainCirc, OneIntersection,
                                    loadZ, sourceZ, transformations) Then

                Return False
            End If
        Next

        ' On getting this far,
        Return True

    End Function ' MatchArbFirstOnG

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCirc">xxxxxxxxxx</param>
    ''' <param name="loadZ">xxxxxxxxxx</param>
    ''' <param name="sourceZ">xxxxxxxxxx</param>
    ''' <param name="transformations">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function MatchArbFirstOnR(ByVal mainCirc As SmithMainCircle,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' The circles are assumed intersect (from MatchArbitrary(
        ' SmithMainCircle, Impedance, Impedance, Transformation())). That is not
        ' useful at the perimeter.

        ' Determine the circle intersection(s).
        Dim LoadCircR As New RCircle(mainCirc, loadZ.Resistance)
        Dim SourceCircG As New GCircle(mainCirc,
                                       sourceZ.ToAdmittance().Conductance)
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                   GenericCircle.GetIntersections(LoadCircR, SourceCircG)

        If Intersections.Count.Equals(1) AndAlso
             Impedance.EqualEnough(Intersections(0).X,
                                   mainCirc.GridRightEdgeX) Then

            ' They intersect at the perimeter.
            ' No update to transformations.
            Return True
        End If

        ' There are now either one or two intersection points. With one, the two
        ' circles are tangent at a point on the resonance line. With two: there
        ' is one above, and one below, the resonance line; the X values match;
        ' the Y values are the same distance above and below the resonance line.

        For Each OneIntersection As OSNW.Numerics.PointD In Intersections
            If Not MatchArbFirstOnR(mainCirc, OneIntersection,
                                    loadZ, sourceZ, transformations) Then

                Return False
            End If
        Next

        ' On getting this far,
        Return True

    End Function ' MatchArbFirstOnR

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    ''' the specified <c>SmithmainCircle</c>.
    ''' </summary>
    ''' <param name="mainCirc">Specifies a <c>SmithMainCircle</c> in which the
    ''' match is to be made.</param>
    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    ''' <paramref name="loadZ"/> should be matched.</param>
    ''' <param name="transformations">Specifies an array of
    ''' <see cref="Transformation"/>s that can be used to match a load
    ''' <c>Impedance</c> to match a source <c>Impedance</c>.</param>
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

        ' Input checking.
        ' Leave this here, at least for now. Bad values should have been
        ' rejected in New(Double, Double), and this should not be needed unless
        ' some reason is discovered that requires extremes to be allowed. Maybe
        ' that could happen if an image impedance has extreme values that cancel
        ' or for some other interim state. Maybe if a match is being made to an
        ' image impedance or a situation involving active components that can
        ' effectively have a negative resitance value.
        ' Check for a short- or open-circuit or for invalid resistances.
        Dim LoadR As System.Double = loadZ.Resistance
        Dim SourceR As System.Double = sourceZ.Resistance
        If LoadR <= 0.0 OrElse System.Double.IsInfinity(LoadR) OrElse
            SourceR <= 0.0 OrElse System.Double.IsInfinity(SourceR) Then

            ' Leave transformations as is.
            Return False
        End If

        ' Check if a match is needed.
        If Impedance.EqualEnough(mainCirc.Z0, loadZ, sourceZ) Then
            ' Add the inaction to the array of transformations.
            Dim Trans As New Transformation With
                {.Style = TransformationStyles.None}
            Dim CurrTransCount As System.Int32 = transformations.Length
            ReDim Preserve transformations(CurrTransCount)
            transformations(CurrTransCount) = Trans
            Return True
        End If

        ' Only if the relevant circles intersect, try each geometric approach to
        ' finding a match,.

        Dim LoadG As System.Double = loadZ.ToAdmittance().Conductance
        Dim LoadCircG As New GCircle(mainCirc, LoadG)
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        If GenericCircle.CirclesIntersect(LoadCircG, SourceCircR) Then
            If Not MatchArbFirstOnG(
                mainCirc, loadZ, sourceZ, transformations) Then

                Return False
            End If
        End If

        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim SourceG As System.Double = sourceZ.ToAdmittance().Conductance
        Dim SourceCircG As New GCircle(mainCirc, SourceG)
        If GenericCircle.CirclesIntersect(LoadCircR, SourceCircG) Then
            If Not MatchArbFirstOnR(
                mainCirc, loadZ, sourceZ, transformations) Then

                Return False
            End If
        End If

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
    ''' <c>Impedance</c> to match a source <c>Impedance</c>.</param>
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

        ' Input checking.
        If z0 <= 0.0 Then
            Return False
        End If

        ' Create a SmithMainCircle for the specified Z0 and pass it to the
        ' geometric worker.
        ' Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        Return MatchArbitrary(MainCirc, loadZ, sourceZ, transformations)

    End Function ' MatchArbitrary

End Structure ' Impedance
