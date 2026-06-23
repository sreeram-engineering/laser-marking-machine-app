Imports System
Imports System.IO

Public NotInheritable Class AppPaths
    Private Sub New()
    End Sub

    Public Shared ReadOnly Property DataDirectory As String
        Get
            Dim dataPath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "LaserMarkingApp")
            System.IO.Directory.CreateDirectory(dataPath)
            Return dataPath
        End Get
    End Property

    Public Shared ReadOnly Property DatabasePath As String
        Get
            Return System.IO.Path.Combine(DataDirectory, "laser_marking.db")
        End Get
    End Property
End Class
