Public Class MainForm

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ManageConnection()

        Try
            Dim login As Form = New LoginForm()
            login.ShowDialog()

            ShowPermission()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ShowPermission()
        Dim PermissionDetail As String

        If currentPermission.ToUpper() = "USER" Then
            PermissionDetail = " (Read Only)."
        Else
            PermissionDetail = " (Full Control)."
        End If

        RoleLabel.Visible = True
        RoleLabel.Text = "Current Username: " & currentUsername & " : Log-in as: " & currentPermission.ToUpper() & PermissionDetail

        UserLoginToolStripButton.Text = currentUsername

        Me.Text = String.Format("{0} {1} {2} {3} : ({5}) ", "Main Form :", strLang, strAnd, dbName, mainProjectName, subProjectName)

        ToolStripStatusLabel2.Text = "VB.NET and MS Access Database : Login System with User Authentication."
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub

    Private Sub LogoutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogoutToolStripMenuItem.Click

        Dim strText As String = "Are you sure you want to log out?"
        Dim strCaption As String = "VB.NET : Log Out."

        If MessageBox.Show(strText, strCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                           MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then
            Return
        End If

        currentUserID = ""
        currentUsername = ""
        currentPermission = ""

        Me.Text = String.Format("{0} {1} {2} {3} Database.", "Main Form :", strLang, strAnd, dbName)

        UserLoginToolStripButton.Text = "Logged-in User"

        RoleLabel.Text = "Logged-Out."
        ToolStripStatusLabel2.Text = "(Logged-Out)"

        MainForm_Load(sender, e)
    End Sub

    Private Sub UserManagementToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserManagementToolStripMenuItem.Click

        Try

            ' Hide()

            Dim user As Form = New UserForm()
            user.ShowDialog()

            'Show()

            ShowPermission()

        Catch ex As Exception

        End Try

    End Sub

    Private Sub WebsiteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebsiteToolStripMenuItem.Click
        Process.Start("https://www.google.com/")
    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        MessageBox.Show("Multi User Role Based Login System." & vbNewLine &
                        "Using VB.NET and MS Access.",
                        "Developed by Rajan Moliya",
                        MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub MainForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        Try

            If MessageBox.Show("Are you sure you want to exit the application?",
                               "Form Closing : Rajan Moliya",
                               MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                               MessageBoxDefaultButton.Button2) = Windows.Forms.DialogResult.No Then

                e.Cancel = True

            End If

            If con.State = ConnectionState.Open Then
                con.Close()
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub UserToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserToolStripButton.Click
        Try

            ' Hide()

            Dim user As Form = New UserForm()
            user.ShowDialog()

            'Show()

            ShowPermission()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub UserLoginToolStripButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserLoginToolStripButton.Click
        MessageBox.Show("User ID: " & currentUserID & vbNewLine & vbNewLine &
               "Username: " & currentUsername & vbNewLine & vbNewLine &
               "Permission: " & currentPermission,
               "VB.NET : Current logged in user.",
               MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
End Class
