Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

' This document contains items related to matching a load impedance to an
' arbitrary source impedance.

''' <summary>
''' A list of four <see cref="Impedance"/> objects representing the image
''' impedances to potentially be used in matching a load <c>Impedance</c> to
''' an arbitrary source <c>Impedance</c>.
''' </summary>
''' <remarks>
''' This class is a fixed-size list of four <see cref="Impedance"/> structures,
''' each of which serves a specific role. The four slots in the list correspond
''' to the following image impedances:
''' <list type="bullet">
''' <item><term>Items 0 and 1</term>
''' <description>hold the image impedances used to perform the equivalent of
''' moving, on a Smith Chart, along a G-circle from the load impedance to the
''' image impedance, followed by moving along an R-circle to the source
''' impedance.</description>
''' </item>
''' <item><term>Items 2 and 3</term>
''' <description>hold the image impedances used to perform the equivalent of
''' moving, on a Smith Chart, along an R-circle from the load impedance to the
''' image impedance, followed by moving along a G-circle to the source
''' impedance.</description>
''' </item>
''' </list>
''' </remarks>
Public Class ImageImpedanceList
    Inherits System.Collections.Generic.List(Of Impedance)
    Implements System.Collections.IList

    ' Ref:System.Collections.IList Interface
    ' https://learn.microsoft.com/en-us/dotnet/api/system.collections.Collections.IList?view=net-10.0

    ''' <summary>
    ''' Defines the fixed capacity of the <c>ImageImpedanceList</c>.
    ''' </summary>
    Private Const FIXEDCAPACITY As System.Int32 = 4

    '    Private ReadOnly _contents(DEFAULTCAPACITY - 1) As System.Object
    '    Private ReadOnly _count As System.Int32

    'System.Collections.IList members.

    'Public Function Add(ByVal value As System.Object) As System.Int32 Implements System.Collections.IList.Add
    '    If _count < _contents.Length Then
    '        _contents(_count) = value
    '        _count += 1

    '        Return _count - 1
    '    End If

    '    Return -1
    'End Function

    'Public Sub Clear() Implements System.Collections.IList.Clear
    '    _count = 0
    'End Sub

    'Public Overloads Function Contains(ByVal value As System.Object) _
    '    As System.Boolean _
    '    Implements System.Collections.IList.Contains
    ' xxxxxxxxxx THIS WOULD LIKELY NEED TO CHANGE EQUALITY TO EQUALENOUGH

    '    Dim ValImpedance As Impedance = CType(value, Impedance)
    '    For i As System.Int32 = 0 To Count - 1
    '        If _contents(i).Equals(ValImpedance) Then Return True
    '    Next

    '    Return False
    'End Function

    'Public Overloads Function IndexOf(ByVal value As System.Object) _
    '    As System.Int32 _
    '    Implements System.Collections.IList.IndexOf
    ' xxxxxxxxxx THIS WOULD LIKELY NEED TO CHANGE EQUALITY TO EQUALENOUGH

    '    Dim ValImpedance As Impedance = CType(value, Impedance)
    '    For i As System.Int32 = 0 To Count - 1
    '        If _contents(i).Equals(ValImpedance) Then Return i
    '    Next
    '    Return -1
    'End Function

    'Public Sub Insert(ByVal index As System.Int32, ByVal value As System.Object) Implements System.Collections.IList.Insert

    '    If _count + 1 <= _contents.Length AndAlso index < Count AndAlso index >= 0 Then
    '        _count += 1

    '        For i As System.Int32 = Count - 1 To index Step -1
    '            _contents(i) = _contents(i - 1)
    '        Next
    '        _contents(index) = value
    '    End If
    'End Sub

    ''' <summary>
    ''' Gets a value indicating that the <c>ImageImpedanceList</c> has a fixed
    ''' size. It is fixed-size because the Z=0+j0 impedances are used to
    ''' indicate that no usable image impedance was created for a particular
    ''' slot.
    ''' </summary>
    ''' <returns>
    ''' This property always returns <c>True</c>.
    ''' </returns>
    Public ReadOnly Property IsFixedSize() As System.Boolean _
        Implements System.Collections.IList.IsFixedSize

        Get
            Return True
        End Get
    End Property

    ''' <summary>
    ''' Gets a value indicating that the <c>ImageImpedanceList</c> is not
    ''' read-only.
    ''' </summary>
    ''' This property always returns <c>False</c>.
    Public ReadOnly Property IsReadOnly() As System.Boolean _
        Implements System.Collections.IList.IsReadOnly

        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Improperly attempts to remove the first occurrence of a specific object
    ''' from the <c>ImageImpedanceList</c>.
    ''' </summary>
    ''' <param name="value">Ignored due to the violation.</param>
    ''' <exception cref="System.NotSupportedException">
    ''' Always thrown, because the <c>ImageImpedanceList</c> has a fixed size.
    ''' </exception>"
    Public Overloads Sub Remove(ByVal value As System.Object) _
        Implements System.Collections.IList.Remove

        Dim CaughtBy As System.Reflection.MethodBase =
            System.Reflection.MethodBase.GetCurrentMethod
        Throw New System.NotSupportedException(
            $"{CaughtBy} {Impedance.MSGFIXEDSIZEVIOLATION}")
    End Sub ' Remove

    ''' <summary>
    ''' Improperly attempts to remove the <c>ImageImpedanceList</c> item at the
    ''' specified index.
    ''' </summary>
    ''' <param name="index">Ignored due to the violation.</param>
    ''' <exception cref="System.NotSupportedException">
    ''' Always thrown, because the <c>ImageImpedanceList</c> has a fixed size.
    ''' </exception>"
    Public Overloads Sub RemoveAt(ByVal index As System.Int32) _
        Implements System.Collections.IList.RemoveAt

        Dim CaughtBy As System.Reflection.MethodBase =
            System.Reflection.MethodBase.GetCurrentMethod
        Throw New System.NotSupportedException(
            $"{CaughtBy} {Impedance.MSGFIXEDSIZEVIOLATION}")
    End Sub ' RemoveAt

    '''' <summary>
    '''' xxxxxxxxxx
    '''' </summary>
    '''' <param name="index">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    'Public Function RemoveAll() As System.Int32 Implements System.Collections.IList.removeall

    '    Me.RemoveAll()

    'End Function
    'xxxx

    '       ' public int RemoveAll(Predicate<T> match);
    '''' <summary>
    '''' Improperly attempts to remove all the elements that match the conditions defined by the specified predicate.
    '''' </summary>
    '''' <param name="match">Ignored System.Predicate(Of T) delegate that defines the conditions of the elements to remove.</param>
    '''' <returns>xxxxxxxxxx</returns>
    'Public Overloads Function RemoveAll(match As Predicate<T> match) As System.Int32
    '       ' Implements System.Collections.IList.removeall

    '       Me.RemoveAll()


    '   End Function
    '   xxxx

    'Public Overloads Property Item(ByVal index As System.Int32) As System.Object _
    '    Implements System.Collections.IList.Item

    '    Get
    '        Return _contents(index)
    '    End Get
    '    Set(ByVal value As System.Object)
    '        _contents(index) = value
    '    End Set
    'End Property

    ' ICollection members.

    'Public Sub CopyTo(ByVal array As Array, ByVal index As System.Int32) Implements ICollection.CopyTo
    '    For i As System.Int32 = 0 To Count - 1
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
    'Public ReadOnly Property SyncRoot() As System.Object Implements ICollection.SyncRoot
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

    '    For i As System.Int32 = 0 To Count - 1
    '        Console.Write($" {_contents(i)}")
    '    Next

    '    Console.WriteLine()
    'End Sub

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
    'Public Shared Shadows Function Add(value As System.Object) As System.Int32
    '    Dim CaughtBy As System.Reflection.MethodBase =
    '        System.Reflection.MethodBase.GetCurrentMethod
    '    Throw New System.NotSupportedException(String.Concat(
    '        $"Failed To process {CaughtBy}.",
    '        $" {value} is not an Impedance.",
    '        " Use Add(Impedance)"))
    'End Function ' Add

    ''' <summary>
    ''' Initializes a new instance of the <c>ImageImpedanceList</c> class that
    ''' contains Z=0+j0 impedances and has the specified initial capacity.
    ''' </summary>
    ''' <param name="capacity">Specifies the number of elements that the new
    ''' list can initially store.</param>
    ''' <remarks>The list is initialized to contain a full set of Z=0+j0
    ''' entries. Those values can be checked later, as done in MatchArbitrary(),
    ''' to determine whether usable per-slot image impedances were
    ''' created.</remarks>
    Public Sub New(capacity As System.Int32)
        MyBase.New(capacity)
        For I As System.Int32 = 0 To capacity - 1
            ' The Nothing value creates an Impedance with R=0.0 and X=0.0. That
            ' cannot be done with New Impedance(), because that would throw an
            ' exception.
            Me.Add(Nothing)
        Next
    End Sub ' New

    ''' <summary>
    ''' Initializes a new instance of the <c>ImageImpedanceList</c> class that
    ''' contains <see cref="FIXEDCAPACITY"/> number of Z=0+j0 impedances.
    ''' </summary>
    Public Sub New()

        '    Dim CaughtBy As System.Reflection.MethodBase =
        '        System.Reflection.MethodBase.GetCurrentMethod
        '    Throw New System.NotSupportedException(String.Concat(
        '        $"{CaughtBy} called directly. Use New(capacity)"))

        Me.New(FIXEDCAPACITY)
    End Sub ' New

End Class ' ImageImpedanceList

Partial Public Structure Impedance

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

            Else
                images(0) = Nothing
                images(1) = Nothing
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

            Else
                images(2) = Nothing
                images(3) = Nothing
            End If

            'xxxxxxxxxxxxxx
            System.Diagnostics.Debug.WriteLine($"Images: {images(0)}; {images(0)}; {images(2)}; {images(3)}")
            'xxxxxxxxxxxxxx

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
    Public Shared Function MatchArbFirstOnG(ByVal z0 As System.Double,
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
        If EqualEnoughZero(ImageB - LoadB, DFLTIMPDTOLERANCE0 * z0) Then

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
            Else ' DeltaB >= 0.0
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
    Public Shared Function MatchArbFirstOnR(ByVal z0 As System.Double,
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
        If EqualEnoughZero(ImageX - LoadX, DFLTIMPDTOLERANCE0 * z0) Then

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
            Else ' DeltaX >= 0.0
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
        ' Try each image impedance in turn. No action needed when no
        ' ImageImpedance was discovered.
        Dim MainZ0 As System.Double = mainCirc.Z0
        If Not ImageImpedances(0).IsZero Then
            If Not MatchArbFirstOnG(MainZ0, ImageImpedances(0), loadZ,
                                    sourceZ, transformations) Then

                Return False
            End If
        End If
        If Not ImageImpedances(1).IsZero Then
            If Not MatchArbFirstOnG(MainZ0, ImageImpedances(1), loadZ,
                                    sourceZ, transformations) Then

                Return False
            End If
        End If
        If Not ImageImpedances(2).IsZero Then
            If Not MatchArbFirstOnR(MainZ0, ImageImpedances(2), loadZ,
                                    sourceZ, transformations) Then

                Return False
            End If
        End If
        If Not ImageImpedances(3).IsZero Then
            If Not MatchArbFirstOnR(MainZ0, ImageImpedances(3), loadZ,
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
