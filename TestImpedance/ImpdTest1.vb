Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System.Globalization
Imports Xunit
Imports OSNW.Numerics
Imports OsnwImpd = OSNW.Numerics.Impedance
Imports OsnwNumSS = OSNW.Numerics.StandardizationStyles

Namespace DevelopmentTests

    Public Class TestEquals

        <Fact>
        Sub Equals_GoodMatch_Passes()
            Dim I1 As New Impedance(1, 2)
            Dim I2 As New Impedance(1, 2)
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_Mismatch_Fails()
            Dim I1 As New Impedance(1, 2)
            Dim I2 As New Impedance(2, 3)
            Assert.False(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleDefault_Passes()
            ' What happens the nothing is sent?
            ' Is a null check needed?
            Dim I1 As Impedance
            Dim I2 As Impedance
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_DoubleEmpty_Passes()
            ' What happens when nothing is set?
            ' Is a null check needed?
            Dim I1 As New Impedance()
            Dim I2 As New Impedance()
            Assert.True(I1.Equals(Nothing))
        End Sub

        <Fact>
        Sub Equals_Nothing_Fails()
            ' What happens the "Nothing" is sent?
            ' Is a null check needed?
            Dim I1 As New Impedance(1, 2)
            Assert.False(I1.Equals(Nothing))
        End Sub

    End Class ' TestEquals

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

End Namespace ' DevTests

Namespace ToStringTests

    Public Class TestToStringDefault

        <Theory>
        <InlineData(1.125, 5.675, "<1.125; 5.675>")>
        <InlineData(1.125, -5.675, "<1.125; -5.675>")>
        <InlineData(0, 5.675, "<0; 5.675>")>
        <InlineData(0, -5.675, "<0; -5.675>")>
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
        <InlineData(1.125, 5.675, "1.125+5.675j")>
        <InlineData(1.125, -5.675, "1.125-5.675j")>
        <InlineData(0, 5.675, "0+5.675j")>
        <InlineData(0, -5.675, "0-5.675j")>
        Sub ToStandardString_Default_Succeeds(r As Double, x As Double, expect As String)
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToStandardString()
            Assert.Equal(expect, ImpdStr)
        End Sub

    End Class ' TestToStandardStringDefault

    Public Class TestToStandardStringStandardization

        <Theory>
        <InlineData(1.125, 5.675, Nothing, "1.125+5.675j")>
        <InlineData(1.125, -5.675, StandardizationStyles.AiB, "1.125-j5.675")>
        <InlineData(0, 5.675, StandardizationStyles.Open, "0 + 5.675j")>
        <InlineData(0, -5.675, StandardizationStyles.OpenAiB, "0 - j5.675")>
        Sub ToStandardString_Standardization_Succeeds(
            r As Double, x As Double, standardizationStyle As StandardizationStyles, expected As String)

            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToStandardString(standardizationStyle)
            Assert.Equal(expected, ImpdStr)
        End Sub

    End Class ' TestToStandardStringStandardization

    Public Class TestToStandardStringFormat

        <Theory>
        <InlineData(1.122, 5.677, "F2", "1.12+5.68j")>
        <InlineData(111_111.122, -555_555.677, "N2", "111,111.12-555,555.68j")>
        <InlineData(111_111.125, 555_555.675, "G5", "1.1111E+05+5.5556E+05j")>
        Sub ToStandardString_Format_Succeeds(r As Double, x As Double, format As String, expect As String)
            ' One round down, one up.
            Dim Z As New OSNW.Numerics.Impedance(r, x)
            Dim ImpdStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expect, ImpdStr)
        End Sub

    End Class ' TestToStandardStringFormat

    Public Class TestToStandardStringCulture

        ' Something that was seen indicated that French could have a space as
        ' the thousands separator, but that does not match that result of the
        ' current .NET 8.0 implementation. Maybe that is allowed for parsing,
        ' but not used by ToString().

        ' Some of the tests below show the impact of (the expected) rounding
        ' when the input exceeds the precision limits of a floating point value.

        <Theory>
        <InlineData(111_111.122, -555_555.677, 0, "111111.122-555555.677j")> ' One round down, one up.
        <InlineData(222_222.122, -555_555.677, 1, "222222.122-555555.677j")> ' One round down, one up.
        <InlineData(1.122, 5.677, 2, "1.122+5.677j")>
        <InlineData(333_333.122, -555_555.677, 3, "333333,122-555555,677j")> ' One round down, one up.
        <InlineData(444_444.125, 555_555.675, 4, "444444,125+555555,675j")>
        <InlineData(555_555_555.555_555_555, -555_555_555.555_555_555, 0, "555555555.5555556-555555555.5555556j")>
        <InlineData(666_666_666.666_666_666, -666_666_666.666_666_666, 1, "666666666.6666666-666666666.6666666j")>
        <InlineData(777_777_777.777_777_777, -777_777_777.777_777_777, 2, "777777777.7777778-777777777.7777778j")>
        <InlineData(888_888_888.888_888_888, -888_888_888.888_888_888, 3, "888888888,8888888-888888888,8888888j")>
        <InlineData(999_999_999.999_999_999, -999_999_999.999_999_999, 4, "1000000000-1000000000j")>
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

    End Class ' TestToStandardStringCulture

End Namespace ' ToStandardStringTests

Namespace TryParseStandardTests


    ' PARSING IS WHERE TO ADD SOME FAILURE TESTS.


    Public Class TestTryParseStandardDefault

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675)>
        <InlineData("1.125-i5.675", 1.125, -5.675)>
        <InlineData(".1125e1+i.5675E1", 1.125, 5.675)>
        <InlineData("112.5E-2+i567.5e-2", 1.125, 5.675)>
        Sub TryParseStandardDefault_GoodInput_Succeeds(standardStr As String, real As Double, imaginary As Double)
            Dim Admt As New OsnwImpd
            If Not OsnwImpd.TryParseStandard(standardStr, Nothing, Nothing, Admt) Then
                Assert.True(False, "Failed to parse.")
            End If
            Assert.True(Admt.Resistance.Equals(real) AndAlso Admt.Reactance.Equals(imaginary), "Parsed with bad conversion.")
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
            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Nothing, Impd) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))
        End Sub

    End Class ' TestTryParseStandardDefaultMixed

    Public Class TestTryParseStandardEnforceStandardization

        Const TightEnforcement As StandardizationStyles =
            StandardizationStyles.EnforceSequence Or StandardizationStyles.EnforceSpacing

        <Theory>
        <InlineData("1.125+i5.675", 1.125, 5.675, StandardizationStyles.ClosedABi)>
        <InlineData("1.125-5.675i", 1.125, -5.675, StandardizationStyles.ClosedAiB)>
        <InlineData("0 + i5.675", 0, 5.675, StandardizationStyles.OpenABi)>
        <InlineData("0 - 5.675i", 0, -5.675, StandardizationStyles.OpenAiB)>
        Sub TryParseStandard_ValidStandardization_Succeeds(standardStr As String, real As Double,
            imaginary As Double, standardizationStyle As StandardizationStyles)

            Dim Impd As New OSNW.Numerics.Impedance
            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, standardizationStyle, Nothing, Impd) Then
                Assert.Fail("Failed to parse.")
            End If
            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + 5.675j'", OsnwNumSS.ClosedABi Or TightEnforcement)> ' Not closed.
        <InlineData("1.125-5.675j", OsnwNumSS.ClosedAiB Or TightEnforcement)> ' Not AiB.
        <InlineData("-1.125+5.675j", OsnwNumSS.OpenABi Or TightEnforcement)> ' Not Open.
        <InlineData("-1.125 - 5.675j", OsnwNumSS.OpenAiB Or TightEnforcement)> ' Not AiB.
        Sub TryParseStandard_InvalidStandardization_Fails(
            standardStr As String, standardizationStyle As StandardizationStyles)

            Dim Impd As New OSNW.Numerics.Impedance
            Assert.False(OSNW.Numerics.Impedance.TryParseStandard(standardStr, standardizationStyle, Nothing, Impd))
        End Sub

    End Class ' TestTryParseStandardEnforceStandardization

    Public Class TestTryParseStandardCulture

        <Theory>
        <InlineData("111111.122-555555.677j", 111_111.122, -555_555.677, 0)> ' One round down, one up.
        <InlineData("111111.122-555555.677j", 111_111.122, -555_555.677, 1)> ' One round down, one up.
        <InlineData("111111.122-555555.677j", 111_111.122, -555_555.677, 2)> ' One round down, one up.
        <InlineData("111111.122-555555.677j", 111_111.122, -555_555.677, 3)> ' One round down, one up.
        <InlineData("111111,122-555555,677j", 111_111.122, -555_555.677, 4)> ' One round down, one up.
        <InlineData("111111,125+555555,675j", 111_111.125, 555_555.675, 5)>
        Sub TryParseStandard_Culture_Succeeds(
            standardStr As String, real As Double, imaginary As Double, index As Integer)

            Dim Providers As System.IFormatProvider() = {
                CultureInfo.InvariantCulture,
                CultureInfo.CurrentCulture,
                New CultureInfo("en-US", False),
                New CultureInfo("en-UK", False),
                New CultureInfo("ru-RU", False),
                New CultureInfo("fr-FR", False)
            }
            Dim Impd As New OSNW.Numerics.Impedance

            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Providers(index), Impd) Then
                Assert.Fail("Failed to parse.")
            End If

            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))

        End Sub

    End Class ' TestTryParseStandardCulture

End Namespace ' TryParseStandardTests

Namespace MathTests

    Public Class TestEqualsObject

        <Fact>
        Sub Equals_MismatchObjectType_Fails1()
            Dim I1 As New Impedance(3, 4)
            Dim C2 As New Admittance(3, 4)
            Assert.False(I1.Equals(C2))
        End Sub

        <Fact>
        Sub Equals_MismatchObjectType_Fails2()
            Dim I1 As New Impedance(3, 4)
            Dim C2 As Object = New Admittance(3, 4)
            Assert.False(I1.Equals(C2))
        End Sub

        <Fact>
        Sub Equals_MismatchObjectValue_Fails()
            Dim I1 As New Impedance(3, 4)
            Dim C2 As Object = New Impedance(4, 5)
            Assert.False(I1.Equals(C2))
        End Sub

    End Class ' TestEqualsObject

    Public Class TestEqualsOther

        <Fact>
        Sub Equals_MatchOther_Passes()
            Dim I1 As New Impedance(1, 2)
            Dim I2 As New Impedance(1, 2)
            Assert.True(I1.Equals(I2))
        End Sub

        <Fact>
        Sub Equals_MismatchOther_Fails()
            Dim I1 As New Impedance(1, 2)
            Dim I2 As New Impedance(1, 3)
            Assert.False(I1.Equals(I2))
        End Sub

    End Class ' EqualsOtherTest

End Namespace ' MathTests

Namespace SerializationTests

    Public Class TestSerialization

        <Fact>
        Sub Serialize_Simple_Passes()

            Dim Imp As New Impedance(1, 2)
            Dim ResultStr As System.String = System.String.Empty
            Dim ExpectedSerialized As String = "{""Resistance"":1,""Reactance"":2}"

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
        <InlineData("{""Resistance"":333333.122,""Reactance"":555555.672}", 333_333.122, 555_555.672)>
        <InlineData("{""Resistance"":444444.127,""Reactance"":-555555.677}", 444_444.127, -555_555.677)>
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
