Imports System.Data.SqlClient
Imports System.Xml
Public Class Registrar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub LimpiarCampos()
        txtNombre.Text = String.Empty
        txtApellido.Text = String.Empty
        txtEspecialidad.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtEmail.Text = String.Empty
        txtNombreUsuario.Text = String.Empty
    End Sub
    Protected Function RegistrarMedico(Medico As Medicos, usuario As Usuarios) As Boolean
        Try
            Dim helper As New DatabaseHelper()
            Dim queryUsuario As String = "INSERT INTO Usuarios (NombreUsuario, PasswordHash, RolID) 
                                      VALUES (@NombreUsuario, @PasswordHash, @RolID);
                                      SELECT SCOPE_IDENTITY();"
            Dim parametrosUsuario As New List(Of SqlParameter) From {
                New SqlParameter("NombreUsuario", usuario.NombreUsuario),
                New SqlParameter("PasswordHash", usuario.PasswordHash),
                New SqlParameter("RolId", 1)
            }
            Dim dt As DataTable = helper.ExecuteQuery(queryUsuario, parametrosUsuario)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then
                Return False
            End If
            Dim nuevoUsuarioId As Integer = Convert.ToInt32(dt.Rows(0)(0))
            Dim queryMedico As String = "INSERT INTO Doctores (UsuarioId, Nombre, Apellidos, Especialidad, Telefono, Correo) 
                                      VALUES (@UsuarioId, @Nombre, @Apellidos, @Especialidad, @Telefono, @Correo);"
            Dim parametrosMedico As New List(Of SqlParameter) From {
                helper.CreateParameter("@UsuarioId", nuevoUsuarioId),
                helper.CreateParameter("@Nombre", Medico.Nombre),
                helper.CreateParameter("@Apellidos", Medico.Apellidos),
                helper.CreateParameter("@Especialidad", Medico.Especialidad),
                helper.CreateParameter("@Telefono", Medico.Telefono),
                helper.CreateParameter("@Correo", Medico.Correo)
                }
            Dim resultado As Boolean = helper.ExecuteNonQuery(queryMedico, parametrosMedico)
            Return resultado

        Catch ex As Exception
            Return False


        End Try

    End Function

    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs)
        If String.IsNullOrWhiteSpace(txtNombre.Text) OrElse
           String.IsNullOrWhiteSpace(txtApellido.Text) OrElse
           String.IsNullOrWhiteSpace(txtEspecialidad.Text) OrElse
           String.IsNullOrWhiteSpace(txtTelefono.Text) OrElse
           String.IsNullOrWhiteSpace(txtEmail.Text) OrElse
           String.IsNullOrWhiteSpace(txtNombreUsuario.Text) OrElse
           String.IsNullOrWhiteSpace(txtPass.Text) Then
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "CamposVacios",
                "Swal.fire('Por favor, complete todos los campos.');",
                True)
            Exit Sub
        End If
        Dim usuarioNuevo As New Usuarios() With {
            .NombreUsuario = txtNombreUsuario.Text.Trim(),
            .PasswordHash = txtPass.Text.Trim(),
            .RolId = "1" ' Asignar rol de médico,
        }
        Dim medicoNuevo As New Medicos() With {
            .Nombre = txtNombre.Text.Trim(),
            .Apellidos = txtApellido.Text.Trim(),
            .Especialidad = txtEspecialidad.Text.Trim(),
            .Telefono = txtTelefono.Text.Trim(),
            .Correo = txtEmail.Text.Trim()
        }
        If RegistrarMedico(medicoNuevo, usuarioNuevo) Then
            LimpiarCampos()
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "RegistroExitoso",
                "Swal.fire('Médico registrado exitosamente.');",
                True)
        Else
            ScriptManager.RegisterStartupScript(
                Me, Me.GetType(),
                "ErrorRegistro",
                "Swal.fire('Error al registrar el médico. Por favor, inténtelo de nuevo.');",
                True)
            LimpiarCampos()
        End If

    End Sub
End Class