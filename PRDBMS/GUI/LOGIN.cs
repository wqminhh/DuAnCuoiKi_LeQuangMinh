using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PRDBMS.DAL;

namespace PRDBMS
{
    public partial class LOGIN : Form
    {
        ConnectDatabase db = new ConnectDatabase();

        public LOGIN()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                db.openConnectionADmin();
                string tk = textBox1.Text;
                string mk = textBox2.Text;
                MySqlCommand cmd = new MySqlCommand("SELECT checkLogin(@user, @pass);", db.getConnectionAdmin);

                cmd.Parameters.AddWithValue("@user", tk);
                cmd.Parameters.AddWithValue("@pass", mk);
                bool count = (bool)cmd.ExecuteScalar();
                if (count == true)
                {
                    GLOBAL.username = textBox1.Text;
                    GLOBAL.password = textBox2.Text;
                    MySqlCommand cmdGetMaNV = new MySqlCommand(
                        "SELECT employee_id FROM Account WHERE username = @u LIMIT 1;",
                        db.getConnectionAdmin
                    );
                    cmdGetMaNV.Parameters.AddWithValue("@u", tk);

                    object o = cmdGetMaNV.ExecuteScalar();
                    if (o == null || string.IsNullOrWhiteSpace(o.ToString()))
                    {
                        MessageBox.Show("Tài khoản chưa được gán mã nhân viên trong bảng Account.",
                            "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string maNV = o.ToString().Trim();

                    FMain f = new FMain(maNV);
                    this.Hide();
                    f.ShowDialog();

                    this.Hide();
                    f.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu chưa chính xác vui lòng nhập lại!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
            finally
            {
                db.closeConnectionAdmin(); 
            }
        }
    }
}