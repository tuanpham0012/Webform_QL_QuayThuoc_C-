//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuayThuoc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HoaDonBan
    {
        public int Id_HoaDonBan { get; set; }
        public string MaHoaDon { get; set; }
        public Nullable<int> GiamGia { get; set; }
        public Nullable<int> TongTien { get; set; }
        public Nullable<int> Id_NhanVien { get; set; }
        public Nullable<int> Id_KhachHang { get; set; }
        public Nullable<System.DateTime> NgayTao { get; set; }
        public bool DaXoa { get; set; }
    
        public virtual KhachHang KhachHang { get; set; }
        public virtual NhanVien NhanVien { get; set; }
    }
}
