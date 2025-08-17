Public Class Citas
    Public Property CitaId As Integer
    Public Property PacienteId As Integer
    Public Property DoctorId As Integer
    Public Property Fecha As DateTime

    Public Property Hora As DateTime
    Public Property Estado As String

    Public Sub New()
    End Sub
    Public Function dtToCitas(dataTable As DataTable) As Citas
        If dataTable Is Nothing OrElse dataTable.Rows.Count = 0 Then
            Return Nothing
        End If
        Dim row As DataRow = dataTable.Rows(0)
        Dim cita As New Citas With {
            .CitaId = Convert.ToInt32(row("CitaId")),
            .PacienteId = Convert.ToInt32(row("PacienteId")),
            .DoctorId = Convert.ToInt32(row("DoctorId")),
            .Fecha = Convert.ToDateTime(row("Fecha")),
            .Hora = Convert.ToDateTime(row("Hora")),
            .Estado = Convert.ToString(row("Estado"))
        }
        Return cita
    End Function


End Class
