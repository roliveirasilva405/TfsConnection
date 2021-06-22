using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TfsConnector.Application.Extensions
{
    public static class StringExtensions
    {
        private static string ToSourcePath(this string self, string ENV)
        {
            if (ENV.Equals("OutSource/Accenture") && self.Contains("EsteiraAgil"))
                return self.Replace("EsteiraAgil/", "").Replace("/Fontes/", "/" + ENV + "/Fontes/");

            return self.Replace("OutSource/Accenture", ENV);

        }

        private static string FileToHash(string file)
        {
            MD5 Md5 = MD5.Create();
            var hash = "";
            using (FileStream stream = File.OpenRead(file))
            {
                hash = Encoding.Default.GetString(Md5.ComputeHash(stream));
            }
            return hash;
        }
    }
}
