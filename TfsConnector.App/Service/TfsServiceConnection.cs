using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using TfsConnector.App.Enum;
using TfsConnector.App.Extensions;
using TfsConnector.App.Model;
using System.Configuration;

namespace TfsConnector.App.Service
{
    public class TfsServiceConnection
    {
        public static string Caminho;
        public VersionControlServer vcs;
        private TipoBranch _tipoBranch;
        private TipoPlanilha _tipoPlanilha;
        private readonly string[] _BdExtensions = new[] { ".sql" };
        private readonly string[] _ComponentsExtensions = new[] { ".dll" };
        private readonly string[] _PagesExtensions = new[] { ".asp", ".html", ".css", ".js", ".config" };
        private readonly string[] _IotaExtensions = new[] { ".exe" };
        private const int START_LINE = 8;
        private TfsUser _tfsUser = null;

        public RetornoVM Run(int tipoPlanilha, int tipoBranch, string changesets, TfsUser tfsUser = null)
        {
            try
            {
                if (tfsUser == null)
                {
                    GetTfsUserByConfiguration();
                }
                else
                {
                    _tfsUser = tfsUser;
                }

                _tipoBranch = (TipoBranch)tipoBranch;
                _tipoPlanilha = (TipoPlanilha)tipoPlanilha;

                if (_tipoPlanilha == TipoPlanilha.Rdm)
                {
                    FileInfo file1 = new FileInfo(@"model.xlsx");
                    if (!file1.Exists)
                        throw new Exception("Planilha modelo não encontrada!");
                }

                Console.WriteLine("Starting TFS Connection...");

                if (!StartTfsConnection())
                {
                    Console.WriteLine("Pressione qualquer tecla para sair...");
                    return new RetornoVM() { Retorno = false, CodErro = "TFS001" };
                    //Environment.Exit(0);
                }

                var list = changesets.Split(',').Select(_ => int.Parse(_.Trim())).OrderByDescending(_ => _).ToArray();
                var result = new MergeBase() { MergeSheetList = ListFilesFromChangeSets(list) };

                if (!result.MergeSheetList.Any())
                {
                    Console.WriteLine("Nenhum arquivo encontrado para os changeSets indicados");
                    return new RetornoVM() { Retorno = false, CodErro = "CD001 - Nenhum arquivo encontrado para os changeSets indicados" };
                }

                Console.WriteLine("Starting.");
                var folder = !string.IsNullOrEmpty(Caminho) ? Caminho : GetFolder();

                GetChangeSets(result);
                Console.WriteLine("Finishing.");

                if (_tipoPlanilha == TipoPlanilha.Merge)
                {
                    var file = CreateFile(result, folder, "MergeFolder");
                    Console.WriteLine($"Created File:  {file}");
                    // System.Diagnostics.Process.Start($@"{folder}\{file}");

                    return new RetornoVM() { Retorno = true, CodErro = "", CreatedFiles = new[] { file } };
                }
                else if (_tipoPlanilha == TipoPlanilha.Rdm)
                {
                    var arquivoImplantacao = CreateFileRdm(result, folder, GetFileName(ChangeSetType.Current), ChangeSetType.Current);
                    var arquivoRollback = CreateFileRdm(result, folder, GetFileName(ChangeSetType.Rollback), ChangeSetType.Rollback);
                    Console.WriteLine($"Created Files:  {arquivoImplantacao} { arquivoRollback}");

                    return new RetornoVM() { Retorno = true, CodErro = "", CreatedFiles = new[] { arquivoImplantacao, arquivoRollback } };
                }

                return new RetornoVM() { Retorno = false, CodErro = "CD002 - TIPO PLANILHA INVÁLIDO" };
            }
            catch (Exception ex)
            {
                return new RetornoVM() { Retorno = false, CodErro = ex.Message, Exception = ex };
                throw;
            }
        }



        private string GetFileName(ChangeSetType changeSetType)
        {
            var fileName = "";
            if (changeSetType == ChangeSetType.Current)
                fileName += "IMPLANTAÇÃO";
            if (changeSetType == ChangeSetType.Rollback)
                fileName += "ROLLBACK";

            fileName += "-" + _tipoBranch.ToString();

            //   fileName += "-" + changesets.Replace(",","");

            fileName += "-" + DateTime.Now.ToString("ddMMyy_HHmmss");

            return fileName;
        }

        private void GetTfsUserByConfiguration()
        {
            _tfsUser = new TfsUser()
            {
                UserName = ConfigurationManager.AppSettings["User"],
                UserPassword = ConfigurationManager.AppSettings["Password"],
                Domain = ConfigurationManager.AppSettings["Domain"],
                TfsUrl = ConfigurationManager.AppSettings["TfsUrl"],
            };
        }

        private void GetChangeSets(MergeBase result)
        {
            if (_tipoPlanilha == TipoPlanilha.Merge)
            {
                if (result.SourceControlEnvironment.Equals(SourceControlEnvironment.Homolog))
                {
                    GetChangeSetsByFile("HML", result, (int)ChangeSetType.Rollback);
                    GetChangeSetsByFile("PRD", result, (int)ChangeSetType.Current);
                }

                if (result.SourceControlEnvironment.Equals(SourceControlEnvironment.EsteiraAgil))
                {
                    GetChangeSetsByFile("EsteiraAgil", result, (int)ChangeSetType.Rollback);
                    GetChangeSetsByFile("HML", result, (int)ChangeSetType.Current);
                    GetChangeSetsByFile("DEV", result, (int)ChangeSetType.Current);
                }

                if (result.SourceControlEnvironment.Equals(SourceControlEnvironment.Accenture))
                {
                    GetChangeSetsByFile("OutSource/Accenture", result, (int)ChangeSetType.Rollback);
                    GetChangeSetsByFile("HML", result, (int)ChangeSetType.Current);
                    GetChangeSetsByFile("DEV", result, (int)ChangeSetType.Current);
                }
            }

            if (_tipoPlanilha == TipoPlanilha.Rdm)
            {
                GetChangeSetsByFile(TipoBranch.HML.ToString(), result, (int)ChangeSetType.Current);
                GetChangeSetsByFile(TipoBranch.HML.ToString(), result, (int)ChangeSetType.Rollback);
            }

            foreach (var item in result.MergeSheetList)
            {
                item.TargetHash = GetHashFromFile(item.PathTfsFull, item.TargetChangeSet);
            }
        }

        private bool StartTfsConnection()
        {
            Console.WriteLine("Connecting on TFS Server...");
            try
            {
                Uri serverUri = new Uri(_tfsUser.TfsUrl);
                NetworkCredential cred = new NetworkCredential(_tfsUser.UserName, _tfsUser.UserPassword, _tfsUser.Domain);
                TfsTeamProjectCollection tpc = new TfsTeamProjectCollection(serverUri, cred);
                tpc.EnsureAuthenticated();
                vcs = tpc.GetService<VersionControlServer>();
                Console.WriteLine("Connected! Hello: " + vcs.AuthenticatedUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Não foi possível conectar ao servidor TFS! Verifique:");
                Console.WriteLine("- Se esta conectado a VPN");
                Console.WriteLine("- Se suas credenciais estão corretas");
                Console.WriteLine("- Se o caminho do servidor esta correto!");
                Console.WriteLine("Tente novamente");
                Console.WriteLine("Erro: " + ex.Message);
                return false;
            }

        }

        private static string GetFolder()
        {
            var directory = Directory.GetCurrentDirectory();
            directory = Path.Combine(directory, "Out");
            if (Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return directory;
        }

        public List<MergeSheet> ListFilesFromChangeSets(int[] codes)
        {
            var result = new List<MergeSheet>();
            foreach (var code in codes)
            {
                var changeset = vcs.GetChangeset(code);

                foreach (var item in changeset.Changes)
                {
                    if (!result.Any(_ => _.PathTfs == item.Item.ServerItem))
                        result.Add(new MergeSheet() { PathTfs = item.Item.ServerItem, TargetChangeSet = code.ToString(), PathTfsFull = item.Item.ServerItem, ObjectExtension = Path.GetExtension(item.Item.ServerItem) });
                }
            }
            return result;
        }

        public void ManageDirectory(string path)
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

        public void GetChangeSetsByFile(string environmentDestiny, MergeBase mergeBase, int index)
        {
            Console.WriteLine($"Getting changesets from {environmentDestiny}, index: {index}");
            foreach (var item in mergeBase.MergeSheetList)
            {
                var changeSet = "N/A";
                var hash = "";
                try
                {
                    Console.WriteLine($"Processing files: {item.PathTfsFull} index: {index}");
                    var pathTfsFull = mergeBase.ToPathTfsFull(item.PathTfsFull, environmentDestiny);
                    var teste = "Outsource/Accenture";
                    var nada = teste.Replace("Outsource/Accenture","1");
                    var changes = vcs.QueryHistory(pathTfsFull, RecursionType.Full);
                    var c = changes.ToList();
                    changeSet = c[index].ChangesetId.ToString();
                    hash = GetHashFromFile(pathTfsFull, changeSet);
                }
                catch (Exception ex)
                {

                }

                switch (environmentDestiny)
                {
                    case "HML":
                        if (index == 0)
                        {
                            item.HMLBranchChangeSet = changeSet;
                            item.HMLBranchHash = hash;
                        }
                        else
                        {
                            item.OutSourceBranchOldChangeSet = changeSet;
                            item.OutSourceBranchOldHash = hash;
                        }
                        break;
                    case "DEV":
                        item.DevBranchChangeSet = changeSet;
                        item.DevBranchHash = hash;
                        break;
                    case "PRD":
                        item.PRDBranchChangeSet = changeSet;
                        item.PRDBranchHash = hash;
                        break;
                    default:
                        item.OutSourceBranchOldChangeSet = changeSet;
                        item.OutSourceBranchOldHash = hash;
                        break;
                }

                item.SplitObject();

            }

        }

        private string GetHashFromFile(string PathTfsFull, string changeSet)
        {
            var splittedPath = PathTfsFull.Split('/');
            var objectName = splittedPath[splittedPath.Length - 1];
            var path = Directory.GetCurrentDirectory();
            path = $@"{path}\DownloadFiles";
            var hash = "";

            if (!string.IsNullOrEmpty(objectName) && objectName.Contains("."))
            {
                try
                {
                    ManageDirectory(path);
                    vcs.DownloadFile(PathTfsFull, 0, VersionSpec.ParseSingleSpec($"C{changeSet}", null), $@"{path}\{objectName}");
                    hash = ($@"{path}\{objectName}").FileToHash();
                }
                catch (Exception)
                {
                }
            }

            return hash;
        }

        public void formatCells(ExcelWorksheet ws)
        {
            try
            {
                ws.Cells[3, 1, 4, 1].Merge = true;
                ws.Cells[3, 2, 4, 2].Merge = true;

                ws.Cells[3, 1, 4, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[3, 1, 4, 1].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[3, 2, 4, 2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells[3, 2, 4, 2].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                ws.Cells[3, 3, 4, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[3, 1, 4, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color: Color.White);
                ws.Cells[3, 1, 4, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[3, 1, 4, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[3, 1, 4, 6].Style.Border.Left.Color.SetColor(Color.White);
                ws.Cells[3, 1, 4, 6].Style.Border.Right.Color.SetColor(Color.White);

                ws.Cells[3, 3, 3, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[3, 3, 3, 6].Style.Border.Bottom.Color.SetColor(Color.White);
                ws.View.ShowGridLines = false;

            }
            catch
            {
            }

        }

        private string CreateFile(MergeBase mergeBase, string path, string fileName)
        {
            var filename = $"{fileName}_{DateTime.Now.ToString("yyyy_MM_dd_HH_mm")}.xlsx";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using (MemoryStream stream = new MemoryStream())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage p = new ExcelPackage();

                var ws = p.Workbook.Worksheets.Add("Merge");
                ws.Column(1).Width = 120;
                ws.Column(2).Width = 50;
                ws.Column(3).Width = 12;
                ws.Column(4).Width = 12;
                ws.Column(5).Width = 12;
                ws.Column(6).Width = 12;

                ws.Column(1).Style.WrapText = true;

                int line = 1;

                ws.Cells[1, 1].Value = "Rollback:";
                ws.Cells[3, 1].Value = "Caminho TFS – Branch";
                ws.Cells[3, 2].Value = "Objeto";
                ws.Cells[3, 3, 3, 6].Value = "ChangeSet";
                ws.Cells[4, 3].Value = mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Homolog) ? "HML" : "Esteira";
                ws.Cells[4, 4].Value = "DEV";
                ws.Cells[4, 5].Value = "HML";
                ws.Cells[4, 6].Value = "PRD";

                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells[1, 1].Style.Font.UnderLine = true;
                ws.Cells[3, 1, 3, 6].Style.Font.Bold = true;
                ws.Cells[4, 3, 4, 6].Style.Font.Bold = true;

                ws.Cells[1, 1].Style.Font.Color.SetColor(Color.Black);
                ws.Cells[3, 1, 3, 6].Style.Font.Color.SetColor(Color.White);
                ws.Cells[4, 3, 4, 6].Style.Font.Color.SetColor(Color.White);

                ws.Cells[3, 1, 3, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[4, 3, 4, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;

                ws.Cells[3, 1, 3, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 150, 70));
                ws.Cells[4, 3, 4, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 150, 70));

                line = 5;

                var list = mergeBase.MergeSheetsFilter();
                foreach (var item in list)
                {
                    ws.Cells[line, 1, line, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[line, 1, line, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 212, 180));
                    ws.Cells[line, 1, line, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    ws.Cells[line, 1, line, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[line, 1, line, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[line, 1, line, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    ws.Cells[line, 3, line, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[line, 1].Value = item.PathTfs;
                    ws.Cells[line, 2].Value = item.ObjectName;
                    ws.Cells[line, 3].Value = item.OutSourceBranchOldChangeSet;
                    ws.Cells[line, 4].Value = item.DevBranchChangeSet;
                    ws.Cells[line, 5].Value = item.HMLBranchChangeSet;
                    ws.Cells[line, 6].Value = item.PRDBranchChangeSet;
                    ws.Row(line).Height = 35;
                    line++;
                }

                line += 3;

                ws.Cells[line + 3, 1, line + 4, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                ws.Cells[line + 3, 1, line + 4, 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(247, 150, 70));
                ws.Cells[line, 1].Style.Font.Color.SetColor(Color.Black);
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
                ws.Cells[line + 4, 3].Value = mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Homolog) ? "HML" : "Esteira"; ;
                ws.Cells[line + 4, 4].Value = "DEV";
                ws.Cells[line + 4, 5].Value = "HML";
                ws.Cells[line + 4, 5].Value = "PRD";
                ws.Cells[line + 3, 3, line + 4, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                ws.Cells[line + 3, 1, line + 4, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin, Color: Color.White);
                ws.Cells[line + 3, 1, line + 4, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                ws.Cells[line + 3, 1, line + 4, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[line + 3, 1, line + 4, 6].Style.Border.Left.Color.SetColor(Color.White);
                ws.Cells[line + 3, 1, line + 4, 6].Style.Border.Right.Color.SetColor(Color.White);

                ws.Cells[line + 3, 1, line + 4, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[line + 3, 1, line + 4, 6].Style.Border.Bottom.Color.SetColor(Color.White);

                line += 5;

                foreach (var item in list)
                {
                    ws.Cells[line, 1, line, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    ws.Cells[line, 1, line, 6].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(251, 212, 180));

                    ws.Cells[line, 1, line, 6].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    ws.Cells[line, 1, line, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    ws.Cells[line, 1, line, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    ws.Cells[line, 1, line, 6].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    ws.Cells[line, 3, line, 6].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    ws.Cells[line, 1].Value = item.PathTfs;
                    ws.Cells[line, 2].Value = item.ObjectName;
                    ws.Cells[line, 3].Value = item.TargetChangeSet;
                    ws.Row(line).Height = 35;
                    line++;
                }

                formatCells(ws);

                if (mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Homolog))
                {
                    ws.Column(4).Hidden = true;
                    ws.Column(5).Hidden = true;
                }
                else
                {
                    ws.Column(6).Hidden = true;
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

        private string CreateFileRdm(MergeBase mergeBase, string path, string fileName, ChangeSetType changeSetType)
        {
            FileInfo file1 = new FileInfo(@"model.xlsx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage excelPackage = new ExcelPackage(file1))
            {
                ExcelWorkbook excelWorkBook = excelPackage.Workbook;

                IEnumerable<MergeSheet> mergeBases;
                ExcelWorksheet workSheet;

                #region BANCO DE DADOS
                var linha = START_LINE;
                mergeBases = mergeBase.MergeSheetList.Where(_ => _BdExtensions.Contains(_.ObjectExtension));
                workSheet = excelWorkBook.Worksheets[0];

                foreach (var item in mergeBases)
                {
                    workSheet.Cells[linha, 1].Value = item.PathTfsFull;
                    workSheet.Cells[linha, 2].Value = item.ObjectName;
                    workSheet.Cells[linha, 3].Value = item.ObjectExtension;
                    if (changeSetType == ChangeSetType.Rollback)
                        workSheet.Cells[linha, 4].Style.Font.Color.SetColor(Color.OrangeRed);
                    workSheet.Cells[linha, 4].Value = changeSetType == ChangeSetType.Current ? item.HMLBranchChangeSet : item.OutSourceBranchOldChangeSet;
                    workSheet.Cells[linha, 5].Value = "";
                    linha++;
                }
                #endregion

                #region COMPONENTES
                linha = START_LINE;
                mergeBases = mergeBase.MergeSheetList.Where(_ => _ComponentsExtensions.Contains(_.ObjectExtension));
                workSheet = excelWorkBook.Worksheets[1];
                foreach (var item in mergeBases)
                {
                    workSheet.Cells[linha, 1].Value = item.PathTfsFull;
                    workSheet.Cells[linha, 2].Value = item.ObjectName;
                    workSheet.Cells[linha, 3].Value = item.ObjectExtension;
                    if (changeSetType == ChangeSetType.Rollback)
                        workSheet.Cells[linha, 4].Style.Font.Color.SetColor(Color.OrangeRed);
                    workSheet.Cells[linha, 4].Value = changeSetType == ChangeSetType.Current ? item.HMLBranchChangeSet : item.OutSourceBranchOldChangeSet;
                    workSheet.Cells[linha, 5].Value = "";
                    workSheet.Cells[linha, 6].Value = "";
                    linha++;
                }
                #endregion

                #region PAGINAS
                linha = START_LINE;
                mergeBases = mergeBase.MergeSheetList.Where(_ => _PagesExtensions.Contains(_.ObjectExtension));
                workSheet = excelWorkBook.Worksheets[2];
                foreach (var item in mergeBases)
                {
                    workSheet.Cells[linha, 1].Value = item.PathTfsFull;
                    workSheet.Cells[linha, 2].Value = item.ObjectName;
                    workSheet.Cells[linha, 3].Value = item.ObjectExtension;
                    if (changeSetType == ChangeSetType.Rollback)
                        workSheet.Cells[linha, 4].Style.Font.Color.SetColor(Color.OrangeRed);
                    workSheet.Cells[linha, 4].Value = changeSetType == ChangeSetType.Current ? item.HMLBranchChangeSet : item.OutSourceBranchOldChangeSet;
                    workSheet.Cells[linha, 5].Value = "";
                    workSheet.Cells[linha, 6].Value = "";
                    linha++;
                }
                #endregion

                #region IOTA
                linha = START_LINE;
                mergeBases = mergeBase.MergeSheetList.Where(_ => _IotaExtensions.Contains(_.ObjectExtension));
                workSheet = excelWorkBook.Worksheets[3];
                foreach (var item in mergeBases)
                {
                    workSheet.Cells[linha, 1].Value = item.PathTfsFull;
                    workSheet.Cells[linha, 2].Value = item.ObjectName;
                    workSheet.Cells[linha, 3].Value = item.ObjectExtension;

                    if (changeSetType == ChangeSetType.Rollback)
                        workSheet.Cells[linha, 4].Style.Font.Color.SetColor(Color.OrangeRed);

                    workSheet.Cells[linha, 4].Value = changeSetType == ChangeSetType.Current ? item.HMLBranchChangeSet : item.OutSourceBranchOldChangeSet;
                    workSheet.Cells[linha, 5].Value = @"\\iota\programas \\iota\programas64";
                    workSheet.Cells[linha, 6].Value = "";
                    linha++;
                }
                #endregion

                #region SAVE FILE
                var filename = $"{fileName}.xlsx";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                using (MemoryStream stream = new MemoryStream())
                {
                    excelPackage.SaveAs(stream);

                    using (FileStream file = new FileStream(path + "/" + filename, FileMode.OpenOrCreate))
                    {
                        stream.Position = 0;
                        stream.Read(stream.ToArray(), 0, (int)stream.Length);
                        file.Write(stream.ToArray(), 0, stream.ToArray().Length);
                        stream.Close();
                        file.Flush();
                        file.Close();
                    }
                }
                #endregion

                return filename;
            }
        }
    }
}
