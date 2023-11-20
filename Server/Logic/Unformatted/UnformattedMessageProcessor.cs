public class UnformattedMessageProcessor : IDisposable
{
    public event Action<IReadOnlyDictionary<string, string>>? OnProcessed;
    public event Action? OnSendMessage;
    public void ProcessUnformattedMessage(RequestUnformattedMessage? req, MessageReceivedEventArgs e)
    {
        ResponseUnformattedMessage
            res = new ResponseUnformattedMessage(req.ID, e.Client.ID, req.State, SuccessResponse);

        OnProcessed?.Invoke(req.Map);

        new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_UNFORMATTED_MESSAGE);

        OnSendMessage?.Invoke();
    }

    public void Dispose()
    {
        OnProcessed = null;
        OnSendMessage = null;
    }
}