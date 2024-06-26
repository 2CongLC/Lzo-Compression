
namespace Lzo.net {
	using System;
	using System.Diagnostics;
	using System.Runtime.InteropServices;

	/// <summary>
	/// Wrapper class for the highly performant LZO compression library
	/// </summary>
	public class LZOCompressor {
		private static TraceSwitch _traceSwitch = new TraceSwitch("Simplicit.Net.Lzo", "Switch for tracing of the LZOCompressor-Class");

		#region Dll-Imports
		[DllImport("lzo.dll")]
		private static extern int __lzo_init3();
		[DllImport("lzo.dll")]
		private static extern string lzo_version_string();
		[DllImport("lzo.dll")]
		private static extern string lzo_version_date();
		[DllImport("lzo.dll")]
		private static extern int lzo1x_1_compress(
			byte[] src,
			int src_len,
			byte[] dst,
			ref int dst_len,
			byte[] wrkmem
			);
		[DllImport("lzo.dll")]
		private static extern int lzo1x_decompress(
			byte[] src,
			int src_len,
			byte[] dst,
			ref int dst_len,
			byte[] wrkmem);
		#endregion
		
		private byte[] _workMemory = new byte[16384L * 4];

		static LZOCompressor() {
			int init = __lzo_init3();
			if(init != 0) {
				throw new Exception("Initialization of LZO-Compressor failed !");
			}
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		public LZOCompressor() {
		}

		/// <summary>
		/// Version string of the compression library.
		/// </summary>
		public string Version {
			get {
				return lzo_version_string();
			}
		}

		/// <summary>
		/// Version date of the compression library
		/// </summary>
		public string VersionDate {
			get {
				return lzo_version_date();
			}
		}

		/// <summary>
		/// Compresses a byte array and returns the compressed data in a new
		/// array. You need the original length of the array to decompress it.
		/// </summary>
		/// <param name="src">Source array for compression</param>
		/// <returns>Byte array containing the compressed data</returns>
		public byte[] Compress(byte[] src) {
			if(_traceSwitch.TraceVerbose) {
				Trace.WriteLine(String.Format("LZOCompressor: trying to compress {0}", src.Length));
			}
			byte[] dst = new byte[src.Length + src.Length / 64 + 16 + 3 + 4];
			int outlen = 0;
			lzo1x_1_compress(src, src.Length, dst, ref outlen, _workMemory);
			if(_traceSwitch.TraceVerbose) {
				Trace.WriteLine(String.Format("LZOCompressor: compressed {0} to {1} bytes", src.Length, outlen));
			}
			byte[] ret = new byte[outlen + 4];
			Array.Copy(dst, 0, ret, 0, outlen);
			byte[] outlenarr = BitConverter.GetBytes(src.Length);
			Array.Copy(outlenarr, 0, ret, outlen, 4);
			return ret;
		}

		/// <summary>
		/// Decompresses compressed data to its original state.
		/// </summary>
		/// <param name="src">Source array to be decompressed</param>
		/// <returns>Decompressed data</returns>
		public byte[] Decompress(byte[] src) {
			if(_traceSwitch.TraceVerbose) {
				Trace.WriteLine(String.Format("LZOCompressor: trying to decompress {0}", src.Length));
			}
			int origlen = BitConverter.ToInt32(src, src.Length - 4);
			byte[] dst = new byte[origlen];
			int outlen = origlen;
			lzo1x_decompress(src, src.Length - 4, dst, ref outlen, _workMemory);
			if(_traceSwitch.TraceVerbose) {
				Trace.WriteLine(String.Format("LZOCompressor: decompressed {0} to {1} bytes", src.Length, origlen));
			}
			return dst;
		}

		/// <summary>
		/// Decompresses compressed data block (no header) to its original state.
		/// </summary>
		/// <param name="src">Source array to be decompressed</param>
		/// <param name="decompressLength">Length expected of decompressed data</param>
		/// <returns>Decompressed data</returns>
		public byte[] DecompressRaw(byte[] src, int decompressLength)
		{
			if (_traceSwitch.TraceVerbose)
			{
				Trace.WriteLine(String.Format("LZOCompressor: trying to decompress {0} as data block", src.Length));
			}
			byte[] dst = new byte[decompressLength];
			int outlen = decompressLength;
			lzo1x_decompress(src, src.Length, dst, ref outlen, _workMemory);
			if (_traceSwitch.TraceVerbose)
			{
				Trace.WriteLine(String.Format("LZOCompressor: decompressed {0} to {1} bytes", src.Length, outlen));
			}
			return dst;
		}
	}
}
