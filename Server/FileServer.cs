
namespace Server;

public class FileServer : Plugin
{
    public override Version Version { get; } = new Version(1, 0, 0);
    
    public override bool ThreadSafe { get; } = false;
    
    public FileServer(PluginLoadData pluginLoadData) : base(pluginLoadData)
    {
        ClientManager.ClientConnected += OnClientConnected;
        ClientManager.ClientDisconnected += OnClientDisconnected;
        
        Logger.Info("FileServer Listening...");
    }

    private void OnClientConnected(object? sender, ClientConnectedEventArgs e)
    {
         e.Client.MessageReceived += OnMessageReceived;
    }

    private void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        ServerReader reader = new ServerReader(this);
        e.Client.MessageReceived += reader.OnMessageReceived;
    }

    private void OnClientDisconnected(object? sender, ClientDisconnectedEventArgs e)
    {
        e.Client.MessageReceived -= OnMessageReceived;
    }
    
    public void DebugLog(string message, Exception? exception = null)
    {
        Logger.Info(message, exception);
    }
}