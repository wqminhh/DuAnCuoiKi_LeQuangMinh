using System;

namespace PRDBMS.DTO
{
    // Data Transfer Object (DTO) for the Product domain.
    // This is intentionally minimal; extend to match your schema.
    public class ProductDto
    {
        public string MaSP { get; set; } = "";
        public string TenSP { get; set; } = "";
        public string MaCH { get; set; } = "";
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
    }
}
