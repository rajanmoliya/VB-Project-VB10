Option Explicit On
Option Strict On
' Option Infer On

Public Class UserForm

    Private selectedID As String = ""

    Private Sub UserForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ShowUserData()
    End Sub

    Private Sub CloseToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles CloseToolStripButton.Click
        Me.Close()
    End Sub

    Private Sub ClearToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles ClearToolStripButton.Click
        ClearControls()
    End Sub

    Private Sub ClearControls() ' Let's try.

        selectedID = ""

        UsernameTextBox.Text = ""
        PasswordTextBox.Text = ""

        If PermissionComboBox.Items.Count > 0 Then
            PermissionComboBox.SelectedIndex = 0
        End If

        ToolStripStatusLabel1.Text = "Status:"

    End Sub

    Private Sub AddToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles AddToolStripButton.Click
        SaveUserData("INSERT")
    End Sub

    Private Sub EditToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles EditToolStripButton.Click
        SaveUserData("UPDATE")
    End Sub

    Private Sub DeleteToolStripButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles DeleteToolStripButton.Click
        SaveUserData("DELETE")
    End Sub

    Private Sub ShowUserData()

        Me.Text = "User Management Form | Current logged-in user: " & currentUsername.ToUpper() &
            " (ID: " & currentUserID & ")" & " (Log-in as: " & currentPermission & ")"

        Dim ds1 As New DataSet()

        sql = "SELECT [AutoID], Username, [Password], [Permission] FROM tblUser ORDER BY [AutoID] ASC;"

        Try

            ds1 = DisplayData(sql, "tblUser", ds1)

            If ds1 Is Nothing Then
                Return
            End If

            If ds1.Tables("tblUser").Rows.Count > 0 Then
                DataGridView1.DataSource = ds1.Tables("tblUser")
            Else
                DataGridView1.DataSource = Nothing
            End If

            Dim dgv1 As DataGridView = DataGridView1

            If dgv1.RowCount > 0 Then

                dgv1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv1.SelectionMode = DataGridViewSelectionMode.FullRowSelect

                dgv1.Columns(0).HeaderText = "AutoID"
                dgv1.Columns(1).HeaderText = "Username"
                dgv1.Columns(2).HeaderText = "Password"
                dgv1.Columns(3).HeaderText = "Permission"

                dgv1.Columns(0).Visible = False

                If currentPermission.ToUpper() = "USER" Then

                    AddToolStripButton.Enabled = False
                    EditToolStripButton.Enabled = False
                    DeleteToolStripButton.Enabled = False
                    PasswordTextBox.UseSystemPasswordChar = True

                    dgv1.Columns(1).Width = 336
                    dgv1.Columns(2).Visible = False

                Else

                    AddToolStripButton.Enabled = True
                    EditToolStripButton.Enabled = True
                    DeleteToolStripButton.Enabled = True
                    PasswordTextBox.UseSystemPasswordChar = False

                    dgv1.Columns(1).Width = 168
                    dgv1.Columns(2).Visible = True
                    dgv1.Columns(2).Width = 168
                    dgv1.Columns(2).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter

                End If

                dgv1.Columns(1).DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
                dgv1.Columns(3).Visible = False
                dgv1.ClearSelection()

            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message.ToString(), "Error Message.",
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub

    Private Sub DataGridView1_CellMouseDoubleClick(ByVal sender As Object, ByVal e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseDoubleClick

        Try

            If e.RowIndex = -1 Then
                Exit Sub
            End If

            selectedID = DataGridView1.Rows(e.RowIndex).Cells(0).Value.ToString().Trim()

            ToolStripStatusLabel1.Text = "Selected ID: " & selectedID

            UsernameTextBox.Text = DataGridView1.Rows(e.RowIndex).Cells(1).Value.ToString().Trim()
            PasswordTextBox.Text = DataGridView1.Rows(e.RowIndex).Cells(2).Value.ToString().Trim()

            If Convert.ToString(DataGridView1.CurrentRow.Cells(3).Value).Trim().ToUpper() = "ADMIN" Then
                PermissionComboBox.SelectedIndex = 1 ' Admin
            Else
                PermissionComboBox.SelectedIndex = 2 ' User
            End If

        Catch ex As Exception

        End Try

    End Sub

    Private Sub SaveUserData(ByVal dbCommand As String)

        Try

            If dbCommand.ToUpper() = "UPDATE" Or dbCommand.ToUpper() = "DELETE" Then
                If selectedID = "" Then
                    MessageBox.Show("ID not found. Please select at least one item.", appTitle1,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If
            End If

            If dbCommand.ToUpper() = "INSERT" Or dbCommand.ToUpper() = "UPDATE" Then

                If UsernameTextBox.Text.Trim() = "" Then
                    MessageBox.Show("Please input Username.", appTitle1, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    UsernameTextBox.Focus()
                    Exit Sub
                End If

                If PasswordTextBox.Text.Trim() = "" Then
                    MessageBox.Show("Please input Password.", appTitle1, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    PasswordTextBox.Focus()
                    Exit Sub
                End If

            End If

            If dbCommand.ToUpper() <> "DELETE" Then

                If PermissionComboBox.SelectedIndex = 0 Then
                    MessageBox.Show("Please select the permission from the ComboBox.", appTitle1, MessageBoxButtons.OK, MessageBoxIcon.Information)

                    Return

                End If

            End If

            Dim intID As Integer

            Select Case dbCommand.ToUpper()

                Case "INSERT"

                    If MessageBox.Show("Do you want to add this new user?",
                                       "Add New User : iBasskung Tutorial.",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                       MessageBoxDefaultButton.Button2) = DialogResult.No Then

                        Exit Sub

                    End If

                    sql = "INSERT INTO tblUser([Username], [Password], [Permission]) "
                    sql &= " VALUES('" & UsernameTextBox.Text.Trim() & "', '" & PasswordTextBox.Text.Trim() & "', "
                    sql &= "'" & PermissionComboBox.SelectedItem.ToString.Trim() & "')"

                Case "UPDATE"

                    If MessageBox.Show("Do you want to update the selected record?", appTitle1, MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                        Exit Sub
                    End If

                    intID = Convert.ToInt32(selectedID.ToString().Trim())

                    sql = "UPDATE tblUser SET [Username] = '" & UsernameTextBox.Text.Trim() & "', "
                    sql &= "[Password] = '" & PasswordTextBox.Text.Trim() & "', "
                    sql &= "[Permission] = '" & PermissionComboBox.SelectedItem.ToString().Trim() & "' "
                    sql &= "WHERE [AutoID] = " & intID & ""

                    If currentUserID = selectedID Then
                        currentUsername = UsernameTextBox.Text.Trim()
                        currentPermission = PermissionComboBox.SelectedItem.ToString().Trim()
                    End If

                Case "DELETE"

                    If currentUserID = selectedID Then
                        MessageBox.Show("Error Deleting Record: " & "The user " & Chr(39) & currentUsername & Chr(39) &
                                        " Is currently logged in.", appTitle1, MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Exit Sub
                    End If

                    If MessageBox.Show("Do you want to delete the selected record?", appTitle1, MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                        Exit Sub
                    End If

                    intID = Convert.ToInt32(selectedID.ToString().Trim())

                    sql = "DELETE FROM tblUser WHERE [AutoID] = " & intID & ""

            End Select

            If ExecuteDb(sql) = True Then
                selectedID = ""
                MessageBox.Show("Your SQL " & dbCommand & " QUERY has been executed successfully.",
                                appTitl1WithDb, MessageBoxButtons.OK, MessageBoxIcon.Information)
                ShowUserData()
            End If

        Catch ex As Exception
            MessageBox.Show("Error: " & ex.Message.ToString(), appTitle1, MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try

    End Sub

End Class