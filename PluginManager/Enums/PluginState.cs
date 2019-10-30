namespace UpsettingBoy.CSharpPluginManager.Enums
{
	public enum PluginState
	{
		Enabled,            //* Plugin enabled successfully.
		Disabled,           //* Plugin disabled by PluginManager (still valid).
		Error,              //* Is CSPlugin but cannot be casted into || Runtime error.
		Invalid,            //* Is not CSPlugin.
		Valid,              //* Is CSPlugin and can be casted into.
		Ready              //* Plugin ready to be enabled.
	}
}