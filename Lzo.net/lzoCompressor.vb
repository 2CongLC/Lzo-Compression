Imports System
Imports System.Text
Imports System.IO
Imports System.IO.Compression
Imports System.Runtime
Imports System.Runtime.InteropServices


Public Class lzoCompressor

    Const DLLUnmanager As String = "lzo.dll"

    <DllImport(DLLUnmanager)>
    Private Shared Function __lzo_init3() As Integer
    End Function

    <DllImport(DLLUnmanager)>
    Private Shared Function lzo_version_string() As String
    End Function

    <DllImport(DLLUnmanager)>
    Private Shared Function lzo_version_date() As String
    End Function

    <DllImport(DLLUnmanager)>
    Private Shared Function lzo1x_1_compress(src As Byte(), src_len As Integer, dst As Byte(), ByRef dst_len As Integer, wrkmem As Byte()) As Integer
    End Function

    <DllImport(DLLUnmanager)>
    Private Shared Function lzo1x_decompress(src As Byte(), src_len As Integer, dst As Byte(), ByRef dst_len As Integer, wrkmem As Byte()) As Integer
    End Function

    Private _workMemory As Byte() = New Byte(16384L * 4 - 1) {}
    Private Shared _traceSwitch As New TraceSwitch("Lzo.net", "Switch for tracing of the LZOCompressor-Class")


    Public Sub New()
        Dim init As Integer = __lzo_init3()
        If init <> 0 Then
            Throw New Exception("Initialization of LZO-Compressor failed !")
        End If
    End Sub

    Public ReadOnly Property Version As String
        Get
            Return lzo_version_string()
        End Get
    End Property

    Public ReadOnly Property VersionDate As String
        Get
            Return lzo_version_date()
        End Get
    End Property

    Public Function Compress(ByVal src As Byte()) As Byte()
        If _traceSwitch.TraceVerbose Then
            Trace.WriteLine(String.Format("LZOCompressor: trying to compress {0}", src.Length))
        End If
        Dim dst As Byte() = New Byte(src.Length + src.Length \ 64 + 16 + 3 + 4 - 1) {}
        Dim outlen As Integer = 0
        lzo1x_1_compress(src, src.Length, dst, outlen, _workMemory)
        If _traceSwitch.TraceVerbose Then
            Trace.WriteLine(String.Format("LZOCompressor: compressed {0} to {1} bytes", src.Length, outlen))
        End If
        Dim ret As Byte() = New Byte(outlen + 4 - 1) {}
        Array.Copy(dst, 0, ret, 0, outlen)
        Dim outlenarr As Byte() = BitConverter.GetBytes(src.Length)
        Array.Copy(outlenarr, 0, ret, outlen, 4)
        Return ret
    End Function

    Public Function Decompress(ByVal src As Byte()) As Byte()
        If _traceSwitch.TraceVerbose Then
            Trace.WriteLine(String.Format("LZOCompressor: trying to decompress {0}", src.Length))
        End If
        Dim origlen As Integer = BitConverter.ToInt32(src, src.Length - 4)
        Dim dst As Byte() = New Byte(origlen - 1) {}
        Dim outlen As Integer = origlen
        lzo1x_decompress(src, src.Length - 4, dst, outlen, _workMemory)
        If _traceSwitch.TraceVerbose Then
            Trace.WriteLine(String.Format("LZOCompressor: decompressed {0} to {1} bytes", src.Length, origlen))
        End If
        Return dst
    End Function

    Public Function DecompressRaw(ByVal src As Byte(), ByVal decompressLength As Integer) As Byte()
        If _traceSwitch.TraceVerbose Then
            Trace.WriteLine(String.Format("LZOCompressor: trying to decompress {0} as data block", src.Length))
        End If
        Dim dst As Byte() = New Byte(decompressLength - 1) {}
        Dim outlen As Integer = decompressLength
        lzo1x_decompress(src, src.Length, dst, outlen, _workMemory)
        If _traceSwitch.TraceVerbose Then
            Trace.WriteLine(String.Format("LZOCompressor: decompressed {0} to {1} bytes", src.Length, outlen))
        End If
        Return dst
    End Function

End Class
