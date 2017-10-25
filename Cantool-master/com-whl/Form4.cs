using System;
using System.Data;
using System.IO;
using System.Windows.Forms;

namespace sf
{
    public partial class Form4 : Form
    {
        private DataTable _dtEmployees;
        public Form4()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            this._dtEmployees = this.CreateDataTable();

            //调用BuildTree方法构建TreeView。
            tree.BuildTree(
                this._dtEmployees, this.treeView1,
                true, "LastName", "EmployeeID", "ReportsTo");
        }

        public void readdbc(StreamWriter File)
        {
            FileStream fsRead = new FileStream(@"", FileMode.Open);//此处输入所使用的数据库文件
            int fsLen = (int)fsRead.Length;
            StreamReader reader = new StreamReader(fsRead);
            string s = reader.ReadToEnd();
            int i = s.IndexOf("BO_ ");
            int j = s.IndexOf(" ");
            string str = s.Substring(i, j);
            // public static extern bool AllocConsole();

        }

        public DataTable CreateDataTable()
        {
            DataTable dataTable = new DataTable();

            // 此列中的值将标识TreeNode
            dataTable.Columns.Add("EmployeeID");

            // 此列中的值将显示在TreeNode上
            dataTable.Columns.Add("LastName");

            // 此列中的值将标识其parentId
            dataTable.Columns.Add("ReportsTo");

            // 填写DataTable
            dataTable.Rows.Add(1, "1", 2);
            dataTable.Rows.Add(2, "2", DBNull.Value);
            dataTable.Rows.Add(3, "3", 2);
            dataTable.Rows.Add(4, "4", 2);
            dataTable.Rows.Add(5, "5", 2);
            dataTable.Rows.Add(6, "6", 5);
            dataTable.Rows.Add(7, "7", 5);
            dataTable.Rows.Add(8, "8", 2);
            dataTable.Rows.Add(9, "9", 5);
            return dataTable;
        }
    }
}
