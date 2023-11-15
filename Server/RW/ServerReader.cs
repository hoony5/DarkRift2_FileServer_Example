using DarkRift;
using Model;
using Server;

public class ServerReader(FileServer server)
{
    private FileServer _server = server;
    public void OnMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        using (Message? msg = e.GetMessage())
        {
            using (DarkRiftReader? reader = msg.GetReader())
            {
                switch ((Tags)e.Tag)
                {
                    #region Processing Upload File 
                    case Tags.REQUEST_UPLOAD_VALIDATION:
                        break;
                    case Tags.RESPONSE_UPLOAD_VALIDATION:
                        break;
                    case Tags.REQUEST_UPLOAD_FILE:
                        break;
                    case Tags.RESPONSE_UPLOAD_FILE:
                        break;
                    #endregion
                    
                    #region Processing Download File
                    case Tags.REQUEST_DOWNLOAD_VALIDATION:
                        break;
                    case Tags.RESPONSE_DOWNLOAD_VALIDATION:
                        break;
                    case Tags.REQUEST_DOWNLOAD_FILE:
                        break;
                    case Tags.RESPONSE_DOWNLOAD_FILE:
                        break;
                    #endregion
                    
                    #region Processing Search File
                    case Tags.REQUEST_FILE_SEARCHING:
                        break;
                    case Tags.RESPONSE_FILE_SEARCHING:
                        break;
                    case Tags.REQUEST_CHECK_FILE_EXIST:
                        break;
                    case Tags.RESPONSE_CHECK_FILE_EXIST:
                        break;
                    #endregion
                    
                    #region Processing Delete File
                    case Tags.REQUEST_DELETE_ALL_FILES:
                        break;
                    case Tags.RESPONSE_DELETE_ALL_FILES:
                        break;
                    #endregion
                    
                    #region Processing Advertise Done File upload
                    case Tags.REQUEST_ADVERTISE_UPLOAD_COMPLETION:
                        break;
                    case Tags.RESPONSE_ADVERTISE_UPLOAD_COMPLETION:
                        break;
                    #endregion
                    
                    #region Processing File Share
                    case Tags.REQUEST_FILE_SHARE_START:
                        break;
                    case Tags.RESPONSE_FILE_SHARE_START:
                        break;
                    case Tags.REQUEST_UPDATE_FILE_SHARE_PROGRESS:
                        break;
                    case Tags.RESPONSE_UPDATE_FILE_SHARE_PROGRESS:
                        break;
                    case Tags.REQUEST_FILE_CURRENT_SHARED_PROGRESS:
                        break;
                    case Tags.RESPONSE_FILE_CURRENT_SHARED_PROGRESS:
                        break;
                    #endregion
                    
                    #region Processing File Share End
                    case Tags.REQUEST_FILE_SHARE_END:
                        break;
                    case Tags.RESPONSE_FILE_SHARE_END:
                        break;
                    #endregion
                    default:
                        _server.DebugLog($"Recv Tag : {(Tags)e.Tag}"
                            , new ServerReaderException($"Current Server Version is not supported the tag."));
                        break;
                }
            }
        }
    }
}