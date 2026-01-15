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
    public partial class FNhanVien : Form
    {
        ConnectDatabase db = new ConnectDatabase();

        public FNhanVien()
        {
            InitializeComponent();
        }

        private void FNhanVien_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCuaHangCombo();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM view_DSNhanVien", db.getConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không thể truy cập dữ liệu. " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = textBox1.Text.Trim();
                MySqlCommand cmd = new MySqlCommand("SearchNhanVien", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Keyword", keyword);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi tìm kiếm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox2.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn Mã cửa hàng.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.openConnection();

                MySqlCommand cmd = new MySqlCommand("UpdateNhanVien", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("MaNVIn", MySqlDbType.VarChar) { Value = textBox2.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("TenNVNew", MySqlDbType.VarChar) { Value = textBox3.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("GioiTinhNew", MySqlDbType.VarChar) { Value = comboBox1.Text });
                cmd.Parameters.Add(new MySqlParameter("NgaySinhNew", MySqlDbType.Date) { Value = dateTimePicker1.Value.Date });
                cmd.Parameters.Add(new MySqlParameter("QueQuanNew", MySqlDbType.VarChar) { Value = textBox4.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("DiaChiNew", MySqlDbType.VarChar) { Value = textBox5.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("SDTNew", MySqlDbType.VarChar) { Value = textBox6.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("MailNew", MySqlDbType.VarChar) { Value = textBox7.Text.Trim() });
                cmd.Parameters.Add(new MySqlParameter("ChucVuNew", MySqlDbType.VarChar) { Value = comboBox3.Text });
                cmd.Parameters.Add(new MySqlParameter("MaCHNew", MySqlDbType.VarChar) { Value = comboBox2.SelectedValue.ToString() });

                int rows = cmd.ExecuteNonQuery();

                MessageBox.Show(rows > 0 ? "Cập nhật thành công." : "Không có thay đổi.",
                    "Thông báo", MessageBoxButtons.OK,
                    rows > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi khi cập nhật nhân viên: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
                FNhanVien_Load(sender, e);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                db.openConnection();
                MySqlCommand cmd = new MySqlCommand("AddNhanVien", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("@MaNV", MySqlDbType.VarChar) { Value = textBox2.Text });
                cmd.Parameters.Add(new MySqlParameter("@TenNV", MySqlDbType.VarChar) { Value = textBox3.Text });
                cmd.Parameters.Add(new MySqlParameter("@GioiTinh", MySqlDbType.VarChar) { Value = comboBox1.Text });
                cmd.Parameters.Add(new MySqlParameter("@NgaySinh", MySqlDbType.Date) { Value = dateTimePicker1.Value.Date });
                cmd.Parameters.Add(new MySqlParameter("@QueQuan", MySqlDbType.VarChar) { Value = textBox4.Text });
                cmd.Parameters.Add(new MySqlParameter("@DiaChi", MySqlDbType.VarChar) { Value = textBox5.Text });
                cmd.Parameters.Add(new MySqlParameter("@SDT", MySqlDbType.VarChar) { Value = textBox6.Text });
                cmd.Parameters.Add(new MySqlParameter("@Mail", MySqlDbType.VarChar) { Value = textBox7.Text });
                cmd.Parameters.Add(new MySqlParameter("@ChucVu", MySqlDbType.VarChar) { Value = comboBox3.Text });
                cmd.Parameters.Add(new MySqlParameter("@MaCH", MySqlDbType.VarChar) { Value = comboBox2.SelectedValue?.ToString() });

                cmd.ExecuteNonQuery();
                MessageBox.Show("Thêm thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi khi thêm nhân viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
                FNhanVien_Load(sender, e);
            }
        }

        private void LoadCuaHangCombo()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT MaCH, TenCH FROM CuaHang ORDER BY MaCH;",
                    db.getConnection
                );

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dt.Columns.Add("HienThi", typeof(string), "MaCH + ' - ' + TenCH");

                comboBox2.DataSource = dt;
                comboBox2.DisplayMember = "HienThi";
                comboBox2.ValueMember = "MaCH";
                comboBox2.SelectedIndex = -1;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không load được Mã cửa hàng: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}