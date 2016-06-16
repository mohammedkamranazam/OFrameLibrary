namespace OFrameLibrary.Models
{
    public class FileUploadSettings
    {
        /// <summary>
        /// Max File Size In MB. Default to 5MB
        /// </summary>
        public int MaxSize { get; set; } = 5;

        /// <summary>
        /// Path where to store the file on the server. Default to ~/Storage/
        /// </summary>
        public string StoragePath { get; set; } = LocalStorages.Storage;

        /// <summary>
        /// File Upload status message settings
        /// </summary>
        public FileUploadMessageSettings Messages { get; set; } = new FileUploadMessageSettings();

        /// <summary>
        /// Type of file such as PDF, Images or Docs
        /// </summary>
        public FileType FileType { get; set; } = FileType.All;
    }
}