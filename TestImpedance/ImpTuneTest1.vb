Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off
Imports OSNW.Numerics
Imports Xunit


' The terminology here relates to solving conjugate matches on a Smith
' chart.

' Chart location cases:
' A: At the short circuit point on the left. Omit; Covered by B.
' B: Anywhere else on the outer circle. R=0.0.
' C: At the open circuit point on the right.
' D: At the center.
' E: On the R=Z0 circle.
'      Omit: On the resonance line. Already covered by C or D.
'      E1: Above the resonance line. Only needs reactance.
'      E2: Below the resonance line. Only needs reactance.
' F: Inside the R=Z0 circle. Two choices: CW or CCW on the G circle.
' G: On the G=Y0 circle.
'      Omit: On the resonance line. Already either B or D.
'      G1: Above the resonance line. Only needs reactance.
'      G2: Below the resonance line. Only needs reactance.
' H: Inside the G=Y0 circle. Two choices: CW or CCW on the R circle.
' I: In the top remainder.
' J: In the bottom remainder.

Class Messages
    Public Const TF As String = "Tuning failed."
    Public Const ITC As String = "Incorrect transformation count."
    Public Const ITS As String = "Incorrect transformation style"
End Class ' Messages

Namespace TrySelectTuningLayoutTests

    Public Class TestTrySelectTuningLayoutA
        ' A: At the short circuit point on the left. Omit; Covered by B.

        <Fact>
        Public Sub TrySelectTuning_PositionA_Fails()

            Dim Z As New OSNW.Numerics.Impedance(0.0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)

            ' This version, with R=0, X=0, does not throw an exception when R=0 is
            ' allowed by Impedance.New(), but it does fail to tune.
            Assert.False(Z.TrySelectTuningLayout(1.0, transformations))
            Assert.True(transformations Is Nothing)

        End Sub

    End Class ' TestTrySelectTuningLayoutA

    Public Class TestTrySelectTuningLayoutB
        ' B: Anywhere else on the outer circle. R=0.0.

        <Fact>
        Public Sub TrySelectTuning_PositionBZeroR_Fails()

            Dim Z As New OSNW.Numerics.Impedance(0.0, 3.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)

            ' This version, with R=0, does not throw an exception when R=0 is
            ' allowed by Impedance.New(), but it does fail to tune.
            Assert.False(Z.TrySelectTuningLayout(1.0, transformations))
            Assert.True(transformations Is Nothing)

        End Sub

    End Class ' TestTrySelectTuningLayoutB

    Public Class TestTrySelectTuningLayoutC
        ' C: At the open circuit point on the right.

        <Fact>
        Public Sub TrySelectTuning_PositionC_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception.
                    Dim Z As New OSNW.Numerics.Impedance(Double.PositiveInfinity, 0.0)
                    Dim transformations As Transformation() = Array.Empty(Of Transformation)
                    Assert.False(Z.TrySelectTuningLayout(1.0, transformations))
                End Sub)
        End Sub

    End Class ' TestTrySelectTuningLayoutC

    Public Class TestTrySelectTuningLayoutD
        ' D: At the center.

        <Fact>
        Public Sub TestTrySelectTuningLayoutD()

            Dim Z As New OSNW.Numerics.Impedance(1.0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)

            Assert.True(Z.TrySelectTuningLayout(1.0, transformations), Messages.TF)
            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.None), Messages.ITS)

        End Sub

    End Class ' TestTrySelectTuningLayoutD

    Public Class TestTrySelectTuningLayoutE
        ' E: On the R=Z0 circle.
        '     Omit: On the resonance line. Already covered by C or D.
        '     E1: Above the resonance line. Only needs reactance.
        '     E2: Below the resonance line. Only needs reactance.

        <Fact>
        Public Sub TrySelectTuning_PositionE1_Succeeds()

            Dim TestZ As New OSNW.Numerics.Impedance(1.0, 3.0)
            Dim TestZ0 As Double = 1.0

            Dim TargetZ As New Impedance(TestZ0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(TestZ0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddSeriesImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.SeriesCap), Messages.ITS)
            Assert.Equal(-3.0, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

        <Fact>
        Public Sub TrySelectTuning_PositionE2_Succeeds()

            Dim TestZ As New OSNW.Numerics.Impedance(1.0, -3.0)
            Dim TestZ0 As Double = 1.0

            Dim TargetZ As New Impedance(TestZ0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(TestZ0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddSeriesImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.SeriesInd), Messages.ITS)
            Assert.Equal(3.0, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

    End Class ' TestTrySelectTuningLayoutE

    Public Class TestTrySelectTuningLayoutG
        ' G: On the G=Y0 circle.
        '     Omit: On the resonance line. Already either B or D.
        '     G1: Above the resonance line. Only needs reactance.
        '     G2: Below the resonance line. Only needs reactance.

        <Fact>
        Public Sub TrySelectTuning_PositionG1_Succeeds()

            Dim TestY As New OSNW.Numerics.Admittance(1.0, 3.0)
            Dim TestZ As OSNW.Numerics.Impedance = TestY.ToImpedance
            Dim TestZ0 As Double = 1.0

            Dim TargetZ As New Impedance(TestZ0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(TestZ0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddShuntImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntCap), Messages.ITS)
            Assert.Equal(1.0 / 3.0, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

        <Fact>
        Public Sub TrySelectTuning_PositionG2_Succeeds()

            Dim TestY As New OSNW.Numerics.Admittance(1.0, -3.0)
            Dim TestZ As OSNW.Numerics.Impedance = TestY.ToImpedance
            Dim TestZ0 As Double = 1.0

            Dim TargetZ As New Impedance(TestZ0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            If Not TestZ.TrySelectTuningLayout(TestZ0, transformations) Then
                Assert.True(False, Messages.TF)
            End If
            Dim AddZ As New Impedance(0.0, transformations(0).Value1)
            Dim CombinedZ As Impedance = Impedance.AddShuntImpedance(TestZ, AddZ)

            Assert.True(transformations.Length = 1, Messages.ITC)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntInd), Messages.ITS)
            Assert.Equal(-1.0 / 3.0, transformations(0).Value1)
            Assert.Equal(TargetZ, CombinedZ)

        End Sub

    End Class ' TestTrySelectTuningLayoutG

    Public Class TestTrySelectTuningX
        ' Chart location cases:
        ' F: Inside the R=Z0 circle. Two choices: CW or CCW on the G circle.
        ' H: Inside the G=Y0 circle. Two choices: CW or CCW on the R circle.
        ' I: In the top remainder.
        ' J: In the bottom remainder.

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
