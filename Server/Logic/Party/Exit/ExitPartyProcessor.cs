public class ExitPartyProcessor
{
    private const int LEADER_COUNT = 1;
    public void ProcessExitPartyRequest(RequestExitParty? req, MessageReceivedEventArgs e)
    {
        ResponseExitParty res = new ResponseExitParty()
        {
            ClientID   = e.Client.ID
        };
        
        if (!DatabaseCenter.Instance.GetPartyDb().PartyMap.TryGetValue(req.PartyName, out Party? party))
        {
            res.State = FailedState;
            res.Log = $"There is no party named {req.PartyName}";
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_EXIT_PARTY);
            return;
        }

        // if the user is my party member, remove the user from the party
        if (party.Members.ContainsKey(req.DepartedUser.AccountID))
        {
             req.DepartedUser.PartyKey = StringNullValue;
             party.Members.Remove(req.DepartedUser.AccountID);
             party.CurrentPlayers--;
             FileServer.DebugLog($@"
[Exit Party]
    - member              : {req.DepartedUser.AccountID}");
             res.State = SuccessState;
             res.DepartedUser = req.DepartedUser;
             res.PartyName = req.PartyName;
             
             _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_EXIT_PARTY);
             DatabaseCenter.Instance.GetUserDb().UserHeaderMap.AddOrUpdate(res.DepartedUser.AccountID, res.DepartedUser);
             return;
        }

        // or when the user is my party leader and there is any no member , remove the user from the party
        if (party.Leader.AccountID.Equals(req.DepartedUser.AccountID) && party.Members.Count is not 0)
        {
            UserHeader member = party.Members.FirstOrDefault().Value;
            if (member.AccountID.Equals(StringNullValue)) return;
            req.DepartedUser.PartyKey = StringNullValue;
            
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
            res.State = SuccessState;
            res.DepartedUser = req.DepartedUser;
            res.PartyName = req.PartyName;
            _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_EXIT_PARTY);
            DatabaseCenter.Instance.GetUserDb().UserHeaderMap.AddOrUpdate(res.DepartedUser.AccountID, res.DepartedUser);
            
            FileServer.DebugLog($@"
[Exit Party]
    - member              : {req.DepartedUser.AccountID}
    - newLeader           : {party.Leader.AccountID}");
            return;
        }

        // delete the party when the user is my party leader and there is no member
        new DeleteFileProcessor().RemovePartyUploadedFiles(party.Key);
        // Remove Party
        DatabaseCenter.Instance.GetPartyDb().PartyMap.Remove(req.PartyName);
    }
}
