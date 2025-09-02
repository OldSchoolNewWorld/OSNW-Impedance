Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports OSNW.Numerics
Imports Xunit
Imports OsnwImpd = OSNW.Numerics.Impedance
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

' Test Data
' These are common test data points for Impedance and Smith Chart tests. They
' include special cases and a mix of +/-, left/right, above/below/on, etc.
' ChartX, ChartY, ChartRad, PlotX, PlotY, RadiusR, RadiusX, RadiusG, and RadiusB
' are in generic "units" relative to the Cartesian plane on which a Smith Chart
' can be drawn.
' Copy the entire list, then delete unused columns as needed to match the
' process under test. After unused columns are stripped, some remaining tests
' may be redundant. Any rows that cause EXPECTED errors can be used as tests of
' bad data.

' Const INF As Double = Double.PositiveInfinity

' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
'<InlineData(ChartX, ChartY, ChartRad,      Z0,        R,       X,      G,       B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Model
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,        R,       X,      G,       B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Base circle
' <Theory>
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      999,     999,    999,     999,    2.5,    6.5, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' Outside of circle
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,  -2.0000,     999,    999,     999,  GridX,  GridY, RadiusR, RadiusX, RadiusG, RadiusB,    VSWR)> ' NormR<=0
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,  0.0000,    INF,  0.0000, 2.0000, 5.0000,  2.0000,     INF,  0.0000,     INF,     INF)> ' A: Short circuit
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.0000,   1/2.0,    INF,  2.0000,    2.8,    6.6,  2.0000,  4.0000,     INF,   1.000,     INF)> ' C: Perimeter
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,      INF,  0.0000, 0.0000,  0.0000,    6.0, 5.0000,  0.0000,     INF,  2.0000,     INF,     INF)> ' B: Open circuit
'
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  0.0000, 1.0000,  0.0000, 4.0000, 5.0000,  1.0000,     INF,  1.0000,     INF,  1.0000)> ' J: Center point
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000,  1.0000, 0.5000, -0.5000,    4.4,    5.8,  1.0000,  2.0000,   4.0/3,  4.0000,  2.6180)> ' On R=Z0 circle, above line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   1.0000, -2.0000, 0.2000,  0.4000, 5.0000, 4.0000,  1.0000,  1.0000,   5.0/3,  5.0000,  5.8284)> ' On R=Z0 circle, below line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000,   1/2.0, 0.4706, -0.1176,  4.703, 5.2162,   2/3.0,  4.0000,  1.3600, 17.0000,  2.1626)> ' Q1: Inside R=Z0 circle, above line
'<InlineData(4.0000, 5.0000,   2.0000, 50.0000, 100.0000, 25.0000, 0.0094, -0.0024,  4.703, 5.2162,   2/3.0,  4.0000,  1.3605, 16.6667,  2.1626)> ' Q2: Inside R=Z0 circle, above line, Z0=50
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   3.0000,  0.0000,  1.0/3,  0.0000, 5.0000, 5.0000,     0.5,     INF,  1.5000,     999,     INF)> ' Inside R=Z0 circle, on line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   2.0000, -2.0000, 0.2500,  0.2500,  5.077,  4.385,   2.0/3,  1.0000,   1.600,  8.0000,  4.2656)> ' M: Inside R=Z0 circle, below line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,   1/2.0, 1.0000,  -1.000,    3.6,    5.8,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, above line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/2.0, 1.0000,  1.0000,    3.6,    4.2,   4/3.0,  4.0000,  1.0000,  2.0000,  2.6180)> ' G=Y0 circle, below line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,  0.0000, 3.0000,  0.0000, 3.0000, 5.0000,     1.5,     INF,  0.5000,     999,     INF)> ' Inside G=Y0 circle, on line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/3.0,   1/3.0, 1.5000, -1.5000,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D1: Inside G=Y0, above line
'<InlineData(4.0000, 5.0000,   2.0000, 75.0000,  25.0000, 25.0000, 0.0200, -0.0200,  3.175,    5.7,     1.5,     6.0,  0.8000,  1.3333,  3.3699)> ' D2: NormZ 1/3 + j1/3, Z0=75
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,    1/2.0,  -1/3.0, 1.3846,  0.9231, 3.4588, 4.4353,   4/3.0,  6.0000,  0.8387,  2.2500,  2.2845)> ' L: Inside G=Y0, below line
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.2000,  1.4000, 0.1000, -0.7000  4.5882, 6.6471,   5/3.0,  1.4286,  1.8182,  2.8571, 14.9330)> ' Top remainder
'<InlineData(4.0000, 5.0000,   2.0000,  1.0000,   0.4000, -0.8000, 0.5000,  1.0000,  3.845,   3.75,  1.4286,     2.5,   4.0/3,  2.0000,  4.2656)> ' Bottom remainder

Public Class CultureTestVals
    ' A common set of test values for parsing routines.
    Public Const SAMERESISTANCE As Double = 111_111.125 ' 1/8 is good for binary fractions.
    Public Const SAMEREACTANCE As Double = 555_555.687_5 ' 11/16 is good for binary fractions.
End Class

Namespace DevelopmentTests
    ' Used as a place for ad hoc tests.

    Public Class TestUnitTestExceptions

        <Fact>
        Public Sub ToString_NegativeResistance_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Z As New OSNW.Numerics.Impedance(-1.125, 5.675)
                    Dim AdmtStr As String = Z.ToString()
                End Sub)
        End Sub

        <Fact>
        Public Sub ToString_NegativeConductance_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Y As New OSNW.Numerics.Admittance(-1.125, 5.675)
                    Dim AdmtStr As String = Y.ToString()
                End Sub)
        End Sub

    End Class ' TestUnitTestExceptions

    Public Class TestCultureStuff

        '    <Fact>
        '    Public Sub ListCulturesNumerics()

        '        ' Use Debug|Windows|Immediate to see the output.
        '        ' That only updates when debugging.

        '        Dim Dbl As Double = 1111_222_333.444_555_666

        '        ' https://zetcode.com/csharp/cultureinfo/
        '        System.Diagnostics.Debug.WriteLine($"{"Culture",-15}{"ISO",-5}{"Display name",-52}{"English Name",-52}{"Output",-20}")
        '        For Each OneCI As CultureInfo In CultureInfo.GetCultures(CultureTypes.AllCultures)
        '            System.Diagnostics.Debug.Write($"{OneCI.Name,-15}")
        '            System.Diagnostics.Debug.Write($"{OneCI.TwoLetterISOLanguageName,-5}")
        '            System.Diagnostics.Debug.Write($"{OneCI.DisplayName,-52}")
        '            System.Diagnostics.Debug.Write($"{OneCI.EnglishName,-52}")
        '            Dim DblStr As String = $"'{Dbl.ToString(OneCI)}'"
        '            System.Diagnostics.Debug.WriteLine($"{DblStr,-20}")
        '        Next

        '    End Sub

    End Class ' TestCultureStuff

End Namespace ' DevTests

Namespace ToStringTests

    Public Class TestToStringDefault

        <Theory>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, "<111111.125; 555555.6875>")>
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, "<111111.125; -555555.6875>")>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, "<111111.125; 555555.6875>")>
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, "<111111.125; -555555.6875>")>
        Sub ToString_Default_Succeeds(r As Double, x As Double, expect As String)
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToString()
            Assert.Equal(expect, ImpdStr)
        End Sub

    End Class ' TestToStringDefault

End Namespace ' ToStringTests

Namespace ToStandardStringTests

    Public Class TestToStandardStringDefault

        <Theory>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, "111111.125+555555.6875j")>
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, "111111.125-555555.6875j")> ' -X
        Sub ToStandardString_Default_Succeeds(resistance As Double, reactance As Double, expected As String)
            Dim Z As New OSNW.Numerics.Impedance(resistance, reactance)
            Dim ImpdStr As String = Z.ToStandardString()
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' TestToStandardStringDefault

    Public Class TestToStandardStringStandardization

        <Theory>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, Nothing, "111111.125+555555.6875j")>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, OsnwNumSS.Open, "111111.125 + 555555.6875j")>
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, OsnwNumSS.AiB, "111111.125-j555555.6875")>
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, OsnwNumSS.OpenAiB, "111111.125 - j555555.6875")>
        Sub ToStandardString_Standardization_Succeeds(resistance As Double, reactance As Double,
                                                      stdStyle As OsnwNumSS, expected As String)
            Dim Z As New OSNW.Numerics.Impedance(resistance, reactance)
            Dim ImpdStr As String = Z.ToStandardString(stdStyle)
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' TestToStandardStringStandardization

    Public Class TestToStandardStringFormat

        <Theory>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, "G", "111111.125+555555.6875j")>
        <InlineData(111_111.122, 111_111.127, "F2", "111111.12+111111.13j")> ' One round down, one up.
        <InlineData(111_111.127, -111_111.122, "N2", "111,111.13-111,111.12j")> ' One round up, one down.
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, "G5", "1.1111E+05-5.5556E+05j")>
        <InlineData(Math.PI, Math.E, "G", "3.141592653589793+2.718281828459045j")>
        Sub ToStandardString_Format_Succeeds(resistance As Double, reactance As Double, format As String, expected As String)
            Dim Z As New OSNW.Numerics.Impedance(resistance, reactance)
            Dim ImpdStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' TestToStandardStringFormat

    Public Class TestToStandardStringCulture

        <Theory>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 0, "111111.125+555555.6875j")>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 1, "111111.125+555555.6875j")>
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, 2, "111111.125-555555.6875j")>
        <InlineData(CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, 3, "111111.125-555555.6875j")>
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 4, "111111,125+555555,6875j")> ' Comma decimal.
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 5, "111111,125+555555,6875j")> ' Comma decimal.
        <InlineData(CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 6,
                    "111111" & CHARARABCOMMA66B & "125+555555" & CHARARABCOMMA66B &
                    "6875j")> ' Arabic comma CHARARABCOMMA66B.
        Sub ToStandardString_Culture_Succeeds(
            resistance As Double, reactance As Double, index As Integer, expected As String)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False),
                New CultureInfo("ar-001", False)
            }
            Dim Z As New OSNW.Numerics.Impedance(resistance, reactance)

            Dim ImpdStr As String = Z.ToStandardString(Nothing, Providers(index))

            Assert.Equal(expected, ImpdStr)

        End Sub

        ' No failure tests. Any valid double values should be allowed.

    End Class ' TestToStandardStringCulture

End Namespace ' ToStandardStringTests

Namespace TryParseStandardTests

    Public Class TestTryParseStandardDefault

        <Theory>
        <InlineData("111111.125+555555.6875j", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)>
        <InlineData("111111.125+j555555.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' A+iB, j in middle.
        <InlineData("111111.125 - 555555.6875j", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE)> ' Open.
        <InlineData("111111.125-555555.6875j", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE)>
        <InlineData("1.11111125E5+.5555556875e6j", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Mixed E/e.
        <InlineData("11111112.5e-2+555555687.5E-3j", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Mixed e/E.
        Sub TryParseStandardDefault_GoodInput_Succeeds(standardStr As String, resistance As Double, reactance As Double)
            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Nothing, Impd) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(resistance.Equals(Impd.Resistance) AndAlso reactance.Equals(Impd.Reactance))
        End Sub

        <Fact>
        Sub TryParseStandardDefault_NegativeResistance_Fails()
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Impd As New OSNW.Numerics.Impedance
                    If OSNW.Numerics.Impedance.TryParseStandard("-111111.125+555555.6875j", Nothing, Nothing, Impd) Then
                        Assert.Fail("Parsed despite bad entry.")
                    End If
                    Assert.False(Impd.Resistance.Equals(-CultureTestVals.SAMERESISTANCE) AndAlso
                                 Impd.Reactance.Equals(CultureTestVals.SAMEREACTANCE))
                End Sub)
        End Sub

        <Theory>
        <InlineData("", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Empty.
        <InlineData("123", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Too short.
        <InlineData("111111.125+555555.6875Q", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Bad char Q.
        <InlineData("111111.125+Q5.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Bad char Q.
        <InlineData("111111.125+555555.6875i", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' i, not j
        <InlineData("111111.125+j555555.6875j", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Excess j.
        <InlineData(".1125e1+j.56875F1", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' F, not E.
        <InlineData("112.5E-2.2+i5687.5e-3", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Non-integer exponent.
        Sub TryParseStandardDefault_BadInput_Fails(standardStr As String, resistance As Double, reactance As Double)
            Dim Impd As New OSNW.Numerics.Impedance
            If OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Nothing, Impd) Then
                Assert.Fail("Parsed despite bad entry.")
            End If
            Assert.False(Impd.Resistance.Equals(resistance) AndAlso Impd.Reactance.Equals(reactance))
        End Sub

    End Class ' TestTryParseStandardDefault

    Public Class TestTryParseStandardDefaultMixed

        <Theory>
        <InlineData("111111.125+j555555.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' A+Bi.
        <InlineData("111111.125+555555.6875j", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' A+Bi.
        <InlineData("+111111.125 - j555555.6875", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE)> ' Open, one space.
        <InlineData(" 111111.125  -   555555.6875j  ", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE)> ' Open, asymmetric spaces.
        <InlineData("111111.125+ j555555.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Open, space one side.
        <InlineData("111111.125 +j555555.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Open, space one side.
        <InlineData("111111125e-3+j.5555556875E6", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE)> ' Exponential notation, upper and lower E.
        Sub TryParse_Default_Succeeds(standardStr As String, resistance As Double, reactance As Double)
            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Nothing, Impd) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Impd.Resistance.Equals(resistance) AndAlso Impd.Reactance.Equals(reactance))
        End Sub

    End Class ' TestTryParseStandardDefaultMixed

    Public Class TestTryParseStandardEnforceStandardization

        Const TightEnforcement As OsnwNumSS =
            OsnwNumSS.EnforceSequence Or OsnwNumSS.EnforceSpacing

        <Theory>
        <InlineData("111111.125+j555555.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, OsnwNumSS.ClosedABi)>
        <InlineData("111111.125+555555.6875j", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, OsnwNumSS.ClosedAiB)>
        <InlineData("111111.125 - j555555.6875", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, OsnwNumSS.OpenABi)>
        <InlineData("111111.125 - 555555.6875j", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, OsnwNumSS.OpenAiB)>
        Sub TryParseStandard_ValidStandardization_Succeeds(
            standardStr As String, resistance As Double, reactance As Double,
            stdStyle As OsnwNumSS)

            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, stdStyle, Nothing, Impd) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Impd.Resistance.Equals(resistance) AndAlso Impd.Reactance.Equals(reactance))
        End Sub

        <Theory>
        <InlineData("1.125 + 5.6875j'", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not closed.
        <InlineData("1.125+5.6875j", OsnwNumSS.ClosedAiB Or TightEnforcement)> ' Not AiB.
        <InlineData("1.125-5.6875j", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not Open.
        <InlineData("1.125 - 5.6875j", OsnwNumSS.OpenAiB Or TightEnforcement)> ' Not AiB.
        Sub TryParseStandard_InvalidStandardization_Fails(
            standardStr As String, stdStyle As OsnwNumSS)

            Dim Impd As New OSNW.Numerics.Impedance
            Assert.False(OSNW.Numerics.Impedance.TryParseStandard(standardStr, stdStyle, Nothing, Impd))
        End Sub

    End Class ' TestTryParseStandardEnforceStandardization

    Public Class TestTryParseStandardCulture

        <Theory>
        <InlineData("111111.125+j555555.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 0)>
        <InlineData("111111.125+j555555.6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 1)> ' When current is "en-US".
        <InlineData("111111.125-555555.6875j", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, 2)> ' A+Bi, i at end.
        <InlineData("111111.125 - j555555.6875", CultureTestVals.SAMERESISTANCE, -CultureTestVals.SAMEREACTANCE, 3)> ' Open, one space.
        <InlineData("111111,125+j555555,6875", CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 4)> ' Comma decimal.
        <InlineData("111" & CHARNNBSP & "111,125+j555" & CHARNNBSP & "555,6875",
                    CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 5)> ' Comma decimal, Non-breaking space.
        <InlineData("111111" & CHARARABCOMMA66B & "125+555555" & CHARARABCOMMA66B & "6875j",
                    CultureTestVals.SAMERESISTANCE, CultureTestVals.SAMEREACTANCE, 6)> ' Arabic comma CHARARABCOMMA66B.
        Sub TryParseStandard_Culture_Succeeds(standardStr As String, resistance As Double, reactance As Double,
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
            Dim Impd As New OSNW.Numerics.Impedance

            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Providers(index), Impd) Then
                Assert.Fail("Failed to parse.")
            End If

            Assert.True(Impd.Resistance.Equals(resistance) AndAlso Impd.Reactance.Equals(reactance))

        End Sub

    End Class ' TestTryParseStandardCulture

End Namespace ' TryParseStandardTests

Namespace MathTests

    Public Class TestEqualsObject

        <Fact>
        Sub EqualsObject_TypeMismatch_Fails1()
            Dim I1 As New OsnwImpd(3, 4)
            Dim C2 As New System.Numerics.Complex(3, 4)
            Assert.False(I1.Equals(C2))
        End Sub

        <Fact>
        Sub EqualsObject_TypeMismatch_Fails2()
            Dim I1 As New OsnwImpd(3, 4)
            Dim C2 As Object = New System.Numerics.Complex(3, 4)
            Assert.False(I1.Equals(C2))
        End Sub

        <Fact>
        Sub EqualsObject_ValueMismatch_Fails()
            Dim I1 As New OsnwImpd(3, 4)
            Dim I2 As Object = New OsnwImpd(4, 5)
            Assert.False(I1.Equals(I2))
        End Sub

    End Class ' TestEqualsObject

    Public Class TestEqualsOther

        <Fact>
        Sub Equals_GoodMatch_Passes()
            Dim I1 As New OsnwImpd(1, 2)
            Dim I2 As New OsnwImpd(1, 2)
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_Mismatch_Fails()
            Dim I1 As New OsnwImpd(1, 2)
            Dim I2 As New OsnwImpd(2, 3)
            Assert.False(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleDefault_Passes()
            Dim I1 As OsnwImpd
            Dim I2 As OsnwImpd
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleEmpty_Passes()
            Dim I1 As New OsnwImpd()
            Dim I2 As New OsnwImpd()
            Assert.True(I1.Equals(Nothing))
        End Sub

        <Fact>
        Sub Equals_Nothing_Fails()
            Dim I1 As New OsnwImpd(1, 2)
            Assert.False(I1.Equals(Nothing))
        End Sub

        <Fact>
        Sub EqualsOther_Match_Passes()
            Dim I1 As New OsnwImpd(1, 2)
            Dim I2 As New OsnwImpd(1, 2)
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub EqualsOther_Mismatch_Fails()
            Dim I1 As New OsnwImpd(1, 2)
            Dim I2 As New OsnwImpd(1, 3)
            Assert.False(I1.Equals(I2))
        End Sub

    End Class ' TestEqualsOther

End Namespace ' MathTests

Namespace SerializationTests

    Public Class TestSerialization

        <Fact>
        Sub Serialize_Simple_Passes()

            Dim Imp As New Impedance(1, 2)
            Dim ExpectedSerialized As String = "{""Resistance"":1,""Reactance"":2}"

            Dim ResultStr As System.String = System.String.Empty
            If Imp.SerializeJSONString(ResultStr) Then
                Assert.True(ExpectedSerialized.Equals(ResultStr))
            Else
                Assert.True(False, "Serialization failed.")
            End If

        End Sub

        <Theory>
        <InlineData(1, 2, "{""Resistance"":1,""Reactance"":2}")>
        <InlineData(1.122, 5.677, "{""Resistance"":1.122,""Reactance"":5.677}")>
        <InlineData(111_111.122, 555_555.677, "{""Resistance"":111111.122,""Reactance"":555555.677}")>
        <InlineData(222_222.127, -555_555.672, "{""Resistance"":222222.127,""Reactance"":-555555.672}")>
        <InlineData(333_333.122, 555_555.672, "{""Resistance"":333333.122,""Reactance"":555555.672}")>
        <InlineData(444_444.127, -555_555.677, "{""Resistance"":444444.127,""Reactance"":-555555.677}")>
        <InlineData(555_555_555.555_555_555, 555_555_555.555_555_555,
                    "{""Resistance"":555555555.5555556,""Reactance"":555555555.5555556}")>
        Sub Serialize_Default_Passes(r As Double, x As Double, expectedStr As String)

            ' Some of the test sets show the impact of (unsurprising) rounding
            ' when the input exceeds the precision limits of a floating-point
            ' value.

            Dim Imp As New Impedance(r, x)

            Dim ResultStr As System.String = System.String.Empty
            If Imp.SerializeJSONString(ResultStr) Then
                Assert.True(expectedStr.Equals(ResultStr))
            Else
                Assert.True(False, "Serialization failed.")
            End If

        End Sub

        <Fact>
        Sub Deserialize_Simple_Passes()

            Dim jsonString As String = "{""Resistance"":1,""Reactance"":2}"

            Dim Imp As Impedance
            If Impedance.DeserializeJSONString(jsonString, Imp) Then
                Assert.True(Imp.Resistance.Equals(1) AndAlso Imp.Reactance.Equals(2))
            Else
                Assert.True(False, "Serialization failed.")
            End If

        End Sub

        ' The last case below intuitively looks like it should fail, but it does not. It passes because
        ' the expected values also get rounded when stored.
        <Theory>
        <InlineData("{""Resistance"":1,""Reactance"":2}", 1, 2)>
        <InlineData("{""Resistance"":1.122,""Reactance"":5.677}", 1.122, 5.677)>
        <InlineData("{""Resistance"":111111.122,""Reactance"":555555.677}", 111_111.122, 555_555.677)>
        <InlineData("{""Resistance"":222222.127,""Reactance"":-555555.672}", 222_222.127, -555_555.672)>
        <InlineData("{""Resistance"":555555555.5555556,""Reactance"":555555555.5555556}",
                    555_555_555.555_555_555, 555_555_555.555_555_555)>
        Sub Deserialize_Default_Passes(jsonString As String, r As Double, x As Double)
            Dim Imp As Impedance
            If Impedance.DeserializeJSONString(jsonString, Imp) Then
                Assert.True(Imp.Resistance.Equals(r) AndAlso Imp.Reactance.Equals(x))
            Else
                Assert.True(False, "Deserialization failed.")
            End If
        End Sub

    End Class ' TestSerialization

End Namespace ' SerializationTests

Namespace TestImpedanceMath

    '
    '
    ' Look for some solved cases to test the math.
    '
    ' Some formulas and schematics are at:
    ' https://www.mathforengineers.com/AC-circuits/formulas-of-impedances-in-ac-circuits.html
    '
    '

    Public Class TestToAdmittance

        Const Precision As Double = 0.0005

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(       R,       X,      G,       B)> ' Model
        <Theory>
        <InlineData(1.0, 0.0000, 1.0, 0.0000)> ' J: Center point
        <InlineData(1.0, 1.0, 0.5, -0.5)> ' On R=Z0 circle, above line
        <InlineData(1.0, -2.0, 0.2, 0.4)> ' On R=Z0 circle, below line
        <InlineData(2.0, 1 / 2.0, 0.4706, -0.1176)> ' Inside R=Z0 circle, above line
        <InlineData(100.0, 25.0, 0.0094, -0.0024)> ' Inside R=Z0 circle, above line
        <InlineData(3.0, 0.0000, 1.0 / 3, 0.0000)> ' Inside R=Z0 circle, on line
        <InlineData(2.0, -2.0, 0.25, 0.25)> ' M: Inside R=Z0 circle, below line
        <InlineData(1 / 2.0, 1 / 2.0, 1.0, -1.0)> ' G=Y0 circle, above line
        <InlineData(1 / 2.0, -1 / 2.0, 1.0, 1.0)> ' G=Y0 circle, below line
        <InlineData(1 / 3.0, 0.0000, 3.0, 0.0000)> ' Inside G=Y0 circle, on line
        <InlineData(1 / 3.0, 1 / 3.0, 1.5, -1.5)> ' D: Inside G=Y0, above line
        <InlineData(25.0, 25.0, 0.02, -0.02)> ' E: NormZ 1/3 + j1/3
        <InlineData(1 / 2.0, -1 / 3.0, 1.3846, 0.9231)> ' L: Inside G=Y0, below line
        <InlineData(0.2, 1.4, 0.1, -0.7)> ' Top remainder
        <InlineData(0.4, -0.8, 0.5, 1.0)> ' Bottom remainder
        Sub ToAdmittance_GoodInput_Succeeds(r As Double, x As Double, expectG As Double, expectB As Double)
            Dim Imp As New Impedance(r, x)
            Dim Y As Admittance = Imp.ToAdmittance()
            Assert.Equal(expectG, Y.Conductance, Precision)
            Assert.Equal(expectB, Y.Susceptance, Precision)
        End Sub

        '<InlineData(999, 999, 999, 999)> ' Outside of circle
        '<InlineData(-2.0, 999, 999, 999)> ' NormR<=0
        '<InlineData(0.0000, 0.0000, INF, 0.0000)> ' A: Short circuit
        '<InlineData(0.0000, 1 / 2.0, INF, 2.0)> ' C: Perimeter
        '<InlineData(INF, 0.0000, 0.0000, 0.0000)> ' B: Open circuit
        'Sub ToAdmittance_BadInput_Fails(r As Double, x As Double, expectG As Double, expectB As Double)
        '    Dim Imp As New Impedance(r, x)
        '    Dim Y As Admittance = Imp.ToAdmittance()
        '    Assert.Equal(expectG, Y.Conductance, Precision)
        '    Assert.Equal(expectB, Y.Susceptance, Precision)
        'End Sub

    End Class ' TestToAdmittance

    Public Class TestVSWR

        Const INF As Double = Double.PositiveInfinity

        ' NOTE: SOME OF THE VALUES BELOW MAY HAVE BEEN TAKEN AS ESTIMATES AND MAY NEED TO BE UPDATED AS MORE TESTS CHECK FOR INCREASED PRECISION.
        '<InlineData(     Z0,        R,       X,    VSWR)> ' Model
        <Theory>
        <InlineData(1.0, 1.0, 0.0000, 1.0)> ' J: Center point
        <InlineData(1.0, 1.0, 1.0, 2.618)> ' On R=Z0 circle, above line
        <InlineData(1.0, 1.0, -2.0, 5.8284)> ' On R=Z0 circle, below line
        <InlineData(1.0, 2.0, 1 / 2.0, 2.1626)> ' Inside R=Z0 circle, above line
        <InlineData(50.0, 100.0, 25.0, 2.1626)> ' Inside R=Z0 circle, above line
        <InlineData(1.0, 3.0, 0.0000, 3.0)> ' Inside R=Z0 circle, on line
        <InlineData(1.0, 2.0, -2.0, 4.2656)> ' M: Inside R=Z0 circle, below line
        <InlineData(1.0, 1 / 2.0, 1 / 2.0, 2.618)> ' G=Y0 circle, above line
        <InlineData(1.0, 1 / 2.0, -1 / 2.0, 2.618)> ' G=Y0 circle, below line
        <InlineData(1.0, 1 / 3.0, 0.0000, 3.0)> ' Inside G=Y0 circle, on line
        <InlineData(1.0, 1 / 3.0, 1 / 3.0, 3.3699)> ' D: Inside G=Y0, above line
        <InlineData(75.0, 25.0, 25.0, 3.3699)> ' E: NormZ 1/3 + j1/3
        <InlineData(1.0, 1 / 2.0, -1 / 3.0, 2.2845)> ' L: Inside G=Y0, below line
        <InlineData(1.0, 0.2, 1.4, 14.933)> ' Top remainder
        <InlineData(1.0, 0.4, -0.8, 4.2656)> ' Bottom remainder
        Public Sub VSWR_GoodInput_Succeeds(z0 As Double, r As Double, x As Double, expectVSWR As Double)

            Const Precision As Double = 0.0005

            Dim Imp As New Impedance(r, x)
            Dim AnsVWSR As Double = Imp.VSWR(z0)
            Assert.Equal(expectVSWR, AnsVWSR, Precision)

        End Sub

        '<Theory>
        '<InlineData(1.0, 999, 999, VSWR)> ' Outside of circle
        '<InlineData(1.0, -2.0, 999, VSWR)> ' NormR<=0
        '<InlineData(1.0, 0.0000, 0.0000, INF)> ' A: Short circuit
        '<InlineData(1.0, 0.0000, 1 / 2.0, INF)> ' C: Perimeter
        '<InlineData(1.0, INF, 0.0000, INF)> ' B: Open circuit
        'Sub VSWR_BadInput_Fails(z0 As Double, r As Double, x As Double)
        '    Dim Imp As New Impedance(r, x)
        '    Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
        '        Sub()
        '            ' Code that throws the exception
        '            Dim AnsVWSR As Double = Imp.VSWR(z0)
        '        End Sub)
        'End Sub

    End Class ' TestVSWR

End Namespace ' TestImpedanceMath
