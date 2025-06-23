Imports System
Imports OSNW.Numerics
Imports Xunit

Namespace TestImpedance

    Public Class ToStringSignedTest

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

    End Class ' ToStringSignedTest

    Public Class ToStandardStringSignedTest

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

    End Class ' ToStandardStringSignedTest

    Public Class ToStandardStringFormatTest

        <Fact>
        Sub ToStandardString_FormatPosPos_Succeeds()
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(1.122, 5.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, "F2")
            Assert.Equal("1.12+j5.68", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultPosNeg_Succeeds()
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(111_111.122, -555_555.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, "N2")
            Assert.Equal("111,111.12-j555,555.68", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultNegPos_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-111_111.125, 555_555.675)
            Dim CplxStr As String = Z.ToStandardString(Nothing, "G5")
            Assert.Equal("-1.1111E+05+j5.5556E+05", CplxStr)
        End Sub

        '<Fact>
        'Sub ToStandardString_DefaultNegNeg_Succeeds()
        '    Dim Z As New OSNW.Numerics.Impedance(-1.125, -5.675)
        '    Dim CplxStr As String = Z.ToStandardString("F2", "N2", "G5")
        '    Assert.Equal("-1.125-j5.675", CplxStr)
        'End Sub

    End Class ' ToStandardStringFormatTest

End Namespace ' TestImpedance
