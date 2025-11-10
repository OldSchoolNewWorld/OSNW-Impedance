Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off
Imports System.ComponentModel.Design



' This document contains items related to matching a load impedance to an
' arbitrary source impedance.

Partial Public Structure Impedance

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="MainCirc">xxxxxxxxxx</param>
    ''' <param name="loadZ">xxxxxxxxxx</param>
    ''' <param name="targetZ">xxxxxxxxxx</param>
    ''' <param name="transformations">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function MatchArbFirstOnG(ByVal MainCirc As SmithMainCircle,
        ByVal loadZ As Impedance, ByVal targetZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        '
        '
        ' At least for now,
        ' On getting this far,
        Return False
        '
        '
        '

        '' Set up useful values.
        'Dim Z0 As Double = MainCirc.Z0
        'Dim Y0 As Double = 1.0 / Z0
        'Dim LoadR As Double = loadZ.Resistance
        'Dim LoadX As Double = loadZ.Reactance
        'Dim LoadCircR As New RCircle(MainCirc, LoadR)
        'Dim LoadY As Admittance = loadZ.ToAdmittance()
        'Dim LoadG As Double = LoadY.Conductance
        'Dim LoadB As Double = LoadY.Susceptance
        'Dim LoadCircG As New GCircle(MainCirc, LoadG)
        'Dim TargetR As Double = targetZ.Resistance
        'Dim TargetX As Double = targetZ.Reactance
        'Dim TargetCircR As New RCircle(MainCirc, TargetR)
        'Dim TargetY As Admittance = targetZ.ToAdmittance()
        'Dim TargetG As Double = TargetY.Conductance
        'Dim TargetB As Double = TargetY.Susceptance
        'Dim TargetCircG As New GCircle(MainCirc, TargetG)

        '' Determine the circle intersection(s).
        'Dim Intersections _
        '    As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
        '           GenericCircle.GetIntersections(TargetCircR, LoadCircG)

        '' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
        '' RESULTS ARE KNOWN TO BE CORRECT.
        '' There should now be either one or two intersection points. With
        '' two, there should be one above, and one below, the resonance line.
        'If Intersections.Count < 1 OrElse Intersections.Count > 2 Then
        '    'Dim CaughtBy As System.Reflection.MethodBase =
        '    '    System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ApplicationException(Impedance.MSGIIC)
        'End If
        'If Intersections.Count.Equals(2) Then
        '    ' The X values should match. Check for reasonable equality when using
        '    ' floating point values.
        '    If Not Impedance.EqualEnough(Intersections(0).X, Intersections(0).X) Then
        '        'Dim CaughtBy As System.Reflection.MethodBase =
        '        '    System.Reflection.MethodBase.GetCurrentMethod
        '        Throw New System.ApplicationException("X values do not match.")
        '    End If
        '    ' The Y values should be the same distance above and below the
        '    ' resonance line. Check for reasonable equality when using floating
        '    ' point values.
        '    Dim Offset0 As System.Double =
        '            System.Math.Abs(Intersections(0).Y - MainCirc.GridCenterY)
        '    Dim Offset1 As System.Double =
        '            System.Math.Abs(Intersections(1).Y - MainCirc.GridCenterY)
        '    If Not Impedance.EqualEnough(Offset1, Offset0) Then
        '        'Dim CaughtBy As System.Reflection.MethodBase =
        '        '    System.Reflection.MethodBase.GetCurrentMethod
        '        Throw New System.ApplicationException("Y offsets do not match.")
        '    End If
        'End If

        '' There are now either one or two intersection points. With two, there
        '' is one above, and one below, the resonance line; the X values match;
        '' the Y values are the same distance above and below the resonance line.

        '' Set up useful values.
        'Dim Style As TransformationStyles
        'Dim DeltaB As System.Double
        'Dim DeltaX As System.Double
        'Dim DeltaY As Admittance
        'Dim DeltaZ As Impedance
        'Dim ImageR As System.Double = TargetR
        'Dim ImageX As System.Double
        'Dim ImageG As System.Double = TargetG
        'Dim ImageB As System.Double = 999
        'Dim ImageY As New Admittance(999, 999)
        'Dim ImageZ As Impedance

        'If Intersections.Count.Equals(1) Then
        '    ' With one intersection, expect one valid solution. That will
        '    ' only happen when the two circles are tangent at a point on the
        '    ' resonance line.
        '    ' xxxxxxxx Move CW or CCW on the LoadG circle to the LoadR circle.

        '    '
        '    '
        '    '
        '    '
        '    '

        '    ' At least for now,
        '    ' On getting this far,
        '    Return False



        'Else
        '    ' With two intersections, expect two valid solutions, one to
        '    ' each intersection.
        '    ' xxxxxxxx Move CW or CCW on the LoadG circle to the LoadR circle.



        '    ' At least for now,
        '    ' On getting this far,
        '    Return False

        'End If

        ''
        ''
        '' At least for now,
        '' On getting this far,
        'Return False
        ''
        ''
        ''

    End Function ' MatchArbFirstOnG

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="MainCirc">xxxxxxxxxx</param>
    ''' <param name="loadZ">xxxxxxxxxx</param>
    ''' <param name="targetZ">xxxxxxxxxx</param>
    ''' <param name="transformations">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    Public Shared Function MatchArbFirstOnR(ByVal MainCirc As SmithMainCircle,
        ByVal loadZ As Impedance, ByVal targetZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        '
        '
        ' At least for now,
        ' On getting this far,
        Return False
        '
        '
        '

        '' Set up useful values.
        'Dim Z0 As Double = MainCirc.Z0
        'Dim Y0 As Double = 1.0 / Z0
        'Dim LoadR As Double = loadZ.Resistance
        'Dim LoadX As Double = loadZ.Reactance
        'Dim LoadCircR As New RCircle(MainCirc, LoadR)
        'Dim LoadY As Admittance = loadZ.ToAdmittance()
        'Dim LoadG As Double = LoadY.Conductance
        'Dim LoadB As Double = LoadY.Susceptance
        'Dim LoadCircG As New GCircle(MainCirc, LoadG)
        'Dim TargetR As Double = targetZ.Resistance
        'Dim TargetX As Double = targetZ.Reactance
        'Dim TargetCircR As New RCircle(MainCirc, TargetR)
        'Dim TargetY As Admittance = targetZ.ToAdmittance()
        'Dim TargetG As Double = TargetY.Conductance
        'Dim TargetB As Double = TargetY.Susceptance
        'Dim TargetCircG As New GCircle(MainCirc, TargetG)

        '' Determine the circle intersection(s).
        'Dim Intersections _
        '    As System.Collections.Generic.List(Of OSNW.Numerics.PointD) =
        '           GenericCircle.GetIntersections(TargetCircR, LoadCircG)

        '' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
        '' RESULTS ARE KNOWN TO BE CORRECT.
        '' There should now be either one or two intersection points. With
        '' two, there should be one above, and one below, the resonance line.
        'If Intersections.Count < 1 OrElse Intersections.Count > 2 Then
        '    'Dim CaughtBy As System.Reflection.MethodBase =
        '    '    System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ApplicationException(Impedance.MSGIIC)
        'End If
        'If Intersections.Count.Equals(2) Then
        '    ' The X values should match. Check for reasonable equality when using
        '    ' floating point values.
        '    If Not Impedance.EqualEnough(Intersections(0).X, Intersections(0).X) Then
        '        'Dim CaughtBy As System.Reflection.MethodBase =
        '        '    System.Reflection.MethodBase.GetCurrentMethod
        '        Throw New System.ApplicationException("X values do not match.")
        '    End If
        '    ' The Y values should be the same distance above and below the
        '    ' resonance line. Check for reasonable equality when using floating
        '    ' point values.
        '    Dim Offset0 As System.Double =
        '            System.Math.Abs(Intersections(0).Y - MainCirc.GridCenterY)
        '    Dim Offset1 As System.Double =
        '            System.Math.Abs(Intersections(1).Y - MainCirc.GridCenterY)
        '    If Not Impedance.EqualEnough(Offset1, Offset0) Then
        '        'Dim CaughtBy As System.Reflection.MethodBase =
        '        '    System.Reflection.MethodBase.GetCurrentMethod
        '        Throw New System.ApplicationException("Y offsets do not match.")
        '    End If
        'End If

        '' There are now either one or two intersection points. With two, there
        '' is one above, and one below, the resonance line; the X values match;
        '' the Y values are the same distance above and below the resonance line.

        '' Set up useful values.
        'Dim Style As TransformationStyles
        'Dim DeltaB As System.Double
        'Dim DeltaX As System.Double
        'Dim DeltaY As Admittance
        'Dim DeltaZ As Impedance
        'Dim ImageR As System.Double = TargetR
        'Dim ImageX As System.Double
        'Dim ImageG As System.Double = TargetG
        'Dim ImageB As System.Double = 999
        'Dim ImageY As New Admittance(999, 999)
        'Dim ImageZ As Impedance

        'If Intersections.Count.Equals(1) Then
        '    ' With one intersection, expect one valid solution. That will
        '    ' only happen when the two circles are tangent at a point on the
        '    ' resonance line.
        '    ' xxxxxxxx Move CW or CCW on the LoadG circle to the LoadR circle.

        '    '
        '    '
        '    '
        '    '
        '    '

        '    ' At least for now,
        '    ' On getting this far,
        '    Return False



        'Else
        '    ' With two intersections, expect two valid solutions, one to
        '    ' each intersection.
        '    ' xxxxxxxx Move CW or CCW on the LoadG circle to the LoadR circle.



        '    ' At least for now,
        '    ' On getting this far,
        '    Return False

        'End If

        ''
        ''
        '' At least for now,
        '' On getting this far,
        'Return False
        ''
        ''
        ''

    End Function ' MatchArbFirstOnR



    ''' <summary>
    ''' xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function MatchArbitraryIntersection(
        ByVal oneIntersection As OSNW.Numerics.PointD,
        ByVal MainCirc As SmithMainCircle, ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        '
        '
        ' At least for now,
        ' On getting this far,
        Return False
        '
        '
        '

        'xxxxxxxxxxxxxxxxxxxxxxxxxx
        ' xxxxxxxx First, move CW or CCW on the LoadG circle to the LoadR circle.
        ' or
        ' xxxxxxxx First, move CW or CCW on the LoadR circle to the LoadG circle.
        'xxxxxxxxxxxxxxxxxxxxxxxxxx

        ' First, move CW or CCW on the LoadG circle to the LoadR circle.


        If Not MatchArbFirstOnG(MainCirc,
                 loadZ, sourceZ, transformations) Then

            '
            '
            ' At least for now,
            ' On getting this far,
            Return False
            '
            '
        End If


        '
        '
        '
        '
        '






        ' First, move CW or CCW on the LoadR circle to the LoadG circle.
        If Not MatchArbFirstOnG(MainCirc,
                 loadZ, sourceZ, transformations) Then

            '
            '
            ' At least for now,
            ' On getting this far,
            Return False
            '
            '
        End If
        '
        '
        '
        '
        '



        '' Calculate the image admittance at the intersection point.

        'DeltaB = GetMatchArbitraryImageDeltaB(MainCirc, loadZ, oneIntersection)
        'DeltaY = New Admittance(0, DeltaB)
        'DeltaZ = DeltaY.ToImpedance
        'ImageZ = Impedance.AddShuntImpedance(loadZ, DeltaZ)
        'ImageX = ImageZ.Reactance
        'DeltaX = SourceX - ImageX
        'If oneIntersection.Y > MainCirc.GridCenterY Then
        '    ' CCW on a G-circle needs a shunt inductor
        '    ' CCW on an R-circle needs a series capacitor
        '    Style = TransformationStyles.ShuntIndSeriesCap
        'ElseIf oneIntersection.Y < MainCirc.GridCenterY Then
        '    ' CW on a G-circle needs a shunt capacitor
        '    ' CW on an R-circle needs a series inductor
        '    Style = TransformationStyles.ShuntCapSeriesInd
        'Else ' OneIntersection.Y = MainCirc.GridCenterY
        '    '
        '    '
        '    '
        '    ' Where might this happen? Account for special cases. Maybe if the
        '    ' load and source are on the R=Z0 and G=Y0 circles?
        '    '
        '    'Dim CaughtBy As System.Reflection.MethodBase =
        '    '    System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.ApplicationException("OneIntersection.Y = 0.0")
        '    '
        '    '
        '    '
        'End If
        'Dim Transformation As New Transformation With {
        '            .Style = Style,
        '            .Value1 = DeltaZ.Reactance,
        '            .Value2 = DeltaX}
        ''If Not Me.InsideGEqualsY0(
        ''    MainCirc, OneIntersection, Transformation) Then
        ''    'Dim CaughtBy As System.Reflection.MethodBase =
        ''    '    System.Reflection.MethodBase.GetCurrentMethod
        ''    Throw New System.ApplicationException("Transformation failed.")
        ''End If
        ''



    End Function ' MatchArbitraryIntersection
    '    xxxx

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, on
    ''' the specified <c>SmithMainCircle</c>.
    ''' </summary>
    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    ''' <paramref name="loadZ"/> should be matched.</param>
    ''' <param name="MainCirc">Specifies a <c>SmithMainCircle</c> in which the
    ''' match is to be made.</param>
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
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByVal MainCirc As SmithMainCircle,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' REF: Smith Chart Full Presentation, page 26.
        ' https://amris.mbi.ufl.edu/wordpress/files/2021/01/SmithChart_FullPresentation.pdf

        ' Set up useful local values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim Z0 As Double = MainCirc.Z0
        Dim Y0 As Double = 1.0 / Z0
        Dim LoadR As Double = loadZ.Resistance
        Dim LoadX As Double = loadZ.Reactance
        Dim LoadPosX As Double
        Dim LoadPosY As Double
        MainCirc.GetPlotXY(LoadR, LoadX, LoadPosX, LoadPosY)
        Dim LoadY As Admittance = loadZ.ToAdmittance()
        Dim LoadG As Double = LoadY.Conductance
        Dim LoadB As Double = LoadY.Susceptance
        Dim LoadCircR As New RCircle(MainCirc, LoadR)
        Dim LoadCircG As New GCircle(MainCirc, LoadG)
        Dim SourceR As Double = sourceZ.Resistance
        Dim SourceX As Double = sourceZ.Reactance
        Dim SourcePosX As Double
        Dim SourcePosY As Double
        MainCirc.GetPlotXY(SourceR, SourceX, SourcePosX, SourcePosY)
        Dim SourceY As Admittance = sourceZ.ToAdmittance()
        Dim SourceG As Double = SourceY.Conductance
        Dim SourceB As Double = SourceY.Susceptance
        Dim SourceCircR As New RCircle(MainCirc, SourceR)
        Dim SourceCircG As New GCircle(MainCirc, SourceG)

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

        ' THESE CHECKS CAN BE DELETED/COMMENTED AFTER THE GetIntersections()
        ' RESULTS ARE KNOWN TO BE CORRECT.
        ' There should now be either one or two intersection points. With
        ' two, there should be one above, and one below, the resonance line.
        If Intersections.Count < 1 OrElse Intersections.Count > 2 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ApplicationException(Impedance.MSGIIC)
        End If
        If Intersections.Count.Equals(2) Then
            ' The X values should match. Check for reasonable equality when using
            ' floating point values.
            If Not Impedance.EqualEnough(Intersections(0).X, Intersections(0).X) Then
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
            If Not Impedance.EqualEnough(Offset1, Offset0) Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ApplicationException("Y offsets do not match.")
            End If
        End If

        ' There are now either one or two intersection points. With two, there
        ' is one above, and one below, the resonance line; the X values match;
        ' the Y values are the same distance above and below the resonance line.

        ' Move
        '     CW or CCW on the LoadG circle to the SourceR circle; there may be
        '     either one or two intersections.
        '     Then CW or CCW on the SourceR circle to SourceZ.
        ' or
        ' Move
        '     CW or CCW on the LoadR circle to the SourceG circle; there may be
        '     either one or two intersections.
        '     Then CW or CCW on the SourceG circle to SourceZ.

        ' Set up useful values. CONSOLIDATE/REMOVE LATER AS ABLE.
        Dim Style As TransformationStyles
        Dim DeltaB As System.Double
        Dim DeltaX As System.Double
        Dim DeltaY As Admittance
        Dim DeltaZ As Impedance
        Dim ImageR As System.Double = SourceR
        Dim ImageX As System.Double
        Dim ImageG As System.Double = SourceG
        Dim ImageB As System.Double = 999
        Dim ImageY As New Admittance(999, 999)
        Dim ImageZ As Impedance

        If Intersections.Count.Equals(1) OrElse
            Intersections.Count.Equals(2) Then

            For Each OneIntersection As OSNW.Numerics.PointD In Intersections
                If MatchArbitraryIntersection(OneIntersection, MainCirc, loadZ, sourceZ, transformations) Then
                    '
                    '
                    '
                    ' At least for now,
                    ' On getting this far,
                    Return False
                    '
                    '
                    '
                Else
                    '
                    '
                    '
                    ' At least for now,
                    ' On getting this far,
                    Return False
                    '
                    '
                    '
                End If
            Next

            '
            '
            ' At least for now,
            ' On getting this far,
            Return False
            '
            '
            '

        Else
            ' There are either no intersections or an invalid number of intersections.
            Return False
        End If

        ' On getting this far,
        Return False

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
        Return MatchArbitrary(loadZ, sourceZ, MainCirc, transformations)
    End Function ' MatchArbitrary

End Structure ' Impedance
