using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpConfig;
using TfsConnector.App.Model;

namespace TfsConnector.ui
{
    public static class FileConfig
    {
        public static void SaveData(TfsUser tfsUser, string caminhoArquivo)
        {
            Configuration cfg = new Configuration();
            cfg["conexao"]["user"].StringValue = tfsUser.UserName;
            cfg["conexao"]["password"].StringValue = Criptografia.Encrypt(tfsUser.UserPassword);
            cfg["conexao"]["domain"].StringValue = tfsUser.Domain;
            cfg["conexao"]["url"].StringValue = tfsUser.TfsUrl;
            cfg["conexao"]["filePath"].StringValue = caminhoArquivo;
            cfg.SaveToFile("UserConfig.cfg");
        }

        public static Tuple<TfsUser, string> LoadData()
        {
            var tfsUser = new TfsUser();
            Configuration cfg = Configuration.LoadFromFile("UserConfig.cfg");
            tfsUser.UserName = cfg["conexao"]["user"].StringValue;
            tfsUser.UserPassword = Criptografia.Decript(cfg["conexao"]["password"].StringValue);
            tfsUser.Domain = cfg["conexao"]["domain"].StringValue;
            tfsUser.TfsUrl = cfg["conexao"]["url"].StringValue;
            return new Tuple<TfsUser, string>(tfsUser, cfg["conexao"]["filePath"].StringValue);
        }
    }
}
