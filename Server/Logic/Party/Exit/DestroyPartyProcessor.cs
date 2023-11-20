public class DestroyPartyProcessor
{
    public void ProcessDestroyPartyRequest(RequestDestroyParty? req, MessageReceivedEventArgs e)
    {
        ResponseDestroyParty res = new ResponseDestroyParty()
        {
            ClientID   = e.Client.ID
        };    
        
        if (!DatabaseCenter.Instance.GetPartyDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = FailedState;
            res.Log = $"There is no party named {res.PartyName}";
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_DESTROY_PARTY);
            return;
        }
        
        party.Leader.PartyKey = StringNullValue;
        _ = DatabaseCenter.Instance.GetUserDb().UserHeaderMap.TryGetValue(party.Leader.AccountID, out UserHeader? leader);
        
        if(leader is null)
        {
            res.State = FailedState;
            res.Log = $"There is no leader named {party.Leader.AccountID}";
            return;
        }
        
        leader.PartyKey = StringNullValue;
        
        res.State = SuccessState;
        res.PartyName = req.PartyName;
        if(party.Members.Count is 0)
        {
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_DESTROY_PARTY);
            return;
        }
        foreach (KeyValuePair<string, UserHeader> member in party.Members)
        {
            member.Value.PartyKey = StringNullValue;
            _ = DatabaseCenter.Instance.GetUserDb().UserHeaderMap.TryGetValue(member.Value.AccountID, out UserHeader? memberInfo);
            if(memberInfo is null) continue;
            memberInfo.PartyKey = StringNullValue;
        }
        
        _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_DESTROY_PARTY);
        FileServer.DebugLog($@"
[Destroy Party]
    - Leader              : {party?.Leader.AccountID}");
        
        // Delete AllFiles
        new DeleteFileProcessor().RemovePartyUploadedFiles(party.Key);
        // Remove Party
        DatabaseCenter.Instance.GetPartyDb().PartyMap.Remove(res.PartyName);
    }
}
