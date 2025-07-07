Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports Xunit
Imports OSNW.Numerics
Imports OsnwAdmt = OSNW.Numerics.Admittance
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

Namespace DevelopmentTests

    Public Class TestEquals

        <Fact>
        Sub Equals_GoodMatch_Passes()
            Dim I1 As New OsnwAdmt(1, 2)
            Dim I2 As New OsnwAdmt(1, 2)
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_Mismatch_Fails()
            Dim I1 As New OsnwAdmt(1, 2)
            Dim I2 As New OsnwAdmt(2, 3)
            Assert.False(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleDefault_Passes()
            ' What happens the nothing is sent?
            ' Is a null check needed?
            Dim I1 As OsnwAdmt
            Dim I2 As OsnwAdmt
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleEmpty_Passes()
            ' What happens when nothing is set?
            ' Is a null check needed?
            Dim I1 As New OsnwAdmt()
            Dim I2 As New OsnwAdmt()
            Assert.True(I1.Equals(Nothing))
        End Sub

        <Fact>
        Sub Equals_Nothing_Fails()
            ' What happens the "Nothing" is sent?
            ' Is a null check needed?
            Dim I1 As New OsnwAdmt(1, 2)
            Assert.False(I1.Equals(Nothing))
        End Sub

    End Class

End Namespace ' DevTests

Namespace ToStringTests

    Public Class TestToStringDefault

        <Theory>
        <InlineData(1.125, 5.675, "<1.125; 5.675>")>
        <InlineData(1.125, -5.675, "<1.125; -5.675>")>
        <InlineData(0, 5.675, "<0; 5.675>")>
        <InlineData(0, -5.675, "<0; -5.675>")>
        Sub ToString_Default_Succeeds(g As Double, b As Double, expect As String)
            Dim Y As New OsnwAdmt(g, b)
            Dim AdmtStr As String = Y.ToString()
            Assert.Equal(expect, AdmtStr)
        End Sub

    End Class ' TestToStringDefault

End Namespace ' ToStringTests

Namespace ToStandardStringTests

    Public Class TestToStandardStringDefault

        <Theory>
        <InlineData(1.125, 5.675, "1.125+5.675j")>
        <InlineData(1.125, -5.675, "1.125-5.675j")>
        <InlineData(0, 5.675, "0+5.675j")>
        <InlineData(0, -5.675, "0-5.675j")>
        Sub ToStandardString_Default_Succeeds(g As Double, b As Double, expect As String)
            Dim Y As New OsnwAdmt(g, b)
            Dim AdmtStr As String = Y.ToStandardString()
            Assert.Equal(expect, AdmtStr)
        End Sub

    End Class ' TestToStandardStringDefault

    Public Class TestToStandardStringStandardization

        <Theory>
        <InlineData(1.125, 5.675, Nothing, "1.125+5.675j")>
        <InlineData(1.125, -5.675, OsnwNumSS.AiB, "1.125-j5.675")>
        <InlineData(0, 5.675, OsnwNumSS.Open, "0 + 5.675j")>
        <InlineData(0, -5.675, OsnwNumSS.OpenAiB, "0 - j5.675")>
        Sub ToStandardString_Standardization_Succeeds(
            g As Double, b As Double, standardizationStyle As OsnwNumSS, expected As String)

            Dim Y As New OsnwAdmt(g, b)
            Dim AdmtStr As String = Y.ToStandardString(standardizationStyle)
            Assert.Equal(expected, AdmtStr)
        End Sub

    End Class ' TestToStandardStringStandardization

    Public Class TestToStandardStringFormat

        <Theory>
        <InlineData(1.122, 5.677, "F2", "1.12+5.68j")>
        <InlineData(111_111.122, -555_555.677, "N2", "111,111.12-555,555.68j")>
        <InlineData(111_111.125, 555_555.675, "G5", "1.1111E+05+5.5556E+05j")>
        Sub ToStandardString_Format_Succeeds(g As Double, b As Double, format As String, expect As String)
            ' One round down, one up.
            Dim Y As New OsnwAdmt(g, b)
            Dim AdmtStr As String = Y.ToStandardString(Nothing, format)
            Assert.Equal(expect, AdmtStr)
        End Sub

    End Class ' TestToStandardStringFormat

    Public Class TestToStandardStringCulture

        <Theory>
        <InlineData(111_111.122, -555_555.677, 0, "111111.122-555555.677j")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 1, "111111.122-555555.677j")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 2, "111111.122-555555.677j")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 3, "111111.122-555555.677j")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 4, "111111,122-555555,677j")> ' One round down, one up.
        <InlineData(111_111.125, 555_555.675, 5, "111111,125+555555,675j")>
        Sub ToStandardString_Culture_Succeeds(
            g As Double, b As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Y As New OsnwAdmt(g, b)

            Dim AdmtStr As String = Y.ToStandardString(Nothing, Providers(index))

            Assert.Equal(expected, AdmtStr)

        End Sub

    End Class ' TestToStandardStringCulture

End Namespace ' ToStandardStringTests

Namespace TryParseStandardTests

    Public Class TestTryParseStandardDefault

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)>
        <InlineData("1.125-i5.675", 1.125, -5.675)>
        <InlineData("0+i5.675", 0, 5.675)>
        <InlineData("0-i5.675", 0, -5.675)>
        Sub TryParseStandard_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Admt As New OsnwAdmt
            If Not OsnwAdmt.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.True(False)
            End If
            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))
        End Sub

    End Class ' TestTryParseStandardDefault

    Public Class TestTryParseStandardDefaultMixed

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)> ' A+Bi.
        <InlineData("1.125-5.675i", 1.125, -5.675)> ' A+Bi.
        <InlineData("0 + i5.675", 0, 5.675)> ' Open, one space.
        <InlineData(" 0  -   5.675i  ", 0, -5.675)> ' Open, asymmetric spaces.
        <InlineData("0+ i5.675", 0, 5.675)> ' Open, space one side.
        <InlineData("0 +i5.675", 0, 5.675)> ' Open, space one side.
        <InlineData("1125e-3+i.5675E1", 1.125, 5.675)> ' Exponential notation.
        Sub TryParseStandard_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Admt As New OsnwAdmt
            If Not OsnwAdmt.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.True(False)
            End If
            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))
        End Sub

    End Class ' TestTryParseStandardDefaultMixed

    Public Class TestTryParseStandardEnforceStandardization

        Const TightEnforcement As OsnwNumSS =
            OsnwNumSS.EnforceSequence Or OsnwNumSS.EnforceSpacing

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675, OsnwNumSS.ClosedABi)>
        <InlineData("1.125-5.675i", 1.125, -5.675, OsnwNumSS.ClosedAiB)>
        <InlineData("0 + i5.675", 0, 5.675, OsnwNumSS.OpenABi)>
        <InlineData("0 - 5.675i", 0, -5.675, OsnwNumSS.OpenAiB)>
        Sub TryParseStandard_ValidStandardization_Succeeds(standardStr As String, real As Double,
            imaginary As Double, standardizationStyle As OsnwNumSS)

            Dim Admt As New OsnwAdmt
            If Not OsnwAdmt.TryParseStandard(standardStr, standardizationStyle, Nothing, Admt) Then
                Assert.True(False)
            End If
            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + j5.675", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not closed.
        <InlineData("1.125+5.j675", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not ABi.
        <InlineData("1.125-5.675j", OsnwNumSS.ClosedAiB Or TightEnforcement)> ' Not AiB.
        <InlineData("1.125+j5.675", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not Open.
        <InlineData("1.125 - j5.675", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not ABi.
        <InlineData("1.125 - 5.675j", OsnwNumSS.OpenAiB Or TightEnforcement)> ' Not AiB.
        Sub TryParseStandard_InvalidStandardization_Fails(
            standardStr As String, standardizationStyle As OsnwNumSS)

            Dim Admt As New OsnwAdmt
            Assert.False(OsnwAdmt.TryParseStandard(standardStr, standardizationStyle, Nothing, Admt))
        End Sub

    End Class ' TestTryParseStandardEnforceStandardization

    Public Class TestTryParseStandardCulture

        <Theory>
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 0)> ' One round down, one up.
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 1)> ' One round down, one up.
        <InlineData("1.122+i5.677", 1.122, 5.677, 2)>
        <InlineData("111111,122-i555555,677", 111_111.122, -555_555.677, 3)> ' One round down, one up.
        <InlineData("111111,125+i555555,675", 111_111.125, 555_555.675, 4)>
        Sub TryParseStandardCulture_Succeeds(
            standardStr As String, real As Double, imaginary As Double, index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Admt As New OsnwAdmt

            If Not OsnwAdmt.TryParseStandard(standardStr, Nothing, Providers(index), Admt) Then
                Assert.True(False)
            End If

            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))

        End Sub

    End Class ' TestTryParseStandardCulture

End Namespace ' TryParseStandardTests

Namespace MathTests

    Public Class TestEqualsObject

        <Fact>
        Sub EqualsObject_TypeMismatch_Fails1()
            Dim I1 As New OsnwAdmt(3, 4)
            Dim C2 As New Impedance(3, 4)
            Assert.False(I1.Equals(C2))
        End Sub

        <Fact>
        Sub EqualsObject_TypeMismatch_Fails2()
            Dim I1 As New OsnwAdmt(3, 4)
            Dim C2 As Object = New Impedance(3, 4)
            Assert.False(I1.Equals(C2))
        End Sub

        <Fact>
        Sub EqualsObject_ValueMismatch_Fails()
            Dim I1 As New OsnwAdmt(3, 4)
            Dim C2 As Object = New OsnwAdmt(4, 5)
            Assert.False(I1.Equals(C2))
        End Sub

    End Class ' TestEqualsObject

    Public Class TestEqualsOther

        <Fact>
        Sub EqualsOther_Match_Passes()
            Dim I1 As New OsnwAdmt(1, 2)
            Dim I2 As New OsnwAdmt(1, 2)
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub EqualsOther_Mismatch_Fails()
            Dim I1 As New OsnwAdmt(1, 2)
            Dim I2 As New OsnwAdmt(1, 3)
            Assert.False(I1.Equals(I2))
        End Sub

    End Class ' TestEqualsOther

End Namespace ' MathTests
