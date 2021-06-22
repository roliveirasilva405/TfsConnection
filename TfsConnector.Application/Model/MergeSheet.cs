using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;

namespace TfsConnector.Application.Model
{
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
        public string TargetChangeSet { get; set; }
        public string TargetHash { get; set; }

    }

}
