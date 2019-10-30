using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace PlgSystem.Plugin
{
	/// <summary>
	/// Extend this class to make an entry point for a plugin.
	/// </summary>
	public abstract class CSPlugin : IDisposable
	{
		public abstract string Author { get; }
		public abstract string PluginName { get; }

		private bool _disposed = false;
      SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

		/// <summary>
		/// Called at check time (before the plugin is enabled). Must be used for
		/// initialization of plugin's data structures, ...
		/// </summary>
		public abstract void OnValidated();

		/// <summary>
		/// Called when the plugin is enabled. Must be the entry point of
		/// the plugin.
		/// </summary>
		public abstract void OnEnabled();

		/// <summary>
		/// Called when the plugin is disabled or the PluginManager
		/// is Disposed.
		/// </summary>
		public abstract void OnDisabled();

		public void Dispose()
		{
         Dispose(true);
         GC.SuppressFinalize(this);
		}
      // Protected implementation of Dispose pattern.
      protected virtual void Dispose(bool disposing)
      {
         if (_disposed)
            return;

         if (disposing)
         {
            handle.Dispose();
            OnDisabled();
         }

         _disposed = true;
      }
   }
}