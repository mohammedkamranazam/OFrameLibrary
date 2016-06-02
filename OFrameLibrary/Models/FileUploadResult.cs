namespace OFrameLibrary.Models
{
    public class FileUploadResult
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Result { get; set; }

        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public long ID { get; set; }
    }
}