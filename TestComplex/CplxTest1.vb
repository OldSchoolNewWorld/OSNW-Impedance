Imports System
Imports Xunit
Imports OSNW.Numerics.ComplexExtensions

' REF: Extension Methods not Recognized
' https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-extend-a-type-with-extension-methods

Namespace TestComplex

    Public Class SignedCplxTest

        <Fact>
        Sub ToStandardString_PosPos_Succeeds()
            Dim Z As New System.Numerics.Complex(1.125, 5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("1.125+i5.675", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_PosNeg_Succeeds()
            Dim Z As New System.Numerics.Complex(1.125, -5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("1.125-i5.675", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_NegPos_Succeeds()
            Dim Z As New System.Numerics.Complex(-1.125, 5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("-1.125+i5.675", CplxStr)
        End Sub

        <Fact>
        Sub ToStandardString_NegNeg_Succeeds()
            Dim Z As New System.Numerics.Complex(-1.125, -5.675)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal("-1.125-i5.675", CplxStr)
        End Sub

    End Class

End Namespace ' TestComplex
