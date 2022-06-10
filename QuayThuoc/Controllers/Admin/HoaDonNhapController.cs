using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    public class HoaDonNhapController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        [HttpGet]
        public JsonResult Show(int pageIndex, DateTime TimTheoNgay, DateTime TimTheoThang, Boolean Ngay, Boolean Thang)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var HoaDonNhap = (from hd in db.HoaDonNhaps
                                 join nv in db.NhanViens on hd.Id_NhanVien equals nv.Id_NhanVien
                                 where hd.DaXoa == false
                                 select new
                                 {
                                     Id = hd.Id_HoaDonNhap,
                                     MaHD = hd.MaHoaDon,
                                     TongTien = hd.TongTien,
                                     GiamGia = hd.GiamGia,
                                     TenNv = nv.TenNV,
                                     NgayTao = hd.NgayNhap,
                                 }).OrderByDescending(hd => hd.Id).ToList();
                if (Ngay)
                {
                    DateTime day = TimTheoNgay.AddDays(1);
                    HoaDonNhap = HoaDonNhap.Where(hd => hd.NgayTao < day && hd.NgayTao > TimTheoNgay).ToList();
                }
                if (Thang)
                {
                    DateTime month = TimTheoThang.AddMonths(1);
                    HoaDonNhap = HoaDonNhap.Where(hd => hd.NgayTao < month && hd.NgayTao > TimTheoThang).ToList();
                }
                var pageSize = 4;
                var soTrang = (HoaDonNhap.Count() % pageSize == 0) ? HoaDonNhap.Count() / pageSize : (HoaDonNhap.Count() / pageSize) + 1;

                HoaDonNhap = HoaDonNhap.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { code = 200, data = HoaDonNhap, soTrang = soTrang, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Detail(int Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var HoaDon = (from hd in db.HoaDonNhaps
                          join nv in db.NhanViens on hd.Id_NhanVien equals nv.Id_NhanVien
                          where hd.DaXoa == false && hd.Id_HoaDonNhap == Id
                          select new
                          {
                              Id = hd.Id_HoaDonNhap,
                              MaHD = hd.MaHoaDon,
                              TongTien = hd.TongTien,
                              GiamGia = hd.GiamGia,
                              TenNv = nv.TenNV,
                              NgayTao = hd.NgayNhap,
                          }).SingleOrDefault();
            if (HoaDon != null)
            {
                var SpNhap = (from hd in db.SanPhamNhaps
                             join sp in db.SanPhams on hd.Id_SanPham equals sp.Id_SanPham
                             where hd.MaHoaDon.Equals(HoaDon.MaHD)
                             select new
                             {
                                 Id = hd.Id_SanPhamNhap,
                                 TenSp = sp.TenSP,
                                 MaSp = sp.MaSP,
                                 SoLuong = hd.SoLuong,
                                 GiaBan = hd.GiaNhap,
                                 NgayTao = hd.NgayNhap,
                             }).OrderByDescending(hd => hd.Id).ToList();
                return Json(new { code = 200, HoaDon = HoaDon, ChiTietHD = SpNhap, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 404, msg = "Lấy dữ liệu thất bại!" }, JsonRequestBehavior.AllowGet);
            }


        }
    }
}