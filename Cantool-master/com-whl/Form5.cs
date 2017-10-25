using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static sf.Form1;
using static sf.DBHelper;
using System.Collections;
using System.Windows.Shapes;

namespace sf
{
    public partial class Form5 : Form
    {

        Form1 form1 = new Form1();
        DBHelper dbHelper = new DBHelper();
        Color sentColor = Color.Blue;
        ArrayList lines = new ArrayList();

        public Form5()
        {
            InitializeComponent();

            //数据库连接字符串,CanMessage
            string sql1 = string.Format("select MessageName from CanMessage");
            DBHelper db1 = new DBHelper();
            //控件名.DataSource=数据集.数据表
            comboBox1.DataSource = db1.GetDataSet(sql1).Tables[0];
            comboBox1.DisplayMember = "MessageName";
            comboBox1.ValueMember = "MessageName";

            
        }

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text1 = comboBox1.SelectedValue.ToString();


            string sql2 = string.Format("select * from CanSignal where ID=(select ID from CanMessage where MessageName = '{0}');", text1);
            DBHelper db2 = new DBHelper();
            //控件名.DataSource=数据集.数据表
            comboBox2.DataSource = db2.GetDataSet(sql2).Tables[0];
            comboBox2.DisplayMember = "SignalName";
            comboBox2.ValueMember = "SignalName";
        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cmd1 = comboBox1.Text;

            string sql2 = string.Format("select ID from CanMessage where MessageName = '{0}';", cmd1);
            DBHelper db2 = new DBHelper();
            //控件名.DataSource=数据集.数据表
            int id = db2.SelectIdInMessage(sql2);
            string iD = id.ToString();

            
            //int id = dbHelper.SelectIdInMessage(cmd1);
            string cmd2 = comboBox2.Text;
            string cmd3 = domainUpDown1.Text;
            string cmd4 = richTextBox1.Text;
            string cmd5 = textBox3.Text;

            string cmd = 't' + iD + cmd3 + cmd4 + cmd5;

            //comboBox1.Items.Add(comboBox1.Text);
            //comboBox1.Text = "";
            //comboBox1.Focus();

            if (cmd.Length > 0)
            {
                //cmd1 = form1.ConvertEscapeSequences(cmd1);

                CommPort com = CommPort.Instance;
                com.Send(cmd);

                if (Settings.Option.LocalEcho)
                {
                    form1.outputList_Add(cmd + "\r", sentColor);
                }
            }
        }

        /// <summary>
        /// 在输出窗口中添加一行
        /// </summary>
        //Line outputList_Add(string str, Color color)
        //{
        //    Line newLine = new Line(str, color);
        //    lines.Add(newLine);

        //    if (outputList_ApplyFilter(newLine.Str))
        //    {
        //        outputList.Items.Add(newLine);
        //        outputList_Scroll();
        //    }

        //    return newLine;
        //}

        /// <summary>
        /// 转义序列
        /// </summary>
        //private string ConvertEscapeSequences(string s)
        //{
        //    Expecting expecting = Expecting.ANY;

        //    int hexNum = 0;
        //    string outs = "";
        //    foreach (char c in s)
        //    {
        //        switch (expecting)
        //        {
        //            case Expecting.ANY:
        //                if (c == '\\')
        //                    expecting = Expecting.ESCAPED_CHAR;
        //                else
        //                    outs += c;
        //                break;
        //            case Expecting.ESCAPED_CHAR:
        //                if (c == 'x')
        //                {
        //                    expecting = Expecting.HEX_1ST_DIGIT;
        //                }
        //                else
        //                {
        //                    char c2 = c;
        //                    switch (c)
        //                    {
        //                        case 'n': c2 = '\n'; break;
        //                        case 'r': c2 = '\r'; break;
        //                        case 't': c2 = '\t'; break;
        //                    }
        //                    outs += c2;
        //                    expecting = Expecting.ANY;
        //                }
        //                break;
        //            case Expecting.HEX_1ST_DIGIT:
        //                hexNum = GetHexDigit(c) * 16;
        //                expecting = Expecting.HEX_2ND_DIGIT;
        //                break;
        //            case Expecting.HEX_2ND_DIGIT:
        //                hexNum += GetHexDigit(c);
        //                outs += (char)hexNum;
        //                expecting = Expecting.ANY;
        //                break;
        //        }
        //    }
        //    return outs;
        //}
    }
}
