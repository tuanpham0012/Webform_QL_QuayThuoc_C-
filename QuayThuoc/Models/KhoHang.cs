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
    
    public partial class KhoHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhoHang()
        {
            this.DonHangs = new HashSet<DonHang>();
        }
    
        public int Id_KhoHang { get; set; }
        public string MaHang { get; set; }
        public Nullable<int> Id_SanPham { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public Nullable<int> GiaBan { get; set; }
        public Nullable<int> GiaNhap { get; set; }
        public Nullable<int> GiamGia { get; set; }
        public Nullable<System.DateTime> NgaySX { get; set; }
        public Nullable<System.DateTime> HanSD { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual SanPham SanPham { get; set; }
    }
}
