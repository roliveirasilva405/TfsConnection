using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsConnector.App.Enum
{
    public enum SourceControlEnvironment
    {
        Accenture,
        EsteiraAgil,
        Homolog,
        Dev,
        Prd
    }

    public enum TipoPlanilha
    {
        Merge,
        Rdm
    }

    public enum TipoBranch
    {
        HML,
        PRD
    }

    public enum ChangeSetType
    {
        Current,
        Rollback
    }
}
