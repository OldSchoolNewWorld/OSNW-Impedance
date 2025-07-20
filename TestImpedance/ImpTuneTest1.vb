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
        ' A: At the open circuit point on the right.

        <Fact>
        Public Sub TrySelectTuning_PositionA_Fails()
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
        ' B: At the short circuit point on the left.

        ' No specific tests needed?
        ' Case B is just a special case of case C, with X=0.0 but no impact?
    End Class ' TestTrySelectTuningLayoutB

    Public Class TestTrySelectTuningLayoutC
        ' C: Anywhere else on the outer circle. Zero resistance.

        <Fact>
        Public Sub TrySelectTuning_PositionC_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Z As New OSNW.Numerics.Impedance(0.0, 3.0)
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
            Assert.True(Z.TrySelectTuningLayout(1.0, transformations), "Tuning failed.")
            Assert.True(transformations.Length = 1, "Incorrect transformation count.")
            Assert.True(transformations(0).Style.Equals(TransformationStyles.None), "Incorrect transformation style")
        End Sub

    End Class ' TestTrySelectTuningLayoutD

    Public Class TestTrySelectTuningLayoutE
        ' E: On the R=Z0 circle.

        <Fact>
        Public Sub TrySelectTuning_PositionE1_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.0, 3.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            Assert.True(Z.TrySelectTuningLayout(1.0, transformations), "Tuning failed.")
            Assert.True(transformations.Length = 1, "Incorrect transformation count.")
            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntInd), "Incorrect transformation style")
            Assert.Equal(0.3, transformations(0).Value1)
        End Sub

        <Fact>
        Public Sub TrySelectTuning_PositionE2_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.0, -3.0)
            Dim transformations As Transformation() = Array.Empty(Of Transformation)
            Assert.True(Z.TrySelectTuningLayout(1.0, transformations), "Tuning failed.")
            Assert.True(transformations.Length = 1, "Incorrect transformation count.")
            Assert.True(transformations(0).Style.Equals(TransformationStyles.ShuntCap), "Incorrect transformation style")
            Assert.Equal(0.3, transformations(0).Value1)
        End Sub

    End Class ' TestTrySelectTuningLayoutE






    Public Class TestTrySelectTuningX
        ' Chart location cases:
        ' F: Inside the R=Z0 circle.
        ' G: On the G=Y0 circle.
        ' H: Inside the G=Y0 circle.
        ' I: In the top remainder.
        ' J: In the bottom remainder.

        <Fact>
        Public Sub TestTrySelectTuningLayoutAmris23()
            'Dim Z As New OSNW.Numerics.Impedance(0.2, 0.2)
            'Dim transformations As Transformation() = Array.Empty(Of Transformation)
            'Assert.True(Z.TrySelectTuningLayout(1.0, transformations), "Tuning failed.")
            'Assert.True(transformations.Length = 1, "Incorrect transformation count.")
            'Assert.True(transformations(0).Style.Equals(TransformationStyles.None), "Incorrect transformation style")
        End Sub

    End Class ' TestTrySelectTuningX

End Namespace ' TrySelectTuningLayoutTests
