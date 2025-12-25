Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports OSNW.Numerics
Imports Xunit

' The terminology here relates to solving conjugate matches on a Smith Chart.

' Chart location cases:
' A: At the short circuit point.
' B: Anywhere else on the perimeter. R=0.0.
' C: At the open circuit point on the right.
' D: At the center.
' On the R=Z0 circle.
'   On R=Z0 circle, on the resonance line. Already covered by C or D.
'   E1: On R=Z0 circle, above resonance line.
'   F1: On R=Z0 circle, below resonance line.
' Inside the R=Z0 circle. Two choices: CW or CCW on the G-circle.
'   G1: Inside R=Z0 circle, above resonance line.
'   G50: Inside R=Z0 circle, above resonance line. Z0=50.
'   H1: Inside R=Z0 circle, on line.
'   I1: Inside R=Z0 circle, below resonance line.
' On the G=Y0 circle.
'   On G=Y0 circle, on resonance line. Omit - already either A or D.
'   J1: On G=Y0 circle, above resonance line.
'   K1: On G=Y0 circle, below resonance line.
' Inside the G=Y0 circle. Two choices: CW or CCW on the R-circle.
' L1: Inside G=Y0 circle, above resonance line.
' L75: Inside G=Y0 circle, above resonance line. Z0=75.
' M1: Inside G=Y0 circle, on line.
' N1: Inside G=Y0 circle, below line.
' O1: In the top center.
' O50: In the top center. Z0=50.
' P1: In the bottom center.
' P50: In the bottom center. Z0=50.
' Q: Outside of main circle. Invalid.
' R: NormR<=0. Invalid.

Class Messages
    Public Const TF As String = "Matching failed."
    Public Const ITC As String = "Incorrect transformation count."
    Public Const ITS As String = "Incorrect transformation style"
End Class ' Messages

Namespace MatchArbitraryTests

    Public Class TestMatchArbitrary_UnlikelyZ0

        Const INF As Double = Double.PositiveInfinity

        '<InlineData(  Z0,        R,         X)> ' Model
        <Theory>
        <InlineData(-1.0, 1.0, 0.0000)> ' Negative Z0. Invalid.
        <InlineData(1.0, INF, 0.0000)> ' C: At the open circuit point on the right.
        <InlineData(1.0, -0.0345, 0.4138)> ' Q: Outside of main circle. Invalid.
        <InlineData(1.0, -2.0, 999)> ' R: NormR<=0. Invalid.
        Public Sub MatchArbitraryZ0_BadInput_ThrowsException(z0 As System.Double, loadR As Double, loadX As Double)
            Try
                ' Code that throws the exception.
                Dim LoadZ As New Impedance(loadR, loadX)
                Dim SourceZ As New Impedance(z0, 0.0)
                Dim Transformations As Transformation() = Array.Empty(Of Transformation)
            Catch ex As Exception
                Assert.True(True)
                Exit Sub
            End Try
            Assert.True(False, "Did not fail.")
        End Sub

        '<InlineData(  Z0,        R,         X)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 0.0000)> ' A: At the short circuit point.
        <InlineData(1.0, 0.0000, 1 / 2.0)> ' B: Anywhere else on the perimeter. R=0.0.
        Public Sub MatchArbitraryZ0_BadInput_Fails(z0 As System.Double, loadR As Double, loadX As Double)

            Dim LoadZ As New Impedance(loadR, loadX)
            Dim SourceZ As New Impedance(z0, 0.0)
            Dim Transformations As Transformation() = Array.Empty(Of Transformation)

            Assert.False(Impedance.MatchArbitrary(z0, LoadZ, SourceZ, Transformations))

        End Sub

    End Class ' TestMatchArbitrary_UnlikelyZ0

    Public Class TestMatchArbitraryZ0

        '<InlineData(  Z0,        R,         X)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000)> ' D1: At the center.
        <InlineData(75.0, 75.0, 0.0000)> ' D75: At the center. Z0=75.
        <InlineData(1.0, 1.0, 1.0)> ' E1: On R=Z0 circle, above resonance line.
        <InlineData(50.0, 50.0, 50.0)> ' E50: On R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 1.0, -2.0)> ' F1: On R=Z0 circle, below resonance line.
        <InlineData(50.0, 50.0, -100.0)> ' F50: On R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 2.0, 1 / 2.0)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(50.0, 100.0, 25.0)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 3.0, 0.0000)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(50.0, 150.0, 0.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
        <InlineData(1.0, 2.0, -2.0)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(50.0, 100.0, -100.0)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0)> ' J1: On G=Y0 circle, above resonance line.
        <InlineData(50.0, 25.0, 25.0)> ' J50: On G=Y0 circle, above resonance line. Z0=50.
        <InlineData(1.0, 1 / 2.0, -1 / 2.0)> ' K1: On G=Y0 circle, below resonance line.
        <InlineData(50.0, 25.0, -25.0)> ' K50: On G=Y0 circle, below resonance line. Z0=50.
        <InlineData(1.0, 1 / 3.0, 1 / 3.0)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(75.0, 25.0, 25.0)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1.0, 1 / 3.0, 0.0000)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(75.0, 25.0, 0.0000)> ' M75: Inside G=Y0 circle, on line. Z0=75.
        <InlineData(1.0, 1 / 2.0, -1 / 3.0)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(75.0, 37.5, -25.0)> ' N75: Inside G=Y0 circle, below line. Z0=75.
        <InlineData(1.0, 0.2, 1.4)> ' O1: In the top center.
        <InlineData(50.0, 10.0, 70.0)> ' O50: In the top center. Z0=50.
        <InlineData(1.0, 0.4, -0.8)> ' P1: In the bottom center.
        <InlineData(50.0, 20.0, -40.0)> ' P50: In the bottom center. Z0=50.
        Public Sub MatchArbitraryZ0_GoodInput_Succeeds(z0 As System.Double, loadR As Double, loadX As Double)

            Dim LoadZ As New Impedance(loadR, loadX)
            Dim SourceZ As New Impedance(z0, 0.0)
            Dim Transformations As Transformation() = Array.Empty(Of Transformation)

            Assert.True(Impedance.MatchArbitrary(z0, LoadZ, SourceZ, Transformations))

        End Sub

    End Class ' TestMatchArbitraryZ0

    Public Class TestMatchArbitraryAny

        <Theory>
        <InlineData(1, 1.0, 1.0, 0.5, 0.2)> ' AMRIS1.
        <InlineData(1, 0.5, 0.2, 1.0, 1.0)> ' AMRIS1 reversed.
        <InlineData(100.0, 100.0, 100.0, 50.0, 20.0)> ' AMRIS100.
        <InlineData(100.0, 50.0, 20.0, 100.0, 100.0)> ' AMRIS100 reversed.
        <InlineData(1, 1.0, 1.0, 2.0, -2.0)> ' E1 to I1.
        <InlineData(1, 2.0, -2.0, 1.0, 1.0)> ' I1 to E1.
        <InlineData(75, 50.0, 50.0, 100.0, -100.0)> ' E50 to I50 (75).
        <InlineData(75, 100.0, -100.0, 50.0, 50.0)> ' I50 to E50 (75).
        <InlineData(1, 1 / 3.0, 1 / 3.0, 1 / 3.0, 0.0000)> ' L1 to M1.
        <InlineData(1, 1 / 3.0, 0.0000, 1 / 3.0, 1 / 3.0)> ' M1 to L1.
        <InlineData(75, 25.0, 25.0, 25.0, 0.0000)> ' L75 to M75.
        <InlineData(75, 25.0, 0.0000, 25.0, 25.0)> ' M75 to L75.
        <InlineData(1.0, 0.2, 1.4, 0.4, -0.8)> ' O1 to P1.
        <InlineData(1.0, 0.4, -0.8, 0.2, 1.4)> ' P1 to O1.
        <InlineData(50.0, 10.0, 70.0, 20.0, -40.0)> ' O50 to P50.
        <InlineData(50.0, 20.0, -40.0, 10.0, 70.0)> ' P50 to O50.
        <InlineData(1.0, 1.0, 1.0, 1 / 2.0, 1 / 2.0)> ' E1 to J1.
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 1.0, 1.0)> ' J1 to E1.
        <InlineData(50.0, 50.0, 50.0, 25.0, 25.0)> ' E50 to J50.
        <InlineData(50.0, 25.0, 25.0, 50.0, 50.0)> ' J50 to E50.
        Public Sub MatchArbitrary_GoodInput_Succeeds(z0 As Double, loadR As Double, loadX As Double,
                                                      sourceR As Double, sourceX As Double)

            Dim LoadZ As New Impedance(loadR, loadX)
            Dim SourceZ As New Impedance(sourceR, sourceX)
            Dim Transformations As Transformation() = Array.Empty(Of Transformation)

            Assert.True(Impedance.MatchArbitrary(z0, LoadZ, SourceZ, Transformations))

        End Sub

    End Class ' TestMatchArbitraryAny

End Namespace ' MatchArbitraryTests
