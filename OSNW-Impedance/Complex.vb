Imports System.Runtime.CompilerServices

Module ComplexExtensions

    ''' <summary>
    ''' Returns a standard form string that represents the current object.
    ''' </summary>
    ''' <param name="complex">The complex number to convert.</param>
    ''' <returns>A string representation of the complex number in the form "A+iB" or "A-iB".</returns>
    <Extension()>
    Public Function ToStandardString(complex As System.Numerics.Complex)
        Return If(complex.Imaginary < 0.0,
            $"{complex.Real}-i{Math.Abs(complex.Imaginary)}",
            $"{complex.Real}+i{complex.Imaginary}")
    End Function ' ToStandardString

End Module ' ComplexExtensions
