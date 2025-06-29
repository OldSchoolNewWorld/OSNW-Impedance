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
    Implements IEquatable(Of Admittance)

    Const MSGVMBGTZ As System.String = Impedance.MSGVMBGTZ

#Region "Fields and Properties"

    ''' <summary>
    '''  This is for some internal conveniences. It provides easy access to
    '''  <c>Complex</c> functionality.
    ''' </summary>
    ''' <returns>A new instance of the <see cref="System.Numerics.Complex"/>
    ''' structure.</returns>
    Friend Function ToComplex() As System.Numerics.Complex
        Return New System.Numerics.Complex(Me.Conductance, Me.Susceptance)
    End Function

    ''' <summary>
    ''' Returns a value that represents this instance as its equivalent
    ''' <see cref="Impedance"/>.
    ''' </summary>
    ''' <returns>The equivalent impedance value of this instance.</returns>
    Public Function ToImpedance() As Impedance
        Dim ComplexRecip As System.Numerics.Complex =
            System.Numerics.Complex.Reciprocal(Me.ToComplex())
        Return New Impedance(ComplexRecip.Real, ComplexRecip.Imaginary)
    End Function

#End Region ' "Fields and Properties"

    ' Use the "has a ..." approach to expose the desired features of a
    ' System.Numerics.Complex.
    ' Do not rename (binary serialization). ??????????????????????????????
    Private ReadOnly m_Complex As System.Numerics.Complex

    ''' <summary>
    ''' Gets the conductance (G) component, in siemens, of the current instance.
    ''' </summary>
    Public ReadOnly Property Conductance As Double
        Get
            Return m_Complex.Real
        End Get
    End Property

    ''' <summary>
    ''' Gets the susceptance (B) component, in siemens, of the current instance.
    ''' </summary>
    Public ReadOnly Property Susceptance As Double
        Get
            Return m_Complex.Imaginary
        End Get
    End Property

#Region "System.ValueType Implementations"

    ' public override bool Equals([NotNullWhen(true)] object? obj)
    ' {
    '     return obj is Complex other && Equals(other);
    ' }
    'Public Overrides Function Equals(obj As Object) As Boolean
    '    Return (TypeOf obj Is Impedance) AndAlso
    '        DirectCast(Me, IEquatable(Of Impedance)).Equals(
    '        DirectCast(obj, Impedance))
    'End Function
    ''' <summary>
    ''' Determines whether the specified object is equal to the current object.
    ''' </summary>
    ''' <param name="obj">The object to compare with the current object.</param>
    ''' <returns><c>True</c> if the specified object is equal to the current
    ''' object; otherwise, <c>False</c>.</returns>
    Public Overrides Function Equals(
        <System.Diagnostics.CodeAnalysis.NotNullWhen(True)>
            ByVal obj As System.Object) As System.Boolean

        Return (TypeOf obj Is Admittance) AndAlso
            DirectCast(Me, IEquatable(Of Admittance)).Equals(
            DirectCast(obj, Admittance))
    End Function

    ' public bool Equals(Complex value)
    ' {
    '     return m_real.Equals(value.m_real) && m_imaginary.Equals(value.m_imaginary);
    ' }
    ''' <summary>
    ''' Returns a value that indicates whether this instance and the specified
    ''' <see cref="Impedance"/> have the same component values.
    ''' </summary>
    ''' <param name="value">The <c>Impedance</c> to compare.</param>
    ''' <returns><c>True</c> if this instance and a specified <c>Impedance</c>
    ''' have the same component values.</returns>
    ''' <remarks>This may have to be changed to determine equality within some
    ''' reasonable bounds. See 
    ''' <see href="https://github.com/dotnet/docs/blob/main/docs/fundamentals/runtime-libraries/system-numerics-complex.md#precision-and-complex-numbers"/>
    ''' </remarks>
    Public Overloads Function Equals(ByVal value As Admittance) As System.Boolean _
        Implements System.IEquatable(Of Admittance).Equals

        Return Me.ToComplex().Equals(value.ToComplex())
    End Function

    ''' <summary>
    ''' Serves as the default hash function.
    ''' </summary>
    ''' <returns>A hash code for the current object.</returns>
    Public Overrides Function GetHashCode() As System.Int32
        Return Me.ToComplex.GetHashCode
    End Function

#End Region ' "System.ValueType Implementations"

#Region "Operator Implementations"

    ''' <summary>
    ''' Returns a value that indicates whether two <c>Admittance</c>s are equal.
    ''' </summary>
    ''' <param name="left">The first <c>Admittance</c> to compare.</param>
    ''' <param name="right">The second <c>Admittance</c> to compare.</param>
    ''' <returns><c>True</c>> if the left and right parameters have the same
    ''' value; otherwise, <c>False</c>.</returns>
    Public Shared Operator =(ByVal left As Admittance,
                             ByVal right As Admittance) As System.Boolean

        Return left.Equals(right)
    End Operator

    ''' <summary>
    ''' Returns a value that indicates whether two <c>Admittance</c>s are not
    ''' equal.
    ''' </summary>
    ''' <param name="left">The first <c>Admittance</c> to compare.</param>
    ''' <param name="right">The second <c>Admittance</c> to compare.</param>
    ''' <returns><c>True</c>> if left and right are not equal; otherwise,
    ''' <c>False</c>.</returns>
    Public Shared Operator <>(ByVal left As Admittance,
                              ByVal right As Admittance) As System.Boolean

        Return Not left = right
    End Operator

    ''' <summary>
    ''' Returns the result of the addition of two <c>Admittance</c>s.
    ''' </summary>
    ''' <param name="admittance1">The first <c>Admittance</c> to add.</param>
    ''' <param name="admittance2">The first <c>Admittance</c> to add.</param>
    ''' <returns>The result of the addition.</returns>
    Public Shared Operator +(admittance1 As Admittance,
                             admittance2 As Admittance) As Admittance
        ' No input checking. impedance1 and impedance2 are presumed to have been
        ' checked when created.
        Dim TotalC As System.Numerics.Complex =
            admittance1.ToComplex + admittance2.ToComplex
        Return New Admittance(TotalC.Real, TotalC.Imaginary)
    End Operator

#End Region ' "Operator Implementations"

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
