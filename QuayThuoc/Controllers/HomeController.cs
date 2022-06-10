using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using QuayThuoc.Models;

namespace QuayThuoc.Controllers
{
    public class HomeController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult RSSFeed()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetRSSFeed()
        {
            String url = "https://ncov.moh.gov.vn/vi/web/guest/rss/-/journal/rss/20182/7199277";
            WebClient wclient = new WebClient();
            wclient.Encoding = System.Text.Encoding.UTF8;
            wclient.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
            string RSSData = wclient.DownloadString(url);

            XDocument xml = XDocument.Parse(RSSData);
            var RSSFeedData = (from x in xml.Descendants("item")
                               select new Item
                               {
                                   Title = ((string)x.Element("title")),
                                   Link = ((string)x.Element("link")),
                                   Description = ((string)x.Element("description")),
                                   PubDate = ((string)x.Element("pubDate"))
                               }).Take(15);

            return Json(new { data = RSSFeedData }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SanPham()
        {
            ViewBag.Loaisp = db.LoaiSPs.ToList();
            return View();
        }

        [HttpGet]
        public JsonResult SpPhoBien()
        {
            var ds = (from k in db.KhoHangs join s in db.SanPhams on k.Id_SanPham equals s.Id_SanPham
                      select new
                      {
                          Id = k.Id_KhoHang,
                          TenSp = s.TenSP,
                          GiaBan = k.GiaBan,
                          GiamGia = k.GiamGia,
                          Soluong = k.SoLuong,
                          HinhAnh = s.HinhAnh,
                      }).OrderByDescending(kh=> kh.Soluong).Take(9).ToList();

            return Json(new { code = 200, data = ds }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SpMoi()
        {
            var ds = (from k in db.KhoHangs
                      join s in db.SanPhams on k.Id_SanPham equals s.Id_SanPham
                      select new
                      {
                          Id = k.Id_KhoHang,
                          Id_SanPham = k.Id_SanPham,
                          TenSp = s.TenSP,
                          GiaBan = k.GiaBan,
                          GiamGia = k.GiamGia,
                          Soluong = k.SoLuong,
                          HinhAnh = s.HinhAnh,
                      }).OrderByDescending(kh => kh.Id_SanPham).Take(4).ToList();

            return Json(new { code = 200, data = ds }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DsSanPham(int pageIndex, String TimKiem, int Id_LoaiSp)
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
                                                                    SoLuong = kh.SoLuong,
                                                                    GiaBan = kh.GiaBan,
                                                                    GiaNhap = kh.GiaNhap,
                                                                    GiamGia = kh.GiamGia,
                                                                    NgaySx = kh.NgaySX,
                                                                    HanSd = kh.HanSD,
                                                                    HinhAnh = sp.HinhAnh,
                                                                }).Where(kh => kh.HanSd > day && kh.SoLuong > 0).OrderByDescending(dh => dh.Id).ToList();
                if (TimKiem.Length > 0)
                {
                    DBKhoHang = DBKhoHang.Where(kh => kh.MaHang.Contains(TimKiem) || kh.TenSp.Contains(TimKiem)).ToList();
                }
                if (Id_LoaiSp != -1)
                {
                    DBKhoHang = DBKhoHang.Where(kh => kh.Id_LoaiSp == Id_LoaiSp).ToList();
                }
                var pageSize = 12;
                var soTrang = (DBKhoHang.Count() % pageSize == 0) ? DBKhoHang.Count() / pageSize : (DBKhoHang.Count() / pageSize) + 1;

                DBKhoHang = DBKhoHang.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                return Json(new { code = 200, data = DBKhoHang, soTrang = soTrang, msg = "Lấy Danh Sách Sản Phẩm Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Danh Sách Sản Phẩm Thất Bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult GioiThieu()
        {
            return View();
        }

        public ActionResult ChiTietSanPham()
        {
            return View();
        }

        public ActionResult Cart()
        {
            return View();
        }
    }
}