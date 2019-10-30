using UpsettingBoy.CSharpPlugin;
using UpsettingBoy.CSharpPluginManager.Enums;

namespace UpsettingBoy.CSharpPluginManager.Models
{
	public class PluginInfo
	{
		public CSPlugin Plugin { get; internal set; }
		public PluginState State { get; internal set; }
		public string Message { get; internal set; }

		public PluginInfo(CSPlugin plug, PluginState state, string msg)
		{
			Plugin = plug;
			State = state;
			Message = msg;
		}

		public PluginInfo((CSPlugin, PluginState, string) tuple) :
				this(tuple.Item1, tuple.Item2, tuple.Item3)
		{ }
	}
}