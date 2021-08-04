using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TfsConnector.App.Model
{
    public class RetornoVM
    {
        public bool Retorno { get; set; }
        public string CodErro { get; set; }
        public string[] CreatedFiles { get; set; }
        public Exception Exception { get; set; }
    }
}
