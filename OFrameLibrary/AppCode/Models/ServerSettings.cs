using System;

namespace OFrameLibrary.Models
{
    [Serializable]
    public class ServerSettings
    {
        public string Domain
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }

        public bool IsHttp
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public string RootDirectory
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }
    }
}
