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
using System.Windows.Forms.DataVisualization.Charting;
using PRDBMS.DAL;

namespace PRDBMS
{
    public partial class FDoanhThu : Form
    {
        ConnectDatabase db = new ConnectDatabase();
        public FDoanhThu()
        {
            InitializeComponent();
        }

        private void FDoanhThu_Load(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;

            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

            LoadData(firstDayOfMonth, today);
        }
        private void LoadData(DateTime formdate, DateTime todate)
        {
            Series series = chart1.Series.FindByName("DoanhThu");

            if (series == null)
            {
                series = chart1.Series.Add("DoanhThu");
            }

            try
            {
                db.openConnection();

                using (MySqlCommand command = new MySqlCommand("DoanhThu", db.getConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@formdate", formdate);
                    command.Parameters.AddWithValue("@todate", todate);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        chart1.Series["DoanhThu"].Points.Clear();

                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                            {
                                decimal tongThanhTien = reader.GetDecimal(0);
                                decimal tongChiPhi = reader.GetDecimal(1);
                                decimal LoiNhuan = reader.GetDecimal(2);

                                chart1.Series["DoanhThu"].Points.AddXY("Doanh Thu", tongThanhTien);
                                chart1.Series["DoanhThu"].Points.AddXY("Lợi Nhuận", LoiNhuan);

                                chart1.Series["DoanhThu"].Points[0].Color = Color.Blue; 
                                chart1.Series["DoanhThu"].Points[1].Color = Color.Green; 

                                labelDoanhThu.Text = tongThanhTien.ToString() + " VNĐ";
                                labelLoiNhuan.Text = LoiNhuan.ToString() + " VNĐ";
                            }
                            else
                            {
                                chart1.Series["DoanhThu"].Points.Clear();
                                labelDoanhThu.Text = "0 VNĐ";
                                labelLoiNhuan.Text = "0 VNĐ";
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Bạn không được phép thực thi lệnh: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu doanh thu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
            }

            chart1.Legends.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            LoadData(firstDayOfMonth, today);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime weekAgo = today.AddDays(-7);
            LoadData(weekAgo, today);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            LoadData(today, today);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}