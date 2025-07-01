Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports Xunit
Imports OSNW.Numerics.ComplexExtensions

Namespace ListValues

    Public Class ConfirmStandardizationValues

        <Theory>
        <InlineData(StandardizationStyles.AiB, 1)>
        <InlineData(StandardizationStyles.Open, 2)>
        <InlineData(StandardizationStyles.EnforceSequence, 4)>
        <InlineData(StandardizationStyles.EnforceSpacing, 8)>
        Public Sub ConfirmBinaryValues(style As StandardizationStyles, expected As Integer)
            Assert.Equal(expected, CInt(style))
        End Sub

        <Theory>
        <InlineData(StandardizationStyles.ClosedABi, 0)>
        <InlineData(StandardizationStyles.ClosedAiB, 1)>
        <InlineData(StandardizationStyles.OpenABi, 2)>
        <InlineData(StandardizationStyles.OpenAiB, 3)>
        Public Sub ConfirmShorthandValues(style As StandardizationStyles, expected As Integer)
            Assert.Equal(expected, CInt(style))
        End Sub

        <Theory>
        <InlineData(StandardizationStyles.EnforceBoth, 12)>
        <InlineData(StandardizationStyles.EnforcedClosedABi, 12)>
        <InlineData(StandardizationStyles.EnforcedClosedAiB, 13)>
        <InlineData(StandardizationStyles.EnforcedOpenABi, 14)>
        <InlineData(StandardizationStyles.EnforcedOpenAiB, 15)>
        Public Sub ConfirmEnforcedValues(style As StandardizationStyles, expected As Integer)
            Assert.Equal(expected, CInt(style))
        End Sub

    End Class ' ConfirmStandardizationValues

End Namespace ' ListValues

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
        <InlineData(1.125, -5.675, StandardizationStyles.AiB, "1.125-i5.675")>
        <InlineData(-1.125, 5.675, StandardizationStyles.Open, "-1.125 + 5.675i")>
        <InlineData(-1.125, -5.675, StandardizationStyles.OpenAiB, "-1.125 - i5.675")>
        Sub ToStandardString_Standardization_Succeeds(real As Double, imaginary As Double,
                                                      standardizationStyle As StandardizationStyles, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString(standardizationStyle)
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
        <InlineData(1.122, 5.677, 2, "1.122+5.677i")>
        <InlineData(111_111.122, -555_555.677, 3, "111111,122-555555,677i")> ' One round down, one up.
        <InlineData(-111_111.125, 555_555.675, 4, "-111111,125+555555,675i")>
        Sub ToStandardString_Culture_Succeeds(
            real As Double, imaginary As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
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

        Const TightEnforcement As StandardizationStyles =
            StandardizationStyles.EnforceSequence Or StandardizationStyles.EnforceSpacing

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675, StandardizationStyles.ClosedABi)>
        <InlineData("1.125-5.675i", 1.125, -5.675, StandardizationStyles.ClosedAiB)>
        <InlineData("-1.125 + i5.675", -1.125, 5.675, StandardizationStyles.OpenABi)>
        <InlineData("-1.125 - 5.675i", -1.125, -5.675, StandardizationStyles.OpenAiB)>
        Sub TryParse_ValidStandardization_Succeeds(standardStr As String, real As Double, imaginary As Double,
                                                       standardizationStyle As StandardizationStyles)
            Dim Cplx As New Numerics.Complex
            If Not TryParseStandard(standardStr, standardizationStyle, Nothing, Cplx) Then
                Assert.True(False)
            End If
            Assert.True(Cplx.Real.Equals(real) AndAlso Cplx.Imaginary.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + i5.675", StandardizationStyles.ClosedABi Or TightEnforcement)>
        <InlineData("1.125-i5.675", StandardizationStyles.ClosedAiB Or TightEnforcement)>
        <InlineData("-1.125+i5.675", StandardizationStyles.OpenABi Or TightEnforcement)>
        <InlineData("-1.125 - i5.675", StandardizationStyles.OpenAiB Or TightEnforcement)>
        Sub TryParse_InvalidStandardization_Fails(standardStr As String,
                                                       standardizationStyle As StandardizationStyles)
            Dim Cplx As New Numerics.Complex
            Assert.False(TryParseStandard(standardStr, standardizationStyle, Nothing, Cplx))
        End Sub

    End Class ' TryParseStandardEnforceStandardizationTest

    Public Class TryParseStandardCultureTest

        <Theory>
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 0)> ' One round down, one up.
        <InlineData("111111.122-i555555.677", 111_111.122, -555_555.677, 1)> ' One round down, one up.
        <InlineData("1.122+i5.677", 1.122, 5.677, 2)>
        <InlineData("111111,122-i555555,677", 111_111.122, -555_555.677, 3)> ' One round down, one up.
        <InlineData("-111111,125+i555555,675", -111_111.125, 555_555.675, 4)>
        Sub TryParse_Culture_Succeeds(standardStr As String, real As Double, imaginary As Double,
                                     index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
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
