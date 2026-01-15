using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PRDBMS
{
    public partial class FMain : Form
    {
        string manv;
        public FMain(string manv)
        {
            InitializeComponent();
            this.manv = manv;
        }

        private Form currentFormChild;
        private void OpenchildForm(Form childFrom)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childFrom;
            childFrom.TopLevel = false;
            childFrom.FormBorderStyle = FormBorderStyle.None;
            childFrom.Dock = DockStyle.Fill;
            panel16.Controls.Add(childFrom);
            panel16.Tag = childFrom;
            childFrom.BringToFront();
            childFrom.Show();
        }

        private void OpenchildForm1(Form childFrom)
        {
            if (currentFormChild != null)
            {
                currentFormChild.Close();
            }
            currentFormChild = childFrom;
            childFrom.TopLevel = false;
            childFrom.FormBorderStyle = FormBorderStyle.None;
            childFrom.Dock = DockStyle.Fill;
            panel10.Controls.Add(childFrom);
            panel10.Tag = childFrom;
            childFrom.BringToFront();
            childFrom.Show();
        }

        private void label2_Click(object sender, EventArgs e)
        {
          
        }

        private void label4_Click(object sender, EventArgs e)
        {
            FLuong fluong = new FLuong(manv);
            OpenchildForm(fluong);
        }

        private void label8_Click(object sender, EventArgs e)
        {
            label12.Text = "Nhân viên";
            FNhanVien fnhanvien = new FNhanVien();
            OpenchildForm(fnhanvien);
        }

        private void label5_Click(object sender, EventArgs e)
        {
            label12.Text = "Hoá đơn";
            FHoaDon fhoadon = new FHoaDon(manv);
            OpenchildForm(fhoadon);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            label12.Text = "Khách hàng";
            FKhachHang fkhachhang = new FKhachHang();
            OpenchildForm(fkhachhang);
        }

        private void label10_Click(object sender, EventArgs e)
        {
            label12.Text = "Phiếu nhập";
            FKhoHang fkhohang = new FKhoHang();
            OpenchildForm(fkhohang);
        }

        private void FMain_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            FCuaHang fcuahang = new FCuaHang();
            OpenchildForm1(fcuahang);
        }

        private void label13_Click(object sender, EventArgs e)
        {
        }

        private void label14_Click(object sender, EventArgs e)
        {
            FNhaCungCap fnhacungcap = new FNhaCungCap();
            OpenchildForm1(fnhacungcap);
        }

        private void label15_Click(object sender, EventArgs e)
        {
            FSanPham fsanpham = new FSanPham();
            OpenchildForm1(fsanpham);
        }

        private void label3_Click(object sender, EventArgs e)
        {
            label12.Text = "Dashboard";
            if (currentFormChild != null)
            {
                currentFormChild.Close();
                FMain_Load(sender as Form, e);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {
            FDoanhThu fdoanhthu = new FDoanhThu();
            OpenchildForm1(fdoanhthu);
        }

        private void label13_Click_1(object sender, EventArgs e)
        {
            FLuong fluong = new FLuong(manv);
            OpenchildForm1(fluong);
        }

        private void label11_Click(object sender, EventArgs e)
        {
            this.Close();
            LOGIN flogin = new LOGIN();
            flogin.Show();
        }
    }
}
