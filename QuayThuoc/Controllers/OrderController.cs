using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers
{
    public class OrderController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        DungChung dc = new DungChung();
        [HttpGet]
        public JsonResult ChiTietSp(int id)
        {
            try
            {
                DateTime day = DateTime.Now;
                var DBKhoHang = db.KhoHangs.Join(db.SanPhams, kh => kh.Id_SanPham,
                                                                sp => sp.Id_SanPham,
                                                                (kh, sp) => new
                                                                {
                                                                    Id = kh.Id_KhoHang,
                                                                    MaHang = kh.MaHang,
                                                                    Id_SanPham = kh.Id_SanPham,
                                                                    TenSp = sp.TenSP,
                                                                    Id_LoaiSp = sp.Id_LoaiSP,
                                                                    ThongTin = sp.ThongTin,
                                                                    HinhAnh = sp.HinhAnh,
                                                                    LoaiSp = sp.LoaiSP.TenLoaiSP,
                                                                    SoLuong = kh.SoLuong,
                                                                    GiaBan = kh.GiaBan,
                                                                    GiaNhap = kh.GiaNhap,
                                                                    GiamGia = kh.GiamGia,
                                                                    NgaySx = kh.NgaySX,
                                                                    HanSd = kh.HanSD,
                                                                }).Where(kh => kh.HanSd > day && kh.SoLuong > 0 && kh.Id == id).OrderByDescending(dh => dh.Id).FirstOrDefault();
                return Json(new { code = 200, data = DBKhoHang, msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult LoadCart(int Id_NhanVien)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
               var DBDonHang = (from dh in db.DonHangs
                                 join sp in db.SanPhams on dh.Id_SanPham equals sp.Id_SanPham
                                 join kh in db.KhoHangs on dh.Id_KhoHang equals kh.Id_KhoHang
                                 join nv in db.NhanViens on dh.Id_NhanVien equals nv.Id_NhanVien
                                 where dh.SoLuong > 0 && dh.Id_NhanVien == Id_NhanVien && dh.Id_Order == -1
                                 select new
                                 {
                                     Id = dh.Id_DonHang,
                                     MaSp = sp.MaSP,
                                     TenSp = sp.TenSP,
                                     Id_KhoHang = kh.Id_KhoHang,
                                     MaHang = kh.MaHang,
                                     GiaBan = kh.GiaBan,
                                     GiamGia = dh.GiamGia,
                                     SoLuong = dh.SoLuong,
                                     TongTien = dh.TongTien,
                                     Id_NhanVien = dh.Id_NhanVien,
                                     TenNv = nv.TenNV,
                                 }).ToList();
                var khachhang = db.KhachHangs.Where(kh => kh.DaXoa == false).ToList();
                return Json(new { code = 200, data = DBDonHang, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DatHang(String TenKhachHang, String LienHe, String DiaChi, String PhuongThucTT, int Id_NhanVien)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var DatHang = new Order();
                DateTime day = DateTime.Now;
                var Id_or = 1;
                var order = db.Orders.OrderByDescending(o => o.Id_Order).FirstOrDefault();
                if(order != null)
                {
                    Id_or = order.Id_Order + 1;
                }
                DatHang.MaGiaoDich = dc.CreateMaHD("DHOL", Id_or);
                DatHang.TenKhachHang = TenKhachHang;
                DatHang.LienHe = LienHe;
                DatHang.DiaChi = DiaChi;
                DatHang.PhuongThucTT = PhuongThucTT;
                DatHang.TrangThai = "Đang chờ duyệt";
                DatHang.Id_NhanVien = Id_NhanVien;
                DatHang.NgayTao = day;

                db.Orders.Add(DatHang);
                db.SaveChanges();

                var dh = db.DonHangs.Where(d => d.Id_NhanVien == Id_NhanVien && d.Id_Order == -1).ToList();
                foreach(var item in dh)
                {
                    item.Id_Order = Id_or;
                    db.SaveChanges();
                }

                return Json(new { code = 200, msg = "Đặt hàng thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DonDaDat(int Id_NhanVien)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var DBOrder = db.Orders.Where(o => o.Id_NhanVien == Id_NhanVien).OrderByDescending(o => o.Id_Order).Take(15);
                return Json(new { code = 200, data = DBOrder, msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ChiTietDonHang(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var DBDonHang = (from dh in db.DonHangs
                                 join sp in db.SanPhams on dh.Id_SanPham equals sp.Id_SanPham
                                 join kh in db.KhoHangs on dh.Id_KhoHang equals kh.Id_KhoHang
                                 where dh.Id_Order == Id
                                 select new
                                 {
                                     Id = dh.Id_DonHang,
                                     MaSp = sp.MaSP,
                                     TenSp = sp.TenSP,
                                     Id_KhoHang = kh.Id_KhoHang,
                                     MaHang = kh.MaHang,
                                     GiaBan = kh.GiaBan,
                                     GiamGia = dh.GiamGia,
                                     SoLuong = dh.SoLuong,
                                     TongTien = dh.TongTien,
                                     Id_NhanVien = dh.Id_NhanVien,
                                 }).ToList();
                var DBOrder = db.Orders.Find(Id);
                return Json(new { code = 200, order = DBOrder, data = DBDonHang, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult DonDatHang(int pageIndex, String TimKiem, String TrangThai)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var pageSize = 15;
                var DBOrder = db.Orders.OrderByDescending(o => o.Id_Order).ToList();
                if(TimKiem.Length > 0)
                {
                    DBOrder = DBOrder.Where(o => o.MaGiaoDich.Contains(TimKiem)).ToList();
                }
                if (TrangThai.Length > 0)
                {
                    DBOrder = DBOrder.Where(o => o.TrangThai.Equals(TrangThai)).ToList();
                }
                var soTrang = (DBOrder.Count % pageSize == 0) ? DBOrder.Count / pageSize : (DBOrder.Count / pageSize) + 1;

                DBOrder = DBOrder.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return Json(new { code = 200, data = DBOrder, soTrang = soTrang, msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ChuyenHang(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var DBOrder = db.Orders.Find(Id);
                DBOrder.TrangThai = "Đang giao hàng";
                db.SaveChanges();
                return Json(new { code = 200, msg = "Succecc!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ThanhToan(int Id, int Id_NhanVien)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                DateTime day = DateTime.Now;
                var DBOrder = db.Orders.Find(Id);
                var DBDonHang = db.DonHangs.Where(dh => dh.Id_Order == Id).ToList();

                var idHD = db.HoaDonBans.OrderByDescending(hd => hd.Id_HoaDonBan).FirstOrDefault();
                var id = 1;
                if(idHD == null)
                {
                    id = idHD.Id_HoaDonBan;
                }
                var GiamGia = 0;
                var TongTien = 0;
                var nd = 1;
                String MaHd = dc.CreateMaHD("HDB", id);
                var ds = new HoaDonBan();
                ds.MaHoaDon = MaHd;

                foreach (var item in DBDonHang)
                {
                    var spBan = new SanPhamBan();
                    spBan.MaHoaDon = MaHd;
                    spBan.Id_SanPham = item.Id_SanPham;
                    spBan.SoLuong = item.SoLuong;
                    spBan.GiaBan = item.TongTien;
                    spBan.GiamGia = item.GiamGia;
                    spBan.NgayBan = day;
                    db.SanPhamBans.Add(spBan);
                    db.SaveChanges();
                    GiamGia = GiamGia + (int)item.GiamGia;
                    TongTien = TongTien + (int)item.TongTien;
                    var kh = db.KhoHangs.Find(item.Id_KhoHang);
                    kh.SoLuong = kh.SoLuong - item.SoLuong;
                    nd = item.Id_NhanVien;
                    db.SaveChanges();

                }
                ds.TongTien = TongTien;
                ds.Id_NhanVien = Id_NhanVien;
                ds.Id_KhachHang = 1;
                ds.NgayTao = day;
                ds.DaXoa = false;
                ds.GiamGia = GiamGia;
                db.HoaDonBans.Add(ds);
                db.SaveChanges();

                DBOrder.TrangThai = "Đã nhận";
                db.SaveChanges();

                DateTime date = DateTime.Today;
                var DBDoanhThu = db.DoanhThus.Where(dt => dt.Ngay == date).FirstOrDefault();
                if (DBDoanhThu == null)
                {
                    var Dt = new DoanhThu();
                    Dt.Ngay = day;
                    Dt.DoanhThuBan = 0;
                    Dt.TienNhapHang = 0;
                    Dt.ThuChiKhac = 0;
                    db.DoanhThus.Add(Dt);
                    db.SaveChanges();

                    DBDoanhThu = db.DoanhThus.Where(dt => dt.Ngay == date).FirstOrDefault();
                }
                DBDoanhThu.DoanhThuBan = (TongTien - GiamGia);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thanh toán thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult HuyDon(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var DBOrder = db.Orders.Find(Id);
                DBOrder.TrangThai = "Đã hủy";
                db.SaveChanges();
                return Json(new { code = 200, msg = "Succecc!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Error!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}