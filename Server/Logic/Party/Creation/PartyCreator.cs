public class PartyCreator
{
    private const ushort PUBLIC_PARTY = 1;
    private const ushort PRIVATE_PARTY = 0;
    
    public void ProcessCreatePartyRequest(RequestCreateParty? req, MessageReceivedEventArgs e)
    {
        Party createdParty 
            = new Party(
                req.PartyName,
                req.Password,Guid.NewGuid().ToString(),
                req.MaxPartyPlayers,
                req.IsPublic ? PUBLIC_PARTY : PRIVATE_PARTY,
                req.Leader.Header);
        // update UserHeader
        req.Leader.Header.PartyKey = createdParty.Key;
        // update Player Count
        createdParty.UpdateCurrentPlayers();
        
        DatabaseCenter.Instance.GetPartyDb().PartyMap.AddOrUpdate(createdParty.Name, createdParty);
        DatabaseCenter.Instance.GetUserDb().UserHeaderMap.AddOrUpdate(createdParty.Leader.AccountID, createdParty.Leader);
        
        // Reply
        ResponseCreateParty res = new ResponseCreateParty(createdParty, e.Client.ID);
        res.State = 1;
        new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_CREATE_PARTY);
        
        // Log
        FileServer.DebugLog($@"
[Created Party]
    - name              : {createdParty.Name}
    - password          : {createdParty.Password} 
    - key               : {createdParty.Key}
    - state             : {createdParty.State}
    - leader            : {createdParty.Leader.AccountID}
    - max               : {createdParty.MaxPlayers}");
    }
}
