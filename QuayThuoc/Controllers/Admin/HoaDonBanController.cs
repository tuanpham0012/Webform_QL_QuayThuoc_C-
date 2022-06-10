using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuayThuoc.Models;

namespace QuayThuoc.Controllers.Admin
{
    public class HoaDonBanController : Controller
    {
        // GET: HoaDonBan

        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        [HttpGet]
        public JsonResult Show(int pageIndex,DateTime TimTheoNgay, DateTime TimTheoThang, Boolean Ngay, Boolean Thang)
        {
            db.Configuration.ProxyCreationEnabled = false;
            try
            {
                var HoaDonBan = (from hd in db.HoaDonBans
                                   join kh in db.KhachHangs on hd.Id_KhachHang equals kh.Id_KhachHang
                                   join nv in db.NhanViens on hd.Id_NhanVien equals nv.Id_NhanVien
                                   where hd.DaXoa == false
                                   select new
                                   {
                                       Id = hd.Id_HoaDonBan,
                                       MaHD = hd.MaHoaDon,
                                       TongTien = hd.TongTien,
                                       GiamGia = hd.GiamGia,
                                       TenNv = nv.TenNV,
                                       KhachHang = kh.TenKhachHang,
                                       NgayTao = hd.NgayTao,
                                   }).OrderByDescending(hd=> hd.Id).ToList();
                if (Ngay)
                {
                    DateTime day = TimTheoNgay.AddDays(1);
                    HoaDonBan = HoaDonBan.Where(hd => hd.NgayTao < day && hd.NgayTao > TimTheoNgay ).ToList();
                }
                if (Thang)
                {
                    DateTime month = TimTheoThang.AddMonths(1);
                    HoaDonBan = HoaDonBan.Where(hd => hd.NgayTao < month && hd.NgayTao > TimTheoThang).ToList();
                }
                var pageSize = 15;
                var soTrang = (HoaDonBan.Count() % pageSize == 0) ? HoaDonBan.Count() / pageSize : (HoaDonBan.Count() / pageSize) + 1;

                HoaDonBan = HoaDonBan.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                return Json(new { code = 200, data = HoaDonBan, soTrang = soTrang, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
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
            var HoaDon = (from hd in db.HoaDonBans
                          join kh in db.KhachHangs on hd.Id_KhachHang equals kh.Id_KhachHang
                          join nv in db.NhanViens on hd.Id_NhanVien equals nv.Id_NhanVien
                          where hd.DaXoa == false && hd.Id_HoaDonBan == Id
                          select new
                          {
                              Id = hd.Id_HoaDonBan,
                              MaHD = hd.MaHoaDon,
                              TongTien = hd.TongTien,
                              GiamGia = hd.GiamGia,
                              TenNv = nv.TenNV,
                              KhachHang = kh.TenKhachHang,
                              NgayTao = hd.NgayTao,
                          }).SingleOrDefault();
            if (HoaDon != null)
            {
                var SpBan = (from hd in db.SanPhamBans
                             join sp in db.SanPhams on hd.Id_SanPham equals sp.Id_SanPham
                             where hd.MaHoaDon.Equals(HoaDon.MaHD)
                             select new
                             {
                                 Id = hd.Id_SanPhamBan,
                                 TenSp = sp.TenSP,
                                 MaSp = sp.MaSP,
                                 SoLuong = hd.SoLuong,
                                 GiaBan = hd.GiaBan,
                                 NgayTao = hd.NgayBan,
                             }).OrderByDescending(hd => hd.Id).ToList();
                return Json(new { code = 200, HoaDon = HoaDon, ChiTietHD = SpBan, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { code = 404, msg = "Lấy dữ liệu thất bại!" }, JsonRequestBehavior.AllowGet);
            }

            
        }
    }
}