Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off
Imports OSNW.Numerics
Imports Xunit



' The terminology here relates to solving conjugate matches on a Smith
' chart.

' Chart location cases:
' A: At the open circuit point on the right.
' B: At the short circuit point on the left.
' C: Anywhere else on the outer circle. Zero resistance.
' D: At the center.
' E: On the R=Z0 circle.
' F: Inside the R=Z0 circle.
' G: On the G=Y0 circle.
' H: Inside the G=Y0 circle.
' I: In the top remainder.
' J: In the bottom remainder.

Namespace TrySelectTuningLayoutTests

    Public Class TestTrySelectTuningLayoutA

        <Fact>
        Public Sub PositionA_OpenCircuit_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Z As New OSNW.Numerics.Impedance(Double.PositiveInfinity, 3.0)
                    Dim transformations As Transformation() = Array.Empty(Of Transformation)
                    Assert.False(Z.TrySelectTuningLayout(1.0, transformations))
                End Sub)
        End Sub

    End Class ' TestTrySelectTuningLayoutA

    Public Class TestTrySelectTuningLayoutB

        <Fact>
        Public Sub PositionB_ShortCircuit_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Z As New OSNW.Numerics.Impedance(0.0, 3.0)
                    Dim transformations As Transformation() = Array.Empty(Of Transformation)
                    Assert.False(Z.TrySelectTuningLayout(1.0, transformations))
                End Sub)
        End Sub

    End Class ' TestTrySelectTuningLayoutB

    Public Class TestTrySelectTuningLayoutC

        <Fact>
        Public Sub PositionC_AtCenter_Passes()
            Dim Z As New OSNW.Numerics.Impedance(1.0, 0.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            Assert.True(Z.TrySelectTuningLayout(1.0, transformations))
            Assert.True(transformations.Length = 1)
            Assert.True(transformations(0).Style.Equals(TransformationStyles.None))
        End Sub

    End Class ' TestTrySelectTuningLayoutC

End Namespace ' TrySelectTuningLayoutTests
