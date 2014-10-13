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
            GoodMatrix.Rows.Add(4);
            
        }

        private void запускToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //Solver slv = new Solver();
            int[] data = { 80, 80, 800, 800 };
            int[] L = { 8, 10, 80, 100 };
            for (int i = 0; i < 4; i++)
            {
                dgvResult.Rows[i].Cells[0].Value = (i + 1).ToString(); // номер
                dgvResult.Rows[i].Cells[1].Value = data[i].ToString(); // размерность
                dgvResult.Rows[i].Cells[2].Value = ((double)L[i] / data[i]).ToString(); // Отношение L/N

                dgvResult.Rows[i].Cells[3].Value = "";
            }
            Application.DoEvents();
            for (int i = 0; i < 4; i++)
            {
                Solver slv = new Solver();
                double avg = 0;
                dgvResult.Rows[i].Cells[2].Value = ((double)L[i] / data[i]).ToString(); // Отношение L/N
                slv.Form_Answer(10, data[i], L[i], ref avg);
                dgvResult.Rows[i].Cells[3].Value = avg.ToString("G3", CultureInfo.InvariantCulture);
                Application.DoEvents();
            }
            int[] matr = {40,60, 280, 670};
            for (int i = 0; i < 4; i++)
            {
                GoodMatrix.Rows[i].Cells[0].Value = (i + 1).ToString(); // номер
                GoodMatrix.Rows[i].Cells[1].Value = matr[i].ToString(); // размерность
                Application.DoEvents();
            }
            for (int i = 0; i < 4; i++)
            {
                Solver slv = new Solver();
                double avg = 0;
                slv.Form_Good_And_Bad_Answer(10, matr[i], ref avg,0);
                GoodMatrix.Rows[i].Cells[2].Value = avg.ToString("G3", CultureInfo.InvariantCulture);
                Application.DoEvents();
            }
        }
    }
}
