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
    Public Shared Function MatchArbFirstOnG1(ByVal mainCirc As SmithMainCircle,
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

    End Function ' MatchArbFirstOnG1

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
    Public Shared Function MatchArbFirstOnR1(ByVal mainCirc As SmithMainCircle,
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

    End Function ' MatchArbFirstOnR1

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
    Public Shared Function MatchArbitrary1(
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

            If Not MatchArbFirstOnG1(mainCirc, OneIntersection, loadZ, sourceZ,
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

            If Not MatchArbFirstOnR1(mainCirc, OneIntersection, loadZ, sourceZ,
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

    End Function ' MatchArbitrary1

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
    Public Shared Function MatchArbitrary1(z0 As System.Double,
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
        Return MatchArbitrary1(MainCirc, loadZ, sourceZ, transformations)

    End Function ' MatchArbitrary1

    '==============================================================

    ''' <summary>
    ''' A list of four <see cref="Impedance"/> objects representing the image
    ''' impedances to potentially be used in matching a load <c>Impedance</c> to
    ''' an arbitrary source <c>Impedance</c>.
    ''' </summary>
    Public Class ImageImpedanceList
        Inherits List(Of Impedance)
        Implements IList

        ' Ref: IList Interface
        ' https://learn.microsoft.com/en-us/dotnet/api/system.collections.ilist?view=net-10.0

        ''' <summary>
        ''' Defines the default capacity of the
        ''' <see cref="ImageImpedanceList"/>.
        ''' </summary>
        Public Const DEFAULTCAPACITY As System.Int32 = 4

        Private ReadOnly _contents(DEFAULTCAPACITY - 1) As System.Object
        '        Private _count As System.Int32

        ' IList members.

        'Public Function Add(ByVal value As Object) As Integer Implements IList.Add
        '    If _count < _contents.Length Then
        '        _contents(_count) = value
        '        _count += 1

        '        Return _count - 1
        '    End If

        '    Return -1
        'End Function

        'Public Sub Clear() Implements IList.Clear
        '    _count = 0
        'End Sub

        'Public Overloads Function Contains(ByVal value As Object) _
        '    As System.Boolean _
        '    Implements IList.Contains
        ' xxxxxxxxxx THIS WOULD LIKELY NEED TO CHANGE EQUALITY TO EQUALENOUGH

        '    Dim ValImpedance As Impedance = CType(value, Impedance)
        '    For i As Integer = 0 To Count - 1
        '        If _contents(i).Equals(ValImpedance) Then Return True
        '    Next

        '    Return False
        'End Function

        'Public Overloads Function IndexOf(ByVal value As Object) _
        '    As System.Int32 _
        '    Implements IList.IndexOf
        ' xxxxxxxxxx THIS WOULD LIKELY NEED TO CHANGE EQUALITY TO EQUALENOUGH

        '    Dim ValImpedance As Impedance = CType(value, Impedance)
        '    For i As Integer = 0 To Count - 1
        '        If _contents(i).Equals(ValImpedance) Then Return i
        '    Next
        '    Return -1
        'End Function

        'Public Sub Insert(ByVal index As Integer, ByVal value As Object) Implements IList.Insert

        '    If _count + 1 <= _contents.Length AndAlso index < Count AndAlso index >= 0 Then
        '        _count += 1

        '        For i As Integer = Count - 1 To index Step -1
        '            _contents(i) = _contents(i - 1)
        '        Next
        '        _contents(index) = value
        '    End If
        'End Sub

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        ''' <returns>xxxxxxxxxx</returns>
        Public ReadOnly Property IsFixedSize() As System.Boolean _
            Implements IList.IsFixedSize

            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property IsReadOnly() As System.Boolean _
            Implements IList.IsReadOnly

            Get
                Return False
            End Get
        End Property

        'Public Sub Remove(ByVal value As Object) Implements IList.Remove
        '    RemoveAt(IndexOf(value))
        'End Sub

        'Public Sub RemoveAt(ByVal index As Integer) Implements IList.RemoveAt

        '    If index >= 0 AndAlso index < Count Then
        '        For i As Integer = index To Count - 2
        '            _contents(i) = _contents(i + 1)
        '        Next
        '        _count -= 1
        '    End If
        'End Sub

        'Public Overloads Property Item(ByVal index As Integer) As Object _
        '    Implements IList.Item

        '    Get
        '        Return _contents(index)
        '    End Get
        '    Set(ByVal value As Object)
        '        _contents(index) = value
        '    End Set
        'End Property

        ' ICollection members.

        'Public Sub CopyTo(ByVal array As Array, ByVal index As Integer) Implements ICollection.CopyTo
        '    For i As Integer = 0 To Count - 1
        '        array.SetValue(_contents(i), index)
        '        index += 1
        '    Next
        'End Sub

        '        Public Overloads ReadOnly Property Count() As System.Int32 _
        '            Implements ICollection.Count

        '            Get
        '                Return _count
        '            End Get
        '        End Property

        'Public ReadOnly Property IsSynchronized() As Boolean Implements ICollection.IsSynchronized
        '    Get
        '        Return False
        '    End Get
        'End Property

        '' Return the current instance since the underlying store is not
        '' publicly available.
        'Public ReadOnly Property SyncRoot() As Object Implements ICollection.SyncRoot
        '    Get
        '        Return Me
        '    End Get
        'End Property

        ' IEnumerable members.

        'Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator

        '    ' Refer to the IEnumerator documentation for an example of
        '    ' implementing an enumerator.
        '    Throw New NotImplementedException("The method or operation is not implemented.")
        'End Function

        'Public Sub PrintContents()
        '    Console.WriteLine($"List has a capacity of {_contents.Length} and currently has {_count} elements.")
        '    Console.Write("List contents:")

        '    For i As Integer = 0 To Count - 1
        '        Console.Write($" {_contents(i)}")
        '    Next

        '    Console.WriteLine()
        'End Sub

        'Public Shared Function GetFixedSizeImageImpedanceList() _
        '    As ImageImpedanceList

        '    'Dim UnfixedSizeAL As New ImageImpedanceList(DEFAULTCAPACITY)
        '    'Dim FixedSizeAL As ArrayList = ArrayList.FixedSize(UnfixedSizeAL)
        '    Dim UnfixedSizeAL As New ImageImpedanceList(DEFAULTCAPACITY)
        '    Dim FixedSizeAL As ImageImpedanceList = ArrayList.FixedSize(UnfixedSizeAL)

        '    '            Dim FixedSizeAAL As ImageImpedanceList = CType(FixedSizeAL, ImageImpedanceList)
        '    '            Dim FixedSizeIIL As ImageImpedanceList =
        '    '                CType(FixedSizeAL, ImageImpedanceList)
        '    Dim FixedSizeIIL As ImageImpedanceList = FixedSizeAL


        '    Return FixedSizeIIL

        'End Function ' GetFixedSizeImageImpedanceList

        '''' <summary>
        '''' Adds an image <see cref="Impedance"/> to the end of the
        '''' <see cref="ImageImpedanceList"/>.
        '''' </summary>
        '''' <param name="value">Specifies the image <see cref="Impedance"/> to
        '''' be added.</param>
        '''' <exception cref="System.NotSupportedException">
        '''' When the <see cref="ImageImpedanceList"/> is read-only or the
        '''' <see cref="ImageImpedanceList"/> ***** has a fixed size *****.
        '''' </exception>
        '''' <returns>The <see cref="ImageImpedanceList"/> index at which the
        '''' <paramref name="value"/> has been added.</returns>
        'Public Shadows Function Add(ByVal value As Impedance) As System.Int32
        '    Return MyBase.Add(value)
        'End Function ' Add

        '''' <summary>
        '''' Improperly attempts to add an <see cref="Object"/> that is not an
        '''' <see cref="Impedance"/>, to the end of the
        '''' <see cref="ImageImpedanceList"/>.
        '''' </summary>
        '''' <param name="value">xxxxxxxxxx</param>
        '''' <exception cref="System.NotSupportedException">
        '''' When the <see cref="ImageImpedanceList"/> is read-only or the
        '''' <see cref="ImageImpedanceList"/> ***** has a fixed size *****.
        '''' </exception>
        '''' <returns>Never returns a value. An exception is always thrown.</returns>
        'Public Shared Shadows Function Add(value As Object) As System.Int32
        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.NotSupportedException(String.Concat(
        '        $"Failed To process {CaughtBy}.",
        '        $" {value} is not an Impedance.",
        '        " Use Add(Impedance)"))
        'End Function ' Add

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        ''' <param name="capacity">xxxxxxxxxx</param>
        Public Sub New(capacity As Integer)
            MyBase.New(capacity)
            Dim BadImpedance As New Impedance(BADIMPDVALUE, BADIMPDVALUE)
            For i As System.Int32 = 0 To capacity - 1
                Me.Add(BadImpedance)
            Next
        End Sub

        ''' <summary>
        ''' Improperly attempts to xxxxxxxxxx.
        ''' </summary>
        Public Sub New()

            '    Dim CaughtBy As System.Reflection.MethodBase =
            '        System.Reflection.MethodBase.GetCurrentMethod
            '    Throw New System.NotSupportedException(String.Concat(
            '        $"{CaughtBy} called directly. Use New(capacity)"))

            Me.New(4)
        End Sub

    End Class ' ImageImpedanceList

    ''' <summary>
    ''' Attempts to populate an <see cref="ImageImpedanceList"/> with four
    ''' candidate image impedances to potentially be used in matching a load
    ''' <c>Impedance</c> to an arbitrary source <c>Impedance</c>.
    ''' </summary>
    ''' <param name="loadZ">Specifies the <c>Impedance</c> to match to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <param name="sourceZ">Specifies the <c>Impedance</c> to which
    ''' <paramref name="loadZ"/> should be matched.</param>
    ''' <param name="images">Returns four candidate image impedances to
    ''' potentially be used in matching a load <paramref name="loadZ"/> to
    ''' <paramref name="sourceZ"/>.</param>
    ''' <returns> Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns, by reference in
    ''' <paramref name="images"/>, the candidate image impedances.</returns>
    Friend Shared Function TryLoadImageImpedancesList2(
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef images As ImageImpedanceList) _
        As System.Boolean

        Try

            ' Determine the image impedances. That is, effectively, done as is
            ' done with a Smith Chart: First, move on a G-circle, from the load
            ' to the image impedance, then on an R-Circle to the source. Next,
            ' move on an R-circle, from the load to the image impedance, then on
            ' a G-Circle to the source. Image impedances occur at the
            ' intersections of each pair of circles.

            ' Full derivation:

            ' Complex reciprocols
            ' Reciprocol = 1 / ComplexNumber
            ' Reciprocol = 1 / (A + Bi)
            ' Reciprocol = (A - Bi) / (A + Bi)(A - Bi)
            ' Reciprocol = (A - Bi) / (A^2 - ABi + ABi + Bi^2)
            ' Reciprocol = (A - Bi) / (A^2 + Bi^2)
            ' (1) Reciprocol = (A / (A^2 + B^2)) + (-B / (A^2 + B^2))i

            ' (2) G = R / (R^2 + X^2) ' (1)
            ' (3) B = -X / (R^2 + X^2) ' (1)
            ' (4) R = G / (G^2 + B^2) ' (1)
            ' (5) X = -B / (G^2 + B^2) ' (1)

            ' By definition of this approach:
            ' (6) Image1R = SourceR
            ' (7) Image2R = SourceR
            ' (8) Image3R = LoadR
            ' (9) Image4R = LoadR

            ' Basics:
            ' (10) LoadG = LoadR / (LoadR ^ 2 + LoadX ^ 2) ' (2)
            ' (11) LoadB = -LoadX / (LoadR ^ 2 + LoadX ^ 2) ' (3)
            ' (12) SourceG = SourceR / (SourceR ^ 2 + SourceX ^ 2) ' (2)
            ' (13) SourceB = -SourceX / (SourceR ^ 2 + SourceX ^ 2) ' (3)

            ' Also by definition of this approach:
            ' (14) Image1G = LoadG
            ' (15) LoadG = LoadG
            ' (16) Image3G = SourceG
            ' (17) SourceG = SourceG

            ' (18) Image1G = Image1R / (Image1R ^ 2 + Image1X ^ 2) ' (10)
            ' (19) (Image1R ^ 2 + Image1X ^ 2) = Image1R / Image1G
            ' (20) Image1X ^ 2 = (Image1R / Image1G) - Image1R ^ 2
            ' (21) Image1X = sqrt((Image1R / Image1G) - Image1R ^ 2)
            ' (22) Image2X = -Image1X

            ' (23) Image3X = sqrt((Image3R / Image3G) - Image3R ^ 2) ' (21)
            ' (24) Image4X = -Image3X ' (22)

            ' Reworked/simplified derivation:

            ' Reusables:
            ' SqrdLoadR = loadR * loadR
            ' SqrdSourceR = sourceR * sourceR

            ' LoadG = LoadR / (SqrdLoadR + (loadX * loadX))
            ' SourceG = SourceR / (SqrdSourceR + (sourceX * sourceX))

            ' Image1R = SourceR
            ' Image1X = sqrt((SourceR / LoadG) - SqrdSourceR)
            ' Image2R = SourceR
            ' Image2X = -Image1X
            ' Image3R = LoadR
            ' Image3X = sqrt((LoadR / SourceG) - SqrdLoadR)
            ' Image4R = LoadR
            ' Image4X = -Image3X

            ' Detailed implemention:

            ' These two implementations account for some trouble cases. On a
            ' Smith Chart, those would be cases where the paired circles do not
            ' intersect.

            ' Init/default the fixed-size list.
            images = New ImageImpedanceList

            Dim SourceR As System.Double = sourceZ.Resistance
            Dim SqrdSourceR As System.Double = SourceR * SourceR
            Dim LoadR As System.Double = loadZ.Resistance
            Dim SqrdLoadR As System.Double = LoadR * LoadR
            Dim LoadX As System.Double = loadZ.Reactance
            Dim SqrdLoadX As System.Double = LoadX * LoadX
            Dim Div As System.Double = SourceR * (SqrdLoadR + SqrdLoadX) / LoadR
            If Div >= SqrdSourceR Then

                Dim image0R As System.Double = SourceR
                Dim image0X As System.Double =
                    System.Math.Sqrt(Div - SqrdSourceR)
                images(0) = New Impedance(image0R, image0X)

                Dim image1R As System.Double = SourceR
                Dim image1X As System.Double = -image0X
                images(1) = New Impedance(image1R, image1X)

            End If

            Dim SourceX As System.Double = sourceZ.Reactance
            Dim SqrdSourceX As System.Double = SourceX * SourceX
            Dim SourceG As System.Double =
                SourceR / (SqrdSourceR + SqrdSourceX) ' (2)
            Div = LoadR / SourceG
            If Div >= SqrdLoadR Then

                Dim image2R As System.Double = LoadR
                Dim image2X As System.Double = System.Math.Sqrt(Div - SqrdLoadR)
                images(2) = New Impedance(image2R, image2X)

                Dim image3R As System.Double = LoadR
                Dim image3X As System.Double = -image2X
                images(3) = New Impedance(image3R, image3X)

            End If

        Catch ex As Exception
            Return False
        End Try
        Return True

    End Function ' TryLoadImageImpedancesList2

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, in
    ''' a system with the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance of the
    ''' system.</param>
    ''' <param name="oneImage">Specifies the image impedance to be used in the
    ''' process.</param>
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
    ''' This method is analogous to solutions done on a Smith Chart, which
    ''' attempts to find a match by first moving, on a G-circle, from the load
    ''' impedance to an image impedance, then moving, on an R-circle, from the
    ''' image impedance to the source impedance.
    ''' </remarks>
    ''' 
    '''
    Public Shared Function MatchArbFirstOnG2(ByVal z0 As System.Double,
        ByVal oneImage As Impedance,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        Dim CurrTransCount As System.Int32 = transformations.Length
        Dim Trans As New Transformation

        ' The intended process is to create an L-section. The (emulated) first
        ' move is on the LoadG-circle, from the load impedance to the image
        ' impedance and the second move is on the SourceR-circle, from the image
        ' impedance to the source impedance.

        ' If the load susceptance already matches the image susceptance, no
        ' transformation is needed to get to the image impedance.
        Dim ImageB As System.Double = oneImage.ToAdmittance.Susceptance
        Dim LoadB As System.Double = loadZ.ToAdmittance.Susceptance
        Dim DeltaX As System.Double
        If EqualEnoughZero(ImageB - LoadB, IMPDTOLERANCE0 * z0) Then

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
        Dim DeltaB As System.Double = ImageB -
            loadZ.ToAdmittance().Susceptance
        DeltaX = sourceZ.Reactance - oneImage.Reactance
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

    End Function ' MatchArbFirstOnG2

    ''' <summary>
    ''' Attempts to obtain a conjugate match from the specified load
    ''' <c>Impedance</c> to the specified arbitrary source <c>Impedance</c>, in
    ''' a system with the specified characteristic impedance.
    ''' </summary>
    ''' <param name="z0">Specifies the characteristic impedance of the
    ''' system.</param>
    ''' <param name="oneImage">Specifies the image impedance to be used in the
    ''' process.</param>
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
    ''' This method is analogous to solutions done on a Smith Chart, which
    ''' attempts to find a match by first moving, on an R-circle, from the load
    ''' impedance to an image impedance, then moving, on a G-circle, from the
    ''' image impedance to the source impedance.
    ''' </remarks>
    Public Shared Function MatchArbFirstOnR2(ByVal z0 As System.Double,
        ByVal oneImage As Impedance,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        Dim CurrTransCount As System.Int32 = transformations.Length
        Dim Trans As New Transformation

        ' The intended process is to create an L-section. The first move is on
        ' the LoadR-circle, from the load impedance to the image impedance and
        ' the second move is on the SourceG-circle, from the image impedance to
        ' the source impedance.

        ' If the load reactance already matches the image reactance, no
        ' transformation is needed to get to the image impedance.
        Dim ImageX As System.Double = oneImage.Reactance
        Dim LoadX As System.Double = loadZ.Reactance
        Dim DeltaB As System.Double
        Dim DeltaX As System.Double
        If EqualEnoughZero(ImageX - LoadX, IMPDTOLERANCE0 * z0) Then

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
        DeltaX = oneImage.Reactance - loadZ.Reactance
        DeltaB = sourceZ.ToAdmittance().Susceptance -
            oneImage.ToAdmittance().Susceptance
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

    End Function ' MatchArbFirstOnR2

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
    Public Shared Function MatchArbitrary2(
        ByVal mainCirc As SmithMainCircle,
        ByVal loadZ As Impedance, ByVal sourceZ As Impedance,
        ByRef transformations As Transformation()) _
        As System.Boolean

        ' REF: Smith Chart Full Presentation, page 26 shows a geometric approach
        ' to finding a match.
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

        ' Try each mathematical approach to finding a match.

        ' Identify the candidate image impedances.
        Dim ImageImpedances As New ImageImpedanceList
        If Not TryLoadImageImpedancesList2(loadZ, sourceZ, ImageImpedances) Then
            Return False
        End If

        ' There are now four image impedances. Some may indicate no
        ' intersection. THEY MAY NOT ALL BE UNIQUE?
        ' Try each image impedance in turn.
        Dim BadImpedance As New Impedance(BADIMPDVALUE, BADIMPDVALUE)
        Dim MainZ0 As System.Double = mainCirc.Z0
        If Not Impedance.EqualEnough(MainZ0, BadImpedance,
                                     ImageImpedances(0)) Then
            If Not MatchArbFirstOnG2(MainZ0, ImageImpedances(0), loadZ,
                                     sourceZ, transformations) Then

                Return False
            End If
        End If
        If Not Impedance.EqualEnough(MainZ0, BadImpedance,
                                     ImageImpedances(1)) Then
            If Not MatchArbFirstOnG2(MainZ0, ImageImpedances(1), loadZ,
                                     sourceZ, transformations) Then

                Return False
            End If
        End If
        If Not Impedance.EqualEnough(mainCirc.Z0, BadImpedance,
                                     ImageImpedances(2)) Then
            If Not MatchArbFirstOnR2(MainZ0, ImageImpedances(2), loadZ,
                                     sourceZ, transformations) Then

                Return False
            End If
        End If
        If Not Impedance.EqualEnough(mainCirc.Z0, BadImpedance,
                                     ImageImpedances(3)) Then
            If Not MatchArbFirstOnR2(MainZ0, ImageImpedances(3), loadZ,
                                     sourceZ, transformations) Then

                Return False
            End If
        End If

        ' THESE TESTS CAN BE DELETED OR SUPPRESSED AFTER ALL WORKS OK.
        For Each OneTrans As Transformation In transformations
            If Not loadZ.ValidateTransformation(mainCirc, sourceZ, OneTrans) Then
                Return False
            End If
        Next

        ' On getting this far,
        Return True

    End Function ' MatchArbitrary2

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
    Public Shared Function MatchArbitrary2(z0 As System.Double,
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
        Return MatchArbitrary2(MainCirc, loadZ, sourceZ, transformations)

    End Function ' MatchArbitrary2

End Structure ' Impedance
