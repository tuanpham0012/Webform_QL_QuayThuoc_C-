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
    
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            this.DonHangs = new HashSet<DonHang>();
            this.DonMuas = new HashSet<DonMua>();
            this.HoaDonBans = new HashSet<HoaDonBan>();
            this.HoaDonNhaps = new HashSet<HoaDonNhap>();
            this.Orders = new HashSet<Order>();
        }
    
        public int Id_NhanVien { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DiaChi { get; set; }
        public string LienHe { get; set; }
        public string GioiTinh { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string CMND { get; set; }
        public string API_Token { get; set; }
        public int Id_ChucVu { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> update_at { get; set; }
        public Nullable<System.DateTime> last_login { get; set; }
        public Nullable<bool> DaXoa { get; set; }
    
        public virtual ChucVu ChucVu { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonMua> DonMuas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDonBan> HoaDonBans { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDonNhap> HoaDonNhaps { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}
