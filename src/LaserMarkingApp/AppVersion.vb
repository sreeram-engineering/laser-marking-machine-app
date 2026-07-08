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
            Dim currentAssembly = Assembly.GetExecutingAssembly()
            Dim attribute = currentAssembly.GetCustomAttribute(Of AssemblyInformationalVersionAttribute)()
            If attribute Is Nothing OrElse String.IsNullOrWhiteSpace(attribute.InformationalVersion) Then
                Return currentAssembly.GetName().Version.ToString()
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
