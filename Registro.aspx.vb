Public Class Registro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub LimpiarCampos()
        txtNombre.Text = String.Empty
        txtApellido.Text = String.Empty
        txtFecha_Nacimiento.Text = String.Empty
        txtTelefono.Text = String.Empty
        txtEmail.Text = String.Empty
        txtNombreUsuario.Text = String.Empty
    End Sub

    Protected Sub btnRegistrar_Click(sender As Object, e As EventArgs)

    End Sub
End Class