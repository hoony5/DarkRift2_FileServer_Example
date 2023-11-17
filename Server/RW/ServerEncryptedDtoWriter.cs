using Server.Security;

public class ServerEncryptedDtoWriter
{
    public bool SendMessage<TModel>(IClient sender, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            // encrypted Message
            writer.Write(model.AesEncrypt());
            using (Message message = Message.Create((ushort)tag, writer))
            {
                bool isSuccess = sender.SendMessage(message, sendMode);
                FileServer.DebugLog($"Send EncryptedMessage Success ? {isSuccess}");
                return isSuccess;
            }
        }
    }
    
    public bool SendMessageToParty<TModel>(Party party, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            // encrypted Message
            writer.Write(model.AesEncrypt());
            using (Message message = Message.Create((ushort)tag, writer))
            {
                // to Leader
                _ = DatabaseCenter.Instance.GetUserDb().UserClientMap.TryGetValue(party.Leader.AccountID, out IClient? toLeader);
                bool? isSuccess = toLeader?.SendMessage(message, sendMode);
                
                if(party.Members.Count is 0) return isSuccess.HasValue && isSuccess.Value;
                
                // to Member
                foreach (string memberAccountID in party.Members.Keys)
                {
                    _ = DatabaseCenter.Instance.GetUserDb().UserClientMap.TryGetValue(memberAccountID, out IClient? toMember);
                    isSuccess = toMember?.SendMessage(message, sendMode);
                }
                return isSuccess.HasValue && isSuccess.Value;
            }
        }
    }
    
    public bool SendAllUsers<TModel>(IClientManager clientManager, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (DarkRiftWriter writer = DarkRiftWriter.Create())
        {
            // encrypted Message
            writer.Write(model.AesEncrypt());
            bool isSuccess = false;
            using (Message message = Message.Create((ushort)tag, writer))
            {
                IClient[]? allClients = clientManager.GetAllClients();
                if (allClients is null) return false;
                foreach (IClient client in allClients)
                {
                    isSuccess = client.SendMessage(message, sendMode);
                    FileServer.DebugLog($"Send EncryptedMessage Success ? {isSuccess}");
                }
                return isSuccess;
            }
        }
    }

}