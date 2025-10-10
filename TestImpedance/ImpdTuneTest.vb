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
' G2: Inside R=Z0 circle, above resonance line, Z0=50
' H: Inside R=Z0 circle, on line
' I: Inside R=Z0 circle, below resonance line.
' On the G=Y0 circle.
' On G=Y0 circle, on resonance line. Omit - already either A or D.
' J: On G=Y0 circle, above resonance line. Only needs reactance.
' K: On G=Y0 circle, below resonance line. Only needs reactance.
' Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
' L1: Inside G=Y0 circle, above resonance line.
' L2: Inside G=Y0 circle, above resonance line. Z0=75.
' M: Inside G=Y0 circle, on line
' N: Inside G=Y0 circle, below line
' O: In the top remainder.
' P: In the bottom remainder.
' Q: Outside of main circle. Invalid.
' R: NormR<=0. Invalid.

Class Messages
    Public Const TF As String = "Tuning failed."
    Public Const ITC As String = "Incorrect transformation count."
    Public Const ITS As String = "Incorrect transformation style"
End Class ' Messages

Namespace TrySelectTuningLayoutTests

    Public Class TestTrySelectTuningLayoutB
        ' A: At the short circuit point. Omit; Covered by B.
        ' B: Anywhere else on the perimeter. R=0.0.

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(1.0, 0.0000, 1 / 2.0)> ' B: Anywhere else on the perimeter. R=0.0.
        Public Sub TrySelectTuning_PositionBZeroR_Fails(z0 As Double, r As Double, x As Double)

            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)

            ' This version, with R=0, does not throw an exception when R=0 is
            ' allowed by Impedance.New(), but it does fail to tune.
            Assert.False(Z.TrySelectTuningLayout(z0, transformations))
            Assert.True(transformations Is Nothing)

        End Sub

    End Class ' TestTrySelectTuningLayoutB

    Public Class TestTrySelectTuningLayoutC
        ' C: At the open circuit point on the right.

        Const INF As Double = Double.PositiveInfinity

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, INF, 0.0000)> ' C: At the open circuit point on the right.
        Public Sub TrySelectTuning_PositionC_Fails(z0 As Double, r As Double, x As Double)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Z As New OSNW.Numerics.Impedance(r, x)
                    Dim transformations As Transformation() = Array.Empty(Of Transformation)
                    Assert.False(Z.TrySelectTuningLayout(z0, transformations))
                End Sub)
        End Sub

    End Class ' TestTrySelectTuningLayoutC

    Public Class TestTrySelectTuningLayoutD
        ' D: At the center.

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000)> ' D: At the center.
        Public Sub TestTrySelectTuningLayoutD(z0 As Double, r As Double, x As Double)

            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)

            Assert.True(Z.TrySelectTuningLayout(z0, transformations), Messages.TF)
            Assert.True(transformations.Length = 0, Messages.ITC)

        End Sub

    End Class ' TestTrySelectTuningLayoutD

    Public Class TestTrySelectTuningLayoutEF
        ' On the R=Z0 circle.
        ' On R=Z0 circle, on the resonance line. Already covered by C or D.
        ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        ' F: On R=Z0 circle, below resonance line. Only needs reactance.

        '<InlineData(    Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 1.0)> ' E: On R=Z0 circle, above resonance line. Only needs reactance.
        Public Sub TrySelectTuning_PositionE_Succeeds(z0 As Double, r As Double, x As Double)

            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(z0, transformations) Then
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
        Public Sub TrySelectTuning_PositionF_Succeeds(z0 As Double, r As Double, x As Double)

            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(z0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddSeriesImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.SeriesInd), Messages.ITS)
            Assert.Equal(-x, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

    End Class ' TestTrySelectTuningLayoutEF

    Public Class TestTrySelectTuningLayoutJK
        ' On the G=Y0 circle.
        ' On G=Y0 circle, on resonance line. Omit - already either A or D.
        ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        ' K: On G=Y0 circle, below resonance line. Only needs reactance.

        '<InlineData(    Z0,      G,       B)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, -1.0)> ' J: On G=Y0 circle, above resonance line. Only needs reactance.
        Public Sub TrySelectTuning_PositionJ_Succeeds(z0 As Double, g As Double, b As Double)

            Dim TestY As New OSNW.Numerics.Admittance(g, b)
            Dim TestZ As OSNW.Numerics.Impedance = TestY.ToImpedance

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(z0, transformations) Then
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
        Public Sub TrySelectTuning_PositionK_Succeeds(z0 As Double, g As Double, b As Double)

            Dim TestY As New OSNW.Numerics.Admittance(g, b)
            Dim TestZ As OSNW.Numerics.Impedance = TestY.ToImpedance

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(z0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddShuntImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntInd), Messages.ITS)
            Assert.Equal(1, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

    End Class ' TestTrySelectTuningLayoutJK

    Public Class TestTrySelectTuningLayoutGHI
        ' GHI: Inside the R=Z0 circle.

        '<InlineData(     Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 2.0, 1 / 2.0)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0)> ' G2: Inside R=Z0 circle, above resonance line, Z0=50
        <InlineData(1.0, 3.0, 0.0000)> ' H: Inside R=Z0 circle, on line
        <InlineData(1.0, 2.0, -2.0)> ' I: Inside R=Z0 circle, below resonance line.
        Public Sub TrySelectTuning_PositionGHI_Succeeds(z0 As Double, r As Double, x As Double)

            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(z0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Assert.True(True)

        End Sub

        '<Theory>
        'Public Sub TrySelectTuning_PositionGHI_Fails()
        '    '
        '    '
        '    '
        '    '
        '    '
        'End Sub

    End Class ' TestTrySelectTuningLayoutGHI

    Public Class TestTrySelectTuningLayoutLMN
        ' LMN: Inside the G=Y0 circle.

        '<InlineData(     Z0,        R,       X)> ' Model
        <Theory>
        <InlineData(1.0, 1 / 3.0, 1 / 3.0)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0)> ' L2: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000)> ' M: Inside G=Y0 circle, on line
        <InlineData(1.0, 1 / 2.0, -1 / 3.0)> ' N: Inside G=Y0 circle, below line
        Public Sub TrySelectTuning_PositionLMN_Succeeds(z0 As Double, r As Double, x As Double)

            Dim TestZ As New OSNW.Numerics.Impedance(r, x)

            Dim TargetZ As New Impedance(z0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(z0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Assert.True(True)

        End Sub

        '<Theory>
        'Public Sub TrySelectTuning_PositionLMN_Fails()
        '    '
        '    '
        '    '
        '    '
        '    '
        'End Sub

    End Class ' TestTrySelectTuningLayoutLMN

    Public Class TestTrySelectTuningX
        ' Chart location cases:
        ' O: In the top remainder.
        ' P: In the bottom remainder.

        <Fact>
        Public Sub TestTrySelectTuningLayoutAmris23()
            'Dim Z As New OSNW.Numerics.Impedance(0.2, 0.2)
            'Dim transformations As Transformation() = Array.Empty(Of Transformation)
            'Assert.True(Z.TrySelectTuningLayout(1.0, transformations), Messages.TF)
            'Assert.True(transformations.Length = 1, Messages.ITC)
            'Assert.True(transformations(0).Style.Equals(TransformationStyles.None), Messages.ITS)
        End Sub

    End Class ' TestTrySelectTuningX

End Namespace ' TrySelectTuningLayoutTests
