using Server.Security;

public class ServerEncryptedDtoWriter
{
    public bool SendMessage<TModel>(IClient sender, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            writer.Write(model.AesEncrypt());
            using (Message message = Message.Create((ushort)tag, writer))
            {
                bool isSuccess = sender.SendMessage(message, sendMode);
                FileServer.DebugLog($"Send EncryptedMessage Success ? {isSuccess}");
                return isSuccess;
            }
        }
    }
}