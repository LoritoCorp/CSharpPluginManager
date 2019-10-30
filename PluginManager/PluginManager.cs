using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.CSharp; //! Don't remove -> Compiler stuff

using UpsettingBoy.CSharpPlugin;

using UpsettingBoy.CSharpPluginManager.Enums;
using UpsettingBoy.CSharpPluginManager.Models;

namespace UpsettingBoy.CSharpPluginManager
{
	/// <summary>
	/// Plugin manager for classes that implements CSPlugin.
	/// </summary>
	public sealed class PluginManager : IDisposable
	{
		public const string PLUGIN_FILE_EXTENSION = ".dll";
		public const string CLASS_NAME = "CSPlugin";

		private const string DIRECTORY_NULL_MSG = "Directory cannot be null!.";
		private const string DIRECTORY_NO_EXISTS_MSG = "Directory do not exists!.";
		private const string INVALID_PLUGIN = "Plugin not contains CSPlugin!.";

		//TODO: Possible change to dictionary (should not be used outside PluginManger).
		public ConcurrentDictionary<string, PluginInfo> Plugins { get; private set; }

		private bool _disposed = false;
		private DirectoryInfo _directory;

		public PluginManager(DirectoryInfo pluginsDirectory)
		{
			if (pluginsDirectory == null)
				throw new ArgumentNullException(DIRECTORY_NULL_MSG);

			if (!pluginsDirectory.Exists)
				throw new DirectoryNotFoundException(DIRECTORY_NO_EXISTS_MSG);

			_directory = pluginsDirectory;
			Plugins = new ConcurrentDictionary<string, PluginInfo>();
		}

		public PluginManager(string pluginsPath) : this(new DirectoryInfo(pluginsPath)) { }

		public async Task LoadPluginsAsync()
		{
			await Task.Run(() => LoadPlugins());
		}

		private void LoadPlugins()
		{
			Parallel.ForEach(_directory.GetFiles(), async (file) =>
			{
				//* Invalid file extension.
				if (!file.Extension.Equals(PLUGIN_FILE_EXTENSION))
					return;

				Assembly dll = null;
				try
				{
					dll = Assembly.LoadFile(file.FullName);
				}
				catch (Exception) { return; } //* Invalid dll.

				//* List of CastToPlugin tasks.
				var tasks = new List<Task<(CSPlugin, PluginState, string)>>();

				var exportedTypes = dll.GetExportedTypes();
				foreach (var t in exportedTypes)
					tasks.Add(CastToPlugin(t));

				var results = await Task.WhenAll(tasks);

				//* Only check ready plugins, if plugin still valid -> problem.
				var firstPlugin = results.Where(type => type.Item2 == PluginState.Ready);

				if (firstPlugin == null)
					Plugins.TryAdd(exportedTypes[0].Namespace,
							new PluginInfo(null, PluginState.Invalid, INVALID_PLUGIN));
				else
					Plugins.TryAdd(firstPlugin.First().Item1.GetType().FullName,
							new PluginInfo(firstPlugin.First()));
			});
		}

		private async Task<(CSPlugin, PluginState, string)> CastToPlugin(Type type)
		{
			CSPlugin plugin = null;
			PluginState state = PluginState.Error;
			string message = null;
			dynamic instance = Activator.CreateInstance(type);

			try
			{
				plugin = (CSPlugin)instance; //* Can be casted into IPlugin -> Valid.
				state = PluginState.Valid;

				await Task.Run(() =>
				{
					plugin.OnValidated(); //* If ends, plugin is ready.
					state = PluginState.Ready;
				});
			}
			catch (InvalidCastException e)
			{
				message = e.Message; //* Error at casting (maybe bad impementation).
			}

			return (plugin, state, message);
		}

		public void Dispose()
		{
			if (_disposed)
				return;

			Parallel.ForEach(Plugins.Values, (pluginInfo) =>
			{
				bool condition = pluginInfo.State == PluginState.Enabled &&
								 pluginInfo.State == PluginState.Ready;
				if (!condition)
					return;

				pluginInfo.Plugin.Dispose();
				pluginInfo.State = PluginState.Disabled;
			});

			Plugins.Clear();
			Plugins = null;

			_directory = null;

			_disposed = true;
		}
	}
}