public class FileServer : Plugin
{
    public override Version Version { get; } = new Version(1, 0, 0);
    
    public override bool ThreadSafe { get; } = false;
    
    public FileServer(PluginLoadData pluginLoadData) : base(pluginLoadData)
    {
        ClientManager.ClientConnected += OnClientConnected;
        ClientManager.ClientDisconnected += OnClientDisconnected;
        
        _ = new DatabaseCenter();
        
        Logger.Info("FileServer Listening...");
    }

    private void OnClientConnected(object? sender, ClientConnectedEventArgs e)
    {
         e.Client.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        new ServerReader().OnMessageReceived(sender, e);
    }

    private void OnClientDisconnected(object? sender, ClientDisconnectedEventArgs e)
    {
        e.Client.MessageReceived -= OnMessageReceived;
    }
    
    public static void DebugLog(string message)
    {
#if Debug
        Debugger.Write("FileServer", message);
#endif
    }
}