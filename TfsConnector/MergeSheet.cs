using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsConnector
{
    public class MergeSheet
    {
        public string PathTfs { get; set; }
        public string PathTfsFull { get; set; }
        public string ObjectName { get; set; }
        public string OutSourceBranchChangeSet { get; set; }
        public string DevBranchChangeSet { get; set; }
        public string HMLBranchChangeSet { get; set; }
        public string TargetChangeSet { get; set; }

    }

    public static class MergeSheetExtensions
    {
        public static void SplitObject(this MergeSheet self)
        {
            var s = self.PathTfsFull.Split('/');
            self.ObjectName = s[s.Length - 1];
            if (!self.ObjectName.Contains(".")) self.ObjectName = "";
            if (self.ObjectName != "")
                self.PathTfs = self.PathTfs.Replace(self.ObjectName, "");
        }
    }
}
