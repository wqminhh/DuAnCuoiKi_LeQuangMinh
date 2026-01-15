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
using PRDBMS.DAL;

namespace PRDBMS
{
    public partial class FKhoHang : Form
    {
        ConnectDatabase db = new ConnectDatabase();
        public FKhoHang()
        {
            InitializeComponent();
        }

        private void FKhoHang_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM view_DSPhieuNhap", db.getConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không thể truy cập dữ liệu. " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private DataTable FKhoHang2_Load()
        {
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand("SELECT * FROM func_ChiTietPhieuNhap(@MaPN)", db.getConnection);
            command.Parameters.AddWithValue("@MaPN", comboBox4.Text);
            command.CommandType = CommandType.Text;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            try
            {
                db.openConnection();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }

            return dataTable;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = FKhoHang2_Load();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();
            MySqlCommand cmd = new MySqlCommand("SearchPhieuNhap", db.getConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Keyword", keyword);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataGridView1.DataSource = dataTable;
        }
    }
}