namespace OFrameLibrary
{
    public static class Constants
    {
        public static class Keys
        {
            public const string AnonymousUserIDCookieKey = "_AnonymousUserID__";
            public const string AvatarPathPerformanceKey = "_AvatarPathPerformanceKey__";
            public const string CurrentCultureCookieKey = "_CurrentCulture__";
            public const string CurrentCultureDirectionCookieKey = "_CurrentCultureDirection__";
            public const string GridViewSortDirection = "_GridViewSortDirection__";
            public const string GridViewSortExpression = "_GridViewSortExpression__";
            public const string GuestEmailIDCookieKey = "_GuestEmailID__";
            public const string PrintSettingsKey = "_PrintSettings__";
            public const string CurrentCultureSessionKey = "_CurrentCultureSessionKey__";
        }

        public static class Messages
        {
            public const string ADD_SUCCESS_MESSAGE = "Item added successfully";
            public const string DELETE_SUCCESS_MESSAGE = "Item deleted successfully";
            public const string ITEM_ALREADY_PRESENT = "Item already present";
            public const string ITEM_NOT_EXISTS_MESSAGE = "Item does not exists";
            public const string RELATED_RECORD_EXISTS_MESSAGE = "Item cannot be deleted as it has related records";
            public const string SAVE_SUCCESS_MESSAGE = "Item saved successfully";
        }
    }
}