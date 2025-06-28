'Partial Public Module Electrical
'
'    Public Structure Impedance
'        Implements IEquatable(Of Ytt.Util.Electrical.Impedance), IFormattable
'


'#Region "Instance Methods"
'
'        ''' <summary>
'        ''' Returns a value that represents this instance as its equivalent <see cref="Admittance"/>.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Function ToAdmittance() As Ytt.Util.Electrical.Admittance
'        ''' </declaration>
'        ''' <returns>
'        '''   <p>Type: <see cref="Admittance"/></p>
'        '''   <p>Value: The equivalent admittance value of this instance.</p>
'        ''' </returns>
'        Public Function ToAdmittance() As Ytt.Util.Electrical.Admittance
'            Dim ComplexRecip As System.Numerics.Complex = System.Numerics.Complex.Reciprocal(Me.ToComplex())
'            Return New Ytt.Util.Electrical.Admittance(ComplexRecip.Real, ComplexRecip.Imaginary)
'        End Function
'



'
'        ''' <summary>
'        ''' Calculates the voltage standing wave ratio for this instance based on the specified
'        ''' characteristic impedance.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Function VSWR(ByVal z0 As System.Double) As System.Double
'        ''' </declaration>
'        ''' <param name="z0">
'        '''   <p>Type: System.Double</p>
'        '''   <p>Value: The characteristic impedance.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: System.Double</p>
'        '''   <p>Value: The voltage standing wave ratio.</p>
'        ''' </returns>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        ''' <exception cref="System.ArgumentOutOfRangeException">
'        ''' Thrown when <paramref name="z0"/> is not a positive, non-zero value.
'        ''' </exception>
'        Public Function VSWR(ByVal z0 As System.Double) As System.Double
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(z0) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            ElseIf Ytt.Util.Electrical.IsInvalidZ0(z0) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.Electrical.NewInvalidZ0Exception(ProcName, "z0", z0)
'            End If
'
'            ' Ref:
'            ' https://www.antenna-theory.com/definitions/vswr.php
'            ' https://www.antenna-theory.com/definitions/vswr-calculator.php
'            ' https://www.microwaves101.com/encyclopedias/voltage-standing-wave-ratio-vswr
'
'            Dim Gamma As System.Numerics.Complex = Me.VoltageReflectionCoefficient(z0)
'            '            Internal calls to Ytt.Util.Electrical.AbsComplex were replaced by direct calls to System.Numerics.Complex.Abs
'            '            Dim AbsGamma As System.Double = Ytt.Util.Electrical.AbsComplex(Gamma)
'            Dim AbsGamma As System.Double = System.Numerics.Complex.Abs(Gamma)
'            Return (1.0 + AbsGamma) / (1.0 - AbsGamma)
'
'        End Function
'
'        ''' <summary>
'        ''' Calculates the power reflection coeffient for this instance based on the specified
'        ''' characteristic impedance.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Function PowerReflectionCoefficient(ByVal z0 As System.Double) As System.Numerics.Complex
'        ''' </declaration>
'        ''' <param name="z0">
'        '''   <p>Type: System.Double</p>
'        '''   <p>Value: The characteristic impedance.</p>
'        ''' </param>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        ''' <exception cref="System.ArgumentOutOfRangeException">
'        ''' Thrown when <paramref name="z0"/> is not a positive, non-zero value.
'        ''' </exception>
'        ''' <returns>
'        '''   <p>Type: System.Numerics.Complex</p>
'        '''   <p>Value: The voltage reflection coeffient.</p>
'        ''' </returns>
'        Public Function PowerReflectionCoefficient(ByVal z0 As System.Double) As System.Numerics.Complex
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(z0) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            ElseIf Ytt.Util.Electrical.IsInvalidZ0(z0) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.Electrical.NewInvalidZ0Exception(ProcName, "z0", z0)
'            End If
'
'            Dim VRC As System.Numerics.Complex = Me.VoltageReflectionCoefficient(z0)
'            Return System.Numerics.Complex.Multiply(VRC, VRC)
'
'        End Function
'
'        ''' <summary>
'        ''' Calculates the voltage reflection coeffient for this instance based on the specified
'        ''' characteristic impedance.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Function VoltageReflectionCoefficient(ByVal z0 As System.Double) As System.Numerics.Complex
'        ''' </declaration>
'        ''' <param name="z0">
'        '''   <p>Type: System.Double</p>
'        '''   <p>Value: The characteristic impedance.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: System.Numerics.Complex</p>
'        '''   <p>Value: The voltage reflection coeffient.</p>
'        ''' </returns>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        ''' <exception cref="System.ArgumentOutOfRangeException">
'        ''' Thrown when <paramref name="z0"/> is not a positive, non-zero value.
'        ''' </exception>
'        Public Function VoltageReflectionCoefficient(ByVal z0 As System.Double) As System.Numerics.Complex
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(z0) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            ElseIf Ytt.Util.Electrical.IsInvalidZ0(z0) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.Electrical.NewInvalidZ0Exception(ProcName, "z0", z0)
'            End If
'
'            ' Ref: https://en.wikipedia.org/wiki/Standing_wave_ratio
'
'            Dim MeAsComplex As System.Numerics.Complex = Me.ToComplex()
'            Return (MeAsComplex - z0) / (MeAsComplex + z0)
'
'        End Function
'
'        ' This is for some internal conveniences. It reduces the number of direct
'        ' accesses to m_Complex elsewhere.
'        Friend Function ToComplex() As System.Numerics.Complex
'            Return Me.m_Complex
'        End Function
'
'#End Region ' "Instance Methods"
'
'#Region "Shared Methods"
'
'        ''' <summary>
'        ''' Adds two impedances in Series.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Function AddSeriesImpedance(ByVal loadZ As Ytt.Util.Electrical.Impedance,
'        ''' ByVal addZ As Ytt.Util.Electrical.Impedance
'        ''' ) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="LoadZ">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The impedance of the load.</p>
'        ''' </param>
'        ''' <param name="addZ">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The impedance of the added component.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The input impedance of the combined load.</p>
'        ''' </returns>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        ''' <remarks>
'        '''   <p>      o-----addZ-----o</p>
'        '''   <p>      |              |</p>
'        '''   <p> Source              loadZ</p>
'        '''   <p>      |              |</p>
'        '''   <p>      o--------------o</p>
'        ''' </remarks>
'        Public Shared Function AddSeriesImpedance(ByVal loadZ As Ytt.Util.Electrical.Impedance,
'                                                  ByVal addZ As Ytt.Util.Electrical.Impedance
'                                                  ) As Ytt.Util.Electrical.Impedance
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(loadZ) OrElse
'                Microsoft.VisualBasic.IsNothing(addZ) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            End If
'
'            Return loadZ + addZ
'
'        End Function
'
'        ''' <summary>
'        ''' Adds two impedances in parallel.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Function AddParallelImpedance(ByVal loadZ As Ytt.Util.Electrical.Impedance,
'        ''' ByVal addZ As Ytt.Util.Electrical.Impedance
'        ''' ) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="LoadZ">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The impedance of the load.</p>
'        ''' </param>
'        ''' <param name="addZ">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The impedance of the added component.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The input impedance of the combined load.</p>
'        ''' </returns>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        ''' <remarks>
'        '''   <p>      o-----o-----o</p>
'        '''   <p>      |     |     |</p>
'        '''   <p> Source   addZ    loadZ</p>
'        '''   <p>      |     |     |</p>
'        '''   <p>      o-----o-----o</p>
'        ''' </remarks>
'        Public Shared Function AddParallelImpedance(ByVal loadZ As Ytt.Util.Electrical.Impedance,
'                                                    ByVal addZ As Ytt.Util.Electrical.Impedance
'                                                    ) As Ytt.Util.Electrical.Impedance
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(loadZ) OrElse
'                Microsoft.VisualBasic.IsNothing(addZ) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            End If
'
'            Return (loadZ.ToAdmittance + addZ.ToAdmittance).ToImpedance
'
'        End Function
'
'        ''' <summary>
'        ''' Adds an admittance in parallel with a load impedance and returns the result.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Function AddParallelAdmittance(ByVal loadZ As Ytt.Util.Electrical.Impedance,
'        ''' ByVal addY As Ytt.Util.Electrical.Admittance
'        ''' ) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="loadZ">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The impedance of the load.</p>
'        ''' </param>
'        ''' <param name="addY">
'        '''   <p>Type: Ytt.Util.Electrical.Admittance</p>
'        '''   <p>Value: The admittance of the added component.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The input impedance of the combined load.</p>
'        ''' </returns>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        ''' <remarks>
'        '''   <p>      o-----o-----o</p>
'        '''   <p>      |     |     |</p>
'        '''   <p> Source   addY    loadZ</p>
'        '''   <p>      |     |     |</p>
'        '''   <p>      o-----o-----o</p>
'        ''' </remarks>
'        Public Shared Function AddParallelAdmittance(ByVal loadZ As Ytt.Util.Electrical.Impedance,
'                                                     ByVal addY As Ytt.Util.Electrical.Admittance
'                                                     ) As Ytt.Util.Electrical.Impedance
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(loadZ) OrElse
'                Microsoft.VisualBasic.IsNothing(addY) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            End If
'
'            Return (loadZ.ToAdmittance + addY).ToImpedance
'
'        End Function
'
'        ''' <summary>
'        ''' Returns the hyperbolic cosine of the specified complex impedance.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Function Cosh(ByVal z As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="z">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The hyperbolic cosine of value.</p>
'        ''' </returns>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        Public Shared Function Cosh(ByVal z As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(z) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            End If
'
'            Dim ValueCosh As System.Numerics.Complex = System.Numerics.Complex.Cosh(z.ToComplex)
'            Return New Ytt.Util.Electrical.Impedance(Cosh.Resistance, Cosh.Reactance)
'
'        End Function
'
'#End Region ' "Shared Methods"
'
'        ''' <summary>
'        ''' Returns the hyperbolic sine of the specified complex impedance.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Function Sinh(ByVal z As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="z">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The hyperbolic sine of value.</p>
'        ''' </returns>
'        ''' <exception cref="System.ArgumentNullException">
'        '''   Thrown when any parameter is <c>Nothing</c>.
'        ''' </exception>
'        Public Shared Function Sinh(ByVal z As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'
'            ' Input checking.
'            If Microsoft.VisualBasic.IsNothing(z) Then
'                Dim ProcName As System.String = ProcNameBase & New System.Diagnostics.StackFrame(0).GetMethod().Name
'                Throw Ytt.Util.RunTime.NewNullParametersException(ProcName)
'            End If
'
'            Dim ValueSinh As System.Numerics.Complex = System.Numerics.Complex.Sinh(z.ToComplex)
'            '            Sinh.Resistance = ValueSinh.Real
'            '            Sinh.Reactance = ValueSinh.Imaginary
'            Return New Ytt.Util.Electrical.Impedance(ValueSinh.Real, ValueSinh.Imaginary)
'
'        End Function
'
'#Region "Operators"
'
'        ''' <summary>
'        ''' Overloaded.
'        ''' Returns the product of two complex impedances.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Operator *(impedance1 As Ytt.Util.Electrical.Impedance,
'        ''' impedance2 As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="impedance1">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <param name="impedance2">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The product of the impedances.</p>
'        ''' </returns>
'        Public Shared Operator *(impedance1 As Ytt.Util.Electrical.Impedance,
'                                 impedance2 As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'            ' No input checking. impedance1 and impedance2 are presumed to have been checked when created.
'            Dim Product As System.Numerics.Complex = impedance1.ToComplex * impedance2.ToComplex
'            Return New Ytt.Util.Electrical.Impedance(Product.Real, Product.Imaginary)
'        End Operator
'
'        ''' <summary>
'        ''' Overloaded.
'        ''' Returns the product of a complex impedance and a scalar value.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Operator *(z As Ytt.Util.Electrical.Impedance, scalar As System.Double) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="z">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <param name="scalar">
'        '''   <p>Type: System.Double</p>
'        '''   <p>Value: The scalar value to be multiplied.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The product of the impedance and the scalar value.</p>
'        ''' </returns>
'        Public Shared Operator *(z As Ytt.Util.Electrical.Impedance, scalar As System.Double) As Ytt.Util.Electrical.Impedance
'            ' No input checking. z is presumed to have been checked when created.
'            Return New Ytt.Util.Electrical.Impedance(z.Resistance, z.Reactance * scalar)
'        End Operator
'
'        ''' <summary>
'        ''' Overloaded.
'        ''' Returns the result of the division of one complex impedance by another.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Operator /(numerator As Ytt.Util.Electrical.Impedance,
'        ''' denominator As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="numerator">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <param name="denominator">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The result of the division.</p>
'        ''' </returns>
'        Public Shared Operator /(numerator As Ytt.Util.Electrical.Impedance,
'                                 denominator As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'            ' No input checking. numerator and denominator are presumed to have been checked when created.
'            Dim Quotient As System.Numerics.Complex = numerator.ToComplex / denominator.ToComplex
'            Return New Ytt.Util.Electrical.Impedance(Quotient.Real, Quotient.Imaginary)
'        End Operator
'
'        ''' <summary>
'        ''' Overloaded.
'        ''' Returns the result of the division of a complex impedance by a scalar value.
'        ''' </summary>
'        ''' <declaration>
'        ''' Public Shared Operator /(z As Ytt.Util.Electrical.Impedance, scalar As System.Double) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="z">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <param name="scalar">
'        '''   <p>Type: System.Double</p>
'        '''   <p>Value: The scalar value of the denominator.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The result of the division.</p>
'        ''' </returns>
'        Public Shared Operator /(z As Ytt.Util.Electrical.Impedance, scalar As System.Double) As Ytt.Util.Electrical.Impedance
'            ' No input checking. z is presumed to have been checked when created.
'            Dim Quotient As System.Numerics.Complex = z.ToComplex / scalar
'            Return New Ytt.Util.Electrical.Impedance(Quotient.Real, Quotient.Imaginary)
'        End Operator
'
'        ''' <summary>
'        ''' Returns the result of the addition of two complex impedances.
'        ''' </summary>
'        ''' <declaration>
'        '''  Public Shared Operator +(impedance1 As Ytt.Util.Electrical.Impedance,
'        ''' impedance2 As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'        ''' </declaration>
'        ''' <param name="impedance1">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <param name="impedance2">
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: An impedance.</p>
'        ''' </param>
'        ''' <returns>
'        '''   <p>Type: Ytt.Util.Electrical.Impedance</p>
'        '''   <p>Value: The result of the addition.</p>
'        ''' </returns>
'        Public Shared Operator +(impedance1 As Ytt.Util.Electrical.Impedance,
'                                 impedance2 As Ytt.Util.Electrical.Impedance) As Ytt.Util.Electrical.Impedance
'            ' No input checking. left and right are presumed to have been checked when created.
'            Dim TotalC As System.Numerics.Complex = impedance1.ToComplex + impedance2.ToComplex
'            Return New Ytt.Util.Electrical.Impedance(TotalC.Real, TotalC.Imaginary)
'        End Operator
'
'#End Region
'
'        ' The resistance and reactance are stored as a complex number.
'        Private m_Complex As System.Numerics.Complex
'
'    End Structure ' Impedance
'
'End Module ' Electrical
