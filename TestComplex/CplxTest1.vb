Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports Xunit
Imports OSNW.Numerics.ComplexExtensions
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

Namespace ListValues

    Public Class TestStandardizationStyles

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

    End Class ' ConfirmStandardizationValues

End Namespace ' TestOsnwNumSS

Namespace TestToStandardString

    Public Class ToStandardStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "1.125+5.675i")>
        <InlineData(1.125, -5.675, "1.125-5.675i")>
        <InlineData(-1.125, 5.675, "-1.125+5.675i")>
        <InlineData(-1.125, -5.675, "-1.125-5.675i")>
        Sub ToStandardString_Default_Succeeds(real As Double, imaginary As Double, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' ToStandardStringDefaultTest

    Public Class ToStandardStringStandardizationTest

        <Theory>
        <InlineData(1.125, 5.675, Nothing, "1.125+5.675i")>
        <InlineData(1.125, -5.675, OsnwNumSS.AiB, "1.125-i5.675")>
        <InlineData(-1.125, 5.675, OsnwNumSS.Open, "-1.125 + 5.675i")>
        <InlineData(-1.125, -5.675, OsnwNumSS.OpenAiB, "-1.125 - i5.675")>
        Sub ToStandardString_Standardization_Succeeds(real As Double, imaginary As Double,
                                                      stdStyle As OsnwNumSS, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString(stdStyle)
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' ToStandardStringStandardizationTest

    Public Class ToStandardStringFormatTest

        <Theory>
        <InlineData(1.122, 5.677, "F2", "1.12+5.68i")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, "N2", "111,111.12-555,555.68i")> ' One round down, one up.
        <InlineData(-111_111.125, 555_555.675, "G5", "-1.1111E+05+5.5556E+05i")>
        Sub ToStandardString_Format_Succeeds(real As Double, imaginary As Double, format As String, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' ToStandardStringFormatTest

    Public Class ToStandardStringCultureTest

        <Theory>
        <InlineData(111_111.122, -555_555.677, 0, "111111.122-555555.677i")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 1, "111111.122-555555.677i")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 2, "111111.122-555555.677i")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 3, "111111.122-555555.677i")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 4, "111111,122-555555,677i")> ' One round down, one up.
        <InlineData(-111_111.125, 555_555.675, 5, "-111111,125+555555,675i")>
        Sub ToStandardString_Culture_Succeeds(
            real As Double, imaginary As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Z As New System.Numerics.Complex(real, imaginary)

            Dim CplxStr As String = Z.ToStandardString(Nothing, Providers(index))

            Assert.Equal(expected, CplxStr)

        End Sub

    End Class ' ToStandardStringCultureTest

End Namespace ' TestToString

Namespace TestTryParseStandard

    Public Class TryParseStandardDefaultTest

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)>
        <InlineData("1.125-i5.675", 1.125, -5.675)>
        <InlineData("-1.125+i5.675", -1.125, 5.675)>
        <InlineData("-1.125-i5.675", -1.125, -5.675)>
        Sub TryParseStandard_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Cplx As New Numerics.Complex
            If Not TryParseStandard(standardStr, Nothing, Nothing, Cplx) Then
                Assert.True(False)
            End If
            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))
        End Sub

    End Class ' TryParseStandardDefaultTest

    Public Class TryParseStandardDefaultMixedTest

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)> ' A+Bi.
        <InlineData("1.125-5.675i", 1.125, -5.675)> ' A+Bi.
        <InlineData("-1.125 + i5.675", -1.125, 5.675)> ' Open, one space.
        <InlineData(" -1.125  -   5.675i  ", -1.125, -5.675)> ' Open, asymmetric spaces.
        <InlineData("-1.125+ i5.675", -1.125, 5.675)> ' Open, space one side.
        <InlineData("-1.125 +i5.675", -1.125, 5.675)> ' Open, space one side.
        <InlineData("1125e-3+i.5675E1", 1.125, 5.675)> ' Exponential notation, upper and lower E.
        Sub TryParse_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Cplx As New Numerics.Complex
            If Not TryParseStandard(standardStr, Nothing, Nothing, Cplx) Then
                Assert.True(False)
            End If
            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))
        End Sub

    End Class ' TryParseStandardDefaultMixedTest

    Public Class TryParseStandardEnforceStandardizationTest

        Const TightEnforcement As OsnwNumSS =
            OsnwNumSS.EnforceSequence Or OsnwNumSS.EnforceSpacing

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675, OsnwNumSS.ClosedABi)>
        <InlineData("1.125-5.675i", 1.125, -5.675, OsnwNumSS.ClosedAiB)>
        <InlineData("-1.125 + i5.675", -1.125, 5.675, OsnwNumSS.OpenABi)>
        <InlineData("-1.125 - 5.675i", -1.125, -5.675, OsnwNumSS.OpenAiB)>
        Sub TryParse_ValidStandardization_Succeeds(standardStr As String, real As Double,
                                                   imaginary As Double, stdStyle As OsnwNumSS)
            Dim Cplx As New Numerics.Complex
            If Not TryParseStandard(standardStr, stdStyle, Nothing, Cplx) Then
                Assert.True(False)
            End If
            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + 5.675i'", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not closed.
        <InlineData("1.125-5.675i", OsnwNumSS.ClosedAiB Or TightEnforcement)> ' Not AiB.
        <InlineData("-1.125+5.675i", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not Open.
        <InlineData("-1.125 - 5.675i", OsnwNumSS.OpenAiB Or TightEnforcement)> ' Not AiB.
        Sub TryParse_InvalidStandardization_Fails(standardStr As String, stdStyle As OsnwNumSS)
            Dim Cplx As New Numerics.Complex
            Assert.False(TryParseStandard(standardStr, stdStyle, Nothing, Cplx))
        End Sub

    End Class ' TryParseStandardEnforceStandardizationTest

    Public Class TryParseStandardCultureTest

        <Theory>
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 0)> ' One round down, one up.
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 1)> ' One round down, one up.
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 2)> ' One round down, one up.
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 3)> ' One round down, one up.
        <InlineData("111111,122-i555555,677", 111_111.122, -555_555.677, 4)> ' One round down, one up.
        <InlineData("-111111,125+i555555,675", -111_111.125, 555_555.675, 5)>
        Sub TryParse_Culture_Succeeds(standardStr As String, real As Double, imaginary As Double,
                                      index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Cplx As New Numerics.Complex

            If Not TryParseStandard(standardStr, Nothing, Providers(index), Cplx) Then
                Assert.True(False)
            End If

            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))

        End Sub

    End Class ' TryParseStandardCultureTest

End Namespace ' TestTryParseStandard
