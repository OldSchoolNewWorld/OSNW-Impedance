Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports Xunit
Imports OSNW.Numerics
Imports OsnwImpd = OSNW.Numerics.Impedance
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

Public Class TestVals
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
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, "<111111.125; 555555.6875>")>
        <InlineData(TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, "<111111.125; -555555.6875>")>
        <InlineData(0, TestVals.SAMEREACTANCE, "<0; 555555.6875>")>
        <InlineData(0, -TestVals.SAMEREACTANCE, "<0; -555555.6875>")>
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
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, "111111.125-555555.6875j")> ' -X
        Sub ToStandardString_Default_Succeeds(resistance As Double, reactance As Double, expected As String)
            Dim Z As New OSNW.Numerics.Impedance(resistance, reactance)
            Dim ImpdStr As String = Z.ToStandardString()
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' TestToStandardStringDefault

    Public Class TestToStandardStringStandardization

        <Theory>
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, Nothing, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, OsnwNumSS.Open, "111111.125 + 555555.6875j")>
        <InlineData(TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, OsnwNumSS.AiB, "111111.125-j555555.6875")>
        <InlineData(TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, OsnwNumSS.OpenAiB, "111111.125 - j555555.6875")>
        Sub ToStandardString_Standardization_Succeeds(resistance As Double, reactance As Double,
                                                      stdStyle As OsnwNumSS, expected As String)
            Dim Z As New OSNW.Numerics.Impedance(resistance, reactance)
            Dim ImpdStr As String = Z.ToStandardString(stdStyle)
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' TestToStandardStringStandardization

    Public Class TestToStandardStringFormat

        <Theory>
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, "G", "111111.125+555555.6875j")>
        <InlineData(111_111.122, 111_111.127, "F2", "111111.12+111111.13j")> ' One round down, one up.
        <InlineData(111_111.127, -111_111.122, "N2", "111,111.13-111,111.12j")> ' One round up, one down.
        <InlineData(TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, "G5", "1.1111E+05-5.5556E+05j")>
        <InlineData(Math.PI, Math.E, "G", "3.141592653589793+2.718281828459045j")>
        Sub ToStandardString_Format_Succeeds(resistance As Double, reactance As Double, format As String, expected As String)
            Dim Z As New OSNW.Numerics.Impedance(resistance, reactance)
            Dim ImpdStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' TestToStandardStringFormat

    Public Class TestToStandardStringCulture

        <Theory>
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 0, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 1, "111111.125+555555.6875j")>
        <InlineData(TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, 2, "111111.125-555555.6875j")>
        <InlineData(TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, 3, "111111.125-555555.6875j")>
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 4, "111111,125+555555,6875j")> ' Comma decimal.
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 5, "111111,125+555555,6875j")> ' Comma decimal.
        <InlineData(TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 6,
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
        <InlineData("111111.125+555555.6875j", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)>
        <InlineData("111111.125+j555555.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' A+iB, j in middle.
        <InlineData("111111.125 - 555555.6875j", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE)> ' Open.
        <InlineData("111111.125-555555.6875j", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE)>
        <InlineData("1.11111125E5+.5555556875e6j", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Mixed E/e.
        <InlineData("11111112.5e-2+555555687.5E-3j", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Mixed e/E.
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
                    Assert.False(Impd.Resistance.Equals(-TestVals.SAMERESISTANCE) AndAlso
                                 Impd.Reactance.Equals(TestVals.SAMEREACTANCE))
                End Sub)
        End Sub

        <Theory>
        <InlineData("", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Empty.
        <InlineData("123", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Too short.
        <InlineData("111111.125+555555.6875Q", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Bad char Q.
        <InlineData("111111.125+Q5.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Bad char Q.
        <InlineData("111111.125+555555.6875i", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' i, not j
        <InlineData("111111.125+j555555.6875j", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Excess j.
        <InlineData(".1125e1+j.56875F1", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' F, not E.
        <InlineData("112.5E-2.2+i5687.5e-3", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Non-integer exponent.
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
        <InlineData("111111.125+j555555.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' A+Bi.
        <InlineData("111111.125+555555.6875j", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' A+Bi.
        <InlineData("+111111.125 - j555555.6875", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE)> ' Open, one space.
        <InlineData(" 111111.125  -   555555.6875j  ", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE)> ' Open, asymmetric spaces.
        <InlineData("111111.125+ j555555.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Open, space one side.
        <InlineData("111111.125 +j555555.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Open, space one side.
        <InlineData("111111125e-3+j.5555556875E6", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE)> ' Exponential notation, upper and lower E.
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
        <InlineData("111111.125+j555555.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, OsnwNumSS.ClosedABi)>
        <InlineData("111111.125+555555.6875j", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, OsnwNumSS.ClosedAiB)>
        <InlineData("111111.125 - j555555.6875", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, OsnwNumSS.OpenABi)>
        <InlineData("111111.125 - 555555.6875j", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, OsnwNumSS.OpenAiB)>
        Sub TryParse_ValidStandardization_Succeeds(standardStr As String, resistance As Double,
                                                   reactance As Double, stdStyle As OsnwNumSS)
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
        Sub TryParse_InvalidStandardization_Fails(standardStr As String, stdStyle As OsnwNumSS)
            Dim Impd As New OSNW.Numerics.Impedance
            Assert.False(OSNW.Numerics.Impedance.TryParseStandard(standardStr, stdStyle, Nothing, Impd))
        End Sub

    End Class ' TestTryParseStandardEnforceStandardization

    Public Class TestTryParseStandardCulture

        <Theory>
        <InlineData("111111.125+j555555.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 0)>
        <InlineData("111111.125+j555555.6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 1)> ' When current is "en-US".
        <InlineData("111111.125-555555.6875j", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, 2)> ' A+Bi, i at end.
        <InlineData("111111.125 - j555555.6875", TestVals.SAMERESISTANCE, -TestVals.SAMEREACTANCE, 3)> ' Open, one space.
        <InlineData("111111,125+j555555,6875", TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 4)> ' Comma decimal.
        <InlineData("111" & CHARNNBSP & "111,125+j555" & CHARNNBSP & "555,6875",
                    TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 5)> ' Comma decimal, Non-breaking space.
        <InlineData("111111" & CHARARABCOMMA66B & "125+555555" & CHARARABCOMMA66B & "6875j",
                    TestVals.SAMERESISTANCE, TestVals.SAMEREACTANCE, 6)> ' Arabic comma CHARARABCOMMA66B.
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
                Assert.True(False, "Serialization failed.")
            End If
        End Sub

    End Class ' TestSerialization

End Namespace ' SerializationTests
