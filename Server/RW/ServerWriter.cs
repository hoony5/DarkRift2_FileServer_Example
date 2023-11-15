
public class ServerWriter
{
    public bool SendMessage<TModel>(IClient sender, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (Message message = Message.Create((ushort)tag, model))
        {
            bool isSuccess = sender.SendMessage(message, sendMode);
            FileServer.DebugLog($"Send Message Success ? {isSuccess}");
            return isSuccess;
        }
    }
}