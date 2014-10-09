using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Task2
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            dgvResult.Rows.Add(4);
            
        }

        private void запускToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            Solver slv = new Solver();
            int[] data = { 60, 500 };
            int[] L = { 6, 15, 50, 125 };
            for (int i = 0; i < 4; i++)
            {
                dgvResult.Rows[i].Cells[0].Value = (i + 1).ToString(); // номер
                dgvResult.Rows[i].Cells[1].Value = data[i / 2].ToString(); // размерность
                dgvResult.Rows[i].Cells[2].Value = ((double)L[i] / data[i / 2]).ToString(); // Отношение L/N

                dgvResult.Rows[i].Cells[3].Value = "";
            }
            Application.DoEvents();
            for (int i = 0; i < 4; i++)
            {

                double avg = 0;
                dgvResult.Rows[i].Cells[0].Value = (i + 1).ToString(); // номер
                dgvResult.Rows[i].Cells[1].Value = data[i / 2].ToString(); // размерность
                dgvResult.Rows[i].Cells[2].Value = ((double)L[i] / data[i / 2]).ToString(); // Отношение L/N
                slv.Form_Answer(100, data[i / 2], L[i], ref avg);
                dgvResult.Rows[i].Cells[3].Value = avg.ToString("G3", CultureInfo.InvariantCulture);
                Application.DoEvents();
            }
        }
    }
}
