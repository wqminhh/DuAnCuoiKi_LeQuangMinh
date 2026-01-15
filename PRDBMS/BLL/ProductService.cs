using System.Data;
using MySqlConnector;
using PRDBMS.DAL;

namespace PRDBMS.BLL
{
    /// <summary>
    /// Example service that isolates product-related queries.
    /// Extend to match your existing stored procedures/views in MySQL.
    /// </summary>
    public sealed class ProductService
    {
        private readonly DbHelper _data;

        public ProductService(DbHelper data) => _data = data;

        public DataTable GetAllProducts()
        {
            return _data.GetDataTable("SELECT * FROM view_DSSanPham", CommandType.Text);
        }

        public DataTable SearchProducts(string keyword)
        {
            return _data.GetDataTable(
                "SearchSanPham",
                CommandType.StoredProcedure,
                new MySqlParameter("@Keyword", keyword)
            );
        }

        public DataTable GetProductsByStore(string maCH)
        {
            // MySQL note: SQL Server TVF calls like DSSanPham_CuaHang(@MaCH) must be re-implemented in MySQL
            // as a stored procedure or view with WHERE.
            return _data.GetDataTable(
                "DSSanPham_CuaHang",
                CommandType.StoredProcedure,
                new MySqlParameter("@MaCH", maCH)
            );
        }
    }
}
