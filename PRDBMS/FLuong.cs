using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySqlConnector;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using PRDBMS.DAL;

namespace PRDBMS
{
    public partial class FLuong : Form
    {
        ConnectDatabase db = new ConnectDatabase();
        string manv;
        public FLuong(string manv)
        {
            InitializeComponent();
            this.manv = manv;
        }

        private void FLuong_Load(object sender, EventArgs e)
        {
            MySqlCommand command = new MySqlCommand("proc_BangLuong", db.getConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@MaNV", manv);
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }
    }
}