
public class ServerWriter
{
    public bool SendMessage<TModel>(IClient to, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (Message message = Message.Create((ushort)tag, model))
        {
            bool isSuccess = to.SendMessage(message, sendMode);
            FileServer.DebugLog($"Send Message Success ? {isSuccess}");
            return isSuccess;
        }
    }
    
    public bool SendMessageToParty<TModel>(Party party, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (Message message = Message.Create((ushort)tag, model))
        {
            _ = DatabaseCenter.Instance.GetUserDb().UserClientMap.TryGetValue(party.Leader.AccountID, out IClient? toLeader);
            bool? isSuccess = toLeader?.SendMessage(message, sendMode);
            
            if(party.Members.Count is 0) return isSuccess.HasValue && isSuccess.Value;
            
            foreach (string memberAccountID in party.Members.Keys)
            {
                _ = DatabaseCenter.Instance.GetUserDb().UserClientMap.TryGetValue(memberAccountID, out IClient? toMember);
                isSuccess = toMember?.SendMessage(message, sendMode);
            }
            return isSuccess.HasValue && isSuccess.Value;
        }
    }
    
    public bool SendAllUsers<TModel>(IClientManager clientManager, TModel model, Tags tag,
        SendMode sendMode = SendMode.Reliable) where TModel : IDarkRiftSerializable
    {
        using (Message message = Message.Create((ushort)tag, model))
        {
            IClient[]? allClients = clientManager.GetAllClients();
            if (allClients is null) return false;
            foreach (IClient client in allClients)
            {
                bool isSuccess = client.SendMessage(message, sendMode);
                FileServer.DebugLog($"Send Message Success ? {isSuccess}");
            }
            return true;
        }
    }
}