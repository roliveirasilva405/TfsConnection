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
            if (mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Homolog))
                return currentFile.Replace("HML", environmentDestiny);

            if (mergeBase.SourceControlEnvironment.Equals(SourceControlEnvironment.Accenture))
                return currentFile.Replace("OutSource/Accenture", environmentDestiny);

            return currentFile.Replace("EsteiraAgil/", "").Replace("/Fontes/", "/" + environmentDestiny + "/Fontes/");
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
