Imports System
Imports System.IO
Imports System.Text
Imports Lzo.net
Imports Lzo.net.lzoCompressor

Public Class Form1

    Private lz As lzoCompressor

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            OpenFileDialog1.Filter = "All Files|*.*"
            SaveFileDialog1.Filter = "All Files|*.*"

            If OpenFileDialog1.ShowDialog = DialogResult.OK AndAlso SaveFileDialog1.ShowDialog = DialogResult.OK Then
                Dim buffer As Byte() = IO.File.ReadAllBytes(OpenFileDialog1.FileName)
                lz = New lzoCompressor()
                Dim data As Byte() = lz.Compress(buffer)
                File.WriteAllBytes(SaveFileDialog1.FileName, data)
                MessageBox.Show("Ok")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub
End Class
