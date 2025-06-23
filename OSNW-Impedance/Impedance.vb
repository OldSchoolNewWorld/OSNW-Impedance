Imports System.Runtime.CompilerServices
Imports System.Numerics

''' <summary>
''' Represents an electrical impedance with resistance (R) and reactance (X).
''' </summary>
Public Structure Impedance
    '    Implements IEquatable(Of Impedance)

    ''' <summary>
    ''' Gets the Impedance as a complex number.
    ''' </summary>
    Private ReadOnly Complex As System.Numerics.Complex

    ''' <summary>
    ''' Gets the resistance component of the Impedance.
    ''' </summary>
    Public ReadOnly Property Resistance As System.Double
        Get
            Return Me.Complex.Real
        End Get
    End Property

    ''' <summary>
    ''' Gets the reactance component of the Impedance.
    ''' </summary>
    Public ReadOnly Property Reactance As System.Double
        Get
            Return Me.Complex.Imaginary
        End Get
    End Property

    ''' <summary>
    ''' Creates a new Impedance with the specified resistance (R) and reactance
    ''' (X).
    ''' </summary>
    ''' <param name="r">Specifies the value of resistance component of the
    ''' Impedance.</param>
    ''' <param name="x">Specifies the value of reactance component of the
    ''' Impedance.</param>
    Public Sub New(r As System.Double, x As System.Double)
        Me.Complex = New System.Numerics.Complex(r, x)
    End Sub

    '    Public Function ToComplex() As System.Numerics.Complex
    '        Return New System.Numerics.Complex(Me.Resistance, Me.Reactance)
    '    End Function

    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent string
    ''' representation in Cartesian form.
    ''' </summary>
    ''' <returns>The string representation of the current instance in Cartesian
    ''' form.</returns>
    Public Overrides Function ToString() As System.String
        Return Me.Complex.ToString()
    End Function

    '    Public Overloads Function Equals(other As Impedance) As System.Boolean
    '        Implements IEquatable(Of Impedance).Equals
    '
    '        '        Return Resistance = other.Resistance AndAlso Reactance = other.Reactance
    '        Return Me.Complex.Equals(other.ToComplex())
    '    End Function

    ''' <summary>
    ''' Converts the value of the current Impedance to its equivalent string
    ''' representation in standard form.
    ''' </summary>
    ''' <returns>The string representation of the current instance in standard
    ''' form.</returns>
    Public Function ToStandardString()
        Return Me.Complex.ToStandardString().Replace("i", "j")
    End Function

End Structure ' Impedance
