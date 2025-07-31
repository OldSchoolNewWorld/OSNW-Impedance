Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports OSNW.Numerics.ComplexExtensions
Imports Xunit
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

Public Class TestVals
    Public Const SAMEREAL As Double = 111_111.125 ' 1/8 is good for binary fractions.
    Public Const SAMEIMAG As Double = 555_555.687_5 ' 11/16 is good for binary fractions.
End Class

Namespace StandardizationStylesValuesTests

    Public Class TestStandardizationStylesValues

        <Theory>
        <InlineData(OsnwNumSS.None, 0)>
        <InlineData(OsnwNumSS.AiB, 1)>
        <InlineData(OsnwNumSS.Open, 2)>
        <InlineData(OsnwNumSS.EnforceSequence, 4)>
        <InlineData(OsnwNumSS.EnforceSpacing, 8)>
        Public Sub ConfirmBinaryValues(stdStyle As OsnwNumSS, expected As Integer)
            Assert.Equal(expected, CInt(stdStyle))
        End Sub

        <Theory>
        <InlineData(OsnwNumSS.ClosedABi, 0)>
        <InlineData(OsnwNumSS.ClosedAiB, 1)>
        <InlineData(OsnwNumSS.OpenABi, 2)>
        <InlineData(OsnwNumSS.OpenAiB, 3)>
        Public Sub ConfirmShorthandValues(stdStyle As OsnwNumSS, expected As Integer)
            Assert.Equal(expected, CInt(stdStyle))
        End Sub

        <Theory>
        <InlineData(OsnwNumSS.EnforceBoth, 12)>
        <InlineData(OsnwNumSS.EnforcedClosedABi, 12)>
        <InlineData(OsnwNumSS.EnforcedClosedAiB, 13)>
        <InlineData(OsnwNumSS.EnforcedOpenABi, 14)>
        <InlineData(OsnwNumSS.EnforcedOpenAiB, 15)>
        Public Sub ConfirmEnforcedValues(stdStyle As OsnwNumSS, expected As Integer)
            Assert.Equal(expected, CInt(stdStyle))
        End Sub

    End Class ' TestStandardizationStylesValues

End Namespace ' StandardizationStylesValuesTests

Namespace ToStandardStringTests

    Public Class TestToStandardStringDefault

        <Theory>
        <InlineData(TestVals.SAMEREAL, TestVals.SAMEIMAG, "111111.125+555555.6875i")>
        <InlineData(-TestVals.SAMEREAL, TestVals.SAMEIMAG, "-111111.125+555555.6875i")>  ' -A
        <InlineData(TestVals.SAMEREAL, -TestVals.SAMEIMAG, "111111.125-555555.6875i")> ' -B
        <InlineData(-TestVals.SAMEREAL, -TestVals.SAMEIMAG, "-111111.125-555555.6875i")> ' -A, -B
        Sub ToStandardString_Default_Succeeds(real As Double, imaginary As Double, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' TestToStandardStringDefault

    Public Class TestToStandardStringStandardization

        <Theory>
        <InlineData(TestVals.SAMEREAL, TestVals.SAMEIMAG, Nothing, "111111.125+555555.6875i")>
        <InlineData(-TestVals.SAMEREAL, TestVals.SAMEIMAG, OsnwNumSS.Open, "-111111.125 + 555555.6875i")> ' -A
        <InlineData(TestVals.SAMEREAL, -TestVals.SAMEIMAG, OsnwNumSS.AiB, "111111.125-i555555.6875")> ' -B
        <InlineData(-TestVals.SAMEREAL, -TestVals.SAMEIMAG, OsnwNumSS.OpenAiB, "-111111.125 - i555555.6875")> ' -A, -B
        Sub ToStandardString_Standardization_Succeeds(real As Double, imaginary As Double,
                                                      stdStyle As OsnwNumSS, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString(stdStyle)
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' TestToStandardStringStandardization

    Public Class TestToStandardStringFormat

        <Theory>
        <InlineData(TestVals.SAMEREAL, TestVals.SAMEIMAG, "G", "111111.125+555555.6875i")>
        <InlineData(-111_111.122, 111_111.127, "F2", "-111111.12+111111.13i")> ' One round down, one up.
        <InlineData(111_111.122, -111_111.127, "N2", "111,111.12-111,111.13i")> ' One round down, one up.
        <InlineData(-TestVals.SAMEREAL, -TestVals.SAMEIMAG, "G5", "-1.1111E+05-5.5556E+05i")>
        <InlineData(Math.PI, Math.E, "G", "3.141592653589793+2.718281828459045i")>
        Sub ToStandardString_Format_Succeeds(real As Double, imaginary As Double, format As String, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' TestToStandardStringFormat

    Public Class TestToStandardStringCulture

        <Theory>
        <InlineData(TestVals.SAMEREAL, TestVals.SAMEIMAG, 0, "111111.125+555555.6875i")>
        <InlineData(-TestVals.SAMEREAL, TestVals.SAMEIMAG, 1, "-111111.125+555555.6875i")>
        <InlineData(TestVals.SAMEREAL, -TestVals.SAMEIMAG, 2, "111111.125-555555.6875i")>
        <InlineData(-TestVals.SAMEREAL, -TestVals.SAMEIMAG, 3, "-111111.125-555555.6875i")>
        <InlineData(TestVals.SAMEREAL, TestVals.SAMEIMAG, 4, "111111,125+555555,6875i")> ' Comma decimal.
        <InlineData(TestVals.SAMEREAL, TestVals.SAMEIMAG, 5, "111111,125+555555,6875i")> ' Comma decimal.
        <InlineData(TestVals.SAMEREAL, TestVals.SAMEIMAG, 6,
                    "111111" & CHARARABCOMMA66B & "125+555555" & CHARARABCOMMA66B &
                    "6875i")> ' Arabic comma CHARARABCOMMA66B.
        Sub ToStandardString_Culture_Succeeds(
            real As Double, imaginary As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False),
                New CultureInfo("ar-001", False)
            }
            Dim Z As New System.Numerics.Complex(real, imaginary)

            Dim CplxStr As String = Z.ToStandardString(Nothing, Providers(index))

            Assert.Equal(expected, CplxStr)

        End Sub

        ' No failure tests. Any valid double values should be allowed.

    End Class ' TestToStandardStringCulture

End Namespace ' ToStandardStringTests

Namespace TryParseStandardTests

    Public Class TestTryParseStandardDefault

        <Theory>
        <InlineData("111111.125+555555.6875i", TestVals.SAMEREAL, TestVals.SAMEIMAG)>
        <InlineData("-111111.125+i555555.6875", -TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' A+iB, i in middle.
        <InlineData("111111.125 - 555555.6875i", TestVals.SAMEREAL, -TestVals.SAMEIMAG)> ' Open.
        <InlineData("-111111.125-555555.6875i", -TestVals.SAMEREAL, -TestVals.SAMEIMAG)>
        <InlineData("1.11111125E5+.5555556875e6i", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Mixed E/e.
        <InlineData("11111112.5e-2+555555687.5E-3i", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Mixed e/E.
        Sub TryParseStandardDefault_GoodInput_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Cplx As New Numerics.Complex
            If Not TryParseStandard(standardStr, Nothing, Nothing, Cplx) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(real.Equals(Cplx.Real) AndAlso imaginary.Equals(Cplx.Imaginary))
        End Sub

        <Theory>
        <InlineData("", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Empty.
        <InlineData("123", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Too short.
        <InlineData("111111.125+555555.6875Q", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Bad char Q.
        <InlineData("111111.125+Q5.6875", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Bad char Q.
        <InlineData("111111.125+555555.6875j", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' j, not i
        <InlineData("111111.125+i555555.6875i", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Excess i.
        <InlineData(".1125e1+i.56875F1", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' F, not E.
        <InlineData("112.5E-2.2+i5687.5e-3", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Non-integer exponent.
        Sub TryParseStandardDefault_BadInput_Fails(standardStr As String, real As Double, imaginary As Double)
            Dim Cplx As New Numerics.Complex
            If TryParseStandard(standardStr, Nothing, Nothing, Cplx) Then
                Assert.Fail("Parsed despite bad entry.")
            End If
            Assert.False(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))
        End Sub

    End Class ' TestTryParseStandardDefault

    Public Class TestTryParseStandardDefaultMixed

        <Theory>
        <InlineData("111111.125+i555555.6875", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' A+Bi.
        <InlineData("-111111.125+555555.6875i", -TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' A+Bi.
        <InlineData("+111111.125 - i555555.6875", TestVals.SAMEREAL, -TestVals.SAMEIMAG)> ' Open, one space.
        <InlineData(" -111111.125  -   555555.6875i  ", -TestVals.SAMEREAL, -TestVals.SAMEIMAG)> ' Open, asymmetric spaces.
        <InlineData("-111111.125+ i555555.6875", -TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Open, space one side.
        <InlineData("-111111.125 +i555555.6875", -TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Open, space one side.
        <InlineData("111111125e-3+i.5555556875E6", TestVals.SAMEREAL, TestVals.SAMEIMAG)> ' Exponential notation, upper and lower E.
        Sub TryParse_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Cplx As New Numerics.Complex
            If Not TryParseStandard(standardStr, Nothing, Nothing, Cplx) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))
        End Sub

    End Class ' TestTryParseStandardDefaultMixed

    Public Class TestTryParseStandardEnforceStandardization

        Const TightEnforcement As OsnwNumSS =
            OsnwNumSS.EnforceSequence Or OsnwNumSS.EnforceSpacing

        <Theory>
        <InlineData("111111.125+i555555.6875", TestVals.SAMEREAL, TestVals.SAMEIMAG, OsnwNumSS.ClosedABi)>
        <InlineData("-111111.125+555555.6875i", -TestVals.SAMEREAL, TestVals.SAMEIMAG, OsnwNumSS.ClosedAiB)>
        <InlineData("111111.125 - i555555.6875", TestVals.SAMEREAL, -TestVals.SAMEIMAG, OsnwNumSS.OpenABi)>
        <InlineData("-111111.125 - 555555.6875i", -TestVals.SAMEREAL, -TestVals.SAMEIMAG, OsnwNumSS.OpenAiB)>
        Sub TryParse_ValidStandardization_Succeeds(standardStr As String, real As Double,
                                                   imaginary As Double, stdStyle As OsnwNumSS)
            Dim Cplx As New Numerics.Complex
            If Not TryParseStandard(standardStr, stdStyle, Nothing, Cplx) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + 5.6875i'", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not closed.
        <InlineData("-1.125+5.6875i", OsnwNumSS.ClosedAiB Or TightEnforcement)> ' Not AiB.
        <InlineData("1.125-5.6875i", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not Open.
        <InlineData("-1.125 - 5.6875i", OsnwNumSS.OpenAiB Or TightEnforcement)> ' Not AiB.
        Sub TryParse_InvalidStandardization_Fails(standardStr As String, stdStyle As OsnwNumSS)
            Dim Cplx As New Numerics.Complex
            Assert.False(TryParseStandard(standardStr, stdStyle, Nothing, Cplx))
        End Sub

    End Class ' TestTryParseStandardEnforceStandardization

    Public Class TestTryParseStandardCulture

        <Theory>
        <InlineData("111111.125+i555555.6875", TestVals.SAMEREAL, TestVals.SAMEIMAG, 0)>
        <InlineData("-111111.125+i555555.6875", -TestVals.SAMEREAL, TestVals.SAMEIMAG, 1)> ' When current is "en-US".
        <InlineData("111111.125-555555.6875i", TestVals.SAMEREAL, -TestVals.SAMEIMAG, 2)> ' A+Bi, i at end.
        <InlineData("-111111.125 - i555555.6875", -TestVals.SAMEREAL, -TestVals.SAMEIMAG, 3)> ' Open, one space.
        <InlineData("111111,125+i555555,6875", TestVals.SAMEREAL, TestVals.SAMEIMAG, 4)> ' Comma decimal.
        <InlineData("111" & CHARNNBSP & "111,125+i555" & CHARNNBSP & "555,6875",
                    TestVals.SAMEREAL, TestVals.SAMEIMAG, 5)> ' Comma decimal, Non-breaking space.
        <InlineData("111111" & CHARARABCOMMA66B & "125+555555" & CHARARABCOMMA66B & "6875i",
                    TestVals.SAMEREAL, TestVals.SAMEIMAG, 6)> ' Arabic comma CHARARABCOMMA66B.
        Sub TryParseStandard_Culture_Succeeds(standardStr As String, real As Double, imaginary As Double,
                                              index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False),
                New CultureInfo("ar-001", False)
                }
            Dim Cplx As New Numerics.Complex

            If Not TryParseStandard(standardStr, Nothing, Providers(index), Cplx) Then
                Assert.Fail("Failed to parse.")
            End If

            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))

        End Sub

    End Class ' TestTryParseStandardCulture

End Namespace ' TryParseStandardTests
