Public Class Usuarios
    Public Property UsuarioID As Integer
    Public Property NombreUsuario As String
    Public Property PasswordHash As String
    Public Property RolId As String

    Public Sub New()
    End Sub
    Public Function dtToUsuarios(dataTable As DataTable) As Usuarios
        If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
            Return Nothing
        End If

        Dim row As DataRow = dataTable.Rows(0)
        Dim usuario As New Usuarios With {
            .UsuarioID = Convert.ToInt32(row("UsuarioID")),
            .NombreUsuario = Convert.ToString(row("NombreUsuario")),
            .PasswordHash = Convert.ToString(row("PasswordHash")),
            .RolId = Convert.ToInt32(row("RolId"))
        }
        Return usuario
    End Function
End Class
