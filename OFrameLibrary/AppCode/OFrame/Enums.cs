namespace OFrameLibrary
{

    public enum EventSchedule
    {
        UpComing,
        Past,
        All,
        Continuing,
    }


    public enum FileType
    {
        Image,
        Document,
        PDF,
        Custom,
        All
    }

    public enum Gender
    {
        Male,
        Female,
        Unspecified,
    }

    public enum HashServiceProvider
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        MD5,
    }

    public enum LogType
    {
        Error,
        Activity
    }


    public enum MemoryCacheItemPriority
    {
        Default = 1,
        NotRemovable = 2,
    }


    public enum PerformanceMode
    {
        None = 0,
        ApplicationState = 1,
        Cache = 2,
        MemoryCache = 3,
        Session = 4,
        Redis = 5
    }

    public enum StatusMessageType
    {
        Info,
        Error,
        Success,
        Warning,
    }

    public enum SymCryptographyServiceProvider
    {
        Rijndael,
        RC2,
        DES,
        TripleDES,
    }

}
