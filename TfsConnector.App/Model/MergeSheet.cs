using System.Collections.Generic;
using System.Linq;
using TfsConnector.App.Enum;

namespace TfsConnector.App.Model
{
    public class MergeBase
    {
        public MergeBase()
        {
            MergeSheetList = new List<MergeSheet>();
        }
        public List<MergeSheet> MergeSheetList { get; set; }
        public SourceControlEnvironment SourceControlEnvironment
        {
            get
            {                
                if (MergeSheetList.First().PathTfsFull.ToUpper().Contains("EsteiraAgil")) return SourceControlEnvironment.EsteiraAgil;
                if (MergeSheetList.First().PathTfsFull.ToUpper().Contains("HML")) return SourceControlEnvironment.Homolog;
                return SourceControlEnvironment.Accenture;
            }
        }
    }

    public class MergeSheet
    {
        public string PathTfs { get; set; }
        public string PathTfsFull { get; set; }
        public string ObjectName { get; set; }
        public string OutSourceBranchOldChangeSet { get; set; }
        public string OutSourceBranchOldHash { get; set; }
        public string DevBranchChangeSet { get; set; }
        public string DevBranchHash { get; set; }
        public string HMLBranchChangeSet { get; set; }
        public string HMLBranchHash { get; set; }
        public string PRDBranchChangeSet { get; set; }
        public string PRDBranchHash { get; set; }
        public string TargetChangeSet { get; set; }
        public string TargetHash { get; set; }

    }

}
