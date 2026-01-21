Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

'''' <summary>
'''' Represents an ordered pair of X and Y double precision coordinates that
'''' define a point in a two-dimensional plane.
'''' </summary>
'''' <remarks>
'''' This is a very simplified implementation of a double precision version
'''' of <see cref="System.Drawing.PointF"/>. In almost any practical case, there
'''' is no significant difference between <c>PointD</c> and <c>PointF</c> on a
'''' monitor or printer. Double precision values are only used here to minimize
'''' any impact of doing calculations with floating point values.
'''' </remarks>
'Public Structure PointD

'    ''' <summary>
'    ''' Represents the X-coordinate of this <see cref='OSNW.Numerics.PointD'/>.
'    ''' </summary>
'    Public X As System.Double

'    ''' <summary>
'    ''' Represents the Y-coordinate of this <see cref='OSNW.Numerics.PointD'/>.
'    ''' </summary>
'    Public Y As System.Double

'    ''' <summary>
'    ''' Initializes a New instance of the <see cref="OSNW.Numerics.PointD"/>
'    ''' class with the specified coordinates.
'    ''' </summary>
'    Public Sub New(ByVal x As System.Double, ByVal y As System.Double)
'        ' No input checking.
'        Me.X = x
'        Me.Y = y
'    End Sub ' New

'    Public Overrides Function ToString() As System.String
'        Return String.Format("({0}, {1})", Me.X, Me.Y)
'    End Function ' ToString

'End Structure ' PointD
