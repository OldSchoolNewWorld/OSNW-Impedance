Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports OSNW.Numerics
Imports Xunit
Imports OsnwAdmt = OSNW.Numerics.Admittance
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

Public Class TestVals
    Public Const SAMECONDUCTANCE As Double = 111_111.125 ' 1/8 is good for binary fractions.
    Public Const SAMESUSCEPTANCE As Double = 555_555.687_5 ' 11/16 is good for binary fractions.
End Class

Namespace ToStringTests

    Public Class TestToStringDefault

        <Theory>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, "<111111.125; 555555.6875>")>
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, "<111111.125; -555555.6875>")>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, "<111111.125; 555555.6875>")>
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, "<111111.125; -555555.6875>")>
        Sub ToString_Default_Succeeds(g As Double, b As Double, expect As String)
            Dim Z As New OsnwAdmt(g, b)
            Dim AdmtStr As String = Z.ToString()
            Assert.Equal(expect, AdmtStr)
        End Sub

    End Class ' TestToStringDefault

End Namespace ' ToStringTests

Namespace ToStandardStringTests

    Public Class TestToStandardStringDefault

        <Theory>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, "111111.125-555555.6875j")> ' -B
        Sub ToStandardString_Default_Succeeds(conductance As Double, susceptance As Double, expected As String)
            Dim Z As New OsnwAdmt(conductance, susceptance)
            Dim AdmtStr As String = Z.ToStandardString()
            Assert.Equal(expected, AdmtStr)
        End Sub

    End Class ' TestToStandardStringDefault

    Public Class TestToStandardStringStandardization

        <Theory>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, Nothing, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, OsnwNumSS.Open, "111111.125 + 555555.6875j")>
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, OsnwNumSS.AiB, "111111.125-j555555.6875")>
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, OsnwNumSS.OpenAiB, "111111.125 - j555555.6875")>
        Sub ToStandardString_Standardization_Succeeds(conductance As Double, susceptance As Double,
                                                      stdStyle As OsnwNumSS, expected As String)
            Dim Z As New OsnwAdmt(conductance, susceptance)
            Dim AdmtStr As String = Z.ToStandardString(stdStyle)
            Assert.Equal(expected, AdmtStr)
        End Sub

    End Class ' TestToStandardStringStandardization

    Public Class TestToStandardStringFormat

        <Theory>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, "G", "111111.125+555555.6875j")>
        <InlineData(111_111.122, 111_111.127, "F2", "111111.12+111111.13j")> ' One round down, one up.
        <InlineData(111_111.127, -111_111.122, "N2", "111,111.13-111,111.12j")> ' One round up, one down.
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, "G5", "1.1111E+05-5.5556E+05j")>
        <InlineData(Math.PI, Math.E, "G", "3.141592653589793+2.718281828459045j")>
        Sub ToStandardString_Format_Succeeds(conductance As Double, susceptance As Double, format As String, expected As String)
            Dim Z As New OsnwAdmt(conductance, susceptance)
            Dim AdmtStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expected, AdmtStr)
        End Sub

    End Class ' TestToStandardStringFormat

    Public Class TestToStandardStringCulture

        <Theory>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 0, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 1, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, 2, "111111.125-555555.6875j")>
        <InlineData(TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, 3, "111111.125-555555.6875j")>
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 4, "111111,125+555555,6875j")> ' Comma decimal.
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 5, "111111,125+555555,6875j")> ' Comma decimal.
        <InlineData(TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 6,
                    "111111" & CHARARABCOMMA66B & "125+555555" & CHARARABCOMMA66B &
                    "6875j")> ' Arabic comma CHARARABCOMMA66B.
        Sub ToStandardString_Culture_Succeeds(
            conductance As Double, susceptance As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False),
                New CultureInfo("ar-001", False)
            }
            Dim Z As New OsnwAdmt(conductance, susceptance)

            Dim AdmtStr As String = Z.ToStandardString(Nothing, Providers(index))

            Assert.Equal(expected, AdmtStr)

        End Sub

        ' No failure tests. Any valid double values should be allowed.

    End Class ' TestToStandardStringCulture

End Namespace ' ToStandardStringTests

Namespace TryParseStandardTests

    Public Class TestTryParseStandardDefault

        <Theory>
        <InlineData("111111.125+555555.6875j", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)>
        <InlineData("111111.125+j555555.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' A+iB, j in middle.
        <InlineData("111111.125 - 555555.6875j", TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE)> ' Open.
        <InlineData("111111.125-555555.6875j", TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE)>
        <InlineData("1.11111125E5+.5555556875e6j", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Mixed E/e.
        <InlineData("11111112.5e-2+555555687.5E-3j", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Mixed e/E.
        Sub TryParseStandardDefault_GoodInput_Succeeds(standardStr As String, conductance As Double, susceptance As Double)
            Dim Admt As New OsnwAdmt
            If Not OsnwAdmt.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(conductance.Equals(Admt.Conductance) AndAlso susceptance.Equals(Admt.Susceptance))
        End Sub

        <Fact>
        Sub TryParseStandardDefault_NegativeConductance_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Admt As New OsnwAdmt
                    If OsnwAdmt.TryParseStandard("-111111.125+555555.6875j", Nothing, Nothing, Admt) Then
                        Assert.Fail("Parsed despite bad entry.")
                    End If
                    Assert.False(Admt.Conductance.Equals(-TestVals.SAMECONDUCTANCE) AndAlso
                                 Admt.Susceptance.Equals(TestVals.SAMESUSCEPTANCE))
                End Sub)
        End Sub

        <Theory>
        <InlineData("", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Empty.
        <InlineData("123", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Too short.
        <InlineData("111111.125+555555.6875Q", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Bad char Q.
        <InlineData("111111.125+Q5.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Bad char Q.
        <InlineData("111111.125+555555.6875i", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' i, not j
        <InlineData("111111.125+j555555.6875j", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Excess j.
        <InlineData(".1125e1+j.56875F1", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' F, not E.
        <InlineData("112.5E-2.2+i5687.5e-3", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Non-integer exponent.
        Sub TryParseStandardDefault_BadInput_Fails(standardStr As String, conductance As Double, susceptance As Double)
            Dim Admt As New OsnwAdmt
            If OsnwAdmt.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.Fail("Parsed despite bad entry.")
            End If
            Assert.False(Admt.Conductance.Equals(conductance) AndAlso Admt.Susceptance.Equals(susceptance))
        End Sub

    End Class ' TestTryParseStandardDefault

    Public Class TestTryParseStandardDefaultMixed

        <Theory>
        <InlineData("111111.125+j555555.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' A+Bi.
        <InlineData("111111.125-555555.6875j", TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE)> ' A+Bi.
        <InlineData("111111.125 + j555555.6875", TestVals.SAMECONDUCTANCE, 555555.6875)> ' Open, one space.
        <InlineData(" 111111.125  -   555555.6875j  ", TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE)> ' Open, asymmetric spaces.
        <InlineData("111111.125+ j555555.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Open, space one side.
        <InlineData("111111.125 +j555555.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Open, space one side.
        <InlineData("111111125e-3+j.5555556875E6", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE)> ' Exponential notation.
        Sub TryParseStandard_Default_Succeeds(standardStr As String, conductance As Double, susceptance As Double)
            Dim Admt As New OsnwAdmt
            If Not OsnwAdmt.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Admt.Conductance.Equals(conductance) AndAlso Admt.Susceptance.Equals(susceptance))
        End Sub

    End Class ' TestTryParseStandardDefaultMixed

    Public Class TestTryParseStandardEnforceStandardization

        Const TightEnforcement As OsnwNumSS =
            OsnwNumSS.EnforceSequence Or OsnwNumSS.EnforceSpacing

        <Theory>
        <InlineData("111111.125+j555555.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, OsnwNumSS.ClosedABi)>
        <InlineData("111111.125-555555.6875j", TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, OsnwNumSS.ClosedAiB)>
        <InlineData("111111.125 + j555555.6875", 111111.125, TestVals.SAMESUSCEPTANCE, OsnwNumSS.OpenABi)>
        <InlineData("111111.125 - 555555.6875j", 111111.125, -TestVals.SAMESUSCEPTANCE, OsnwNumSS.OpenAiB)>
        Sub TryParseStandard_ValidStandardization_Succeeds(
            standardStr As String, conductance As Double, susceptance As Double,
            standardizationStyle As OsnwNumSS)

            Dim Admt As New OsnwAdmt
            If Not OsnwAdmt.TryParseStandard(standardStr, standardizationStyle, Nothing, Admt) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Admt.Conductance.Equals(conductance) AndAlso Admt.Susceptance.Equals(susceptance))
        End Sub

        <Theory>
        <InlineData("111111.125 + j5555555.6875", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not closed.
        <InlineData("111111.125+5555555.j6875", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not ABi.
        <InlineData("111111.125-5555555.6875j", OsnwNumSS.ClosedAiB Or TightEnforcement)> ' Not AiB.
        <InlineData("111111.125+j5555555.6875", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not Open.
        <InlineData("111111.125 - j5555555.6875", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not ABi.
        <InlineData("111111.125 - 5555555.6875j", OsnwNumSS.OpenAiB Or TightEnforcement)> ' Not AiB.
        Sub TryParseStandard_InvalidStandardization_Fails(
            standardStr As String, standardizationStyle As OsnwNumSS)

            Dim Admt As New OsnwAdmt
            Assert.False(OsnwAdmt.TryParseStandard(standardStr, standardizationStyle, Nothing, Admt))
        End Sub

    End Class ' TestTryParseStandardEnforceStandardization

    Public Class TestTryParseStandardCulture

        <Theory>
        <InlineData("111111.125+j555555.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 0)>
        <InlineData("111111.125+j555555.6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 1)> ' When current is "en-US".
        <InlineData("111111.125-555555.6875j", TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, 2)> ' A+Bi, i at end.
        <InlineData("111111.125 - j555555.6875", TestVals.SAMECONDUCTANCE, -TestVals.SAMESUSCEPTANCE, 3)> ' Open, one space.
        <InlineData("111111,125+j555555,6875", TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 4)> ' Comma decimal.
        <InlineData("111" & CHARNNBSP & "111,125+j555" & CHARNNBSP & "555,6875",
                    TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 5)> ' Comma decimal, Non-breaking space.
        <InlineData("111111" & CHARARABCOMMA66B & "125+555555" & CHARARABCOMMA66B & "6875j",
                    TestVals.SAMECONDUCTANCE, TestVals.SAMESUSCEPTANCE, 6)> ' Arabic comma CHARARABCOMMA66B.
        Sub TryParseStandard_Culture_Succeeds(standardStr As String, conductance As Double,
                                              susceptance As Double, index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False),
                New CultureInfo("ar-001", False)
            }
            Dim Admt As New OsnwAdmt

            If Not OsnwAdmt.TryParseStandard(standardStr, Nothing, Providers(index), Admt) Then
                Assert.Fail("Failed to parse.")
            End If

            Assert.True(Admt.Conductance.Equals(conductance) AndAlso Admt.Susceptance.Equals(susceptance))

        End Sub

    End Class ' TestTryParseStandardCulture

End Namespace ' TryParseStandardTests

Namespace MathTests

    Public Class TestEqualsObject

        <Fact>
        Sub EqualsObject_TypeMismatch_Fails1()
            Dim A1 As New OsnwAdmt(3, 4)
            Dim C2 As New System.Numerics.Complex(3, 4)
            Assert.False(A1.Equals(C2))
        End Sub

        <Fact>
        Sub EqualsObject_TypeMismatch_Fails2()
            Dim A1 As New OsnwAdmt(3, 4)
            Dim C2 As Object = New System.Numerics.Complex(3, 4)
            Assert.False(A1.Equals(C2))
        End Sub

        <Fact>
        Sub EqualsObject_ValueMismatch_Fails()
            Dim A1 As New OsnwAdmt(3, 4)
            Dim A2 As Object = New OsnwAdmt(4, 5)
            Assert.False(A1.Equals(A2))
        End Sub

    End Class ' TestEqualsObject

    Public Class TestEqualsOther

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
            Dim I1 As OsnwAdmt
            Dim I2 As OsnwAdmt
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleEmpty_Passes()
            Dim I1 As New OsnwAdmt()
            Dim I2 As New OsnwAdmt()
            Assert.True(I1.Equals(Nothing))
        End Sub

        <Fact>
        Sub Equals_Nothing_Fails()
            Dim I1 As New OsnwAdmt(1, 2)
            Assert.False(I1.Equals(Nothing))
        End Sub

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
