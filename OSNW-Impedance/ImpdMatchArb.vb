Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off
Imports System.Net.Security



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

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        'Dim Z0 As Double = mainCirc.Z0
        'Dim Y0 As Double = 1.0 / Z0
        'Dim LoadR As Double = loadZ.Resistance
        'Dim LoadX As Double = loadZ.Reactance
        'Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim LoadY As Admittance = loadZ.ToAdmittance()
        'Dim LoadG As Double = LoadY.Conductance
        Dim LoadB As Double = LoadY.Susceptance
        'Dim LoadCircG As New GCircle(mainCirc, LoadG)
        'Dim SourceR As Double = sourceZ.Resistance
        Dim SourceX As Double = sourceZ.Reactance
        'Dim SourceCircR As New RCircle(mainCirc, SourceR)
        'Dim SourceY As Admittance = sourceZ.ToAdmittance()
        'Dim SourceG As Double = SourceY.Conductance
        'Dim SourceB As Double = SourceY.Susceptance
        'Dim SourceCircG As New GCircle(mainCirc, SourceG)

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim PD As PlotDetails =
            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)
        Dim ImageZ As Impedance = PD.Impedance
        Dim ImageY As Admittance = PD.Admittance
        Dim DeltaB As System.Double = 999
        'Dim ImageR As System.Double = TargetR
        Dim ImageX As System.Double = ImageZ.Reactance
        'Dim ImageG As System.Double = TargetG
        Dim ImageB As System.Double = ImageY.Susceptance
        Dim DeltaX As System.Double = 999
        Dim DeltaY As Admittance
        Dim DeltaZ As Impedance
        Dim Style As TransformationStyles

        ' Determine the changes to take place.
        DeltaB = ImageB - LoadB
        DeltaY = New Admittance(0, DeltaB)
        DeltaZ = DeltaY.ToImpedance
        DeltaX = SourceX - ImageX

        ' Set up the transformation.
        Dim Trans As New Transformation
        With Trans
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
            .Value1 = DeltaZ.Reactance
            .Value2 = DeltaX
        End With

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
        ' RESULTS ARE KNOWN TO BE CORRECT.
        If Not loadZ.ValidateTransformation(mainCirc, sourceZ, Trans) Then
            Return False
        End If

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

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        'Dim Z0 As Double = mainCirc.Z0
        'Dim Y0 As Double = 1.0 / Z0
        'Dim LoadR As Double = loadZ.Resistance
        Dim LoadX As Double = loadZ.Reactance
        'Dim LoadCircR As New RCircle(mainCirc, LoadR)
        'Dim LoadY As Admittance = loadZ.ToAdmittance()
        'Dim LoadG As Double = LoadY.Conductance
        'Dim LoadB As Double = LoadY.Susceptance
        'Dim LoadCircG As New GCircle(mainCirc, LoadG)
        'Dim SourceR As Double = sourceZ.Resistance
        'Dim SourceX As Double = sourceZ.Reactance
        'Dim SourceCircR As New RCircle(mainCirc, SourceR)
        Dim SourceY As Admittance = sourceZ.ToAdmittance()
        'Dim SourceG As Double = SourceY.Conductance
        Dim SourceB As Double = SourceY.Susceptance
        'Dim SourceCircG As New GCircle(mainCirc, SourceG)

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim PD As PlotDetails =
            mainCirc.GetDetailsFromPlot(oneIntersection.X, oneIntersection.Y)
        Dim ImageZ As Impedance = PD.Impedance
        Dim ImageY As Admittance = PD.Admittance
        Dim DeltaB As System.Double
        ''Dim ImageR As System.Double = TargetR
        Dim ImageX As System.Double = ImageZ.Reactance
        ''Dim ImageG As System.Double = TargetG
        Dim ImageB As System.Double = ImageY.Susceptance
        Dim DeltaX As System.Double
        Dim DeltaY As Admittance
        Dim DeltaZ As Impedance
        ''Dim Style As TransformationStyles

        '' Determine the changes to take place.
        'DeltaX = ImageX - LoadX
        'DeltaZ = New Impedance(0, DeltaX)
        'DeltaY = DeltaZ.ToAdmittance
        'DeltaB = SourceB - ImageB

        ' Determine the changes to take place.
        DeltaX = ImageX - LoadX
        DeltaB = SourceB - ImageB
        DeltaY = New Admittance(0, DeltaB)
        DeltaZ = DeltaY.ToImpedance

        ' Set up the transformation.
        Dim Trans As New Transformation
        With Trans
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
            .Value2 = DeltaZ.Reactance
        End With

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
        ' RESULTS ARE KNOWN TO BE CORRECT.
        If Not loadZ.ValidateTransformation(mainCirc, sourceZ, Trans) Then
            Return False
        End If

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

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim Z0 As Double = mainCirc.Z0
        Dim Y0 As Double = 1.0 / Z0
        Dim LoadR As Double = loadZ.Resistance
        Dim LoadX As Double = loadZ.Reactance
        Dim LoadPosX As Double = 999
        Dim LoadPosY As Double = 999
        mainCirc.GetPlotXY(LoadR, LoadX, LoadPosX, LoadPosY)
        Dim LoadY As Admittance = loadZ.ToAdmittance()
        Dim LoadG As Double = LoadY.Conductance
        Dim LoadB As Double = LoadY.Susceptance
        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim LoadCircG As New GCircle(mainCirc, LoadG)
        Dim SourceR As Double = sourceZ.Resistance
        Dim SourceX As Double = sourceZ.Reactance
        Dim SourcePosX As Double = 999
        Dim SourcePosY As Double = 999
        mainCirc.GetPlotXY(SourceR, SourceX, SourcePosX, SourcePosY)
        Dim SourceY As Admittance = sourceZ.ToAdmittance()
        Dim SourceG As Double = SourceY.Conductance
        Dim SourceB As Double = SourceY.Susceptance
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        Dim SourceCircG As New GCircle(mainCirc, SourceG)

        Dim ImageR As System.Double = 999
        Dim ImageX As System.Double = 999
        Dim ImageG As System.Double = 999
        Dim ImageB As System.Double = 999
        Dim ImageY As Admittance
        Dim ImageZ As Impedance

        ' Determine the circle intersection(s).
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                   GenericCircle.GetIntersections(LoadCircG, SourceCircR)

        ' ===== FOR DIAGNOSTIC PURPOSES ONLY. =====
        Dim IntersectionsDiagInfo As New System.Text.StringBuilder
        With IntersectionsDiagInfo
            .Append($"{NameOf(Intersections.Count)}: {Intersections.Count}")
            .Append($"; Intersection: {Intersections(0)}")
            If Intersections.Count.Equals(2) Then
                .Append($"; Intersection: {Intersections(1)}")
            End If
        End With

        ' The circles do intersect. That is not useful at the perimeter.
        If Intersections.Count.Equals(1) AndAlso
             Impedance.EqualEnough(Intersections(0).X,
                                   mainCirc.GridLeftEdgeX) Then

            ' They intersect at the perimeter.
            ' No update to transformations.
            Return True
        End If

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
        ' RESULTS ARE KNOWN TO BE CORRECT.
        ' There should now be either one or two intersection points. With
        ' two, there should be one above, and one below, the resonance line.
        If Intersections.Count < 1 OrElse Intersections.Count > 2 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException(Impedance.MSGIIC)
        End If
        If Intersections.Count.Equals(1) Then
            ' The tangency should be on the resonance line.
            If Not Impedance.EqualEnough(Intersections(0).Y,
                                         mainCirc.GridCenterY) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException(
                    "The tangency is not on the resonance line.")
            End If
        End If
        If Intersections.Count.Equals(2) Then
            ' The X values should match. Check for reasonable equality when
            ' using floating point values.
            If Not Impedance.EqualEnough(Intersections(0).X,
                                         Intersections(0).X) Then
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
            If Not Impedance.EqualEnough(Offset1, Offset0) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException("Y offsets do not match.")
            End If
        End If

        ' There are now either one or two intersection points. With one, the two
        ' circles are tagent at a point on the resonance line. With two: there
        ' is one above, and one below, the resonance line; the X values match;
        ' the Y values are the same distance above and below the resonance line.

        '' Set up useful values. CONSOLIDATE/REMOVE LATER AS ABLE.
        'Dim Style As TransformationStyles
        'Dim DeltaB As System.Double
        'Dim DeltaX As System.Double
        'Dim DeltaY As Admittance
        'Dim DeltaZ As Impedance
        'Dim ImageR As System.Double = SourceR
        'Dim ImageX As System.Double
        'Dim ImageG As System.Double = SourceG
        'Dim ImageB As System.Double = 999
        'Dim ImageY As New Admittance(999, 999)
        'Dim ImageZ As Impedance

        For Each OneIntersection As OSNW.Numerics.PointD In Intersections
            If Not MatchArbitraryIntersectionFirstOnG(
                mainCirc, OneIntersection, loadZ, sourceZ, transformations) Then

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

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim Z0 As Double = mainCirc.Z0
        Dim Y0 As Double = 1.0 / Z0
        Dim LoadR As Double = loadZ.Resistance
        Dim LoadX As Double = loadZ.Reactance
        Dim LoadPosX As Double
        Dim LoadPosY As Double
        mainCirc.GetPlotXY(LoadR, LoadX, LoadPosX, LoadPosY)
        Dim LoadY As Admittance = loadZ.ToAdmittance()
        Dim LoadG As Double = LoadY.Conductance
        Dim LoadB As Double = LoadY.Susceptance
        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim LoadCircG As New GCircle(mainCirc, LoadG)
        Dim SourceR As Double = sourceZ.Resistance
        Dim SourceX As Double = sourceZ.Reactance
        Dim SourcePosX As Double
        Dim SourcePosY As Double
        mainCirc.GetPlotXY(SourceR, SourceX, SourcePosX, SourcePosY)
        Dim SourceY As Admittance = sourceZ.ToAdmittance()
        Dim SourceG As Double = SourceY.Conductance
        Dim SourceB As Double = SourceY.Susceptance
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        Dim SourceCircG As New GCircle(mainCirc, SourceG)

        Dim ImageR As System.Double = 999
        Dim ImageX As System.Double = 999
        Dim ImageG As System.Double = 999
        Dim ImageB As System.Double = 999
        Dim ImageY As Admittance
        Dim ImageZ As Impedance

        ' Determine the circle intersection(s).
        Dim Intersections _
            As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
                   GenericCircle.GetIntersections(LoadCircR, SourceCircG)

        ' ===== FOR DIAGNOSTIC PURPOSES ONLY. =====
        Dim IntersectionsDiagInfo As New System.Text.StringBuilder
        With IntersectionsDiagInfo
            .Append($"{NameOf(Intersections.Count)}: {Intersections.Count}")
            .Append($"; Intersection: {Intersections(0)}")
            If Intersections.Count.Equals(2) Then
                .Append($"; Intersection: {Intersections(1)}")
            End If
        End With

        ' The circles do intersect. That is not useful at the perimeter.
        If Intersections.Count.Equals(1) AndAlso
             Impedance.EqualEnough(Intersections(0).X,
                                   mainCirc.GridRightEdgeX) Then

            ' They intersect at the perimeter.
            ' No update to transformations.
            Return True
        End If

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
        ' RESULTS ARE KNOWN TO BE CORRECT.
        ' There should now be either one or two intersection points. With
        ' two, there should be one above, and one below, the resonance line.
        If Intersections.Count < 1 OrElse Intersections.Count > 2 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException(Impedance.MSGIIC)
        End If
        If Intersections.Count.Equals(1) Then
            ' The tangency should be on the resonance line.
            If Not Impedance.EqualEnough(Intersections(0).Y,
                                         mainCirc.GridCenterY) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException(
                    "The tangency is not on the resonance line.")
            End If
        End If
        If Intersections.Count.Equals(2) Then
            ' The X values should match. Check for reasonable equality when
            ' using floating point values.
            If Not Impedance.EqualEnough(Intersections(0).X,
                                         Intersections(0).X) Then
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
            If Not Impedance.EqualEnough(Offset1, Offset0) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException("Y offsets do not match.")
            End If
        End If

        ' There are now either one or two intersection points. With one, the two
        ' circles are tagent at a point on the resonance line. With two: there
        ' is one above, and one below, the resonance line; the X values match;
        ' the Y values are the same distance above and below the resonance line.

        '' Set up useful values. CONSOLIDATE/REMOVE LATER AS ABLE.
        'Dim Style As TransformationStyles
        'Dim DeltaB As System.Double
        'Dim DeltaX As System.Double
        'Dim DeltaY As Admittance
        'Dim DeltaZ As Impedance
        'Dim ImageR As System.Double = SourceR
        'Dim ImageX As System.Double
        'Dim ImageG As System.Double = SourceG
        'Dim ImageB As System.Double = 999
        'Dim ImageY As New Admittance(999, 999)
        'Dim ImageZ As Impedance

        For Each OneIntersection As OSNW.Numerics.PointD In Intersections
            If Not MatchArbitraryIntersectionFirstOnR(
                mainCirc, OneIntersection, loadZ, sourceZ, transformations) Then

                Return False
            End If
        Next

        ' On getting this far,
        Return True

    End Function ' MatchArbFirstOnG

    ''' <summary>
    ''' xxxxxxxxxxxxxxxxxxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxxxxxxxxxxxxxxxxxx</returns>
    Public Shared Function MatchArbitraryIntersectionFirstOnG(
        ByVal mainCirc As SmithMainCircle,
        ByVal oneIntersection As OSNW.Numerics.PointD, ByVal loadZ As Impedance,
        ByVal sourceZ As Impedance, ByRef transformations As Transformation()) _
        As System.Boolean

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim Z0 As Double = mainCirc.Z0
        Dim Y0 As Double = 1.0 / Z0
        Dim LoadR As Double = loadZ.Resistance
        Dim LoadX As Double = loadZ.Reactance
        Dim LoadPosX As Double
        Dim LoadPosY As Double
        mainCirc.GetPlotXY(LoadR, LoadX, LoadPosX, LoadPosY)
        Dim LoadY As Admittance = loadZ.ToAdmittance()
        Dim LoadG As Double = LoadY.Conductance
        Dim LoadB As Double = LoadY.Susceptance
        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim LoadCircG As New GCircle(mainCirc, LoadG)
        Dim SourceR As Double = sourceZ.Resistance
        Dim SourceX As Double = sourceZ.Reactance
        Dim SourcePosX As Double
        Dim SourcePosY As Double
        mainCirc.GetPlotXY(SourceR, SourceX, SourcePosX, SourcePosY)
        Dim SourceY As Admittance = sourceZ.ToAdmittance()
        Dim SourceG As Double = SourceY.Conductance
        Dim SourceB As Double = SourceY.Susceptance
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        Dim SourceCircG As New GCircle(mainCirc, SourceG)

        Dim ImageR As System.Double = 999
        Dim ImageX As System.Double = 999
        Dim ImageG As System.Double = 999
        Dim ImageB As System.Double = 999
        Dim ImageY As Admittance
        Dim ImageZ As Impedance

        If Not MatchArbFirstOnG(mainCirc, oneIntersection,
                 loadZ, sourceZ, transformations) Then

            Return False
        End If

        ' ===== FOR DIAGNOSTIC PURPOSES ONLY. =====
        Dim TransformationDiagInfo As New System.Text.StringBuilder
        TransformationDiagInfo.Append($"FirstOnG:")
        For Each OneTransformation As Transformation In transformations
            With TransformationDiagInfo
                .Append($" {NameOf(OneTransformation.Style)}: {OneTransformation.Style}")
                .Append($"; Value1: {OneTransformation.Value1}")
                .Append($"; Value2: {OneTransformation.Value2}")
            End With
        Next

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
        ' RESULTS ARE KNOWN TO BE CORRECT.
        For Each OneTransformation As Transformation In transformations
            If Not loadZ.ValidateTransformation(
                mainCirc, sourceZ, OneTransformation) Then

                Return False
            End If
        Next

        ' On getting this far,
        Return True

    End Function ' MatchArbitraryIntersectionFirstOnG

    ''' <summary>
    ''' xxxxxxxxxxxxxxxxxxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxxxxxxxxxxxxxxxxxx</returns>
    Public Shared Function MatchArbitraryIntersectionFirstOnR(
        ByVal mainCirc As SmithMainCircle,
        ByVal oneIntersection As OSNW.Numerics.PointD, ByVal loadZ As Impedance,
        ByVal sourceZ As Impedance, ByRef transformations As Transformation()) _
        As System.Boolean

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim Z0 As Double = mainCirc.Z0
        Dim Y0 As Double = 1.0 / Z0
        Dim LoadR As Double = loadZ.Resistance
        Dim LoadX As Double = loadZ.Reactance
        Dim LoadPosX As Double
        Dim LoadPosY As Double
        mainCirc.GetPlotXY(LoadR, LoadX, LoadPosX, LoadPosY)
        Dim LoadY As Admittance = loadZ.ToAdmittance()
        Dim LoadG As Double = LoadY.Conductance
        Dim LoadB As Double = LoadY.Susceptance
        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim LoadCircG As New GCircle(mainCirc, LoadG)
        Dim SourceR As Double = sourceZ.Resistance
        Dim SourceX As Double = sourceZ.Reactance
        Dim SourcePosX As Double
        Dim SourcePosY As Double
        mainCirc.GetPlotXY(SourceR, SourceX, SourcePosX, SourcePosY)
        Dim SourceY As Admittance = sourceZ.ToAdmittance()
        Dim SourceG As Double = SourceY.Conductance
        Dim SourceB As Double = SourceY.Susceptance
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        Dim SourceCircG As New GCircle(mainCirc, SourceG)

        Dim ImageR As System.Double = 999
        Dim ImageX As System.Double = 999
        Dim ImageG As System.Double = 999
        Dim ImageB As System.Double = 999
        Dim ImageY As Admittance
        Dim ImageZ As Impedance

        If Not MatchArbFirstOnR(mainCirc, oneIntersection,
                 loadZ, sourceZ, transformations) Then

            Return False
        End If

        ' ===== FOR DIAGNOSTIC PURPOSES ONLY. =====
        Dim TransformationDiagInfo As New System.Text.StringBuilder
        TransformationDiagInfo.Append($"FirstOnG:")
        For Each OneTransformation As Transformation In transformations
            With TransformationDiagInfo
                .Append($" {NameOf(OneTransformation.Style)}: {OneTransformation.Style}")
                .Append($"; Value1: {OneTransformation.Value1}")
                .Append($"; Value2: {OneTransformation.Value2}")
            End With
        Next

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE Transformation
        ' RESULTS ARE KNOWN TO BE CORRECT.
        For Each OneTransformation As Transformation In transformations
            If Not loadZ.ValidateTransformation(
                mainCirc, sourceZ, OneTransformation) Then

                Return False
            End If
        Next

        ' On getting this far,
        Return True

    End Function ' MatchArbitraryIntersectionFirstOnR

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
    ''' <para> An assumption is made that the calling code has determined that
    ''' the <c>Impedance</c>s lie in valid positions. Failure to meet that
    ''' assumption could result in invalid, or incomplete, results.</para>
    ''' A succcessful process might result in an empty
    ''' <paramref name="transformations"/>.
    ''' </remarks>
    Public Shared Function MatchArbitrary(
        ByVal mainCirc As SmithMainCircle,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' REF: Smith Chart Full Presentation, page 26.
        ' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim Z0 As Double = mainCirc.Z0
        Dim Y0 As Double = 1.0 / Z0
        Dim LoadR As Double = loadZ.Resistance
        Dim LoadX As Double = loadZ.Reactance
        Dim LoadPosX As Double
        Dim LoadPosY As Double
        mainCirc.GetPlotXY(LoadR, LoadX, LoadPosX, LoadPosY)
        Dim LoadY As Admittance = loadZ.ToAdmittance()
        Dim LoadG As Double = LoadY.Conductance
        Dim LoadB As Double = LoadY.Susceptance
        Dim LoadCircR As New RCircle(mainCirc, LoadR)
        Dim LoadCircG As New GCircle(mainCirc, LoadG)
        Dim SourceR As Double = sourceZ.Resistance
        Dim SourceX As Double = sourceZ.Reactance
        Dim SourcePosX As Double
        Dim SourcePosY As Double
        mainCirc.GetPlotXY(SourceR, SourceX, SourcePosX, SourcePosY)
        Dim SourceY As Admittance = sourceZ.ToAdmittance()
        Dim SourceG As Double = SourceY.Conductance
        Dim SourceB As Double = SourceY.Susceptance
        Dim SourceCircR As New RCircle(mainCirc, SourceR)
        Dim SourceCircG As New GCircle(mainCirc, SourceG)

        ' ===== FOR DIAGNOSTIC PURPOSES ONLY. =====
        Dim BaseDiagInfo As New System.Text.StringBuilder
        With BaseDiagInfo
            .Append($"{NameOf(Y0)}: {Y0}")
            .Append($"; {NameOf(LoadR)}: {LoadR}")
            .Append($"; {NameOf(LoadX)}: {LoadX}")
            .Append($"; {NameOf(LoadY)}: {LoadY}")
            .Append($"; {NameOf(LoadG)}: {LoadG}")
            .Append($"; {NameOf(LoadB)}: {LoadB}")
            .Append($"; {NameOf(SourceR)}: {SourceR}")
            .Append($"; {NameOf(SourceX)}: {SourceX}")
            .Append($"; {NameOf(SourceY)}: {SourceY}")
            .Append($"; {NameOf(SourceG)}: {SourceG}")
            .Append($"; {NameOf(SourceB)}: {SourceB}")
        End With

        ' ===== FOR DIAGNOSTIC PURPOSES ONLY. =====
        Dim CircleDiagInfo As New System.Text.StringBuilder
        With CircleDiagInfo
            .Append($"{NameOf(LoadCircR)}: {LoadCircR}")
            .Append($"; {NameOf(LoadCircG)}: {LoadCircG}")
            .Append($"; {NameOf(SourceCircR)}: {SourceCircR}")
            .Append($"; {NameOf(SourceCircG)}: {SourceCircG}")
        End With

        ' Try each approach to finding a match, only if the circles intersect.
        If GenericCircle.CirclesIntersect(LoadCircG, SourceCircR) Then
            If Not MatchArbFirstOnG(
                mainCirc, loadZ, sourceZ, transformations) Then

                Return False
            End If
        End If
        If GenericCircle.CirclesIntersect(LoadCircR, SourceCircG) Then
            If Not MatchArbFirstOnR(
                mainCirc, loadZ, sourceZ, transformations) Then

                Return False
            End If
        End If

        ' ===== FOR DIAGNOSTIC PURPOSES ONLY. =====
        Dim TransformationDiagInfo As New System.Text.StringBuilder
        For Each OneTransformation As Transformation In transformations
            With TransformationDiagInfo
                .Append($" {NameOf(OneTransformation.Style)}:" &
                        $" {OneTransformation.Style}")
                .Append($"; Value1: {OneTransformation.Value1}")
                .Append($"; Value2: {OneTransformation.Value2}")
            End With
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

        ' Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
        Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
        Return MatchArbitrary(MainCirc, loadZ, sourceZ, transformations)
    End Function ' MatchArbitrary

End Structure ' Impedance
