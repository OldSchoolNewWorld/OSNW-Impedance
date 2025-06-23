Imports System
Imports Xunit

Namespace TestImpedance

    Public Class SignedImpdTest

        <Fact>
        Sub ToStandardString_PosPos_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.125, 5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("1.125+j5.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_PosNeg_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.125, -5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("1.125-j5.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_NegPos_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-1.125, 5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("-1.125+j5.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_NegNeg_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-1.125, -5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("-1.125-j5.675", ImpStr)
        End Sub

    End Class

End Namespace ' TestImpedance
