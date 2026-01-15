using System;
using System.Data;
using MySqlConnector;
using System.Windows.Forms;
using PRDBMS.DAL;

namespace PRDBMS
{
    public partial class FHoaDon : Form
    {
        private readonly ConnectDatabase db = new ConnectDatabase();
        private readonly string manv;

        public FHoaDon(string manv)
        {
            InitializeComponent();
            this.manv = manv;
        }

        private void LoadKhachHangCombo()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT MaKH, TenKH FROM KhachHang ORDER BY MaKH;",
                    db.getConnection
                );

                MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);

                if (!dt.Columns.Contains("HienThi"))
                    dt.Columns.Add("HienThi", typeof(string), "MaKH + ' - ' + TenKH");

                comboBox1.DataSource = dt;
                comboBox1.DisplayMember = "HienThi";
                comboBox1.ValueMember = "MaKH";
                comboBox1.SelectedIndex = -1;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không load được khách hàng: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSanPhamCombo()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT MaSP, TenSP FROM SanPham ORDER BY MaSP;",
                    db.getConnection
                );

                MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);

                if (!dt.Columns.Contains("HienThi"))
                    dt.Columns.Add("HienThi", typeof(string), "MaSP + ' - ' + TenSP");

                comboBox3.DataSource = dt;
                comboBox3.DisplayMember = "HienThi";
                comboBox3.ValueMember = "MaSP";
                comboBox3.SelectedIndex = -1;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không load được sản phẩm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMaHoaDonCombo()
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT MaHD FROM HoaDon ORDER BY NgayGiaoDich DESC, MaHD DESC;",
                    db.getConnection
                );

                MySqlDataAdapter ad = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);

                comboBox4.DataSource = dt;
                comboBox4.DisplayMember = "MaHD";
                comboBox4.ValueMember = "MaHD";
                comboBox4.SelectedIndex = -1;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không load được mã hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void FHoaDon_Load(object sender, EventArgs e)
        {
            try
            {
   
                LoadKhachHangCombo();
                LoadSanPhamCombo();
                LoadMaHoaDonCombo();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM view_DSHoaDon", db.getConnection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Không thể truy cập dữ liệu hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private DataTable FHoaDon2_Load()
        {
            DataTable dataTable = new DataTable();

            try
            {
                string maHD = comboBox4.Text.Trim();
                if (string.IsNullOrWhiteSpace(maHD))
                    return dataTable;

                MySqlCommand command = new MySqlCommand("func_ChiTietHoaDon_MaHD", db.getConnection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new MySqlParameter("MaHD", MySqlDbType.VarChar) { Value = maHD });

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                db.openConnection();
                adapter.Fill(dataTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load chi tiết hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
            }

            return dataTable;
        }

        private void CreateHoaDonAndAddCTHD_Transaction(string maHD, DateTime ngay, string maKH, string maNV,
                                                       string maSP, int soLuong)
        {
            if (string.IsNullOrWhiteSpace(maHD)) throw new Exception("Mã hóa đơn không được rỗng.");
            if (string.IsNullOrWhiteSpace(maKH)) throw new Exception("Chưa chọn khách hàng.");
            if (string.IsNullOrWhiteSpace(maNV)) throw new Exception("Mã nhân viên không hợp lệ.");
            if (string.IsNullOrWhiteSpace(maSP)) throw new Exception("Chưa chọn sản phẩm.");
            if (soLuong <= 0) throw new Exception("Số lượng phải > 0.");

            db.openConnection();
            MySqlTransaction tx = db.getConnection.BeginTransaction();

            try
            {
                MySqlCommand cmdCheck = new MySqlCommand(
                    "SELECT COUNT(*) FROM HoaDon WHERE MaHD = @MaHD;",
                    db.getConnection, tx
                );
                cmdCheck.Parameters.AddWithValue("@MaHD", maHD);
                long exists = Convert.ToInt64(cmdCheck.ExecuteScalar());

                if (exists == 0)
                {
                    MySqlCommand cmdAddHD = new MySqlCommand("AddHoaDon", db.getConnection, tx);
                    cmdAddHD.CommandType = CommandType.StoredProcedure;

                    cmdAddHD.Parameters.Add(new MySqlParameter("MaHDIn", MySqlDbType.VarChar) { Value = maHD });
                    cmdAddHD.Parameters.Add(new MySqlParameter("NgayGiaoDichIn", MySqlDbType.Date) { Value = ngay.Date });
                    cmdAddHD.Parameters.Add(new MySqlParameter("MaKHIn", MySqlDbType.VarChar) { Value = maKH });
                    cmdAddHD.Parameters.Add(new MySqlParameter("MaNVIn", MySqlDbType.VarChar) { Value = maNV });

                    cmdAddHD.ExecuteNonQuery();
                }

                MySqlCommand cmdAddCT = new MySqlCommand("AddChiTietHoaDon", db.getConnection, tx);
                cmdAddCT.CommandType = CommandType.StoredProcedure;

                cmdAddCT.Parameters.Add(new MySqlParameter("MaHD", MySqlDbType.VarChar) { Value = maHD });
                cmdAddCT.Parameters.Add(new MySqlParameter("MaSP", MySqlDbType.VarChar) { Value = maSP });
                cmdAddCT.Parameters.Add(new MySqlParameter("SoLuong", MySqlDbType.Int32) { Value = soLuong });

                cmdAddCT.ExecuteNonQuery();

                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
            finally
            {
                db.closeConnection();
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selected = tabControl1.SelectedTab;
            if (selected == tabPage1)
            {
                FHoaDon_Load(sender, e);
            }
            else if (selected == tabPage2)
            {

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maKH = comboBox1.SelectedValue.ToString();
                string maHD = textBox2.Text.Trim();

                if (string.IsNullOrWhiteSpace(maHD))
                {
                    MessageBox.Show("Vui lòng nhập Mã hóa đơn.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.openConnection();

                MySqlCommand cmd = new MySqlCommand("AddHoaDon", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new MySqlParameter("MaHDIn", MySqlDbType.VarChar) { Value = maHD });
                cmd.Parameters.Add(new MySqlParameter("NgayGiaoDichIn", MySqlDbType.Date) { Value = dateTimePicker1.Value.Date });
                cmd.Parameters.Add(new MySqlParameter("MaKHIn", MySqlDbType.VarChar) { Value = maKH });
                cmd.Parameters.Add(new MySqlParameter("MaNVIn", MySqlDbType.VarChar) { Value = manv });

                int rowsAffected = cmd.ExecuteNonQuery();

                MessageBox.Show(rowsAffected > 0 ? "Thêm thành công." : "Thêm thất bại.",
                    "Thông báo", MessageBoxButtons.OK,
                    rowsAffected > 0 ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi MySQL: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
            }

            FHoaDon_Load(sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // MaHD: ưu tiên comboBox4, nếu trống thì lấy từ textBox2
                string maHD = comboBox4.Text.Trim();
                if (string.IsNullOrWhiteSpace(maHD))
                    maHD = textBox2.Text.Trim();

                if (string.IsNullOrWhiteSpace(maHD))
                {
                    MessageBox.Show("Vui lòng nhập/chọn Mã hóa đơn (textBox2 hoặc comboBox4).",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBox1.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (comboBox3.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn sản phẩm.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string maKH = comboBox1.SelectedValue.ToString();
                string maSP = comboBox3.SelectedValue.ToString();
                int soLuong = Convert.ToInt32(numericUpDown2.Value);

                CreateHoaDonAndAddCTHD_Transaction(
                    maHD,
                    dateTimePicker1.Value,
                    maKH,
                    manv,
                    maSP,
                    soLuong
                );

                MessageBox.Show("Thêm thành công.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload
                FHoaDon_Load(sender, e);
                comboBox4.Text = maHD;
                dataGridView2.DataSource = FHoaDon2_Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = FHoaDon2_Load();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string keyword = textBox1.Text.Trim();

                MySqlCommand cmd = new MySqlCommand("SearchHoaDon", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("Keyword", MySqlDbType.VarChar) { Value = keyword });

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tìm kiếm hóa đơn: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string maHD = textBox2.Text.Trim();
                if (string.IsNullOrWhiteSpace(maHD))
                {
                    MessageBox.Show("Vui lòng nhập Mã hóa đơn.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                db.openConnection();

                MySqlCommand cmd = new MySqlCommand("UpdateHoaDon", db.getConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new MySqlParameter("MaHD", MySqlDbType.VarChar) { Value = maHD });

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Cập nhật hóa đơn thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Cập nhật hóa đơn thất bại.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
            }

            FHoaDon_Load(sender, e);
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
