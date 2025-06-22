Imports System
Imports Xunit

Namespace TestImpedance

    Public Class BasicImpTest

        <Fact>
        Sub ToStandardString_PosPos_Succeeds()
            Dim Imp As New OSNW.Numerics.Impedance(1.125, 6.675)
            Dim ImpStr As String = Imp.ToStandardString()
            Assert.Equal("1.125+j6.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_PosNeg_Succeeds()
            Dim Imp As New OSNW.Numerics.Impedance(1.125, -6.675)
            Dim ImpStr As String = Imp.ToStandardString()
            Assert.Equal("1.125-j6.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_NegPos_Succeeds()
            Dim Imp As New OSNW.Numerics.Impedance(-1.125, 6.675)
            Dim ImpStr As String = Imp.ToStandardString()
            Assert.Equal("-1.125+j6.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_NegNeg_Succeeds()
            Dim Imp As New OSNW.Numerics.Impedance(-1.125, -6.675)
            Dim ImpStr As String = Imp.ToStandardString()
            Assert.Equal("-1.125-j6.675", ImpStr)
        End Sub

    End Class

End Namespace ' TestImpedance
