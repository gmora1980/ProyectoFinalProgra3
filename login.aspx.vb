Imports System.Data.SqlClient

Public Class login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.Cookies("UsuarioRecordaro") IsNot Nothing Then
                txtNombreUsuario.Text = Request.Cookies("UsuarioRecordaro").Value
                chkRecordar.Checked = True
            End If
        End If
        If Not Session("UsuarioID") Is Nothing Then
            Dim rol As String = Session("UsuarioRol")?.ToString()
            If rol = "1" Then
                Response.Redirect("Doctores.aspx")
            ElseIf rol = "2" Then
                Response.Redirect("Pacientes.aspx")
            End If
        End If
    End Sub
    Protected Function verificarUsuario(usuario As Usuarios) As Usuarios
        Try
            Dim helper As New DatabaseHelper()
            Dim wrapper As New Simple3Des("Encriptacion123")
            Dim passwordEncriptada As String = wrapper.EncryptData(usuario.PasswordHash)
            Dim parametros As New List(Of SqlParameter) From {
                New SqlParameter("@NombreUsuario", usuario.NombreUsuario),
                New SqlParameter("@PasswordHash", passwordEncriptada)
            }
            Dim Query As String = "Select * FROM Usuarios WHERE NombreUsuario = @NombreUsuario And PasswordHash = @PasswordHash"
            Dim dataTable As DataTable = helper.ExecuteQuery(Query, parametros)
            If dataTable.Rows.Count > 0 Then
                Dim usuarioCompleto As Usuarios = usuario.dtToUsuarios(dataTable)
                Session("UsuarioID") = usuarioCompleto.UsuarioID.ToString()
                Session("NomnbreUsuario") = usuarioCompleto.NombreUsuario.ToString()
                Session("UsuarioRol") = usuarioCompleto.RolId.ToString()
                Return usuarioCompleto
            Else
                Return Nothing
            End If
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Protected Sub btnLogin_Click(sender As Object, e As EventArgs)
        Dim usuario As String = txtNombreUsuario.Text.Trim()
        If chkRecordar.checked Then
            Dim cookie As New HttpCookie("UsuarioRecordaro")
            cookie.Value = usuario
            cookie.Expires = DateTime.Now.AddDays(7)
            Response.Cookies.Add(cookie)
        Else
            If Request.Cookies("UsuarioRecordaro") IsNot Nothing Then
                Dim cookie As New HttpCookie("UsuarioRecordaro")
                cookie.Expires = DateTime.Now.AddDays(-1)
                Response.Cookies.Add(cookie)
            End If
        End If
        lblError.Visible = False
        Dim usuarioIn As New Usuarios() With {
            .NombreUsuario = txtNombreUsuario.Text.Trim(),
            .PasswordHash = txtPass.Text.Trim()
        }
        If String.IsNullOrEmpty(usuarioIn.NombreUsuario) OrElse String.IsNullOrEmpty(usuarioIn.PasswordHash) Then
            ScriptManager.RegisterStartupScript(
                 Me, Me.GetType(),
                "CamposVacios",
                "Swal.fire('Por favor, ingrese email y contraseña.');",
                True)
            Exit Sub

        End If
        Dim usuariocompleto As Usuarios = verificarUsuario(usuarioIn)
        If usuariocompleto IsNot Nothing Then
            Select Case usuariocompleto.RolId
                Case "1"
                    ScriptManager.RegisterStartupScript(
                 Me, Me.GetType(),
                 "AccesoExitoso",
                 "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'Doctores.aspx'; });",
                 True)

                Case "2"
                    ScriptManager.RegisterStartupScript(
                 Me, Me.GetType(),
                 "AccesoExitoso",
                 "Swal.fire('Acceso Exitoso').then(() => { window.location.href = 'Pacientes.aspx'; });",
                 True)
                Case Else
                    lblError.Text = "Rol no reconocido."
            End Select
        Else
            ScriptManager.RegisterStartupScript(
                 Me, Me.GetType(),
                 "AccesoFallido",
                 "Swal.fire('Usuario o contraseña incorrectos.');",
                 True)
            lblError.Visible = True
            lblError.Text = "Usuario o contraseña incorrectos."
        End If
    End Sub
End Class