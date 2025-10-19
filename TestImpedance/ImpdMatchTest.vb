Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit

' The terminology here relates to solving conjugate matches on a Smith Chart.

' Chart location cases:
' A: At the short circuit point. Omit - covered by B.
' B: Anywhere else on the perimeter. R=0.0.
' C: At the open circuit point on the right.
' D: At the center.
' On the R=Z0 circle.
' On R=Z0 circle, on the resonance line. Already covered by C or D.
' E: On R=Z0 circle, above resonance line. Only needs reactance.
' F: On R=Z0 circle, below resonance line. Only needs reactance.
' Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
' G1: Inside R=Z0 circle, above resonance line.
' G2: Inside R=Z0 circle, above resonance line. Z0=50.
' H: Inside R=Z0 circle, on line.
' I: Inside R=Z0 circle, below resonance line.
' On the G=Y0 circle.
' On G=Y0 circle, on resonance line. Omit - already either A or D.
' J: On G=Y0 circle, above resonance line. Only needs reactance.
' K: On G=Y0 circle, below resonance line. Only needs reactance.
' Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
' L1: Inside G=Y0 circle, above resonance line.
' L2: Inside G=Y0 circle, above resonance line. Z0=75.
' M: Inside G=Y0 circle, on line.
' N: Inside G=Y0 circle, below line.
' O1: In the top center.
' O2: In the top center. Z0=50.
' P1: In the bottom center.
' P2: In the bottom center. Z0=50.
' Q: Outside of main circle. Invalid.
' R: NormR<=0. Invalid.

Class Messages
    Public Const TF As String = "Matching failed."
    Public Const ITC As String = "Incorrect transformation count."
    Public Const ITS As String = "Incorrect transformation style"
End Class ' Messages

Namespace TrySelectMatchLayoutTests

    Public Class TestTrySelectMatchLayoutB
        ' A: At the short circuit point. Omit; Covered by B.
        ' B: Anywhere else on the perimeter. R=0.0.

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0)> ' B: Anywhere else on the perimeter. R=0.0.
        Public Sub TrySelectMatch_PositionBZeroR_Fails(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)

            ' This version, with R=0, does not throw an exception when R=0 is
            ' allowed by Impedance.New(), but it does fail to match.
            Assert.False(Z.TrySelectMatchLayout(MainCirc, transformations))
            Assert.True(transformations Is Nothing)

        End Sub

    End Class ' TestTrySelectMatchLayoutB

    Public Class TestTrySelectMatchLayoutC
        ' C: At the open circuit point on the right.

        Const INF As Double = Double.PositiveInfinity

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, INF, 0.0000)> ' C: At the open circuit point on the right.
        Public Sub TrySelectMatch_PositionC_Fails(z0 As Double, r As Double, x As Double)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
                    'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
                    Dim Z As New OSNW.Numerics.Impedance(r, x)
                    Dim transformations As Transformation() = Array.Empty(Of Transformation)
                    Assert.False(Z.TrySelectMatchLayout(MainCirc, transformations))
                End Sub)
        End Sub

    End Class ' TestTrySelectMatchLayoutC

    Public Class TestTrySelectMatchLayoutD
        ' D: At the center.

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000)> ' D: At the center.
        Public Sub TestTrySelectMatchLayoutD(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)

            Assert.True(Z.TrySelectMatchLayout(MainCirc, transformations), Messages.TF)
            Assert.True(transformations.Length = 0, Messages.ITC)

        End Sub

    End Class ' TestTrySelectMatchLayoutD

    Public Class TestTrySelectMatchLayoutEF
        ' On the R=Z0 circle.
        ' On R=Z0 circle, on the resonance line. Already covered by C or D.
        ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        ' F: On R=Z0 circle, below resonance line. Only needs reactance.

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 1.0)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        Public Sub TrySelectMatch_PositionE_Succeeds(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddSeriesImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.SeriesCap), Messages.ITS)
            Assert.Equal(-x, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, -2.0)> ' F: On R=Z0 circle, below resonance line. Only needs reactance.
        Public Sub TrySelectMatch_PositionF_Succeeds(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddSeriesImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.SeriesInd), Messages.ITS)
            Assert.Equal(-x, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

    End Class ' TestTrySelectMatchLayoutEF

    Public Class TestTrySelectMatchLayoutJK
        ' On the G=Y0 circle.
        ' On G=Y0 circle, on resonance line. Omit - already either A or D.
        ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        ' K: On G=Y0 circle, below resonance line. Only needs reactance.

        '<InlineData(    Z0,      G,       B)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, -1.0)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        Public Sub TrySelectMatch_PositionJ_Succeeds(z0 As Double, g As Double, b As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestY As New OSNW.Numerics.Admittance(g, b)
            Dim TestZ As OSNW.Numerics.Impedance = TestY.ToImpedance

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddShuntImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntCap), Messages.ITS)
            Assert.Equal(-1, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

        '<InlineData(    Z0,      G,       B)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 1.0)> ' K: On G=Y0 circle, below resonance line. Only needs reactance.
        Public Sub TrySelectMatch_PositionK_Succeeds(z0 As Double, g As Double, b As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestY As New OSNW.Numerics.Admittance(g, b)
            Dim TestZ As OSNW.Numerics.Impedance = TestY.ToImpedance

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddShuntImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntInd), Messages.ITS)
            Assert.Equal(1, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

    End Class ' TestTrySelectMatchLayoutJK

    Public Class TestTrySelectMatchLayoutGHI
        ' GHI: Inside the R=Z0 circle.

        '<InlineData(     Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 2.0, 1 / 2.0)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0)> ' G2: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000)> ' H: Inside R=Z0 circle, on line.
        <InlineData(1.0, 2.0, -2.0)> ' I: Inside R=Z0 circle, below resonance line.
        Public Sub TrySelectMatch_PositionGHI_Succeeds(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Assert.True(True)

        End Sub

        '<Theory>
        'Public Sub TrySelectMatch_PositionGHI_Fails()
        '    '
        '    '
        '    '
        '    '
        '    '
        'End Sub

    End Class ' TestTrySelectMatchLayoutGHI

    Public Class TestTrySelectMatchLayoutLMN
        ' LMN: Inside the G=Y0 circle.

        '<InlineData(     Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 1 / 3.0, 1 / 3.0)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000)> ' M: Inside G=Y0 circle, on line.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0)> ' N: Inside G=Y0 circle, below line.
        Public Sub TrySelectMatch_PositionLMN_Succeeds(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Assert.True(True)

        End Sub

        '<Theory>
        'Public Sub TrySelectMatch_PositionLMN_Fails()
        '    '
        '    '
        '    '
        '    '
        '    '
        'End Sub

    End Class ' TestTrySelectMatchLayoutLMN

    Public Class TestTrySelectMatchO
        ' O: In the top center.

        '<InlineData(     Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 0.2, 1.4)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0)> ' O2: In the top center. Z0=50.
        Public Sub TestTrySelectMatchLayoutO(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Assert.True(True)

        End Sub

    End Class ' TestTrySelectMatchO

    Public Class TestTrySelectMatchP
        ' P: In the bottom center.

        '<InlineData(     Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 0.4, -0.8)> ' P: In the bottom center.
        <InlineData(50.0, 20.0, -40.0)> ' P: In the bottom center.
        Public Sub TestTrySelectMatchLayoutP(z0 As Double, r As Double, x As Double)

            Dim MainCirc As New SmithMainCircle(4.0, 5.0, 4.0, z0) ' Test data.
            'Dim MainCirc As New SmithMainCircle(1.0, 1.0, 1.0, z0) ' Arbitrary.
            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectMatchLayout(MainCirc, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Assert.True(True)

        End Sub

    End Class ' TestTrySelectMatchP

End Namespace ' TrySelectMatchLayoutTests
