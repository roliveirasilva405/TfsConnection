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
using OfficeOpenXml.Style;
using System.Security.Cryptography;
using System.Text;


namespace BasicSccExample
{
    class Example
	{
		
		public static VersionControlServer vcs;
		static void Main(string[] args)
		{

			Console.WriteLine("Digite os changeSets que deseja aplicar no outros ambientes separando por vírgula e depois enter:");
			var changesSetsInput = Console.ReadLine();
			var list = changesSetsInput.Split(',').Select(_ => int.Parse(_.Trim())).OrderByDescending(_ => _).ToArray();

			Uri serverUri = new Uri(TfsUser.TfsUrl);
			NetworkCredential cred = new NetworkCredential(TfsUser.UserName, TfsUser.UserPassword, TfsUser.Domain);
			TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(serverUri, cred);
			tpc.EnsureAuthenticated();
			vcs = tpc.GetService<VersionControlServer>();

			var result = ListFilesFromChangeSets(list);

			Console.WriteLine("Starting.");
			GetCurrentChangeSet("OutSource/Accenture", result);
			GetCurrentChangeSet("HML", result);
			GetCurrentChangeSet("DEV", result);

            foreach (var item in result)
            {
				item.TargetHash = GetHashFromFile(item.PathTfsFull, item.TargetChangeSet);
            }

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

		public static List<MergeSheet> ListFilesFromChangeSets(int[] codes)
		{
			var result = new List<MergeSheet>();
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

		public static void manageDirectory(string path)
        {
			try
			{
				if (Directory.Exists(path))
					Directory.Delete(path, true);

				Directory.CreateDirectory(path);
			}
			catch (Exception ex)
			{
			}
		}

		public static void GetCurrentChangeSet(string ENV, List<MergeSheet> result)
		{

			foreach (var item in result)
			{
				var changeSet = "N/A";
				var hash = "";
				try
                {
                    var index = ENV.Equals("OutSource/Accenture") ? 1 : 0;
					var pathTfsFull = item.PathTfsFull.Replace("OutSource/Accenture", ENV);
					var changes = vcs.QueryHistory(pathTfsFull, RecursionType.Full);
                    var c = changes.ToList();
                    changeSet = c[index].ChangesetId.ToString();                   
                    hash = GetHashFromFile(pathTfsFull, changeSet);
                }
                catch (Exception ex)
				{
				}

				switch (ENV)
				{
					case "HML":
						item.HMLBranchChangeSet = changeSet;
						item.HMLBranchHash = hash;
						break;
					case "DEV":
						item.DevBranchChangeSet = changeSet;
						item.DevBranchHash = hash;
						break;
					default:
						item.OutSourceBranchOldChangeSet = changeSet;
						item.OutSourceBranchOldHash = hash;
						break;
				}

				item.SplitObject();

			}

		}

        private static string GetHashFromFile(string PathTfsFull, string changeSet)
        {
			var splittedPath = PathTfsFull.Split('/');
			var objectName = splittedPath[splittedPath.Length - 1];
			var path = Directory.GetCurrentDirectory();
			path = $@"{path}\file";
			var hash = "";

			if (!string.IsNullOrEmpty(objectName) && objectName.Contains("."))
            {
                try
                {
					manageDirectory(path);
					vcs.DownloadFile(PathTfsFull, 0, VersionSpec.ParseSingleSpec($"C{changeSet}", null), $@"{path}\{objectName}");
                    hash = FileToHash($@"{path}\{objectName}");
                }
                catch (Exception)
                {
                }
            }

            return hash;
        }

		public static void formatCells(ExcelWorksheet ws)
        {
			try
			{
				ws.Cells[3, 1, 4, 1].Merge = true;
				ws.Cells[3, 2, 4, 2].Merge = true;

				ws.Cells[3, 1, 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
				ws.Cells[3, 1, 4, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

				ws.Cells[3, 2, 4, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
				ws.Cells[3, 2, 4, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

				ws.Cells[3, 3, 4, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

				ws.Cells[3, 1, 4, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color: Color.White);
				ws.Cells[3, 1, 4, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[3, 1, 4, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[3, 1, 4, 5].Style.Border.Left.Color.SetColor(Color.White);
				ws.Cells[3, 1, 4, 5].Style.Border.Right.Color.SetColor(Color.White);

				ws.Cells[3, 3, 3, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[3, 3, 3, 5].Style.Border.Bottom.Color.SetColor(Color.White);
				ws.View.ShowGridLines = false;

			}
			catch
			{
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
				ws.Column(1).Width = 120;
				ws.Column(2).Width = 50;
				ws.Column(3).Width = 12;
				ws.Column(4).Width = 12;
				ws.Column(5).Width = 12;

				ws.Column(1).Style.WrapText = true;

				int line = 1;

				ws.Cells[1, 1].Value = "Rollback:";
				ws.Cells[3, 1].Value = "Caminho TFS – Branch";
				ws.Cells[3, 2].Value = "Objeto";
				ws.Cells[3, 3, 3, 5].Value = "ChangeSet";
				ws.Cells[4, 3].Value = "Esteira";
				ws.Cells[4, 4].Value = "DEV";
				ws.Cells[4, 5].Value = "HML";

				ws.Cells[1, 1].Style.Font.Bold = true;
				ws.Cells[1, 1].Style.Font.UnderLine = true;
				ws.Cells[3, 1, 3, 5].Style.Font.Bold = true;
				ws.Cells[4, 3, 4, 5].Style.Font.Bold = true;

				ws.Cells[1, 1].Style.Font.Color.SetColor(Color.Black);
				ws.Cells[3, 1, 3, 5].Style.Font.Color.SetColor(Color.White);
				ws.Cells[4, 3, 4, 5].Style.Font.Color.SetColor(Color.White);

				ws.Cells[3, 1, 3, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
				ws.Cells[4, 3, 4, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;

				ws.Cells[3, 1, 3, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 150, 70));
				ws.Cells[4, 3, 4, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 150, 70));

				line = 5;

				foreach (var item in list.Where(_ => _.DevBranchHash != _.TargetHash || _.HMLBranchHash != _.TargetHash))
				{
					ws.Cells[line, 1, line, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
					ws.Cells[line, 1, line, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 212, 180));
					ws.Cells[line, 1, line, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

					ws.Cells[line, 1, line, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
					ws.Cells[line, 1, line, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[line, 1, line, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

					ws.Cells[line, 3, line, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

					ws.Cells[line, 1].Value = item.PathTfs;
					ws.Cells[line, 2].Value = item.ObjectName;
					ws.Cells[line, 3].Value = item.OutSourceBranchOldChangeSet;
					ws.Cells[line, 4].Value = item.DevBranchChangeSet;
					ws.Cells[line, 5].Value = item.HMLBranchChangeSet;
					ws.Row(line).Height = 35;
					line++;					
				}

				line += 3;

				ws.Cells[line + 3, 1, line + 4, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
				ws.Cells[line + 3, 1, line + 4, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 150, 70));
				ws.Cells[line,1 ].Style.Font.Color.SetColor(Color.Black);
				ws.Cells[line + 3, 1, line + 4, 5].Style.Font.Color.SetColor(Color.White);

				ws.Cells[line, 1].Value = "Implantação:";
				ws.Cells[line, 1].Style.Font.Bold = true;
				ws.Cells[line, 1].Style.Font.UnderLine = true;

				ws.Cells[line + 3, 1].Value = "Caminho TFS – Branch";
				ws.Cells[line + 3, 1, line + 4, 1].Merge = true;
				ws.Cells[line + 3, 1, line + 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
				ws.Cells[line + 3, 1, line + 4, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
				ws.Cells[line + 3, 1, line + 4, 5].Style.Font.Bold = true;

				ws.Cells[line + 3, 2].Value = "Objeto";
				ws.Cells[line + 3, 2, line + 4, 2].Merge = true;
				ws.Cells[line + 3, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

				ws.Cells[line + 3, 3, line + 3, 5].Value = "ChangeSet";
				ws.Cells[line + 4, 3].Value = "Esteira";
				ws.Cells[line + 4, 4].Value = "DEV";
				ws.Cells[line + 4, 5].Value = "HML";
				ws.Cells[line + 3, 3, line + 4, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

				ws.Cells[line + 3, 1, line + 4, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color: Color.White);
				ws.Cells[line + 3, 1, line + 4, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
				ws.Cells[line + 3, 1, line + 4, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
				ws.Cells[line + 3, 1, line + 4, 5].Style.Border.Left.Color.SetColor(Color.White);
				ws.Cells[line + 3, 1, line + 4, 5].Style.Border.Right.Color.SetColor(Color.White);

				ws.Cells[line + 3, 1, line + 4, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
				ws.Cells[line + 3, 1, line + 4, 5].Style.Border.Bottom.Color.SetColor(Color.White);

				line += 5;

				foreach (var item in list.Where(_ => _.DevBranchHash != _.TargetHash || _.HMLBranchHash != _.TargetHash))
				{
					ws.Cells[line, 1, line, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
					ws.Cells[line, 1, line, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 212, 180));

					ws.Cells[line, 1, line, 5].Style.Border.BorderAround(ExcelBorderStyle.Thin);
					ws.Cells[line, 1, line, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					ws.Cells[line, 1, line, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;

					ws.Cells[line, 1, line, 5].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					ws.Cells[line, 3, line, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

					ws.Cells[line, 1].Value = item.PathTfs;
					ws.Cells[line, 2].Value = item.ObjectName;
					ws.Cells[line, 3].Value = item.TargetChangeSet;
					ws.Row(line).Height = 35;
					line++;
				}


				formatCells(ws);
				

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
