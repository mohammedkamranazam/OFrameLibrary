namespace OFrameLibrary.Models
{
    public class JSONConfirmation
    {
        public JSONConfirmation() { }

        public JSONConfirmation(bool isSuccess, string message)
        {
            Message = message;
            IsSuccess = isSuccess;
        }

        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public object Object { get; set; }
    }
}