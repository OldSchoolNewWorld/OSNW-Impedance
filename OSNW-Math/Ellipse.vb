Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Partial Public Module Math

    Partial Public Structure D2

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        Public Class Ellipse

            ' These REFs might help.
            ' https://math.libretexts.org/Bookshelves/Algebra/College_Algebra_1e_(OpenStax)/08%3A_Analytic_Geometry/8.02%3A_The_Ellipse
            ' https://math.libretexts.org/Bookshelves/Calculus/Elementary_Calculus_2e_(Corral)/07%3A_Analytic_Geometry_and_Plane_Curves/7.04%3A_Translations_and_Rotations
            ' https://www.math.drexel.edu/~tolya/rotated%20ellipse.pdf
            ' https://www.emathhelp.net/calculators/algebra-2/ellipse-calculator/?type=d&f=&cx=3&cy=6&f1x=&f1y=&f2x=&f2y=&v1x=&v1y=&v2x=&v2y=&cv1x=&cv1y=&cv2x=&cv2y=&ma=8&mi=4&e=&a=&d1=&d2=&p1x=&p1y=&p2x=&p2y=&p3x=&p3y=&p4x=&p4y=

            Private Const MSGHNBS As System.String = " has not been set."

#Region "Persistent Properties"

            ' These are properties whose value does not change with rotation.

#Region "Persistent Assigned Properties"

            ' These properties are read-only and set by New(). Only these
            ' properties should be included in serialization, with the other
            ' properties being derived from them in New().

            ' Width Property.
            Private m_Width As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Width As System.Double
                Get
                    Return Me.m_Width
                End Get
                Private Set
                    Me.m_Width = Value
                End Set
            End Property

            ' Height Property.
            Private m_Height As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Height As System.Double
                Get
                    Return Me.m_Height
                End Get
                Private Set
                    Me.m_Height = Value
                End Set
            End Property

            ' Center Property.
            Private m_Center As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Center As Math.D2.Point
                Get
                    Return Me.m_Center
                End Get
                Private Set
                    Me.m_Center = Value
                End Set
            End Property

            ' Rotation Property.
            Private m_Rotation As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' Angle in RADIANS.
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            Public Property Rotation As System.Double
                Get
                    Return Me.m_Rotation
                End Get
                Private Set
                    Me.m_Rotation = Value
                End Set
            End Property

#End Region ' "Persistent Assigned Properties"

#Region "Persistent Derived Properties"

            ' These properties should be excluded from serialization, with their
            ' values being derived in New().

            ' MajorLen Property.
            Private m_MajorLen As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx when the value has been set, or
            ''' <c>System.Double</c> when the value has not been set.</returns>
            Public ReadOnly Property MajorLen As System.Double
                Get
                    Return Me.m_MajorLen
                End Get
            End Property

            ' MinorLen Property.
            Private m_MinorLen As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx when the value has been set, or
            ''' <c>System.Double</c> when the value has not been set.</returns>
            Public ReadOnly Property MinorLen As System.Double
                Get
                    Return Me.m_MinorLen
                End Get
            End Property

            ' StdA Property.
            Private m_StdA As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx when the value has been set, or
            ''' <c>System.Double</c> when the value has not been set.</returns>
            Public ReadOnly Property StdA As System.Double
                Get
                    Return Me.m_StdA
                End Get
            End Property

            ' StdB Property.
            Private m_StdB As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx when the value has been set, or
            ''' <c>System.Double</c> when the value has not been set.</returns>
            Public ReadOnly Property StdB As System.Double
                Get
                    Return Me.m_StdB
                End Get
            End Property

            ' StdC Property.
            Private m_StdC As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx when the value has been set, or
            ''' <c>System.Double</c> when the value has not been set.</returns>
            Public ReadOnly Property StdC As System.Double
                Get
                    Return Me.m_StdC
                End Get
            End Property

            ' Eccentricity Property.
            Private m_Eccentricity As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property Eccentricity As System.Double
                Get
                    Return Me.m_Eccentricity
                End Get
            End Property

            ' PathLen Property.
            Private m_PathLen As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property PathLen As System.Double
                Get
                    Return Me.m_PathLen
                End Get
            End Property

#End Region ' "Persistent Derived Properties"

#End Region ' "Persistent Properties"

#Region "Rotatable Properties"

            ' These are properties whose value changes with rotation.

#Region "Foci"

            ' These properties should be excluded from serialization, with their
            ' values being derived in New().
            ' The "r_" prefix denotes rotatable values used in the initial calculations.

            ' Focus0 Property.
            Private m_Focus0 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property Focus0 As Math.D2.Point
                Get
                    Return Me.m_Focus0
                End Get
            End Property

            ' Focus1 Property.
            Private m_Focus1 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property Focus1 As Math.D2.Point
                Get
                    Return Me.m_Focus1
                End Get
            End Property

            '' FociDist Property.
            'Private FociDistSet As System.Boolean
            'Private m_FociDist As System.Double
            '''' <summary>
            '''' xxxxxxxxxx
            '''' </summary>
            '''' <returns>xxxxxxxxxx</returns>
            '''' <exception cref="System.InvalidOperationException">When the value has
            '''' not been set.</exception>
            'Public ReadOnly Property FociDist As System.Double
            '    Get
            '        If Not Me.FociDistSet Then
            '            'Dim CaughtBy As System.Reflection.MethodBase =
            '            '    System.Reflection.MethodBase.GetCurrentMethod
            '            Throw New System.InvalidOperationException(
            '                $"{NameOf(FociDist)} {MSGHNBS}")
            '        End If
            '        Return Me.m_FociDist
            '    End Get
            'End Property

#End Region '  "Foci"

#Region "Major Axis"

            ' These properties should be excluded from serialization, with their
            ' values being derived in New().

            ' MajorVertex0 Property.
            Private m_MajorVertex0 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MajorVertex0 As Math.D2.Point
                Get
                    Return Me.m_MajorVertex0
                End Get
            End Property

            ' MajorVertex1 Property.
            Private m_MajorVertex1 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MajorVertex1 As Math.D2.Point
                Get
                    Return Me.m_MajorVertex1
                End Get
            End Property

            ' MajorM Property.
            Private m_MajorM As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MajorM As System.Double
                Get
                    Return Me.m_MajorM
                End Get
            End Property

            ' MajorAngleR Property.
            Private m_MajorAngleR As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MajorAngleR As System.Double
                Get
                    Return Me.m_MajorAngleR
                End Get
            End Property

            ' MajorAngleD Property.
            Private m_MajorAngleD As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MajorAngleD As System.Double
                Get
                    Return Me.m_MajorAngleD
                End Get
            End Property

            ' MajorB Property.
            Private m_MajorB As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MajorB As System.Double
                Get
                    Return Me.m_MajorB
                End Get
            End Property

#End Region ' "Major Axis"

#Region "Minor Axis"

            ' These properties should be excluded from serialization, with their
            ' values being derived in New().

            ' MinorVertex0 Property.
            Private m_MinorVertex0 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MinorVertex0 As Math.D2.Point
                Get
                    Return Me.m_MinorVertex0
                End Get
            End Property

            ' MinorVertex1 Property.
            Private m_MinorVertex1 As Math.D2.Point
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MinorVertex1 As Math.D2.Point
                Get
                    Return Me.m_MinorVertex1
                End Get
            End Property

            ' MinorM Property.
            Private m_MinorM As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MinorM As System.Double
                Get
                    Return Me.m_MinorM
                End Get
            End Property

            ' MinorAngleR Property.
            Private m_MinorAngleR As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MinorAngleR As System.Double
                Get
                    Return Me.m_MinorAngleR
                End Get
            End Property

            ' MinorAngleD Property.
            Private m_MinorAngleD As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MinorAngleD As System.Double
                Get
                    Return Me.m_MinorAngleD
                End Get
            End Property

            ' MinorB Property.
            Private m_MinorB As System.Double
            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <returns>xxxxxxxxxx</returns>
            ''' <exception cref="System.InvalidOperationException">When the value has
            ''' not been set.</exception>
            Public ReadOnly Property MinorB As System.Double
                Get
                    Return Me.m_MinorB
                End Get
            End Property

#End Region ' "Minor Axis"

#End Region ' "Rotatable Properties"

#Region "Methods"

            Private Sub PushFromNew()

                ' Local rotatable values for use in the initial calculations.
                Dim r_Focus0 As Math.D2.Point
                Dim r_Focus1 As Math.D2.Point
                Dim r_MajorVertex0 As Math.D2.Point
                Dim r_MajorVertex1 As Math.D2.Point
                Dim r_MinorVertex0 As Math.D2.Point
                Dim r_MinorVertex1 As Math.D2.Point

                With Me

                    ' REF: Ellipse
                    ' https://en.wikipedia.org/wiki/Ellipse

                    ' REF: LibreText Mathematics 8.2: The Ellipse
                    ' https://math.libretexts.org/Bookshelves/Algebra/College_Algebra_1e_(OpenStax)/08%3A_Analytic_Geometry/8.02%3A_The_Ellipse

                    ' REF: Ellipse Calculator
                    ' Use to compare results.
                    ' https://www.emathhelp.net/calculators/algebra-2/ellipse-calculator/

                    ' Start work using an ellipse with no rotation.

                    ' Identify the axes.
                    If Me.Height > Me.Width Then
                        Me.m_MajorLen = Me.Height
                        Me.m_MinorLen = Me.Width
                    Else
                        Me.m_MajorLen = Me.Width
                        Me.m_MinorLen = Me.Height
                    End If

                    ' Set values for the standard form (X^2/A^2 + Y^2/B^2 = 1)
                    ' equation.
                    ' The semi-major and semi-minor axes.
                    Me.m_StdA = Me.MajorLen / 2.0
                    Dim SqrStdA As System.Double = Me.StdA * Me.StdA
                    Me.m_StdB = Me.MinorLen / 2.0
                    Dim SqrStdB As System.Double = Me.StdB * Me.StdB
                    ' The distance from center to a focus.
                    ' The foci are located +/-StdC from the center point.
                    Me.m_StdC = System.Math.Sqrt(SqrStdA - SqrStdB)
                    '                Dim SqrStdC As System.Double = StdC * StdC

                    ' Calculate Eccentricity.
                    Me.m_Eccentricity = System.Math.Sqrt(1 - (SqrStdB / SqrStdA))

                    ' The foci and vertices.

                    If Me.Height > Me.Width Then
                        r_Focus0 =
                        New Math.D2.Point(Me.Center.X, Me.Center.Y + StdC)
                        r_Focus1 =
                        New Math.D2.Point(Me.Center.X, Me.Center.Y - StdC)
                        r_MajorVertex0 =
                        New Math.D2.Point(Me.Center.X, Me.Center.Y + StdA)
                        r_MajorVertex1 =
                        New Math.D2.Point(Me.Center.X, Me.Center.Y - StdA)
                        r_MinorVertex0 =
                        New Math.D2.Point(Me.Center.X - StdB, Me.Center.Y)
                        r_MinorVertex1 =
                        New Math.D2.Point(Me.Center.X + StdB, Me.Center.Y)
                    Else
                        r_Focus0 =
                        New Math.D2.Point(Me.Center.X - StdC, Me.Center.Y)
                        r_Focus1 =
                        New Math.D2.Point(Me.Center.X + StdC, Me.Center.Y)
                        r_MajorVertex0 =
                        New Math.D2.Point(Me.Center.X - StdA, Me.Center.Y)
                        r_MajorVertex1 =
                        New Math.D2.Point(Me.Center.X + StdA, Me.Center.Y)
                        r_MinorVertex0 =
                        New Math.D2.Point(Me.Center.X, Me.Center.Y + StdB)
                        r_MinorVertex1 =
                        New Math.D2.Point(Me.Center.X, Me.Center.Y - StdB)
                    End If

                    ' COMING LATER
                    '            ' Set values for the slope-intercept form (Y = mX + b) equation for the
                    '            ' major and minor axes. Also determine the angles from the slopes.
                    '                ' THESE ARE STILL IN NON-ROTATED STATE.

                    ' XXX Progress check. XXX
                    Dim PathLenMajor0 As System.Double =
                        OSNW.Math.D2.Point.Distance(r_Focus0, r_MajorVertex0) _
                        + OSNW.Math.D2.Point.Distance(r_Focus1, r_MajorVertex0)
                    Dim PathLenMajor1 As System.Double =
                        OSNW.Math.D2.Point.Distance(r_Focus0, r_MajorVertex1) _
                        + OSNW.Math.D2.Point.Distance(r_Focus1, r_MajorVertex1)
                    Dim PathLenMinor0 As System.Double =
                    OSNW.Math.D2.Point.Distance(r_Focus0, r_MinorVertex0) _
                    + OSNW.Math.D2.Point.Distance(r_Focus1, r_MinorVertex0)
                    Dim PathLenMinor1 As System.Double =
                        OSNW.Math.D2.Point.Distance(r_Focus0, r_MinorVertex1) _
                        + OSNW.Math.D2.Point.Distance(r_Focus1, r_MinorVertex1)

                    m_PathLen =
                        OSNW.Math.D2.Point.Distance(r_Focus0, r_MajorVertex0) _
                        + OSNW.Math.D2.Point.Distance(r_Focus1, r_MajorVertex0)

                    ' Now there is enough info to start using rotated values.

                    ' Calculate the foci and vertices.
                    Dim Cx As System.Double = Me.Center.X
                    Dim Cy As System.Double = Me.Center.Y
                    Me.m_Focus0 = r_Focus0.RotatedAround(Me.Rotation, Cx, Cy)
                    Me.m_Focus1 = r_Focus1.RotatedAround(Me.Rotation, Cx, Cy)
                    Me.m_MajorVertex0 =
                    r_MajorVertex0.RotatedAround(Me.Rotation, Cx, Cy)
                    Me.m_MajorVertex1 =
                    r_MajorVertex1.RotatedAround(Me.Rotation, Cx, Cy)
                    Me.m_MinorVertex0 =
                    r_MinorVertex0.RotatedAround(Me.Rotation, Cx, Cy)
                    Me.m_MinorVertex1 =
                    r_MinorVertex1.RotatedAround(Me.Rotation, Cx, Cy)

                    ' Calculate the slopes.
                    Me.m_MajorM = OSNW.Math.D2.Line.GetSlopeFromTwoPoints(
                        Me.Focus0.X, Me.Focus0.Y, Me.Focus1.X, Me.Focus1.Y)
                    Me.m_MinorM = -1.0 / Me.MajorM

                    ' Calculate the intercepts.
                    ' Y = m*X + b; Slope-intercept form of a line.
                    ' b = Y - m*X; Solve for the Y-intercept.
                    ' Base these on the center point.
                    Me.m_MajorB = Me.Center.Y - Me.MajorM * Me.Center.X
                    Me.m_MinorB = Me.Center.Y - Me.MinorM * Me.Center.X

                    ' Set the angles.
                    Me.m_MajorAngleR = System.Double.Atan(Me.MajorM)
                    Me.m_MajorAngleD = System.Double.RadiansToDegrees(Me.MajorAngleR)
                    Me.m_MinorAngleR = System.Double.Atan(Me.MinorM)
                    Me.m_MinorAngleD = System.Double.RadiansToDegrees(Me.MinorAngleR)

                End With

            End Sub ' PushFromNew

#End Region ' "Methods"

#Region "Constructors"

            ''' <summary>
            ''' xxxxxxxxxx
            ''' Default contructor.
            ''' xxxxxxxxxx
            ''' </summary>
            Public Sub New()
                With Me
                    .m_Center = New Math.D2.Point(System.Double.NaN,
                                                  System.Double.NaN)
                    .m_Eccentricity = System.Double.NaN
                    .m_Focus0 = New Math.D2.Point(System.Double.NaN,
                                                  System.Double.NaN)
                    .m_Focus1 = New Math.D2.Point(System.Double.NaN,
                                                  System.Double.NaN)
                    .m_Height = System.Double.NaN
                    .m_MajorAngleD = System.Double.NaN
                    .m_MajorAngleR = System.Double.NaN
                    .m_MajorLen = System.Double.NaN
                    .m_MajorM = System.Double.NaN
                    .m_MajorVertex0 = New Math.D2.Point(System.Double.NaN,
                                                        System.Double.NaN)
                    .m_MajorVertex1 = New Math.D2.Point(System.Double.NaN,
                                                        System.Double.NaN)
                    .m_MinorAngleD = System.Double.NaN
                    .m_MinorAngleR = System.Double.NaN
                    .m_MinorB = System.Double.NaN
                    .m_MinorLen = System.Double.NaN
                    .m_MinorM = System.Double.NaN
                    .m_MinorVertex0 = New Math.D2.Point(System.Double.NaN,
                                                        System.Double.NaN)
                    .m_MinorVertex1 = New Math.D2.Point(System.Double.NaN,
                                                        System.Double.NaN)
                End With
            End Sub ' New

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="width">xxxxxxxxxx</param>
            ''' <param name="height">xxxxxxxxxx</param>
            ''' <param name="center">xxxxxxxxxx</param>
            ''' <param name="rotation">xxxxxxxxxx</param>
            Public Sub New(ByVal width As System.Double, ByVal height As System.Double,
                ByVal center As Math.D2.Point, ByVal rotation As System.Double)

                Me.New()

                ' Input checking.
                If width <= 0 Then
                    'Dim CaughtBy As System.Reflection.MethodBase =
                    '    System.Reflection.MethodBase.GetCurrentMethod
                    Throw New System.ArgumentOutOfRangeException(NameOf(width), MSGVMBGTZ)
                End If
                If height <= 0 Then
                    'Dim CaughtBy As System.Reflection.MethodBase =
                    '    System.Reflection.MethodBase.GetCurrentMethod
                    Throw New System.ArgumentOutOfRangeException(NameOf(height), MSGVMBGTZ)
                End If

                With Me

                    ' Take the provided values.
                    Me.m_Width = width
                    Me.m_Height = height
                    .m_Center = center
                    .m_Rotation = rotation

                    .PushFromNew()

                End With

            End Sub ' New

            ''' <summary>
            ''' xxxxxxxxxx
            ''' </summary>
            ''' <param name="width">xxxxxxxxxx</param>
            ''' <param name="height">xxxxxxxxxx</param>
            ''' <param name="centerX">xxxxxxxxxx</param>
            ''' <param name="centerY">xxxxxxxxxx</param>
            ''' <param name="rotation">xxxxxxxxxx</param>
            Public Sub New(ByVal width As System.Double,
                ByVal height As System.Double, ByVal centerX As System.Double,
                ByVal centerY As System.Double, ByVal rotation As System.Double)

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                Me.New(width, height, New OSNW.Math.D2.Point(centerX, centerY), rotation)
            End Sub ' New

#End Region ' "Constructors"

        End Class ' Ellipse

    End Structure ' Math2D

End Module ' Math
