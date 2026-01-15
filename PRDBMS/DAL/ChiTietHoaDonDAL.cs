using System.Data;
using MySqlConnector;

namespace PRDBMS.DAL
{
    public class ChiTietHoaDonDAL
    {
        public void AddChiTietHoaDon(MySqlConnection con, MySqlTransaction tx,
                                     string maHD, string maSP, int soLuong)
        {
            MySqlCommand cmd = new MySqlCommand("AddChiTietHoaDon", con, tx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("MaHD", MySqlDbType.VarChar) { Value = maHD });
            cmd.Parameters.Add(new MySqlParameter("MaSP", MySqlDbType.VarChar) { Value = maSP });
            cmd.Parameters.Add(new MySqlParameter("SoLuong", MySqlDbType.Int32) { Value = soLuong });

            cmd.ExecuteNonQuery();
        }
    }
}
