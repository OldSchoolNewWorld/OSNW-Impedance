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

    End Class

End Namespace ' DevTests




Namespace TestToString

    Public Class ToStringDefaultTest

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

    End Class ' ToStringDefaultTest

End Namespace ' TestToString

Namespace TestToStandardString

    Public Class ToStandardStringDefaultTest

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

    End Class ' ToStandardStringDefaultTest

    Public Class ToStandardStringStandardizationTest

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

    End Class ' ToStandardStringStandardizationTest

    Public Class ToStandardStringFormatTest

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

    End Class ' ToStandardStringFormatTest

    Public Class ToStandardStringCultureTest

        <Theory>
        <InlineData(111_111.122, -555_555.677, 0, "111111.122-555555.677j")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 1, "111111.122-555555.677j")> ' One round down, one up.
        <InlineData(1.122, 5.677, 2, "1.122+5.677j")>
        <InlineData(111_111.122, -555_555.677, 3, "111111,122-555555,677j")> ' One round down, one up.
        <InlineData(111_111.125, 555_555.675, 4, "111111,125+555555,675j")>
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
        <InlineData("0+i5.675", 0, 5.675)>
        <InlineData("0-i5.675", 0, -5.675)>
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
                Assert.True(False)
            End If
            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))
        End Sub

    End Class ' TryParseStandardDefaultMixedTest

    Public Class TryParseStandardEnforceStandardizationTest

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
                Assert.True(False)
            End If
            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))
        End Sub

        <Theory>
        <InlineData("1.125 + i5.675", StandardizationStyles.ClosedABi Or TightEnforcement)>
        <InlineData("1.125-i5.675", StandardizationStyles.ClosedAiB Or TightEnforcement)>
        <InlineData("-1.125+i5.675", StandardizationStyles.OpenABi Or TightEnforcement)>
        <InlineData("-1.125 - i5.675", StandardizationStyles.OpenAiB Or TightEnforcement)>
        Sub TryParseStandard_InvalidStandardization_Fails(
            standardStr As String, standardizationStyle As StandardizationStyles)

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
            Dim Impd As New OSNW.Numerics.Impedance

            If Not OSNW.Numerics.Impedance.TryParseStandard(standardStr, Nothing, Providers(index), Impd) Then
                Assert.True(False)
            End If

            Assert.True(Impd.Resistance.Equals(real) AndAlso Impd.Reactance.Equals(imaginary))

        End Sub

    End Class ' TryParseStandardCultureTest

End Namespace ' TestTryParseStandard

Namespace TestMath

    Public Class EqualsObjectTest

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

    End Class ' EqualsObjectTest

    Public Class EqualsOtherTest

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

End Namespace ' TestMath

Namespace TestSerialization

    Public Class SerializationTest

        <Fact>
        Sub Serialize_Simple_Passes()

            Dim I1 As New Impedance(1, 2)
            Dim Serialized1 As System.String = System.String.Empty
            Dim ExpectedSerialized As String = "{""Resistance"":1,""Reactance"":2}"

            If I1.SerializeJSONString_Curr(Serialized1) Then
                Assert.True(ExpectedSerialized.Equals(Serialized1))
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
        Sub Serialize_Curr_Passes(r As Double, x As Double, expectedStr As String)

            Dim I1 As New Impedance(r, x)
            Dim Serialized1 As System.String = System.String.Empty

            If I1.SerializeJSONString_Curr(Serialized1) Then
                '                Dim PeekStr As String = Serialized1
                Assert.True(expectedStr.Equals(Serialized1))
            Else
                Assert.True(False, "Serialization failed.")
            End If

        End Sub



        '
        '
        '
        '
        '





    End Class ' SerializationTest



    '
    '
    '
    '
    '

End Namespace ' TestSerialization
