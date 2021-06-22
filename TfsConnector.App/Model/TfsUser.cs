using System.Configuration;

namespace TfsConnector.App.Model
{
    public static class TfsUser
    {
        public static string UserName = ConfigurationManager.AppSettings["User"];

        public static string UserPassword = ConfigurationManager.AppSettings["Password"];

        public static string Domain = ConfigurationManager.AppSettings["Domain"];
        
        public static string TfsUrl = ConfigurationManager.AppSettings["TfsUrl"];
    }
}
