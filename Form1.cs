using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR_2_Fin
{
    
    public partial class Form1 : Form
    {
        string input;
        string hdb3;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            input = textBox1.Text;
            if (input == "")
            {
                MessageBox.Show(
                    "Вы не ввели сообщение, попробуйте снова",
                    "Сообщение",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly
                );
                return;
            }
            
            execPaint();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) { }
        //private void textBox2_TextChanged(object sender, EventArgs e) { }



        //private void panel1_Paint(object sender, PaintEventArgs e) { }

        void swBlock(char ch, ref int x, ref int y, ref int nowX, ref int nowY, 
            ref bool minus, ref bool zero)
        {
            if (ch == '1')
            {
                x = nowX;
                y = nowY - 25;
            }
            else if (ch == '-')
            {
                x = nowX;
                y = nowY + 25;
                minus = true;
            }
            else
            {
                x = nowX + 17;
                y = nowY;
                nowX = x;
                zero = true;
            }
        }

        private void execPaint()
        {
            Encoding obj = new Encoding(input);
            string ansAmi = obj.Ami();
            string[] ansHDB3 = obj.Hdb3();
            hdb3 = ansHDB3[0];
            Pen p = new Pen(Color.Black, 2);   // цвет линии и ширина

            int nowX_AMI = 90;
            int nowY_AMI = 170;
            paint(p, nowX_AMI, nowY_AMI, "AMI", ansAmi);

            int nowX_HDB3 = 90;
            int nowY_HDB3 = 270;
            paint(p, nowX_HDB3, nowY_HDB3, "HDB3", ansHDB3[1]);
        }

        private void paint(Pen p, int nowX, int nowY, string nazv, string strKod)
        {
            var gr = CreateGraphics();

            int startX = nowX;
            int startLine = nowY;
            int x = nowX;
            int y = nowY;
            bool minus = false;
            bool zero = false;
            Point p1;
            Point p2;

            Font drawFont = new Font("Arial", 16);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            gr.DrawString(nazv, drawFont, drawBrush, nowX-90, nowY - 10);


            Point pKoord1 = new Point(nowX - 10, nowY - 35);
            Point pKoord2 = new Point(nowX - 10, nowY + 35);
            Pen penKoord = new Pen(Color.DarkGray, 1);

            gr.DrawLine(penKoord, pKoord1, pKoord2);
            pKoord1 = new Point(nowX - 10, startLine); //Точка для будущей отрисовки оси ОХ

            Font drawFont2 = new Font("Arial", 10);
            SolidBrush drawBrush2 = new SolidBrush(Color.DarkGray);
            gr.DrawString("1", drawFont2, drawBrush2, nowX - 25, nowY - 35);
            gr.DrawString("-1", drawFont2, drawBrush2, nowX - 25, nowY + 30);

            for (int i = 0; i < strKod.Length; i++)
            {
                if (minus)
                {
                    minus = !minus;
                    continue;
                }

                p1 = new Point(nowX, nowY);   // первая точка

                if (nowY != startLine)
                {
                    nowX = x;
                    nowY = startLine;

                    p1 = new Point(nowX, nowY);
                    p2 = new Point(x, y);
                    gr.DrawLine(p, p1, p2);
                }
                swBlock(strKod[i], ref x, ref y, ref nowX, ref nowY, ref minus, ref zero);
                

                p2 = new Point(x, y);         // вторая точка
                gr.DrawLine(p, p1, p2);

                if (!zero)
                {
                    nowX = x;
                    nowY = y;

                    p1 = new Point(nowX, nowY);

                    x = nowX + 17;
                    y = nowY;

                    p2 = new Point(x, y);
                    gr.DrawLine(p, p1, p2);
                }
                zero = false;
            }

            pKoord2 = new Point(nowX + 50, startLine);
            gr.DrawLine(penKoord, pKoord1, pKoord2);

            if (nazv == "HDB3")
            {
                int Xos = startX;
                for (int i = 0; i < hdb3.Length; i++)
                {
                    gr.DrawString(hdb3[i].ToString(), drawFont2, drawBrush, Xos, startLine + 40);
                    Xos += 17;
                }
            }
            gr.Dispose();
        }

    }

    public class Encoding
    {
        public string str;

        public Encoding(string getString)
        {
            str = getString;
        }

        public string analis(char ch, ref bool pred)
        {
            bool sigB = false;
            switch (ch)
            {
                case '1':
                    if (pred)
                    {
                        pred = !pred;
                        return "1";
                    }
                    else
                    {
                        pred = !pred;
                        return "-1";
                    }
                case 'B':
                    if (pred)
                    {
                        pred = !pred;
                        return "1";
                    }
                    else
                    {
                        pred = !pred;
                        return "-1";
                    }

                case 'V':
                    sigB = false;
                    if (!pred)
                        return "1";
                    else
                        return "-1";

                default:
                    return "0";
            }
        }

        public string Ami()
        {
            string strAns = "";
            string ch;
            bool UpDown = true;

            foreach (char i in str)
            {
                ch = analis(i, ref UpDown);
                strAns += ch;
            }
            return strAns;
        }

        public void change(ref string st, int i, bool num)
        {
            if (num)
            {
                st = st.Remove(i - 3, 4);
                st = st.Insert(i - 3, "B00V");
            }
            else
            {
                st = st.Remove(i, 1);
                st = st.Insert(i, "V");
            }
        }
        public string[] Hdb3()
        {
            int count0 = 0;
            int count1 = 0;
            string st = str;

            for (int i = 0; i < st.Length; i++)
            {
                if (st[i] == '0')
                {
                    count0++;
                }
                if (st[i] == '1')
                {
                    count1++;
                    count0 = 0;
                }

                if (count0 == 4)
                {
                    if (count1 % 2 != 0)
                    {
                        change(ref st, i, false);
                    }
                    else
                    {
                        change(ref st, i, true);
                    }
                    count0 = 0;
                    count1 = 0;
                }
            }

            string strAns = "";
            string ch;
            bool UpDown = true;

            foreach (char i in st)
            {
                ch = analis(i, ref UpDown);
                strAns += ch;
            }
            return new string[] { st, strAns };
        }

    }
}
