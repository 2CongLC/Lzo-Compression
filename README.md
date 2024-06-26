# Lzo Compression
 Nén dữ liệu sử dụng thư viện Lzo
 * Hỗ trợ .Netframework 4.8
 * Cách sử dụng :
 * compress
 ```vbnet
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
 ```
* decompress
```vbnet
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
```
* Email : 2conglc.vn@gmail.compress
* © 2008 - 2024 . By 2CongLc