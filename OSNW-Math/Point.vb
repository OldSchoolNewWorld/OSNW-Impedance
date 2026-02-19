Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

Partial Public Module Math

    Partial Public Structure D2

        ''' <summary>
        ''' Returns the normalized angle that is the result of rotating the
        ''' specified <paramref name="angle"/> by the specified angle of
        ''' <paramref name="rotation"/>. All values are in radians.
        ''' </summary>
        ''' <param name="angle">Specifies the angle in radians to be
        ''' rotated.</param>
        ''' <param name="rotation">Specifies the angle in radians (positive for
        ''' CCW; negative for CW) by which to rotate.</param>
        ''' <returns>The normalized angle that is the result of the
        ''' rotation.</returns>
        ''' <exception cref="System.ArgumentOutOfRangeException">
        ''' Thrown when any parameter is infinite.
        ''' </exception>
        ''' <remarks>Out-of-range values of both <paramref name="angle"/> and
        ''' <paramref name="rotation"/> are accepted but the result is
        ''' normalized.</remarks>
        Private Shared Function RotateNormalRad(ByVal angle As System.Double,
            rotation As System.Double) As System.Double

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' Input checking.
            If System.Double.IsInfinity(angle) OrElse
                    System.Double.IsInfinity(rotation) Then

                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                $"Arguments to {NameOf(RotateNormalRad)} {MSGCHIV}")
            End If

            ' Calculate and normalize the resulting angle.
            ' WORK OUT A MORE SOPHISTICATED APPROACH THAN LOOPED STEPS?
            ' REMAINDER OF A DIVISION?
            Dim NewAngle As System.Double = angle + rotation
            While NewAngle > OSNW.Math.PId
                NewAngle -= OSNW.Math.TWOPId
            End While
            While NewAngle <= -OSNW.Math.PId
                ' NOTE: -PI will become PI.
                NewAngle += OSNW.Math.TWOPId
            End While
            Return NewAngle

        End Function ' RotateAngleRad

        ''' <summary>
        ''' Returns the normalized angle that is the result of rotating the
        ''' specified <paramref name="angle"/> by the specified angle of
        ''' <paramref name="rotation"/>. All values are in degrees.
        ''' </summary>
        ''' <param name="angle">Specifies the angle in degrees to be
        ''' rotated.</param>
        ''' <param name="rotation">Specifies the angle in degrees (positive for
        ''' CCW; negative for CW) by which to rotate.</param>
        ''' <returns>The normalized angle that is the result of the
        ''' rotation.</returns>
        ''' <exception cref="System.ArgumentOutOfRangeException">
        ''' Thrown when any parameter is infinite.
        ''' </exception>
        ''' <remarks>Out-of-range values of both <paramref name="angle"/> and
        ''' <paramref name="rotation"/> are accepted but the result is
        ''' normalized.</remarks>
        Private Shared Function RotateAngleDeg(ByVal angle As System.Double,
            rotation As System.Double) As System.Double

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' Input checking.
            If System.Double.IsInfinity(angle) OrElse
                    System.Double.IsInfinity(rotation) Then

                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                $"Arguments to {NameOf(RotateNormalRad)} {MSGCHIV}")
            End If

            ' Calculate and normalize the resulting angle.
            ' WORK OUT A MORE SOPHISTICATED APPROACH THAN LOOPED STEPS?
            ' REMAINDER OF A DIVISION?
            Dim NewAngle As System.Double = angle + rotation
            While NewAngle > 360.0
                NewAngle -= 360.0
            End While
            While NewAngle <= 360.0
                ' NOTE: -360.0 will become 360.0.
                NewAngle += 360.0
            End While
            Return NewAngle

        End Function ' RotateAngleDeg

        ''' <summary>
        ''' Represents an ordered pair of X and Y double precision coordinates
        ''' that define a point in a two-dimensional plane.
        ''' </summary>
        Public Class Point

            ''' <summary>
            ''' Represents the X-coordinate of this <c>Point2D</c>.
            ''' </summary>
            Public X As System.Double

            ''' <summary>
            ''' Represents the Y-coordinate of this <c>Point2D</c>.
            ''' </summary>
            Public Y As System.Double

#Region "Movement"

            ''' <summary>
            ''' Returns a <see cref="Math.D2.Point"/> that is the result of
            ''' rotating the current instance, by the specified
            ''' <paramref name="angle"/> in radians, around the specified center
            ''' of rotation.
            ''' </summary>
            ''' <param name="angle">Specifies the angle in radians (positive for
            ''' CCW; negative for CW) by which to rotate. </param>
            ''' <param name="centerX">Specifies the X-coordinate of the center
            ''' of rotation.</param>
            ''' <param name="centerY">Specifies the Y-coordinate of the center
            ''' of rotation.</param>
            ''' <returns>The result of rotating the current instance around the
            ''' specified center of rotation.</returns>
            ''' <exception cref="System.ArgumentOutOfRangeException">
            ''' Thrown when any parameter is infinite.
            ''' </exception>
            ''' <remarks>Out-of-range values of <paramref name="angle"/> are
            ''' accepted but the result is normalized.</remarks>
            Public Function RotatedRad(
                ByVal angle As System.Double, ByVal centerX As System.Double,
                ByVal centerY As System.Double) As Math.D2.Point

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                ' DEV: This is the worker for the related routine(s).

                ' Input checking.
                If System.Double.IsInfinity(angle) OrElse
                    System.Double.IsInfinity(centerX) OrElse
                    System.Double.IsInfinity(centerY) Then

                    Dim CaughtBy As System.Reflection.MethodBase =
                        System.Reflection.MethodBase.GetCurrentMethod
                    Throw New System.ArgumentOutOfRangeException(
                        $"Arguments to {NameOf(CaughtBy)} {MSGCHIV}")
                End If

                ' Determine the current angle and radial length.
                Dim DeltaX As System.Double = Me.X - centerX
                Dim DeltaY As System.Double = Me.Y - centerY
                Dim CurrentAngle As System.Double
                Dim CurrentTan As System.Double
                Dim Hyp As System.Double
                If DeltaX.Equals(0.0) Then
                    ' Same X.
                    If DeltaY > 0.0 Then
                        CurrentAngle = OSNW.Math.HALFPId
                    ElseIf DeltaY < 0.0 Then
                        CurrentAngle = -OSNW.Math.HALFPId
                    Else
                        ' Same point.
                        Return New OSNW.Math.D2.Point(Me.X, Me.Y)
                    End If
                    Hyp = System.Math.Abs(DeltaY)
                Else
                    ' Different X.
                    ' CurrentTan has a value in the interval
                    ' [-infinity, +infinity] and does not distinguish
                    ' Me.X<centerX from Me.X>centerX.
                    ' ATAN computes arctan(centerX) in the interval [-PI/2, +PI/2]
                    ' radians.
                    ' https://learn.microsoft.com/en-us/dotnet/api/system.double.atan?view=net-10.0&f1url=%3FappId%3DDev17IDEF1%26l%3DEN-US%26k%3Dk(System.Double.Atan)%3Bk(DevLang-VB)%26rd%3Dtrue
                    CurrentTan = DeltaY / DeltaX
                    CurrentAngle = System.Double.Atan(CurrentTan)
                    If DeltaX < 0.0 Then
                        If DeltaY < 0.0 Then
                            CurrentAngle -= OSNW.Math.PId
                        Else
                            CurrentAngle += OSNW.Math.PId
                        End If
                    End If
                    Hyp = System.Math.Sqrt((DeltaX * DeltaX) _
                                           + (DeltaY * DeltaY))
                End If

                ' Calculate the resulting normalized angle.
                Dim NewAngle As System.Double =
                    OSNW.Math.D2.RotateNormalRad(CurrentAngle, angle)

                ' sin(alpha) = opposite / hypotenuse.
                ' sin(NewAngle) = (NewY - centerY) / Hyp
                ' sin(NewAngle) * Hyp = NewY - centerY
                ' (sin(NewAngle) * Hyp) + centerY = NewY
                Dim NewY As System.Double =
                    centerY + (System.Double.Sin(NewAngle) * Hyp)

                ' cos(alpha) = adjacent / hypotenuse.
                ' cos(NewAngle) = (NewX - centerX) / Hyp
                ' cos(NewAngle) * Hyp = NewX - centerX
                ' (cos(NewAngle) * Hyp) + centerX = NewX
                Dim NewX As System.Double =
                    centerX + (System.Double.Cos(NewAngle) * Hyp)

                Return New Math.D2.Point(NewX, NewY)

            End Function ' RotatedRad

            ''' <summary>
            ''' Returns a <see cref="Math.D2.Point"/> that is the result of
            ''' rotating the current instance, by the specified
            ''' <paramref name="angle"/> in radians, around the specified center
            ''' of rotation.
            ''' </summary>
            ''' <param name="angle">Specifies the angle in degrees (positive for
            ''' CCW; negative for CW) by which to rotate. </param>
            ''' <param name="center">Specifies the center of
            ''' rotation.</param>
            ''' <returns>The result of rotating the current instance around the
            ''' specified center of rotation.</returns>
            ''' <exception cref="System.ArgumentOutOfRangeException">
            ''' Thrown when any parameter is infinite.
            ''' </exception>
            ''' <remarks>Out-of-range values of <paramref name="angle"/> are
            ''' accepted but the result is normalized.</remarks>
            Public Function RotatedRad(ByVal angle As System.Double,
                ByVal center As Math.D2.Point) As Math.D2.Point

                Return Me.RotatedRad(angle, center.X, center.Y)
            End Function ' RotatedRad

            ''' <summary>
            ''' Returns the result of rotating the current instance, by the
            ''' specified <paramref name="angle"/>, around the specified center
            ''' of rotation.
            ''' </summary>
            ''' <param name="angle">Specifies the angle (positive for CCW;
            ''' negative for CW) by which to rotate. </param>
            ''' <param name="x">Specifies the X-coordinate of the center of
            ''' rotation.</param>
            ''' <param name="y">Specifies the Y-coordinate of the center of
            ''' rotation.</param>
            ''' <returns>The result of rotating the current instance around the
            ''' specified center of rotation.</returns>
            ''' <exception cref="System.ArgumentOutOfRangeException">
            ''' Thrown when any parameter is infinite.
            ''' </exception>
            ''' <remarks>Out-of-range values of <paramref name="angle"/> are
            ''' accepted but the result is normalized.</remarks>
            Public Function RotatedDeg(ByVal angle As System.Double,
                ByVal x As System.Double, ByVal y As System.Double) _
                As Math.D2.Point

                Return Me.RotatedRad(Double.DegreesToRadians(angle), x, y)
            End Function ' RotatedDeg

            ''' <summary>
            ''' Returns the result of shifting the current instance by the
            ''' specified horizontal (<paramref name="x"/>) and vertical
            ''' (<paramref name="y"/>) amounts.
            ''' </summary>
            ''' <param name="x">Specifies the amount of the horizontal shift; a
            ''' positive value shifts right and a negative value shifts
            ''' left.</param>
            ''' <param name="y">Specifies the amount of the horizontal shift; a
            ''' positive value shifts up and a negative value shifts
            ''' down.</param>
            ''' <returns>The shifted equivalent of the current
            ''' instance.</returns>
            Public Function Shifted(ByVal x As System.Double,
                ByVal y As System.Double) As Math.D2.Point

                Return New Math.D2.Point(Me.X + x, Me.Y + y)
            End Function ' Shifted

#End Region ' "Movement"

            ''' <summary>
            ''' Returns the distance between the current instance and another
            ''' <c>Point2D</c> in the same 2D plane.
            ''' </summary>
            ''' <param name="other">Specifies a distant <c>Point2D</c>.</param>
            ''' <returns>The distance between the two points.</returns>
            ''' <remarks>
            ''' <see cref="Math.D2.Distance(Double, Double, Double, Double)"/>,
            ''' <see cref="Math.D2.Distance(Math.D2.Point, Math.D2.Point)"/>,
            ''' and <see cref="Math.D2.Point.Distance(OSNW.Math.D2.Point)"/>
            ''' are effectively the same thing. Use whichever best suits the
            ''' variables at hand.
            ''' </remarks>
            Public Function Distance(ByVal other As Point) As System.Double

                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                ' No input checking.
                Return Math.D2.Distance(Me.X, Me.Y, other.X, other.Y)
            End Function ' Distance

            ''' <summary>
            ''' Converts the numeric value of this instance to its equivalent string
            ''' representation.
            ''' </summary>
            ''' <returns>
            ''' The string representation of the value of this instance.
            ''' </returns>
            Public Overrides Function ToString() As System.String
                Return String.Format("<{0}, {1}>", Me.X, Me.Y)
            End Function ' ToString

            ''' <summary>
            ''' Initializes a new instance of the <c>Point2D</c> class with the
            ''' specified coordinates.
            ''' </summary>
            Public Sub New(ByVal x As System.Double, ByVal y As System.Double)
                ' No input checking.
                Me.X = x
                Me.Y = y
            End Sub ' New

        End Class ' Point

        ''' <summary>
        ''' Computes the distance between two points in a 2D plane.
        ''' </summary>
        ''' <param name="x0">Specifies the X-coordinate of one point.</param>
        ''' <param name="y0">Specifies the Y-coordinate of one point.</param>
        ''' <param name="x1">Specifies the X-coordinate of the other
        ''' point.</param>
        ''' <param name="y1">Specifies the Y-coordinate of the other
        ''' point.</param>
        ''' <returns>The distance between the two points.</returns>
        ''' <remarks>t
        ''' <see cref="Math.D2.Distance(Double, Double, Double, Double)"/>,
        ''' <see cref="Math.D2.Distance(Math.D2.Point, Math.D2.Point)"/>,
        ''' and <see cref="OSNW.Math.D2.Point.Distance(OSNW.Math.D2.Point)"/>
        ''' are effectively the same thing. Use whichever best suits the
        ''' variables at hand.
        ''' </remarks>
        Public Shared Function Distance(ByVal x0 As System.Double,
            ByVal y0 As System.Double, ByVal x1 As System.Double,
            ByVal y1 As System.Double) As System.Double

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            Dim DeltaX As System.Double = x1 - x0
            Dim DeltaY As System.Double = y1 - y0
            Return System.Math.Sqrt((DeltaX * DeltaX) + (DeltaY * DeltaY))
        End Function ' Distance

        ''' <summary>
        ''' Computes the distance between two points in a 2D plane.
        ''' </summary>
        ''' <param name="p0">Specifies one point.</param>
        ''' <param name="p1">Specifies the other point.</param>
        ''' <returns>The distance between the two points.</returns>
        ''' <remarks>
        ''' <see cref="Math.D2.Distance(Double, Double, Double, Double)"/>,
        ''' <see cref="Math.D2.Distance(Math.D2.Point, Math.D2.Point)"/>, and
        ''' <see cref="OSNW.Math.D2.Point.Distance(OSNW.Math.D2.Point)"/>
        ''' are effectively the same thing. Use whichever best suits the
        ''' variables at hand.
        ''' </remarks>
        Public Shared Function Distance(ByVal p0 As Math.D2.Point,
            ByVal p1 As Math.D2.Point) As System.Double

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' No input checking.
            Return Math.D2.Distance(p0.X, p0.Y, p1.X, p1.Y)
        End Function ' Distance

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        ''' <param name="x0">xxxxxxxxxx</param>
        ''' <param name="y0">xxxxxxxxxx</param>
        ''' <param name="x1">xxxxxxxxxx</param>
        ''' <param name="y1"></param>
        ''' <returns>xxxxxxxxxx</returns>
        Public Shared Function Slope(ByVal x0 As System.Double,
            ByVal y0 As System.Double, ByVal x1 As System.Double,
            ByVal y1 As System.Double) As System.Double

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' No input checking.
            Return (y1 - y0) / (x1 - x0)
        End Function ' Slope

        ''' <summary>
        ''' xxxxxxxxxx
        ''' </summary>
        ''' <param name="x0">xxxxxxxxxx</param>
        ''' <param name="y0">xxxxxxxxxx</param>
        ''' <param name="x1">xxxxxxxxxx</param>
        ''' <param name="y1">xxxxxxxxxx</param>
        ''' <param name="m">xxxxxxxxxx</param>
        ''' <param name="b">xxxxxxxxxx</param>
        Public Shared Sub GetLineEq(ByVal x0 As System.Double,
            ByVal y0 As System.Double, ByVal x1 As System.Double,
            ByVal y1 As System.Double, ByRef m As System.Double,
            ByRef b As System.Double)

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' No input checking.

            ' Get the equation for the line.
            ' Y = M*X + B; Standard form line.
            ' B = Y - M*X; Solve for the Y-intercept.
            m = Slope(x0, x1, y0, y1)
            b = y0 - m * x0
        End Sub ' GetLineEq

    End Structure ' D2

    Partial Public Structure D3

        ''' <summary>
        ''' Computes the distance between two points in a 3D space.
        ''' </summary>
        ''' <param name="x0">Specifies the X-coordinate of one point.</param>
        ''' <param name="y0">Specifies the Y-coordinate of one point.</param>
        ''' <param name="z0">Specifies the Z-coordinate of one point.</param>
        ''' <param name="x1">Specifies the X-coordinate of the other
        ''' point.</param>
        ''' <param name="y1">Specifies the Y-coordinate of the other
        ''' point.</param>
        ''' <param name="z1">Specifies the Z-coordinate of the other
        ''' point.</param>
        ''' <returns>The distance between the two points.</returns>
        ''' <remarks>
        ''' <see cref="D3.Distance(Double, Double, Double, Double, Double,
        ''' Double)"/>,
        ''' <see cref="D3.Distance(D3.Point, D3.Point)"/>,
        ''' and <see cref="OSNW.D3.Point.Distance(OSNW.D3.Point)"/>
        ''' are effectively the same thing. Use whichever best suits the
        ''' variables at hand.
        ''' </remarks>
        Public Shared Function Distance(ByVal x0 As System.Double,
            ByVal y0 As System.Double, ByVal z0 As System.Double,
            ByVal x1 As System.Double, ByVal y1 As System.Double,
            ByVal z1 As System.Double) As System.Double

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            ' Based on the Pythagorean theorem.
            Dim DeltaX As System.Double = x1 - x0
            Dim DeltaY As System.Double = y1 - y0
            Dim DeltaZ As System.Double = z1 - z0
            Return System.Math.Sqrt(
                (DeltaX * DeltaX) + (DeltaY * DeltaY) + (DeltaZ * DeltaZ))
        End Function ' Distance

        ''' <summary>
        ''' Represents an ordered triplet of X, Y and Z double precision coordinates
        ''' that define a point in a three-dimensional space.
        ''' </summary>
        Public Class Point

            ''' <summary>
            ''' Represents the X-coordinate of the current instance.
            ''' </summary>
            Public X As System.Double

            ''' <summary>
            ''' Represents the Y-coordinate of the current instance.
            ''' </summary>
            Public Y As System.Double

            ''' <summary>
            ''' Represents the Z-coordinate of the current instance.
            ''' </summary>
            Public Z As System.Double

            ''' <summary>
            ''' Initializes a new instance of the <c>Point3D</c> class with the
            ''' specified coordinates.
            ''' </summary>
            Public Sub New(ByVal x As System.Double, ByVal y As System.Double,
                           ByVal z As System.Double)
                ' No input checking.
                Me.X = x
                Me.Y = y
                Me.Z = z
            End Sub ' New

            ''' <summary>
            ''' Computes the distance between the current instance and another
            ''' <c>Point3D</c> in a 3D space.
            ''' </summary>
            ''' <param name="other">Specifies a distant <c>Point3D</c>.</param>
            ''' <returns>The distance between the two points.</returns>
            ''' <remarks>
            ''' <see cref="Math.D3.Distance(Double, Double, Double, Double,
            ''' Double, Double)"/>,
            ''' <see cref="Math.D3.Distance(Math.D3.Point, Math.D3.Point)"/>, and
            ''' <see cref="OSNW.Math.D3.Point.Distance(OSNW.Math.D3.Point)"/>
            ''' are effectively the same thing. Use whichever best suits the
            ''' variables at hand.
            ''' </remarks>
            Public Function Distance(ByVal other As Point) As System.Double
                ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

                Return Math.D3.Distance(Me.X, Me.Y, Me.Z,
                                        other.X, other.Y, other.Z)
            End Function ' Distance

            ''' <summary>
            ''' Converts the numeric value of this instance to its equivalent string
            ''' representation.
            ''' </summary>
            ''' <returns>
            ''' The string representation of the value of this instance.
            ''' </returns>
            Public Overrides Function ToString() As System.String
                Return System.String.Format("<{0}, {1}, {2}>", Me.X, Me.Y, Me.Z)
            End Function ' ToString

        End Class ' Point

        ''' <summary>
        ''' Computes the distance between two points in a 3D space.
        ''' </summary>
        ''' <param name="p0">Specifies one point.</param>
        ''' <param name="p1">Specifies the other point.</param>
        ''' <returns>The distance between the two points.</returns>
        ''' <remarks>
        ''' <see cref="Math.D3.Distance(Double, Double, Double, Double, Double,
        ''' Double)"/>,
        ''' <see cref="Math.D3.Distance(Math.D3.Point, Math.D3.Point)"/>,
        ''' and <see cref="OSNW.Math.D3.Point.Distance(OSNW.Math.D3.Point)"/>
        ''' are effectively the same thing. Use whichever best suits the
        ''' variables at hand.
        ''' </remarks>
        Public Shared Function Distance(ByVal p0 As Math.D3.Point,
            ByVal p1 As Math.D3.Point) As System.Double

            ' xxxxxxxxxx NO TEST HAS BEEN ADDED FOR THIS. xxxxxxxxxx

            Return Math.D3.Distance(p0.X, p0.Y, p0.Z, p1.X, p1.Y, p1.Z)
        End Function ' Distance

    End Structure ' D3

End Module ' Math
