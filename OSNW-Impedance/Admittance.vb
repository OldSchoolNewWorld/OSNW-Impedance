''' <summary>
''' Represents an electrical admittance with conductance (G) and susceptance (B).
''' An electrical Admittance (Y) is a number of the standard form Y=G+jB, where
'''   Y is the admittance (siemens);
'''   G is the conductance (siemens);
'''   B is the susceptance (siemens); and
'''   j^2 = −1, the imaginary unit.
''' </summary>
Public Structure Admittance
    '         Implements IEquatable(Of Ytt.Util.Electrical.Admittance), IFormattable

    Const MSGVMBGTZ As System.String = Impedance.MSGVMBGTZ

#Region "Fields and Properties"

#End Region ' "Fields and Properties"

    ' The conductance and susceptance are stored as a complex number.
    Private m_Complex As System.Numerics.Complex

    ''' <summary>
    ''' Gets or sets the conductance (G) in siemens.
    ''' </summary>
    Public Property Conductance As Double

    ''' <summary>
    ''' Gets or sets the susceptance (B) in siemens.
    ''' </summary>
    Public Property Susceptance As Double

    ''' <summary>
    ''' Gets the real part of the admittance.
    ''' </summary>
    Public ReadOnly Property RealPart As Double
        Get
            Return Conductance
        End Get
    End Property

    ''' <summary>
    ''' Gets the imaginary part of the admittance.
    ''' </summary>
    Public ReadOnly Property ImaginaryPart As Double
        Get
            Return Susceptance
        End Get
    End Property

#Region "Constructors"

    ''' <summary>
    ''' Initializes a new instance of the <c>Admittance</c> structure using the
    ''' specified  <paramref name="conductance"/> (G) and
    ''' <paramref name="susceptance"/> (B) values.
    ''' </summary>
    ''' <param name="conductance">Specifies the value of the conductance
    ''' component of the current instance in siemens.</param>
    ''' <param name="susceptance">Specifies the value of the susceptance
    ''' component of the current instance in siemens.</param>
    ''' <exception cref="System.ArgumentOutOfRangeException">
    ''' When <paramref name="conductance"/> is negative.
    ''' </exception>
    ''' <remarks>
    ''' An exception is thrown if <paramref name="conductance"/> is negative.
    ''' <para>
    ''' No real electrical component can have zero conductance, or its reciprocal
    ''' - infinite resistance. Nor can the opposite case exist. Some theoretical
    ''' calculations, such as choosing a component to resonate a circuit, use
    ''' pure reactances. When necessary, use a very small
    ''' <paramref name="conductance"/>, such as
    ''' <see cref="System.Double.Epsilon"/>, to avoid <c>NaN</c>> results in
    ''' other calculations.
    ''' </para>
    ''' </remarks>
    Public Sub New(ByVal conductance As System.Double,
                   ByVal susceptance As System.Double)

        ' Input checking.
        If conductance < 0.0 Then
            Dim CaughtBy As System.Reflection.MethodBase =
                    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(conductance),
                                                         MSGVMBGTZ)
        End If

        Me.m_Complex = New System.Numerics.Complex(conductance, susceptance)

    End Sub ' New

#End Region ' "Constructors"

End Structure ' Electrical



