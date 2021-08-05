using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TfsConnector.ui.Helpers.Helpers
{
    public partial class FrmQuestion : Form
    {
        private string _caminhoDestino;
        private string[] _arquivos;
        public FrmQuestion(string caminhoDestino, string[] arquivos)
        {
            InitializeComponent();

            _caminhoDestino = caminhoDestino;
            _arquivos = arquivos;
        }

        private void FrmQuestion_Load(object sender, EventArgs e)
        {

        }

        private void btn1_Click(object sender, EventArgs e)
        {
            Process.Start(_caminhoDestino);
            this.Close();
        }

        private void btnAbrirArquivo_Click(object sender, EventArgs e)
        {
            foreach (var item in _arquivos)
            {
               Process.Start(_caminhoDestino + @"\" + item);
            }
           
            this.Close();
        }
    }
}
