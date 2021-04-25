using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace ECS.Utilities
{
    public static partial class Utils
    {
		/// <summary>
		/// Specifies to the DLL resolver which DLL files need to be read based on the platform.
		/// </summary>
		/// <param name="nativeReferencesDirectory">The name of the directory containing the DLLs.</param>
		public static void SetDllImportResolver(string nativeReferencesDirectory)
        {
			NativeLibrary.SetDllImportResolver(typeof(Game).Assembly, (name, assembly, searchPath) => {
				switch (name)
				{
					case "FAudio":
					case "FNA3D":
					case "libtheorafile":
					case "SDL2":
						if (NativeLibrary.TryLoad(Path.Combine(GetOSLibraryDirectory(nativeReferencesDirectory), name), out IntPtr handle))
						{
							return handle;
						}

						break;
				}

				return IntPtr.Zero;
			});
		}

		/// <summary>
		/// Determines which folder of DLLs is necessary based on the platform.
		/// </summary>
		/// <param name="nativeReferencesDirectory">The name of the directory containing the DLLs.</param>
		private static string GetOSLibraryDirectory(string nativeReferencesDirectory)
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				switch (RuntimeInformation.OSArchitecture)
				{
					case Architecture.X64:
						return Path.Combine(nativeReferencesDirectory, "x64");
					case Architecture.X86:
						return Path.Combine(nativeReferencesDirectory, "x86");
				}
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.OSArchitecture == Architecture.X64)
			{
				return Path.Combine(nativeReferencesDirectory, "lib64");
			}

			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && RuntimeInformation.OSArchitecture == Architecture.X64)
			{
				return Path.Combine(nativeReferencesDirectory, "osx");
			}

			return ".";
		}
	}
}
