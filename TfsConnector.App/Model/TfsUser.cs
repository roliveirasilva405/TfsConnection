using System.Configuration;

namespace TfsConnector.App.Model
{
    public class TfsUser
    {
        public string UserName { get; set; }    //ConfigurationManager.AppSettings["User"];
                                         
        public string UserPassword { get; set; } //ConfigurationManager.AppSettings["Password"];

        public string Domain { get; set; }       //ConfigurationManager.AppSettings["Domain"];

        public string TfsUrl { get; set; }       //ConfigurationManager.AppSettings["TfsUrl"];
    }
}
