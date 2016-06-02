namespace OFrameLibrary.Models
{
    public class StatusMessage
    {
        public StatusMessage(string message, StatusMessageType messageType = StatusMessageType.Info)
        {
            Message = message;
            MessageType = messageType;
        }

        public string Message { get; set; }
        public StatusMessageType MessageType { get; set; }
    }
}