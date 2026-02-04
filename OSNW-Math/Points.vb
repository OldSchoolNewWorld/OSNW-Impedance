Partial Public Module Math

    Public Structure Math2D

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
        ''' <remarks>
        ''' <see cref="OSNW.Math2D.Distance(Double, Double, Double, Double)"/>
        ''' and  <see cref="OSNW.Math2D.Point.Distance(OSNW.Math2D.Point)"/> are
        ''' effectively the same thing. Use whichever best suits the variables
        ''' at hand.
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

            ''' <summary>
            ''' Initializes a new instance of the <c>Point2D</c> class with the
            ''' specified coordinates.
            ''' </summary>
            Public Sub New(ByVal x As System.Double, ByVal y As System.Double)
                ' No input checking.
                Me.X = x
                Me.Y = y
            End Sub ' New

            ''' <summary>
            ''' Computes the distance between the current instance and another
            ''' <c>Point2D</c> in a 2D plane.
            ''' </summary>
            ''' <param name="other">Specifies a distant <c>Point2D</c>.</param>
            ''' <returns>The distance between the two points.</returns>
            ''' <remarks>
            ''' <see cref="OSNW.Math2D.Distance(Double, Double, Double,
            ''' Double)"/> and
            ''' <see cref="OSNW.Math2D.Point.Distance(OSNW.Math2D.Point)"/> are
            ''' effectively the same thing. Use whichever best suits the
            ''' variables at hand.
            ''' </remarks>
            Public Function Distance(ByVal other As Point) As System.Double
                Return Math2D.Distance(Me.X, Me.Y, other.X, other.Y)
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

        End Class ' Point

    End Structure ' Math2D

    Public Structure Math3D

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
        ''' <see cref="OSNW.Math3D.Distance(Double, Double, Double, Double,
        ''' Double, Double)"/> and
        ''' <see cref="OSNW.Math3D.Point.Distance(OSNW.Math3D.Point)"/> are
        ''' effectively the same thing. Use whichever best suits the variables
        ''' at hand.
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
            ''' Initializes a New instance of the <c>Point3D</c> class with the
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
            ''' <see cref="OSNW.Math3D.Distance(Double, Double, Double, Double,
            ''' Double, Double)"/> and
            ''' <see cref="OSNW.Math3D.Point.Distance(OSNW.Math3D.Point)"/>
            ''' are effectively the same thing. Use whichever best suits the
            ''' variables at hand.
            ''' </remarks>
            ''' 
            Public Function Distance(ByVal other As Point) As System.Double
                Return Math3D.Distance(Me.X, Me.Y, Me.Z,
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

    End Structure ' Math3D

End Module ' Math
