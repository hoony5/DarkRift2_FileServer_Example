public class RemoveMemberProcessor
{
    private const int LEADER_COUNT = 1;
    public void ProcessRemoveMemberRequest(RequestRemoveMember? req, MessageReceivedEventArgs e)
    {
        ResponseRemoveMember res = new ResponseRemoveMember(e.Client.ID);
        if (!DatabaseCenter.Instance.GetPartyDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = 0;
            res.Log = $"There is no party named {req.PartyName}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_REMOVE_MEMBER);
            return;
        }
        
        if (!party.Members.ContainsKey(req.RemovedUserInfo.AccountID))
        {
            res.State = 0;
            res.Log = $"There is no member named {req.RemovedUserInfo.AccountID}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_REMOVE_MEMBER);
            return;
        }
        
        party.Members.Remove(req.RemovedUserInfo.AccountID);
        party.CurrentPlayers = LEADER_COUNT + party.Members.Count;
        req.RemovedUserInfo.PartyKey = PreventExceptionStringValue;
        res.State = 1;
        res.RemovedUserInfo = req.RemovedUserInfo;
        res.PartyName = req.PartyName;
        new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_REMOVE_MEMBER);
        DatabaseCenter.Instance.GetUserDb().UserHeaderMap.AddOrUpdate(req.PartyName, req.RemovedUserInfo);
        FileServer.DebugLog($@"
[Remove Member]
    - member              : {req.RemovedUserInfo.AccountID}");
    }
}
