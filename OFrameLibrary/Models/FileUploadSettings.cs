namespace OFrameLibrary.Models
{
    public class FileUploadSettings
    {
        /// <summary>
        /// Max File Size In MB
        /// </summary>
        public int MaxSize { get; set; }

        public string StoragePath { get; set; }

        public FileUploadMessageSettings Messages { get; set; }

        public FileType FileType { get; set; } = FileType.All;
    }
}