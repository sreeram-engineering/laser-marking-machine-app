Imports System.Reflection

Public NotInheritable Class AppVersion
    Private Sub New()
    End Sub

    Public Shared ReadOnly Property ProductName As String
        Get
            Return "Laser Marking QR App"
        End Get
    End Property

    Public Shared ReadOnly Property InformationalVersion As String
        Get
            Dim assembly = Assembly.GetExecutingAssembly()
            Dim attribute = assembly.GetCustomAttribute(Of AssemblyInformationalVersionAttribute)()
            If attribute Is Nothing OrElse String.IsNullOrWhiteSpace(attribute.InformationalVersion) Then
                Return assembly.GetName().Version.ToString()
            End If

            Return attribute.InformationalVersion
        End Get
    End Property

    Public Shared ReadOnly Property DisplayVersion As String
        Get
            Return $"Version {InformationalVersion}"
        End Get
    End Property
End Class
