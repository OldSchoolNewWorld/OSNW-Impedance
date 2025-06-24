Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Imports System
Imports System.Configuration
Imports System.Globalization
Imports OSNW.Numerics.ComplexExtensions
Imports Xunit

' REF: Extension Methods not Recognized
' https://docs.microsoft.com/en-us/dotnet/standard/base-types/how-to-extend-a-type-with-extension-methods
'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

Namespace TestComplex

    Public Class ToStandardStringDefaultTest

        <Theory>
        <InlineData(1.125, 5.675, "1.125+i5.675")>
        <InlineData(1.125, -5.675, "1.125-i5.675")>
        <InlineData(-1.125, 5.675, "-1.125+i5.675")>
        <InlineData(-1.125, -5.675, "-1.125-i5.675")>
        Sub ToStandardString_Default_Succeeds(real As Double, imaginary As Double, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString()
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' ToStandardStringDefaultTest

    Public Class ToStandardStringStandardizationTest

        <Theory>
        <InlineData(1.125, 5.675, Nothing, "1.125+i5.675")>
        <InlineData(1.125, -5.675, StandardizationStyles.ABi, "1.125-5.675i")>
        <InlineData(-1.125, 5.675, StandardizationStyles.Open, "-1.125 + i5.675")>
        <InlineData(-1.125, -5.675, StandardizationStyles.OpenABi, "-1.125 - 5.675i")>
        Sub ToStandardString_Standardization_Succeeds(real As Double, imaginary As Double,
                                                      standardizationStyle As StandardizationStyles, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString(standardizationStyle)
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' ToStandardStringStandardizationTest

    Public Class ToStandardStringFormatTest

        <Theory>
        <InlineData(1.122, 5.677, "F2", "1.12+i5.68")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, "N2", "111,111.12-i555,555.68")> ' One round down, one up.
        <InlineData(-111_111.125, 555_555.675, "G5", "-1.1111E+05+i5.5556E+05")>
        Sub ToStandardString_Format_Succeeds(real As Double, imaginary As Double, format As String, expected As String)
            Dim Z As New System.Numerics.Complex(real, imaginary)
            Dim CplxStr As String = Z.ToStandardString(Nothing, format)
            Assert.Equal(expected, CplxStr)
        End Sub

    End Class ' ToStandardStringFormatTest

    Public Class ToStandardStringCultureTest

        <Theory>
        <InlineData(111_111.122, -555_555.677, 0, "111111.122-i555555.677")> ' One round down, one up.
        <InlineData(111_111.122, -555_555.677, 1, "111111.122-i555555.677")> ' One round down, one up.
        <InlineData(1.122, 5.677, 2, "1.122+i5.677")>
        <InlineData(111_111.122, -555_555.677, 3, "111111,122-i555555,677")> ' One round down, one up.
        <InlineData(-111_111.125, 555_555.675, 4, "-111111,125+i555555,675")>
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

End Namespace ' TestComplex
