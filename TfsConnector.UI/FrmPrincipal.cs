using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleRedirection;
using SharpConfig;
using TfsConnector.App.Enum;
using TfsConnector.App.Model;
using TfsConnector.App.Service;
using TfsConnector.ui;
using TfsConnector.ui.Helper;
using TfsConnector.ui.Helpers.Helpers;

namespace TfsConnector
{


    public partial class Form1 : Form
    {
        TextWriter _writer = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(btnAbrirPasta, "Abrir pasta de destino");

            cbHttp.SelectedIndex = 0;

            txtChangesets.Focus();

            try
            {
                FillConfig();
            }
            catch (Exception ex)
            {
                Helpers.MsgError(ex.Message);
            }

        }

        public void FillConfig()
        {

            var retorno = FileConfig.LoadData();
            if (string.IsNullOrEmpty(retorno.Item1.UserName.ToString()))
            {
                txtUser.Text = "";
                txtPassword.Text = "";
            }
            else
            {
                txtUser.Text = retorno.Item1.UserName;
                txtPassword.Text = retorno.Item1.UserPassword;
                txtDomain.Text = retorno.Item1.Domain;
                cbHttp.SelectedItem = retorno.Item1.TfsUrl.Contains("https") ? "https://" : "http://";
                txtTfsUrl.Text = retorno.Item1.TfsUrl.Replace((retorno.Item1.TfsUrl.Contains("https") ? "https://" : "http://"), "");
                txtCaminhoArquivos.Text = retorno.Item2;
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            bool validation = GrpConnectionValidate();

            if (validation)
            {
                var tfsUser = new TfsUser();

                tfsUser.UserName = txtUser.Text;
                tfsUser.UserPassword = txtPassword.Text;
                tfsUser.Domain = txtDomain.Text;
                tfsUser.TfsUrl = cbHttp.SelectedItem.ToString() + txtTfsUrl.Text;



                FileConfig.SaveData(tfsUser, txtCaminhoArquivos.Text);
                Helpers.MsgInfo("Salvo com sucesso!");
                groupTabs.SelectedTab = tabPrincipal;
            }
        }

        private void btnProcessar_Click(object sender, EventArgs e)
        {
            if (GrpConnectionValidate() && tabPrincipalValidate())
                startProcess();
        }

        private bool tabPrincipalValidate()
        {
            var changeSets = txtChangesets.Text.Split(',');

            var changes = new List<int>();

            foreach (var item in changeSets)
            {
                if (!int.TryParse(item, out int changeSetInt))
                {
                    Helpers.MsgWarning("Changeset:" + item + " inválido, favor corrigir!");
                    return false;
                }
                else
                {
                    if (!changes.Contains(changeSetInt))
                        changes.Add(changeSetInt);
                    else
                    {
                        Helpers.MsgWarning("Changeset: " + changeSetInt + " duplicado! favor corrigir!");
                        return false;
                    }
                }
            }
            return true;
        }

        private void startProcess()
        {
            _writer = new TextBoxStreamWriter(txtog);
            Console.SetOut(_writer);

            var tipoBranch = GetTipoBranch();
            var tipoPlanilha = GetTipoPlanilha();
            var tfsUser = GetTfsUser();

            if (!string.IsNullOrEmpty(txtCaminhoArquivos.Text))
                TfsServiceConnection.Caminho = txtCaminhoArquivos.Text;

            var retorno = new TfsServiceConnection().Run(tipoPlanilha, tipoBranch, txtChangesets.Text, tfsUser);

            if (retorno.Retorno)
            {
                FrmQuestion teste = new FrmQuestion(txtCaminhoArquivos.Text, retorno.CreatedFiles);
                teste.ShowDialog();
            }
            else
            {
                if (retorno.CodErro == "TFS001")
                {
                    Helpers.MsgError("Erro ao conectar ao TFS!");
                }
                else
                    if (retorno.Exception != null)
                    Helpers.MsgError("Erro ao processar arquivo: " + retorno.Exception.Message);
                else
                    Helpers.MsgError("Erro ao processar arquivo: " + retorno.CodErro);
            }

        }

        private TfsUser GetTfsUser()
        {
            return new TfsUser { Domain = txtDomain.Text, TfsUrl = cbHttp.SelectedItem.ToString() + txtTfsUrl.Text, UserName = txtUser.Text, UserPassword = txtPassword.Text };
        }

        private bool GrpConnectionValidate()
        {
            if (string.IsNullOrEmpty(txtUser.Text))
            {
                Helpers.MsgWarning("User deve ser informado.");
                groupTabs.SelectedTab = tabConfig;
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                Helpers.MsgWarning("A senha deve ser informada.");
                groupTabs.SelectedTab = tabConfig;
                return false;
            }

            if (string.IsNullOrEmpty(txtTfsUrl.Text))
            {
                Helpers.MsgWarning("Informe o caminho do servidor TFS!");
                groupTabs.SelectedTab = tabConfig;
                return false;
            }
            if (!string.IsNullOrEmpty(txtCaminhoArquivos.Text))
            {
                if (!Directory.Exists(txtCaminhoArquivos.Text))
                {
                    Helpers.MsgWarning("Caminho informado para o arquivo não existe!");
                    groupTabs.SelectedTab = tabConfig;
                    return false;
                }
            }

            return true;
        }

        private int GetTipoBranch()
        {
            if (rbHml.Checked)
                return (int)TipoBranch.HML;

            if (rbPrd.Checked)
                return (int)TipoBranch.PRD;

            return (int)TipoBranch.HML;

        }

        private int GetTipoPlanilha()
        {
            if (rbMerge.Checked)
                return (int)TipoPlanilha.Merge;

            if (rbRdm.Checked)
                return (int)TipoPlanilha.Rdm;

            return (int)TipoPlanilha.Merge;
        }

        private void rbPrd_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbMerge_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbRdm_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txtChangesets.Focus();
        }

        private void btnSelecionaPasta_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCaminhoArquivos.Text))
                fbDialog.SelectedPath = txtCaminhoArquivos.Text;
            fbDialog.ShowDialog();

            var caminho = fbDialog.SelectedPath;
            txtCaminhoArquivos.Text = caminho;
        }

        private void txtTfsUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTfsUrl_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtTfsUrl_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void txtTfsUrl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                e.SuppressKeyPress = true;
        }

        private void btnAbrirPasta_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCaminhoArquivos.Text))
                if (Directory.Exists(txtCaminhoArquivos.Text))
                    Process.Start(txtCaminhoArquivos.Text);
                else
                    Process.Start(@"out");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //startProcess();
        }
    }

}
