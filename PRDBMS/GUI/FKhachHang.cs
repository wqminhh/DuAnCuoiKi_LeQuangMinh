using System;
using System.Data;
using System.Windows.Forms;
using MySqlConnector;
using PRDBMS.DAL;

namespace PRDBMS
{
    public partial class FKhachHang : Form
    {
        private readonly ConnectDatabase db = new ConnectDatabase();

        public FKhachHang()
        {
            InitializeComponent();
        }

        private void FKhachHang_Load(object sender, EventArgs e)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM view_DSKhachHang", db.getConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không thể truy cập dữ liệu: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            try
            {
                string keyword = textBox1.Text.Trim();

                MySqlCommand cmd = new MySqlCommand("SearchKhachHang", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("Keyword", MySqlDbType.VarChar) { Value = keyword });

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi tìm kiếm khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                db.openConnection();

                MySqlCommand cmd = new MySqlCommand("AddKhachHang", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("MaKH", MySqlDbType.VarChar) { Value = textBox2.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("TenKH", MySqlDbType.VarChar) { Value = textBox3.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("GioiTinh", MySqlDbType.VarChar) { Value = comboBox1.Text });
                cmd.Parameters.Add(new MySqlParameter("DiaChi", MySqlDbType.VarChar) { Value = textBox5.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("SDT", MySqlDbType.VarChar) { Value = textBox6.Text.Trim() });

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    MessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Thêm thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi khi thêm khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
            }

            FKhachHang_Load(sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                db.openConnection();

                MySqlCommand cmd = new MySqlCommand("UpdateKhachHang", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("MaKH", MySqlDbType.VarChar) { Value = textBox2.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("DiaChi", MySqlDbType.VarChar) { Value = textBox5.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("SDT", MySqlDbType.VarChar) { Value = textBox6.Text.Trim() });

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                    MessageBox.Show("Sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show("Sửa thất bại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi khi sửa khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
            }

            FKhachHang_Load(sender, e);
        }
    }
}
