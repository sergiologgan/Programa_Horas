namespace CalculadoraLancamento
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.arquivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salvarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verRecentesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sairToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.visualizaçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resumoLançamentosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.totalLançamentosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lançamentoDetalhepessoaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lançamentosPorPessoaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lançamentoResumopessoaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configuraçãoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.definirFeriadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ajudaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sobreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.abrirToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.abrirLocalDePastaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.visualizarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.criarPlanilhaGeralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.criarPlanilhaIndividualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // arquivoToolStripMenuItem
            // 
            this.arquivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem,
            this.salvarToolStripMenuItem,
            this.verRecentesToolStripMenuItem,
            this.sairToolStripMenuItem});
            this.arquivoToolStripMenuItem.Name = "arquivoToolStripMenuItem";
            this.arquivoToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.arquivoToolStripMenuItem.Text = "Arquivo";
            // 
            // abrirToolStripMenuItem
            // 
            this.abrirToolStripMenuItem.Name = "abrirToolStripMenuItem";
            this.abrirToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.abrirToolStripMenuItem.Text = "Abrir             Ctrl+O";
            this.abrirToolStripMenuItem.Click += new System.EventHandler(this.menuAbrirArquivo_click);
            // 
            // salvarToolStripMenuItem
            // 
            this.salvarToolStripMenuItem.Name = "salvarToolStripMenuItem";
            this.salvarToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.salvarToolStripMenuItem.Text = "Salvar ";
            this.salvarToolStripMenuItem.Click += new System.EventHandler(this.salvarToolStripMenuItem_Click);
            // 
            // verRecentesToolStripMenuItem
            // 
            this.verRecentesToolStripMenuItem.Name = "verRecentesToolStripMenuItem";
            this.verRecentesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.verRecentesToolStripMenuItem.Text = "Ver recentes";
            this.verRecentesToolStripMenuItem.Click += new System.EventHandler(this.menuVerRecentes_click);
            // 
            // sairToolStripMenuItem
            // 
            this.sairToolStripMenuItem.Name = "sairToolStripMenuItem";
            this.sairToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sairToolStripMenuItem.Text = "Sair";
            this.sairToolStripMenuItem.Click += new System.EventHandler(this.sairToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.arquivoToolStripMenuItem,
            this.visualizaçãoToolStripMenuItem,
            this.configuraçãoToolStripMenuItem,
            this.visualizarToolStripMenuItem,
            this.ajudaToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(926, 24);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // visualizaçãoToolStripMenuItem
            // 
            this.visualizaçãoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resumoLançamentosToolStripMenuItem,
            this.totalLançamentosToolStripMenuItem,
            this.lançamentoDetalhepessoaToolStripMenuItem,
            this.lançamentosPorPessoaToolStripMenuItem,
            this.lançamentoResumopessoaToolStripMenuItem});
            this.visualizaçãoToolStripMenuItem.Enabled = false;
            this.visualizaçãoToolStripMenuItem.Name = "visualizaçãoToolStripMenuItem";
            this.visualizaçãoToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.visualizaçãoToolStripMenuItem.Text = "Visualização";
            // 
            // resumoLançamentosToolStripMenuItem
            // 
            this.resumoLançamentosToolStripMenuItem.Name = "resumoLançamentosToolStripMenuItem";
            this.resumoLançamentosToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.resumoLançamentosToolStripMenuItem.Text = "Resumo horas | quantidade";
            this.resumoLançamentosToolStripMenuItem.Click += new System.EventHandler(this.menuResumoHorasRequisicao_click);
            // 
            // totalLançamentosToolStripMenuItem
            // 
            this.totalLançamentosToolStripMenuItem.Name = "totalLançamentosToolStripMenuItem";
            this.totalLançamentosToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.totalLançamentosToolStripMenuItem.Text = "Simular régua";
            this.totalLançamentosToolStripMenuItem.Click += new System.EventHandler(this.menuSimularRegua_click);
            // 
            // lançamentoDetalhepessoaToolStripMenuItem
            // 
            this.lançamentoDetalhepessoaToolStripMenuItem.Name = "lançamentoDetalhepessoaToolStripMenuItem";
            this.lançamentoDetalhepessoaToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.lançamentoDetalhepessoaToolStripMenuItem.Text = "Simular régua (pessoa)";
            this.lançamentoDetalhepessoaToolStripMenuItem.Click += new System.EventHandler(this.menuSimularReguaPessoa_click);
            // 
            // lançamentosPorPessoaToolStripMenuItem
            // 
            this.lançamentosPorPessoaToolStripMenuItem.Name = "lançamentosPorPessoaToolStripMenuItem";
            this.lançamentosPorPessoaToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.lançamentosPorPessoaToolStripMenuItem.Text = "Lançamento resumo";
            this.lançamentosPorPessoaToolStripMenuItem.Click += new System.EventHandler(this.menuLancamentoResumo_click);
            // 
            // lançamentoResumopessoaToolStripMenuItem
            // 
            this.lançamentoResumopessoaToolStripMenuItem.Name = "lançamentoResumopessoaToolStripMenuItem";
            this.lançamentoResumopessoaToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.lançamentoResumopessoaToolStripMenuItem.Text = "Lançamento resumo (pessoa)";
            this.lançamentoResumopessoaToolStripMenuItem.Click += new System.EventHandler(this.menuLancamentoResumoPessoa_click);
            // 
            // configuraçãoToolStripMenuItem
            // 
            this.configuraçãoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.definirFeriadoToolStripMenuItem});
            this.configuraçãoToolStripMenuItem.Name = "configuraçãoToolStripMenuItem";
            this.configuraçãoToolStripMenuItem.Size = new System.Drawing.Size(91, 20);
            this.configuraçãoToolStripMenuItem.Text = "Configuração";
            // 
            // definirFeriadoToolStripMenuItem
            // 
            this.definirFeriadoToolStripMenuItem.Name = "definirFeriadoToolStripMenuItem";
            this.definirFeriadoToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.definirFeriadoToolStripMenuItem.Text = "Definir feriado";
            this.definirFeriadoToolStripMenuItem.Click += new System.EventHandler(this.menuDefinirFeriado_click);
            // 
            // ajudaToolStripMenuItem
            // 
            this.ajudaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sobreToolStripMenuItem});
            this.ajudaToolStripMenuItem.Name = "ajudaToolStripMenuItem";
            this.ajudaToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.ajudaToolStripMenuItem.Text = "Ajuda";
            // 
            // sobreToolStripMenuItem
            // 
            this.sobreToolStripMenuItem.Name = "sobreToolStripMenuItem";
            this.sobreToolStripMenuItem.Size = new System.Drawing.Size(104, 22);
            this.sobreToolStripMenuItem.Text = "Sobre";
            this.sobreToolStripMenuItem.Click += new System.EventHandler(this.sobreToolStripMenuItem_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 24);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(926, 443);
            this.webBrowser1.TabIndex = 11;
            this.webBrowser1.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 158);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(583, 297);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.abrirToolStripMenuItem1,
            this.abrirLocalDePastaToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(176, 48);
            // 
            // abrirToolStripMenuItem1
            // 
            this.abrirToolStripMenuItem1.Name = "abrirToolStripMenuItem1";
            this.abrirToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            this.abrirToolStripMenuItem1.Text = "Abrir";
            // 
            // abrirLocalDePastaToolStripMenuItem
            // 
            this.abrirLocalDePastaToolStripMenuItem.Name = "abrirLocalDePastaToolStripMenuItem";
            this.abrirLocalDePastaToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.abrirLocalDePastaToolStripMenuItem.Text = "Abrir local de pasta";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 142);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Selecione um arquivo recente";
            // 
            // visualizarToolStripMenuItem
            // 
            this.visualizarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.criarPlanilhaGeralToolStripMenuItem,
            this.criarPlanilhaIndividualToolStripMenuItem});
            this.visualizarToolStripMenuItem.Name = "visualizarToolStripMenuItem";
            this.visualizarToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.visualizarToolStripMenuItem.Text = "Imprimir";
            // 
            // criarPlanilhaGeralToolStripMenuItem
            // 
            this.criarPlanilhaGeralToolStripMenuItem.Name = "criarPlanilhaGeralToolStripMenuItem";
            this.criarPlanilhaGeralToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.criarPlanilhaGeralToolStripMenuItem.Text = "Criar planilha geral";
            this.criarPlanilhaGeralToolStripMenuItem.Click += new System.EventHandler(this.criarPlanilhaGeralToolStripMenuItem_Click);
            // 
            // criarPlanilhaIndividualToolStripMenuItem
            // 
            this.criarPlanilhaIndividualToolStripMenuItem.Name = "criarPlanilhaIndividualToolStripMenuItem";
            this.criarPlanilhaIndividualToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.criarPlanilhaIndividualToolStripMenuItem.Text = "Criar planilha individual";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(926, 467);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TOTVS - Visualizador de Lançamentos de Horas (3.1.2.2)";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem arquivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem salvarToolStripMenuItem;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStripMenuItem sairToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualizaçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resumoLançamentosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem totalLançamentosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lançamentosPorPessoaToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem lançamentoDetalhepessoaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lançamentoResumopessoaToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem abrirToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem abrirLocalDePastaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verRecentesToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem configuraçãoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem definirFeriadoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ajudaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sobreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem visualizarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem criarPlanilhaGeralToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem criarPlanilhaIndividualToolStripMenuItem;
    }
}

