Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off




' REF: Mathematical Construction and Properties of the Smith Chart
' https://www.allaboutcircuits.com/technical-articles/mathematical-construction-and-properties-of-the-smith-chart/

' REF: Reflection and Transmission Coefficients Explained
' https://www.rfwireless-world.com/terminology/reflection-and-transmission-coefficients




''' <summary>
''' Represents a base class to define a generic circle, with a center and
''' radius, for use on a Cartesian grid. Dimensions are in generic "units".
''' </summary>
Public Class GenericCircle

    Private m_GridCenterX As System.Double
    ''' <summary>
    ''' Represents the X-coordinate of the center of the <c>GenericCircle</c>,
    ''' on a Cartesian grid. Dimensions are in generic "units".
    ''' </summary>
    Public Property GridCenterX As System.Double
        Get
            Return Me.m_GridCenterX
        End Get
        Set(value As System.Double)
            Me.m_GridCenterX = value
        End Set
    End Property

    Private m_GridCenterY As System.Double
    ''' <summary>
    ''' Represents the Y-coordinate of the center of the <c>GenericCircle</c>,
    ''' on a Cartesian grid. Dimensions are in generic "units".
    ''' </summary>
    Public Property GridCenterY As System.Double
        Get
            Return Me.m_GridCenterY
        End Get
        Set(value As System.Double)
            Me.m_GridCenterY = value
        End Set
    End Property

    Private m_GridRadius As System.Double
    ''' <summary>
    ''' Represents the radius of the <c>GenericCircle</c>, on a Cartesian grid.
    ''' Dimensions are in generic "units".
    ''' </summary>
    Public Property GridRadius As System.Double
        Get
            Return Me.m_GridRadius
        End Get
        Set(value As System.Double)

            ' Input checking.
            ' A zero value is useless, but possibly valid.
            If value < 0.0 Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                    NameOf(value), Impedance.MSGCHNV)
            End If

            Me.m_GridRadius = value

        End Set
    End Property

    ''' <summary>
    ''' Represents the diameter of the <c>GenericCircle</c>, on a Cartesian
    ''' grid. Dimensions are in generic "units".
    ''' </summary>
    Public Property GridDiameter As System.Double
        Get
            Return Me.GridRadius * 2.0
        End Get
        Set(value As System.Double)

            ' Input checking.
            ' A zero value is useless, but possibly valid.
            If value < 0.0 Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                    NameOf(value), Impedance.MSGCHNV)
            End If

            Me.GridRadius = value / 2.0

        End Set
    End Property

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <remarks>
    ''' This is required to allow inheritance.
    ''' </remarks>
    Public Sub New()
        With Me
            '.m_GridCenterX = 0.0
            '.m_GridCenterY = 0.0
            .m_GridRadius = 1.0 ' Default to a unit circle.
        End With
    End Sub ' New

    ''' <summary>
    ''' Creates a new instance of the <c>GenericCircle</c> class with the
    ''' specified center coordinates and radius.
    ''' </summary>
    ''' <param name="gridCenterX"> Specifies the X-coordinate of the center of
    ''' the <c>GenericCircle</c>, on a Cartesian grid. Dimensions are in generic
    ''' "units".</param>
    ''' <param name="gridCenterY"> Specifies the Y-coordinate of the center of
    ''' the <c>GenericCircle</c>, on a Cartesian grid. Dimensions are in generic
    ''' "units".</param>
    ''' <param name="gridRadius">Specifies the radius of the
    ''' <c>GenericCircle</c>, on a Cartesian grid. Dimensions are in generic
    ''' "units".</param>
    Public Sub New(ByVal gridCenterX As System.Double,
                   ByVal gridCenterY As System.Double,
                   ByVal gridRadius As System.Double)

        ' Input checking.
        ' A zero value is useless, but possibly valid.
        If gridRadius < 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                    NameOf(gridRadius), Impedance.MSGCHNV)
        End If

        With Me
            .m_GridCenterX = gridCenterX
            .m_GridCenterY = gridCenterY
            .m_GridRadius = gridRadius
        End With

    End Sub ' New

End Class ' GenericCircle

''' <summary>
''' xxxxxxxxxx
''' </summary>
Public Class SmithMainCircle
    Inherits GenericCircle

    Private m_Z0 As System.Double
    ''' <summary>
    ''' Represents the characteristic impedance of the <c>SmithMainCircle</c> in
    ''' ohms.
    ''' </summary>
    ''' <remarks>This Z0 is a common reference for all associated circles,
    ''' individual impedances, etc.</remarks>
    Public Property Z0 As System.Double
        Get
            Return Me.m_Z0
        End Get
        Set(value As System.Double)

            ' Input checking.
            If value <= 0.0 Then
                'Dim CaughtBy As System.Reflection.MethodBase =
                '    System.Reflection.MethodBase.GetCurrentMethod
                Throw New System.ArgumentOutOfRangeException(
                    NameOf(value), Impedance.MSGVMBGTZ)
            End If

            Me.m_Z0 = value

        End Set
    End Property

    ''' <summary>
    ''' Represents the characteristic admittance of the <c>SmithMainCircle</c>
    ''' in siemens.
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>This Y0 is a common reference for all associated circles,
    ''' individual conductances, etc.</remarks>
    Public ReadOnly Property Y0 As System.Double
        Get
            Return 1.0 / Me.Z0
        End Get
    End Property

    ''' <summary>
    ''' Returns the Cartesian X-coordinate of the leftmost edge of the current
    ''' <c>SmithMainCircle</c>. Dimensions are in generic "units".
    ''' </summary>
    Public ReadOnly Property GridLeftEdgeX As System.Double
        Get
            Return Me.GridCenterX - Me.GridRadius
        End Get
    End Property

    ''' <summary>
    ''' Returns the Cartesian X-coordinate of the rightmost edge of the current
    ''' <c>SmithMainCircle</c>. Dimensions are in generic "units".
    ''' </summary>
    Public ReadOnly Property GridRightEdgeX As System.Double
        Get
            Return Me.GridCenterX + Me.GridRadius
        End Get
    End Property

    ''' <summary>
    ''' Calculates the radius of a R- or X-circle associated with the current
    ''' instance for a <paramref name="resistance"/> specified in ohms.
    ''' </summary>
    ''' <param name="resistance">Specifies the resistance in ohms.</param>
    ''' <returns>The radius of the R- or X-circle in generic "units".</returns>
    Public Function RadiusRX(ByVal resistance As System.Double) As System.Double

        'Dim NormR As System.Double = resistance / Me.Z0
        'Dim ScaleDia As System.Double = 1 / (NormR + 1)
        'Dim AnsRad As System.Double = Me.GridRadius * ScaleDia
        'Return AnsRad

        Return Me.GridRadius * (1 / ((resistance / Me.Z0) + 1))

    End Function ' RadiusRX

    ''' <summary>
    ''' Calculates the radius of a G- or B-circle associated with the current
    ''' instance for a <paramref name="conductance"/> specified in ohms.
    ''' <paramref name="conductance"/> in ohms.
    ''' </summary>
    ''' <param name="conductance">Specifies the conductance in ohms.</param>
    ''' <returns>The radius of the G- or B-circle in generic "units".</returns>
    Public Function RadiusGY(ByVal conductance As System.Double) As System.Double
        ' Derived like RadiusRX.
        Return Me.GridRadius * (1 / ((conductance / Me.Y0) + 1))
    End Function ' RadiusGY

    ''' <summary>
    ''' Creates a new instance of the <c>SmithMainCircle</c> class with the
    ''' specified center coordinates, diameter, and characteristic impedance.
    ''' Dimensions are in generic "units".
    ''' </summary>
    ''' <param name="gridCenterX">Specifies the X-coordinate of the center of
    ''' the circle in generic "units".</param>
    ''' <param name="gridCenterY">Specifies the Y-coordinate of the center of
    ''' the circle in generic "units".</param>
    ''' <param name="gridDiameter">Specifies the diameter of the circle in
    ''' generic "units".</param>
    ''' <param name="z0">Specifies the characteristic impedance of the
    ''' <c>SmithMainCircle</c> in ohms.</param>
    Public Sub New(ByVal gridCenterX As System.Double,
                   ByVal gridCenterY As System.Double,
                   ByVal gridDiameter As System.Double,
                   ByVal z0 As System.Double)

        ' Input checking.
        If gridDiameter <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(gridDiameter), Impedance.MSGVMBGTZ)
        End If
        If z0 <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(NameOf(z0), Impedance.MSGVMBGTZ)
        End If

        With Me
            .GridCenterX = gridCenterX
            .GridCenterY = gridCenterY
            .GridDiameter = gridDiameter
            .m_Z0 = z0
        End With

    End Sub ' New

End Class ' SmithMainCircle

''' <summary>
''' xxxxxxxxxx
''' </summary>
Public Class RCircle
    Inherits GenericCircle

    Private ReadOnly m_MainCircle As SmithMainCircle
    ''' <summary>
    ''' Specifies the <see cref="SmithMainCircle"/> with which this instance is
    ''' associated.
    ''' </summary>
    Public ReadOnly Property MainCircle As SmithMainCircle
        Get
            Return Me.m_MainCircle
        End Get
    End Property

    Private ReadOnly m_Resistance As System.Double
    ''' <summary>
    ''' xxxxxxxxxx in ohms.
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    Public ReadOnly Property Resistance As System.Double
        Get
            Return Me.m_Resistance
        End Get
    End Property

    ''' <summary>
    ''' Attempts to generate a set of values describing the geometry of the
    ''' <c>RCircle</c>, on the associated <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <returns>Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns a set of values describing the R
    ''' circle.</returns>
    Public Function TryGetCircleBasics(ByRef gridCenterX As System.Double,
        ByRef gridCenterY As System.Double, ByRef gridRadius As System.Double) _
        As System.Boolean

        ' By observation,
        '     FracL = NormReal * FracR
        '     FracL + FracR = 1
        '         FracL = 1 - FracR

        ' Derivation:
        ' FracL = NormReal * FracR
        ' 1 - FracR = NormReal * FracR
        ' (1/FracR) - (FracR/FracR) = NormReal
        ' (1/FracR) - (FracR/FracR) = NormReal
        ' (1/FracR) - 1 = NormReal
        ' (1/FracR) = NormReal + 1
        ' FracR = 1/(NormReal + 1)

        ' Calculate values relative to the host outer circle.
        ' Then populate values relative to the Cartesian grid.
        With Me
            '            Dim NormReal As System.Double = .Resistance / .MainCircle.Z0
            '            Dim FracR As System.Double = 1.0 / (NormReal + 1.0)
            '            gridRadius = FracR * .MainCircle.GridRadius
            '            gridRadius = (1.0 / (NormReal + 1.0)) * .MainCircle.GridRadius
            '            gridRadius = .MainCircle.GridRadius / (NormReal + 1.0)
            .GridRadius = .MainCircle.GridRadius / ((.Resistance / .MainCircle.Z0) + 1.0)
            .GridCenterX = .MainCircle.GridRightEdgeX - .GridRadius
            .GridCenterY = .MainCircle.GridCenterY
        End With

        Return True

    End Function ' TryGetCircleBasics

    ''' <summary>
    ''' Sets the Cartesian coordinates and radius of the <c>RCircle</c> based on
    ''' its conductance and the values in the associated
    ''' <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <remarks>
    ''' This method is intended to be called after the circle has been
    ''' constructed, to set the basic properties.
    ''' </remarks>
    Public Sub SetCircleBasics()

        Dim GridCenterX As System.Double
        Dim GridCenterY As System.Double
        Dim GridRadius As System.Double

        If Not Me.TryGetCircleBasics(GridCenterX, GridCenterY, GridRadius) Then
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.InvalidOperationException(
                $"Failed to process {CaughtBy}.")
        End If

        With Me
            .GridCenterX = GridCenterX
            .GridCenterY = GridCenterY
            .GridRadius = GridRadius
        End With

    End Sub ' SetCircleBasics

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCircle">Specifies the
    ''' <see cref="SmithMainCircle"></see> with which the circle is
    ''' associated.</param>
    ''' <param name="resistance">xxxxxxxxxx in ohms.</param>
    Public Sub New(ByVal mainCircle As SmithMainCircle,
                   ByVal resistance As System.Double)

        MyBase.New()

        ' Input checking.
        If resistance <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(resistance), Impedance.MSGVMBGTZ)
        End If

        Me.m_MainCircle = mainCircle
        Me.m_Resistance = resistance

    End Sub ' New

End Class ' RCircle

''' <summary>
''' xxxxxxxxxx
''' </summary>
Public Class XCircle
    Inherits GenericCircle

    Private ReadOnly m_MainCircle As SmithMainCircle
    ''' <summary>
    ''' Specifies the <see cref="SmithMainCircle"/> with which this instance is
    ''' associated.
    ''' </summary>
    Public ReadOnly Property MainCircle As SmithMainCircle
        Get
            Return Me.m_MainCircle
        End Get
    End Property

    Private ReadOnly m_Reactance As System.Double
    ''' <summary>
    ''' xxxxxxxxxx in ohms.
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    Public ReadOnly Property Reactance As System.Double
        Get
            Return Me.m_Reactance
        End Get
    End Property

    ''' <summary>
    ''' Attempts to generate a set of values describing the geometry of the
    ''' <c>XCircle</c>, on the associated <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <returns>Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns a set of values describing the X
    ''' circle.</returns>
    Public Function TryGetCircleBasics(ByRef gridCenterX As System.Double,
        ByRef gridCenterY As System.Double, ByRef gridRadius As System.Double) _
        As System.Boolean

        ' By observation, this is similar to RCircle.TryGetCircleBasics.

        ' Calculate values relative to the host outer circle.
        ' Then populate values relative to the Cartesian grid.
        With Me
            .GridRadius = .MainCircle.GridRadius / ((.Reactance / .MainCircle.Z0) + 1.0)
            .GridCenterX = .MainCircle.GridRightEdgeX
            .GridCenterY = If(.Reactance < 0.0,
                .MainCircle.GridCenterY - System.Math.Abs(.GridRadius),
                .MainCircle.GridCenterY + System.Math.Abs(.GridRadius))
        End With

        Return True

    End Function ' TryGetCircleBasics

    ''' <summary>
    ''' Sets the Cartesian coordinates and radius of the <c>XCircle</c> based on
    ''' its reactance and the values in the associated
    ''' <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <remarks>
    ''' This method is intended to be called after the circle has been
    ''' constructed, to set the basic properties.
    ''' </remarks>
    Public Sub SetCircleBasics()

        Dim GridCenterX As System.Double
        Dim GridCenterY As System.Double
        Dim GridRadius As System.Double

        If Not Me.TryGetCircleBasics(GridCenterX, GridCenterY, GridRadius) Then
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.InvalidOperationException(
                $"Failed to process {CaughtBy}.")
        End If

        With Me
            .GridCenterX = GridCenterX
            .GridCenterY = GridCenterY
            .GridRadius = GridRadius
        End With

    End Sub ' SetCircleBasics

    ''' <summary>
    ''' xxxxxxxxxx in ohms.
    ''' </summary>
    ''' <param name="mainCircle">Specifies the
    ''' <see cref="SmithMainCircle"></see> with which the circle is
    ''' associated.</param>
    ''' <param name="reactance">xxxxxxxxxx</param>
    Public Sub New(ByVal mainCircle As SmithMainCircle,
                   ByVal reactance As System.Double)

        MyBase.New()
        Me.m_MainCircle = mainCircle
        Me.m_Reactance = reactance
    End Sub ' New

End Class ' XCircle

''' <summary>
''' xxxxxxxxxx
''' </summary>
Public Class GCircle
    Inherits GenericCircle

    Private ReadOnly m_MainCircle As SmithMainCircle
    ''' <summary>
    ''' Specifies the <see cref="SmithMainCircle"/> with which this instance is
    ''' associated.
    ''' </summary>
    Public ReadOnly Property MainCircle As SmithMainCircle
        Get
            Return Me.m_MainCircle
        End Get
    End Property

    Private ReadOnly m_Conductance As System.Double
    ''' <summary>
    ''' xxxxxxxxxx in siemens.
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    Public ReadOnly Property Conductance As System.Double
        Get
            Return Me.m_Conductance
        End Get
    End Property

    ''' <summary>
    ''' Attempts to generate a set of values describing the geometry of the
    ''' <c>GCircle</c>, on the associated <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <returns>Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns a set of values describing the G
    ''' circle.</returns>
    Public Function TryGetCircleBasics(ByRef gridCenterX As System.Double,
        ByRef gridCenterY As System.Double, ByRef gridRadius As System.Double) _
        As System.Boolean

        ' By observation, this is similar to RCircle.TryGetCircleBasics.

        ' Calculate values relative to the host outer circle.
        ' Then populate values relative to the Cartesian grid.
        With Me
            .GridRadius = .MainCircle.GridRadius / ((.Conductance / .MainCircle.Y0) + 1.0)
            .GridCenterX = .MainCircle.GridLeftEdgeX
            .GridCenterY = .MainCircle.GridCenterY
        End With

        Return True

    End Function ' TryGetCircleBasics

    ''' <summary>
    ''' Sets the Cartesian coordinates and radius of the <c>GCircle</c> based on
    ''' its conductance and the values in the associated
    ''' <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <remarks>
    ''' This method is intended to be called after the circle has been
    ''' constructed, to set the basic properties.
    ''' </remarks>
    Public Sub SetCircleBasics()

        Dim GridCenterX As System.Double
        Dim GridCenterY As System.Double
        Dim GridRadius As System.Double

        If Not Me.TryGetCircleBasics(GridCenterX, GridCenterY, GridRadius) Then
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.InvalidOperationException(
                $"Failed to process {CaughtBy}.")
        End If

        With Me
            .GridCenterX = GridCenterX
            .GridCenterY = GridCenterY
            .GridRadius = GridRadius
        End With

    End Sub ' SetCircleBasics

    ''' <summary>
    ''' xxxxxxxxxx in siemens.
    ''' </summary>
    ''' <param name="mainCircle">Specifies the
    ''' <see cref="SmithMainCircle"></see> with which the circle is
    ''' associated.</param>
    ''' <param name="conductance">xxxxxxxxxx in siemens.</param>
    Public Sub New(ByVal mainCircle As SmithMainCircle,
                   ByVal conductance As System.Double)

        MyBase.New()

        ' Input checking.
        If conductance <= 0.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(conductance), Impedance.MSGVMBGTZ)
        End If

        Me.m_MainCircle = mainCircle
        Me.m_Conductance = conductance

    End Sub ' New

End Class ' GCircle

''' <summary>
''' xxxxxxxxxx
''' </summary>
Public Class BCircle
    Inherits GenericCircle

    Private ReadOnly m_MainCircle As SmithMainCircle
    ''' <summary>
    ''' Specifies the <see cref="SmithMainCircle"/> with which this instance is
    ''' associated.
    ''' </summary>
    Public ReadOnly Property MainCircle As SmithMainCircle
        Get
            Return Me.m_MainCircle
        End Get
    End Property

    Private ReadOnly m_Susceptance As System.Double
    ''' <summary>
    ''' xxxxxxxxxx in siemens.
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    Public ReadOnly Property Susceptance As System.Double
        Get
            Return Me.m_Susceptance
        End Get
    End Property

    ''' <summary>
    ''' Attempts to generate a set of values describing the geometry of the
    ''' <c>GCircle</c>, on the associated <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <returns>Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns a set of values describing the G
    ''' circle.</returns>
    Public Function TryGetCircleBasics(ByRef gridCenterX As System.Double,
        ByRef gridCenterY As System.Double, ByRef gridRadius As System.Double) _
        As System.Boolean

        ' By observation, this is similar to RCircle.TryGetCircleBasics.

        ' Calculate values relative to the host outer circle.
        ' Then populate values relative to the Cartesian grid.
        With Me
            .GridRadius = .MainCircle.GridRadius / ((.Susceptance / .MainCircle.Y0) + 1.0)
            .GridCenterX = .MainCircle.GridLeftEdgeX
            .GridCenterY = If(.Susceptance < 0.0,
                .MainCircle.GridCenterY + System.Math.Abs(.GridRadius),
                .MainCircle.GridCenterY - System.Math.Abs(.GridRadius))
        End With

        Return True

    End Function ' TryGetCircleBasics

    ''' <summary>
    ''' Sets the Cartesian coordinates and radius of the <c>BCircle</c> based on
    ''' its susceptance and the values in the associated
    ''' <see cref="SmithMainCircle"/>.
    ''' xxxx
    ''' </summary>
    ''' <remarks>
    ''' This method is intended to be called after the circle has been
    ''' constructed, to set the basic properties.
    ''' </remarks>
    Public Sub SetCircleBasics()

        Dim GridCenterX As System.Double
        Dim GridCenterY As System.Double
        Dim GridRadius As System.Double

        If Not Me.TryGetCircleBasics(GridCenterX, GridCenterY, GridRadius) Then
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.InvalidOperationException(
                $"Failed to process {CaughtBy}.")
        End If

        With Me
            .GridCenterX = GridCenterX
            .GridCenterY = GridCenterY
            .GridRadius = GridRadius
        End With

    End Sub ' SetCircleBasics

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCircle">Specifies the
    ''' <see cref="SmithMainCircle"></see> with which the circle is
    ''' associated.</param>
    ''' <param name="susceptance">xxxxxxxxxx in siemens.</param>
    Public Sub New(ByVal mainCircle As SmithMainCircle,
                   ByVal susceptance As System.Double)

        MyBase.New()
        Me.m_MainCircle = mainCircle
        Me.m_Susceptance = susceptance
    End Sub ' New

End Class ' BCircle

''' <summary>
''' xxxxxxxxxx
''' </summary>
Public Class VCircle
    Inherits GenericCircle

    Private ReadOnly m_MainCircle As SmithMainCircle
    ''' <summary>
    ''' Specifies the <see cref="SmithMainCircle"/> with which this instance is
    ''' associated.
    ''' </summary>
    Public ReadOnly Property MainCircle As SmithMainCircle
        Get
            Return Me.m_MainCircle
        End Get
    End Property

    Private ReadOnly m_VSWR As System.Double
    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    Public ReadOnly Property VSWR As System.Double
        Get
            Return Me.m_VSWR
        End Get
    End Property

    ''' <summary>
    ''' Attempts to generate a set of values describing the geometry of the
    ''' <c>VCircle</c>, on the associated <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <returns>Returns <c>True</c> if the process succeeds; otherwise,
    ''' <c>False</c>. Also returns a set of values describing the VSWR
    ''' circle.</returns>
    Public Function TryGetCircleBasics(ByRef gridCenterX As System.Double,
        ByRef gridCenterY As System.Double, ByRef gridRadius As System.Double) _
        As System.Boolean

        ' By observation,
        '     The rightmost edge of the VSWR circle is at the leftmost edge of
        '     the R circle that has the same conductance magnitude as the VSWR
        '     magnitude.

        With Me

            ' First, calculate the radius of the R circle relative to the host
            ' outer circle.
            Dim RCircGridRadius As System.Double =
                Me.MainCircle.GridRadius / ((Me.VSWR / Me.MainCircle.Z0) + 1.0)

            ' Then populate values for the VSWR circle relative to the Cartesian
            ' grid.
            .GridRadius = .MainCircle.GridRadius - (RCircGridRadius * 2.0)
            .GridCenterX = .MainCircle.GridCenterX
            .GridCenterY = .MainCircle.GridCenterY

        End With
        Return True

    End Function ' TryGetCircleBasics

    ''' <summary>
    ''' Sets the Cartesian coordinates and radius of the <c>VCircle</c> based on
    ''' its VWSR and the values in the associated
    ''' <see cref="SmithMainCircle"/>.
    ''' </summary>
    ''' <remarks>
    ''' This method is intended to be called after the circle has been
    ''' constructed, to set the basic properties.
    ''' </remarks>
    Public Sub SetCircleBasics()

        Dim GridCenterX As System.Double
        Dim GridCenterY As System.Double
        Dim GridRadius As System.Double

        If Not Me.TryGetCircleBasics(GridCenterX, GridCenterY, GridRadius) Then
            Dim CaughtBy As System.Reflection.MethodBase =
                System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.InvalidOperationException(
                $"Failed to process {CaughtBy}.")
        End If

        With Me
            .GridCenterX = GridCenterX
            .GridCenterY = GridCenterY
            .GridRadius = GridRadius
        End With

    End Sub ' SetCircleBasics

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCircle">Specifies the
    ''' <see cref="SmithMainCircle"></see> with which the circle is
    ''' associated.</param>
    ''' <param name="vswr">xxxxxxxxxx</param>
    Public Sub New(ByVal mainCircle As SmithMainCircle,
                   ByVal vswr As System.Double)

        MyBase.New()

        ' Input checking.
        If vswr < 1.0 Then
            'Dim CaughtBy As System.Reflection.MethodBase =
            '    System.Reflection.MethodBase.GetCurrentMethod
            Throw New System.ArgumentOutOfRangeException(
                NameOf(vswr), Impedance.MSGVMBGTE1)
        End If

        Me.m_MainCircle = mainCircle
        Me.m_VSWR = vswr

    End Sub ' New

End Class ' VCircle
