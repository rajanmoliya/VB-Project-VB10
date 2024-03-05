Public Class LoginForm

    Private Sub LoginForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            Me.Text = appTitle2
            Me.AcceptButton = LoginButton

            ToolStripStatusLabel2.Text = dbName
            PasswordTextBox.UseSystemPasswordChar = True

        Catch ex As Exception

        End Try
    End Sub

    Private Sub LoginButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoginButton.Click

        If UsernameTextBox.Text.Trim = "" Then
            MessageBox.Show("Please Input Username.", subProjectName, MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            UsernameTextBox.Focus()
            Exit Sub
        End If

        If PasswordTextBox.Text.Trim() = "" Then
            MessageBox.Show("Please Input Password.", subProjectName, MessageBoxButtons.OK,
                            MessageBoxIcon.Warning)
            PasswordTextBox.Focus()
            Exit Sub
        End If

        Dim ds1 As New DataSet()

        Try

            sql = "SELECT * FROM tblUSer WHERE [Username] = '" & UsernameTextBox.Text.Trim() & "' "
            sql &= "AND [Password] = '" & PasswordTextBox.Text.Trim() & "' "

            ds1 = DisplayData(sql, "user", ds1)

            If ds1 Is Nothing Then
                MsgBox("ds1 was nothing.")
                Return
            End If

            If ds1.Tables("user").Rows.Count > 0 Then

                currentUserID = ds1.Tables("user").Rows(0)("AutoID").ToString().Trim()
                currentUsername = ds1.Tables("user").Rows(0)("Username").ToString().Trim()
                currentPermission = ds1.Tables("user").Rows(0)("Permission").ToString().Trim()

                Me.Close()

            Else

                MessageBox.Show("The Username Or Password is incorrect. Try again.",
                                appTitle2, MessageBoxButtons.OK, MessageBoxIcon.Error)

                UsernameTextBox.Clear()
                PasswordTextBox.Clear()
                UsernameTextBox.Focus()

                Exit Sub

            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message, appTitle2, MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try


    End Sub

    Private Sub ExitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitButton.Click
        End
    End Sub
End Class