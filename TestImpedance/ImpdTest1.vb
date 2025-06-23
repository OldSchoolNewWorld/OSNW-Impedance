Imports System
Imports Xunit

Namespace TestImpedance

    Public Class SignedToStringTest

        <Fact>
        Sub ToString_DefaultPosPos_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.125, 5.675)
            Dim ImpStr As String = Z.ToString()
            Assert.Equal("<1.125; 5.675>", ImpStr)
        End Sub

        <Fact>
        Sub ToString_DefaultPosNeg_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.125, -5.675)
            Dim ImpStr As String = Z.ToString()
            Assert.Equal("<1.125; -5.675>", ImpStr)
        End Sub

        <Fact>
        Sub ToString_DefaultNegPos_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-1.125, 5.675)
            Dim ImpStr As String = Z.ToString()
            Assert.Equal("<-1.125; 5.675>", ImpStr)
        End Sub

        <Fact>
        Sub ToString_DefaultNegNeg_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-1.125, -5.675)
            Dim ImpStr As String = Z.ToString()
            Assert.Equal("<-1.125; -5.675>", ImpStr)
        End Sub

    End Class ' SignedToStringTest

    Public Class SignedToStandardStringTest

        <Fact>
        Sub ToStandardString_DefaultPosPos_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.125, 5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("1.125+j5.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultPosNeg_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(1.125, -5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("1.125-j5.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultNegPos_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-1.125, 5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("-1.125+j5.675", ImpStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultNegNeg_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-1.125, -5.675)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal("-1.125-j5.675", ImpStr)
        End Sub

    End Class ' SignedToStandardStringTest

End Namespace ' TestImpedance
