using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using OfficeOpenXml;
using TfsConnector;

namespace BasicSccExample
{
    class Example
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Digite os changeSets que deseja aplicar no outros ambientes separando por vírgula e depois enter:");
            var changesSetsInput = Console.ReadLine();
            var list = changesSetsInput.Split(',').Select(_ => int.Parse(_.Trim())).OrderByDescending(_ => _).ToArray();
            var result = ListFilesFromChangeSets(list);

            Console.WriteLine("Starting.");
            GetCurrentChangeSet("OutSource/Accenture", result);
            GetCurrentChangeSet("HML", result);
            GetCurrentChangeSet("DEV", result);
            Console.WriteLine("Finishing.");
            var folder = GetFolder();
            var file = CreateFile(result.Where(_ => _.ObjectName != "").ToList(), folder, "merge");
            Console.WriteLine($"Created File:  {file}");
            System.Diagnostics.Process.Start($@"{folder}\{file}");

            Console.ReadLine();
        }

        private static string GetFolder()
        {
            var directory = Directory.GetCurrentDirectory();
            directory = Path.Combine(directory, "Out");
            if (Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return directory;

        }

        public static List<MergeSheet> ListFilesFromChangeSets(int[] codes)
        {
            var result = new List<MergeSheet>();
            Uri serverUri = new Uri("http://tfs.fleury.com.br:8080/tfs/GrupoFleury");
            NetworkCredential cred = new NetworkCredential(TfsUser.UserName, TfsUser.UserPassword, TfsUser.Domain);
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(serverUri, cred);
            tpc.EnsureAuthenticated();
            VersionControlServer vcs = tpc.GetService<VersionControlServer>();

            foreach (var code in codes)
            {
                var changeset = vcs.GetChangeset(code);

                foreach (var item in changeset.Changes)
                {
                    if (!result.Any(_ => _.PathTfs == item.Item.ServerItem))
                        result.Add(new MergeSheet() { PathTfs = item.Item.ServerItem, TargetChangeSet = code.ToString(), PathTfsFull = item.Item.ServerItem });
                }
            }
            return result;
        }

        public static void GetCurrentChangeSet(string ENV, List<MergeSheet> result)
        {
            Uri serverUri = new Uri("http://tfs.fleury.com.br:8080/tfs/GrupoFleury");
            NetworkCredential cred = new NetworkCredential(TfsUser.UserName, TfsUser.UserPassword, TfsUser.Domain);
            TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(serverUri, cred);
            tpc.EnsureAuthenticated();
            VersionControlServer vcs = tpc.GetService<VersionControlServer>();

            foreach (var item in result)
            {
                var changeSet = "N/A";
                try
                {
                    var index = ENV.Equals("OutSource/Accenture") ? 1 : 0;
                    var changes = vcs.QueryHistory(item.PathTfsFull.Replace("OutSource/Accenture", ENV), RecursionType.Full);
                    var c = changes.ToList();
                    changeSet = c[index].ChangesetId.ToString();
                }
                catch (Exception)
                {
                }

                switch (ENV)
                {
                    case "HML":
                        item.HMLBranchChangeSet = changeSet;
                        break;
                    case "DEV":
                        item.DevBranchChangeSet = changeSet;
                        break;
                    default:
                        item.OutSourceBranchChangeSet = changeSet;
                        break;
                }

                item.SplitObject();

            }

        }

        private static string CreateFile(List<MergeSheet> list, string path, string fileName)
        {
            var filename = $"{fileName}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm")}.xlsx";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MemoryStream stream = new MemoryStream())
            {
                ExcelPackage p = new ExcelPackage();
                var ws = p.Workbook.Worksheets.Add("table1");
                ws.Cells.AutoFitColumns();

                int line = 1;

                ws.Cells[1, 1].Value = "Rollback:";
                ws.Cells[3, 1].Value = "Caminho TFS – Branch";
                ws.Cells[3, 2].Value = "Objeto";
                ws.Cells[3, 3].Value = "ChangeSet";
                ws.Cells[3, 4].Value = "ChangeSet";
                ws.Cells[3, 5].Value = "ChangeSet";
                ws.Cells[4, 3].Value = "Esteira";
                ws.Cells[4, 4].Value = "DEV";
                ws.Cells[4, 5].Value = "HML";

                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells[3, 1].Style.Font.Bold = true;
                ws.Cells[3, 2].Style.Font.Bold = true;
                ws.Cells[3, 3].Style.Font.Bold = true;
                ws.Cells[3, 4].Style.Font.Bold = true;
                ws.Cells[3, 5].Style.Font.Bold = true;
                ws.Cells[4, 3].Style.Font.Bold = true;
                ws.Cells[4, 4].Style.Font.Bold = true;
                ws.Cells[4, 5].Style.Font.Bold = true;

                ws.Cells[1, 1].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[3, 1].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[3, 2].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[3, 3].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[3, 4].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[3, 5].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[4, 3].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[4, 4].Style.Font.Color.SetColor(Color.OrangeRed);
                ws.Cells[4, 5].Style.Font.Color.SetColor(Color.OrangeRed);

                line = 6;

                foreach (var item in list)
                {
                    ws.Cells[line, 1].Value = item.PathTfs;
                    ws.Cells[line, 2].Value = item.ObjectName;
                    ws.Cells[line, 3].Value = item.OutSourceBranchChangeSet;
                    ws.Cells[line, 4].Value = item.DevBranchChangeSet;
                    ws.Cells[line, 5].Value = item.HMLBranchChangeSet;
                    line++;
                }

                line += 3;
                ws.Cells[line, 1].Value = "Implantação:";
                ws.Cells[line + 3, 1].Value = "Caminho TFS – Branch";
                ws.Cells[line + 3, 2].Value = "Objeto";
                ws.Cells[line + 3, 3].Value = "ChangeSet";
                ws.Cells[line + 3, 4].Value = "ChangeSet";
                ws.Cells[line + 3, 5].Value = "ChangeSet";
                ws.Cells[line + 4, 3].Value = "Esteira";
                ws.Cells[line + 4, 4].Value = "DEV";
                ws.Cells[line + 4, 5].Value = "HML";

                line += 6;

                foreach (var item in list)
                {
                    ws.Cells[line, 1].Value = item.PathTfs;
                    ws.Cells[line, 2].Value = item.ObjectName;
                    ws.Cells[line, 3].Value = item.TargetChangeSet;
                    line++;
                }


                p.SaveAs(stream);


                using (FileStream file = new FileStream(path + "/" + filename, FileMode.OpenOrCreate))
                {
                    stream.Position = 0;
                    stream.Read(stream.ToArray(), 0, (int)stream.Length);
                    file.Write(stream.ToArray(), 0, stream.ToArray().Length);
                    stream.Close();
                    file.Flush();
                    file.Close();
                }
                return filename;
            }

        }

    }
}
