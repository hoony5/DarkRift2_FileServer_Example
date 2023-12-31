﻿public class RemoveMemberProcessor
{
    private const int LEADER_COUNT = 1;
    public void ProcessRemoveMemberRequest(RequestRemoveMember? req, MessageReceivedEventArgs e)
    {
        ResponseRemoveMember res = new ResponseRemoveMember()
        {
            ClientID   = e.Client.ID
        };
        if (!DatabaseCenter.Instance.GetPartyDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = FailedState;
            res.Log = $"There is no party named {req.PartyName}";
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_REMOVE_MEMBER);
            return;
        }
        
        if (!party.Members.ContainsKey(req.RemovedUserInfo.AccountID))
        {
            res.State = FailedState;
            res.Log = $"There is no member named {req.RemovedUserInfo.AccountID}";
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_REMOVE_MEMBER);
            return;
        }
        
        party.Members.Remove(req.RemovedUserInfo.AccountID);
        party.CurrentPlayers = LEADER_COUNT + party.Members.Count;
        req.RemovedUserInfo.PartyKey = StringNullValue;
        res.State = SuccessState;
        res.RemovedUserInfo = req.RemovedUserInfo;
        res.PartyName = req.PartyName;
        _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_REMOVE_MEMBER);
        DatabaseCenter.Instance.GetUserDb().UserHeaderMap.AddOrUpdate(req.PartyName, req.RemovedUserInfo);
        FileServer.DebugLog($@"
[Remove Member]
    - member              : {req.RemovedUserInfo.AccountID}");
    }
}
