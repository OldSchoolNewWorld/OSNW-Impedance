' TODO:
' Look into "grade" as used for hills and slopes. It is the tangent of the
'   angle, expressed as a percentage. It is not a unit of measure for angles,
'   but it is a way to express the steepness of a slope. It is commonly used in
'   civil engineering and transportation. It might be worth adding a method to
'   convert an angle to its grade, and vice versa.  

Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Partial Public Module Math

    Partial Public Structure D2

#Region "Conversion Constants"
        ' Angular dimension (unit of measure) conversion constants.

        ''' <summary>
        ''' The SI unit of angular measure is the radian. One radian is defined
        ''' as the angle created when the arc length equals the radius of the
        ''' circle. Therefore, a full circle is 2*PI (~6.28318) radians.
        ''' </summary>
        Public Const RADIANPERCIRCLE As System.Double = 2.0 * OSNW.Math.PId

        ''' <summary>
        ''' Half of see <see cref="RADIANPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERSEMICIRCLE As System.Double = OSNW.Math.PId

        ''' <summary>
        ''' One revolution is one full circle. Therefore, a full circle is
        ''' 1 revolution and one revolution is 2*PI (~6.28319) radians.
        ''' </summary>
        Public Const REVOLUTIONPERCIRCLE As System.Double = 1.0

        ''' <summary>
        ''' Half of see <see cref="REVOLUTIONPERCIRCLE"/>.
        ''' </summary>
        Public Const REVOLUTIONPERSEMICIRCLE As System.Double = 0.5

        ''' <summary>
        ''' See <see cref="REVOLUTIONPERCIRCLE"/>.
        ''' </summary>
        Public Const REVOLUTIONPERRADIAN As System.Double =
            1.0 / (2.0 * OSNW.Math.PId) ' ~0.159155

        ''' <summary>
        ''' See <see cref="REVOLUTIONPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERREVOLUTION As System.Double =
            2.0 * OSNW.Math.PId ' ~6.28319

        ''' <summary>
        ''' One degree is 1/360 of a full circle. Therefore, a full circle is
        ''' 360 degrees and one degree is 2*PI/360 (~1.74533E-2) radians.
        ''' </summary>
        Public Const DEGREEPERCIRCLE As System.Double = 360.0

        ''' <summary>
        ''' Half of see <see cref="DEGREEPERCIRCLE"/>.
        ''' </summary>
        Public Const DEGREEPERSEMICIRCLE As System.Double = 180.0

        ''' <summary>
        ''' See <see cref="DEGREEPERCIRCLE"/>.
        ''' </summary>
        Public Const DEGREEPERRADIAN As System.Double =
            180.0 / OSNW.Math.PId ' ~57.2958

        ''' <summary>
        ''' See <see cref="DEGREEPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERDEGREE As System.Double =
            OSNW.Math.PId / 180.0 ' ~1.74533e-2

        ''' <summary>
        ''' One gradian is defined as one-hundredth of a right angle, which is
        ''' 1/400 of a full circle. Therefore, a full circle is 400 gradians and
        ''' one gradian is 2*PI/400 (~1.57080e-2) radians.
        ''' Gradian and gon are interchangeable.
        ''' </summary>
        Public Const GRADIANPERCIRCLE As System.Double = 400.0

        ''' <summary>
        ''' One gon is equivalent to one gradian. Gradian and gon are
        ''' interchangeable.
        ''' </summary>
        Public Const GONPERCIRCLE As System.Double = GRADIANPERCIRCLE

        ''' <summary>
        ''' Half of see <see cref="GRADIANPERCIRCLE"/>.
        ''' </summary>
        Public Const GRADIANPERSEMICIRCLE As System.Double = 200.0

        ''' <summary>
        ''' Half of see <see cref="GONPERCIRCLE"/>.
        ''' </summary>
        Public Const GONPERSEMICIRCLE As System.Double = GRADIANPERSEMICIRCLE

        ''' <summary>
        ''' See <see cref="GRADIANPERCIRCLE"/>.
        ''' </summary>
        Public Const GRADIANPERRADIAN As System.Double =
            200.0 / OSNW.Math.PId ' ~63.6620

        ''' <summary>
        ''' See <see cref="GONPERCIRCLE"/>.
        ''' </summary>
        Public Const GONPERRADIAN As System.Double = GRADIANPERRADIAN

        ''' <summary>
        ''' See <see cref="GRADIANPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERGRADIAN As System.Double =
            OSNW.Math.PId / 200.0 ' ~1.57080e-2

        ''' <summary>
        ''' See <see cref="GONPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERGON As System.Double = RADIANPERGRADIAN

        ''' <summary>
        ''' One arcminute is 1/60 of a degree and one degree is 1/360 of a full
        ''' circle. Therefore, a full circle is 60*360 (21600) arcminutes and
        ''' one arcminute is (2*PI)/(60*360) (~2.91e-3) radians.
        ''' </summary>
        Public Const ARCMINUTEPERCIRCLE As System.Double = 21600

        ''' <summary>
        ''' Half of see <see cref="ARCMINUTEPERCIRCLE"/>
        ''' </summary>
        Public Const ARCMINUTEPERSEMICIRCLE As System.Double = 10800

        ''' <summary>
        ''' See <see cref="ARCMINUTEPERCIRCLE"/>.
        ''' </summary>
        Public Const ARCMINUTEPERRADIAN As System.Double =
            10800 / OSNW.Math.PId ' (~34.3774)

        ''' <summary>
        ''' See <see cref="ARCMINUTEPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERARCMINUTE As System.Double =
            OSNW.Math.PId / 10800 ' ~2.90888e-4

        ''' <summary>
        ''' One arcsecond is 1/60 of an arcminute. Therefore, a full circle is
        ''' 60*ARCMINUTEPERCIRCLE (1,296,000) arcseconds and one arcsecond is
        ''' 2*PI/1,296,000 (~4.84814e-6) radians.
        ''' </summary>
        Public Const ARCSECONDPERCIRCLE As System.Double = 1296000

        ''' <summary>
        ''' Half of see <see cref="ARCSECONDPERCIRCLE"/>
        ''' </summary>
        Public Const ARCSECONDPERSEMICIRCLE As System.Double = 648000

        ''' <summary>
        ''' See <see cref="ARCSECONDPERCIRCLE"/>.
        ''' </summary>
        Public Const ARCSECONDPERRADIAN As System.Double =
            648000 / OSNW.Math.PId ' (~20626)

        ''' <summary>
        ''' See <see cref="ARCSECONDPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERARCSECOND As System.Double =
            OSNW.Math.PId / 648000 ' ~4.84814e-6

        ''' <summary>
        ''' One milliradian is 1/1000 of a radian. Therefore, a full circle is
        ''' 1000*2*PI milliradians and one milliradian is 2*PI/1000
        ''' (~6.28319e-3) radians.
        ''' Milliradian and mil are interchangeable.
        ''' </summary>
        Public Const MILLIRADIANPERCIRCLE As System.Double =
            2000 * OSNW.Math.PId ' ~6283.19

        ''' <summary>
        ''' One mil is equivalent to one milliradian. Milliradian and mil are
        ''' interchangeable.
        ''' </summary>
        Public Const MILPERCIRCLE As System.Double = MILLIRADIANPERCIRCLE

        ''' <summary>
        ''' Half of see <see cref="MILLIRADIANPERCIRCLE"/>
        ''' </summary>
        Public Const MILLIRADIANPERSEMICIRCLE As System.Double =
            1000 * OSNW.Math.PId ' ~3141.59

        ''' <summary>
        ''' Half of see <see cref="MILPERCIRCLE"/>
        ''' </summary>
        Public Const MILPERSEMICIRCLE As System.Double =
            MILLIRADIANPERSEMICIRCLE

        ''' <summary>
        ''' See <see cref="MILLIRADIANPERCIRCLE"/>.
        ''' </summary>
        Public Const MILLIRADIANPERRADIAN As System.Double = 1000

        ''' <summary>
        ''' See <see cref="MILPERCIRCLE"/>.
        ''' </summary>
        Public Const MILPERRADIAN As System.Double = MILLIRADIANPERRADIAN

        ''' <summary>
        ''' See <see cref="MILLIRADIANPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERMILLIRADIAN As System.Double = 1 / 1000

        ''' <summary>
        ''' See <see cref="MILPERCIRCLE"/>.
        ''' </summary>
        Public Const RADIANPERMIL As System.Double = RADIANPERMILLIRADIAN

#End Region ' "Conversion Constants"

        ''' <summary>
        ''' Represents an angle in a two-dimensional plane.
        ''' </summary>
        ''' <remarks>
        ''' The magnitude of an angle can be either positive or negative. No
        ''' assumption is made as to the associated direction of rotation.
        ''' Magnetic compasses, thermometers, and gauges usually increase in
        ''' magnitude when moving CW. Many geometry texts show positive movement
        ''' as CCW.
        ''' </remarks>
        Public Class Angle

            Public Const DFLTMAGNITUDE As System.Double = System.Double.NaN
            Public Const DFLTDIMENSION As D2.Angle.AngularDimension =
                D2.Angle.AngularDimension.Radian
            Public Const DFLTSTYLE As D2.Angle.NormalizationStyle =
                D2.Angle.NormalizationStyle.Full

            ''' <summary>
            ''' Specifies the dimension (unit of measure to describe an angle
            ''' in a 2-dimensional plane. The SI unit of angular measure is the
            ''' radian.
            ''' </summary>
            ''' <remarks>
            ''' The following values are available:
            ''' <list type="type">
            '''    <item>
            '''       <term>Radian</term>
            '''       <description>
            '''       The SI unit of angular measure is the radian. One radian
            '''       is defined as the angle created when the arc length equals
            '''       the radius of the circle.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>Revolution</term>
            '''       <description>
            '''       One revolution is one full circle.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>Degree</term>
            '''       <description>
            '''       One degree is 1/360 of a full circle.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>Gradian</term>
            '''       <description>
            '''       One gradian is 1/400 of a full circle.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>Gon</term>
            '''       <description>
            '''       Equivalent to one Gradian.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>ArcMinute</term>
            '''       <description>
            '''       One arcminute is 1/60 degree.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>ArcSecond</term>
            '''       <description>
            '''       One arcsecond is 1/60 arcminute or 1/3600 degree.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>Milliradian</term>
            '''       <description>
            '''       One milliradian (mil) is 1/1000 radian.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>Mil</term>
            '''       <description>
            '''       Equivalent to one Milliradian.
            '''       </description>
            '''    </item>
            ''' </list>
            ''' </remarks>
            Public Enum AngularDimension
                ' DEV: D2.Angle.IsDefinedDimension(D2.Angle.AngularDimension),
                ' D2.Angle.HasDefinedDimension(D2.Angle), and
                ' D2.Angle.HasDefinedDimension() currently depend on these being
                ' automatically assigned, with Radian being first and Mil being
                ' last.
                Radian
                Revolution
                Degree
                Gradian
                Gon = Gradian
                ArcMinute
                ArcSecond
                Milliradian
                Mil = Milliradian
            End Enum ' AngularDimension

            ''' <summary>
            ''' Specifies how to normalize the value of an angle in a
            ''' 2-dimensional plane.
            ''' </summary>
            ''' <remarks>
            ''' Normalization can be handled two ways. The following values are
            ''' available:
            ''' <list type="bullet">
            '''    <item>
            '''       <term>Full</term>
            '''       <description>
            '''       The angle rotates away from zero toward the full range,
            '''       wrapping over to zero for each full rotation. The value is
            '''       always positive.
            '''       </description>
            '''    </item>
            '''    <item>
            '''       <term>Half</term>
            '''       <description>
            '''       The angle rotates away from zero in one direction (CW or
            '''       CCW) for positive values and in the opposite direction for
            '''       negative values, wrapping over from positive to negative
            '''       at the half rotation.
            '''       </description>
            '''    </item>
            ''' </list>
            ''' </remarks>
            Public Enum NormalizationStyle
                ' DEV: D2.Angle.IsValidStyle(D2.Angle.NormalizationStyle),
                ' D2.Angle.HasValidStyle(D2.Angle), and D2.Angle.HasValidStyle()
                ' currently depend on these being automatically assigned, with
                ' Full being first and Half being last.
                Full
                Half
            End Enum

#Region "Fields and Properties"

            Private m_Magnitude As System.Double
            ''' <summary>
            ''' Represents the magnitude of this <c>D2.Angle</c>.
            ''' </summary>
            ''' <remarks>
            ''' Any valid <c>System.Double</c> is allowed, in consideration of
            ''' such cases as accumulated rotation of a motor or wheel. The
            ''' expected range of "normal" values is based on the
            ''' <c>Dimension</c> and <c>Style</c> properties.
            ''' <br/>
            ''' Assignment of <c>NaN</c> or an infinite value is allowed, but
            ''' may cause unexpected results. Calling routines might need to
            ''' either verify values prior to calling
            ''' <see cref="New(System.Double, D2.Angle.AngularDimension,
            ''' D2.Angle.NormalizationStyle)"/> or use special handling, after
            ''' the call, where those values are valid.
            ''' </remarks>
            Public Property Magnitude As System.Double
                Get
                    Return Me.m_Magnitude
                End Get
                Private Set
                    Me.m_Magnitude = Value
                End Set
            End Property

            Private m_Dimension As D2.Angle.AngularDimension
            ''' <summary>
            ''' Represents the units of measure associated with this
            ''' <c>D2.Angle</c> and is combined with the scalar value in the
            ''' <see cref="Magnitude"/> property.
            ''' </summary>
            ''' <remarks>
            ''' Assignment of values not defined in
            ''' <see cref="D2.Angle.AngularDimension"/> is allowed, but may
            ''' cause unexpected results. Calling routines might need to either
            ''' verify values prior to calling <see cref="New(System.Double,
            ''' D2.Angle.AngularDimension, D2.Angle.NormalizationStyle)"/> or
            ''' use special handling, after the call, where those values are
            ''' valid.
            ''' </remarks>
            Public Property Dimension As D2.Angle.AngularDimension
                Get
                    Return Me.m_Dimension
                End Get
                Private Set
                    Me.m_Dimension = Value
                End Set
            End Property

            Private m_Style As D2.Angle.NormalizationStyle
            ''' <summary>
            ''' Represents the <see cref="Angle.NormalizationStyle"/> of this
            ''' <c>D2.Angle</c>.
            ''' </summary>
            ''' <remarks>
            ''' Assignment of values not defined in
            ''' <see cref="D2.Angle.NormalizationStyle"/> is allowed, but may
            ''' cause unexpected results. Calling routines might need to either
            ''' verify values prior to calling <see cref="New(System.Double,
            ''' D2.Angle.AngularDimension, D2.Angle.NormalizationStyle)"/> or
            ''' use special handling, after the call, where those values are
            ''' valid.
            ''' </remarks>
            Public Property Style As D2.Angle.NormalizationStyle
                Get
                    Return Me.m_Style
                End Get
                Private Set
                    Me.m_Style = Value
                End Set
            End Property

#End Region ' "Fields and Properties"

#Region "Methods"

#Region "Grade"

            ''' <summary>
            ''' Returns the grade corresponding to the specified
            ''' <paramref name="angleInRadians"/>.
            ''' </summary>
            ''' <param name="angleInRadians">Specifies the angle in radians, in
            ''' the -PI/2 (-90 degrees) to PI/2 (90 degrees) range.</param>
            ''' <returns>The grade corresponding to the specified
            ''' <paramref name="angleInRadians"/>. A negative
            ''' <paramref name="angleInRadians"/> slopes down.</returns>
            ''' <remarks>
            ''' Grade is the tangent of the angle, expressed as a
            ''' percentage. When <paramref name="angleInRadians"/> is
            ''' <c>System.Double.NaN</c> or greater than +/-PI/2 (90 degrees),
            ''' the result is <c>System.Double.IsNaN</c>.
            ''' <br/> 
            ''' "Grade" is commonly used in civil engineering and
            ''' transportation. It is the tangent of an angle, expressed as a
            ''' percentage. It is not a unit of measure for angles; it is a way
            ''' to represent the steepness of a slope, for example, a road. It
            ''' can also represent rise over run for the pitch of a roof,
            ''' expressed as a percentage.
            ''' <br/> 
            ''' Other terms for grade are gradient, slope, incline, mainfall,
            ''' pitch, and rise.
            ''' </remarks>"
            Public Shared Function AngleToGrade(
                ByVal angleInRadians As System.Double) As System.Double

                Const Tolerance As Double =
                    OSNW.Math.DFLTEQUALITYTOLERANCE * OSNW.Math.TWOPId

                ' Input checking.
                If System.Double.Abs(angleInRadians) > OSNW.Math.RAD90d Then

                    Return System.Double.NaN
                End If

                ' Check special cases.
                If OSNW.Math.EqualEnoughZero(angleInRadians, Tolerance) Then
                    Return 0.0
                ElseIf OSNW.Math.EqualEnough(OSNW.Math.RAD90d, Tolerance,
                    System.Math.Abs(angleInRadians)) Then

                    ' The tangent of 90 degrees is undefined, so the grade is
                    ' effectively infinite. Return the largest possible value.
                    If OSNW.Math.EqualEnough(
                        OSNW.Math.RAD90d, 0.001, angleInRadians) Then

                        Return System.Double.PositiveInfinity
                    ElseIf OSNW.Math.EqualEnough(
                        -OSNW.Math.RAD90d, 0.001, angleInRadians) Then

                        Return System.Double.NegativeInfinity
                    End If
                End If

                ' Grade is the tangent of the angle, expressed as a percentage.
                ' Grade = tan(angle) * 100
                ' The angle needs to be in radians for the tangent function.
                Return System.Math.Tan(angleInRadians) * 100.0

            End Function ' AngleToGrade

            ''' <summary>
            ''' Returns the angle in radians corresponding to the specified
            ''' <paramref name="grade"/>.
            ''' </summary>
            ''' <param name="grade">Specifies the percent grade of the
            ''' slope. A negative <paramref name="grade"/> slopes down.</param>
            ''' <returns>The angle in radians corresponding to the specified
            ''' <c>grade</c>.</returns>
            ''' <remarks>
            ''' The angle is calculated as the arctangent of the grade
            ''' divided by 100. When <paramref name="grade"/> is
            ''' <c>Double.NaN</c> the result is <c>System.Double.IsNaN</c>.
            ''' <br/> 
            ''' "Grade" is commonly used in civil engineering and
            ''' transportation. It is the tangent of an angle, expressed as a
            ''' percentage. It is not a unit of measure for angles; it is a way
            ''' to represent the steepness of a slope, for example, a road. It
            ''' can also represent rise over run for the pitch of a roof,
            ''' expressed as a percentage.
            ''' <br/> 
            ''' Other terms for grade are gradient, slope, incline, mainfall,
            ''' pitch, and rise.
            ''' </remarks>
            Public Shared Function GradeToAngle(ByVal grade As System.Double) _
                As System.Double

                ' Atan() computes the arctangent of grade, in radians, in the
                ' [-PI/2, +PI/2] range.
                Return System.Math.Atan(grade / 100.0)

            End Function ' GradeToAngle 

#End Region ' "Grade"

            ''' <summary>
            ''' Converts an angle in degrees to the equivalent angle in degrees
            ''' and minutes.
            ''' </summary>
            ''' <param name="dIn">Specifies the angle in decimal
            ''' degrees.</param>
            ''' <param name="dOut">Returns the integer degrees portion of the
            ''' equivalent angle.</param>
            ''' <param name="mOut">Returns the decimal minutes portion of the
            ''' equivalent angle.</param>
            ''' <remarks>When <paramref name="dIn"/> is <c>Double.NaN</c> or an
            ''' infinite value, <paramref name="dOut"/> is set to zero and
            ''' <paramref name="mOut"/> is set to
            ''' <c>System.Double.IsNaN</c>.</remarks>
            Public Shared Sub DegToDddMm(ByVal dIn As System.Double,
                ByRef dOut As System.Int32, ByRef mOut As System.Double)

                ' Input checking.
                If System.Double.IsNaN(dIn) OrElse
                    System.Double.IsInfinity(dIn) Then

                    dOut = 0
                    mOut = System.Double.NaN
                    Exit Sub
                End If

                ' Break down using a positive angle.
                Dim AbsD As System.Double = System.Double.Abs(dIn)
                dOut = CInt(System.Double.Truncate(AbsD))
                mOut = (AbsD - dOut) * 60.0

                If dIn < 0.0 Then
                    ' The first non-zero term needs to be negative.
                    If dOut > 0 Then
                        dOut = -dOut
                    Else
                        mOut = -mOut
                    End If
                End If

            End Sub ' DegToDddMm

            ''' <summary>
            ''' Converts an angle in degrees to the equivalent angle in degrees,
            ''' minutes, and seconds.
            ''' </summary>
            ''' <param name="dIn">Specifies the angle in decimal
            ''' degrees.</param>
            ''' <param name="dOut">Returns the integer degrees portion of the
            ''' equivalent angle.</param>
            ''' <param name="mOut">Returns the integer minutes portion of the
            ''' equivalent angle.</param>
            ''' <param name="sOut">Returns the decimal seconds portion of the
            ''' equivalent angle.</param>
            ''' <remarks>When <paramref name="dIn"/> is <c>Double.NaN</c> or an
            ''' infinite value, <paramref name="dOut"/> and
            ''' <paramref name="mOut"/> are set to zero and
            ''' <paramref name="sOut"/> is set to
            ''' <c>System.Double.IsNaN</c>.</remarks>
            Public Shared Sub DegToDddMmSs(ByVal dIn As System.Double,
                ByRef dOut As System.Int32, ByRef mOut As System.Int32,
                ByRef sOut As System.Double)

                ' Input checking.
                If System.Double.IsNaN(dIn) OrElse
                    System.Double.IsInfinity(dIn) Then

                    dOut = 0
                    mOut = 0
                    sOut = System.Double.NaN
                    Exit Sub
                End If

                ' Break down using a positive angle.
                Dim AbsD As System.Double = System.Double.Abs(dIn)
                dOut = CInt(System.Double.Truncate(AbsD))
                Dim Min As System.Double = (AbsD - dOut) * 60.0
                mOut = CInt(System.Double.Truncate(Min))
                sOut = (Min - mOut) * 60.0

                If dIn < 0.0 Then
                    ' The first non-zero term needs to be negative.
                    If dOut > 0 Then
                        dOut = -dOut
                    Else
                        If mOut > 0 Then
                            mOut = -mOut
                        Else
                            sOut = -sOut
                        End If
                    End If
                End If

            End Sub ' DegToDddMmSs

            ''' <summary>
            ''' Converts an angle in degrees and minutes to the equivalent angle
            ''' in degrees.
            ''' </summary>
            ''' <param name="dIn">Specifies the integer degrees portion of the
            ''' angle.</param>
            ''' <param name="mIn">Specifies the decimal minutes portion of the
            ''' angle.</param>
            ''' <param name="dOut">Returns the equivalent angle in decimal
            ''' degrees.</param>
            ''' <remarks>When <paramref name="mIn"/> is
            ''' <c>System.Double.NaN</c>, <paramref name="dOut"/> returns
            ''' <c>System.Double.NaN</c>. When <paramref name="mIn"/> is an
            ''' infinite value, <paramref name="dOut"/> returns the same
            ''' infinite value.</remarks>
            Public Shared Sub DddMmToDeg(ByVal dIn As System.Int32,
                ByVal mIn As System.Double, ByRef dOut As System.Double)

                ' Input checking.
                If System.Double.IsNaN(dIn) OrElse
                    System.Double.IsInfinity(dIn) Then
                    dOut = System.Double.NaN
                    '                   dOut = dIn
                    Exit Sub
                End If

                Dim DPart As System.Int32 = dIn
                Dim MPart As System.Double = mIn / 60.0
                If dIn < 0 Then
                    dOut = DPart - MPart
                ElseIf mIn < 0.0 Then
                    dOut = DPart + MPart
                Else
                    dOut = DPart + MPart
                End If

            End Sub ' DddMmToDeg

            ''' <summary>
            ''' Converts an angle in degrees, minutes, and seconds to the
            ''' equivalent angle in decimal degrees.
            ''' </summary>
            ''' <param name="dIn">Specifies the integer degrees portion of the
            ''' angle.</param>
            ''' <param name="mIn">Specifies the integer minutes portion of the
            ''' angle.</param>
            ''' <param name="sIn">Specifies the decimal seconds portion of the
            ''' angle.</param>
            ''' <param name="dOut">Returns the equivalent angle in decimal
            ''' degrees.</param>
            ''' <remarks>When <paramref name="sIn"/> is
            ''' <c>System.Double.NaN</c>, <paramref name="dOut"/> returns
            ''' <c>System.Double.NaN</c>. When <paramref name="sIn"/> is an
            ''' infinite value, <paramref name="dOut"/> returns the same
            ''' infinite value.</remarks>
            Public Shared Sub DddMmSsToDeg(ByVal dIn As System.Int32,
                ByVal mIn As System.Int32, ByVal sIn As System.Double,
                ByRef dOut As System.Double)

                ' Input checking.
                If System.Double.IsInfinity(sIn) Then
                    dOut = sIn
                    Exit Sub
                End If

                Dim DPart As System.Int32 = dIn
                Dim MPart As System.Double = mIn / 60.0
                Dim SPart As System.Double = sIn / 3600.0
                If dIn < 0 Then
                    dOut = DPart - MPart - SPart
                ElseIf mIn < 0.0 Then
                    dOut = DPart + MPart + SPart
                Else
                    dOut = DPart + MPart + SPart
                End If

            End Sub ' DddMmSsToDeg

            ''' <summary>
            ''' Determines if <paramref name="dimension"/> refers to a value
            ''' defined in <see cref="D2.Angle.AngularDimension"/>.
            ''' </summary>
            ''' <param name="dimension">Specifies the <c>AngularDimension</c> to
            ''' be evaluated.</param>
            ''' <returns><c>True</c> if <paramref name="dimension"/> is defined
            ''' in <c>D2.Angle.AngularDimension</c>; otherwise,
            ''' <c>False</c>.</returns>
            ''' <remarks><see cref="D2.Angle.IsDefinedDimension(
            ''' D2.Angle.AngularDimension)"/> and
            ''' <see cref="D2.Angle.HasDefinedDimension()"/> are effectively the
            ''' same thing. Use whichever version best suits the variables at
            ''' hand.</remarks>
            Public Shared Function IsDefinedDimension(
                ByVal dimension As D2.Angle.AngularDimension) As System.Boolean

                '' This approach checks individually for each valid value.
                'Return dimension.Equals(
                '        D2.Angle.AngularDimension.Radian) OrElse
                '    dimension.Equals(
                '        D2.Angle.AngularDimension.Degree) OrElse
                '    dimension.Equals(
                '        D2.Angle.AngularDimension.Revolution) OrElse
                '    dimension.Equals(
                '        D2.Angle.AngularDimension.ArcMinute) OrElse
                '    dimension.Equals(
                '        D2.Angle.AngularDimension.ArcSecond) OrElse
                '    dimension.Equals(
                '        D2.Angle.AngularDimension.Gradian) OrElse
                '    dimension.Equals(
                '        D2.Angle.AngularDimension.Milliradian)

                ' As long as no non-automatic values are assigned, or the
                ' current ones rearranged, it can be done this way:
                Return dimension >= D2.Angle.AngularDimension.Radian AndAlso
                    dimension <= D2.Angle.AngularDimension.Milliradian

            End Function ' IsDefinedDimension

            ''' <summary>
            ''' Determines if the <see cref="D2.Angle.Dimension"/> property of
            ''' the current instance is defined in
            ''' <c>D2.Angle.AngularDimension</c>.
            ''' </summary>
            ''' <returns><c>True</c> if the <see cref="D2.Angle.Dimension"/>
            ''' property of the current instance is defined in
            ''' <c>D2.Angle.AngularDimension</c>; otherwise,
            ''' <c>False</c>.</returns>
            ''' <remarks><see cref="D2.Angle.IsDefinedDimension(
            ''' D2.Angle.AngularDimension)"/> and
            ''' <see cref="D2.Angle.HasDefinedDimension()"/> are effectively the
            ''' same thing. Use whichever version best suits the variables at
            ''' hand.</remarks>
            Public Function HasDefinedDimension() As System.Boolean
                Return D2.Angle.IsDefinedDimension(Me.Dimension)
            End Function ' HasDefinedDimension

            ''' <summary>
            ''' Determines if <paramref name="style"/> refers to a value
            ''' defined in <c>D2.Angle.NormalizationStyle</c>.
            ''' </summary>
            ''' <param name="style">Specifies the <c>NormalizationStyle</c> to
            ''' be evaluated.</param>
            ''' <returns><c>True</c> if <paramref name="style"/> is defined
            ''' in <c>D2.Angle.NormalizationStyle</c>; otherwise,
            ''' <c>False</c>.</returns>
            ''' <remarks><see cref="D2.Angle.IsDefinedStyle(
            ''' D2.Angle.NormalizationStyle)"/> and
            ''' <see cref="D2.Angle.HasDefinedStyle()"/> are effectively the
            ''' same thing. Use whichever version best suits the variables at
            ''' hand.</remarks>
            Public Shared Function IsDefinedStyle(
                ByVal style As D2.Angle.NormalizationStyle) As System.Boolean

                '' This approach checks individually for each valid value.
                'Return style.Equals(D2.Angle.NormalizationStyle.Half) OrElse
                '    style.Equals(D2.Angle.NormalizationStyle.Full)

                ' As long as no non-automatic values are assigned, or the
                ' current ones rearranged, it can be done this way:
                Return style >= D2.Angle.NormalizationStyle.Full AndAlso
                    style <= D2.Angle.NormalizationStyle.Half

            End Function ' IsDefinedStyle

            ''' <summary>
            ''' Determines if the <see cref="D2.Angle.Style"/> property of the
            ''' current instance refers to a value defined in
            ''' <c>D2.Angle.NormalizationStyle</c>.
            ''' </summary>
            ''' <returns> <c>True</c> if the <see cref="D2.Angle.Style"/>
            ''' property of the current instance refers to a value defined in
            ''' <c>D2.Angle.NormalizationStyle</c>; otherwise,
            ''' <c>False</c>.</returns>
            ''' <remarks><see cref="D2.Angle.IsDefinedStyle(
            ''' D2.Angle.NormalizationStyle)"/> and
            ''' <see cref="D2.Angle.HasDefinedStyle()"/> are effectively the
            ''' same thing. Use whichever version best suits the variables at
            ''' hand.</remarks>
            Public Function HasDefinedStyle() As System.Boolean
                Return D2.Angle.IsDefinedStyle(Me.Style)
            End Function ' HasDefinedStyle

            ''' <summary>
            ''' Converts the magnitude of an angle from one angular unit of
            ''' measure to another.
            ''' </summary>
            ''' <param name="origMag">Specifies the magnitude in the current
            ''' unit of measure.</param>
            ''' <param name="radiansPerUnitIn">Specifies the number of radians
            ''' per original unit of measure.</param>
            ''' <param name="unitsOutPerRadian">Specifies the number of
            ''' resulting units of measure per radian.</param>
            ''' <returns>The magnitude in the new dimension. Also returns
            ''' <c>System.Double.NaN</c> when any argument is
            ''' <c>System.Double.NaN</c>.</returns>
            ''' <remarks>
            ''' Use this when either, or both, of the units of measure does not
            ''' refer to a value defined in <see cref="D2.Angle.Dimension"/>.
            ''' Calculate the conversion values like this:
            ''' <br/>- When converting FROM degrees, <c>radiansPerUnitIn</c>
            ''' = 2*PI radians per 360 degrees = (2*PI)/360 = PI/180.
            ''' <br/>- When converting TO degrees, <c>unitsOutPerRadian</c>
            ''' = 360 degrees per 2*PI radians = 360/(2*PI) = 180/PI.
            ''' </remarks>
            Public Shared Function ScaleDimension(
                ByVal origMag As System.Double,
                ByVal radiansPerUnitIn As System.Double,
                ByVal unitsOutPerRadian As System.Double) As System.Double

                ' No input checking.
                Return origMag * radiansPerUnitIn * unitsOutPerRadian
                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' ScaleDimension

            ''' <summary>
            ''' Converts the magnitude of an angle from one
            ''' <see cref="D2.Angle.AngularDimension"/> to another.
            ''' </summary>
            ''' <param name="origMag">Specifies the magnitude in the current
            ''' unit of measure.</param>
            ''' <param name="dimensionIn">Specifies the current unit of
            ''' measure.</param>
            ''' <param name="dimensionOut">Specifies the outgoing unit of
            ''' measure.</param>
            ''' <returns>The magnitude in the new dimension, when both of the
            ''' units of measure refer to a value defined in
            ''' <see cref="D2.Angle.Dimension"/>; otherwise,
            ''' <c>System.Double.NaN</c>.</returns>
            ''' <remarks>
            ''' When either, or both, of the units of measure does not refer to
            ''' a value defined in <see cref="D2.Angle.Dimension"/>,
            ''' <c>System.Double.NaN</c> is returned. Use
            ''' <see cref="ScaleDimension(System.Double, System.Double,
            ''' System.Double)"/> for that situation.
            ''' </remarks>
            Public Shared Function ScaleDimension(
                ByVal origMag As System.Double,
                ByVal dimensionIn As D2.Angle.AngularDimension,
                ByVal dimensionOut As D2.Angle.AngularDimension) _
                As System.Double

                ' No input checking.
                ' Mismatches are detected during selection.

                If dimensionOut.Equals(dimensionIn) Then
                    ' Just copy it.
                    Return origMag ' Early exit.
                End If

                ' Set the scale for the incoming magnitude.
                Dim RadiansPerUnitIn As System.Double
                If dimensionIn.Equals(D2.Angle.AngularDimension.Radian) Then
                    RadiansPerUnitIn = 1.0
                ElseIf dimensionIn.Equals(D2.Angle.AngularDimension.Degree) Then
                    RadiansPerUnitIn = RADIANPERDEGREE
                ElseIf dimensionIn.Equals(
                    D2.Angle.AngularDimension.Revolution) Then

                    RadiansPerUnitIn = RADIANPERREVOLUTION
                ElseIf dimensionIn.Equals(
                    D2.Angle.AngularDimension.ArcMinute) Then

                    RadiansPerUnitIn = RADIANPERARCMINUTE
                ElseIf dimensionIn.Equals(
                    D2.Angle.AngularDimension.ArcSecond) Then

                    RadiansPerUnitIn = RADIANPERARCSECOND
                ElseIf dimensionIn.Equals(
                    D2.Angle.AngularDimension.Gradian) Then

                    RadiansPerUnitIn = RADIANPERGRADIAN
                ElseIf dimensionIn.Equals(AngularDimension.Milliradian) Then
                    RadiansPerUnitIn = RADIANPERMILLIRADIAN
                Else
                    ' No match.
                    Return System.Double.NaN
                End If

                ' Set the scale for the outgoing magnitude.
                Dim UnitsOutPerRadian As System.Double
                If dimensionOut.Equals(D2.Angle.AngularDimension.Radian) Then
                    UnitsOutPerRadian = 1.0
                ElseIf dimensionOut.Equals(
                    D2.Angle.AngularDimension.Degree) Then

                    UnitsOutPerRadian = DEGREEPERRADIAN
                ElseIf dimensionOut.Equals(
                    D2.Angle.AngularDimension.Revolution) Then

                    UnitsOutPerRadian = REVOLUTIONPERRADIAN
                ElseIf dimensionOut.Equals(
                    D2.Angle.AngularDimension.ArcMinute) Then

                    UnitsOutPerRadian = ARCMINUTEPERRADIAN
                ElseIf dimensionOut.Equals(
                    D2.Angle.AngularDimension.ArcSecond) Then

                    UnitsOutPerRadian = ARCSECONDPERRADIAN
                ElseIf dimensionOut.Equals(
                    D2.Angle.AngularDimension.Gradian) Then

                    UnitsOutPerRadian = GRADIANPERRADIAN
                ElseIf dimensionOut.Equals(
                    D2.Angle.AngularDimension.Milliradian) Then

                    UnitsOutPerRadian = MILLIRADIANPERRADIAN
                Else
                    ' No match.
                    Return System.Double.NaN
                End If

                'Dim AsRadians As System.Double = origMag * RadiansPerUnitIn
                'Dim Result As System.Double = AsRadians * UnitsOutPerRadian
                'Return Result
                Return origMag * RadiansPerUnitIn * UnitsOutPerRadian

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' ScaleDimension

            ''' <summary>
            ''' Returns an equivalent, of the current instance, angle having the
            ''' new dimension.
            ''' </summary>
            ''' <param name="newDimension">Specifies the resultant unit of
            ''' measure.</param>
            ''' <returns>An equivalent angle having the new dimension, when both
            ''' of the units of measure refer to a value defined in
            ''' <see cref="D2.Angle.Dimension"/>; otherwise,
            ''' <c>System.Double.NaN</c>.</returns>
            ''' <remarks>
            ''' When either, or both, of the units of measure does not refer to
            ''' a value defined in <see cref="D2.Angle.Dimension"/>, an angle
            ''' having <c>System.Double.NaN</c> as its magnitude is returned.
            ''' Use <see cref="ScaleDimension(System.Double, System.Double,
            ''' System.Double)"/> for that situation.
            ''' </remarks>
            Public Function ScaleDimension(
                ByVal newDimension As D2.Angle.AngularDimension) As D2.Angle

                ' Input checking.
                If Not (Me.HasDefinedDimension() AndAlso
                    D2.Angle.IsDefinedDimension(newDimension)) Then

                    Return New D2.Angle(System.Double.NaN, newDimension, Me.Style)
                End If

                Dim NewMag As System.Double = D2.Angle.ScaleDimension(
                    Me.Magnitude, Me.Dimension, newDimension)
                Return New D2.Angle(NewMag, newDimension, Me.Style)

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' ScaleDimension

            ''' <summary>
            ''' Returns the full-scale range size for the specified
            ''' <paramref name="dimension"/>.
            ''' </summary>
            ''' <param name="dimension">Specifies the
            ''' <see cref="D2.Angle.AngularDimension"/> to examine.</param>
            ''' <returns>The full-scale range size.</returns>
            ''' <exception cref="System.ArgumentOutOfRangeException">When
            ''' xxxxxxxxxxxxxxxxxxx
            ''' <paramref name="dimension"/> specifies an
            ''' <see cref="D2.Angle.AngularDimension"/> that is not defined in
            ''' <see cref="D2.Angle.AngularDimension"/>.</exception>
            ''' <remarks>
            ''' This returns the size of the full range without regard to
            ''' whether it is marked
            ''' <see cref="D2.Angle.NormalizationStyle.Half"/> or
            ''' <see cref="D2.Angle.NormalizationStyle.Full"/>, not the
            ''' range-limited maximum magnitude. For example, -180 to 180 and
            ''' 0 to 360 degree ranges both have a range size of 360.
            ''' <br/>
            ''' When <paramref name="dimension"/> does not refer to
            ''' a value defined in <see cref="D2.Angle.Dimension"/>,
            ''' <c>System.Double.NaN</c> is returned.
            ''' </remarks>
            Public Shared Function GetFullDimensionSize(
                ByVal dimension As D2.Angle.AngularDimension) As System.Double

                ' No input checking.

                If dimension.Equals(D2.Angle.AngularDimension.Radian) Then
                    Return D2.RADIANPERCIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Degree) Then
                    Return D2.DEGREEPERCIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Revolution) Then
                    Return D2.REVOLUTIONPERCIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.ArcMinute) Then
                    Return D2.ARCMINUTEPERCIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.ArcSecond) Then
                    Return D2.ARCSECONDPERCIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Gradian) Then
                    Return D2.GRADIANPERCIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Milliradian) Then
                    Return D2.MILLIRADIANPERCIRCLE
                Else
                    ' No match.
                    Return System.Double.NaN
                End If

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' GetFullDimensionSize

            ''' <summary>
            ''' Returns the full-scale range size for the current instance.
            ''' </summary>
            ''' <returns>The full-scale range size.</returns>
            ''' <exception cref="System.ArgumentOutOfRangeException">When the
            ''' current instance has a <see cref="D2.Angle.Dimension"/> property
            ''' that is not defined in
            ''' <see cref="D2.Angle.AngularDimension"/>.</exception>
            ''' <remarks>
            ''' This returns the size of the full range without regard to
            ''' whether it is marked
            ''' <see cref="D2.Angle.NormalizationStyle.Half"/> or
            ''' <see cref="D2.Angle.NormalizationStyle.Full"/>, not the
            ''' range-limited maximum magnitude. For example, -180 to 180 and
            ''' 0 to 360 degree ranges both have a range size of 360.
            ''' </remarks>
            Public Function GetFullDimensionSize() As System.Double
                ' No input checking.
                Return Angle.GetFullDimensionSize(Me.Dimension)
                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' GetFullDimensionSize

            ''' <summary>
            ''' Returns the half-scale range size for the specified
            ''' <paramref name="dimension"/>.
            ''' </summary>
            ''' <param name="dimension">Specifies the
            ''' <see cref="D2.Angle.AngularDimension"/> to examine.</param>
            ''' <returns>The half-scale range size.</returns>
            ''' <exception cref="System.ArgumentOutOfRangeException">When
            ''' <paramref name="dimension"/> specifies an
            ''' <see cref="D2.Angle.AngularDimension"/> that is not defined in
            ''' <see cref="D2.Angle.AngularDimension"/>.</exception>
            ''' <remarks>
            ''' This returns the size of the half range without regard to
            ''' whether it is marked
            ''' <see cref="D2.Angle.NormalizationStyle.Half"/> or
            ''' <see cref="D2.Angle.NormalizationStyle.Full"/>, not the
            ''' range-limited maximum magnitude. For example, -180 to 180 and
            ''' 0 to 360 degree ranges both have a range size of 360.
            ''' <br/>
            ''' When <paramref name="dimension"/> does not refer to
            ''' a value defined in <see cref="D2.Angle.Dimension"/>,
            ''' <c>System.Double.NaN</c> is returned.
            ''' </remarks>
            Public Shared Function GetHalfDimensionSize(
                ByVal dimension As D2.Angle.AngularDimension) As System.Double

                ' No input checking.

                If dimension.Equals(D2.Angle.AngularDimension.Radian) Then
                    Return D2.RADIANPERSEMICIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Degree) Then
                    Return D2.DEGREEPERSEMICIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Revolution) Then
                    Return D2.REVOLUTIONPERSEMICIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.ArcMinute) Then
                    Return D2.ARCMINUTEPERSEMICIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.ArcSecond) Then
                    Return D2.ARCSECONDPERSEMICIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Gradian) Then
                    Return D2.GRADIANPERSEMICIRCLE
                ElseIf dimension.Equals(D2.Angle.AngularDimension.Milliradian) Then
                    Return D2.MILLIRADIANPERSEMICIRCLE
                Else
                    ' No match.
                    Return System.Double.NaN
                End If

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' GetHalfDimensionSize

            ''' <summary>
            ''' Returns the half-scale range size for the current instance.
            ''' </summary>
            ''' <returns>The half-scale range size.</returns>
            ''' <exception cref="System.ArgumentOutOfRangeException">When the
            ''' current instance has a <see cref="D2.Angle.Dimension"/> property
            ''' xxxxxxxxxxxxxxxx
            ''' that is not defined in
            ''' <see cref="D2.Angle.AngularDimension"/>.</exception>
            ''' <remarks>
            ''' This returns the size of the half range without regard to
            ''' whether it is marked
            ''' <see cref="D2.Angle.NormalizationStyle.Half"/> or
            ''' <see cref="D2.Angle.NormalizationStyle.Full"/>, not the
            ''' range-limited maximum magnitude. For example, -180 to 180 and
            ''' 0 to 360 degree ranges both have a range size of 360.
            ''' </remarks>
            Public Function GetHalfDimensionSize() As System.Double
                ' No input checking.
                Return Angle.GetHalfDimensionSize(Me.Dimension)
                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' GetHalfDimensionSize

            ''' <summary>
            ''' Determines if the <see cref="D2.Angle.Magnitude"/> property of
            ''' the current instance is in the normalized range for its
            ''' <see cref="D2.Angle.Dimension"/> and
            ''' <see cref="D2.Angle.Style"/> properties.
            ''' </summary>
            ''' <returns><c>True</c> if <see cref="D2.Angle.Magnitude"/>
            ''' property is in the normalized range; otherwise, <c>False</c>.
            ''' Also returns <c>False</c> when the current instance has an
            ''' undefined <see cref="D2.Angle.Dimension"/> or
            ''' <see cref="D2.Angle.Style"/> property.</returns>
            Public Function IsNormalized() As System.Boolean

                ' Input checking.
                If Not (Me.HasDefinedDimension() AndAlso Me.HasDefinedStyle()) Then
                    Return False
                End If

                Dim Mag As System.Double = Me.Magnitude
                Dim FullScaleSize As System.Double =
                    Me.GetFullDimensionSize()
                If Me.Style.Equals(D2.Angle.NormalizationStyle.Full) Then
                    Return (Mag >= 0.0) AndAlso (Mag < FullScaleSize)
                Else
                    ' NormalizationStyle.Half.
                    Dim HalfLimit As System.Double = FullScaleSize / 2.0
                    Return (Mag > -HalfLimit) AndAlso (Mag <= HalfLimit)
                End If

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' IsNormalized

            ''' <summary>
            ''' Returns a normalized value for the
            ''' <see cref="D2.Angle.Magnitude"/> property of the current
            ''' instance that conforms to the <see cref="D2.Angle.Style"/>
            ''' property.
            ''' 
            ''' xxxxxxxxxxxxxxxxxx
            ''' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
            ''' INCLUDING THE DEFAULT ANGLE CASE.
            ''' xxxxxxxxxxxxxxxxxx
            ''' 
            ''' </summary>
            ''' <returns> The normalized magnitude, if the
            ''' <see cref="D2.Angle.Style"/> property of the current instance is
            ''' defined in <see cref="D2.Angle.NormalizationStyle"/>; otherwise,
            ''' <c>System.Double.NaN</c>.
            ''' 
            ''' xxxxxxxxxxxxxxxxxx
            ''' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
            ''' INCLUDING THE DEFAULT ANGLE CASE.
            ''' xxxxxxxxxxxxxxxxxx
            ''' 
            ''' </returns>
            Public Function GetNormalizedMagnitude() As System.Double

                ' Input checking.
                If Not Me.HasDefinedStyle Then
                    ' Not in defined enum.
                    'Dim CaughtBy As System.Reflection.MethodBase =
                    '    System.Reflection.MethodBase.GetCurrentMethod
                    'Throw New System.ArgumentOutOfRangeException(
                    '    $"Invalid {NameOf(Style)} in " &
                    '    $"{NameOf(GetNormalizedMagnitude)}.")
                    Return System.Double.NaN
                End If

                If Double.IsInfinity(Me.Magnitude) Then
                    Return Me.Magnitude ' Early exit.
                End If

                If Me.IsNormalized() Then
                    ' Do not expose this to floating point limitations.
                    Return Me.Magnitude ' Early exit.
                End If

                ' Normalize the magnitude in the current range.
                ' Truncate rounds IntegerPart to the nearest integer toward
                ' zero, keeping the sign intact.
                Dim StyleFullSize As System.Double = Me.GetFullDimensionSize
                Dim Denom As System.Double = Me.GetFullDimensionSize
                Dim CurrMag As System.Double = Me.Magnitude
                Dim IntegerPart As System.Double =
                    System.Math.Truncate(CurrMag / Denom)
                Dim FractionalPart As System.Double =
                    CurrMag - (IntegerPart * Denom)
                If Me.Style.Equals(D2.Angle.NormalizationStyle.Full) Then
                    If FractionalPart < 0.0 Then
                        Return StyleFullSize + FractionalPart
                    Else
                        ' >= 0
                        Return FractionalPart
                    End If
                Else
                    ' Process as half-range.
                    Dim StyleHalfSize As System.Double = Me.GetHalfDimensionSize
                    If System.Math.Abs(FractionalPart) < StyleHalfSize Then
                        Return FractionalPart
                    ElseIf System.Math.Abs(FractionalPart) > StyleHalfSize Then
                        ' More than a half revolution.
                        If FractionalPart > 0.0 Then
                            Return -(StyleFullSize - FractionalPart)
                        ElseIf FractionalPart < 0.0 Then
                            Return FractionalPart + StyleFullSize
                        Else
                            Return StyleHalfSize - FractionalPart
                        End If
                    Else
                        ' Exactly a half revolution.
                        Return StyleHalfSize
                    End If
                End If

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' GetNormalizedMagnitude

            ''' <summary>
            ''' Returns a normalized <c>D2.Angle</c> based on the specified <paramref name="magnitude"/>, <paramref name="dimension"/>, and <paramref name="style"/>.
            ''' </summary>
            ''' <param name="magnitude">Specifies the scalar value of the magnitude of the angle.</param>
            ''' <param name="dimension">Specifies the <see cref="D2.Angle.AngularDimension"/> of the angle.</param>
            ''' <param name="style">Specifies the <see cref="D2.Angle.NormalizationStyle"/> of the angle.</param>
            ''' 
            ''' 
            ''' 
            ''' 
            ''' <returns>
            ''' Returns a normalized <c>D2.Angle</c> based on the specified <paramref name="magnitude"/>,
            ''' <paramref name="dimension"/>, and <paramref name="style"/>.
            ''' Also returns the default angle when
            ''' <paramref name="dimension"/> is not defined in <see cref="D2.Angle.AngularDimension"/>, or
            ''' <paramref name="style"/> is not defined in <see cref="D2.Angle.NormalizationStyle"/>.
            ''' Also returns an <c>Angle</c> with an infinite <c>magnitude</c> when
            ''' <paramref name="magnitude"/> is infinite.
            ''' 
            ''' xxxxxxxxxxxxxxxxxx
            ''' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
            ''' INCLUDING THE DEFAULT ANGLE CASE.
            ''' xxxxxxxxxxxxxxxxxx
            ''' 
            ''' </returns>
            ''' 
            ''' 
            ''' 
            ''' 
            ''' 
            ''' 
            ''' 
            ''' <exception cref="System.ArgumentOutOfRangeException">xxxxxxxxxxxxxxxxWhen
            ''' 
            ''' xxxxxxxxxxxxxxxxxx
            ''' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
            ''' INCLUDING THE DEFAULT ANGLE CASE.
            ''' xxxxxxxxxxxxxxxxxx
            ''' 
            ''' <paramref name="magnitude"/> is <c>NaN</c> or infinite,
            ''' <paramref name="dimension"/> is not defined in
            ''' <see cref="D2.Angle.AngularDimension"/>, or
            ''' <paramref name="style"/> is not defined in
            ''' <see cref="D2.Angle.NormalizationStyle"/>xxxxxxxxxxxx.</exception>
            ''' <remarks>
            ''' 
            ''' 
            ''' 
            ''' Assignment of values not defined in
            ''' <see cref="D2.Angle.NormalizationStyle"/> is allowed, but may
            ''' cause unexpected results. Calling routines might need to either
            ''' verify values prior to calling <see cref="New(System.Double,
            ''' D2.Angle.AngularDimension, D2.Angle.NormalizationStyle)"/> or
            ''' use special handling, after the call, where those values are
            ''' valid.
            ''' 
            ''' 
            ''' xxxxxxxxxxxxxxxxxx
            ''' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
            ''' INCLUDING THE DEFAULT ANGLE CASE.
            ''' xxxxxxxxxxxxxxxxxx
            ''' first check is for undefined style.
            ''' 
            ''' 
            ''' 
            ''' </remarks>
            Public Shared Function CreateNormalizedAngle(ByVal magnitude As System.Double,
                ByVal dimension As D2.Angle.AngularDimension,
                ByVal style As D2.Angle.NormalizationStyle) As D2.Angle

                '' Suspend to avoid exceptions:
                '' Input checking.
                'If Not D2.Angle.IsDefinedStyle(style) Then
                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Unknown {NameOf(style)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If
                'If System.Double.IsNaN(magnitude) OrElse
                '    System.Double.IsInfinity(magnitude) Then

                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Invalid {NameOf(magnitude)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If
                'If Not D2.Angle.IsDefinedDimension(dimension) Then
                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Unknown {NameOf(dimension)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If

                Dim A As New D2.Angle(magnitude, dimension, style)
                Dim NewM As System.Double = A.GetNormalizedMagnitude()
                Return New D2.Angle(NewM, dimension, style)

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' CreateNormalizedAngle

            ''' <summary>
            ''' Returns the normalized angle that is the result of rotating the
            ''' specified <paramref name="angle"/> by the specified angle of
            ''' <paramref name="rotation"/>.
            ''' </summary>
            ''' <param name="angle">Specifies the angle to be rotated.</param>
            ''' <param name="rotation">Specifies the angle, in the same
            ''' <see cref="D2.Angle.AngularDimension"/> as
            ''' <paramref name="angle"/>, by which to rotate.</param>
            ''' <returns>The normalized angle that is the result of the
            ''' rotation.</returns>
            ''' <remarks>
            ''' xxxxxxxxxxxxxxxx
            ''' Out-of-range values of both <paramref name="angle"/> and
            ''' <paramref name="rotation"/> are accepted.
            ''' xxxxxxxxxxxxxxxx
            ''' </remarks>
            Public Shared Function GetNormalizedRotatedAngle(
                ByVal angle As D2.Angle, ByVal rotation As System.Double) _
                As D2.Angle

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                '' Suspend to avoid exceptions:
                '' Input checking.
                'If System.Double.IsInfinity(angle) OrElse
                '    System.Double.IsInfinity(rotation) Then

                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '    $"Arguments to {NameOf(RotateNormalRad)} {MSGCHIV}")
                'End If

                ' Calculate and normalize the resulting angle.
                Return D2.Angle.CreateNormalizedAngle(
                    angle.Magnitude + rotation, angle.Dimension, angle.Style)

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Function ' GetNormalizedRotatedAngle

#End Region ' "Methods"

#Region "Constructors"

            ''' <summary>
            ''' A default constructor that creates a new instance of the
            ''' <c>Angle</c> class with the default <c>Magnitude</c>,
            ''' <c>Dimension</c>, and <c>Style</c>.
            ''' </summary>
            Public Sub New()

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS xxxxxxxxxx

                ' A default constructor is required to allow inheritance.
                With Me
                    Me.m_Magnitude = D2.Angle.DFLTMAGNITUDE
                    Me.m_Dimension = D2.Angle.DFLTDIMENSION
                    Me.m_Style = D2.Angle.DFLTSTYLE
                End With
                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 
            End Sub ' New

            ''' <summary>
            ''' Creates a new instance of the <c>2D.Angle</c> class with the
            ''' specified <paramref name="magnitude"/>,
            ''' <paramref name="dimension"/>, and <paramref name="style"/>.
            ''' </summary>
            ''' <param name="magnitude">Specifies the scalar value of the
            ''' magnitude of the angle.</param>
            ''' <param name="dimension">Specifies the
            ''' <see cref="D2.Angle.AngularDimension"/> of the angle.</param>
            ''' <param name="style">Specifies the
            ''' <see cref="D2.Angle.NormalizationStyle"/> of the angle.</param>
            ''' <exception cref="System.ArgumentOutOfRangeException">When
            ''' <paramref name="magnitude"/> is <c>NaN</c> or infinite,
            ''' <paramref name="dimension"/> is invalid, or
            ''' <paramref name="style"/> is invalid.</exception>
            Public Sub New(ByVal magnitude As System.Double,
                           ByVal dimension As D2.Angle.AngularDimension,
                           ByVal style As D2.Angle.NormalizationStyle)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                '' Suspend to avoid exceptions:
                '' Input checking.
                'If System.Double.IsNaN(magnitude) OrElse
                '    System.Double.IsInfinity(magnitude) Then

                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Invalid {NameOf(magnitude)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If
                'If Not D2.Angle.IsDefinedDimension(dimension) Then
                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Invalid {NameOf(dimension)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If
                'If Not D2.Angle.IsValidStyle(style) Then
                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Invalid {NameOf(style)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If

                With Me
                    .m_Magnitude = magnitude
                    .m_Dimension = dimension
                    .m_Style = style
                End With

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 

            End Sub ' New

            ''' <summary>
            ''' Creates a new instance of the <c>2D.Angle</c> class with the
            ''' specified <paramref name="magnitude"/> and
            ''' <paramref name="dimension"/>, and the default
            ''' <see cref="D2.Angle.NormalizationStyle"/>"/>.
            ''' </summary>
            ''' <param name="magnitude">Specifies the scalar value of the
            ''' magnitude of the angle.</param>
            ''' <param name="dimension">Specifies the
            ''' <see cref="D2.Angle.AngularDimension"/> of the angle.</param>
            ''' <exception cref="System.ArgumentOutOfRangeException">When
            ''' <paramref name="magnitude"/> is <c>NaN</c> or infinite, or when
            ''' <paramref name="dimension"/> is invalid.</exception>
            Public Sub New(ByVal magnitude As System.Double,
                           ByVal dimension As D2.Angle.AngularDimension)

                ' xxxxxxxxxx IS INPUT CHECKING NEEDED? xxxxxxxxxx
                ' xxxxxxxxxx ARE THERE ANY EXCEPTIONS? xxxxxxxxxx
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS? xxxxxxxxxx

                '' Suspend to avoid exceptions:
                '' Input checking.
                'If System.Double.IsNaN(magnitude) OrElse
                '    System.Double.IsInfinity(magnitude) Then

                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Invalid {NameOf(magnitude)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If
                'If Not D2.Angle.IsDefinedDimension(dimension) Then
                '    'Dim CaughtBy As System.Reflection.MethodBase =
                '    '    System.Reflection.MethodBase.GetCurrentMethod
                '    Throw New System.ArgumentOutOfRangeException(
                '        $"Invalid {NameOf(dimension)} in " &
                '        $"{NameOf(CreateNormalizedAngle)}.")
                'End If

                With Me
                    .m_Magnitude = magnitude
                    .m_Dimension = dimension
                    .m_Style = D2.Angle.DFLTSTYLE
                End With

                '' 
                '' xxxxxxxxxxxxxxxxxx
                '' GO CHECK THAT THE TESTING MATCHES THIS, AND THAT THE TESTING COVERS ALL OF THESE CASES,
                '' INCLUDING THE DEFAULT ANGLE CASE.
                '' xxxxxxxxxxxxxxxxxx
                '' 

            End Sub ' New

#End Region ' "Constructors"

        End Class ' Angle

    End Structure ' D2

End Module ' Math
