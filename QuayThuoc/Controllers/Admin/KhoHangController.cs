using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    public class KhoHangController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        DungChung dc = new DungChung();
        // GET: KhoHang
        [HttpGet]
        public JsonResult LayDuLieuKhoHang(int Id_NhanVien, String TimKiem, int Id_LoaiSp, int pageIndex)
        {
            db.Configuration.ProxyCreationEnabled = false;
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
                                                                    SoLuong = kh.SoLuong,
                                                                    GiaBan = kh.GiaBan,
                                                                    GiaNhap = kh.GiaNhap,
                                                                    GiamGia = kh.GiamGia,
                                                                    NgaySx = kh.NgaySX,
                                                                    HanSd = kh.HanSD,
                                                                }).Where(kh=> kh.HanSd > day && kh.SoLuong > 0).OrderByDescending(dh => dh.Id).ToList();
                if (TimKiem.Length > 0)
                {
                    DBKhoHang = DBKhoHang.Where(kh => kh.MaHang.Contains(TimKiem) || kh.TenSp.Contains(TimKiem)).ToList();
                }
                if (Id_LoaiSp != -1)
                {
                    DBKhoHang = DBKhoHang.Where(kh => kh.Id_LoaiSp == Id_LoaiSp).ToList();
                }
                var pageSize = 9;
                var soTrang = (DBKhoHang.Count() % pageSize == 0) ? DBKhoHang.Count() / pageSize : (DBKhoHang.Count() / pageSize) + 1;

                DBKhoHang = DBKhoHang.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var DBDonHang = (from dh in db.DonHangs
                                   join sp in db.SanPhams on dh.Id_SanPham equals sp.Id_SanPham
                                   join kh in db.KhoHangs on dh.Id_KhoHang equals kh.Id_KhoHang
                                   join nv in db.NhanViens on dh.Id_NhanVien equals nv.Id_NhanVien
                                   where dh.SoLuong > 0 && dh.Id_NhanVien == Id_NhanVien
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
                return Json(new { code = 200, KhachHang = khachhang, soTrang = soTrang, khohang = DBKhoHang, donhang = DBDonHang, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public JsonResult KiemTraKhoHang(int pageIndex, int LocSp)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var DBKhoHang = db.KhoHangs.Join(db.SanPhams, kh => kh.Id_SanPham,
                                                                sp => sp.Id_SanPham,
                                                                (kh, sp) => new
                                                                {
                                                                    Id = kh.Id_KhoHang,
                                                                    MaHang = kh.MaHang,
                                                                    Id_SanPham = kh.Id_SanPham,
                                                                    TenSp = sp.TenSP,
                                                                    Id_LoaiSp = sp.Id_LoaiSP,
                                                                    SoLuong = kh.SoLuong,
                                                                    GiaBan = kh.GiaBan,
                                                                    GiaNhap = kh.GiaNhap,
                                                                    GiamGia = kh.GiamGia,
                                                                    NgaySx = kh.NgaySX,
                                                                    HanSd = kh.HanSD,
                                                                }).OrderBy(dh => dh.SoLuong).ToList();
                if(LocSp == 0)
                {
                    DBKhoHang = DBKhoHang.Where(kh => kh.SoLuong < 30).ToList();
                }
                if (LocSp == 1)
                {
                    DateTime date = DateTime.Now;
                    DateTime date2 = date.AddMonths(2);
                    DBKhoHang = DBKhoHang.Where(kh => kh.HanSd > date && kh.HanSd <= date2).ToList();
                }
                if (LocSp == 2)
                {
                    DateTime date = DateTime.Now;

                    DBKhoHang = DBKhoHang.Where(kh => kh.HanSd < date).ToList();
                }
               
                var pageSize = 9;
                var soTrang = (DBKhoHang.Count() % pageSize == 0) ? DBKhoHang.Count() / pageSize : (DBKhoHang.Count() / pageSize) + 1;

                DBKhoHang = DBKhoHang.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return Json(new { code = 200,soTrang = soTrang, dbKhoHang = DBKhoHang, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult ThongKeSp(DateTime date)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;

                DateTime day = date.AddMonths(-1);

                var group = (from dh in db.SanPhamBans
                             where dh.NgayBan > day
                             join sp in db.SanPhams on dh.Id_SanPham equals sp.Id_SanPham
                             group dh by dh.Id_SanPham into n
                             select new
                             {
                                 Id = n.Key,
                                 SoLuong = n.Sum(c=> c.SoLuong),
                                 TenSp = n.Select(sl=> sl.SanPham.TenSP).FirstOrDefault(),
                                 MaSp = n.Select(sl => sl.SanPham.MaSP).FirstOrDefault(),
                             }).OrderByDescending(sp=> sp.SoLuong).ToList();

              
                return Json(new { code = 200, data = group, msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult Detail(int id)
        {
            try
            {
                var DBKhoHang = db.KhoHangs.Join(db.SanPhams, kh => kh.Id_SanPham,
                                                                sp => sp.Id_SanPham,
                                                                (kh, sp) => new
                                                                {
                                                                    Id = kh.Id_KhoHang,
                                                                    MaHang = kh.MaHang,
                                                                    Id_SanPham = kh.Id_SanPham,
                                                                    TenSp = sp.TenSP,
                                                                    Id_LoaiSp = sp.Id_LoaiSP,
                                                                    SoLuong = kh.SoLuong,
                                                                    GiaBan = kh.GiaBan,
                                                                    GiaNhap = kh.GiaNhap,
                                                                    GiamGia = kh.GiamGia,
                                                                    NgaySx = kh.NgaySX,
                                                                    HanSd = kh.HanSD,
                                                                }).Where(kh => kh.Id == id).FirstOrDefault();

                return Json(new { code = 200, data = DBKhoHang, msg = "Lấy thông tin thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy thông tin thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult Update(int Id, int GiaBan, int GiamGia, DateTime NgaySx, DateTime HanSd)
        {
            try
            {
                var ds = db.KhoHangs.Find(Id);
                ds.GiaBan = GiaBan;
                ds.GiamGia = GiamGia;
                ds.NgaySX = NgaySx;
                ds.HanSD = HanSd;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Cập nhật thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Cập nhật thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}