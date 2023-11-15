public static class PartyLogic
{
    public static void ProcessCreatePartyRequest(RequestCreateParty req, MessageReceivedEventArgs e)
    {
        //Update User, Party Info
        NetworkData.UpdateParty(ret.MyParty.connectionKey, ret.MyParty);
        NetworkData.UpdateAccountIDToClient(ret.MyParty.leaderInfo.AccountID, e.Client);
        NetworkData.UpdateClientToParty(e.Client.ID, ret.MyParty);
        FileServerPlugin.Debug($"Create :: {ret.MyParty.leaderInfo.AccountID} | {ret.MyParty.connectionKey}", ConsoleColor.Green);
    }
    public static void ProcessJoinPartyResult(byte[] decryptedData, MessageReceivedEventArgs e)
    {
        var ret = decryptedData.Deserializer<PartyData.JoinPartyResult>();
        if (!ret.isSuccess) return;
        
        //Update User, Party Info
        NetworkData.UpdateParty(ret.JoinedParty.connectionKey, ret.JoinedParty);
        NetworkData.UpdateAccountIDToClient(ret.JoinedUserInfo.AccountID, e.Client);
        NetworkData.UpdateClientToParty(e.Client.ID, ret.JoinedParty);
        FileServerPlugin.Debug($"Join :: {ret.JoinedUserInfo.AccountID} to {ret.JoinedParty.connectionKey}", ConsoleColor.Green);
    }
    
    public static void ProcessExitPartyResult(byte[] decryptedData, MessageReceivedEventArgs e)
    {
        var ret = decryptedData.Deserializer<PartyData.ExitPartyResult>();

        if (!ret.isSuccess) return;

        var party = NetworkData.GetPartyByName(ret.PartyName);
        if (party.membersInfos.Contains(ret.ExitUserInfo))
        {
            FileServerPlugin.Debug($"Member Exit :: {ret.ExitUserInfo.AccountID} from {party.connectionKey}");
            // remove User & Update Party Info
            var removedClient = NetworkData.GetClientByID(ret.ExitUserInfo.AccountID);
            NetworkData.RemoveAccountIDToClient(ret.ExitUserInfo.AccountID);
            NetworkData.RemoveClientToParty(removedClient.ID);
            party.membersInfos.Remove(ret.ExitUserInfo);
            NetworkData.UpdateParty(party.connectionKey, party);
            return;
        }

        if (!party.leaderInfo.AccountID.Equals(ret.ExitUserInfo.AccountID) && party.membersInfos.Count != 0) return;
        
        var removedLeader = NetworkData.GetClientByID(party.leaderInfo.AccountID);
        // remove User & Party Info 
        NetworkData.RemoveAccountIDToClient(party.leaderInfo.AccountID);
        NetworkData.RemoveClientToParty(removedLeader.ID);
        FileServerPlugin.Debug($"Leader Exit :: {party.leaderInfo.AccountID} to {party.connectionKey}");
         DeleteFileLogic.RemoveDatas(party.connectionKey);
        NetworkData.RemoveParty(party.connectionKey);
    }
    
    public static void ProcessRemoveMemberResult(byte[] decryptedData, MessageReceivedEventArgs e)
    {
        var ret = decryptedData.Deserializer<PartyData.RemovePartyMemberResult>();

        if (!ret.isSuccess) return;
        var party = NetworkData.GetPartyByName(ret.PartyName);
        if (!party.membersInfos.Contains(ret.RemovedUserInfo) && party.membersInfos.Count != 0) return;

        party.membersInfos.Remove(ret.RemovedUserInfo);
        FileServerPlugin.Debug($"Remove Member :: {ret.RemovedUserInfo.AccountID} to {party.connectionKey}");
        // remove User & Update Party Info
        NetworkData.UpdateParty(party.connectionKey, party);
        var removedClient = NetworkData.GetClientByID(ret.RemovedUserInfo.AccountID);
        NetworkData.RemoveAccountIDToClient(ret.RemovedUserInfo.AccountID);
        NetworkData.RemoveClientToParty(removedClient.ID);
    }
    public static void ProcessDestroyPartyResult(byte[] decryptedData, MessageReceivedEventArgs e)
    {
        var ret = decryptedData.Deserializer<PartyData.DestroyPartyResult>();

        if (!ret.isSuccess) return;

        //Remove Leader, member Party Info
        var party = NetworkData.GetPartyByName(ret.PartyName);
        var removedLeader = NetworkData.GetClientByID(party.leaderInfo.AccountID);
        NetworkData.RemoveAccountIDToClient(party.leaderInfo.AccountID);
        NetworkData.RemoveClientToParty(removedLeader.ID);
        
        foreach (var m in party.membersInfos)
        {
            var removedClient = NetworkData.GetClientByID(m.AccountID);
            NetworkData.RemoveAccountIDToClient(m.AccountID);
            NetworkData.RemoveClientToParty(removedClient.ID);
        }
        FileServerPlugin.Debug($"Party Destroyed :: {party.leaderInfo} to {party.connectionKey}");
        DeleteFileLogic.RemoveDatas(party.connectionKey);
        NetworkData.RemoveParty(party.connectionKey);
    }
}