' Option Explicit On
' Option Strict On

Imports System.Data.OleDb

Module ModCenter

    Public con As New OleDbConnection
    Public da As OleDbDataAdapter
    Public cmd As OleDbCommand
    Public sql As String

    Public strLang As String = "VB.NET 2022"
    Public strAnd As String = Chr(38) ' &
    Public strSpace As String = Chr(32) ' Blank space
    Public dbName As String = "Access"

    Public mainProjectName As String = "Dormitory Management System."
    Public subProjectName As String = "Login System with User Authentication."

    Public appTitle1 As String = String.Format("{0} {1}", strLang, mainProjectName)
    Public appTitl1WithDb As String = String.Format("{0} {1} {2} {3}", strLang, strAnd, dbName, mainProjectName)

    Public appTitle2 As String = String.Format("{0} {1}", strLang, subProjectName)
    Public appTitle2WithDb As String = String.Format("{0} {1} {2} {3}", strLang, strAnd, dbName, subProjectName)

    Public conState As Boolean = False

    Public currentUserID As String = ""
    Public currentUsername As String = ""
    Public currentPermission As String = ""

    Public Sub ManageConnection()

        conState = False

        Try

            Dim conString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & Application.StartupPath

            conString &= "\RoleManageDatabase.accdb"

            If con.State = ConnectionState.Open Then
                con.Close()
            End If

            con.ConnectionString = conString
            con.Open()

            conState = True

            ' MessageBox.Show("Connected to Microsoft Access Database.", appTitle1,
            ' MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            conState = False

            MessageBox.Show("Error Database Connection: " & ex.Message, appTitl1WithDb,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)

            Return

        End Try

    End Sub

    Public Function DisplayData(ByVal str As String, ByVal tbl As String, ByVal myDS As DataSet) As DataSet

        If conState = False Then
            ManageConnection()
        End If

        Dim ds As New DataSet

        Try

            ds.Clear()
            da = New OleDbDataAdapter(str, con)
            da.Fill(myDS, tbl)

            ds = myDS

        Catch ex As Exception
            ds = Nothing
            conState = False
            MessageBox.Show("Error Displaying Data: " & ex.Message, appTitl1WithDb,
                            MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conState = False
            con.Close()
        End Try

        DisplayData = ds

    End Function

    Public Function ExecuteDb(ByVal mySQL As String) As Boolean

        Dim bCheck As Boolean = False

        If conState = False Then
            ManageConnection()
        End If

        Try

            cmd = New OleDbCommand(mySQL, con)
            cmd.CommandTimeout = 600
            cmd.ExecuteNonQuery()

            bCheck = True

        Catch ex As Exception
            bCheck = False
            MessageBox.Show("Error: " & ex.Message, appTitl1WithDb, MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            conState = False
            con.Close()
        End Try

        ExecuteDb = bCheck

    End Function

End Module
