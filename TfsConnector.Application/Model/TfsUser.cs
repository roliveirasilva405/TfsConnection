using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsConnector.Application.Model
{
    public static class TfsUser
    {
        public static string UserName = ConfigurationManager.AppSettings["User"];

        public static string UserPassword = ConfigurationManager.AppSettings["Password"];

        public static string Domain = ConfigurationManager.AppSettings["Domain"];
        
        public static string TfsUrl = ConfigurationManager.AppSettings["TfsUrl"];
    }
}
