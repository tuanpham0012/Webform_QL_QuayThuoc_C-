using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuayThuoc.Models;

namespace QuayThuoc.Controllers
{
    public class AdminController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            //ViewBag.KhachHang = db.KhachHangs.ToList();
            ViewBag.LoaiSp = db.LoaiSPs.ToList();
            return View();
        }

        public JsonResult GetKhoHang()
        {
            var ds = (from k in db.KhoHangs
                      join s in db.SanPhams on k.Id_SanPham equals s.Id_SanPham where k.SoLuong > 0
                      select new
                      {
                          Id = k.Id_KhoHang,
                          MaHang = k.MaHang,
                          TenSp = s.TenSP,
                          MaSp = s.MaSP,
                          GiaBan = k.GiaBan,
                          SoLuong = k.SoLuong,
                          GiamGia = k.GiamGia,
                          NgaySx = k.NgaySX,
                      }).ToList();
            return Json(new { code = 200, data = ds }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SanPham()
        {
            ViewBag.Loaisp = db.LoaiSPs.ToList();
            ViewBag.NhaCC= db.NhaCungCaps.ToList();
            ViewBag.DonVi = db.DonVis.ToList(); ;
            return View();
        }
        [HttpPost]
        public JsonResult ThemKhachHang(String TenKh, String DiaChi, String LienHe, int Id_LoaiKh)
        {
            try
            {
                var khachhang = new KhachHang();
                khachhang.TenKhachHang = TenKh;
                khachhang.DiaChi = DiaChi;
                khachhang.LienHe = LienHe;
                khachhang.Id_LoaiKH = Id_LoaiKh;
                khachhang.DaXoa = false;
                db.KhachHangs.Add(khachhang);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm khách hàng thành công!" }, JsonRequestBehavior.AllowGet);
            }catch(Exception ex)
            {
                return Json(new { msg = "Thêm khách hàng thất bại!"+ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult NhanVien()
        {
            ViewBag.ChucVu = db.ChucVus.Where(cv=> cv.Id_ChucVu != 1).ToList();
            return View();
        }

        public ActionResult NhapHang()
        {
            ViewBag.LoaiSp = db.LoaiSPs.ToList();
            return View();
        }

        public ActionResult HoaDonBan()
        {
            return View();
        }
        public ActionResult HoaDonNhap()
        {
            return View();
        }

        public ActionResult KiemTraSp()
        {
            return View();
        }

        public ActionResult DoanhThu()
        {
            return View();
        }
        public ActionResult KhoHang()
        {
            ViewBag.LoaiSp = db.LoaiSPs.ToList();
            return View();
        }

        public ActionResult KhachHang()
        {
            ViewBag.LoaiKh = db.LoaiKHs.ToList();
            return View();
        }
        public ActionResult DaXoa()
        {
            return View();
        }
        public ActionResult DatHang()
        {
            return View();
        }
    }
}