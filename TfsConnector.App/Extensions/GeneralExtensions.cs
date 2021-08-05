using System.Collections.Generic;
using System.Linq;
using TfsConnector.App.Enum;
using TfsConnector.App.Model;

namespace TfsConnector.App.Extensions
{
    public static class GeneralExtensions
    {
        public static void SplitObject(this MergeSheet self)
        {
            var s = self.PathTfsFull.Split('/');
            self.ObjectName = s[s.Length - 1];
            if (!self.ObjectName.Contains(".")) self.ObjectName = "";
            if (self.ObjectName != "")
                self.PathTfs = self.PathTfs.Replace(self.ObjectName, "");
        }

        public static string ToPathTfsFull(this MergeBase mergeBase, string currentFile, string environmentDestiny)
        {
            var pathReturn = "";
            if (mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Homolog))
            {
                pathReturn = currentFile.Replace("HML", environmentDestiny);
                return pathReturn;
            }

            if (mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Accenture))
            {
                pathReturn = currentFile.Replace("Outsource/Accenture", environmentDestiny);
                return pathReturn;
            }

            pathReturn = currentFile.Replace("EsteiraAgil/", "").Replace("/Fontes/", "/" + environmentDestiny + "/Fontes/");
            return pathReturn;
        }

        public static List<MergeSheet> MergeSheetsFilter(this MergeBase mergeBase)
        {
            var list = mergeBase.MergeSheetList.Where(_ => _.ObjectName != "");
            if (mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Homolog))
                return list.Where(_ => _.PRDBranchHash != _.TargetHash).ToList();

            return list.Where(_ => _.DevBranchHash != _.TargetHash || _.HMLBranchHash != _.TargetHash).ToList();

        }
    }
}
