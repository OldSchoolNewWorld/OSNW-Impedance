Imports System
Imports Xunit
Imports OSNW.Numerics.ComplexExtensions

' REF: Extension Methods not Recognized
' https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-extend-a-type-with-extension-methods

Namespace TestComplex

    Public Class CplxSignedTest

        <Fact>
        Sub ToStandardString_DefaultPosPos_Succeeds()
            Dim Z As New System.Numerics.Complex(1.125, 5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("1.125+i5.675", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultPosNeg_Succeeds()
            Dim Z As New System.Numerics.Complex(1.125, -5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("1.125-i5.675", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultNegPos_Succeeds()
            Dim Z As New System.Numerics.Complex(-1.125, 5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("-1.125+i5.675", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultNegNeg_Succeeds()
            Dim Z As New System.Numerics.Complex(-1.125, -5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("-1.125-i5.675", CplxStr)
        End Sub

    End Class

    Public Class CplxFormatTest

        <Fact>
        Sub ToStandardString_FormatPosPos_Succeeds()
            ' One round down, one up.
            Dim Z As New System.Numerics.Complex(1.122, 5.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, "F2")
            Assert.Equal("1.12+i5.68", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultPosNeg_Succeeds()
            ' One round down, one up.
            Dim Z As New System.Numerics.Complex(111_111.122, -555_555.677)
            Dim CplxStr As String = Z.ToStandardString(Nothing, "N2")
            Assert.Equal("111,111.12-i555,555.68", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_DefaultNegPos_Succeeds()
            Dim Z As New System.Numerics.Complex(-111_111.125, 555_555.675)
            Dim CplxStr As String = Z.ToStandardString(Nothing, "G5")
            Assert.Equal("-1.1111E+05+i5.5556E+05", CplxStr)
        End Sub

        '<Fact>
        'Sub ToStandardString_DefaultNegNeg_Succeeds()
        '    Dim Z As New System.Numerics.Complex(-1.125, -5.675)
        '    Dim CplxStr As String = Z.ToStandardString("F2", "N2", "G5")
        '    Assert.Equal("-1.125-i5.675", CplxStr)
        'End Sub

    End Class

End Namespace ' TestComplex
