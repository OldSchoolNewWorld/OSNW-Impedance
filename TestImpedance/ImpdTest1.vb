Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System
Imports System.Globalization
Imports OSNW.Numerics
Imports Xunit


Namespace TestToString

    Public Class ToStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "<1.125; 5.675>")>
        <InlineData(1.125, -5.675, "<1.125; -5.675>")>
        <InlineData(-1.125, 5.675, "<-1.125; 5.675>")>
        <InlineData(-1.125, -5.675, "<-1.125; -5.675>")>
        Sub ToString_Default_Succeeds(r As Double, x As Double, expect As String)
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToString()
            Assert.Equal(expect, ImpdStr)
        End Sub

    End Class ' ToStringDefaultTest

End Namespace ' TestToString

Namespace TestToStandardString

    Public Class ToStandardStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "1.125+j5.675")>
        <InlineData(1.125, -5.675, "1.125-j5.675")>
        <InlineData(-1.125, 5.675, "-1.125+j5.675")>
        <InlineData(-1.125, -5.675, "-1.125-j5.675")>
        Sub ToStandardString_Default_Succeeds(r As Double, x As Double, expect As String)
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToStandardString()
            Assert.Equal(expect, ImpdStr)
        End Sub

    End Class ' ToStandardStringDefaultTest

    Public Class ToStandardStringStandardizationTest

        <Theory>
        <InlineData(1.125, 5.675, Nothing, "1.125+j5.675")>
        <InlineData(1.125, -5.675, StandardizationStyles.ABi, "1.125-5.675j")>
        <InlineData(-1.125, 5.675, StandardizationStyles.Open, "-1.125 + j5.675")>
        <InlineData(-1.125, -5.675, StandardizationStyles.OpenABi, "-1.125 - 5.675j")>
        Sub ToStandardString_Standardization_Succeeds(r As Double, x As Double,
                                                      standardizationStyle As StandardizationStyles, expected As String)
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToStandardString(standardizationStyle)
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' ToStandardStringStandardizationTest

    Public Class ToStandardStringFormatTest

        <Theory>
        <InlineData(1.122, 5.677, "F2", "1.12+j5.68")>
        <InlineData(111_111.122, -555_555.677, "N2", "111,111.12-j555,555.68")>
        <InlineData(-111_111.125, 555_555.675, "G5", "-1.1111E+05+j5.5556E+05")>
        Sub ToStandardString_Format_Succeeds(r As Double, x As Double, format As String, expect As String)
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expect, ImpdStr)
        End Sub

    End Class ' ToStandardStringFormatTest

    Public Class ToStandardStringCultureTest

        <Theory>
        <InlineData(111_111.122, -555_555.677, 0, "111111.122-j555555.677")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 1, "111111.122-j555555.677")> ' One round down, one up.
        <InlineData(1.122, 5.677, 2, "1.122+j5.677")>
        <InlineData(111_111.122, -555_555.677, 3, "111111,122-j555555,677")> ' One round down, one up.
        <InlineData(-111_111.125, 555_555.675, 4, "-111111,125+j555555,675")>
        Sub ToStandardString_Culture_Succeeds(
            r As Double, x As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Z As New OSNW.Numerics.Impedance(r, x)

            Dim ImpdStr As String = Z.ToStandardString(Nothing, Providers(index))

            Assert.Equal(expected, ImpdStr)

        End Sub

    End Class ' ToStandardStringCultureTest

End Namespace ' TestToStandardString

Namespace TestTryParseStandard

    Public Class TryParseStandardDefaultTest

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)>
        <InlineData("1.125-i5.675", 1.125, -5.675)>
        <InlineData("-1.125+i5.675", -1.125, 5.675)>
        <InlineData("-1.125-i5.675", -1.125, -5.675)>
        Sub TryParseStandard_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Nothing, Impd) Then
                Assert.True(False)
            End If
            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))
        End Sub

    End Class ' TryParseStandardDefaultTest

    Public Class TryParseStandardDefaultMixedTest

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)> ' A+iB.
        <InlineData("1.125-5.675i", 1.125, -5.675)> ' A+Bi.
        <InlineData("-1.125 + i5.675", -1.125, 5.675)> ' Open, one space.
        <InlineData(" -1.125  -   5.675i  ", -1.125, -5.675)> ' Open, asymmetric spaces.
        <InlineData("-1.125+ i5.675", -1.125, 5.675)> ' Open, space one side.
        <InlineData("-1.125 +i5.675", -1.125, 5.675)> ' Open, space one side.
        <InlineData("1125e-3+i.5675E1", 1.125, 5.675)> ' Exponential notation.
        Sub TryParseStandard_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Nothing, Impd) Then
                Assert.True(False)
            End If
            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))
        End Sub

    End Class ' TryParseStandardDefaultMixedTest

    Public Class TryParseStandardEnforceStandardizationTest

        Const TightEnforcement As StandardizationStyles =
            StandardizationStyles.EnforceSequence Or StandardizationStyles.EnforceSpacing

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675, StandardizationStyles.ClosedAiB)>
        <InlineData("1.125-5.675i", 1.125, -5.675, StandardizationStyles.ClosedABi)>
        <InlineData("-1.125 + i5.675", -1.125, 5.675, StandardizationStyles.OpenAiB)>
        <InlineData("-1.125 - 5.675i", -1.125, -5.675, StandardizationStyles.OpenABi)>
        Sub TryParseStandard_CompliantStandardization_Succeeds(standardStr As String, real As Double, imaginary As Double,
                                                       standardizationStyle As StandardizationStyles)
            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, standardizationStyle, Nothing, Impd) Then
                Assert.True(False)
            End If
            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + i5.675", StandardizationStyles.ClosedAiB Or TightEnforcement)>
        <InlineData("1.125-i5.675", StandardizationStyles.ClosedABi Or TightEnforcement)>
        <InlineData("-1.125+i5.675", StandardizationStyles.OpenAiB Or TightEnforcement)>
        <InlineData("-1.125 - i5.675", StandardizationStyles.OpenABi Or TightEnforcement)>
        Sub TryParseStandard_NonCompliantStandardization_Fails(standardStr As String,
                                                       standardizationStyle As StandardizationStyles)
            Dim Impd As New OSNW.Numerics.Impedance
            Assert.False(OSNW.Numerics.Impedance.TryParseStandard(standardStr, standardizationStyle, Nothing, Impd))
        End Sub

    End Class ' TryParseStandardEnforceStandardizationTest

    Public Class TryParseStandardCultureTest

        <Theory>
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 0)> ' One round down, one up.
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 1)> ' One round down, one up.
        <InlineData("1.122+i5.677", 1.122, 5.677, 2)>
        <InlineData("111111,122-i555555,677", 111_111.122, -555_555.677, 3)> ' One round down, one up.
        <InlineData("-111111,125+i555555,675", -111_111.125, 555_555.675, 4)>
        Sub TryParseStandard_Culture_Succeeds(standardStr As String, real As Double, imaginary As Double,
                                     index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Impd As New OSNW.Numerics.Impedance

            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Providers(index), Impd) Then
                Assert.True(False)
            End If

            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))

        End Sub

    End Class ' TryParseStandardCultureTest

End Namespace ' TestTryParseStandard
