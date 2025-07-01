Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports Xunit
Imports OSNW.Numerics


Namespace DevelopmentTests

    Public Class TestEquals

        <Fact>
        Sub Equals_GoodMatch_Passes()
            Dim I1 As New Admittance(1, 2)
            Dim I2 As New Admittance(1, 2)
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_Mismatch_Fails()
            Dim I1 As New Admittance(1, 2)
            Dim I2 As New Admittance(2, 3)
            Assert.False(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleDefault_Passes()
            ' What happens the nothing is sent?
            ' Is a null check needed?
            Dim I1 As Admittance
            Dim I2 As Admittance
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleEmpty_Passes()
            ' What happens when nothing is set?
            ' Is a null check needed?
            Dim I1 As New Admittance()
            Dim I2 As New Admittance()
            Assert.True(I1.Equals(Nothing))
        End Sub

        <Fact>
        Sub Equals_Nothing_Fails()
            ' What happens the "Nothing" is sent?
            ' Is a null check needed?
            Dim I1 As New Admittance(1, 2)
            Assert.False(I1.Equals(Nothing))
        End Sub

    End Class

End Namespace ' DevTests




Namespace TestToString

    Public Class ToStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "<1.125; 5.675>")>
        <InlineData(1.125, -5.675, "<1.125; -5.675>")>
        <InlineData(0, 5.675, "<0; 5.675>")>
        <InlineData(0, -5.675, "<0; -5.675>")>
        Sub ToString_Default_Succeeds(g As Double, b As Double, expect As String)
            Dim Y As New OSNW.Numerics.Admittance(g, b)
            Dim AdmtStr As String = Y.ToString()
            Assert.Equal(expect, AdmtStr)
        End Sub

    End Class ' ToStringDefaultTest

End Namespace ' TestToString

Namespace TestToStandardString

    Public Class ToStandardStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "1.125+j5.675")>
        <InlineData(1.125, -5.675, "1.125-j5.675")>
        <InlineData(0, 5.675, "0+j5.675")>
        <InlineData(0, -5.675, "0-j5.675")>
        Sub ToStandardString_Default_Succeeds(g As Double, b As Double, expect As String)
            Dim Y As New OSNW.Numerics.Admittance(g, b)
            Dim AdmtStr As String = Y.ToStandardString()
            Assert.Equal(expect, AdmtStr)
        End Sub

    End Class ' ToStandardStringDefaultTest

    Public Class ToStandardStringStandardizationTest

        <Theory>
        <InlineData(1.125, 5.675, Nothing, "1.125+j5.675")>
        <InlineData(1.125, -5.675, StandardizationStyles.ABi, "1.125-5.675j")>
        <InlineData(0, 5.675, StandardizationStyles.Open, "0 + j5.675")>
        <InlineData(0, -5.675, StandardizationStyles.OpenABi, "0 - 5.675j")>
        Sub ToStandardString_Standardization_Succeeds(
            g As Double, b As Double, standardizationStyle As StandardizationStyles, expected As String)

            Dim Y As New OSNW.Numerics.Admittance(g, b)
            Dim AdmtStr As String = Y.ToStandardString(standardizationStyle)
            Assert.Equal(expected, AdmtStr)
        End Sub

    End Class ' ToStandardStringStandardizationTest

    Public Class ToStandardStringFormatTest

        <Theory>
        <InlineData(1.122, 5.677, "F2", "1.12+j5.68")>
        <InlineData(111_111.122, -555_555.677, "N2", "111,111.12-j555,555.68")>
        <InlineData(111_111.125, 555_555.675, "G5", "1.1111E+05+j5.5556E+05")>
        Sub ToStandardString_Format_Succeeds(g As Double, b As Double, format As String, expect As String)
            ' One round down, one up.
            Dim Y As New OSNW.Numerics.Admittance(g, b)
            Dim AdmtStr As String = Y.ToStandardString(Nothing, format)
            Assert.Equal(expect, AdmtStr)
        End Sub

    End Class ' ToStandardStringFormatTest

    Public Class ToStandardStringCultureTest

        <Theory>
        <InlineData(111_111.122, -555_555.677, 0, "111111.122-j555555.677")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 1, "111111.122-j555555.677")> ' One round down, one up.
        <InlineData(1.122, 5.677, 2, "1.122+j5.677")>
        <InlineData(111_111.122, -555_555.677, 3, "111111,122-j555555,677")> ' One round down, one up.
        <InlineData(111_111.125, 555_555.675, 4, "111111,125+j555555,675")>
        Sub ToStandardString_Culture_Succeeds(
            g As Double, b As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Y As New OSNW.Numerics.Admittance(g, b)

            Dim AdmtStr As String = Y.ToStandardString(Nothing, Providers(index))

            Assert.Equal(expected, AdmtStr)

        End Sub

    End Class ' ToStandardStringCultureTest

End Namespace ' TestToStandardString

Namespace TestTryParseStandard

    Public Class TryParseStandardDefaultTest

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)>
        <InlineData("1.125-i5.675", 1.125, -5.675)>
        <InlineData("0+i5.675", 0, 5.675)>
        <InlineData("0-i5.675", 0, -5.675)>
        Sub TryParseStandard_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Admt As New OSNW.Numerics.Admittance
            If Not OSNW.Numerics.Admittance.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.True(False)
            End If
            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))
        End Sub

    End Class ' TryParseStandardDefaultTest

    Public Class TryParseStandardDefaultMixedTest

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)> ' A+iB.
        <InlineData("1.125-5.675i", 1.125, -5.675)> ' A+Bi.
        <InlineData("0 + i5.675", 0, 5.675)> ' Open, one space.
        <InlineData(" 0  -   5.675i  ", 0, -5.675)> ' Open, asymmetric spaces.
        <InlineData("0+ i5.675", 0, 5.675)> ' Open, space one side.
        <InlineData("0 +i5.675", 0, 5.675)> ' Open, space one side.
        <InlineData("1125e-3+i.5675E1", 1.125, 5.675)> ' Exponential notation.
        Sub TryParseStandard_Default_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Admt As New OSNW.Numerics.Admittance
            If Not OSNW.Numerics.Admittance.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.True(False)
            End If
            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))
        End Sub

    End Class ' TryParseStandardDefaultMixedTest

    Public Class TryParseStandardEnforceStandardizationTest

        Const TightEnforcement As StandardizationStyles =
            StandardizationStyles.EnforceSequence Or StandardizationStyles.EnforceSpacing

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675, StandardizationStyles.ClosedAiB)>
        <InlineData("1.125-5.675i", 1.125, -5.675, StandardizationStyles.ClosedABi)>
        <InlineData("0 + i5.675", 0, 5.675, StandardizationStyles.OpenAiB)>
        <InlineData("0 - 5.675i", 0, -5.675, StandardizationStyles.OpenABi)>
        Sub TryParseStandard_ValidStandardization_Succeeds(standardStr As String, real As Double,
            imaginary As Double, standardizationStyle As StandardizationStyles)

            Dim Admt As New OSNW.Numerics.Admittance
            If Not OSNW.Numerics.Admittance.TryParseStandard(standardStr, standardizationStyle, Nothing, Admt) Then
                Assert.True(False)
            End If
            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + i5.675", StandardizationStyles.ClosedAiB Or TightEnforcement)>
        <InlineData("1.125-i5.675", StandardizationStyles.ClosedABi Or TightEnforcement)>
        <InlineData("-1.125+i5.675", StandardizationStyles.OpenAiB Or TightEnforcement)>
        <InlineData("-1.125 - i5.675", StandardizationStyles.OpenABi Or TightEnforcement)>
        Sub TryParseStandard_InvalidStandardization_Fails(
            standardStr As String, standardizationStyle As StandardizationStyles)

            Dim Admt As New OSNW.Numerics.Admittance
            Assert.False(OSNW.Numerics.Admittance.TryParseStandard(standardStr, standardizationStyle, Nothing, Admt))
        End Sub

    End Class ' TryParseStandardEnforceStandardizationTest

    Public Class TryParseStandardCultureTest

        <Theory>
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 0)> ' One round down, one up.
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 1)> ' One round down, one up.
        <InlineData("1.122+i5.677", 1.122, 5.677, 2)>
        <InlineData("111111,122-i555555,677", 111_111.122, -555_555.677, 3)> ' One round down, one up.
        <InlineData("111111,125+i555555,675", 111_111.125, 555_555.675, 4)>
        Sub TryParseStandard_Culture_Succeeds(
            standardStr As String, real As Double, imaginary As Double, index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Admt As New OSNW.Numerics.Admittance

            If Not OSNW.Numerics.Admittance.TryParseStandard(standardStr, Nothing, Providers(index), Admt) Then
                Assert.True(False)
            End If

            Assert.True(Admt.Conductance.Equals(real) AndAlso Admt.Susceptance.Equals(imaginary))

        End Sub

    End Class ' TryParseStandardCultureTest

End Namespace ' TestTryParseStandard
