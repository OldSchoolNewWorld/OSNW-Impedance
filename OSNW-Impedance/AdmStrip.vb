' Partial Public Structure Electrical
' 
'     ''' <summary>
'     ''' A structure containing the values to define an admittance.
'     ''' </summary>
'     ''' <declaration>
'     ''' Public Structure Admittance
'     ''' </declaration>
'     ''' <remarks></remarks>
'     <SerializableAttribute()>
'     Public Structure Admittance
'         Implements IEquatable(Of Ytt.Util.Electrical.Admittance), IFormattable
' 
'         ''' <summary>
'         ''' Initializes a new instance of the Admittance structure using the specified conductance and susceptance values.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Sub New(ByVal conductance As System.Double, ByVal susceptance As System.Double)
'         ''' </declaration>
'         ''' <param name="conductance">
'         '''   <p>Type: System.Double</p>
'         '''   <p>Value: The conductance component in siemens.</p>
'         ''' </param>
'         ''' <param name="susceptance">
'         '''   <p>Type: System.Double</p>
'         '''   <p>Value: The susceptance component in siemens.</p>
'         ''' </param>
'         ''' <remarks></remarks>
'         Public Sub New(ByVal conductance As System.Double, ByVal susceptance As System.Double)
' 
'             ' Input checking.
'             If Ytt.Util.Electrical.InvalidConductance(conductance) Then
'                 Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                 Throw Ytt.Util.RunTime.NewValueOutOfRangeException(ProcName, conductance, "conductance")
'             End If
' 
'             Me.m_Complex = New System.Numerics.Complex(conductance, susceptance)
' 
'         End Sub
' 
'         ''' <summary>
'         ''' The conductance component in siemens.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Property Conductance As System.Double
'         ''' </declaration>
'         ''' <value>The conductance component in siemens.</value>
'         ''' <remarks></remarks>
'         Public Property Conductance As System.Double
'             Get
'                 Return Me.m_Complex.Real
'             End Get
'             Set(ByVal value As System.Double)
' 
'                 ' Input checking.
'                 If Ytt.Util.Electrical.InvalidConductance(value) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Throw Ytt.Util.RunTime.NewValueOutOfRangeException(ProcName, value, "value")
'                 End If
' 
'                 Me.m_Complex = New System.Numerics.Complex(value, Me.Susceptance)
' 
'             End Set
'         End Property
' 
'         ''' <summary>
'         ''' Returns a value that indicates whether the current instance and a specified admittance have the same component values.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Shadows Function Equals(ByVal value As Ytt.Util.Electrical.Admittance) As System.Boolean
'         ''' </declaration>
'         ''' <param name="value">
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: The admittance to compare.</p>
'         ''' </param>
'         ''' <returns>
'         '''   <p>Type: System.Boolean</p>
'         '''   <p>Value: <c>True</c> if the current instance and a specified admittance have the same component values.</p>
'         ''' </returns>
'         ''' <remarks>This may have to be changed to determine equality within some reasonable bounds.</remarks>
'         Public Shadows Function Equals(ByVal value As Ytt.Util.Electrical.Admittance) As System.Boolean _
'             Implements System.IEquatable(Of Ytt.Util.Electrical.Admittance).Equals
'             Return Me.ToComplex.Equals(New System.Numerics.Complex(value.Conductance, value.Susceptance))
'         End Function
' 
'         ''' <summary>
'         ''' The susceptance component in siemens.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Property Susceptance As System.Double
'         ''' </declaration>
'         ''' <value>The susceptance component in siemens.</value>
'         ''' <remarks></remarks>
'         Public Property Susceptance As System.Double
'             Get
'                 Return Me.m_Complex.Imaginary
'             End Get
'             Set(ByVal value As System.Double)
'                 ' No input checking.
'                 Me.m_Complex = New System.Numerics.Complex(Me.Conductance, value)
'             End Set
'         End Property
' 
'         ''' <summary>
'         ''' Returns a value that represents the current instance as the equivalent impedance.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Function ToImpedance() As Ytt.Util.Electrical.Impedance
'         ''' </declaration>
'         ''' <returns>
'         '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'         '''   <p>Value: The equivalent impedance of the current instance.</p>
'         ''' </returns>
'         ''' <remarks></remarks>
'         Public Function ToImpedance() As Ytt.Util.Electrical.Impedance
'             Dim ComplexRecip = System.Numerics.Complex.Reciprocal(Me.ToComplex)
'             Return New Ytt.Util.Electrical.Impedance(ComplexRecip.Real, ComplexRecip.Imaginary)
'         End Function
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Converts the value of the current admittance to its equivalent
'         ''' string representation in complex form. (Shadows ValueType.ToString.)
'         ''' </summary>
'         ''' <declaration>
'         ''' Function ToString() As System.String
'         ''' </declaration>
'         ''' <returns>
'         '''   <p>Type: System.String</p>
'         '''   <p>Value: The value of the current instance in the default format.</p>
'         ''' </returns>
'         ''' <remarks></remarks>
'         Shadows Function ToString() As System.String
'             Return Me.ToString(Nothing, Nothing)
'         End Function
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Converts the value of the current admittance to its equivalent
'         ''' string representation in complex form. (Shadows ValueType.ToString.)
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Shadows Function ToString(ByVal format As System.String,
'         ''' ByVal formatProvider As System.IFormatProvider) As System.String
'         ''' </declaration>
'         ''' <param name="format">
'         '''   <p>Type: System.String</p>
'         '''   <p>Value:</p>
'         '''     <p>The format to use.</p>
'         '''     <p>-or-</p>
'         '''     <p>A null reference (Nothing in Visual Basic) to use the default format defined for the type
'         '''        of the IFormattable implementation.</p>
'         ''' </param>
'         ''' <param name="formatProvider">
'         '''   <p>Type: System.IFormatProvider</p>
'         '''   <p>Value:</p>
'         '''     <p>The provider to use to format the component values.</p>
'         '''     <p>-or-</p>
'         '''     <p>A null reference (Nothing in Visual Basic) to obtain the numeric format information from
'         '''        the current locale setting of the operating system.</p>
'         ''' </param>
'         ''' <returns>
'         '''   <p>Type: System.String</p>
'         '''   <p>Value: The value of the current instance in the specified format.</p>
'         ''' </returns>
'         ''' <remarks></remarks>
'         Public Shadows Function ToString(ByVal format As System.String, ByVal formatProvider As System.IFormatProvider) As System.String _
'             Implements System.IFormattable.ToString
'             Return Ytt.Util.Electrical.ComplexJString(Me.ToComplex, format, formatProvider)
'         End Function
' 
'         ' This is for some internal conveniences.
'         Friend Function ToComplex() As System.Numerics.Complex
'             Return Me.m_Complex
'         End Function
' 
'         ''' <summary>
'         ''' Returns the result of the addition of two complex admittances.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Shared Operator +(admittance1 As Ytt.Util.Electrical.Admittance,
'         ''' admittance2 As Ytt.Util.Electrical.Admittance) As Ytt.Util.Electrical.Admittance
'         ''' </declaration>
'         ''' <param name="admittance1">
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: An admittance.</p>
'         ''' </param>
'         ''' <param name="admittance2">
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: An admittance.</p>
'         ''' </param>
'         ''' <returns>
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: The result of the addition.</p>
'         ''' </returns>
'         ''' <remarks></remarks>
'         Public Shared Operator +(admittance1 As Ytt.Util.Electrical.Admittance,
'                                  admittance2 As Ytt.Util.Electrical.Admittance) As Ytt.Util.Electrical.Admittance
'             ' No input checking. admittance1 and admittance2 are presumed to have been checked when created.
'             Dim TotalComplex = (admittance1.ToComplex + admittance2.ToComplex)
'             Return New Ytt.Util.Electrical.Admittance(TotalComplex.Real, TotalComplex.Imaginary)
'         End Operator
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Returns the result of the division of a complex admittance by a scalar value.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Shared Operator /(y As Ytt.Util.Electrical.Admittance, scalar As System.Double) As Ytt.Util.Electrical.Admittance
'         ''' </declaration>
'         ''' <param name="y">
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: An admittance.</p>
'         ''' </param>
'         ''' <param name="scalar">
'         '''   <p>Type: System.Double</p>
'         '''   <p>Value: The scalar value of the denominator.</p>
'         ''' </param>
'         ''' <returns>
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: The result of the division.</p>
'         ''' </returns>
'         ''' <remarks></remarks>
'         Public Shared Operator /(y As Ytt.Util.Electrical.Admittance, scalar As System.Double) As Ytt.Util.Electrical.Admittance
'             ' No input checking. y is presumed to have been checked when created.
'             Dim Quotient = (y.ToComplex / scalar)
'             Return New Ytt.Util.Electrical.Admittance(Quotient.Real, Quotient.Imaginary)
'         End Operator
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Returns the product of a complex admittance and a scalar value.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Shared Operator *(y As Ytt.Util.Electrical.Admittance, scalar As System.Double) As Ytt.Util.Electrical.Admittance
'         ''' </declaration>
'         ''' <param name="y">
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: An admittance.</p>
'         ''' </param>
'         ''' <param name="scalar">
'         '''   <p>Type: System.Double</p>
'         '''   <p>Value: The scalar value to be multiplied.</p>
'         ''' </param>
'         ''' <returns>
'         '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'         '''   <p>Value: The product of the admittance and the scalar value.</p>
'         ''' </returns>
'         ''' <remarks></remarks>
'         Public Shared Operator *(y As Ytt.Util.Electrical.Admittance, scalar As System.Double) As Ytt.Util.Electrical.Admittance
'             ' No input checking. y is presumed to have been checked when created.
'             Dim ResultC = (y.ToComplex * scalar)
'             Return New Ytt.Util.Electrical.Admittance(ResultC.Real, ResultC.Imaginary)
'         End Operator
' 
'     End Structure ' Admittance
' 
' End Structure ' Electrical
