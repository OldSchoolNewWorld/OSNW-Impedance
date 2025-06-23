Imports System
Imports System.Globalization
Imports OSNW.Numerics
Imports Xunit

Namespace TestImpedance

    Public Class ToStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "<1.125; 5.675>")>
        <InlineData(1.125, -5.675, "<1.125; -5.675>")>
        <InlineData(-1.125, 5.675, "<-1.125; 5.675>")>
        <InlineData(-1.125, -5.675, "<-1.125; -5.675>")>
        Sub ToString_Default_Succeeds(r As Double, x As Double, expect As String)
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpStr As String = Z.ToString()
            Assert.Equal(expect, ImpStr)
        End Sub

    End Class ' ToStringDefaultTest

    Public Class ToStandardStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "1.125+j5.675")>
        <InlineData(1.125, -5.675, "1.125-j5.675")>
        <InlineData(-1.125, 5.675, "-1.125+j5.675")>
        <InlineData(-1.125, -5.675, "-1.125-j5.675")>
        Sub ToStandardString_Default_Succeeds(r As Double, x As Double, expect As String)
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpStr As String = Z.ToStandardString()
            Assert.Equal(expect, ImpStr)
        End Sub

    End Class ' ToStandardStringDefaultTest

    Public Class ToStandardStringFormatTest

        <Theory>
        <InlineData(1.122, 5.677, "F2", "1.12+j5.68")>
        <InlineData(111_111.122, -555_555.677, "N2", "111,111.12-j555,555.68")>
        <InlineData(-111_111.125, 555_555.675, "G5", "-1.1111E+05+j5.5556E+05")>
        Sub ToStandardString_Format_Succeeds(r As Double, x As Double, format As String, expect As String)
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim CplxStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expect, CplxStr)
        End Sub

    End Class ' ToStandardStringFormatTest

    Public Class ToStandardStringCultureTest

        <Fact>
        Sub ToStandardString_InvariantCulture_Succeeds()
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(111_111.122, -555_555.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, CultureInfo.InvariantCulture)
            Assert.Equal("111111.122-j555555.677", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_CurrentCulture_Succeeds()
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(111_111.122, -555_555.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, CultureInfo.CurrentCulture)
            Assert.Equal("111111.122-j555555.677", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_UKCulture_Succeeds()
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(1.122, 5.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, New CultureInfo("en-UK", False))
            Assert.Equal("1.122+j5.677", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_RussiaCulture_Succeeds()
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(111_111.122, -555_555.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, New CultureInfo("ru-RU", False))
            Assert.Equal("111111,122-j555555,677", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_FranceCulture_Succeeds()
            Dim Z As New OSNW.Numerics.Impedance(-111_111.125, 555_555.675)
            Dim CplxStr As String = Z.ToStandardString(Nothing, New CultureInfo("fr-FR", False))
            Assert.Equal("-111111,125+j555555,675", CplxStr)
        End Sub

    End Class ' ToStandardStringCultureTest

End Namespace ' TestImpedance
