using System;
using System.Data;
using MySqlConnector;

namespace PRDBMS.DAL
{
    public class HoaDonDAL
    {
        public void AddHoaDon(MySqlConnection con, MySqlTransaction tx,
                              string maHD, DateTime ngayGiaoDich, string maKH, string maNV)
        {
            MySqlCommand cmd = new MySqlCommand("AddHoaDon", con, tx);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new MySqlParameter("MaHDIn", MySqlDbType.VarChar) { Value = maHD });
            cmd.Parameters.Add(new MySqlParameter("NgayGiaoDichIn", MySqlDbType.Date) { Value = ngayGiaoDich.Date });
            cmd.Parameters.Add(new MySqlParameter("MaKHIn", MySqlDbType.VarChar) { Value = maKH });
            cmd.Parameters.Add(new MySqlParameter("MaNVIn", MySqlDbType.VarChar) { Value = maNV });

            cmd.ExecuteNonQuery();
        }
    }
}
