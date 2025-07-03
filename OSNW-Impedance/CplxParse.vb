Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Partial Public Module ComplexExtensions

#Region "Parsing Utils"

    '''' <summary>
    '''' Counts the appearances of a specified character in a string.
    '''' </summary>
    Private Function GetCharCount(ByVal s As System.String,
        ByVal searchChar As System.Char) As System.Int32

        If String.IsNullOrWhiteSpace(s) Then
            Return 0
        End If
        Dim Count As System.Int32 = 0
        For Each OneChar As System.Char In s
            If OneChar.Equals(searchChar) Then
                Count += 1
            End If
        Next
        Return Count
    End Function ' GetCharCount

    '''' <summary>
    '''' Counts the appearances of the plus and minus signs in a string. The
    '''' plus and minus signs are counted as one sign each, even if they appear
    '''' multiple times in a row.
    '''' </summary>
    Private Function GetSignCount(ByVal s As System.String) As System.Int32
        If String.IsNullOrWhiteSpace(s) Then
            Return 0
        End If
        Return OSNW.Numerics.ComplexExtensions.GetCharCount(s, CHARPLUS) +
            OSNW.Numerics.ComplexExtensions.GetCharCount(s, CHARMINUS)
    End Function ' GetSignCountGetSignCount

    '''' <summary>
    '''' Counts the appearances of the letter "E" or "e" in a string. The letter
    '''' is counted as one character, even if it appears multiple times in a
    '''' row.
    '''' </summary>
    Private Function GetECount(ByVal s As System.String) As System.Int32
        If String.IsNullOrWhiteSpace(s) Then
            Return 0
        End If
        Dim ECount As System.Int32
        For Each OneCh As System.Char In s
            If Char.ToUpper(OneCh).Equals(CHARUPPERE) Then
                ECount += 1
            End If
        Next
        Return ECount
    End Function ' GetECount

#End Region ' "Parsing Utils"

#Region "Parsing Implementations"

    ' As of when recorded, these Complex signatures match in .NET 8.0 and
    '   .NET 9.0.
    '
    '   public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);
    '   public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex result)
    '   public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result) => TryParse(s, DefaultNumberStyle, provider, out result);

    ' For these emulations,
    '   Examine the string for a valid standard form.
    '   Extract the component strings.
    '   Create a string matching Complex.ToString().
    '   Use Complex.TryParse() to parse the string.

    ' public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, out Complex result)
    ' public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, out Complex result)
    ''' <summary>
    ''' Attempts to convert the standard form representation of a complex number
    ''' to its <see cref="System.Numerics.Complex"/> equivalent using the
    ''' specified layout format, numeric format, and culture-specific format
    ''' information.
    ''' </summary>
    ''' <param name="s">Specifies the standard form string to be parsed.</param>
    ''' <param name="standardizationStyle">Specifies the layout formats
    ''' permitted in numeric string arguments that are passed to the TryParse
    ''' method of the <c>System.Numerics.Complex</c> numeric type.</param>
    ''' <param name="style">Determines the styles permitted in numeric string
    ''' arguments that are passed to the TryParse method of the
    ''' <c>System.Numerics.Complex</c> numeric type.</param>
    ''' <param name="provider">Provides a mechanism for retrieving an object to
    ''' control formatting.</param>
    ''' <param name="result">Returns the <c>System.Numerics.Complex</c>
    ''' represented by <paramref name="s"/>.</param>
    ''' <returns>Returns <c>True</c> if the conversion succeeds; otherwise,
    ''' <c>False</c>.</returns>
    Public Function TryParseStandard(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal s As System.String,
        ByVal standardizationStyle As StandardizationStyles,
        ByVal style As System.Globalization.NumberStyles,
        ByVal provider As System.IFormatProvider,
        ByRef result As System.Numerics.Complex) _
        As System.Boolean

        Const MINLEN As System.Int32 = 4 ' #+#i or #+i#
        ' Some cultures use a comma as a decimal, or as a thousands, separator.
        ' The open form includes spaces.
        Const VALIDCHARS As System.String = "1234567890.+-iEe ,"

        '        ValidateParseStyleFloatingPoint(style)
        ' That was called at the top of Complex.Tryparse.
        ' IS THAT COMMENT BASED ON AN OLD APPROACH???????

        ' Start with the most basic failures.
        If String.IsNullOrWhiteSpace(s) Then
            result = New System.Numerics.Complex
            Return False
        End If
        Dim StrLen As System.Int32 = s.Length
        If StrLen < MINLEN Then
            result = New System.Numerics.Complex
            Return False ' Early exit.
        Else
            For I As System.Int32 = 0 To StrLen - 1
                If Not VALIDCHARS.Contains(s(I)) Then
                    ' Allow only the specified characters.
                    result = New System.Numerics.Complex
                    Return False
                End If
            Next
        End If

        ' Expect the string to be in the, "A+Bi" or "A+iB", closed or open,
        ' standard form.
        ' s must have exacty one 'i' character.
        ' s must have one to four signs.
        ' s may have maximum of two 'E'/'e' characters.
        Dim ICount As System.Int32 =
            OSNW.Numerics.ComplexExtensions.GetCharCount(s, CHARI)
        Dim SignCount As System.Int32 =
            OSNW.Numerics.ComplexExtensions.GetSignCount(s)
        Dim ECount As System.Int32 =
            OSNW.Numerics.ComplexExtensions.GetECount(s)
        If ICount <> 1 OrElse
            SignCount < 1 OrElse SignCount > 4 OrElse
            ECount > 2 Then

            result = New System.Numerics.Complex
            Return False ' Early exit.
        End If

        Dim WorkStr As New System.String(s)

        ' Expect the string to begin with a valid double substring. Trim back
        ' anything that does not leave a valid double.
        ' In open form, RealStr will end with the space preceeding the sign of
        ' the imaginary component.
        Dim RealStr As System.String = WorkStr
        Dim TestD As System.Double
        Dim KeepGoing As System.Boolean = True
        While KeepGoing
            If System.Double.TryParse(RealStr, TestD) Then
                ' There is now a valid double.
                KeepGoing = False
            Else
                ' Remove the last character.
                RealStr = RealStr.Substring(0, StrLen - 1)
                StrLen -= 1
                If StrLen = 0 Then
                    ' We have run out of characters.
                    result = New System.Numerics.Complex
                    Return False
                End If
            End If
        End While

        ' Shift focus to what remains.
        WorkStr = s.Substring(StrLen)
        StrLen = WorkStr.Length

        ' Expect the sign of the imaginary component to be next and not be the
        ' end of the string. Identify the sign.
        If StrLen <= 1 OrElse
            Not (WorkStr(0).Equals(CHARPLUS) OrElse
                 WorkStr(0).Equals(CHARMINUS)) Then

            result = New System.Numerics.Complex
            Return False
        End If
        Dim IsNeg As System.Boolean = WorkStr(0).Equals(CHARMINUS)

        WorkStr = WorkStr.Substring(1)
        StrLen -= 1

        ' Is a spacing match required?
        If (standardizationStyle And
            StandardizationStyles.EnforceSpacing) > 0 Then

            If (standardizationStyle And StandardizationStyles.Open) > 0 Then
                ' Must use spacing.
                If Not (RealStr.EndsWith(CHARSPACE) AndAlso
                    WorkStr.StartsWith(CHARSPACE)) Then

                    result = New System.Numerics.Complex
                    Return False
                End If
            Else
                ' Must not use spacing.
                If RealStr.EndsWith(CHARSPACE) OrElse
                    WorkStr.StartsWith(CHARSPACE) Then

                    result = New System.Numerics.Complex
                    Return False
                End If
            End If
        End If

        ' What remains, exclusive of spaces, should be the "i" and a valid
        ' double string.

        ' The "i" should be at either the beginning or end of the double.
        Dim TrimmedStr As System.String = WorkStr.Trim()
        If Not (TrimmedStr.StartsWith(CHARI) OrElse
            TrimmedStr.EndsWith(CHARI)) Then

            result = New System.Numerics.Complex
            Return False
        End If

        ' Is a sequence match required?
        If (standardizationStyle And
            StandardizationStyles.EnforceSequence) > 0 Then

            If (standardizationStyle And StandardizationStyles.AiB) > 0 Then
                ' Must match FormerABi.
                If Not TrimmedStr.EndsWith(CHARI) Then
                    result = New System.Numerics.Complex
                    Return False
                End If
            Else
                ' Must match FormerAiB.
                If Not TrimmedStr.StartsWith(CHARI) Then
                    result = New System.Numerics.Complex
                    Return False
                End If
            End If
        End If

        ' Extract the "i".
        WorkStr = WorkStr.Remove(WorkStr.IndexOf(CHARI), 1)

        If IsNeg Then
            ' Reinsert the negative sign after any leading spaces.
            Dim StillSpace As System.Boolean = True
            Dim Rebuilt As New System.Text.StringBuilder()
            Dim Look As Integer = 0
            While StillSpace AndAlso Look < StrLen
                If WorkStr(Look).Equals(CHARSPACE) Then
                    ' Keep the space.
                    Rebuilt.Append(WorkStr(Look))
                    Look += 1
                Else
                    StillSpace = False
                    Rebuilt.Append($"-{WorkStr.Substring(Look)}")
                End If
            End While
            WorkStr = Rebuilt.ToString()
        End If

        ' This is failing, but should be used this way when style is sent in.
        Return System.Numerics.Complex.TryParse(
            $"<{RealStr}; {WorkStr}>", style, provider, result)

    End Function ' TryParseStandard

    ' public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out Complex result)
    ' public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, out Complex result)
    ''' <summary>
    ''' Attempts to convert the standard form representation of a complex number
    ''' to its <see cref="System.Numerics.Complex"/> equivalent using the
    ''' specified layout format and culture-specific format information.
    ''' </summary>
    ''' <param name="s">Specifies the standard form string to be parsed.</param>
    ''' <param name="standardizationStyle">Specifies the layout formats
    ''' permitted in numeric string arguments that are passed to the TryParse
    ''' method of the <c>System.Numerics.Complex</c> numeric type.</param>
    ''' <param name="provider">Provides a mechanism for retrieving an object to
    ''' control formatting.</param>
    ''' <param name="result">Returns the <c>System.Numerics.Complex</c>
    ''' represented by <paramref name="s"/>.</param>
    ''' <returns>Returns <c>True</c> if the conversion succeeds; otherwise,
    ''' <c>False</c>.</returns>
    Public Function TryParseStandard(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal s As System.String,
        ByVal standardizationStyle As StandardizationStyles,
        ByVal provider As System.IFormatProvider,
        ByRef result As System.Numerics.Complex) _
        As System.Boolean

        Return TryParseStandard(s, standardizationStyle,
                                DEFAULTCOMPLEXNUMBERSTYLE, provider, result)
    End Function ' TryParseStandard

#End Region ' "Parsing Implementations"

End Module ' ComplexExtensions
