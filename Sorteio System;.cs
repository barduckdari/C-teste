using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SorteioApp
{
    public class SorteioApp : Form
    {
        private Panel configPanel;
        private Panel sorteioPanel;
        private Label intervaloLabel;
        private Label inicioLabel;
        private TextBox inicioTextBox;
        private Label fimLabel;
        private TextBox fimTextBox;
        private CheckBox repetirCheckBox;
        private Button iniciarButton;
        private Label intervaloSorteioLabel;
        private Label resultLabel;
        private Button sortearButton;
        private Button reiniciarButton;
        private Button voltarButton;
        private Label historicoLabel;
        private Label historicoText;
        private HashSet<int> numerosSorteados;
        private Random random;

        public SorteioApp()
        {
            InitializeComponent();
            random = new Random();
            numerosSorteados = new HashSet<int>();
        }

        private void InitializeComponent()
        {
            this.Text = "Sorteio de Números";
            this.Size = new Size(1920, 1080);
            this.BackColor = Color.FromArgb(240, 240, 240);

            configPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240)
            };

            intervaloLabel = new Label
            {
                Text = "Intervalo de números",
                Font = new Font("Helvetica", 36),
                AutoSize = true
            };
            intervaloLabel.Location = new Point((this.ClientSize.Width - intervaloLabel.Width) / 2, 20);
            configPanel.Controls.Add(intervaloLabel);

            inicioLabel = new Label
            {
                Text = "Início:",
                Font = new Font("Helvetica", 24),
                AutoSize = true
            };
            inicioLabel.Location = new Point(this.ClientSize.Width / 2 - 200, 100);
            configPanel.Controls.Add(inicioLabel);

            inicioTextBox = new TextBox
            {
                Font = new Font("Helvetica", 24),
                Width = 100
            };
            inicioTextBox.Location = new Point(inicioLabel.Right + 10, 100);
            configPanel.Controls.Add(inicioTextBox);

            fimLabel = new Label
            {
                Text = "Fim:",
                Font = new Font("Helvetica", 24),
                AutoSize = true
            };
            fimLabel.Location = new Point(inicioTextBox.Right + 20, 100);
            configPanel.Controls.Add(fimLabel);

            fimTextBox = new TextBox
            {
                Font = new Font("Helvetica", 24),
                Width = 100
            };
            fimTextBox.Location = new Point(fimLabel.Right + 10, 100);
            configPanel.Controls.Add(fimTextBox);

            repetirCheckBox = new CheckBox
            {
                Text = "Permitir repetição",
                Font = new Font("Helvetica", 24),
                AutoSize = true
            };
            repetirCheckBox.Location = new Point((this.ClientSize.Width - repetirCheckBox.Width) / 2, 180);
            configPanel.Controls.Add(repetirCheckBox);

            iniciarButton = new Button
            {
                Text = "Iniciar Sorteio",
                Font = new Font("Helvetica", 24),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                AutoSize = true
            };
            iniciarButton.Location = new Point((this.ClientSize.Width - iniciarButton.Width) / 2, 260);
            iniciarButton.Click += ConfigurarSorteio;
            configPanel.Controls.Add(iniciarButton);

            this.Controls.Add(configPanel);

            sorteioPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 240, 240),
                Visible = false
            };

            intervaloSorteioLabel = new Label
            {
                Font = new Font("Helvetica", 36),
                AutoSize = true
            };
            intervaloSorteioLabel.Location = new Point((this.ClientSize.Width - intervaloSorteioLabel.Width) / 2, 20);
            sorteioPanel.Controls.Add(intervaloSorteioLabel);

            resultLabel = new Label
            {
                Font = new Font("Helvetica", 400),
                AutoSize = true
            };
            resultLabel.Location = new Point((this.ClientSize.Width - resultLabel.Width) / 2, (this.ClientSize.Height - resultLabel.Height) / 2);
            sorteioPanel.Controls.Add(resultLabel);

            sortearButton = new Button
            {
                Text = "Sortear",
                Font = new Font("Helvetica", 24),
                BackColor = Color.FromArgb(76, 175, 80),
                ForeColor = Color.White,
                AutoSize = true
            };
            sortearButton.Location = new Point(40, this.ClientSize.Height - sortearButton.Height - 40);
            sortearButton.Click += Sortear;
            sorteioPanel.Controls.Add(sortearButton);

            reiniciarButton = new Button
            {
                Text = "Reiniciar Sorteio",
                Font = new Font("Helvetica", 24),
                BackColor = Color.FromArgb(255, 87, 51),
                ForeColor = Color.White,
                AutoSize = true
            };
            reiniciarButton.Location = new Point(sortearButton.Right + 20, this.ClientSize.Height - reiniciarButton.Height - 40);
            reiniciarButton.Click += ReiniciarSorteio;
            sorteioPanel.Controls.Add(reiniciarButton);

            voltarButton = new Button
            {
                Text = "Voltar",
                Font = new Font("Helvetica", 24),
                BackColor = Color.FromArgb(255, 87, 51),
                ForeColor = Color.White,
                AutoSize = true
            };
            voltarButton.Location = new Point(reiniciarButton.Right + 20, this.ClientSize.Height - voltarButton.Height - 40);
            voltarButton.Click += VoltarConfig;
            sorteioPanel.Controls.Add(voltarButton);

            historicoLabel = new Label
            {
                Text = "Histórico de Números Sorteados:",
                Font = new Font("Helvetica", 24),
                AutoSize = true
            };
            historicoLabel.Location = new Point(this.ClientSize.Width - historicoLabel.Width - 40, 40);
            sorteioPanel.Controls.Add(historicoLabel);

            historicoText = new Label
            {
                Font = new Font("Helvetica", 18),
                AutoSize = true
            };
            historicoText.Location = new Point(this.ClientSize.Width - 300, historicoLabel.Bottom + 20);
            sorteioPanel.Controls.Add(historicoText);

            this.Controls.Add(sorteioPanel);
        }

        private void ConfigurarSorteio(object sender, EventArgs e)
        {
            configPanel.Visible = false;
            sorteioPanel.Visible = true;

            string minNum = inicioTextBox.Text;
            string maxNum = fimTextBox.Text;
            intervaloSorteioLabel.Text = $"Sorteando números de {minNum} a {maxNum}";
            intervaloSorteioLabel.Location = new Point((this.ClientSize.Width - intervaloSorteioLabel.Width) / 2, 20);
        }

        private async void Sortear(object sender, EventArgs e)
        {
            bool repetir = repetirCheckBox.Checked;
            int minNum = int.Parse(inicioTextBox.Text);
            int maxNum = int.Parse(fimTextBox.Text);

            int numero = random.Next(minNum, maxNum + 1);
            if (!repetir)
            {
                while (numerosSorteados.Contains(numero))
                {
                    numero = random.Next(minNum, maxNum + 1);
                }
                numerosSorteados.Add(numero);
            }

            await AnimarSorteio(numero, minNum, maxNum);
            AtualizarHistorico(numero);
        }

        private async Task AnimarSorteio(int numeroFinal, int minNum, int maxNum)
        {
            for (int i = 0; i < 10; i++)
            {
                resultLabel.Text = random.Next(minNum, maxNum + 1).ToString();
                await Task.Delay(100);
            }

            for (int i = 0; i < 10; i++)
            {
                resultLabel.Text = random.Next(minNum, maxNum + 1).ToString();
                await Task.Delay(200);
            }

            resultLabel.Text = numeroFinal.ToString();
            resultLabel.Location = new Point((this.ClientSize.Width - resultLabel.Width) / 2, (this.ClientSize.Height - resultLabel.Height) / 2);
        }

        private void AtualizarHistorico(int numero)
        {
            historicoText.Text = historicoText.Text == "" ? numero.ToString() : $"{historicoText.Text} {numero}";
        }

        private void ReiniciarSorteio(object sender, EventArgs e)
        {
            numerosSorteados.Clear();
            historicoText.Text = "";
            resultLabel.Text = "";
        }

        private void VoltarConfig(object sender, EventArgs e)
        {
            sorteioPanel.Visible = false;
            configPanel.Visible = true;
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SorteioApp());
        }
    }
}

