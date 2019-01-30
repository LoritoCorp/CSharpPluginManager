using System;

namespace UpsettingBoy.CSharpPlugin
{
	/// <summary>
	/// Extend this class to make an entry point for a plugin.
	/// </summary>
	public abstract class CSPlugin : IDisposable
	{
		public abstract string Author { get; }
		public abstract string PluginName { get; }

		private bool _disposed = false;

		public CSPlugin() { }

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
			if (_disposed)
				return;

			OnDisabled();
			_disposed = true;
		}
	}
}