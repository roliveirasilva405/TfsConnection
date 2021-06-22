using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TfsConnector.App.Extensions
{
    public static class StringExtensions
    {
        public static string ToSourcePath(this string self, string ENV)
        {
            if (ENV.Equals("OutSource/Accenture") && self.Contains("EsteiraAgil"))
                return self.Replace("EsteiraAgil/", "").Replace("/Fontes/", "/" + ENV + "/Fontes/");

            return self.Replace("OutSource/Accenture", ENV);

        }

        public static string FileToHash(this string filePath)
        {
            MD5 Md5 = MD5.Create();
            var hash = "";
            using (FileStream stream = File.OpenRead(filePath))
            {
                hash = Encoding.Default.GetString(Md5.ComputeHash(stream));
            }
            return hash;
        }
    }
}
