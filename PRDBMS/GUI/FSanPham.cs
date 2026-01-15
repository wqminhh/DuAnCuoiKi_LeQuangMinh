using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using MySqlConnector;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using PRDBMS.DAL;
using PRDBMS.BLL;
using AppContext = PRDBMS.BLL.AppContext;

namespace PRDBMS
{
    public partial class FSanPham : Form
    {
        private readonly ProductService _productService = new ProductService(PRDBMS.BLL.AppContext.Data);
        ConnectDatabase db = new ConnectDatabase();

        public FSanPham()
        {
            InitializeComponent();
        }

        private void FSanPham_Load(object sender, EventArgs e)
        {
            DataTable dataTable = _productService.GetAllProducts();
            dataGridView1.DataSource = dataTable;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox1.Text.Trim();
            DataTable dataTable = _productService.SearchProducts(keyword);
            dataGridView1.DataSource = dataTable;
        }

        private DataTable Load2()
        {
            DataTable dataTable = new DataTable();
            MySqlCommand command = new MySqlCommand("DSSanPham_CuaHang", db.getConnection);
            command.Parameters.AddWithValue("@MaCH", comboBox1.Text);
            command.CommandType = CommandType.StoredProcedure;
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);
            try
            {
                db.openConnection();
                adapter.Fill(dataTable);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi không xác định: " + ex.Message);
            }
            finally
            {
                db.closeConnection();
            }

            return dataTable;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selected = tabControl1.SelectedTab;
            if(selected == tabPage1) 
            {
                FSanPham_Load(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView2.DataSource = Load2();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string keyword = textBox2.Text.Trim();
            MySqlCommand cmd = new MySqlCommand("SearchSanPham2", db.getConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Keyword", keyword);
            cmd.Parameters.AddWithValue("@MaCH", comboBox1.Text);
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            try
            {
                db.openConnection();
                adapter.Fill(dataTable);
                dataGridView2.DataSource = dataTable;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Lỗi khi tìm sản phẩm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                db.closeConnection();
            }
        }
    }
}