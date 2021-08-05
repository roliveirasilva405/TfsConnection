
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TfsConnector.ui.Helper
{
    public static class Helpers
    {
        public static void MsgError(string msg)
        {
            MessageBox.Show(msg, "Erro do sistema", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void MsgInfo(string msg)
        {
            MessageBox.Show(msg, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static void MsgWarning(string msg)
        {
            MessageBox.Show(msg, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
