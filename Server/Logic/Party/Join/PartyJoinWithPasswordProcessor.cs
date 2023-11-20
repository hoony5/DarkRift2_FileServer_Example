public class PartyJoinWithPasswordProcessor
{
    public void ProcessJoinPartyWithPasswordRequest(RequestJoinPartyWithPassword? req, MessageReceivedEventArgs e)
    {
        ResponseJoinPartyWithPassword res = new ResponseJoinPartyWithPassword
        {
            ClientID = e.Client.ID
        };

        // Invalidated Party
        if (!DatabaseCenter.Instance.GetPartyDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = FailedState;
            res.Log = $"There is no party named {req.PartyName}";
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);
            return;
        }

        if (party is null)
        {
            res.State = FailedState;
            res.Log = $"There is no Value. => key: {req.PartyName}";
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);
            return;
        }

        // Invalidated password
        if (!party.Password.Equals(req.PartyPassword))
        {
            res.State = FailedState;
            res.Log = "Password is not correct";
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);
            return;
        }

        // update from server level - Party
        party.AddOrUpdateMember(req.JoinedUserInfo);
        party.UpdateCurrentPlayers();

        // update from client level - Joined User
        req.JoinedUserInfo.PartyKey = party.Key;
        res.State = SuccessState;
        res.JoinedUserInfo = req.JoinedUserInfo;
        res.JoinedParty = party;

        DatabaseCenter.Instance.GetUserDb().UserHeaderMap.AddOrUpdate(res.JoinedUserInfo.AccountID, res.JoinedUserInfo);
        _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_JOIN_PARTY_WITH_PASSWORD);

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
