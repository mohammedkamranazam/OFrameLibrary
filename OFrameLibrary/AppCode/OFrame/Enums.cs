namespace OFrameLibrary
{
    public enum FileType
    {
        Image,
        Document,
        PDF,
        Custom,
        All
    }

    public enum ManageMessageId
    {
        AddPhoneSuccess,
        ChangePasswordSuccess,
        SetTwoFactorSuccess,
        SetPasswordSuccess,
        RemoveLoginSuccess,
        RemovePhoneSuccess,
        Error
    }

    public enum BlinkRate
    {
        None,
        Slow,
        Regular,
        Fast,
    }

    public enum EventSchedule
    {
        UpComing,
        Past,
        All,
        Continuing,
    }

    public enum FieldWidth
    {
        xxsmall,
        xsmall,
        small,
        medium,
        large,
        largeXL,
        full,
    }

    public enum Gender
    {
        Male,
        Female,
        Unspecified,
    }

    public enum HashServiceProvider : int
    {
        SHA1,
        SHA256,
        SHA384,
        SHA512,
        MD5,
    }

    public enum MemoryCacheItemPriority
    {
        Default = 1,
        NotRemovable = 2,
    }

    public enum MessageColor
    {
        None,
        White,
        Black,
        Blue,
        Orange,
        Yellow,
        Red,
        Green,
    }

    public enum PageSetting
    {
        Add = 0,
        List = 1,
        Manage = 2,
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

    public enum SymCryptographyServiceProvider : int
    {
        Rijndael,
        RC2,
        DES,
        TripleDES,
    }

    public enum TipPosition
    {
        Left = 0,
        Right = 2,
    }

    public enum LogType
    {
        Error,
        Activity
    }
}