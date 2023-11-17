
public class PartyFinder
{
    public void ProcessSearchingPartyListRequest(RequestPartySearching? req, MessageReceivedEventArgs e)
    {
        ResponsePartySearching res = new ResponsePartySearching(e.Client.ID);
        if (DatabaseCenter.Instance.GetPartyDb().PartyMap.Count is 0)
        {
            res.SearchingPartyArray = new[] { new Party() };
            res.State = 0;
            res.Log = "There is No Party";
        }

        else
        {
            res.SearchingPartyArray 
                = !req.Keyword.Equals(StringNullValue) ?
                    DatabaseCenter.Instance.GetPartyDb().PartyMap.Values
                        .Where(party => party.Name.Contains(req.Keyword))
                        .ToArray() 
                    : DatabaseCenter.Instance.GetPartyDb().PartyMap.Values;
            res.State = 1;
        }
        
        _ = new ServerWriter().SendMessage(e.Client, res, Tags.RESPONSE_PARTY_LIST);
    }
}
