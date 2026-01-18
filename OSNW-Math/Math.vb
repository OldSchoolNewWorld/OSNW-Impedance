Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Public Module Math

    ''' <summary>
    ''' Computes the distance between two points in a 3D space.
    ''' </summary>
    ''' <param name="x1">Specifies the X-coordinate of one point.</param>
    ''' <param name="y1">Specifies the Y-coordinate of one point.</param>
    ''' <param name="z1">Specifies the Z-coordinate of one point.</param>
    ''' <param name="x2">Specifies the X-coordinate of the other point.</param>
    ''' <param name="y2">Specifies the Y-coordinate of the other point.</param>
    ''' <param name="z2">Specifies the Z-coordinate of the other point.</param>
    ''' <returns>The distance between the two points.</returns>
    Public Function Distance3D(ByVal x1 As System.Double,
        ByVal y1 As System.Double, ByVal z1 As System.Double,
        ByVal x2 As System.Double, ByVal y2 As System.Double,
        ByVal z2 As System.Double) As System.Double

        ' Based on Pythagorean theorem.
        Dim DeltaX As System.Double = x2 - x1
        Dim DeltaY As System.Double = y2 - y1
        Dim DeltaZ As System.Double = z2 - z1
        Return System.Math.Sqrt(
            (DeltaX * DeltaX) + (DeltaY * DeltaY) + (DeltaZ * DeltaZ))
    End Function ' Distance3D

    ''' <summary>
    ''' Attempts to solve the quadratic equation a*x^2 + b*x + c = 0 for real
    ''' solutions.
    ''' </summary>
    ''' <param name="a">Specifies the <paramref name="a"/> in the well-known
    ''' formula.</param>
    ''' <param name="b">Specifies the <paramref name="b"/> in the well-known
    ''' formula.</param>
    ''' <param name="c">Specifies the <paramref name="c"/> in the well-known
    ''' formula.</param>
    ''' <returns><c>True</c> if the process succeeds; otherwise, <c>False</c>.
    ''' When valid, also returns the results in <paramref name="x1"/> and
    ''' <paramref name="x2"/>.</returns>
    Public Function TryQuadratic(ByVal a As System.Double,
        ByVal b As System.Double, ByVal c As System.Double,
        ByRef x1 As System.Double, ByRef x2 As System.Double) As System.Boolean

        ' Input checking.
        Dim Discriminant As System.Double = b * b - 4.0 * a * c
        If a.Equals(0.0) OrElse Discriminant < 0.0 Then
            ' Not a quadratic equation.
            x1 = Double.NaN
            x2 = Double.NaN
            Return False
        End If

        Dim DiscRoot As System.Double = System.Math.Sqrt(Discriminant)
        Dim A2 As System.Double = 2.0 * a
        x1 = (-b + DiscRoot) / A2
        x2 = (-b - DiscRoot) / A2
        Return True

    End Function ' TryQuadratic

    '''' <Summary>
    '''' xxxxxxxxxx
    '''' </Summary>
    '''' <param name="circleX">xxxxxxxxxx</param>
    '''' <param name="circleY">xxxxxxxxxx</param>
    '''' <param name="circleR">xxxxxxxxxx</param>
    '''' <param name="lineM">xxxxxxxxxx</param>
    '''' <param name="lineI">xxxxxxxxxx</param>
    '''' <param name="int1X">xxxxxxxxxx</param>
    '''' <param name="int1Y">xxxxxxxxxx</param>
    '''' <param name="int2X">intxxxxxxxxxx2X</param>
    '''' <param name="Int2Y">xxxxxxxxxx</param>
    '''' <returns>xxxxxxxxxx</returns>
    '''' <remarks>xxxxxxxxxx</remarks>
    Public Function TryCircleLineIntersection(
        ByVal circleX As System.Double, ByVal circleY As System.Double,
        ByVal circleR As System.Double, ByVal lineM As System.Double,
        ByVal lineI As System.Double, ByRef int1X As System.Double,
        ByRef int1Y As System.Double, ByRef int2X As System.Double,
        ByRef Int2Y As System.Double) As System.Boolean


        Dim cHECKPOINT1 As System.Double
        Dim cHECKPOINT2 As System.Double


        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        ' First, check for the vertical case?
        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!






        ' The derivation follows:
        ' Standard form of a circle and a line.
        ' (X - circleX)^2 + (Y - circleY)^2 = circleR^2
        ' Y = lineM * X + B

        ' Localize parameters, for one point of intersection.
        ' (int1X - circleX)^2 + (int1Y - circleY)^2 = circleR^2
        ' y1 = lineM * int1X + lineI

        cHECKPOINT1 = (int1X - circleX) ^ 2 + (int1Y - circleY) ^ 2
        cHECKPOINT2 = circleR ^ 2
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If
        cHECKPOINT1 = int1Y
        cHECKPOINT2 = lineM * int1X + lineI
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If

        ' A point at the intersection of the circle and the line conforms to
        ' both equations.
        ' (int1X - circleX)^2 + ((lineM * int1X + lineI)- circleY)^2 = circleR^2
        ' (int1X - circleX)^2 + (lineM * int1X + lineI - circleY)^2 = circleR^2

        cHECKPOINT1 = (int1X - circleX) ^ 2 + (lineM * int1X + lineI - circleY) ^ 2
        cHECKPOINT2 = circleR ^ 2
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If

        ' Rewrite for visibility.
        ' (int1X - circleX)^2
        ' + ((lineM * int1X) + lineI - circleY)^2
        ' = circleR^2

        cHECKPOINT1 = (int1X - circleX) ^ 2 + (lineM * int1X + lineI - circleY) ^ 2
        cHECKPOINT2 = circleR ^ 2
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If

        ' Expand the squares.
        ' int1X^2 - (2 * circleX * int1X) + circleX^2
        ' + (lineM * int1X) * ((lineM * int1X) + lineI - circleY)
        ' + lineI           * ((lineM * int1X) + lineI - circleY)
        ' - circleY         * ((lineM * int1X) + lineI - circleY)
        ' = circleR^2

        cHECKPOINT1 =
            int1X ^ 2 - (2 * circleX * int1X) + circleX ^ 2 _
            + (lineM * int1X) * ((lineM * int1X) + lineI - circleY) _
            + lineI * ((lineM * int1X) + lineI - circleY) _
            - circleY * ((lineM * int1X) + lineI - circleY)
        cHECKPOINT2 = circleR ^ 2
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If

        ' Distribute the multiplications.
        ' int1X^2 -2*circleX*int1X + circleX^2
        ' + (lineM*int1X*lineM*int1X + lineM*int1X*lineI - lineM*int1X*circleY)
        ' + (lineI*lineM*int1X + lineI*lineI - lineI*circleY)
        ' - (circleY*lineM*int1X + circleY*lineI - circleY*circleY)
        ' = circleR^2

        cHECKPOINT1 =
            int1X ^ 2 - 2 * circleX * int1X + circleX ^ 2 _
            + (lineM * int1X * lineM * int1X + lineM * int1X * lineI - lineM * int1X * circleY) _
            + (lineI * lineM * int1X + lineI * lineI - lineI * circleY) _
         - (circleY * lineM * int1X + circleY * lineI - circleY * circleY)
        cHECKPOINT2 = circleR ^ 2
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If

        ' Normalize terms.
        ' int1X^2 -2*circleX*int1X + circleX^2
        ' + lineM*lineM*int1X^2 + lineM*lineI*int1X - lineM*circleY*int1X
        ' + lineI*lineM*int1X + lineI*lineI - lineI*circleY
        ' - circleY*lineM*int1X - circleY*lineI + circleY*circleY
        ' = circleR^2

        cHECKPOINT1 =
            int1X ^ 2 - 2 * circleX * int1X + circleX ^ 2 _
            + lineM * lineM * int1X ^ 2 + lineM * lineI * int1X - lineM * circleY * int1X _
            + lineI * lineM * int1X + lineI * lineI - lineI * circleY _
            - circleY * lineM * int1X - circleY * lineI + circleY * circleY

        cHECKPOINT2 = circleR ^ 2
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If

        ' Gather like terms. Arrange for quadratic formula.
        ' int1X^2 + (lineM^2)*int1X^2
        ' -(2*circleX)*int1X + (2*lineM*lineI)*int1X - (2*lineM*circleY)*int1X
        ' + circleX^2 + lineI*lineI - 2*lineI*circleY + circleY^2 - circleR^2
        ' = 0

        cHECKPOINT1 =
             int1X ^ 2 + (lineM ^ 2) * int1X ^ 2 _
             - (2 * circleX) * int1X + (2 * lineM * lineI) * int1X - (2 * lineM * circleY) * int1X _
             + circleX ^ 2 + lineI * lineI - 2 * lineI * circleY + circleY ^ 2 - circleR ^ 2
        cHECKPOINT2 = 0
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If






        '        REWORKED ABOVE HERE



        ' Extract X terms.
        ' (1 + lineM*lineM)*int1X^2
        ' + 2*(-circleX + lineM*lineI - lineM*circleY)*int1X
        ' + circleX^2 + lineI*lineI - 2*lineI*circleY + circleY*circleY - circleR^2
        ' = 0

        cHECKPOINT1 = (1 + lineM * lineM) * int1X ^ 2 + 2 * (-circleX + lineM * lineI - lineM * circleY) * int1X + circleX ^ 2 + lineI * lineI - 2 * lineI * circleY + circleY * circleY - circleR ^ 2
        cHECKPOINT2 = 0
        If Not Double.Abs(cHECKPOINT2 - cHECKPOINT1) < 0.01 Then
            Return False
        End If

        ' Set up for quadratic formula.
        ' a = 1 + (lineM*lineM)
        ' b = 2*(-circleX + lineM*lineI - lineM*circleY)
        ' c = circleX^2 + lineI*lineI - 2*lineI*circleY + circleY*circleY - circleR^2

        ' Implementation:

        Dim a As System.Double = 1 + (lineM * lineM)
        Dim b As System.Double = 2 * (-circleX + lineM * lineI - lineM * circleY)
        Dim c As System.Double = circleX ^ 2 + lineI * lineI - 2 * lineI * circleY + circleY * circleY - circleR ^ 2

        If Not TryQuadratic(a, b, c, int1X, int2X) Then
            Return False
        End If

        ' y = lineMx + b
        int1Y = lineM * int1X + lineI
        Int2Y = lineM * int2X + lineI
        Return True

    End Function ' TryCircleLineIntersection

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="circleX">xxxxxxxxxx</param>
    ''' <param name="circleY">xxxxxxxxxx</param>
    ''' <param name="circleR">xxxxxxxxxx</param>
    ''' <param name="lineX1">xxxxxxxxxx</param>
    ''' <param name="lineY1">xxxxxxxxxx</param>
    ''' <param name="lineint2X">xxxxxxxxxx</param>
    ''' <param name="lineY2">xxxxxxxxxx</param>
    ''' <param name="intersect1X">xxxxxxxxxx</param>
    ''' <param name="intersect1Y">xxxxxxxxxx</param>
    ''' <param name="intersect2X">xxxxxxxxxx</param>
    ''' <param name="intersect2Y">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>xxxxxxxxxx</remarks>
    Public Function TryCircleLineIntersection(ByVal circleX As System.Double,
        ByVal circleY As System.Double, ByVal circleR As System.Double,
        ByVal lineX1 As System.Double, ByVal lineY1 As System.Double,
        ByVal lineint2X As System.Double, ByVal lineY2 As System.Double,
        ByRef intersect1X As System.Double, ByRef intersect1Y As System.Double,
        ByRef intersect2X As System.Double,
        ByRef intersect2Y As System.Double) As System.Boolean

        ' Get the slope of the line.
        ' M = (Y2 - Y1) / (int2X - X1); generic slope.
        Dim lineM As System.Double = (lineY2 - lineY1) / (lineint2X - lineX1)


        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        ' First, check for the vertical case?
        '!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!





        ' Get the equation for the line.
        ' Y = M*X + B; Standard form line.
        ' B = Y - M*X; Solve for the Y-intercept.
        Dim lineB As System.Double = lineY1 - lineM * lineX1

        Return TryCircleLineIntersection(circleX, circleY, circleR, lineX1,
            lineB, intersect1X, intersect1Y, intersect2X, intersect2Y)

    End Function ' TryCircleLineIntersection

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="x1">xxxxxxxxxx</param>
    ''' <param name="y1">xxxxxxxxxx</param>
    ''' <param name="r1">xxxxxxxxxx</param>
    ''' <param name="int2X">xxxxxxxxxx</param>
    ''' <param name="y2">xxxxxxxxxx</param>
    ''' <param name="r2">xxxxxxxxxx</param>
    ''' <returns>xxxxxxxxxx
    ''' </returns>
    ''' <remarks>
    ''' Tangent circles will have only one intersection. When both circles
    ''' specify the same circle, they are considered not to intersect.
    ''' </remarks>
    Public Function CirclesIntersect(ByVal x1 As System.Double,
        ByVal y1 As System.Double, ByVal r1 As System.Double,
        ByVal int2X As System.Double, ByVal y2 As System.Double,
        ByVal r2 As System.Double) As System.Boolean

        ' Input checking.
        If (r1 < 0.0) OrElse (r2 < 0.0) Then
            Return False
        End If

        ' Check for solvability.
        Dim CtrSeparation As System.Double =
            System.Double.Hypot(int2X - x1, y2 - y1)
        If CtrSeparation > (r1 + r2) Then
            ' Two isolated circles.
            Return False
        ElseIf CtrSeparation < System.Math.Abs(r2 - r1) Then
            ' One inside the other.
            Return False
        ElseIf int2X.Equals(x1) AndAlso y2.Equals(y1) Then
            ' They are concentric, with either zero or infinite common points.
            ' The second case is consirered not to be intersecting.
            Return False
        End If
        Return True

    End Function ' CirclesIntersect

    Public Function TryCircleIntersection(ByVal centerX1 As System.Double,
        ByVal centerY1 As System.Double, ByVal radius1 As System.Double,
        ByVal centerint2X As System.Double, ByVal centerY2 As System.Double,
        ByVal radius2 As System.Double, ByRef intersect1X As System.Double,
        ByRef intersect1Y As System.Double, ByRef intersect2X As System.Double,
        ByRef intersect2Y As System.Double) As System.Boolean

        If Not CirclesIntersect(centerX1, centerY1, radius1,
            centerint2X, centerY2, radius2) Then
            intersect1X = Double.NaN
            intersect1Y = Double.NaN
            intersect2X = Double.NaN
            intersect2Y = Double.NaN
            Return False
        End If

        intersect1X = 999.99
        intersect1Y = 999.99
        intersect2X = 999.99
        intersect2Y = 999.99



        '        xxxx




        Return False ' Until implemented.

    End Function ' TryCircleIntersection

End Module ' Math

' Partial Public Structure Math
' 
'     ''' <summary>
'     ''' A structure to define two circles in order to consider their overlap relationship.
'     ''' </summary>
'     ''' <remarks>
'     ''' 
'     ''' Check the status of CirclesIntersect before reading the intersection points!
'     ''' 
'     ''' The following possibilities have been identified for two circles:
'     '''   They may be separate and not intersect.
'     '''   They may be externally tangent and intersect at one point.
'     '''   They may intersect at two points.
'     '''   A small circle may be internally tangent to a large circle and intersect at one point.
'     '''   A small circle may be inside the other with no intersecting points.
'     '''   They may describe the same circle. CirclesIntersect returns <c>False</c> for this case.
'     ''' 
'     ''' </remarks>
'     Public Structure IntersectingCircleData
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Creates a new Ytt.Util.Math.IntersectingCircleData.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Sub New(ByVal circle1 As Ytt.Util.Math.CircleData, ByVal circle2 As Ytt.Util.Math.CircleData)
'         ''' </declaration>
'         ''' <param name="circle1">Describes one circle.</param>
'         ''' <param name="circle2">Describes one circle.</param>
'         ''' <remarks></remarks>
'         Public Sub New(ByVal circle1 As Ytt.Util.Math.CircleData, ByVal circle2 As Ytt.Util.Math.CircleData)
'             Me.New(circle1.CenterX, circle1.CenterY, circle1.Radius,
'                    circle2.CenterX, circle2.CenterY, circle2.Radius)
'         End Sub ' New
' 
'         ''' <summary>
'         ''' Overloaded.
'         ''' Creates a new Ytt.Util.Math.IntersectingCircleData.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public Sub New(ByVal centerX1 As System.Double, ByVal centerY1 As System.Double, ByVal radius1 As System.Double,
'         ''' ByVal centerint2X As System.Double, ByVal centerY2 As System.Double, ByVal radius2 As System.Double)
'         ''' </declaration>
'         ''' <param name="centerX1">The X coordinate of the center of one circle.</param>
'         ''' <param name="centerY1">The Y coordinate of the center of one circle.</param>
'         ''' <param name="radius1">The radius of one circle. Cannot be negative.</param>
'         ''' <param name="centerint2X">The X coordinate of the center of the other circle.</param>
'         ''' <param name="centerY2">The Y coordinate of the center of the other circle.</param>
'         ''' <param name="radius2">The radius of the other circle. Cannot be negative.</param>
'         ''' <exception cref="System.ArgumentException">
'         ''' Thrown when <paramref name="radius1"/> or <paramref name="radius1"/> is negative.
'         ''' </exception>
'         ''' <remarks></remarks>
'         Public Sub New(ByVal centerX1 As System.Double, ByVal centerY1 As System.Double, ByVal radius1 As System.Double,
'                        ByVal centerint2X As System.Double, ByVal centerY2 As System.Double, ByVal radius2 As System.Double)
' 
'             ' References:
'             ' http://paulbourke.net/geometry/circlesphere/
'             ' http://paulbourke.net/geometry/2circle/
'             ' http://paulbourke.net/geometry/2circle/tvoght.c
' 
'             ' Input checking.
'             If (radius1 < 0.0) Then
'                 Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                 Dim Ex = Ytt.Util.RunTime.NewValueCannotBeNegativeException(ProcName, radius1, "radius1")
'                 Ex.Source = ProcName
'                 Ex.Data.Add("centerX1", centerX1)
'                 Ex.Data.Add("centerY1", centerY1)
'                 Ex.Data.Add("radius1", radius1)
'                 Ex.Data.Add("centerint2X", centerint2X)
'                 Ex.Data.Add("centerY2", centerY2)
'                 Ex.Data.Add("radius2", radius2)
'                 Throw Ex
'             End If
'             If (radius2 < 0.0) Then
'                 Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                 Dim Ex = Ytt.Util.RunTime.NewValueCannotBeNegativeException(ProcName, radius2, "radius2")
'                 Ex.Source = ProcName
'                 Ex.Data.Add("centerX1", centerX1)
'                 Ex.Data.Add("centerY1", centerY1)
'                 Ex.Data.Add("radius1", radius1)
'                 Ex.Data.Add("centerint2X", centerint2X)
'                 Ex.Data.Add("centerY2", centerY2)
'                 Ex.Data.Add("radius2", radius2)
'                 Throw Ex
'             End If
' 
'             With Me
'                 .m_X1 = centerX1 : .m_Y1 = centerY1 : .m_R1 = radius1
'                 .m_int2X = centerint2X : .m_Y2 = centerY2 : .m_R2 = radius2
'             End With
' 
'             ' DeltaX and DeltaY are the vertical and horizontal distances between the circle centers.
'             Dim DeltaX = (m_int2X - m_X1)
'             Dim DeltaY = (m_Y2 - m_Y1)
' 
'             ' Determine the straight-line distance between the centers. 
'             Dim CenterSeparation = Ytt.Util.Math.Hypotenuse(DeltaX, DeltaY)
' 
'             ' Check for solvability.
'             Me.m_CirclesIntersect = True ' For now.
'             If ((Me.m_int2X = Me.m_X1) AndAlso (Me.m_Y2 = Me.m_Y1) AndAlso (Me.m_R2 = Me.m_R1)) Then
'                 ' They are both the same.
'                 Me.m_CirclesIntersect = False
'             ElseIf (CenterSeparation > (m_R1 + m_R2)) Then
'                 ' Two isolated circles.
'                 Me.m_CirclesIntersect = False
'             ElseIf (CenterSeparation < System.Math.Abs(m_R1 - m_R2)) Then
'                 ' One inside the other.
'                 Me.m_CirclesIntersect = False
'             End If
' 
'             If Me.m_CirclesIntersect Then
' 
' 
'                 ' "point 3" is the point where the line through the circle
'                 ' intersection points crosses the line between the circle
'                 ' centers.  
' 
'                 Dim A, H, Rx, Ry, X3, Y3 As System.Double
' 
'                 ' Determine the distance from point 1 to point 3. 
'                 A = ((m_R1 * m_R1) - (m_R2 * m_R2) + (CenterSeparation * CenterSeparation)) / (2.0 * CenterSeparation)
' 
'                 ' Determine the coordinates of point 3. 
'                 Dim OnceACS = (A / CenterSeparation)
'                 X3 = (m_X1 + (DeltaX * OnceACS))
'                 Y3 = (m_Y1 + (DeltaY * OnceACS))
' 
'                 ' Determine the distance from point 3 to either of the
'                 ' intersection points.
'                 H = System.Math.Sqrt((m_R1 * m_R1) - (A * A))
' 
'                 ' Now determine the offsets of the intersection points from point 3.
'                 Dim OnceHCS = (H / CenterSeparation)
'                 Rx = (-DeltaY * OnceHCS)
'                 Ry = (DeltaX * OnceHCS)
' 
'                 ' Determine the absolute intersection points. 
'                 m_Intersect1X = X3 + Rx
'                 m_Intersect2X = X3 - Rx
'                 m_Intersect1Y = Y3 + Ry
'                 m_Intersect2Y = Y3 - Ry
' 
'             End If ' Me.m_CirclesIntersect
' 
'         End Sub ' New
' 
'         ''' <summary>
'         ''' Returns <c>True</c> if the circles intersect. Otherwise <c>False</c>.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public ReadOnly Property CirclesIntersect As System.Boolean
'         ''' </declaration>
'         ''' <value><c>True</c> if the circles intersect. Otherwise <c>False</c>.</value>
'         ''' <remarks></remarks>
'         Public ReadOnly Property CirclesIntersect As System.Boolean
'             Get
'                 Return Me.m_CirclesIntersect
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The X coordinate of the first intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' Public ReadOnly Property Intersect1X As System.Double
'         ''' </declaration>
'         ''' <value>The X coordinate of the first intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect1X As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect1X
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The Y coordinate of the first intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' The Y coordinate of the first intersection.
'         ''' </declaration>
'         ''' <value>The Y coordinate of the first intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect1Y As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect1Y
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The X coordinate of the second intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' The X coordinate of the second intersection.
'         ''' </declaration>
'         ''' <value>The X coordinate of the second intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect2X As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect2X
'             End Get
'         End Property
' 
'         ''' <summary>
'         ''' The Y coordinate of the second intersection.
'         ''' </summary>
'         ''' <declaration>
'         ''' The Y coordinate of the second intersection.
'         ''' </declaration>
'         ''' <value>The Y coordinate of the second intersection.</value>
'         ''' <remarks>
'         ''' Check the status of CirclesIntersect before reading the intersection points!
'         ''' </remarks>
'         Public ReadOnly Property Intersect2Y As System.Double
'             Get
'                 ' State checking.
'                 If (Not Me.CirclesIntersect) Then
'                     Dim ProcName = (ProcNameBase() & New System.Diagnostics.StackFrame(0).GetMethod().Name)
'                     Dim Ex = Ytt.Util.Math.IntersectingCircleData.GetCheckFirstException(ProcName)
'                     Ex.Source = ProcName
'                     Throw Ex
'                 End If
'                 Return m_Intersect2Y
'             End Get
'         End Property
' 
'         Private Shared Function GetCheckFirstException(ByVal sourceProcName As System.String) As System.ApplicationException
'             Dim S1 = "Check the status of CirclesIntersect before reading the intersection points."
'             Dim S2 = "Circles do not intersect."
'             Return New System.ApplicationException(Ytt.Util.RunTime.FormattedExceptionString(S1, sourceProcName, S2))
'         End Function
' 
'         Private m_CirclesIntersect As System.Boolean
' 
'         ' Center and radius of 1st circle.
'         Private m_X1 As System.Double
'         Private m_Y1 As System.Double
'         Private m_R1 As System.Double
'         ' Center and radius of 2nd circle.
'         Private m_int2X As System.Double
'         Private m_Y2 As System.Double
'         Private m_R2 As System.Double
' 
'         ' 1st intersection point.
'         Private m_Intersect1X, m_Intersect1Y As System.Double
'         ' 2nd intersection point.
'         Private m_Intersect2X, m_Intersect2Y As System.Double
' 
'     End Structure ' IntersectingCircleData
' 
' End Structure ' Math
