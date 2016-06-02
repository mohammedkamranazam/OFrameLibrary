namespace OFrameLibrary.Models
{
    public class FileUploadSettings
    {
        /// <summary>
        /// Max File Size In MB
        /// </summary>
        public int MaxSize { get; set; }

        public string StoragePath { get; set; }
    }
}