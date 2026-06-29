Imports System
Imports System.Drawing
Imports System.Windows.Forms

Public Class UserManagementForm
    Inherits Form

    Private ReadOnly _database As DatabaseService
    Private ReadOnly _currentUser As UserRecord
    Private ReadOnly _usersCombo As ComboBox
    Private ReadOnly _usernameBox As TextBox
    Private ReadOnly _passwordBox As TextBox
    Private ReadOnly _confirmPasswordBox As TextBox
    Private ReadOnly _roleCombo As ComboBox
    Private ReadOnly _statusLabel As Label
    Private ReadOnly _deleteButton As Button

    Public Sub New(database As DatabaseService, currentUser As UserRecord)
        _database = database
        _currentUser = currentUser

        Text = "User Management"
        StartPosition = FormStartPosition.CenterParent
        FormBorderStyle = FormBorderStyle.FixedDialog
        MinimizeBox = False
        MaximizeBox = False
        ShowInTaskbar = False
        TopMost = True
        ClientSize = New Size(420, 326)

        Dim existingLabel = New Label With {.Text = "Existing User", .Location = New Point(24, 24), .AutoSize = True}
        _usersCombo = New ComboBox With {.Location = New Point(136, 20), .Width = 230, .DropDownStyle = ComboBoxStyle.DropDownList}

        Dim usernameLabel = New Label With {.Text = "Username", .Location = New Point(24, 68), .AutoSize = True}
        _usernameBox = New TextBox With {.Location = New Point(136, 64), .Width = 230}

        Dim passwordLabel = New Label With {.Text = "New Password", .Location = New Point(24, 108), .AutoSize = True}
        _passwordBox = New TextBox With {.Location = New Point(136, 104), .Width = 230, .UseSystemPasswordChar = True}

        Dim confirmPasswordLabel = New Label With {.Text = "Confirm", .Location = New Point(24, 148), .AutoSize = True}
        _confirmPasswordBox = New TextBox With {.Location = New Point(136, 144), .Width = 230, .UseSystemPasswordChar = True}

        Dim roleLabel = New Label With {.Text = "Role", .Location = New Point(24, 188), .AutoSize = True}
        _roleCombo = New ComboBox With {.Location = New Point(136, 184), .Width = 230, .DropDownStyle = ComboBoxStyle.DropDownList}
        _roleCombo.Items.Add(UserRole.OperatorUser)
        _roleCombo.Items.Add(UserRole.Setter)
        _roleCombo.Items.Add(UserRole.Admin)
        _roleCombo.SelectedItem = UserRole.OperatorUser

        _statusLabel = New Label With {.Location = New Point(24, 230), .Size = New Size(342, 24), .ForeColor = Color.DarkGreen}

        _deleteButton = New Button With {.Text = "Delete User", .Location = New Point(154, 266), .Size = New Size(102, 32), .Enabled = False}
        Dim saveButton = New Button With {.Text = "Save User", .Location = New Point(264, 266), .Size = New Size(102, 32)}
        Dim closeButton = New Button With {.Text = "Close", .Location = New Point(292, 298), .Size = New Size(74, 24), .DialogResult = DialogResult.Cancel}

        AddHandler _usersCombo.SelectedIndexChanged, AddressOf UsersCombo_SelectedIndexChanged
        AddHandler _deleteButton.Click, AddressOf DeleteButton_Click
        AddHandler saveButton.Click, AddressOf SaveButton_Click

        Controls.AddRange({
            existingLabel, _usersCombo, usernameLabel, _usernameBox, passwordLabel, _passwordBox,
            confirmPasswordLabel, _confirmPasswordBox, roleLabel, _roleCombo, _statusLabel, _deleteButton, saveButton, closeButton
        })

        LoadUsers()
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        BringToFront()
        Activate()
        _usernameBox.Focus()
    End Sub

    Private Sub LoadUsers()
        _usersCombo.Items.Clear()
        For Each user In _database.GetUsers()
            _usersCombo.Items.Add(user)
        Next
        _deleteButton.Enabled = False
    End Sub

    Private Sub UsersCombo_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim user = TryCast(_usersCombo.SelectedItem, UserRecord)
        If user Is Nothing Then
            _deleteButton.Enabled = False
            Return
        End If

        _usernameBox.Text = user.Username
        _roleCombo.SelectedItem = user.Role
        _passwordBox.Clear()
        _confirmPasswordBox.Clear()
        _deleteButton.Enabled = True
        _passwordBox.Focus()
    End Sub

    Private Sub DeleteButton_Click(sender As Object, e As EventArgs)
        Dim user = TryCast(_usersCombo.SelectedItem, UserRecord)
        If user Is Nothing OrElse user.Id <= 0 Then
            _statusLabel.ForeColor = Color.DarkRed
            _statusLabel.Text = "Select a saved user."
            Return
        End If

        If MessageBox.Show(Me, $"Delete user {user.Username}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If

        Try
            _database.SoftDeleteUser(user.Id, If(_currentUser Is Nothing, 0, _currentUser.Id))
            _usersCombo.SelectedItem = Nothing
            _usernameBox.Clear()
            _passwordBox.Clear()
            _confirmPasswordBox.Clear()
            _roleCombo.SelectedItem = UserRole.OperatorUser
            LoadUsers()
            _statusLabel.ForeColor = Color.DarkGreen
            _statusLabel.Text = "User deleted."
            _usernameBox.Focus()
        Catch ex As Exception
            _statusLabel.ForeColor = Color.DarkRed
            _statusLabel.Text = ex.Message
        End Try
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs)
        Try
            If _passwordBox.Text <> _confirmPasswordBox.Text Then
                _statusLabel.ForeColor = Color.DarkRed
                _statusLabel.Text = "Passwords do not match."
                _confirmPasswordBox.SelectAll()
                _confirmPasswordBox.Focus()
                Return
            End If

            Dim selectedRole = CType(_roleCombo.SelectedItem, UserRole)
            _database.SaveUser(_usernameBox.Text.Trim(), _passwordBox.Text, selectedRole)
            _statusLabel.ForeColor = Color.DarkGreen
            _statusLabel.Text = "User saved."
            _passwordBox.Clear()
            _confirmPasswordBox.Clear()
            LoadUsers()
        Catch ex As Exception
            _statusLabel.ForeColor = Color.DarkRed
            _statusLabel.Text = ex.Message
        End Try
    End Sub
End Class
