
namespace TfsConnector
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labChangeset = new System.Windows.Forms.Label();
            this.txtChangesets = new System.Windows.Forms.TextBox();
            this.grpBranch = new System.Windows.Forms.GroupBox();
            this.rbPrd = new System.Windows.Forms.RadioButton();
            this.rbHml = new System.Windows.Forms.RadioButton();
            this.groupTabs = new System.Windows.Forms.TabControl();
            this.tabPrincipal = new System.Windows.Forms.TabPage();
            this.btnAbrirPasta = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grpTipoPlanilha = new System.Windows.Forms.GroupBox();
            this.rbRdm = new System.Windows.Forms.RadioButton();
            this.rbMerge = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtog = new System.Windows.Forms.TextBox();
            this.btnProcessar = new System.Windows.Forms.Button();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.grpConection = new System.Windows.Forms.GroupBox();
            this.cbHttp = new System.Windows.Forms.ComboBox();
            this.btnSelecionaPasta = new System.Windows.Forms.Button();
            this.txtCaminhoArquivos = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.txtTfsUrl = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.labTfsUrl = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.labUser = new System.Windows.Forms.Label();
            this.lasPassword = new System.Windows.Forms.Label();
            this.labDomain = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.fbDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.grpBranch.SuspendLayout();
            this.groupTabs.SuspendLayout();
            this.tabPrincipal.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.grpTipoPlanilha.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.grpConection.SuspendLayout();
            this.SuspendLayout();
            // 
            // labChangeset
            // 
            this.labChangeset.AutoSize = true;
            this.labChangeset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labChangeset.Location = new System.Drawing.Point(4, 10);
            this.labChangeset.Name = "labChangeset";
            this.labChangeset.Size = new System.Drawing.Size(82, 16);
            this.labChangeset.TabIndex = 0;
            this.labChangeset.Text = "ChangeSets";
            // 
            // txtChangesets
            // 
            this.txtChangesets.AccessibleDescription = "changeset";
            this.txtChangesets.Location = new System.Drawing.Point(7, 29);
            this.txtChangesets.Name = "txtChangesets";
            this.txtChangesets.Size = new System.Drawing.Size(375, 20);
            this.txtChangesets.TabIndex = 1;
            // 
            // grpBranch
            // 
            this.grpBranch.Controls.Add(this.rbPrd);
            this.grpBranch.Controls.Add(this.rbHml);
            this.grpBranch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpBranch.Location = new System.Drawing.Point(6, 19);
            this.grpBranch.Name = "grpBranch";
            this.grpBranch.Size = new System.Drawing.Size(127, 50);
            this.grpBranch.TabIndex = 2;
            this.grpBranch.TabStop = false;
            this.grpBranch.Text = "Branch";
            // 
            // rbPrd
            // 
            this.rbPrd.AllowDrop = true;
            this.rbPrd.AutoSize = true;
            this.rbPrd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbPrd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbPrd.Location = new System.Drawing.Point(73, 21);
            this.rbPrd.Name = "rbPrd";
            this.rbPrd.Size = new System.Drawing.Size(48, 17);
            this.rbPrd.TabIndex = 5;
            this.rbPrd.Text = "PRD";
            this.rbPrd.UseVisualStyleBackColor = true;
            this.rbPrd.CheckedChanged += new System.EventHandler(this.rbPrd_CheckedChanged);
            // 
            // rbHml
            // 
            this.rbHml.AllowDrop = true;
            this.rbHml.AutoSize = true;
            this.rbHml.Checked = true;
            this.rbHml.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbHml.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbHml.Location = new System.Drawing.Point(6, 21);
            this.rbHml.Name = "rbHml";
            this.rbHml.Size = new System.Drawing.Size(48, 17);
            this.rbHml.TabIndex = 5;
            this.rbHml.TabStop = true;
            this.rbHml.Text = "HML";
            this.rbHml.UseVisualStyleBackColor = true;
            // 
            // groupTabs
            // 
            this.groupTabs.Controls.Add(this.tabPrincipal);
            this.groupTabs.Controls.Add(this.tabConfig);
            this.groupTabs.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.groupTabs.Location = new System.Drawing.Point(2, 0);
            this.groupTabs.Name = "groupTabs";
            this.groupTabs.SelectedIndex = 0;
            this.groupTabs.Size = new System.Drawing.Size(396, 399);
            this.groupTabs.TabIndex = 4;
            // 
            // tabPrincipal
            // 
            this.tabPrincipal.Controls.Add(this.btnAbrirPasta);
            this.tabPrincipal.Controls.Add(this.label1);
            this.tabPrincipal.Controls.Add(this.groupBox2);
            this.tabPrincipal.Controls.Add(this.groupBox1);
            this.tabPrincipal.Controls.Add(this.btnProcessar);
            this.tabPrincipal.Controls.Add(this.txtChangesets);
            this.tabPrincipal.Controls.Add(this.labChangeset);
            this.tabPrincipal.Location = new System.Drawing.Point(4, 22);
            this.tabPrincipal.Name = "tabPrincipal";
            this.tabPrincipal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPrincipal.Size = new System.Drawing.Size(388, 373);
            this.tabPrincipal.TabIndex = 0;
            this.tabPrincipal.Text = "Principal";
            this.tabPrincipal.UseVisualStyleBackColor = true;
            // 
            // btnAbrirPasta
            // 
            this.btnAbrirPasta.BackgroundImage = global::TfsConnector.ui.Properties.Resources.open_file_folder_emoji;
            this.btnAbrirPasta.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAbrirPasta.Location = new System.Drawing.Point(269, 145);
            this.btnAbrirPasta.Name = "btnAbrirPasta";
            this.btnAbrirPasta.Size = new System.Drawing.Size(32, 23);
            this.btnAbrirPasta.TabIndex = 8;
            this.btnAbrirPasta.UseVisualStyleBackColor = true;
            this.btnAbrirPasta.Click += new System.EventHandler(this.btnAbrirPasta_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(80, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "*separados por virgula";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grpBranch);
            this.groupBox2.Controls.Add(this.grpTipoPlanilha);
            this.groupBox2.Location = new System.Drawing.Point(4, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(378, 84);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Configurações";
            // 
            // grpTipoPlanilha
            // 
            this.grpTipoPlanilha.Controls.Add(this.rbRdm);
            this.grpTipoPlanilha.Controls.Add(this.rbMerge);
            this.grpTipoPlanilha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpTipoPlanilha.Location = new System.Drawing.Point(139, 19);
            this.grpTipoPlanilha.Name = "grpTipoPlanilha";
            this.grpTipoPlanilha.Size = new System.Drawing.Size(131, 50);
            this.grpTipoPlanilha.TabIndex = 3;
            this.grpTipoPlanilha.TabStop = false;
            this.grpTipoPlanilha.Text = "Tipo de planilha";
            // 
            // rbRdm
            // 
            this.rbRdm.AllowDrop = true;
            this.rbRdm.AutoSize = true;
            this.rbRdm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbRdm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbRdm.Location = new System.Drawing.Point(75, 21);
            this.rbRdm.Name = "rbRdm";
            this.rbRdm.Size = new System.Drawing.Size(50, 17);
            this.rbRdm.TabIndex = 5;
            this.rbRdm.Text = "RDM";
            this.rbRdm.UseVisualStyleBackColor = true;
            this.rbRdm.CheckedChanged += new System.EventHandler(this.rbRdm_CheckedChanged);
            // 
            // rbMerge
            // 
            this.rbMerge.AllowDrop = true;
            this.rbMerge.AutoSize = true;
            this.rbMerge.Checked = true;
            this.rbMerge.Cursor = System.Windows.Forms.Cursors.Hand;
            this.rbMerge.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.rbMerge.Location = new System.Drawing.Point(8, 21);
            this.rbMerge.Name = "rbMerge";
            this.rbMerge.Size = new System.Drawing.Size(64, 17);
            this.rbMerge.TabIndex = 5;
            this.rbMerge.TabStop = true;
            this.rbMerge.Text = "MERGE";
            this.rbMerge.UseVisualStyleBackColor = true;
            this.rbMerge.CheckedChanged += new System.EventHandler(this.rbMerge_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtog);
            this.groupBox1.Location = new System.Drawing.Point(7, 174);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 193);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Informações";
            // 
            // txtog
            // 
            this.txtog.BackColor = System.Drawing.Color.White;
            this.txtog.Location = new System.Drawing.Point(7, 19);
            this.txtog.Multiline = true;
            this.txtog.Name = "txtog";
            this.txtog.Size = new System.Drawing.Size(362, 157);
            this.txtog.TabIndex = 0;
            // 
            // btnProcessar
            // 
            this.btnProcessar.BackColor = System.Drawing.Color.Transparent;
            this.btnProcessar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnProcessar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnProcessar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcessar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnProcessar.Location = new System.Drawing.Point(307, 145);
            this.btnProcessar.Name = "btnProcessar";
            this.btnProcessar.Size = new System.Drawing.Size(75, 23);
            this.btnProcessar.TabIndex = 4;
            this.btnProcessar.Text = "Processar";
            this.btnProcessar.UseVisualStyleBackColor = false;
            this.btnProcessar.Click += new System.EventHandler(this.btnProcessar_Click);
            // 
            // tabConfig
            // 
            this.tabConfig.Controls.Add(this.grpConection);
            this.tabConfig.Location = new System.Drawing.Point(4, 22);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(388, 373);
            this.tabConfig.TabIndex = 1;
            this.tabConfig.Text = "Configuração";
            this.tabConfig.UseVisualStyleBackColor = true;
            // 
            // grpConection
            // 
            this.grpConection.Controls.Add(this.cbHttp);
            this.grpConection.Controls.Add(this.btnSelecionaPasta);
            this.grpConection.Controls.Add(this.txtCaminhoArquivos);
            this.grpConection.Controls.Add(this.label2);
            this.grpConection.Controls.Add(this.btnSaveConfig);
            this.grpConection.Controls.Add(this.txtTfsUrl);
            this.grpConection.Controls.Add(this.txtPassword);
            this.grpConection.Controls.Add(this.txtDomain);
            this.grpConection.Controls.Add(this.labTfsUrl);
            this.grpConection.Controls.Add(this.txtUser);
            this.grpConection.Controls.Add(this.labUser);
            this.grpConection.Controls.Add(this.lasPassword);
            this.grpConection.Controls.Add(this.labDomain);
            this.grpConection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpConection.Location = new System.Drawing.Point(3, 6);
            this.grpConection.Name = "grpConection";
            this.grpConection.Size = new System.Drawing.Size(348, 340);
            this.grpConection.TabIndex = 20;
            this.grpConection.TabStop = false;
            this.grpConection.Text = "Conexão";
            // 
            // cbHttp
            // 
            this.cbHttp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbHttp.FormattingEnabled = true;
            this.cbHttp.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.cbHttp.Items.AddRange(new object[] {
            "https://",
            "http://"});
            this.cbHttp.Location = new System.Drawing.Point(7, 217);
            this.cbHttp.MinimumSize = new System.Drawing.Size(64, 0);
            this.cbHttp.Name = "cbHttp";
            this.cbHttp.Size = new System.Drawing.Size(64, 24);
            this.cbHttp.TabIndex = 10;
            // 
            // btnSelecionaPasta
            // 
            this.btnSelecionaPasta.Location = new System.Drawing.Point(312, 270);
            this.btnSelecionaPasta.Name = "btnSelecionaPasta";
            this.btnSelecionaPasta.Size = new System.Drawing.Size(30, 23);
            this.btnSelecionaPasta.TabIndex = 9;
            this.btnSelecionaPasta.Text = "...";
            this.btnSelecionaPasta.UseVisualStyleBackColor = true;
            this.btnSelecionaPasta.Click += new System.EventHandler(this.btnSelecionaPasta_Click);
            // 
            // txtCaminhoArquivos
            // 
            this.txtCaminhoArquivos.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtCaminhoArquivos.Location = new System.Drawing.Point(6, 271);
            this.txtCaminhoArquivos.Name = "txtCaminhoArquivos";
            this.txtCaminhoArquivos.Size = new System.Drawing.Size(300, 22);
            this.txtCaminhoArquivos.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 255);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Caminho Geração Arquivos";
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveConfig.Location = new System.Drawing.Point(130, 311);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfig.TabIndex = 3;
            this.btnSaveConfig.Text = "Salvar";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // txtTfsUrl
            // 
            this.txtTfsUrl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtTfsUrl.Location = new System.Drawing.Point(70, 217);
            this.txtTfsUrl.Multiline = true;
            this.txtTfsUrl.Name = "txtTfsUrl";
            this.txtTfsUrl.Size = new System.Drawing.Size(272, 24);
            this.txtTfsUrl.TabIndex = 6;
            this.txtTfsUrl.Text = "http://tfs.fleury.com.br:8080/tfs/GrupoFleury";
            this.txtTfsUrl.TextChanged += new System.EventHandler(this.txtTfsUrl_TextChanged);
            this.txtTfsUrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTfsUrl_KeyDown);
            this.txtTfsUrl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTfsUrl_KeyPress);
            this.txtTfsUrl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtTfsUrl_KeyUp);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(6, 102);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(336, 22);
            this.txtPassword.TabIndex = 2;
            // 
            // txtDomain
            // 
            this.txtDomain.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtDomain.Location = new System.Drawing.Point(6, 160);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(336, 22);
            this.txtDomain.TabIndex = 5;
            this.txtDomain.Text = "fleurynt";
            // 
            // labTfsUrl
            // 
            this.labTfsUrl.AutoSize = true;
            this.labTfsUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labTfsUrl.Location = new System.Drawing.Point(3, 202);
            this.labTfsUrl.Name = "labTfsUrl";
            this.labTfsUrl.Size = new System.Drawing.Size(44, 16);
            this.labTfsUrl.TabIndex = 3;
            this.labTfsUrl.Text = "TfsUrl";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(6, 44);
            this.txtUser.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(336, 22);
            this.txtUser.TabIndex = 1;
            this.txtUser.Text = "avanade.";
            // 
            // labUser
            // 
            this.labUser.AutoSize = true;
            this.labUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labUser.Location = new System.Drawing.Point(3, 25);
            this.labUser.Name = "labUser";
            this.labUser.Size = new System.Drawing.Size(37, 16);
            this.labUser.TabIndex = 0;
            this.labUser.Text = "User";
            // 
            // lasPassword
            // 
            this.lasPassword.AutoSize = true;
            this.lasPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lasPassword.Location = new System.Drawing.Point(3, 83);
            this.lasPassword.Name = "lasPassword";
            this.lasPassword.Size = new System.Drawing.Size(68, 16);
            this.lasPassword.TabIndex = 2;
            this.lasPassword.Text = "Password";
            // 
            // labDomain
            // 
            this.labDomain.AutoSize = true;
            this.labDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDomain.Location = new System.Drawing.Point(3, 144);
            this.labDomain.Name = "labDomain";
            this.labDomain.Size = new System.Drawing.Size(55, 16);
            this.labDomain.TabIndex = 2;
            this.labDomain.Text = "Domain";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(398, 402);
            this.Controls.Add(this.groupTabs);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximumSize = new System.Drawing.Size(414, 441);
            this.MinimumSize = new System.Drawing.Size(414, 441);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TfsConnector";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.grpBranch.ResumeLayout(false);
            this.grpBranch.PerformLayout();
            this.groupTabs.ResumeLayout(false);
            this.tabPrincipal.ResumeLayout(false);
            this.tabPrincipal.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.grpTipoPlanilha.ResumeLayout(false);
            this.grpTipoPlanilha.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabConfig.ResumeLayout(false);
            this.grpConection.ResumeLayout(false);
            this.grpConection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labChangeset;
        private System.Windows.Forms.TextBox txtChangesets;
        private System.Windows.Forms.GroupBox grpBranch;
        private System.Windows.Forms.RadioButton rbPrd;
        private System.Windows.Forms.RadioButton rbHml;
        private System.Windows.Forms.TabControl groupTabs;
        private System.Windows.Forms.TabPage tabPrincipal;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.GroupBox grpConection;
        private System.Windows.Forms.Label labUser;
        private System.Windows.Forms.Label labTfsUrl;
        private System.Windows.Forms.Label lasPassword;
        private System.Windows.Forms.Label labDomain;
        private System.Windows.Forms.TextBox txtTfsUrl;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Button btnProcessar;
        private System.Windows.Forms.GroupBox grpTipoPlanilha;
        private System.Windows.Forms.RadioButton rbRdm;
        private System.Windows.Forms.RadioButton rbMerge;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCaminhoArquivos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelecionaPasta;
        private System.Windows.Forms.FolderBrowserDialog fbDialog;
        private System.Windows.Forms.Button btnAbrirPasta;
        private System.Windows.Forms.ComboBox cbHttp;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

