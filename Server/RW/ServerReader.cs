
public class ServerReader
{
    public void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        using (Message? msg = e.GetMessage())
        {
            using (DarkRiftReader? reader = msg.GetReader())
            {
                FileServer.DebugLog($"OnMessageReceived - ClientID: {e.Client.ID} | Tag: {(Tags)e.Tag}");
                switch ((Tags)e.Tag)
                {
                    #region Processing Party
                    case Tags.REQUEST_CREATE_PARTY:
                        new PartyCreator().ProcessCreatePartyRequest(reader.ReadSerializable<RequestCreateParty>(), e);
                        break;
                    case Tags.REQUEST_PARTY_LIST:
                        new PartyFinder().ProcessSearchingPartyListRequest(reader.ReadSerializable<RequestPartySearching>(), e);
                        break;
                    case Tags.REQUEST_JOIN_PARTY:
                        new PartyJoinProcessor().ProcessJoinPartyRequest(reader.ReadSerializable<RequestJoinParty>(), e);
                        break;
                    case Tags.REQUEST_JOIN_PARTY_WITH_PASSWORD:
                        new PartyJoinWithPasswordProcessor().ProcessJoinPartyWithPasswordRequest(reader.ReadSerializable<RequestJoinPartyWithPassword>(), e);
                        break;
                    case Tags.REQUEST_EXIT_PARTY:
                        new ExitPartyProcessor().ProcessExitPartyRequest(reader.ReadSerializable<RequestExitParty>(), e);
                        break;
                    case Tags.REQUEST_REMOVE_MEMBER:
                        new RemoveMemberProcessor().ProcessRemoveMemberRequest(reader.ReadSerializable<RequestRemoveMember>(), e);
                        break;
                    case Tags.REQUEST_DESTROY_PARTY:
                        new DestroyPartyProcessor().ProcessDestroyPartyRequest(reader.ReadSerializable<RequestDestroyParty>(), e);
                        break;
                    #endregion

                    #region Processing Upload File
                    case Tags.REQUEST_UPLOAD_ACCEPT:
                        break;
                    case Tags.REQUEST_UPLOAD_FILE:
                        break;
                    #endregion

                    #region Processing Download File
                    case Tags.REQUEST_DOWNLOAD_ACCEPT:
                        break;
                    case Tags.REQUEST_DOWNLOAD_FILE:
                        break;
                    #endregion

                    #region Processing Search File
                    case Tags.REQUEST_FILE_SEARCHING:
                        break;
                    #endregion

                    #region Processing Delete File
                    case Tags.REQUEST_DELETE_ALL_FILES:
                        break;
                    #endregion

                    #region Processing Advertise Done File upload
                    case Tags.REQUEST_ADVERTISE_UPLOAD_COMPLETION:
                        break;
                    #endregion

                    #region Processing File Share
                    case Tags.REQUEST_FILE_SHARE_START:
                        break;
                    case Tags.REQUEST_UPDATE_FILE_SHARE_PROGRESS:
                        break;
                    case Tags.REQUEST_FILE_CURRENT_SHARED_PROGRESS:
                        break;
                    #endregion

                    #region Processing File Share End
                    case Tags.REQUEST_FILE_SHARE_END:
                        break;
                    #endregion
                    default:
                        FileServer.DebugLog($"Recv Tag : {(Tags)e.Tag} | Current Server Version is not supported the tag.");
                        break;
                }
            }
        }
    }
}