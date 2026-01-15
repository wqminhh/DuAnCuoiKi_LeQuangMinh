using System;
using System.Collections.Generic;
using MySqlConnector;
using PRDBMS.DAL;

namespace PRDBMS.BUS
{
    public class HoaDonItem
    {
        public string MaSP { get; set; }
        public int SoLuong { get; set; }
    }

    public class HoaDonBUS
    {
        private readonly ConnectDatabase db = new ConnectDatabase();
        private readonly HoaDonDAL hoaDonDAL = new HoaDonDAL();
        private readonly ChiTietHoaDonDAL ctDAL = new ChiTietHoaDonDAL();

        public void TaoHoaDon(
            string maHD,
            DateTime ngayGiaoDich,
            string maKH,
            string maNV,
            List<HoaDonItem> items
        )
        {
            if (string.IsNullOrWhiteSpace(maHD))
                throw new Exception("Mã hóa đơn không được rỗng");

            if (string.IsNullOrWhiteSpace(maKH))
                throw new Exception("Chưa chọn khách hàng");

            if (items == null || items.Count == 0)
                throw new Exception("Chưa có sản phẩm trong hóa đơn");

            db.openConnection();
            MySqlTransaction tx = db.getConnection.BeginTransaction();

            try
            {
                hoaDonDAL.AddHoaDon(
                    db.getConnection, tx,
                    maHD, ngayGiaoDich, maKH, maNV
                );

                foreach (HoaDonItem item in items)
                {
                    if (item.SoLuong <= 0)
                        throw new Exception("Số lượng phải lớn hơn 0");

                    ctDAL.AddChiTietHoaDon(
                        db.getConnection, tx,
                        maHD, item.MaSP, item.SoLuong
                    );
                }

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
    }
}
