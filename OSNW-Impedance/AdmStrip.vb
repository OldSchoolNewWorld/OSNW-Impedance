' Partial Public Structure Electrical
' 
'     ''' <summary>
'     ''' A structure containing the values to define an admittance.
'     ''' </summary>
'     ''' <declaration>
'     ''' Public Structure Admittance
'     ''' </declaration>
'     ''' <remarks>xxxxxxxxxx</remarks>
'     <SerializableAttribute()>
'     Public Structure Admittance
'         Implements IEquatable(Of Ytt.Util.Electrical.Admittance), IFormattable
' 

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
'         ''' <remarks>xxxxxxxxxx</remarks>
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
'         ''' <remarks>xxxxxxxxxx</remarks>
'         Public Shared Operator *(y As Ytt.Util.Electrical.Admittance, scalar As System.Double) As Ytt.Util.Electrical.Admittance
'             ' No input checking. y is presumed to have been checked when created.
'             Dim ResultC = (y.ToComplex * scalar)
'             Return New Ytt.Util.Electrical.Admittance(ResultC.Real, ResultC.Imaginary)
'         End Operator
' 
'     End Structure ' Admittance
' 
' End Structure ' Electrical
