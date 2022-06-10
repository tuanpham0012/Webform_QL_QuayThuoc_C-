using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    public class DonHangController : Controller
    {

        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        DungChung dc = new DungChung();
        // GET: ThanhToan
        [HttpPost]
        public JsonResult ThemDonHang(int Id_KhoHang, int Id_NhanVien, int SoLuong, Boolean check)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var DBDonHang = db.DonHangs.Where(dh => dh.Id_KhoHang == Id_KhoHang && dh.Id_NhanVien == Id_NhanVien && dh.Id_Order == -1).FirstOrDefault();
                var khohang = db.KhoHangs.Find(Id_KhoHang);
                if (DBDonHang != null )
                {

                    if (SoLuong == -1 || check)
                    {
                        return Json(new { code = 500, msg = "Sản phẩm đã có trong đơn!" }, JsonRequestBehavior.AllowGet); ;
                    }
                    if (SoLuong > khohang.SoLuong)
                    {
                        return Json(new { code = 500, msg = "Số lượng sản phẩm trong kho không đủ!" }, JsonRequestBehavior.AllowGet);
                    }
                    if (SoLuong == 0)
                    {
                        db.DonHangs.Remove(DBDonHang);
                        db.SaveChanges();
                        return Json(new { code = 200, msg = "Xóa sản phẩm khỏi đơn thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        DBDonHang.SoLuong = SoLuong;
                        DBDonHang.GiamGia = SoLuong * khohang.GiamGia;
                        DBDonHang.TongTien = SoLuong * khohang.GiaBan;
                        db.SaveChanges();
                        return Json(new { code = 100, msg = "Cập nhật số lượng thành công thành công!" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var ds = new DonHang();
                    ds.Id_KhoHang = Id_KhoHang;
                    ds.Id_SanPham = (int)khohang.Id_SanPham;
                    ds.SoLuong = (SoLuong == -1) ? 1 : SoLuong;
                    ds.TongTien = (int)khohang.GiaBan;
                    ds.GiamGia = khohang.GiamGia;
                    ds.Id_NhanVien = Id_NhanVien;
                    ds.Id_Order = -1;
                    db.DonHangs.Add(ds);
                    db.SaveChanges();

                    return Json(new { code = 200, msg = "Thêm sản phẩm vào đơn hàng thành công!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { msg = "error " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ThanhToan(int Id_NhanVien, int Id_KhachHang, int TongTien, DateTime NgayTao)
        {
            try
            {
                var idHD = db.HoaDonBans.OrderByDescending(hd => hd.Id_HoaDonBan).FirstOrDefault();
                var GiamGia = 0;
                String MaHd = dc.CreateMaHD("HDB", idHD.Id_HoaDonBan);
                var ds = new HoaDonBan();
                ds.MaHoaDon = MaHd;
                ds.TongTien = TongTien;
                ds.Id_NhanVien = Id_NhanVien;
                ds.Id_KhachHang = Id_KhachHang;
                ds.NgayTao = NgayTao;
                ds.DaXoa = false;

                var SpDH = db.DonHangs.Where(dh => dh.Id_NhanVien == Id_NhanVien).ToList();
                foreach (var item in SpDH)
                {
                    var spBan = new SanPhamBan();
                    spBan.MaHoaDon = MaHd;
                    spBan.Id_SanPham = item.Id_SanPham;
                    spBan.SoLuong = item.SoLuong;
                    spBan.GiaBan = item.TongTien;
                    spBan.GiamGia = item.GiamGia;
                    spBan.NgayBan = NgayTao;
                    db.SanPhamBans.Add(spBan);
                    db.SaveChanges();
                    GiamGia = GiamGia + (int)item.GiamGia;
                    var kh = db.KhoHangs.Find(item.Id_KhoHang);
                    kh.SoLuong = kh.SoLuong - item.SoLuong;
                    db.SaveChanges();

                    db.DonHangs.Remove(item);
                    db.SaveChanges();
                }

                ds.GiamGia = GiamGia;
                db.HoaDonBans.Add(ds);
                db.SaveChanges();

                DateTime day = DateTime.Today;
                var DBDoanhThu = db.DoanhThus.Where(dt => dt.Ngay == day).FirstOrDefault();
                if(DBDoanhThu == null)
                {
                    var Dt = new DoanhThu();
                    Dt.Ngay = day;
                    Dt.DoanhThuBan = 0;
                    Dt.TienNhapHang = 0;
                    Dt.ThuChiKhac = 0;
                    db.DoanhThus.Add(Dt);
                    db.SaveChanges();

                    DBDoanhThu = db.DoanhThus.Where(dt => dt.Ngay == day).FirstOrDefault();
                }
                  DBDoanhThu.DoanhThuBan = (TongTien - GiamGia);
                  db.SaveChanges();
                
                return Json(new { code = 200, msg = "Thanh toán thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { msg = "error!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult XoaDonBan(int Id_NhanVien)
        {
            try
            {
                var dn = db.DonHangs.Where(dh => dh.Id_NhanVien == Id_NhanVien).ToList();

                if (dn.Count > 0)
                {
                    foreach (var item in dn)
                    {
                        db.DonHangs.Remove(item);
                        db.SaveChanges();
                    }
                }

                return Json(new { code = 200, msg = "Hủy đơn thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Hủy đơn thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}