Imports OSNW.Math
Imports Xunit
Imports OsnwAngDim2D = OSNW.Math.D2.Angle.AngularDimension
Imports OsnwAngle2D = OSNW.Math.D2.Angle
Imports OsnwAngNormStyle = OSNW.Math.D2.Angle.NormalizationStyle

Namespace GeometricTests

    Public Class AngleTests

#Region "TestGrade"

        <Theory>
        <InlineData(RAD090d, Double.PositiveInfinity)>
        <InlineData(RAD045d, 100.0)>
        <InlineData(0.0, 0.0)>
        <InlineData(-RAD045d, -100.0)>
        <InlineData(-RAD090d, Double.NegativeInfinity)>
        Public Sub AngleToGrade_GoodValues_Succeeds(angleInRadians As Double, expected As Double)

            Const ZeroTolerance As Double = OSNW.Math.DFLTEQUALITYTOLERANCE * OSNW.Math.TWOPId
            Const ResultTolerance As Double = OSNW.Math.DFLTEQUALITYTOLERANCE * OSNW.Math.TWOPId

            Dim Result As Double = D2.Angle.AngleToGrade(angleInRadians)

            If EqualEnoughZero(ZeroTolerance, angleInRadians) Then
                ' Force it to be exactly zero.
                Assert.Equal(0.0, Result)
            Else
                Assert.Equal(expected, Result, ResultTolerance)
            End If

        End Sub

        <Theory>
        <InlineData(0.0, 0.0)>
        <InlineData(RAD090d, Double.PositiveInfinity)>
        <InlineData(-RAD090d, Double.NegativeInfinity)>
        Public Sub AngleToGrade_AbormalValues_AlsoSucceeds(angleInRadians As Double, expected As Double)
            Const Tolerance As Double = OSNW.Math.DFLTEQUALITYTOLERANCE * OSNW.Math.TWOPId
            Dim Result As Double = D2.Angle.AngleToGrade(angleInRadians)
            Assert.Equal(expected, Result, Tolerance)
        End Sub

        <Theory>
        <InlineData(RAD090d + 0.001)>
        <InlineData(-RAD090d - 0.001)>
        <InlineData(Double.NaN)>
        Public Sub AngleToGrade_BadValues_Fails(angleInRadians As Double)
            Dim Result As Double = D2.Angle.AngleToGrade(angleInRadians)
            Assert.True(Double.IsNaN(Result))
        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, RAD090d)>
        <InlineData(100.0, RAD045d)>
        <InlineData(0.0, 0.0)>
        <InlineData(-100.0, -RAD045d)>
        <InlineData(Double.NegativeInfinity, -RAD090d)>
        Public Sub GradeToAngle_GoodValues_Succeeds(grade As Double, expected As Double)
            Const Tolerance As Double = OSNW.Math.DFLTEQUALITYTOLERANCE * OSNW.Math.TWOPId
            Dim Result As Double = D2.Angle.GradeToAngle(grade)
            Assert.Equal(expected, Result, Tolerance)
        End Sub

        <Theory>
        <InlineData(0.0, 0.0)>
        <InlineData(Double.PositiveInfinity, RAD090d)>
        <InlineData(Double.NegativeInfinity, -RAD090d)>
        Public Sub GradeToAngle_AbormalValues_AlsoSucceeds(grade As Double, expected As Double)
            Const Tolerance As Double = OSNW.Math.DFLTEQUALITYTOLERANCE * OSNW.Math.TWOPId
            Dim Result As Double = D2.Angle.GradeToAngle(grade)
            Assert.Equal(expected, Result, Tolerance)
        End Sub

        <Theory>
        <InlineData(Double.NaN)>
        Public Sub GradeToAngle_BadValues_Fails(grade As Double)
            Dim Result As Double = D2.Angle.GradeToAngle(grade)
            Assert.True(Double.IsNaN(Result))
        End Sub

#End Region ' "TestGrade"

#Region "TestNewAngle"

        <Fact>
        Public Sub New_Default_Succeeds()

            Dim A As New OsnwAngle2D()

            Assert.Equal(OsnwAngle2D.DFLTMAGNITUDE, A.Magnitude)
            Assert.Equal(OsnwAngle2D.DFLTDIMENSION, A.Dimension)
            Assert.Equal(OsnwAngle2D.DFLTSTYLE, A.Style)

        End Sub

        <Theory>
        <InlineData(PId / 4.0, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full)>
        <InlineData(45.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        <InlineData(45.0 * 60.0, OsnwAngDim2D.ArcMinute, OsnwAngNormStyle.Full)>
        <InlineData(45.0 * 60.0 * 60.0, OsnwAngDim2D.ArcSecond, OsnwAngNormStyle.Full)>
        <InlineData(400.0 * PId / 4.0, OsnwAngDim2D.Gradian, OsnwAngNormStyle.Full)>
        <InlineData(1000.0 * PId / 4.0, OsnwAngDim2D.Milliradian, OsnwAngNormStyle.Full)>
        <InlineData(PId / 4.0, OsnwAngDim2D.Radian, OsnwAngNormStyle.Half)>
        Public Sub New_GoodValues_Succeeds(m As Double, d As OsnwAngDim2D, s As OsnwAngNormStyle)

            Const Tolerance As Double = 0.001

            Dim A As New OsnwAngle2D(m, d, s)

            Assert.Equal(m, A.Magnitude, Tolerance)
            Assert.Equal(d, A.Dimension)
            Assert.Equal(s, A.Style)

        End Sub

        <Fact>
        Public Sub New_NaN_Succeeds()

            Dim A As New OsnwAngle2D(Double.NaN, OsnwAngle2D.DFLTDIMENSION, OsnwAngle2D.DFLTSTYLE)

            Assert.True(Double.IsNaN(A.Magnitude))
            Assert.Equal(OsnwAngle2D.DFLTDIMENSION, A.Dimension)
            Assert.Equal(OsnwAngle2D.DFLTSTYLE, A.Style)

        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity)>
        <InlineData(Double.NegativeInfinity)>
        Public Sub New_Infinity_Succeeds(m As Double)

            Dim A As New OsnwAngle2D(m, OsnwAngle2D.DFLTDIMENSION, OsnwAngle2D.DFLTSTYLE)

            Assert.True(Double.IsInfinity(A.Magnitude))
            Assert.Equal(m, A.Magnitude)
            Assert.Equal(OsnwAngle2D.DFLTDIMENSION, A.Dimension)
            Assert.Equal(OsnwAngle2D.DFLTSTYLE, A.Style)

        End Sub

        <Theory>
        <InlineData(PId / 4.0, 0, OsnwAngNormStyle.Full)>
        <InlineData(PId / 4.0, 5, OsnwAngNormStyle.Full)>
        <InlineData(PId / 4.0, 6, OsnwAngNormStyle.Full)>
        <InlineData(PId / 4.0, OsnwAngDim2D.Radian, 0)>
        <InlineData(PId / 4.0, OsnwAngDim2D.Radian, 1)>
        <InlineData(PId / 4.0, OsnwAngDim2D.Radian, 2)>
        Public Sub New_NonStd_Succeeds(m As Double, d As OsnwAngDim2D, s As OsnwAngNormStyle)

            Const Tolerance As Double = 0.001

            Dim A As New OsnwAngle2D(m, d, s)

            Assert.Equal(m, A.Magnitude, Tolerance)
            Assert.Equal(d, A.Dimension)
            Assert.Equal(s, A.Style)

        End Sub

#End Region ' "TestNewAngle"

#Region "TestScaleDimension"

        <Theory>
        <InlineData(45.0, D2.RADIANPERDEGREE, D2.DEGREEPERRADIAN, 45.0)> ' Same in/out.
        <InlineData(PId / 4.0, 1.0, D2.DEGREEPERRADIAN, 45.0)> ' Radians in.
        <InlineData(45.0, D2.RADIANPERDEGREE, 1.0, PId / 4.0)> ' Radians out.
        <InlineData(45.0, D2.RADIANPERDEGREE, D2.REVOLUTIONPERRADIAN, 1.0 / 8.0)>
        <InlineData(1.0 / 8.0, D2.RADIANPERREVOLUTION, D2.DEGREEPERRADIAN, 45.0)>
        <InlineData(200.0, D2.RADIANPERGRADIAN, D2.REVOLUTIONPERRADIAN, 1.0 / 2.0)>
        <InlineData(1.0 / 2.0, D2.RADIANPERREVOLUTION, D2.GRADIANPERRADIAN, 200.0)>
        <InlineData(-45.0, D2.RADIANPERDEGREE, D2.REVOLUTIONPERRADIAN, -1.0 / 8.0)>
        Public Sub ScaleDimensionFactors_GoodValues_Succeeds(
            InM As Double, radiansPerUnitIn As Double, unitsOutPerRadian As Double, expectedM As Double)

            Const Tolerance As Double = 0.001
            Dim Scaled As Double = D2.Angle.ScaleDimension(InM, radiansPerUnitIn, unitsOutPerRadian)
            Assert.Equal(expectedM, Scaled, Tolerance)
        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, D2.RADIANPERDEGREE, D2.REVOLUTIONPERRADIAN,
                    Double.PositiveInfinity)>
        <InlineData(Double.NegativeInfinity, D2.RADIANPERDEGREE, D2.REVOLUTIONPERRADIAN,
                    Double.NegativeInfinity)>
        Public Sub ScaleDimensionFactors_AbnormalValues_AlsoSucceeds(
            InM As Double, radiansPerUnitIn As Double, unitsOutPerRadian As Double, expectedM As Double)

            Const Tolerance As Double = 0.001
            Dim Scaled As Double = D2.Angle.ScaleDimension(InM, radiansPerUnitIn, unitsOutPerRadian)
            Assert.Equal(expectedM, Scaled, Tolerance)
        End Sub

        <Theory>
        <InlineData(Double.NaN, D2.RADIANPERDEGREE, D2.REVOLUTIONPERRADIAN)>
        <InlineData(45.0, Double.NaN, D2.REVOLUTIONPERRADIAN)>
        <InlineData(45.0, D2.RADIANPERDEGREE, Double.NaN)>
        <InlineData(45.0, Double.PositiveInfinity, D2.REVOLUTIONPERRADIAN)>
        <InlineData(45.0, Double.NegativeInfinity, D2.REVOLUTIONPERRADIAN)>
        <InlineData(45.0, D2.RADIANPERDEGREE, Double.PositiveInfinity)>
        <InlineData(45.0, D2.RADIANPERDEGREE, Double.NegativeInfinity)>
        <InlineData(45.0, -D2.RADIANPERDEGREE, D2.REVOLUTIONPERRADIAN)>
        <InlineData(45.0, D2.RADIANPERDEGREE, -D2.REVOLUTIONPERRADIAN)>
        Public Sub ScaleDimensionFactors_BadValues_Fails(InM As Double, radiansPerUnitIn As Double,
                                                         unitsOutPerRadian As Double)

            Dim Scaled As Double = D2.Angle.ScaleDimension(InM, radiansPerUnitIn, unitsOutPerRadian)
            Assert.True(Double.IsNaN(Scaled))
        End Sub

        <Theory>
        <InlineData(45.0, OsnwAngDim2D.Degree, OsnwAngDim2D.Degree, 45.0)> ' Same in/out.
        <InlineData(PId / 4.0, OsnwAngDim2D.Radian, OsnwAngDim2D.Degree, 45.0)> ' Radians in.
        <InlineData(45.0, OsnwAngDim2D.Degree, OsnwAngDim2D.Radian, PId / 4.0)> ' Radians out.
        <InlineData(45.0, OsnwAngDim2D.Degree, OsnwAngDim2D.Revolution, 1.0 / 8.0)>
        <InlineData(1.0 / 8.0, OsnwAngDim2D.Revolution, OsnwAngDim2D.Degree, 45.0)>
        <InlineData(-1.0 / 8.0, OsnwAngDim2D.Revolution, OsnwAngDim2D.Degree, -45.0)>
        <InlineData(Double.PositiveInfinity, OsnwAngDim2D.Revolution, OsnwAngDim2D.Degree,
                    Double.PositiveInfinity)>
        <InlineData(Double.NegativeInfinity, OsnwAngDim2D.Revolution, OsnwAngDim2D.Degree,
                    Double.NegativeInfinity)>
        Public Sub ScaleDimensionValues_Defined_Succeeds(InM As Double, InD As OsnwAngDim2D,
            OutD As OsnwAngDim2D, expectedM As Double)

            Const Tolerance As Double = 0.001
            Dim Scaled As Double = D2.Angle.ScaleDimension(InM, InD, OutD)
            Assert.Equal(expectedM, Scaled, Tolerance)
        End Sub

        <Theory>
        <InlineData(PId, OsnwAngDim2D.Radian - 1, OsnwAngDim2D.Milliradian)>
        <InlineData(PId, OsnwAngDim2D.Radian, OsnwAngDim2D.Milliradian + 1)>
        <InlineData(PId, OsnwAngDim2D.Radian - 1, OsnwAngDim2D.Radian - 1)>
        <InlineData(PId, OsnwAngDim2D.Milliradian + 1, OsnwAngDim2D.Milliradian + 1)>
        Public Sub ScaleDimensionValues_Undefined_Fails(InM As Double, InD As OsnwAngDim2D,
                                                        OutD As OsnwAngDim2D)

            Dim Scaled As Double = D2.Angle.ScaleDimension(InM, InD, OutD)
            Assert.True(Double.IsNaN(Scaled))
        End Sub

        <Theory>
        <InlineData(90.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full,
                    OsnwAngDim2D.Radian, RAD090d)>
        <InlineData(RAD090d, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full,
                    OsnwAngDim2D.Degree, 90.0)>
        <InlineData(270.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full,
                    OsnwAngDim2D.Radian, RAD270d)>
        <InlineData(RAD270d, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full,
                    OsnwAngDim2D.Degree, 270.0)>
        Public Sub ScaleDimensionObj_Defined_Succeeds(mIn As Double, dIn As D2.Angle.AngularDimension,
            sIn As D2.Angle.NormalizationStyle, newD As D2.Angle.AngularDimension, expectedM As Double)

            Const Tolerance As Double = 0.001
            Dim InA As New D2.Angle(mIn, dIn, sIn)

            Dim OutA As D2.Angle = InA.ScaleDimension(newD)

            Assert.Equal(expectedM, OutA.Magnitude, Tolerance)
            Assert.Equal(newD, OutA.Dimension)
            Assert.Equal(sIn, OutA.Style)

        End Sub

        <Theory>
        <InlineData(RAD090d, OsnwAngDim2D.Radian - 1, OsnwAngNormStyle.Full,
                    OsnwAngDim2D.Milliradian)>
        <InlineData(RAD090d, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full,
                    OsnwAngDim2D.Milliradian + 1)>
        Public Sub ScaleDimensionObj_Undefined_Fails(mIn As Double, dIn As D2.Angle.AngularDimension,
            sIn As D2.Angle.NormalizationStyle, newD As D2.Angle.AngularDimension)

            Dim InA As New D2.Angle(mIn, dIn, sIn)
            Dim Scaled As D2.Angle = InA.ScaleDimension(newD)
            Assert.True(Double.IsNaN(Scaled.Magnitude))
        End Sub

#End Region ' "TestScaleDimension"

#Region "TestDefinedDimension"

        <Theory>
        <InlineData(OsnwAngDim2D.Radian)>
        <InlineData(OsnwAngDim2D.Milliradian)>
        Public Sub IsDefinedDimension_Defined_Succeeds(ByVal d As OsnwAngDim2D)
            Assert.True(D2.Angle.IsDefinedDimension(d))
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian - 1)>
        <InlineData(OsnwAngDim2D.Milliradian + 1)>
        Public Sub IsDefinedDimension_Undefined_Fails(ByVal d As OsnwAngDim2D)
            Assert.False(D2.Angle.IsDefinedDimension(d))
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian)>
        <InlineData(OsnwAngDim2D.Milliradian)>
        Public Sub HasDefinedDimension_Defined_Succeeds(d As OsnwAngDim2D)
            Dim A As New OsnwAngle2D(1, d, OsnwAngNormStyle.Full)
            Assert.True(A.HasDefinedDimension)
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian - 1)>
        <InlineData(OsnwAngDim2D.Milliradian + 1)>
        Public Sub HasDefinedDimension_Undefined_Fails(d As OsnwAngDim2D)
            Dim A As New OsnwAngle2D(1, d, OsnwAngNormStyle.Full)
            Assert.False(A.HasDefinedDimension)
        End Sub

#End Region ' "TestDefinedDimension"

#Region "TestDefinedStyle"

        <Theory>
        <InlineData(OsnwAngNormStyle.Full)>
        <InlineData(OsnwAngNormStyle.Half)>
        Public Sub IsDefinedStyle_Defined_Succeeds(ByVal s As OsnwAngNormStyle)
            Assert.True(D2.Angle.IsDefinedStyle(s))
        End Sub

        <Theory>
        <InlineData(OsnwAngNormStyle.Full - 1)>
        <InlineData(OsnwAngNormStyle.Half + 1)>
        Public Sub IsDefinedStyle_Undefined_Fails(ByVal s As OsnwAngNormStyle)
            Assert.False(D2.Angle.IsDefinedStyle(s))
        End Sub

        ''' <summary>
        ''' Tests Public Function HasDefinedStyle() As System.Boolean
        ''' </summary>
        <Theory>
        <InlineData(OsnwAngNormStyle.Full)>
        <InlineData(OsnwAngNormStyle.Half)>
        Public Sub HasDefinedStyle_Defined_Succeeds(s As OsnwAngNormStyle)
            Dim A As New OsnwAngle2D(1, OsnwAngDim2D.Radian, s)
            Assert.True(A.HasDefinedStyle)
        End Sub

        ''' <summary>
        ''' Tests Public Function HasDefinedStyle() As System.Boolean
        ''' </summary>
        <Theory>
        <InlineData(OsnwAngNormStyle.Full - 1)>
        <InlineData(OsnwAngNormStyle.Half + 1)>
        Public Sub HasDefinedStyle_Undefined_Fails(s As OsnwAngNormStyle)
            Dim A As New OsnwAngle2D(1, OsnwAngDim2D.Radian, s)
            Assert.False(A.HasDefinedStyle)
        End Sub

#End Region ' "TestDefinedStyle"

#Region "TestDimensionSize"

        <Theory>
        <InlineData(OsnwAngDim2D.Radian, TWOPId)>
        <InlineData(OsnwAngDim2D.Milliradian, 1000.0 * TWOPId)>
        Public Sub GetFullDimensionSizeDimen_Defined_Succeeds(
            d As D2.Angle.AngularDimension, expectedSize As Double)

            Const Tolerance As Double = 0.001
            Dim Size As Double = OsnwAngle2D.GetFullDimensionSize(d)
            Assert.Equal(expectedSize, Size, Tolerance)
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian - 1)>
        <InlineData(OsnwAngDim2D.Milliradian + 1)>
        Public Sub GetFullDimensionSizeDimen_Undefined_Fails(d As D2.Angle.AngularDimension)
            Dim Size As Double = OsnwAngle2D.GetFullDimensionSize(d)
            Assert.True(Double.IsNaN(Size))
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian, TWOPId)>
        <InlineData(OsnwAngDim2D.Milliradian, 1000.0 * TWOPId)>
        Public Sub GetFullDimensionSizeObj_Defined_Succeeds(
            d As D2.Angle.AngularDimension, expectedSize As Double)

            Const Tolerance As Double = 0.001
            Dim A As New D2.Angle(1.0, d, OsnwAngNormStyle.Full)
            Dim Size As Double = A.GetFullDimensionSize()
            Assert.Equal(expectedSize, Size, Tolerance)
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian - 1)>
        <InlineData(OsnwAngDim2D.Milliradian + 1)>
        Public Sub GetFullDimensionSizeObj_Undefined_Fails(d As D2.Angle.AngularDimension)
            Dim A As New D2.Angle(1.0, d, OsnwAngNormStyle.Full)
            Dim Size As Double = A.GetFullDimensionSize()
            Assert.True(Double.IsNaN(Size))
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian, PId)>
        <InlineData(OsnwAngDim2D.Milliradian, 1000.0 * PId)>
        Public Sub GetHalfDimensionSizeDimen_Defined_Succeeds(
            d As D2.Angle.AngularDimension, expectedSize As Double)

            Const Tolerance As Double = 0.001
            Dim Size As Double = OsnwAngle2D.GetHalfDimensionSize(d)
            Assert.Equal(expectedSize, Size, Tolerance)
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian - 1)>
        <InlineData(OsnwAngDim2D.Milliradian + 1)>
        Public Sub GetHalfDimensionSizeDimen_Undefined_Fails(d As D2.Angle.AngularDimension)
            Dim Size As Double = OsnwAngle2D.GetHalfDimensionSize(d)
            Assert.True(Double.IsNaN(Size))
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian, PId)>
        <InlineData(OsnwAngDim2D.Milliradian, 1000.0 * PId)>
        Public Sub GetHalfDimensionSizeObj_Defined_Succeeds(d As D2.Angle.AngularDimension,
                                                            expectedSize As Double)

            Const Tolerance As Double = 0.001
            Dim A As New D2.Angle(1.0, d, OsnwAngNormStyle.Half)
            Dim S As Double = A.GetHalfDimensionSize()
            Assert.Equal(expectedSize, S, Tolerance)
        End Sub

        <Theory>
        <InlineData(OsnwAngDim2D.Radian - 1)>
        <InlineData(OsnwAngDim2D.Milliradian + 1)>
        Public Sub GetHalfDimensionSizeObj_Undefined_Fails(d As D2.Angle.AngularDimension)
            Dim A As New D2.Angle(1.0, d, OsnwAngNormStyle.Half)
            Dim Size As Double = A.GetHalfDimensionSize()
            Assert.True(Double.IsNaN(Size))
        End Sub

#End Region ' "TestDimensionSize"

#Region "TestIsNormalized"

        <Theory>
        <InlineData(0.0, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full)>
        <InlineData(1.99 * PId, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        <InlineData(359.99, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        <InlineData(-179.99, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half)>
        Public Sub IsNormalized_Normal_Succeeds(m As Double, d As OsnwAngDim2D, s As OsnwAngNormStyle)
            Dim A As New D2.Angle(m, d, s)
            Assert.True(A.IsNormalized())
        End Sub

        <Theory>
        <InlineData(0.0, OsnwAngDim2D.Radian - 1, OsnwAngNormStyle.Full)>
        <InlineData(0.0, OsnwAngDim2D.Milliradian + 1, OsnwAngNormStyle.Full)>
        <InlineData(0.0, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full - 1)>
        <InlineData(0.0, OsnwAngDim2D.Radian, OsnwAngNormStyle.Half + 1)>
        <InlineData(-0.01, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full)>
        <InlineData(2.01 * PId, OsnwAngDim2D.Radian, OsnwAngNormStyle.Full)>
        <InlineData(-0.01, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        <InlineData(-180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half)>
        <InlineData(180.01, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half)>
        Public Sub IsNormalized_Abnormal_Fails(m As Double, d As OsnwAngDim2D, s As OsnwAngNormStyle)
            Dim A As New D2.Angle(m, d, s)
            Assert.False(A.IsNormalized())
        End Sub

#End Region ' "TestIsNormalized"

#Region "TestGetNormalizedMagnitude"

        <Theory>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.0)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.0)>
        <InlineData(0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.1)>
        <InlineData(0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.1)>
        <InlineData(179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 179.9)>
        <InlineData(179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 179.9)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.1)>
        <InlineData(180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -179.9)>
        <InlineData(359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 359.9)>
        <InlineData(359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -0.1)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.0)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 90.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 90.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 90.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 90.0)>
        <InlineData(540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 270.0)>
        <InlineData(630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -90.0)>
        <InlineData(-0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 359.9)>
        <InlineData(-0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -0.1)>
        <InlineData(-179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.1)>
        <InlineData(-179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -179.9)>
        <InlineData(-180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(-180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(-180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 179.9)>
        <InlineData(-180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 179.9)>
        <InlineData(-359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.1)>
        <InlineData(-359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.1)>
        <InlineData(-360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.0)>
        <InlineData(-360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 270.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -90.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 270.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -90.0)>
        <InlineData(-540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(-540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(-630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 90.0)>
        <InlineData(-630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 90.0)>
        Public Sub GetNormalizedMagnitude_Defined_Succeeds(m As Double, d As OsnwAngDim2D,
                                                           s As OsnwAngNormStyle, expectedM As Double)

            Const Tolerance As Double = 0.001
            Dim A As New D2.Angle(m, d, s)
            Dim NormM As Double = A.GetNormalizedMagnitude()
            Assert.Equal(expectedM, NormM, Tolerance)
        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        <InlineData(Double.NegativeInfinity, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        Public Sub GetNormalizedMagnitude_Infinite_AlsoSucceeds(m As Double, d As OsnwAngDim2D,
            s As OsnwAngNormStyle)

            Dim A As New OsnwAngle2D(m, d, s)

            Assert.True(Double.IsInfinity(A.Magnitude))
            Assert.Equal(d, A.Dimension)
            Assert.Equal(s, A.Style)

        End Sub

        <Theory>
        <InlineData(45.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full - 1)>
        <InlineData(45.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half + 1)>
        Public Sub GetNormalizedMagnitude_Undefined_Fails(m As Double, d As OsnwAngDim2D,
                                                          s As OsnwAngNormStyle)
            Dim A As New D2.Angle(m, d, s)
            Dim NorM As Double = A.GetNormalizedMagnitude()
            Assert.True(Double.IsNaN(NorM))
        End Sub

#End Region ' "TestGetNormalizedMagnitude"

#Region "TestCreateNormalizedAngle"

        <Theory>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.0)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.0)>
        <InlineData(0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.1)>
        <InlineData(0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.1)>
        <InlineData(179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 179.9)>
        <InlineData(179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 179.9)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.1)>
        <InlineData(180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -179.9)>
        <InlineData(359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 359.9)>
        <InlineData(359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -0.1)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.0)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 90.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 90.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 90.0)>
        <InlineData(450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 90.0)>
        <InlineData(540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 270.0)>
        <InlineData(630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -90.0)>
        <InlineData(-0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 359.9)>
        <InlineData(-0.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -0.1)>
        <InlineData(-179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.1)>
        <InlineData(-179.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -179.9)>
        <InlineData(-180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(-180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(-180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 179.9)>
        <InlineData(-180.1, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 179.9)>
        <InlineData(-359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.1)>
        <InlineData(-359.9, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.1)>
        <InlineData(-360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.0)>
        <InlineData(-360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 270.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -90.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 270.0)>
        <InlineData(-450.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -90.0)>
        <InlineData(-540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 180.0)>
        <InlineData(-540.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 180.0)>
        <InlineData(-630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 90.0)>
        <InlineData(-630.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 90.0)>
        Public Sub CreateNormalizedAngle_GoodValues_Succeeds(m As System.Double,
            d As D2.Angle.AngularDimension, s As D2.Angle.NormalizationStyle, expectedM As Double)

            Const Tolerance As Double = 0.001

            Dim A As D2.Angle = D2.Angle.CreateNormalizedAngle(m, d, s)

            Assert.Equal(expectedM, A.Magnitude, Tolerance)
            Assert.Equal(d, A.Dimension)
            Assert.Equal(s, A.Style)

        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        <InlineData(Double.NegativeInfinity, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half)>
        Public Sub CreateNormalizedAngle_Infinite_AlsoSucceeds(ByVal m As System.Double,
            ByVal d As D2.Angle.AngularDimension, ByVal s As D2.Angle.NormalizationStyle)

            Dim A As D2.Angle = D2.Angle.CreateNormalizedAngle(m, d, s)

            Assert.True(Double.IsInfinity(A.Magnitude))
            Assert.Equal(d, A.Dimension)
            Assert.Equal(s, A.Style)

        End Sub

        <Theory>
        <InlineData(Double.NaN, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full)>
        Public Sub CreateNormalizedAngle_NaN_Fails(ByVal m As System.Double,
            ByVal d As D2.Angle.AngularDimension, ByVal s As D2.Angle.NormalizationStyle)

            Dim A As D2.Angle = D2.Angle.CreateNormalizedAngle(m, d, s)
            Assert.True(Double.IsNaN(A.Magnitude))
        End Sub

        <Theory>
        <InlineData(0.0, OsnwAngDim2D.Radian - 1, OsnwAngNormStyle.Full)>
        <InlineData(0.0, OsnwAngDim2D.Milliradian + 1, OsnwAngNormStyle.Full)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full - 1)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half + 1)>
        Public Sub CreateNormalizedAngle_Undefined_Fails(ByVal m As System.Double,
            ByVal d As D2.Angle.AngularDimension, ByVal s As D2.Angle.NormalizationStyle)

            'Dim Ex As Exception = Assert.Throws(Of ArgumentOutOfRangeException)(
            '    Sub()
            '        ' Code that throws the exception.
            '        Dim A As D2.Angle = D2.Angle.CreateNormalizedAngle(m, d, s)
            '    End Sub)

            Dim A As D2.Angle = D2.Angle.CreateNormalizedAngle(m, d, s)
            Assert.True(Double.IsNaN(A.Magnitude))

        End Sub


#End Region ' "TestCreateNormalizedAngle"

#Region "TestGetNormalizedRotatedAngle"

        <Theory>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.1, 0.1)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.1, 0.1)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, -0.1, 359.9)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -0.1, -0.1)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.1, 180.1)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.1, -179.9)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, -0.1, 179.9)>
        <InlineData(180.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -0.1, 179.9)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 0.1, 0.1)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 0.1, 0.1)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, -0.1, 359.9)>
        <InlineData(360.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -0.1, -0.1)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 450.0, 90.0)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 450.0, 90.0)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, -450.0, 270.0)>
        <InlineData(0.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -450.0, -90.0)>
        <InlineData(-390.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, 60.0, 30.0)>
        <InlineData(-390.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 60.0, 30.0)>
        <InlineData(-390.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full, -60.0, 270.0)>
        <InlineData(-390.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, -60.0, -90.0)>
        <InlineData(90.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 45.0, 135.0)>
        Public Shared Sub GetNormalizedRotatedAngle_Defined_Succeeds(
            m As System.Double, d As D2.Angle.AngularDimension, s As D2.Angle.NormalizationStyle,
            rotation As System.Double, expectedM As System.Double)

            Const Tolerance As Double = 0.001
            Dim AIn As New D2.Angle(m, d, s)

            Dim RotatedA As D2.Angle = D2.Angle.GetNormalizedRotatedAngle(AIn, rotation)

            Assert.Equal(expectedM, RotatedA.Magnitude, Tolerance)
            Assert.Equal(d, RotatedA.Dimension)
            Assert.Equal(s, RotatedA.Style)

        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 45.0)>
        <InlineData(Double.NegativeInfinity, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 45.0)>
        <InlineData(90.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, Double.PositiveInfinity)>
        <InlineData(90.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, Double.NegativeInfinity)>
        Public Shared Sub GetNormalizedRotatedAngle_Infinities_AlsoSucceeds(
            m As System.Double, d As D2.Angle.AngularDimension, s As D2.Angle.NormalizationStyle,
            rotation As System.Double)

            Dim AIn As New D2.Angle(m, d, s)

            Dim RotatedA As D2.Angle = D2.Angle.GetNormalizedRotatedAngle(AIn, rotation)

            Assert.True(Double.IsInfinity(RotatedA.Magnitude))
            Assert.Equal(d, RotatedA.Dimension)
            Assert.Equal(s, RotatedA.Style)

        End Sub

        <Theory>
        <InlineData(Double.NaN, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, 45.0)>
        <InlineData(90.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half, Double.NaN)>
        Public Shared Sub GetNormalizedRotatedAngle_NaN_AlsoSucceeds(
            m As System.Double, d As D2.Angle.AngularDimension, s As D2.Angle.NormalizationStyle,
            rotation As System.Double)

            Dim AIn As New D2.Angle(m, d, s)

            Dim RotatedA As D2.Angle = D2.Angle.GetNormalizedRotatedAngle(AIn, rotation)

            Assert.True(Double.IsNaN(RotatedA.Magnitude))
            Assert.Equal(d, RotatedA.Dimension)
            Assert.Equal(s, RotatedA.Style)

        End Sub

        <Theory>
        <InlineData(90.0, OsnwAngDim2D.Radian - 1, OsnwAngNormStyle.Half, 45.0)>
        <InlineData(90.0, OsnwAngDim2D.Milliradian + 1, OsnwAngNormStyle.Half, 45.0)>
        <InlineData(90.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Full - 1, 45.0)>
        <InlineData(90.0, OsnwAngDim2D.Degree, OsnwAngNormStyle.Half + 1, 45.0)>
        Public Shared Sub GetNormalizedRotatedAngle_Undefined_Fails(
            m As System.Double, d As D2.Angle.AngularDimension, s As D2.Angle.NormalizationStyle,
            rotation As System.Double)

            Dim AIn As New D2.Angle(m, d, s)

            Dim RotatedA As D2.Angle = D2.Angle.GetNormalizedRotatedAngle(AIn, rotation)

            Assert.True(Double.IsNaN(RotatedA.Magnitude))
            Assert.Equal(d, RotatedA.Dimension)
            Assert.Equal(s, RotatedA.Style)

        End Sub

#End Region ' "TestGetNormalizedRotatedAngle"

#Region "TestDegMinSec"

        <Theory>
        <InlineData(0.5, 0, 30.0)>
        <InlineData(90.5, 90, 30.0)>
        <InlineData(270.5, 270, 30.0)>
        <InlineData(450.5, 450, 30.0)>
        <InlineData(-0.5, 0, -30.0)>
        <InlineData(-90.5, -90, 30.0)>
        <InlineData(-270.5, -270, 30.0)>
        <InlineData(-450.5, -450, 30.0)>
        Public Sub DegToDddMm_GoodValues_Succeeds(dIn As Double, expectedD As Int32, expectedM As Double)

            Const Tolerance As Double = 0.001
            Dim DOut As System.Int32
            Dim MOut As System.Double

            D2.Angle.DegToDddMm(dIn, DOut, MOut)

            Assert.Equal(expectedD, DOut)
            Assert.Equal(expectedM, MOut, Tolerance)

        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity)>
        <InlineData(Double.NegativeInfinity)>
        <InlineData(Double.NaN)>
        Public Sub DegToDddMm_AbnormalValues_AlsoSucceeds(dIn As Double)

            Dim ResultD As Int32
            Dim ResultM As Double

            D2.Angle.DegToDddMm(dIn, ResultD, ResultM)

            Assert.Equal(0, ResultD)
            Assert.Equal(Double.NaN, ResultM)

        End Sub

        ' 30 sec * (1 min / 60 sec) * (1 deg / 60 min)
        ' 30 * (1 / 60) * (1 deg / 60)
        ' 30 / (60*60) deg
        ' 1 / (2*60) deg
        ' (1 / 120) deg
        Const SEC30 As Double = 1 / 120.0 ' 0.0083333333333333333

        <Theory>
        <InlineData(30.2639, 30, 15, 50.0)>
        <InlineData(10.344444, 10, 20, 40.0)>
        <InlineData(SEC30, 0, 0, 30.0)>
        <InlineData(90.0 + SEC30, 90, 0, 30.0)>
        <InlineData(270.0 + SEC30, 270, 0, 30.0)>
        <InlineData(450.0 + SEC30, 450, 0, 30.0)>
        <InlineData(-0.0 - SEC30, 0, 0, -30.0)>
        <InlineData(-90.0 - SEC30, -90, 0, 30.0)>
        <InlineData(-270.0 - SEC30, -270, 0, 30.0)>
        <InlineData(-450.0 - SEC30, -450, 0, 30.0)>
        Public Sub DegToDddMmSs_GoodValues_Succeeds(dIn As Double, expectedD As Int32,
                                                    expectedM As Int32, expectedS As Double)

            Const Tolerance As Double = 0.001
            Dim DOut As System.Int32
            Dim MOut As System.Int32
            Dim SOut As System.Double

            D2.Angle.DegToDddMmSs(dIn, DOut, MOut, SOut)

            Assert.Equal(expectedD, DOut)
            Assert.Equal(expectedM, MOut)
            Assert.True(OSNW.Math.EqualEnough(SOut, Tolerance, expectedS))

        End Sub

        <Theory>
        <InlineData(Double.PositiveInfinity)>
        <InlineData(Double.NegativeInfinity)>
        <InlineData(Double.NaN)>
        Public Sub DegToDddMmSs_AbnormalValues_AlsoSucceeds(dIn As Double)

            Dim ResultD As Int32
            Dim ResultM As Int32
            Dim ResultS As Double

            D2.Angle.DegToDddMmSs(dIn, ResultD, ResultM, ResultS)

            Assert.Equal(0, ResultD)
            Assert.Equal(0, ResultM)
            Assert.Equal(Double.NaN, ResultS)

        End Sub

        <Theory>
        <InlineData(0, 30.0, 0.5)>
        <InlineData(90, 30.0, 90.5)>
        <InlineData(270, 30.0, 270.5)>
        <InlineData(450, 30.0, 450.5)> ' Past -360.
        <InlineData(0, -30.0, -0.5)>
        <InlineData(-90, 30.0, -90.5)> ' Negative.
        <InlineData(-270, 30.0, -270.5)>
        <InlineData(-450, 30.0, -450.5)> ' Past -360.
        <InlineData(450, 60, 451.0)> ' Mixed signs.
        <InlineData(-1, 120, -3.0)> ' Mixed signs.
        Public Sub DddMmToDeg_GoodValues_Succeeds(dIn As Int32, mIn As Double, expectedD As Double)
            Const Tolerance As Double = 0.001
            Dim D As Double
            D2.Angle.DddMmToDeg(dIn, mIn, D)
            Assert.True(OSNW.Math.EqualEnough(D, Tolerance, expectedD))
        End Sub

        <Theory>
        <InlineData(30, Double.PositiveInfinity)>
        <InlineData(30, Double.NegativeInfinity)>
        <InlineData(30, Double.NaN)>
        <InlineData(91, -60)> ' Mixed signs.
        <InlineData(1, -120)> ' Negative mIn.
        Public Sub DddMmToDeg_BadValues_Fails(dIn As Int32, mIn As Double)
            Dim D As Double
            D2.Angle.DddMmToDeg(dIn, mIn, D)
            Assert.True(Double.IsNaN(D))
        End Sub

        <Theory>
        <InlineData(30, 15, 50.0, 30.2639)>
        <InlineData(10, 20, 40.0, 10.3444)>
        <InlineData(90, 60, 7200.0, 93.0)> ' +++
        <InlineData(-90, 60, 7200.0, -93.0)> ' -++
        <InlineData(0, -15, 0.0, -0.25)> ' 0-+
        <InlineData(0, 0, -60.0, -1 / 60)> ' 00-
        Public Sub DddMmSsToDeg_GoodValues_Succeeds(dIn As Int32, mIn As Int32, sIn As Double,
                                                    expectedD As Double)

            Const Tolerance As Double = 0.001
            Dim DOut As Double

            D2.Angle.DddMmSsToDeg(dIn, mIn, sIn, DOut)

            Assert.True(OSNW.Math.EqualEnough(DOut, Tolerance, expectedD))

        End Sub

        <Theory>
        <InlineData(30, 15, Double.NaN)>
        <InlineData(30, 15, Double.PositiveInfinity)>
        <InlineData(30, 15, Double.NegativeInfinity)>
        <InlineData(90, 60, -7200.0)> ' ++-
        <InlineData(90, -60, 7200.0)> ' +-+
        <InlineData(90, -60, -7200.0)> ' +--
        <InlineData(-90, 60, -7200.0)> ' -+-
        <InlineData(-90, -60, 7200.0)> ' --+
        <InlineData(-90, -60, -7200.0)> ' ---
        Public Sub DddMmSsToDeg_BadValues_Fails(dIn As Int32, mIn As Int32, sIn As Double)
            Dim D As Double
            D2.Angle.DddMmSsToDeg(dIn, mIn, sIn, D)
            Assert.True(Double.IsNaN(D))
        End Sub

#End Region ' "TestDegMinSec"

    End Class ' AngleTests

End Namespace ' GeometricTests
