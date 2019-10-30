# CSharp Plugin Manager
This library reduces the amount of work needed for the implementation of a plugin manager into an application.

## Usage (host application)
Firstly, add **_PluginManager_** project into yours by cloning and referencing or by **Nuget** (added later). Then, follow the example below:

```csharp
using UpsettingBoy.CSharpPluginManager;

public class DesiredClass
{
    PluginManager _manager;

    // your code...
    
    public async Task InitilizePluginManager()
    {
        _manager = new PluginManager(@"<plugins_path>");
        var loadPlugins = _manager.LoadPluginsAsync();
        // some stuff...
        return await loadPlugins;
    }
}
```

Or if you don't want to do some stuff in ```InitizalizePlugins()```, directly:

```csharp
using UpsettingBoy.CSharpPluginManager;

public class DesiredClass
{
    PluginManager _manager;

    // your code...
    
    public async Task InitilizePluginManager()
    {
        _manager = new PluginManager(@"<plugins_path>");
        return await _manager.LoadPluginsAsync();
    }
}
```

You also need to provide plugin developers an API for access your application features. 

If you are concerned about the lack of UI support for plugin implementation, I am working on it (adding a new method on **_CSPlugin_** abstract class).

## Usage (plugin developer)
In order to your plugin to be loaded by _**PluginManager**_ it has to implement
**_CSPlugin_** class. You can add this class by cloning and referencing 
**_CSPlugin_** project into yours or by the **_Nuget_** package (added later on), then, simply implements it in **ONLY ONE CLASS** of your plugin. An example class:

```csharp
using UpsettingBoy.CSharpPlugin;

namespace your.namaspace
{
    public class MainClass : CSPlugin
    {
        public override string Name => "YourPluginName";
        public override string Author => "YourName";

        public override void OnValidated()
        {
            // Initilization of plugin's data structures.
        }

        public override void OnEnabled()
        {
            // Entry point of the plugin.
        }

        public override void OnDisabled()
        {
            // Called when the plugin gets disabled or disposed.
        }
    }
}
```

In the future, it might be added new methods/properties into **_CSPlugin_** so backwards compatibility migth be a problem. Watch out **_PluginManager_**
version given by the host application of your plugin.

## License
This project is under a [MIT](https://www.github.com/UpsettingBoy/CSharpPluginManager/blob/master/LICENSE) license. 

**_CSharpPluginManager_** also uses:
- **Microsoft.CSharp** -> [MIT](https://github.com/dotnet/corefx/blob/master/LICENSE.TXT) license.

Additional information of licenses on *Licenses* folder.