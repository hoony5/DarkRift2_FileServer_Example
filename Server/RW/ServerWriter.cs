using DarkRift;
using Model;
using Server;

public class ServerWriter(FileServer server)
{
    private FileServer _server = server;

    public bool SendMessage<TModel>(IClient sender, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (Message message = Message.Create((ushort)tag, model))
        {
            bool isSuccess = sender.SendMessage(message, sendMode);
            _server.DebugLog($"Send Message Success ? {isSuccess}");
            return isSuccess;
        }
    }
}