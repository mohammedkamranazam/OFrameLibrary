using OFrameLibrary.SettingsHelpers;

namespace OFrameLibrary
{
    public static class LocalStorages
    {
        public static string Storage
        {
            get
            {
                return LocalStoragesHelper.GetStoragePath("Storage");
            }

            set
            {
                LocalStoragesHelper.SetStoragePath("Storage", value);
            }
        }

        public static string Storage_Logs
        {
            get
            {
                return LocalStoragesHelper.GetStoragePath("Storage_Logs");
            }

            set
            {
                LocalStoragesHelper.SetStoragePath("Storage_Logs", value);
            }
        }

        public static string Storage_Temp
        {
            get
            {
                return LocalStoragesHelper.GetStoragePath("Storage_Temp");
            }

            set
            {
                LocalStoragesHelper.SetStoragePath("Storage_Temp", value);
            }
        }

        public static string Storage_Uploads
        {
            get
            {
                return LocalStoragesHelper.GetStoragePath("Storage_Uploads");
            }

            set
            {
                LocalStoragesHelper.SetStoragePath("Storage_Uploads", value);
            }
        }
    }
}
