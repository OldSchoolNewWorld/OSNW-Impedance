Option Explicit On
Option Strict On
Option Compare Binary
Option Infer Off

''' <summary>
''' Represents a base class to define a generic circle, with a center and
''' radius, for use on a Cartesian grid. Dimensions are in generic "units".
''' </summary>
Public Class GenericCircle

    Private m_CenterX As System.Double
    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public Property CenterX As System.Double
        Get
            Return Me.m_CenterX
        End Get
        Set(value As System.Double)
            Me.m_CenterX = value
        End Set
    End Property

    Private m_CenterY As System.Double
    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public Property CenterY As System.Double
        Get
            Return Me.m_CenterY
        End Get
        Set(value As System.Double)
            Me.m_CenterY = value
        End Set
    End Property

    Private m_Radius As System.Double
    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public Property Radius As System.Double
        Get
            Return Me.m_Radius
        End Get
        Set(value As System.Double)
            Me.m_Radius = value
        End Set
    End Property

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public Property Diameter As System.Double
        Get
            Return Me.m_Radius * 2.0
        End Get
        Set(value As System.Double)
            Me.m_Radius = value / 2.0
        End Set
    End Property

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <remarks>
    ''' This is included to allow inheritance.
    ''' </remarks>
    Public Sub New()
        With Me
            .m_CenterX = 0.0
            .m_CenterY = 0.0
            .m_Radius = 0.0
        End With
    End Sub ' New

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="centerX">xxxxxxxxxx</param>
    ''' <param name="centerY">xxxxxxxxxx</param>
    ''' <param name="diameter">xxxxxxxxxx</param>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public Sub New(ByVal centerX As System.Double,
                   ByVal centerY As System.Double,
                   ByVal diameter As System.Double)

        With Me
            .m_CenterX = centerX
            .m_CenterY = centerY
            .Diameter = diameter
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
    ''' xxxxxxxxxx in ohms.
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>This Z0 is a common reference for all associated circles,
    ''' individual impedances, etc.</remarks>
    Public Property Z0 As System.Double
        Get
            Return Me.m_Z0
        End Get
        Set(value As System.Double)
            Me.m_Z0 = value
        End Set
    End Property

    ''' <summary>
    ''' xxxxxxxxxx in siemens.
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
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public ReadOnly Property LeftEdgeX As System.Double
        Get
            Return Me.CenterX - Me.Radius
        End Get
    End Property

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <returns>xxxxxxxxxx</returns>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public ReadOnly Property RightEdgeX As System.Double
        Get
            Return Me.CenterX + Me.Radius
        End Get
    End Property

    ''' <summary>
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="centerX">xxxxxxxxxx in generic "units".</param>
    ''' <param name="centerY">xxxxxxxxxx in generic "units".</param>
    ''' <param name="diameter">xxxxxxxxxx in generic "units".</param>
    ''' <param name="z0">xxxxxxxxxx in ohms.</param>
    ''' <remarks>
    ''' Dimensions are in generic "units".
    ''' </remarks>
    Public Sub New(ByVal centerX As System.Double,
                   ByVal centerY As System.Double,
                   ByVal diameter As System.Double,
                   ByVal z0 As System.Double)

        With Me
            .CenterX = centerX
            .CenterY = centerY
            .Diameter = diameter
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
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCircle">xxxxxxxxxx</param>
    ''' <param name="resistance">xxxxxxxxxx in ohms.</param>
    Public Sub New(ByVal mainCircle As SmithMainCircle,
                   ByVal resistance As System.Double)

        MyBase.New()
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
    ''' xxxxxxxxxx in ohms.
    ''' </summary>
    ''' <param name="mainCircle">xxxxxxxxxx</param>
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
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCircle">xxxxxxxxxx</param>
    ''' <param name="conductance">xxxxxxxxxx in siemens.</param>
    Public Sub New(ByVal conductance As System.Double,
                   ByVal mainCircle As SmithMainCircle)

        MyBase.New()
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
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCircle">xxxxxxxxxx</param>
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
Public Class VSWRCircle
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
    ''' xxxxxxxxxx
    ''' </summary>
    ''' <param name="mainCircle">xxxxxxxxxx</param>
    ''' <param name="vswr">xxxxxxxxxx</param>
    Public Sub New(ByVal mainCircle As SmithMainCircle,
                   ByVal vswr As System.Double)

        MyBase.New()
        Me.m_MainCircle = mainCircle
        Me.m_VSWR = vswr
    End Sub ' New

End Class ' VSWRCircle
