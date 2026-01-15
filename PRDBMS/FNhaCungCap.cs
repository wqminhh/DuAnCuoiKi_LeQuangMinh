using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySqlConnector;
using PRDBMS.DAL;

namespace PRDBMS
{
    public partial class FNhaCungCap : Form
    {
        ConnectDatabase db = new ConnectDatabase();

        public FNhaCungCap()
        {
            InitializeComponent();
        }

        private void FNhaCungCap_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT * FROM view_DSNhaCungCap",
                    db.getConnection
                );

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Không thể truy cập dữ liệu.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = textBox1.Text.Trim();

                MySqlCommand cmd = new MySqlCommand(
                    "SearchNhaCungCap",
                    db.getConnection
                );
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", keyword);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(
                    "Lỗi tìm kiếm nhà cung cấp.\n" + ex.Message,
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
    }
}
