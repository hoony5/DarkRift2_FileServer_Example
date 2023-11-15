public class PartyJoinProcessor
{
    public void ProcessJoinPartyRequest(RequestJoinParty req, MessageReceivedEventArgs e)
    {
        ResponseJoinParty res = new ResponseJoinParty(e.Client.ID);
        // Invalidated Party
        if (!DatabaseCenter.Instance.GetDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = 0;
            res.Log = $"There is no party named {req.PartyName}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY);
            return;
        }

        if (party is null)
        {
            res.State = 0;
            res.Log = $"There is no Value. => key: {req.PartyName}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);
            return;
        }
        
        // update from server level - Party
        party.AddOrUpdateMember(req.JoinedUserInfo);
        party.UpdateCurrentPlayers();
        
        // update from client level - Joined User
        req.JoinedUserInfo.PartyKey = party.Key;
        res.JoinedUserInfo = req.JoinedUserInfo;
        res.JoinedParty = party;
        res.State = 1;
        
        DatabaseCenter.Instance.GetDb().UserHeaderMap.AddOrUpdate(res.JoinedUserInfo.AccountID, res.JoinedUserInfo);
        
        new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY);

        FileServer.DebugLog($@"
[Joined Party :{(res.State == 1 ? "Success" : "Fail")}]
    - name              : {party.Name}
    - password          : {party.Password} 
    - key               : {party.Key}
    - state             : {party.State}
    - leader            : {party.Leader.AccountID}
    - max               : {party.MaxPlayers}

[Joined User]
    - id                : {req.JoinedUserInfo.AccountID}
    - nickName          : {req.JoinedUserInfo.NickName}
    - partyKey          : {req.JoinedUserInfo.PartyKey}
    - connectionState   : {req.JoinedUserInfo.ConnectionState}");
    }
    public void ProcessJoinPartyWithPasswordRequest(RequestJoinPartyWithPassword req, MessageReceivedEventArgs e)
    {
        ResponseJoinPartyWithPassword res = new ResponseJoinPartyWithPassword(e.Client.ID);
        // Invalidated Party
        if (!DatabaseCenter.Instance.GetDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = 0;
            res.Log = $"There is no party named {req.PartyName}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);
            return;
        }

        if (party is null)
        {
            res.State = 0;
            res.Log = $"There is no Value. => key: {req.PartyName}";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);
            return;
        }
        
        // Invalidated password
        if (!party.Password.Equals(req.PartyPassword))
        {
            res.State = 0;
            res.Log = "Password is not correct";
            new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);
            return;
        }

        // update from server level - Party
        party.AddOrUpdateMember(req.JoinedUserInfo);
        party.UpdateCurrentPlayers();
        
        // update from client level - Joined User
        req.JoinedUserInfo.PartyKey = party.Key;
        res.State = 1;
        res.JoinedUserInfo = req.JoinedUserInfo;
        res.JoinedParty = party;
        
        DatabaseCenter.Instance.GetDb().UserHeaderMap.AddOrUpdate(res.JoinedUserInfo.AccountID, res.JoinedUserInfo);
        new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);

        FileServer.DebugLog($@"
[Joined Party With Password :{(res.State == 1 ? "Success" : "Fail")}]
    - name              : {party.Name}
    - password          : {party.Password} 
    - key               : {party.Key}
    - state             : {party.State}
    - leader            : {party.Leader.AccountID}
    - max               : {party.MaxPlayers}

[Joined User]
    - id                : {req.JoinedUserInfo.AccountID}
    - nickName          : {req.JoinedUserInfo.NickName}
    - partyKey          : {req.JoinedUserInfo.PartyKey}
    - connectionState   : {req.JoinedUserInfo.ConnectionState}");
    }
}
