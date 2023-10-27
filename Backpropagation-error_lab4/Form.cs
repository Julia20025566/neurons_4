using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace Backpropagation_error_lab4
{
    public partial class Form : System.Windows.Forms.Form
    {
        public int m { get; set; } = 9;
        public int n { get; set; } = 9;
        public int sample { get; set; } = 12;
        public double[] input { get; set; }
        public Neuronet net;
        
        public Form()
        {
            InitializeComponent();
        }

        
        private void Form_Load(object sender, EventArgs e)
        {
            dataRecog.RowCount = sample;
            
            dataInput.ColumnCount = n;
            dataInput.RowCount = m;

            dataRecog.Rows[0].Cells[0].Selected = false;
            dataInput.Rows[0].Cells[0].Selected = false;

            this.button2.Enabled = false;
          /*  for (int i = 0; i < 12; i++)
            {
                dataRecog.Rows[i].Cells[0].Value = (i+1).ToString();
            }*/
            dataRecog.Rows[0].Cells[0].Value = "Водолей";
            dataRecog.Rows[1].Cells[0].Value = "Овен";
            dataRecog.Rows[2].Cells[0].Value = "Козерог";
            dataRecog.Rows[3].Cells[0].Value = "Близнецы";
            dataRecog.Rows[4].Cells[0].Value = "Рыбы";
            dataRecog.Rows[5].Cells[0].Value = "Весы";
            dataRecog.Rows[6].Cells[0].Value = "Телец";
            dataRecog.Rows[7].Cells[0].Value = "Стрелец";
            dataRecog.Rows[8].Cells[0].Value = "Скорпион";
            dataRecog.Rows[9].Cells[0].Value = "Лев";
            dataRecog.Rows[10].Cells[0].Value = "Рак";
            dataRecog.Rows[11].Cells[0].Value = "Дева";
        }

        private void dataInput_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex; int j = e.ColumnIndex;
            if (dataInput.Rows[i].Cells[j].Style.BackColor == Color.Empty)            
                dataInput.Rows[i].Cells[j].Style.BackColor = Color.Black;
            else
                dataInput.Rows[i].Cells[j].Style.BackColor = Color.Empty;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            input = new double[m*n];
            
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (dataInput.Rows[i].Cells[j].Style.BackColor == Color.Black)
                    {
                        input[i * n + j] = 1;
                    }
                    else
                    {
                        input[i * n + j] = 0;
                    }
                }
            }
            int check = net.test(input);
            for (int i = 0; i < sample; i++)
            {
                dataRecog.Rows[i].Cells[1].Value = net.actual[i].ToString();
            }
            dataRecog.Rows[check].Selected = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Parser p = new Parser();
             net = new Neuronet(m * n, Int32.Parse(textBox1.Text), sample, double.Parse(textBox2.Text));
             net.assignRandomWeights();
             int epoch = Int32.Parse(textBox3.Text);
             string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Samples\";
             string end = @".png", currentPath;
             for (int i = 0; i < epoch; i++)
             {
                 for (int j = 0; j < sample; j++)
                 {
                     double[] res = new double[sample];
                     currentPath = path + (j + 1).ToString() + end;
                     res[j] = 1;
                     net.learn(p.parse(currentPath), res);
                 }
                 for (int j = 0; j < sample; j++)
                 {
                     double[] res = new double[sample];
                     currentPath = path + (j + 1).ToString() + end;
                     res[j] = 1;
                     net.learn(p.wrap(0.2, currentPath), res);
                 }
                 for (int j = 0; j < sample; j++)
                 {
                     double[] res = new double[sample];
                     currentPath = path + (j + 1).ToString() + end;
                     res[j] = 1;
                     net.learn(p.wrap(0.5, currentPath), res);
                 }
             }*/
            net = new Neuronet(m * n, Int32.Parse(textBox1.Text), sample, double.Parse(textBox2.Text));
            net.assignRandomWeights();
            int epoch = Int32.Parse(textBox3.Text);
            string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Data\";           
            string s;          
            for (int i = 0; i < epoch; i++)
            {
                StreamReader rd = new StreamReader(path + @"data1.txt");
                for (int j = 0; j < sample; j++)
                {
                    double[] input = new double[m * n];
                    double[] res = new double[sample];

                    int r = 0;
                    while ((s = rd.ReadLine()) != null && !String.IsNullOrEmpty(s))
                    {
                        for (int c = 0; c < s.Length; c++)
                        {
                            input[r * n + c] = Int32.Parse(s[c].ToString());
                        }
                        r++;
                    }
                    res[j] = 1;
                    net.learn(input, res);
                }
                rd.Close();
                StreamReader rd2 = new StreamReader(path + @"data2.txt");
                for (int k = 0; k < 2; k++)
                {
                    for (int j = 0; j < sample; j++)
                    {
                        double[] input = new double[m * n];
                        double[] res = new double[sample];

                        int r = 0;
                        while ((s = rd2.ReadLine()) != null && !String.IsNullOrEmpty(s))
                        {
                            for (int c = 0; c < s.Length; c++)
                            {
                                input[r * n + c] = Int32.Parse(s[c].ToString());
                            }
                            r++;
                        }
                        res[j] = 1;
                        net.learn(input, res);
                    }
                }
                rd2.Close();
            }
            button2.Enabled = true;
        }

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    string path = AppDomain.CurrentDomain.BaseDirectory.ToString() + @"Data\";           
        //    StreamWriter w = new StreamWriter(path + @"data2.txt", true);
        //    for (int i = 0; i < m; i++)
        //    {
        //        string tmp = "";
        //        for (int j = 0; j < n; j++)
        //        {                    
        //            if (dataInput.Rows[i].Cells[j].Style.BackColor == Color.Black)
        //            {
        //                tmp += "1";
        //            }
        //            else
        //            {
        //                tmp += "0";
        //            }
                   
        //        }
        //        w.WriteLine(tmp);
        //    }
        //    w.WriteLine();
        //    w.Close();
        //    button2.Enabled = true;
        //}

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (dataInput.Rows[i].Cells[j].Style.BackColor == Color.Black)
                    {
                        dataInput.Rows[i].Cells[j].Style.BackColor = Color.Empty;
                    }
                }
            }
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}
