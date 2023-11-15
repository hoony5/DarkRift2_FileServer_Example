public class PartyDissolveProcessor
{
    private const int LEADER_COUNT = 1;
    
    public void ProcessExitPartyRequest(RequestExitParty req, MessageReceivedEventArgs e)
    {
        ResponseExitParty res = new ResponseExitParty(e.Client.ID);
        
        if (!DatabaseCenter.Instance.GetDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = 0;
            res.Log = $"There is no party named {req.PartyName}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_EXIT_PARTY);
            return;
        }

        // if the user is my party member, remove the user from the party
        if (party.Members.ContainsKey(req.DepartedUser.AccountID))
        {
             req.DepartedUser.PartyKey = PreventExceptionStringValue;
             party.Members.Remove(req.DepartedUser.AccountID);
             party.CurrentPlayers--;
             FileServer.DebugLog($@"
[Exit Party]
    - member              : {req.DepartedUser.AccountID}");
             res.State = 1;
             res.DepartedUser = req.DepartedUser;
             res.PartyName = req.PartyName;
             
             new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_EXIT_PARTY);
             DatabaseCenter.Instance.GetDb().UserHeaderMap.AddOrUpdate(res.DepartedUser.AccountID, res.DepartedUser);
             return;
        }

        // or when the user is my party leader and there is any no member , remove the user from the party
        if (party.Leader.AccountID.Equals(req.DepartedUser.AccountID) && party.Members.Count is not 0)
        {
            UserHeader member = party.Members.FirstOrDefault().Value;
            if (member.AccountID.Equals(PreventExceptionStringValue)) return;
            req.DepartedUser.PartyKey = PreventExceptionStringValue;
            
            // Swap Leader with Member One.
            party.Leader = new UserHeader(member.AccountID)
            {
                NickName = member.NickName,
                ConnectionState = member.ConnectionState,
                PartyKey = party.Key
            };

            // then remove the member from the party
            party.Members.Remove(member.AccountID);
            party.CurrentPlayers = LEADER_COUNT + party.Members.Count;
            res.State = 1;
            res.DepartedUser = req.DepartedUser;
            res.PartyName = req.PartyName;
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_EXIT_PARTY);
            DatabaseCenter.Instance.GetDb().UserHeaderMap.AddOrUpdate(res.DepartedUser.AccountID, res.DepartedUser);
            
            FileServer.DebugLog($@"
[Exit Party]
    - member              : {req.DepartedUser.AccountID}
    - newLeader           : {party.Leader.AccountID}");
            return;
        }

        // TODO:: Delete All Files
        
        // Remove Party
        DatabaseCenter.Instance.GetDb().PartyMap.Remove(req.PartyName);
    }
    
    public void ProcessRemoveMemberRequest(RequestRemoveMember req, MessageReceivedEventArgs e)
    {
        ResponseRemoveMember res = new ResponseRemoveMember(e.Client.ID);
        if (!DatabaseCenter.Instance.GetDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
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
        DatabaseCenter.Instance.GetDb().UserHeaderMap.AddOrUpdate(req.PartyName, req.RemovedUserInfo);
        FileServer.DebugLog($@"
[Remove Member]
    - member              : {req.RemovedUserInfo.AccountID}");
    }
    public void ProcessDestroyPartyRequest(RequestDestroyParty req, MessageReceivedEventArgs e)
    {
        ResponseDestroyParty res = new ResponseDestroyParty(e.Client.ID);       
        
        if (!DatabaseCenter.Instance.GetDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = 0;
            res.Log = $"There is no party named {res.PartyName}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_DESTROY_PARTY);
            return;
        }
        
        party.Leader.PartyKey = PreventExceptionStringValue;
        _ = DatabaseCenter.Instance.GetDb().UserHeaderMap.TryGetValue(party.Leader.AccountID, out UserHeader? leader);
        
        if(leader is null)
        {
            res.State = 0;
            res.Log = $"There is no leader named {party.Leader.AccountID}";
            return;
        }
        
        leader.PartyKey = PreventExceptionStringValue;
        
        res.State = 1;
        res.PartyName = req.PartyName;
        if(party.Members.Count is 0)
        {
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_DESTROY_PARTY);
            return;
        }
        foreach (KeyValuePair<string, UserHeader> member in party.Members)
        {
            member.Value.PartyKey = PreventExceptionStringValue;
            _ = DatabaseCenter.Instance.GetDb().UserHeaderMap.TryGetValue(member.Value.AccountID, out UserHeader? memberInfo);
            if(memberInfo is null) continue;
            memberInfo.PartyKey = PreventExceptionStringValue;
        }
        
        new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_DESTROY_PARTY);
        FileServer.DebugLog($@"
[Destroy Party]
    - Leader              : {party?.Leader.AccountID}");
        
        // Delete AllFiles
        
        // Remove Party
        DatabaseCenter.Instance.GetDb().PartyMap.Remove(res.PartyName);
    }
}
