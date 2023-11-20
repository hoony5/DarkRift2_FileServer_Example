
public class AdvertisementProcessor
{
    public void ProcessAdvertisement(RequestAdvertiseUploadCompletion? req, MessageReceivedEventArgs e)
    {
        ResponseAdvertiseUploadCompletion res
            = new ResponseAdvertiseUploadCompletion(
                req.FileFullName,
                e.Client.ID,
                $"Received Message :: {req.ID} | {req.FileFullName}",
                SuccessState);

        _ = new ServerEncryptedDtoWriter().SendMessage(e.Client, res, Tags.RESPONSE_ADVERTISE_UPLOAD_COMPLETION);
    }
}