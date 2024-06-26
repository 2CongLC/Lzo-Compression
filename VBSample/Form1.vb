Imports System
Imports System.IO
Imports System.Text
Imports Lzo.net
Imports Lzo.net.LZOCompressor


Public Class Form1

    Private lz As LZOCompressor

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://2conglc-vn.blogspot.com/")
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try

            If OpenFileDialog1.ShowDialog = DialogResult.OK AndAlso SaveFileDialog1.ShowDialog = DialogResult.OK Then
                lz = New LZOCompressor()
                Dim buffer As Byte() = IO.File.ReadAllBytes(OpenFileDialog1.FileName)
                Dim compressed As Byte() = lz.Compress(buffer)
                IO.File.WriteAllBytes(SaveFileDialog1.FileName, compressed)
                MessageBox.Show("Đã xong !")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try

            If OpenFileDialog1.ShowDialog = DialogResult.OK AndAlso SaveFileDialog1.ShowDialog = DialogResult.OK Then
                lz = New LZOCompressor()
                Dim buffer As Byte() = IO.File.ReadAllBytes(OpenFileDialog1.FileName)
                Dim Uncompressed As Byte() = lz.Decompress(buffer)
                IO.File.WriteAllBytes(SaveFileDialog1.FileName, Uncompressed)
                MessageBox.Show("Đã xong !")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString())
        End Try
    End Sub


End Class
