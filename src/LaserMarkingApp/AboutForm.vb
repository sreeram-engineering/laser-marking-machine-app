Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class AboutForm
    Inherits Form

    Public Sub New()
        Text = "About"
        StartPosition = FormStartPosition.CenterParent
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        ShowInTaskbar = False
        TopMost = True
        ClientSize = New Size(360, 280)
        Font = New Font("Segoe UI", 10.0F, FontStyle.Regular, GraphicsUnit.Point)
        BackColor = SystemColors.Control

        Dim iconLabel = New Label With {
            .Text = "i",
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font("Segoe UI", 36.0F, FontStyle.Bold, GraphicsUnit.Point),
            .ForeColor = SystemColors.ControlText,
            .Location = New Point(150, 24),
            .Size = New Size(60, 60)
        }

        Dim titleLabel = New Label With {
            .Text = AppVersion.ProductName,
            .TextAlign = ContentAlignment.MiddleCenter,
            .Font = New Font(Font, FontStyle.Bold),
            .ForeColor = SystemColors.ControlText,
            .Location = New Point(24, 98),
            .Size = New Size(312, 30)
        }

        Dim versionLabel = New Label With {
            .Text = AppVersion.DisplayVersion,
            .TextAlign = ContentAlignment.MiddleCenter,
            .ForeColor = SystemColors.GrayText,
            .Location = New Point(24, 132),
            .Size = New Size(312, 26)
        }

        Dim buildLabel = New Label With {
            .Text = "Self-contained Windows build",
            .TextAlign = ContentAlignment.MiddleCenter,
            .ForeColor = SystemColors.ControlText,
            .Location = New Point(24, 178),
            .Size = New Size(312, 26)
        }

        Dim closeButton = New Button With {
            .Text = "Close",
            .DialogResult = DialogResult.OK,
            .Location = New Point(120, 226),
            .Size = New Size(120, 32)
        }

        AcceptButton = closeButton
        CancelButton = closeButton
        Controls.AddRange({iconLabel, titleLabel, versionLabel, buildLabel, closeButton})
    End Sub
End Class
