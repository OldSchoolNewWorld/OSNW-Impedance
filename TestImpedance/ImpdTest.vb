Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports OSNW.Numerics
Imports Xunit
Imports OsnwImpd = OSNW.Numerics.Impedance
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

#Region "Test Data"

Public Class CultureTestVals
    ' A common set of test values for parsing routines.
    Public Const SAMERESISTANCE As Double = 111_111.125 ' 1/8 is good for binary fractions.
    Public Const SAMEREACTANCE As Double = 555_555.687_5 ' 11/16 is good for binary fractions.
End Class

' These are common test data points for Impedance and Smith Chart tests. They
' include some special cases and a mix of left/right, +/-, above/below/on, etc.
' ChartX, ChartY, ChartRad, PlotX, PlotY, RadiusR, RadiusX, RadiusG, and RadiusB
' are in generic "units" relative to a Cartesian plane on which a Smith Chart
' can be drawn.

' Copy an entire list, then delete unused columns as needed to match the process
' under test. After unused columns are stripped, some remaining tests may be
' redundant, resulting in a lower result count than expected. Any rows that
' cause EXPECTED errors can be used as tests of bad data.

' Const Precision As Double = 0.0005
' Const INF As Double = Double.PositiveInfinity

' Test values related to reflection.

'<InlineData(  Z0,        R,         X,    VRC,    PRC,    VTC,    PTC,       AOR,      AOT,    VSWR)> ' Model
'<Theory>
'<InlineData( 1.0,   0.0000,    0.0000, 1.0000, 1.0000, 0.0000, 0.0000,       INF,   2.0000,     INF)> ' A: At the short circuit point. Omit - covered by B.
'<InlineData( 1.0,   0.0000,     1/2.0, 1.0000, 1.0000, 0.8944, 0.0000,       INF,      2.8,     INF)> ' B: Anywhere else on the perimeter. R=0.0.
'<InlineData( 1.0,      INF,    0.0000, 1.0000,    999,    999,    999,       INF,   6.0000,     INF)> ' C: At the open circuit point on the right.
'<InlineData( 1.0,   1.0000,    0.0000, 0.0000, 0.0000, 1.0000, 1.0000,    0.0000,   0.0000,  1.0000)> ' D1: At the center.
'<InlineData(75.0,  75.0000,    0.0000, 0.0000, 0.0000, 1.0000, 1.0000,    0.0000,   0.0000,  1.0000)> ' D75: At the center. Z0=75.
'<InlineData( 1.0,   1.0000,    1.0000, 0.4472, 0.2000, 1.2649,  0.800,   63.4349,  18.4350,  2.6180)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
'<InlineData(50.0,  50.0000,   50.0000, 0.4472, 0.2000, 1.2649,  0.800,   63.4349,  18.4350,  2.6180)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
'<InlineData( 1.0,   1.0000,   -2.0000, 0.7071, 0.5000, 1.5811, 0.5000,  -45.0000, -18.4350,  5.8284)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
'<InlineData(50.0,  50.0000, -100.0000, 0.7071, 0.5000, 1.5811, 0.5000,  -45.0000, -18.4350,  5.8284)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
'<InlineData( 1.0,   2.0000,     1/2.0, 0.3676, 0.1351, 1.3557, 0.8649,   17.1027,   4.5739,  2.1626)> ' G1: Inside R=Z0 circle, above resonance line.
'<InlineData(50.0, 100.0000,   25.0000, 0.3676, 0.1351, 1.3557, 0.8649,   17.1027,   4.5739,  2.1626)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
'<InlineData( 1.0,   3.0000,    0.0000, 0.5000, 0.2500, 1.5000, 0.7500,    0.0000,   0.0000,  3.0000)> ' H1: Inside R=Z0 circle, on line.
'<InlineData(50.0, 150.0000,    0.0000, 0.5000, 0.2500, 1.5000, 0.7500,    0.0000,   0.0000,  3.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
'<InlineData( 1.0,   2.0000,   -2.0000, 0.6200, 0.3846, 1.5689, 0.6154,  -29.7449, -11.3099,  4.2656)> ' I1: Inside R=Z0 circle, below resonance line.
'<InlineData(50.0, 100.0000, -100.0000, 0.6200, 0.3846, 1.5689, 0.6154,  -29.7449, -11.3099,  4.2656)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
'<InlineData( 1.0,    1/2.0,     1/2.0, 0.4472, 0.2000, 0.8944, 0.8000,  116.5651,  26.5651,  2.6180)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
'<InlineData(50.0,  25.0000,   25.0000, 0.4472, 0.2000, 0.8944, 0.8000,  116.5651,  26.5651,  2.6180)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
'<InlineData( 1.0,    1/2.0,    -1/2.0, 0.4472, 0.2000, 0.8944, 0.8000, -116.5651, -26.5651,  2.6180)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
'<InlineData(50.0,  25.0000,  -25.0000, 0.4472, 0.2000, 0.8944, 0.8000, -116.5651, -26.5651,  2.6180)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
'<InlineData( 1.0,    1/3.0,     1/3.0, 0.5423, 0.2941, 0.6860, 0.7059,  139.3987,  30.9638,  3.3699)> ' L1: Inside G=Y0 circle, above resonance line.
'<InlineData(75.0,  25.0000,   25.0000, 0.5423, 0.2941, 0.6860, 0.7059,  139.3987,  30.9638,  3.3699)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
'<InlineData( 1.0,    1/3.0,    0.0000, 0.5000, 0.2500, 0.5000, 0.7500,  180.0000,   0.0000,  3.0000)> ' M1: Inside G=Y0 circle, on line.
'<InlineData(75.0,  25.0000,    0.0000, 0.5000, 0.2500, 0.5000, 0.7500,  180.0000,   0.0000,  3.0000)> ' M75: Inside G=Y0 circle, on line. Z0=75.
'<InlineData( 1.0,    1/2.0,    -1/3.0, 0.3911, 0.1529, 0.7822, 0.8471, -133.7811, -21.1613,  2.2845)> ' N1: Inside G=Y0 circle, below line.
'<InlineData(75.0,  37.5000,  -25.0000, 0.3911, 0.1529, 0.7822, 0.8471, -133.7811, -21.1613,  2.2845)> ' N75: Inside G=Y0 circle, below line. Z0=75.
'<InlineData( 1.0,   0.2000,    1.4000, 0.8745, 0.7647, 1.5340, 0.2353,  70.34617,  32.4712, 14.9330)> ' O1: In the top center.
'<InlineData(50.0,  10.0000,   70.0000, 0.8745, 0.7647, 1.5340, 0.2353,  70.34617,  32.4712, 14.9330)> ' O50: In the top center. Z0=50.
'<InlineData( 1.0,   0.4000,   -0.8000, 0.6200, 0.3846, 1.1094, 0.6154,  -97.1250, -33.6901,  4.2656)> ' P1: In the bottom center.
'<InlineData(50.0,  20.0000,  -40.0000, 0.6200, 0.3846, 1.1094, 0.6154,  -97.1250, -33.6901,  4.2656)> ' P50: In the bottom center. Z0=50.
'<InlineData( 1.0,  -0.0345,    0.4138, 1.0000,    999,    999,    999,       999,      2.5,     999)> ' Q: Outside of main circle. Invalid.
'<InlineData( 1.0,  -2.0000,       999,    999,    999,    999,    999,       999,      999,     999)> ' R: NormR<=0. Invalid.

' Test values related to Smith Chart geometry.

'<InlineData(ChartX, ChartY, ChartRad,   Z0,        R,         X,        G,         B,  PlotX,  PlotY, RadiusR, RadiusX, RadiusG, RadiusB, RadiusV)> ' Model
'<Theory>
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   0.0000,    0.0000,   0.0000,    0.0000, 2.0000, 5.0000,  2.0000,     INF,  0.0000,     INF,  2.0000)> ' A: At the short circuit point. Omit - covered by B.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   0.0000,     1/2.0,   0.0000,   -2.0000,    2.8,    6.6,  2.0000,  4.0000,     INF,  1.0000,  2.0000)> ' B: Anywhere else on the perimeter. R=0.0.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,      INF,    0.0000,   0.0000,    0.0000, 6.0000, 5.0000,  0.0000,     INF,  2.0000,     INF,  2.0000)> ' C: At the open circuit point on the right.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   1.0000,    0.0000,   1.0000,    0.0000, 4.0000, 5.0000,  1.0000,     INF,  1.0000,     INF,  0.0000)> ' D1: At the center.
'<InlineData(4.0000, 5.0000,   2.0000, 75.0,  75.0000,    0.0000,   1/75.0,    0.0000, 4.0000, 5.0000,  1.0000,     INF,  1.0000,     INF,  0.0000)> ' D75: At the center. Z0=75.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   1.0000,    1.0000,   0.5000,   -0.5000, 4.4000, 5.8000,  1.0000,  2.0000,   4.0/3,  4.0000,  0.8944)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0,  50.0000,   50.0000,   0.0100,   -0.0100, 4.4000, 5.8000,  1.0000,  2.0000,   4.0/3,  4.0000,  0.8944)> ' E50: On R=Z0 circle, above resonance line. Only needs reactance. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   1.0000,   -2.0000,   0.2000,    0.4000, 5.0000, 4.0000,  1.0000,  1.0000,   5.0/3,  5.0000,  1.4142)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0,  50.0000, -100.0000,   0.0040,    0.0080, 5.0000, 4.0000,  1.0000,  1.0000,   5.0/3,  5.0000,  1.4142)> ' F50: On R=Z0 circle, below resonance line. Only needs reactance. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   2.0000,     1/2.0,   8/17.0,   -2/17.0, 4.7027, 5.2162,   2.0/3,  4.0000,  1.3600, 17.0000,  0.7352)> ' G1: Inside R=Z0 circle, above resonance line.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0, 100.0000,   25.0000,  4/425.0,  -1/425.0, 4.7027, 5.2162,   2.0/3,  4.0000,  1.3600, 17.0000,  0.7352)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   3.0000,    0.0000,    1/3.0,    0.0000, 5.0000, 5.0000,  0.5000,     INF,  1.5000,     INF,  1.0000)> ' H1: Inside R=Z0 circle, on line.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0, 150.0000,    0.0000,   0.02/3,    0.0000, 5.0000, 5.0000,  0.5000,     INF,  1.5000,     INF,  1.0000)> ' H50: Inside R=Z0 circle, on line. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   2.0000,   -2.0000,   0.2500,    0.2500, 5.0769, 4.3846,   2.0/3,  1.0000,  1.6000,  8.0000,  1.2404)> ' I1: Inside R=Z0 circle, below resonance line.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0, 100.0000, -100.0000,   0.0050,    0.0050, 5.0769, 4.3846,   2.0/3,  1.0000,  1.6000,  8.0000,  1.2404)> ' I50: Inside R=Z0 circle, below resonance line. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,    1/2.0,     1/2.0,   1.0000,   -1.0000, 3.6000, 5.8000,   4.0/3,  4.0000,  1.0000,  2.0000,  0.8944)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0,  25.0000,   25.0000,   0.0200,   -0.0200, 3.6000, 5.8000,   4.0/3,  4.0000,  1.0000,  2.0000,  0.8944)> ' J50: On G=Y0 circle, above resonance line. Only needs reactance. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,    1/2.0,    -1/2.0,   1.0000,    1.0000, 3.6000, 4.2000,   4.0/3,  4.0000,  1.0000,  2.0000,  0.8944)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0,  25.0000,  -25.0000,   0.0200,    0.0200, 3.6000, 4.2000,   4.0/3,  4.0000,  1.0000,  2.0000,  0.8944)> ' K50: On G=Y0 circle, below resonance line. Only needs reactance. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,    1/3.0,     1/3.0,   1.5000,   -1.5000, 3.1765, 5.7059,  1.5000,  6.0000,  0.8000,  1.3333,  1.0846)> ' L1: Inside G=Y0 circle, above resonance line.
'<InlineData(4.0000, 5.0000,   2.0000, 75.0,  25.0000,   25.0000,   0.0200,   -0.0200, 3.1765, 5.7059,  1.5000,  6.0000,  0.8000,  1.3333,  1.0846)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,    1/3.0,    0.0000,   3.0000,    0.0000, 3.0000, 5.0000,  1.5000,     INF,  0.5000,     INF,  1.0000)> ' M1: Inside G=Y0 circle, on line.
'<InlineData(4.0000, 5.0000,   2.0000, 75.0,  25.0000,    0.0000,   0.0400,    0.0000, 3.0000, 5.0000,  1.5000,     INF,  0.5000,     INF,  1.0000)> ' M75: Inside G=Y0 circle, on line. Z0=75.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,    1/2.0,    -1/3.0,  18/13.0,   12/13.0, 3.4588, 4.4353,   4.0/3,  6.0000,  0.8387,  2.1666,  0.7822)> ' N1: Inside G=Y0 circle, below line.
'<InlineData(4.0000, 5.0000,   2.0000, 75.0,  37.5000,  -25.0000,  6/325.0,   4/325.0, 3.4588, 4.4353,   4.0/3,  6.0000,  0.8387,  2.1666,  0.7822)> ' N75: Inside G=Y0 circle, below line. Z0=75.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   0.2000,    1.4000,   0.1000,   -0.7000, 4.5882, 6.6471,   5.0/3,  1.4286,  1.8182,  2.8571,  1.7489)> ' O1: In the top center.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0,  10.0000,   70.0000,   0.0020,   -0.0140, 4.5882, 6.6471,   5.0/3,  1.4286,  1.8182,  2.8571,  1.7489)> ' O50: In the top center. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,   0.4000,   -0.8000,   0.5000,    1.0000, 3.8462, 3.7692,  1.4286,  2.5000,   4.0/3,  2.0000,  1.2404)> ' P1: In the bottom center.
'<InlineData(4.0000, 5.0000,   2.0000, 50.0,  20.0000,  -40.0000,   0.0100,    0.0200, 3.8462, 3.7692,  1.4286,  2.5000,   4.0/3,  2.0000,  1.2404)> ' P50: In the bottom center. Z0=50.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,  -0.0345,    0.4138,      999,       999,    2.5,    6.5,     999,     999,     999,     999, RadiusV)> ' Q: Outside of main circle. Invalid.
'<InlineData(4.0000, 5.0000,   2.0000,  1.0,  -2.0000,       999,      999,       999,    999,    999,     999,     999,     999,     999, RadiusV)> ' R: NormR<=0. Invalid.

#End Region ' "Test Data"

Namespace DevelopmentTests
    ' Used as a place to perform ad hoc tests.

    Public Class TestUnitTestExceptions

        ' The first two tests below check for a SPECIFIC exception. The third
        ' test checks for ANY exception.

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

        <Theory>
        <InlineData(-2.0, 999)> ' NormR<=0
        <InlineData(Double.PositiveInfinity, 0.0000)> ' C: At the open circuit point on the right.
        Sub ToAdmittance_BadInput_Fails(r As Double, x As Double)
            Try
                ' Code that throws the exception.
                Dim Imp As New Impedance(r, x)
                Dim Y As Admittance = Imp.ToAdmittance()
            Catch ex As Exception
                Assert.True(True)
                Exit Sub
            End Try
            Assert.True(False, "Did not fail")
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

    Public Class TestToAdmittance

        Const INF As Double = Double.PositiveInfinity
        Const Precision As Double = 0.0005

        '<InlineData(       R,       X,       G,        B)> ' Model
        <Theory>
        <InlineData(0.0000, 0.0000, 0.0000, 0.0000)> ' A: At the short circuit point. Omit - covered by B.
        <InlineData(0.0000, 1 / 2.0, 0.0000, -2.0)> ' B: Anywhere else on the perimeter. R=0.0.
        <InlineData(1.0, 0.0000, 1.0, 0.0000)> ' D1: At the center.
        <InlineData(1.0, 1.0, 0.5, -0.5)> ' E1: On R=Z0 circle, above resonance line. Only needs reactance.
        <InlineData(1.0, -2.0, 0.2, 0.4)> ' F1: On R=Z0 circle, below resonance line. Only needs reactance.
        <InlineData(2.0, 1 / 2.0, 8 / 17.0, -2 / 17.0)> ' G1: Inside R=Z0 circle, above resonance line.
        <InlineData(100.0, 25.0, 4 / 425.0, -1 / 425.0)> ' G50: Inside R=Z0 circle, above resonance line. Z0=50.
        <InlineData(3.0, 0.0000, 1.0 / 3, 0.0000)> ' H1: Inside R=Z0 circle, on line.
        <InlineData(2.0, -2.0, 0.25, 0.25)> ' I1: Inside R=Z0 circle, below resonance line.
        <InlineData(1 / 2.0, 1 / 2.0, 1.0, -1.0)> ' J1: On G=Y0 circle, above resonance line. Only needs reactance.
        <InlineData(1 / 2.0, -1 / 2.0, 1.0, 1.0)> ' K1: On G=Y0 circle, below resonance line. Only needs reactance.
        <InlineData(1 / 3.0, 1 / 3.0, 1.5, -1.5)> ' L1: Inside G=Y0 circle, above resonance line.
        <InlineData(25.0, 25.0, 0.02, -0.02)> ' L75: Inside G=Y0 circle, above resonance line. Z0=75.
        <InlineData(1 / 3.0, 0.0000, 3.0, 0.0000)> ' M1: Inside G=Y0 circle, on line.
        <InlineData(1 / 2.0, -1 / 3.0, 18 / 13.0, 12 / 13.0)> ' N1: Inside G=Y0 circle, below line.
        <InlineData(0.2, 1.4, 0.1, -0.7)> ' O1: In the top center.
        <InlineData(10.0, 70.0, 0.002, -0.014)> ' O50: In the top center. Z0=50.
        <InlineData(0.4, -0.8, 0.5, 1.0)> ' P1: In the bottom center.
        <InlineData(20.0, -40.0, 0.01, 0.02)> ' P50: In the bottom center. Z0=50.
        Sub ToAdmittance_GoodInput_Succeeds(r As Double, x As Double, expectG As Double, expectB As Double)
            Dim Imp As New Impedance(r, x)
            Dim Y As Admittance = Imp.ToAdmittance()
            Assert.Equal(expectG, Y.Conductance, Precision)
            Assert.Equal(expectB, Y.Susceptance, Precision)
        End Sub

        '<InlineData(       R,       X)> ' Model
        <Theory>
        <InlineData(INF, 0.0000)> ' C: At the open circuit point on the right.
        <InlineData(-0.0345, 0.4138)> ' Q: Outside of main circle. Invalid.
        <InlineData(-2.0, 999)> ' R: NormR<=0. Invalid.
        Sub ToAdmittance_BadInput_Fails1(r As Double, x As Double)
            Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
                Sub()
                    ' Code that throws the exception
                    Dim Imp As New Impedance(r, x)
                    Dim Y As Admittance = Imp.ToAdmittance()
                End Sub)
        End Sub

        ''<InlineData(       R,       X)> ' Model
        '<Theory>
        '<InlineData(INF, 0.0000)> ' C: At the open circuit point on the right.
        '<InlineData(-0.0345, 0.4138)> ' Q: Outside of main circle. Invalid.
        '<InlineData(-2.0, 999)> ' R: NormR<=0. Invalid.
        'Sub ToAdmittance_BadInput_Fails2(r As Double, x As Double)
        '    Try
        '        ' Code that throws the exception.
        '        Dim Imp As New Impedance(r, x)
        '        Dim Y As Admittance = Imp.ToAdmittance()
        '    Catch ex As Exception
        '        Assert.True(True)
        '        Exit Sub
        '    End Try
        '    Assert.True(False, "Did not fail")
        'End Sub

    End Class ' TestToAdmittance

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
